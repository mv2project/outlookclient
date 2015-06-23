using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;
using System.Net.Sockets;



namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    public class Pop3MessageParser : IMessageParser<string, string> {

        private readonly TcpClient client;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;
        private readonly NetworkStream stream;


        public Pop3MessageParser(TcpClient client) {
            this.client = client;
            this.stream = client.GetStream();
            this.reader = new StreamReader(stream, Encoding.ASCII);
            this.writer = new StreamWriter(stream, Encoding.ASCII);
        }

        public string ReadNext() {
            return reader.ReadLine();
        }

        public void Send(string output) {
            writer.WriteLine(output);
            writer.Flush();
        }

        public void Close() {
            writer.Flush();
            writer.Close();
            reader.Close();
            client.Close();
        }
    }
}
