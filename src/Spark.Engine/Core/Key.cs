namespace Spark.Engine.Core
{
    using Extensions;

    // BALLOT: ResourceId is in the standard called "Logical Id" but this term doesn't have a lot of meaning. I propose "Technical Id" or "Surrogate key"
    // http://en.wikipedia.org/wiki/Surrogate_key

    public class Key : IKey
    {
        public string Base { get; set; }
        public string TypeName { get; set; }
        public string ResourceId { get; set; }
        public string VersionId { get; set; }

        public Key() { }

        public Key(string @base, string type, string resourceId, string versionId)
        {
            this.Base = @base?.TrimEnd('/');
            this.TypeName = type;
            this.ResourceId = resourceId;
            this.VersionId = versionId;
        }

        public static Key Create(string type)
        {
            return new Key(null, type, null, null);
        }

        public static Key Create(string type, string resourceId)
        {
            return new Key(null, type, resourceId, null);
        }

        public static Key Create(string type, string resourceId, string versionId)
        {
            return new Key(null, type, resourceId, versionId);
        }

        public static Key Null => default;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Key)obj;
            return this.ToUriString() == other.ToUriString();
        }

        public override int GetHashCode()
        {
            return this.ToUriString().GetHashCode();
        }

        public static Key ParseOperationPath(string path)
        {
            var key = new Key();
            path = path.Trim('/');
            var indexOfQueryString = path.IndexOf('?');
            if (indexOfQueryString >= 0)
            {
                path = path.Substring(0, indexOfQueryString);
            }
            var segments = path.Split('/');
            if (segments.Length >= 1)
            {
                key.TypeName = segments[0];
            }

            if (segments.Length >= 2)
            {
                key.ResourceId = segments[1];
            }

            if (segments.Length == 4 && segments[2] == "_history")
            {
                key.VersionId = segments[3];
            }

            return key;
        }

        public override string ToString()
        {
            return this.ToUriString();
        }
    }

}
