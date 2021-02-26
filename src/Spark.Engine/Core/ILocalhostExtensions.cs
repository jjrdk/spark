using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;

namespace Spark.Engine.Core
{
    using Extensions;

    public static class LocalhostExtensions
    {
        public static bool IsLocal(this ILocalhost localhost, IKey key)
        {
            return key.Base == null ? true : localhost.IsBaseOf(key.Base);
        }

        public static bool IsForeign(this ILocalhost localhost, IKey key)
        {
            return !localhost.IsLocal(key);
        }

        public static Uri RemoveBase(this ILocalhost localhost, Uri uri)
        {
            var @base = localhost.GetBaseOf(uri)?.ToString();
            if (@base == null)
            {
                return uri;
            }
            else
            {
                var s = uri.ToString();
                var path = s.Remove(0, @base.Length);
                return new Uri(path, UriKind.Relative);
            }
        }

        public static Key LocalUriToKey(this ILocalhost localhost, Uri uri)
        {
            var s = uri.ToString();
            var @base = localhost.GetBaseOf(uri)?.ToString();
            var path = s.Remove(0, @base == null ? 0 : @base.Length);

            return Key.ParseOperationPath(path).WithBase(@base);
        }

        public static Key UriToKey(this ILocalhost localhost, Uri uri)
        {

            if (uri.IsAbsoluteUri && (uri.IsTemporaryUri() == false))
            {
                return localhost.IsBaseOf(uri)
                    ? localhost.LocalUriToKey(uri)
                    : throw new ArgumentException("Cannot create a key from a foreign Uri");
            }
            else if (uri.IsTemporaryUri())
            {
              return   Key.Create(null, uri.ToString());
            }
            else
            {
                var path = uri.ToString();
                return Key.ParseOperationPath(path);
            }
        }
        
        public static Key UriToKey(this ILocalhost localhost, string uristring)
        {
            var uri = new Uri(uristring, UriKind.RelativeOrAbsolute);
            return localhost.UriToKey(uri);
        }

        public static Uri GetAbsoluteUri(this ILocalhost localhost, IKey key)
        {
            return key.ToUri(localhost.DefaultBase);
        }

        public static KeyKind GetKeyKind(this ILocalhost localhost, IKey key)
        {
            if (key.IsTemporary())
            {
                return KeyKind.Temporary;
            }
            else if (!key.HasBase())
            {
                return KeyKind.Internal;
            }
            else
            {
                return localhost.IsLocal(key) ? KeyKind.Local : KeyKind.Foreign;
            }
        }

        public static bool IsBaseOf(this ILocalhost localhost, string uristring)
        {
            var uri = new Uri(uristring, UriKind.RelativeOrAbsolute);
            return localhost.IsBaseOf(uri);
        }

        //public static string GetOperationPath(this ILocalhost localhost, Uri uri)
        //{
        //    Key key = localhost.AnyUriToKey(uri).WithoutBase();

        //    return key.ToOperationPath();
        //    //Uri endpoint = localhost.GetBaseOf(uri);
        //    //string _base = endpoint.ToString();
        //    //string path = uri.ToString().Remove(0, _base.Length);
        //    //return path;
        //}

        public static Uri Uri(this ILocalhost localhost, params string[] segments)
        {
            return new RestUrl(localhost.DefaultBase).AddPath(segments).Uri;
        }

        public static Uri Uri(this ILocalhost localhost, IKey key)
        {
            return key.ToUri(localhost.DefaultBase);
        }

        public static Bundle CreateBundle(this ILocalhost localhost, Bundle.BundleType type)
        {
            var bundle = new Bundle
            {
                Type = type
            };
            return bundle;
        }
    }

}
