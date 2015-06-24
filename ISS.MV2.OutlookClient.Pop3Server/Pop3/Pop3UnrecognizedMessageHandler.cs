using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;


namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    class Pop3UnrecognizedMessageHandler : IMessageHandler<string, string>  {



        public bool Handle(IMessageParser<string, string> parser, string message) {
            parser.Send("-ERR Unrecognized");
            return true;
        }
    }
}
