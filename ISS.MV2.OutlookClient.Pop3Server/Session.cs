using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;
using ISS.MV2.IO;
using ISS.MV2.Messaging;
using ISS.MV2.OutlookClient.ServerDummy.Client;



namespace ISS.MV2.OutlookClient.ServerDummy {
    public class Session : ClientSession {


        public X509Certificate UserCertificate { get; set; }
        public AsymmetricKeyParameter UserPrivateKey { get; set; }




        public override ICommunicationPartner CreateClient() {
            IntermediateClient intermediateClient = new IntermediateClient();
            string host = Identifier.Split('@')[1].Trim();
            intermediateClient.Connect(host, 4503);
            return intermediateClient;
        }

        public override IMessageCryptorSettings CreateNewCryptorSettings() {
            return new AESWithRSACryptoSettings();
        }

        public override IMessageCryptorSettings CryptorSettings {
            get { return new AESWithRSACryptoSettings(); }
        }


    }
}
