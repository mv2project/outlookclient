using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;
using ISS.MV2.OutlookClient.ServerDummy.Pop3;


namespace DummyStandalone {
    class Program {
        static void Main(string[] args) {

            Server<string, string> server = new Server<string, string>(new Pop3MessageParserFactory(), 110);
            server.Start();

            Console.WriteLine("Server started...");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
