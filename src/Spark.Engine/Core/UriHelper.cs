using System;

namespace Spark.Engine.Core
{
    public static class UriHelper
    {
        public static bool IsTemporaryUri(this Uri uri)
        {
            return uri != null && IsTemporaryUri(uri.ToString());
        }

        public static bool IsTemporaryUri(string uri)
        {
            return uri.StartsWith("urn:uuid:")
                || uri.StartsWith("urn:guid:")
                || uri.StartsWith("cid:");
        }

        /// <summary>
        /// Determines whether the uri contains a hash (#) fragment.
        /// </summary>
        public static bool HasFragment(this Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                var fragment = uri.Fragment;
                return !string.IsNullOrEmpty(fragment);
            }

            var s = uri.ToString();
            return s.StartsWith("#");
        }

        /// <summary>
        /// Bug fixed_IsBaseOf is a fix for Uri.IsBaseOf which has a bug
        /// </summary>
        public static bool IsBaseOf(this Uri @base, Uri uri)
        {
            var b = @base.ToString().ToLowerInvariant();
            var u = uri.ToString().ToLowerInvariant();

            return u.StartsWith(b);
        }

    }
}
