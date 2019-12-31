using Microsoft.Extensions.Logging;

namespace RedditImageDownloader.Logging
{
    public static class Log4netExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, bool skipDiagnosticLogs)
        {
            factory.AddProvider(new Log4NetProvider("log4net.config", skipDiagnosticLogs));
            return factory;
        }
    }
}
