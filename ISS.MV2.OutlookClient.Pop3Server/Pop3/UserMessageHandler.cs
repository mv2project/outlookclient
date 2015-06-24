using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISS.MV2.OutlookClient.ServerDummy.ServerCore;

namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    class UserMessageHandler : Pop3MessageHandler{

        private readonly Session session;

        public UserMessageHandler(Session session) {
            this.session = session;
        }

        public override bool Handle(IMessageParser<string, string> parser, string message) {
            if (!IsMessage(message, "USER")) return false;
            Pop3MessageInterpreter interpreter = new Pop3MessageInterpreter(message);
            if (interpreter.ArgumentsCount != 1) {
                Fail(parser, "Missing argument: Mail account is missing.");
                return true;   
            }
            string mail = interpreter[0];
            if (!mail.Contains("@")) {
                Fail(parser, "Invalid argument: Illegal mail address.");
                return true;
            }
            session.Identifier = mail;
            parser.Send("+OK");
            return true;
        }
    }
}
