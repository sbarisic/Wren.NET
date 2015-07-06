using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using WrenNET;

namespace Test {
	class Program {
		static void WrenMethod(IntPtr VM) {
			WrenVM W = new WrenVM(VM);
			//W.Return("Hello Wren.NET World!");
		}

		static void Main(string[] args) {
			Console.Title = "Wren.NET Test";

			WrenVM WVM = new WrenVM(new WrenConfig((VM, Module, ClassName, Static, Sig) => {
				WrenVM W = new WrenVM(VM);

				if (ClassName == "DotNet" && Sig == "WrenMethod" && Static)
					return WrenVM.ToFuncPtr(WrenMethod);

				return IntPtr.Zero;
			}, (VM, Module) => {
				if (File.Exists(Module))
					return File.ReadAllText(Module);
				return "";
			}));

			WVM.Interpret("Test", File.ReadAllText("test.wren"));

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}