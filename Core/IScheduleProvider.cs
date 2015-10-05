using System.Reactive.Concurrency;

namespace Core
{
    public interface IScheduleProvider
    {
        IScheduler Current { get; }
        IScheduler Default { get; }    
        IScheduler NewThread { get; }
    }
}