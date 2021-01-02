﻿namespace Spark.Postgres
{
    using System;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Service;
    using Microsoft.Extensions.Logging;

    public class LogListener : IServiceListener
    {
        private readonly ILogger<LogListener> _logger;

        public LogListener(ILogger<LogListener> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public Task Inform(Uri location, Entry interaction)
        {
            _logger.LogDebug($"{location} -> {interaction.Key}");
            return Task.CompletedTask;
        }
    }
}