using System;
using System.IO;
using System.Threading;

namespace ProtocolBuffers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3) {
				Console.Error.WriteLine ("Usage:\tCodeGenerator.exe <path-to.proto> <namespace> <output.cs>");
                Console.Read();
				return;						
			}
			
			string protoPath = Path.GetFullPath (args [0]);
			string codeNamespace = args [1];
			string codePath = Path.GetFullPath (args [2]);
			
			if (File.Exists (protoPath) == false) {
				Console.Error.WriteLine ("File not found: " + protoPath);
				return;						
			}
			
			Console.WriteLine ("Parsing " + protoPath);
			Proto proto = ProtoParser.Parse (protoPath);
			if (proto == null)
				return;
			Console.WriteLine (proto);
			
			Console.WriteLine ("Generating code");
			CodeGenerator.Save (proto, codeNamespace, codePath);
			Console.WriteLine ("Saved: " + codePath);
        }
    }
}
