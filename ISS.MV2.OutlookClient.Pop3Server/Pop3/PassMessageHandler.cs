using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;
using ISS.MV2.OutlookClient.ServerDummy.Client;
using ISS.MV2.Threading;
using ISS.MV2.Messaging;

namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    class PassMessageHandler : Pop3MessageHandler {

        private readonly Session session;

        public PassMessageHandler(Session session) {
            this.session = session;
        }


        public override bool Handle(IMessageParser<string, string> parser, string message) {
            if (!IsMessage(message, "PASS")) return false;
            if (string.IsNullOrWhiteSpace(session.Identifier)) {
                Fail(parser, "Invalid state: USER expected.");
                return true;
            }
            Pop3MessageInterpreter interpreter = new Pop3MessageInterpreter(message);
            if (!interpreter.HasArguments) {
                Fail(parser, "Missing argument: The passphrase was missing.");
                return true;
            }
            session.Passphrase = interpreter[0].ToCharArray();
            LoginProcedure lp = new LoginProcedure(new InstantDispatcher(), session);

            bool result = true;
            try {
                result = lp.ExecuteImmediate();
            } catch (RequestException) {
                result = false;
            }
            if (!result) {
                Fail(parser, "Invalid passphrase.");
                return true;
            }
            parser.Send("+OK");
            return true;
        }
    }
}