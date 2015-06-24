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
    public class Session {

        public string Username { get; set; }
        public string Passphrase { get; set; }

        public X509Certificate UserCertificate { get; set; }
        public AsymmetricKeyParameter UserPrivateKey { get; set; }



    }
}
