// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Postgres
{
    using System;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using Marten;

    public class MartenFhirStorePagedReader : IFhirStorePagedReader
    {
        private readonly Func<IDocumentSession> _sessionFunc;

        public MartenFhirStorePagedReader(Func<IDocumentSession> sessionFunc) => _sessionFunc = sessionFunc;

        /// <inheritdoc />
        public async Task<IPageResult<Entry>> ReadAsync(FhirStorePageReaderOptions options = null)
        {
            var pagesize = options?.PageSize ?? 100;
            var total = 0;
            using (var session = _sessionFunc())
            {
                total = await session.Query<EntryEnvelope>().CountAsync().ConfigureAwait(false);
            }

            return new MartenPageResult<Entry>(_sessionFunc, pagesize, total, e => e);
        }
    }
}