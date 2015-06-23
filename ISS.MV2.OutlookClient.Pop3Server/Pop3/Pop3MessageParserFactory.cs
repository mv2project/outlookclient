using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;


namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    public class Pop3MessageParserFactory : IMessageParserFactory<string, string> {
        public IMessageParser<string, string> CreateParser(System.Net.Sockets.TcpClient client) {
            return new Pop3MessageParser(client);
        }

        public void RegisterHandlers(ClientWorker<string, string> clientWorker) {
            
        }
    }
}
