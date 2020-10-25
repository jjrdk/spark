/* 
 * Copyright (c) 2020, Kufu (info@kufu.no) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Spark.Mongo.Tests.Search
{
    internal class BsonSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(System.Type type)
        {
            if (type == typeof(BsonNull))
            {
                return new BsonNullSerializer();
            }
            else if (type == typeof(string))
            {
                return new StringBsonSerializer();
            }

            return null;
        }
    }
}
