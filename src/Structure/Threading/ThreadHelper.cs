using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Structure.Helpers
{
    public static class ThreadHelper
    {
        public static void StartAfterSleep(ThreadStart metodo, int timeout)
        {
            Thread.Sleep(timeout);
            Start(metodo);
        }

        public static void StartAfterTimeout(Action metodo, int timeout)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = timeout;
            timer.Elapsed += (s, e) =>
            {
                ((System.Timers.Timer)(s)).Enabled = false;
                metodo();
            };
            timer.Enabled = true;
        }


        public static void Start(ThreadStart metodo)
        {
            Thread th = new Thread(metodo);
            th.Start();
        }
    }
}
