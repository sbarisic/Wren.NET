using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

using System.Runtime.InteropServices;

namespace WrenNET {
	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	public delegate IntPtr WrenRellocateFn(IntPtr Mem, int NewSize);

	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	public delegate void WrenForeignMethodFn(IntPtr VM);

	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	public delegate string WrenLoadModuleFn(IntPtr VM, string Name);

	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	public delegate IntPtr WrenBindForeignMethodFn(IntPtr VM, string Module, string ClassName, bool IsStatic, string Signature);


	public enum WrenInterpretResult {
		Success,
		CompileError,
		RuntimeError
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct WrenConfig {
		IntPtr RellocateFn;
		IntPtr LoadModuleFn;
		IntPtr BindForeignMethodFn;
		int InitialHeapSize;
		int MinHeapSize;
		int HeapGrowPercent;

		public WrenConfig(WrenBindForeignMethodFn BindForeignMethod, WrenLoadModuleFn LoadModule)
			: this() {
			BindForeignMethodFn = WrenVM.ToFuncPtr(BindForeignMethod);
			LoadModuleFn = WrenVM.ToFuncPtr(LoadModule);
		}

		/*public WrenConfig() {
			RellocateFn = IntPtr.Zero;
			LoadModuleFn = IntPtr.Zero;
			BindForeignMethodFn = IntPtr.Zero;
			InitialHeapSize = 10 * 1024 * 1024;
			MinHeapSize = 1024 * 1024;
			HeapGrowPercent = 50;
		}*/
	}

	public unsafe class WrenVM {
		internal const string DllName = "Wren";
		internal const CallingConvention CConv = CallingConvention.Cdecl;
		internal const CharSet CSet = CharSet.Ansi;

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern IntPtr wrenNewVM(IntPtr Cfg);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern void wrenFreeVM(IntPtr VM);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern WrenInterpretResult wrenInterpret(IntPtr VM, string SrcPath, string Src);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern IntPtr wrenGetMethod(IntPtr VM, string Module, string Variable, string Signature);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern void wrenCall(IntPtr VM, IntPtr Method, string Types, __arglist);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern void wrenReleaseMethod(IntPtr VM, IntPtr Method);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern bool wrenGetArgumentBool(IntPtr VM, int Idx);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern double wrenGetArgumentDouble(IntPtr VM, int Idx);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		internal static extern string wrenGetArgumentString(IntPtr VM, int Idx);

		[DllImport(DllName, EntryPoint = "wrenReturnBool", CallingConvention = CConv, CharSet = CSet)]
		public static extern void ReturnBool(IntPtr VM, bool Val = false);

		[DllImport(DllName, EntryPoint = "wrenReturnDouble", CallingConvention = CConv, CharSet = CSet)]
		public static extern void ReturnDouble(IntPtr VM, double Val = 0.0);

		[DllImport(DllName, EntryPoint = "wrenReturnString", CallingConvention = CConv, CharSet = CSet)]
		public static extern void ReturnString(IntPtr VM, string Txt = "", int Len = -1);

		public static IntPtr ToFuncPtr(WrenBindForeignMethodFn Fn) {
			return ToFuncPtr((Delegate)Fn);
		}

		public static IntPtr ToFuncPtr(WrenForeignMethodFn Fn) {
			return ToFuncPtr((Delegate)Fn);
		}

		public static IntPtr ToFuncPtr(Delegate D) {
			return Marshal.GetFunctionPointerForDelegate(D);
		}

		IntPtr VMPtr;
		bool Destroy;

		public static implicit operator IntPtr(WrenVM VM) {
			return VM.VMPtr;
		}

		public WrenVM(WrenConfig Conf) {
			VMPtr = wrenNewVM(new IntPtr(&Conf));
			Destroy = true;
		}

		public WrenVM(IntPtr VM) {
			Destroy = false;
		}

		~WrenVM() {
			if (Destroy)
				wrenFreeVM(VMPtr);
		}

		public WrenInterpretResult Interpret(string SrcPath, string Src) {
			return wrenInterpret(VMPtr, SrcPath, Src);
		}

		public T Get<T>(int Idx) {
			if (typeof(T) == typeof(bool))
				return (T)(object)wrenGetArgumentBool(VMPtr, Idx);
			else if (typeof(T) == typeof(double))
				return (T)(object)wrenGetArgumentDouble(VMPtr, Idx);
			else if (typeof(T) == typeof(string))
				return (T)(object)wrenGetArgumentString(VMPtr, Idx);
			throw new Exception("Getter for type " + typeof(T).Name + " not implemented");
		}


		public void Return(bool Val = false) {
			ReturnBool(VMPtr, Val);
		}

		public void Return(double Val = 0.0) {
			ReturnDouble(VMPtr, Val);
		}

		public void Return(string Val = "") {
			ReturnString(VMPtr, Val);
		}

		public override string ToString() {
			return string.Format("WrenVM {0}", VMPtr);
		}
	}
}