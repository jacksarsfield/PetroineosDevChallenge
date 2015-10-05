using System.Reactive.Concurrency;

namespace Core
{
    public class ScheduleProvider : IScheduleProvider
    {
        public IScheduler Current
        {
            get { return Scheduler.CurrentThread; }
        }

        public IScheduler Default
        {
            get { return Scheduler.Default; }
        }

        public IScheduler NewThread
        {
            get { return NewThreadScheduler.Default; }
        }
    }
}