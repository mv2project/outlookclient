using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace ISS.MV2.OutlookClient.ServerDummy.ServerCore {

    /// <summary>
    /// A basic server implementation to handle incoming messages/commands.
    /// </summary>
    public class Server<I, O> {

        private TcpListener serverListener;
        private Thread serverThread;

        private bool started = false;
        private readonly int port;

        private readonly object state_changes_lock = new object();

        private readonly IMessageParserFactory<I, O> factory;

        public Server(IMessageParserFactory<I, O> factory, int port) {
            this.port = port;
            this.factory = factory;
        }


        public void Start() {
            lock (state_changes_lock) {
                if (started) return;
                started = true;
                serverListener = new TcpListener(IPAddress.Loopback, port);
                serverThread = new Thread(Server_Work);
                serverThread.Start(serverListener);
            }
        }


        private void Server_Work(object param) {
            TcpListener listener = (TcpListener)param;
            listener.Start(10);
            TcpClient client;
            while (started) {
                client = listener.AcceptTcpClient();
                new ClientWorker<I, O>(factory, client).Start();
            }

        }


    }
}
