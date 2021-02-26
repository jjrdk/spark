using System;

namespace Spark.Engine.Core
{
    public class Localhost : ILocalhost
    {
        public Uri DefaultBase { get; set; }

        public Localhost(Uri baseuri)
        {
            this.DefaultBase = baseuri;
        }

        public Uri Absolute(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return uri;
            }

            var @base = DefaultBase.ToString().TrimEnd('/') + "/";
            return new Uri(@base + uri);
        }

        public bool IsBaseOf(Uri uri)
        {
            return uri.IsAbsoluteUri && UriHelper.IsBaseOf(DefaultBase, uri);
        }

        public Uri GetBaseOf(Uri uri)
        {
            return this.IsBaseOf(uri) ? this.DefaultBase : null;
        }
    }


}
