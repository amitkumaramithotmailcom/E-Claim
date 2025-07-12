using EClaim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Infrastructure
{
    public class DbLogger : ILogger
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _category;

        public DbLogger(IServiceScopeFactory scopeFactory, string category)
        {
            _scopeFactory = scopeFactory;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var message = formatter(state, exception);
            if (exception != null)
                message += $"\nException : {exception.Message}\n{exception.StackTrace}";


            try
            {
                dbContext.Logs.Add(new AppLog
                {
                    Level = logLevel.ToString(),
                    Message = message,
                    Category = _category,
                });

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
           
        }
    }
}
