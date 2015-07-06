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

		static void SomeFunction(WrenVM VM) {
			VM.ReturnString("Hello " + VM.GetArgumentString(1) + " World #" + Rand.Next(1, 100) + "!");
		}

		static WrenForeignMethodFn BindForeignMethod(WrenVM VM, string Module, string ClassName, bool Static, string Sig) {
			if (ClassName == "DotNet" && Sig == "SomeFunction(_)" && Static)
				return SomeFunction;
			return null;
		}

		static string LoadModule(WrenVM VM, string Module) {
			if (File.Exists(Module))
				return File.ReadAllText(Module);
			return "";
		}

		static void Main(string[] args) {
			Console.Title = "Wren.NET Test";

			WrenConfig Cfg = new WrenConfig(BindForeignMethod, LoadModule);
			WrenVM WVM = Cfg.NewVM();

			string Test = File.ReadAllText("test.wren");
			Console.WriteLine("{0}\n\n----------", Test);
			WVM.Interpret("Test", Test);
			Console.ReadLine();
		}
	}
}