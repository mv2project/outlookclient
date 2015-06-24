using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;

namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    abstract class Pop3MessageHandler  : IMessageHandler<string, string> {



        public abstract bool Handle(IMessageParser<string, string> parser, string message);

        protected bool IsMessage(string message, string expected) {
            if (message == null) {
                return (message == expected);
            }
            return message.StartsWith(expected, StringComparison.OrdinalIgnoreCase);
        }

    }
}
