using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterBot
{
    class SafeThread
    {
        private Action function;
        private Thread thread;
        private bool shouldStop = false;
        private bool stopped = true;

        public bool Stopped { get { return stopped; } }

        public SafeThread(Action function)
        {
            this.function = function;
            thread = new Thread(Work);
        }
        
        public void Start()
        {
            if (!stopped)
            {
                Stop();
                while (!stopped || thread.ThreadState == ThreadState.Running) { }
            }
            thread.Start();
            stopped = false;
        }

        public void Stop()
        {
            shouldStop = true;
        }

        private void Work()
        {
            while (!shouldStop)
            {
                function();
            }
            stopped = true;
        }
    }
}
