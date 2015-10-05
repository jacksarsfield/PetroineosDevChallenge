using System;
using PowerIntradayReporting.Constant;

namespace PowerIntradayReporting
{
    public class FileNameGenerator : IFileNameGenerator
    {
        public string Generate(DateTime extractTime)
        {
            return string.Format(ReportConstants.FileNameFormatString, extractTime);
        }
    }
}