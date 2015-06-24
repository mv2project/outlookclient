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
        private readonly IList<IMessageHandler<I, O>> unrecognizedHandlers = new List<IMessageHandler<I, O>>();

        public IClientConnectedHandler<I, O> ConnectedHandler { get; set; }
        

        public ClientWorker(IMessageParserFactory<I, O> factory, TcpClient client) {
            this.parser = factory.CreateParser(client);
            factory.RegisterHandlers(this);
            this.workerThread = new Thread(DoWork);
            Register(new LoggingHandler<I, O>());
        }

        public void Register(IMessageHandler<I, O> handler) {
            if (!handlers.Contains(handler)) handlers.Add(handler);
        }

        public void RegisterUnrecognizedHandler(IMessageHandler<I, O> handler) {
            if (!unrecognizedHandlers.Contains(handler)) unrecognizedHandlers.Add(handler);
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
                if (ConnectedHandler != null) ConnectedHandler.Connected(parser);
                I message;
                bool recognized;
                while (active) {
                    message = parser.ReadNext();
                    recognized = false;
                    foreach (IMessageHandler<I, O> h in handlers) {
                        if ((recognized = h.Handle(parser, message))) break;
                    }
                    if (!recognized) foreach (IMessageHandler<I, O> h in unrecognizedHandlers) h.Handle(parser, message);
                }
            } catch (ThreadAbortException) {

            } catch (System.IO.IOException ex) {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            parser.Close();
        }


    }
}
