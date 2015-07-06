using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WrenNET;

namespace Test {
	class Program {
		static void Main(string[] args) {
			Console.Title = "Wren.NET Test";

			WrenVM WVM = new WrenVM(new WrenConfig());
			WVM.Interpret("Shell", "IO.print(\"Hello Wren World!\")");

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}
