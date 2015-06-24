using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISS.MV2.OutlookClient.ServerDummy.ServerCore {
    public interface IMessageParserFactory<I, O>  {


        IMessageParser<I, O> CreateParser(System.Net.Sockets.TcpClient client);

        void RegisterHandlers(ClientWorker<I, O> clientWorker);




    }
}
