// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Search.Common
{
    public static class ArgumentFactory
    {
        public static Argument Create(Hl7.Fhir.Model.SearchParamType type, bool fuzzy = false)
        {
            switch (type)
            {
                case Hl7.Fhir.Model.SearchParamType.Number:
                    return new IntArgument();
                case Hl7.Fhir.Model.SearchParamType.String:
                    return new StringArgument();
                case Hl7.Fhir.Model.SearchParamType.Date:
                    return new DateArgument();
                case Hl7.Fhir.Model.SearchParamType.Token:
                    return new TokenArgument();
                case Hl7.Fhir.Model.SearchParamType.Reference:
                    return new ReferenceArgument();
                case Hl7.Fhir.Model.SearchParamType.Composite:
                    //TODO: Implement Composite arguments
                    return new Argument();
                default:
                    return new Argument();
            }
        }
    }
}