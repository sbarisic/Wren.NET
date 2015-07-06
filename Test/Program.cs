using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using WrenNET;

namespace Test {
	class Program {
		static Random Rand = new Random();

		static void SomeProperty(IntPtr VM) {
			WrenVM.ReturnString(VM, "Hello Wren World #" + Rand.Next(1, 100) + "!");
		}

		static void Main(string[] args) {
			Console.Title = "Wren.NET Test";

			WrenVM WVM = new WrenVM(new WrenConfig((VM, Module, ClassName, Static, Sig) => {
				WrenVM W = new WrenVM(VM);
				if (ClassName == "DotNet" && Sig == "SomeProperty" && Static)
					return WrenVM.ToFuncPtr(SomeProperty);
				return IntPtr.Zero;
			}, (VM, Module) => {
				if (File.Exists(Module))
					return File.ReadAllText(Module);
				return "";
			}));

			string Test = File.ReadAllText("test.wren");
			Console.WriteLine("{0}\n\n----------", Test);
			WVM.Interpret("Test", Test);
			Console.ReadLine();
		}
	}
}