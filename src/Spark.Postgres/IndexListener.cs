﻿namespace Spark.Postgres
{
    using System;
    using System.Threading.Tasks;
    using Engine.Core;
    using Service;

    public class IndexListener : IServiceListener
    {
        private readonly IIndexService _index;

        public IndexListener(IIndexService index)
        {
            _index = index;
        }

        /// <inheritdoc />
        public Task Inform(Uri location, Entry interaction) => _index.Process(interaction);
    }
}