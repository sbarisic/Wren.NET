using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace WrenNET {
	public enum WrenInterpretResult {
		Success,
		CompileError,
		RuntimeError
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe class WrenConfig {
		WrenRellocateFn RellocateFn;
		WrenLoadModuleFn LoadModuleFn;
		WrenBindForeignMethodFn BindForeignMethodFn;
		int InitialHeapSize;
		int MinHeapSize;
		int HeapGrowPercent;

		public WrenConfig(WrenBindForeignMethodFn BindForeignMethod, WrenLoadModuleFn LoadModule) {
			BindForeignMethodFn = BindForeignMethod;
			LoadModuleFn = LoadModule;
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct WrenVM {
		public IntPtr Pointer;
	}
}