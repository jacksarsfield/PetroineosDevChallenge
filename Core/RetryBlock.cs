using System;
using System.Threading;

namespace Core
{
    public class RetryBlock<T>
    {
        private readonly Func<T> _function;
        private Action _waitAction;

        public RetryBlock(Func<T> function)
        {
            _function = function;
        }

        public int MaxRetries { get; private set; }

        public int WaitTime { get; private set; }

        public RetryBlock<T> WithMaxRetries(int attempts)
        {
            if (attempts < 1)
            {
                throw new ArgumentException("Must specify at least one retry", "attempts");
            }

            MaxRetries = attempts;
            return this;
        }

        public RetryBlock<T> WithWaitBetweenRetries(int milliseconds, bool randomise = false)
        {
            if (milliseconds > 0)
            {
                WaitTime = milliseconds;
                if (randomise)
                {
                    var random = new Random();
                    var factor = (int)(WaitTime * 0.1);
                    WaitTime = random.Next(WaitTime - factor, WaitTime + factor);
                }
            }

            return this;
        }

        public RetryBlock<T> WithActionBetweenRetries(Action waitAction)
        {
            _waitAction = waitAction;
            return this;
        }

        public T Execute()
        {
            int attempts = 0;
            do
            {
                try
                {
                    return _function.Invoke();
                }
                catch (Exception)
                {
                    if (++attempts > MaxRetries)
                    {
                        throw;
                    }

                    if (WaitTime > 0)
                    {
                        if (_waitAction != null)
                        {
                            _waitAction.Invoke();
                        }

                        Thread.Sleep(WaitTime);
                    }
                }
            } while (true);
        }
    }
}