using System.Reactive.Concurrency;
using Core;
using Microsoft.Reactive.Testing;

namespace PowerIntradayReportingTest
{
    public class TestSchedulers : IScheduleProvider
    {
        private readonly TestScheduler _currentSchedule = new TestScheduler();
        private readonly TestScheduler _defaultSchedule = new TestScheduler();
        private readonly TestScheduler _newThreadSchedule = new TestScheduler();

        IScheduler IScheduleProvider.Current
        {
            get { return _currentSchedule; }
        }

        IScheduler IScheduleProvider.Default
        {
            get { return _defaultSchedule; }
        }

        IScheduler IScheduleProvider.NewThread
        {
            get { return _newThreadSchedule; }
        }

        public TestScheduler Current { get { return _currentSchedule; } }
        public TestScheduler Default { get { return _defaultSchedule; } }
        public TestScheduler NewThread { get { return _currentSchedule; } }
    }
}