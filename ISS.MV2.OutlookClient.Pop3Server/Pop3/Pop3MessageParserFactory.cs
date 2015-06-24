using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;


namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    public class Pop3MessageParserFactory : IMessageParserFactory<string, string>, IClientConnectedHandler<string, string> {
        public IMessageParser<string, string> CreateParser(System.Net.Sockets.TcpClient client) {
            Pop3MessageParser parser = new Pop3MessageParser(client);
            return parser;
        }

        public void RegisterHandlers(ClientWorker<string, string> clientWorker) {
            clientWorker.ConnectedHandler = this;
            clientWorker.RegisterUnrecognizedHandler(new Pop3UnrecognizedMessageHandler());
            clientWorker.Register(new UserMessageHandler());
        }

        public void Connected(IMessageParser<string, string> parser) {
            parser.Send("+OK localhost");
        }
    }
}
