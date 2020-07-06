using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Structure.Gerenciadores
{
    public class TimeManager
    {
        private Dictionary<Timer, bool> timers = new Dictionary<Timer, bool>();

        public void Add(double interval, Action action, bool autostart = true, bool firstImmediateStart = false)
        {
            Add(interval, (s, e) => action(), firstImmediateStart, autostart);
        }

        public void Add(double interval, CustomElapsedEventHandler elapsed, bool autostart = true, bool firstImmediateStart = false)
        {
            Timer timer = new Timer();
            timer.Interval = firstImmediateStart ? 1 : interval;
            timer.Elapsed += (s, e) => wrapperTimer_Elapsed(s, e, elapsed, interval);

            bool iniciado = false;

            if (autostart)
            {
                timer.Start();
                iniciado = true;
            }

            timers.Add(timer, iniciado);
        }

        public void AddLateAtStartup(double interval, CustomElapsedEventHandler elapsed, double delay,
            bool firstImmediateStart = false)
        {
            Add(interval, elapsed, firstImmediateStart, false);

            var dicItem = timers.Last();

            Timer delayTimer = new Timer();
            delayTimer.Interval = delay;
            delayTimer.Elapsed += (s, e) =>
            {
                try
                {
                    (s as Timer).Stop();

                    Start(dicItem.Key);
                }
                finally
                {
                    (s as Timer).Dispose();
                }
            };

            delayTimer.Start();
        }

        public void StopAll()
        {
            foreach (var item in timers)
            {
                if (item.Value)
                {
                    item.Key.Stop();
                    timers[item.Key] = false;
                }
            }
        }

        public void StartAll()
        {
            foreach (var item in timers)
            {
                if (!item.Value)
                {
                    item.Key.Start();
                    timers[item.Key] = true;
                }
            }
        }

        public void Start(Timer timer)
        {
            timer.Start();
            timers[timer] = true;
        }

        private void wrapperTimer_Elapsed(object sender, ElapsedEventArgs e, CustomElapsedEventHandler elapsed, double intervalo)
        {
            Timer timer = sender as Timer;
            timer.Stop();

            CustomElapsedEventArgs args = new CustomElapsedEventArgs();
            elapsed(timer, args);

            if (!args.Stop)
            {
                timer.Interval = intervalo;
                timer.Start();
            }
        }
    }

    public delegate void CustomElapsedEventHandler(Timer sender, CustomElapsedEventArgs args);

    public class CustomElapsedEventArgs
    {
        public bool Stop { get; set; }
    }
}
