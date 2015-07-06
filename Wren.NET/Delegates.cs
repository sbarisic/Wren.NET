using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace WrenNET {
	[UnmanagedFunctionPointer(WrenNative.CConv, CharSet = WrenNative.CSet)]
	public delegate IntPtr WrenRellocateFn(IntPtr Mem, int NewSize);

	[UnmanagedFunctionPointer(WrenNative.CConv, CharSet = WrenNative.CSet)]
	public delegate void WrenForeignMethodFn(WrenVM VM);

	[UnmanagedFunctionPointer(WrenNative.CConv, CharSet = WrenNative.CSet)]
	public delegate string WrenLoadModuleFn(WrenVM VM, string Name);

	[UnmanagedFunctionPointer(WrenNative.CConv, CharSet = WrenNative.CSet)]
	public delegate WrenForeignMethodFn WrenBindForeignMethodFn(WrenVM VM, string Module, string ClassName,
		bool IsStatic, string Signature);
}