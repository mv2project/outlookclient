using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ISS.MV2.OutlookClient.ServerDummy.ServerCore {
    public class ClientWorker<I, O> {

        private readonly IMessageParser<I, O> parser;
        private readonly Thread workerThread;
        private bool active;

        private readonly IList<IMessageHandler<I, O>> handlers = new List<IMessageHandler<I, O>>();

        public ClientWorker(IMessageParserFactory<I, O> factory, TcpClient client) {
            this.parser = factory.CreateParser(client);
            factory.RegisterHandlers(this);
            this.workerThread = new Thread(DoWork);
            Register(new LoggingHandler<I, O>());
        }

        public void Register(IMessageHandler<I, O> handler) {
            if (!handlers.Contains(handler)) handlers.Add(handler);
        }

        public void Start() {
            active = true;
            workerThread.Start();
        }


        public void Stop() {
            if (!active) return;
            active = false;
            workerThread.Abort();
        }

        private void DoWork() {
            try {
                I message;
                while (active) {
                    message = parser.ReadNext();
                    foreach (IMessageHandler<I, O> h in handlers) {
                        if (h.Handle(parser, message)) break;
                    }
                }
            } catch (ThreadAbortException ex) {
                parser.Close();
            }
        }


    }
}
