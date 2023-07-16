using long_term_care.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

public class DatabaseLoggerProvider : ILoggerProvider
{
    private readonly longtermcareContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DatabaseLoggerProvider(longtermcareContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DatabaseLogger(categoryName, null, _dbContext,_httpContextAccessor);
    }

    public void Dispose()
    {
        // 如果有需要，可以在這裡釋放資源
    }
}

public class DatabaseLogger : ILogger
{
    private readonly string _categoryName;
    private readonly Func<string, LogLevel, bool> _filter;
    private readonly longtermcareContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DatabaseLogger(string categoryName, Func<string, LogLevel, bool> filter, longtermcareContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _categoryName = categoryName;
        _filter = filter;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return (_filter == null || _filter(_categoryName, logLevel));
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (exception != null)
        {
            message += Environment.NewLine + exception;
        }

        _dbContext.ChangeLogs.Add(new ChangeLog
        {
            TableName = "YourTableName",
            ActionType = logLevel.ToString(),
            ActionDate = DateTime.UtcNow,
            ActionBy = userName, // 設定執行動作的使用者名稱
            Details = message
        });

        _dbContext.SaveChanges();
    }
}
