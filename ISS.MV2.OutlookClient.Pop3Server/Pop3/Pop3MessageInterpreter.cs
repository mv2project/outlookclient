using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISS.MV2.OutlookClient.ServerDummy.Pop3 {
    class Pop3MessageInterpreter {

        private readonly string command;
        public string Command { get { return command; } }

        private readonly string[] arguments;

        public string this[int index] {
            get {
                return arguments[index];
            }
        }

        public bool HasArguments { get { return arguments.Length > 0; } }
        public int ArgumentsCount { get { return arguments.Length; } }



        public Pop3MessageInterpreter(string command) {
            if (command.Contains(" ")) {
                string[] args = command.Split(' ');
                this.command = args[0];
                this.arguments = new string[args.Length - 1];
                for (int i = 1; i < args.Length; i++) {
                    this.arguments[i - 1] = args[i];
                }
            } else {
                this.command = command;
                this.arguments = new string[0];
            }
        }

    }
}
