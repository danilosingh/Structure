using System;
using System.Timers;

namespace Structure.Helpers
{
    public static class TimerHelper
    {
        public static void OnTick(Action acao, double intervalo)
        {
            Timer timer = new Timer();
            
            timer.Elapsed += (s, e) => 
            {
                ((Timer)s).Enabled = false;
                acao();                
            };

            timer.Interval = intervalo;
            timer.Enabled = true;
        }
    }
}
