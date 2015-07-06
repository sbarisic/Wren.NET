using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace WrenNET {
	public unsafe static class WrenNative {
		internal const string DllName = "Wren";
		internal const CallingConvention CConv = CallingConvention.Cdecl;
		internal const CharSet CSet = CharSet.Ansi;

		[DllImport(DllName, EntryPoint = "wrenNewVM", CallingConvention = CConv, CharSet = CSet)]
		public static extern WrenVM NewVM(this WrenConfig Cfg);

		[DllImport(DllName, EntryPoint = "wrenFreeVM", CallingConvention = CConv, CharSet = CSet)]
		public static extern void FreeVM(this WrenVM VM);

		[DllImport(DllName, EntryPoint = "wrenInterpret", CallingConvention = CConv, CharSet = CSet)]
		public static extern WrenInterpretResult Interpret(this WrenVM VM, string SrcPath, string Src);

		[DllImport(DllName, EntryPoint = "wrenGetMethod", CallingConvention = CConv, CharSet = CSet)]
		public static extern IntPtr GetMethod(this WrenVM VM, string Module, string Variable, string Signature);

		[DllImport(DllName, EntryPoint = "wrenCall", CallingConvention = CConv, CharSet = CSet)]
		public static extern void Call(this WrenVM VM, IntPtr Method, string Types, __arglist);

		[DllImport(DllName, EntryPoint = "wrenReleaseMethod", CallingConvention = CConv, CharSet = CSet)]
		public static extern void ReleaseMethod(this WrenVM VM, IntPtr Method);

		[DllImport(DllName, EntryPoint = "wrenGetArgumentBool", CallingConvention = CConv, CharSet = CSet)]
		public static extern bool GetArgumentBool(this WrenVM VM, int Idx);

		[DllImport(DllName, EntryPoint = "wrenGetArgumentDouble", CallingConvention = CConv, CharSet = CSet)]
		public static extern double GetArgumentDouble(this WrenVM VM, int Idx);

		[DllImport(DllName, EntryPoint = "wrenGetArgumentString", CallingConvention = CConv, CharSet = CSet)]
		public static extern string GetArgumentString(this WrenVM VM, int Idx);

		[DllImport(DllName, EntryPoint = "wrenReturnBool", CallingConvention = CConv, CharSet = CSet)]
		public static extern void ReturnBool(this WrenVM VM, bool Val = false);

		[DllImport(DllName, EntryPoint = "wrenReturnDouble", CallingConvention = CConv, CharSet = CSet)]
		public static extern void ReturnDouble(this WrenVM VM, double Val = 0.0);

		[DllImport(DllName, EntryPoint = "wrenReturnString", CallingConvention = CConv, CharSet = CSet)]
		public static extern void ReturnString(this WrenVM VM, string Txt = "", int Len = -1);
	}
}
