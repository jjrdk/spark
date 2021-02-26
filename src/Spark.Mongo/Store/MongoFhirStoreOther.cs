﻿namespace Spark.Mongo.Store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Engine.Core;
    using Engine.Store.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Search.Infrastructure;

    //TODO: decide if we still need this
    [Obsolete("Don't use it at all")]
    public class MongoFhirStoreOther
    {
        private readonly IFhirStore _mongoFhirStoreOther;
        readonly IMongoDatabase _database;
        readonly IMongoCollection<BsonDocument> _collection;

        public MongoFhirStoreOther(string mongoUrl, IFhirStore mongoFhirStoreOther)
        {
            _mongoFhirStoreOther = mongoFhirStoreOther;
            this._database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            this._collection = _database.GetCollection<BsonDocument>(Collection.RESOURCE);
            //this.transaction = new MongoSimpleTransaction(collection);
        }

        //TODO: I've commented this. Do we still need it?
        //public IList<string> List(string resource, DateTimeOffset? since = null)
        //{
        //    var clauses = new List<FilterDefinition<BsonDocument>>();

        //    clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.TYPENAME, resource));
        //    if (since != null)
        //    {
        //        clauses.Add(Builders<BsonDocument>.Filter.GT(Field.WHEN, BsonDateTime.Create(since)));
        //    }
        //    clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT));

        //    return FetchPrimaryKeys(clauses);
        //}


        public async Task<bool> Exists(IKey key)
        {
            // PERF: efficiency
            var existing = await _mongoFhirStoreOther.Get(key).ConfigureAwait(false);
            return existing != null;
        }

        //public Interaction Get(string primarykey)
        //{
        //    FilterDefinition<BsonDocument> query = MonQ.Query.Eq(Field.PRIMARYKEY, primarykey);
        //    BsonDocument document = collection.FindOne(query);
        //    if (document != null)
        //    {
        //        Interaction entry = document.ToInteraction();
        //        return entry;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public IList<Entry> GetCurrent(IEnumerable<string> identifiers, string sortby = null)
        {
            var clauses = new List<FilterDefinition<BsonDocument>>();
            var ids = identifiers.Select(i => (BsonValue)i);

            clauses.Add(Builders<BsonDocument>.Filter.In(Field.REFERENCE, ids));
            clauses.Add(Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT));
            var query = Builders<BsonDocument>.Filter.And(clauses);

            var cursor = _collection.Find(query);

            if (sortby != null)
            {
                cursor = cursor.Sort(Builders<BsonDocument>.Sort.Ascending(sortby));
            }
            else
            {
                cursor = cursor.Sort(Builders<BsonDocument>.Sort.Descending(Field.WHEN));
            }

            return cursor.ToEnumerable().ToEntries().ToList();
        }

        private void Supercede(IEnumerable<IKey> keys)
        {
            var pks = keys.Select(k => k.ToBsonReferenceKey());
            var query = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.In(Field.REFERENCE, pks),
                Builders<BsonDocument>.Filter.Eq(Field.STATE, Value.CURRENT)
                );
            var update = Builders<BsonDocument>.Update.Set(Field.STATE, Value.SUPERCEDED);
            _collection.UpdateMany(query, update);
        }

        public void Add(IEnumerable<Entry> entries)
        {
            var keys = entries.Select(i => i.Key);
            Supercede(keys);
            IList<BsonDocument> documents = entries.Select(SparkBsonHelper.ToBsonDocument).ToList();
            _collection.InsertMany(documents);
        }

        public void Replace(Entry entry)
        {
            /*
            string versionid = entry.Resource.Meta.VersionId;

            FilterDefinition<BsonDocument> query = Builders<BsonDocument>.Filter.Eq(Field.VERSIONID, versionid);
            BsonDocument current = collection.Find(query).FirstOrDefault();
            BsonDocument replacement = SparkBsonHelper.ToBsonDocument(entry);
            SparkBsonHelper.TransferMetadata(current, replacement);

            IMongoUpdate update = MongoDB.Driver.Builders.Update.Replace(replacement);
            collection.Update(query, update);
            */
        }

        public bool CustomResourceIdAllowed(string value)
        {
            if (value.StartsWith(Value.IDPREFIX))
            {
                var remainder = value.Substring(1);
                int i;
                var isint = int.TryParse(remainder, out i);
                return !isint;
            }
            return true;
        }

        /*public Tag BsonValueToTag(BsonValue item)
        {
            Tag tag = new Tag(
                   item["term"].AsString,
                   new Uri(item["scheme"].AsString),
                   item["label"].AsString);

            return tag;
        }

        public IEnumerable<Tag> Tags()
        {
            return collection.Distinct(Field.CATEGORY).Select(BsonValueToTag);
        }

        public IEnumerable<Tag> Tags(string resourcetype)
        {
            FilterDefinition<BsonDocument> query = MonQ.Query.Eq(Field.COLLECTION, resourcetype);
            return collection.Distinct(Field.CATEGORY, query).Select(BsonValueToTag);
        }

        public IEnumerable<Uri> Find(params Tag[] tags)
        {
            throw new NotImplementedException("Finding tags is not implemented on database level");
        }
        */





        /// <summary>
        /// Does a complete wipe and reset of the database. USE WITH CAUTION!
        /// </summary>



    }
}