using System;

namespace Core
{
    public interface IClock
    {
        DateTime UtcNow();
        DateTime Now();
    }
}