using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;

namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    class UserMessageHandler : Pop3MessageHandler{


        public override bool Handle(IMessageParser<string, string> parser, string message) {
            if (!IsMessage(message, "USER")) return false;
            parser.Send("+OK");
            return true;
        }
    }
}
