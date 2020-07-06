using Structure.Runtime;
using System;

namespace Structure.Threading
{
    public class DisposeAction : IDisposable
    {
        private readonly Action action;

        public DisposeAction(Action action)
        {
            Check.NotNull(action, nameof(action));
            this.action = action;
        }

        public void Dispose()
        {
            action();
        }
    }
}
