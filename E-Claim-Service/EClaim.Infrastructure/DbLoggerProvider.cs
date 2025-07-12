using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Infrastructure
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbLoggerProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(_scopeFactory, categoryName);
        }

        public void Dispose() { }
    }
}
