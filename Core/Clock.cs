using System;

namespace Core
{
    public class Clock : IClock
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}