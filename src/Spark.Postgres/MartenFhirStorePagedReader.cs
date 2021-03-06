﻿// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Postgres
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using Marten;
    using Marten.Pagination;

    public class MartenFhirStorePagedReader : IFhirStorePagedReader
    {
        private readonly Func<IDocumentSession> _sessionFunc;

        public MartenFhirStorePagedReader(Func<IDocumentSession> sessionFunc) => _sessionFunc = sessionFunc;

        /// <inheritdoc />
        public async IAsyncEnumerable<Entry> ReadAsync(
            FhirStorePageReaderOptions options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var pagesize = options?.PageSize ?? 100;
            var pageNumber = 0;
            using var session = _sessionFunc();
            while (true)
            {
                var data = await session.Query<EntryEnvelope>()
                    .OrderBy(x => x.Id)
                    .ToPagedListAsync(pageNumber, pagesize, cancellationToken)
                    .ConfigureAwait(false);

                foreach (var envelope in data)
                {
                    yield return Entry.Create(envelope.Method, envelope.Key, envelope.Resource);
                }

                if (data.IsLastPage)
                {
                    break;
                }
            }
        }
    }
}