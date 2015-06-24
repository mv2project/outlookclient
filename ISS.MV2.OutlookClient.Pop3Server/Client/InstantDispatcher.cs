using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.Threading;

namespace ISS.MV2.OutlookClient.ServerDummy.Client {
    class InstantDispatcher : IEventDispatcher {


        public void Invoke(Threading.EventInvokationDelegate eventDelegate) {
            eventDelegate.Invoke();
        }
    }
}
