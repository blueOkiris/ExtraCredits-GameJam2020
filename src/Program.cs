using System;
using System.Threading;

namespace engine {
    class Program {
        static void Main(string[] args) {
            var engine = Engine.getInstance();
            engine.windowThread.Join();
        }
    }
}
