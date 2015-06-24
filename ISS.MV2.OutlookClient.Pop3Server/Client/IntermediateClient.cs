using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.Messaging;
using ISS.MV2.Security;
using ISS.MV2.IO;
using Org.BouncyCastle.X509;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;


namespace ISS.MV2.OutlookClient.ServerDummy.Client {
    class IntermediateClient : ICommunicationPartner {

        private TcpClient connection;
        private NetworkStream connectionStream;
        private MessageParser parser;
        public delegate void MessageTransferDelegate(ICommunicationPartner sender, MV2Message message);
        public event MessageTransferDelegate MessageSent;
        public event MessageTransferDelegate MessageReceived;

        private X509Certificate serverCerificate;

        public X509Certificate Certificate {
            get {
                return serverCerificate;
            }
        }

        public IMessageCryptorSettings CryptorSettings {
            get {
                if (parser == null) return null;
                return parser.Settings;
            }
            set {
                if (parser == null) throw new InvalidOperationException();
                parser.Settings = value;
            }
        }

        private string[] availableDomains = new string[0];
        public string[] AvailableDomains { get { return availableDomains; } }

        private readonly IList<IMessagePreProcessor> preProcessors = new List<IMessagePreProcessor>();
        private readonly IList<IMessageProcessor> processors = new List<IMessageProcessor>();

        public IntermediateClient() {
            AddPreProcessor(new CertificateResponsePreProcessor());
            AddPreProcessor(new DomainNamesResponsePreProcessor());

        }

        public void Connect(string host, int port) {
            if (connection != null) throw new IOException("Already connected!");
            connection = new TcpClient(host, port);
            connectionStream = connection.GetStream();
            parser = new MessageParser(connectionStream);
            parser.Settings = new AESWithRSACryptoSettings();
            HelloMessage helloMessage = new HelloMessage();
            helloMessage.HostName = host;
            Send(helloMessage);
            HandleNext();
            Send(new MV2Message(DEF_MESSAGE.CERT_REQUEST));
            HandleNext();
        }

        public void AddPreProcessor(IMessagePreProcessor preProcessor) {
            if (!preProcessors.Contains(preProcessor)) preProcessors.Add(preProcessor);
        }

        public void AddProcessort(IMessageProcessor processor) {
            if (!processors.Contains(processor)) processors.Add(processor);
        }

        public MV2Message HandleNext() {
            MV2Message message = parser.ReadNext();
            foreach (IMessagePreProcessor pp in preProcessors) {
                message = pp.Prepare(this, message);
            }
            if (message is CertificateResponseMessage) {
                CertificateResponseMessage crm = (CertificateResponseMessage)message;
                X509Certificate cert = crm.Certificate;
                if (VerifyCertificate(cert)) serverCerificate = cert;
            }
            if (message is DomainNamesResponse) {
                availableDomains = ((DomainNamesResponse)message).AvailableDomainNames;
            }
            foreach (IMessageProcessor p in processors) {
                p.Process(this, message);
            }
            if (MessageReceived != null) MessageReceived(this, message);
            return message;
        }

        private bool VerifyCertificate(X509Certificate cert) {
            return true;
        }

        public void Send(MV2Message message) {
            bool isEncrypted = false;
            if (Certificate != null && message.MessageType != DEF_MESSAGE.ENCRYPTED_MESSAGE) {
                EncryptedMessage encrypted = new EncryptedMessage(parser.Settings, Certificate.GetPublicKey(), message.MessageType);
                MV2Message.Merge(encrypted, message);
                message = encrypted;
                isEncrypted = true;
            }
            message.Serialize(connectionStream);
            if (isEncrypted && !parser.Settings.KeyGenerator.HasFixedKeyAndIV) {
                EncryptedMessage encrypted = (EncryptedMessage)message;
                parser.Settings.KeyGenerator.SetFixedIV(encrypted.UsedSymmetricIV);
                parser.Settings.KeyGenerator.SetFixedKey(encrypted.UsedSymmetricKey);
            }
            if (MessageSent != null) MessageSent(this, message);
        }



    }
}
