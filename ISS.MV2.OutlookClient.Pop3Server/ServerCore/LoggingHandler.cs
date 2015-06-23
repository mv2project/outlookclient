using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISS.MV2.OutlookClient.ServerDummy.ServerCore {
    public class LoggingHandler<I, O> : IMessageHandler<I, O> {




        public bool Handle(IMessageParser<I, O> parser, I message) {
            System.Diagnostics.Debug.WriteLine(message);
            return false;
        }
    }
}
