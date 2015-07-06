using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace WrenNET {
	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	delegate IntPtr WrenRellocateFn(IntPtr Mem, int NewSize);

	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	delegate void WrenForeignMethodFn(IntPtr VM);

	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	delegate string WrenLoadModuleFn(IntPtr VM, string Name);

	[UnmanagedFunctionPointer(WrenVM.CConv, CharSet = WrenVM.CSet)]
	delegate IntPtr WrenBindForeignMethodFn(IntPtr VM, string Module, string ClassName, bool IsStatic, string Signature);


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

		IntPtr VMPtr;

		public WrenVM(WrenConfig Conf) {
			VMPtr = wrenNewVM(new IntPtr(&Conf));
		}

		~WrenVM() {
			wrenFreeVM(VMPtr);
		}

		public WrenInterpretResult Interpret(string SrcPath, string Src) {
			return wrenInterpret(VMPtr, SrcPath, Src);
		}

		public override string ToString() {
			return string.Format("WrenVM {0}", VMPtr);
		}
	}
}