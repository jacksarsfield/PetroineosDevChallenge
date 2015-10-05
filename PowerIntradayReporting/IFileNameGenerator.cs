using System;

namespace PowerIntradayReporting
{
    public interface IFileNameGenerator
    {
        string Generate(DateTime extractTime);
    }
}