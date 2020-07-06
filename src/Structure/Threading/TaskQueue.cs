using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Structure.Threading
{
    public class TaskQueue : IDisposable
    {
        public static TaskQueue Global = new TaskQueue();

        private BlockingCollection<Action> queue = new BlockingCollection<Action>();

        public int CurrentlyQueuedTasks { get { return queue.Count; } }

        public TaskQueue()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Action action = queue.Take();
                    action();
                }
            });
        }

        public void Append(Action action)
        {
            queue.Add(action);
        }

        public void Dispose()
        {
            queue.CompleteAdding();
        }
    }
}
