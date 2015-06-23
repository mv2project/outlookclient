using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISS.MV2.OutlookClient.ServerDummy.ServerCore {
    public interface IMessageHandler<I, O> {

        bool Handle(IMessageParser<I, O> parser, I message);

    }
}
