using System.Threading.Tasks;

namespace Structure.Threading
{
    public static class TaskHelper
    {
        public static Task NullResult
        {
            get { return Task.FromResult<object>(null); }
        }
    }
}
