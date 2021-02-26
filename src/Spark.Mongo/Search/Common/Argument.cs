/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

namespace Spark.Mongo.Search.Common
{
    using Searcher;

    public class Argument
    {
        public virtual string GroomElement(string value)
        {
            return value;
        }
        public virtual string ValueToString(ITerm term)
        {
            return term.Value;
        }
        public virtual string FieldToString(ITerm term)
        {
            return term.Operator != null ? term.Field + ":" + term.Operator : term.Field;
        }
        public virtual bool Validate(string value)
        {
            return true;
        }
        private static string FieldToInternalField(string field)
        {
            if (Config.Equal(field, UniversalField.ID))
            {
                field = InternalField.JUSTID;
            }

            return field;
        }
    }
}