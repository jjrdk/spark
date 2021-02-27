// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Search.Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Common;
    using Engine.Extensions;
    using Engine.Search.ValueExpressionTypes;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Utility;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Utils;
    using Expression = Engine.Search.ValueExpressionTypes.Expression;

    // todo: DSTU2 - NonExistent classes: Operator, Expression, ValueExpression

    internal static class CriteriaMongoExtensions
    {
        private static readonly List<MethodInfo> _fixedQueries = CacheQueryMethods();

        private static List<MethodInfo> CacheQueryMethods()
        {
            return typeof(CriteriaMongoExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(m => m.Name.EndsWith("FixedQuery"))
                .ToList();
        }

        //internal static FilterDefinition<BsonDocument> ResourceFilter(this Query query)
        //{
        //    return ResourceFilter(query.ResourceType);
        //}

        internal static FilterDefinition<BsonDocument> ResourceFilter(string resourceType, int level)
        {
            var queries = new List<FilterDefinition<BsonDocument>>();
            if (level == 0)
            {
                queries.Add(Builders<BsonDocument>.Filter.Eq(InternalField.LEVEL, 0));
            }

            queries.Add(Builders<BsonDocument>.Filter.Eq(InternalField.RESOURCE, resourceType));

            return Builders<BsonDocument>.Filter.And(queries);
        }

        internal static ModelInfo.SearchParamDefinition FindSearchParamDefinition(
            this Criterium param,
            string resourceType)
        {
            return param.SearchParameters?.FirstOrDefault(
                sp => sp.Resource == resourceType || sp.Resource == "Resource");

            //var sp = ModelInfo.SearchParameters;
            //return sp.Find(defn => defn.Name == param.ParamName && defn.Resource == resourceType);
        }

        internal static FilterDefinition<BsonDocument> ToFilter(this Criterium param, string resourceType)
        {
            //Maybe it's a generic parameter.
            var methodForParameter = _fixedQueries.Find(m => m.Name.Equals(param.ParamName + "FixedQuery"));
            if (methodForParameter != null)
            {
                return (FilterDefinition<BsonDocument>) methodForParameter.Invoke(null, new object[] {param});
            }

            //Otherwise it should be a parameter as defined in the metadata
            var critSp = FindSearchParamDefinition(param, resourceType);
            if (critSp != null)
            {
                // todo: DSTU2 - modifier not in SearchParameter
                return CreateFilter(critSp, param.Operator, param.Modifier, param.Operand);
                //return null;
            }

            throw new ArgumentException($"Resource {resourceType} has no parameter with the name {param.ParamName}.");
        }

        //private static FilterDefinition<BsonDocument> SetParameter(this FilterDefinition<BsonDocument> query, string parameterName, IEnumerable<string> values)
        //{
        //    return query.SetParameter(new BsonArray() { parameterName }.ToJson(), new BsonArray(values).ToJson());
        //}

        //private static FilterDefinition<BsonDocument> SetParameter(this FilterDefinition<BsonDocument> query, string parameterName, string value)
        //{
        //    return BsonDocument.Parse(query.ToString().Replace(parameterName, value));
        //}

        private static FilterDefinition<BsonDocument> CreateFilter(
            ModelInfo.SearchParamDefinition parameter,
            Operator op,
            string modifier,
            Expression operand)
        {
            if (op == Operator.CHAIN)
            {
                throw new NotSupportedException("Chain operators should be handled in MongoSearcher.");
            }

            var parameterName = parameter.Name;
            if (parameterName == "_id")
            {
                parameterName = "fhir_id"; //See MongoIndexMapper for counterpart.

                // This search finds the patient resource with the given id (there can only be one resource for a given id).
                // Functionally, this is equivalent to a simple read operation
                modifier = Modifier.EXACT;
            }

            var valueOperand = (ValueExpression) operand;
            switch (parameter.Type)
            {
                case SearchParamType.Composite:
                    return CompositeQuery(parameter, op, modifier, valueOperand);
                case SearchParamType.Date:
                    return DateQuery(parameterName, op, modifier, valueOperand);
                case SearchParamType.Number:
                    return NumberQuery(parameter.Name, op, valueOperand);
                case SearchParamType.Quantity:
                    return QuantityQuery(parameterName, op, valueOperand);
                case SearchParamType.Reference:
                    //Chain is handled in MongoSearcher, so here we have the result of a closed criterium: IN [ list of id's ]
                    if (parameter.Target?.Any() == true && !valueOperand.ToUnescapedString().Contains("/"))
                    {
                        // For searching by reference without type specified.
                        // If reference target type is known, create the exact query like ^(Person|Group)/123$
                        return Builders<BsonDocument>.Filter.Regex(
                            parameterName,
                            new BsonRegularExpression(
                                new Regex(
                                    $"^({string.Join("|", parameter.Target)})/{valueOperand.ToUnescapedString()}$")));
                    }
                    else
                    {
                        return StringQuery(parameterName, op, Modifier.EXACT, valueOperand);
                    }
                case SearchParamType.String:
                    return StringQuery(parameterName, op, modifier, valueOperand);
                case SearchParamType.Token:
                    return TokenQuery(parameterName, op, modifier, valueOperand);
                case SearchParamType.Uri:
                    return UriQuery(parameterName, op, modifier, valueOperand);
                default:
                    //return Builders<BsonDocument>.Filter.Null;
                    throw new NotSupportedException(
                        $"SearchParamType {parameter.Type} on parameter {parameter.Name} not supported.");
            }
        }

        private static List<string> GetTargetedReferenceTypes(
            ModelInfo.SearchParamDefinition parameter,
            string modifier)
        {
            var allowedResourceTypes =
                parameter.Target.Select(t => t.GetLiteral())
                    .ToList(); // ModelInfo.SupportedResources; //TODO: restrict to parameter.ReferencedResources. This means not making this static, because you want to use IFhirModel.
            var searchResourceTypes = new List<string>();
            if (string.IsNullOrEmpty(modifier))
            {
                searchResourceTypes.AddRange(allowedResourceTypes);
            }
            else if (allowedResourceTypes.Contains(modifier))
            {
                searchResourceTypes.Add(modifier);
            }
            else
            {
                throw new NotSupportedException("Referenced type cannot be of type %s.");
            }

            return searchResourceTypes;
        }

        internal static List<string> GetTargetedReferenceTypes(this Criterium chainCriterium, string resourceType)
        {
            if (chainCriterium.Operator != Operator.CHAIN)
            {
                throw new ArgumentException("Targeted reference types are only relevent for chained criteria.");
            }

            var critSp = chainCriterium.FindSearchParamDefinition(resourceType);
            var modifier = chainCriterium.Modifier;
            var nextInChain = (Criterium) chainCriterium.Operand;
            var nextParameter = nextInChain.ParamName;
            // The modifier contains the type of resource that the referenced resource must be. It is optional.
            // If not present, search all possible types of resources allowed at this reference.
            // If it is present, it should be of one of the possible types.

            var searchResourceTypes = GetTargetedReferenceTypes(critSp, modifier);

            // Afterwards, filter on the types that actually have the requested searchparameter.
            return searchResourceTypes.Where(
                    rt => InternalField.All.Contains(nextParameter)
                          || UniversalField.All.Contains(nextParameter)
                          || ModelInfo.SearchParameters.Exists(
                              sp => rt.Equals(sp.Resource) && nextParameter.Equals(sp.Name)))
                .ToList();
        }

        private static FilterDefinition<BsonDocument> StringQuery(
            string parameterName,
            Operator optor,
            string modifier,
            ValueExpression operand)
        {
            switch (optor)
            {
                case Operator.EQ:
                    var typedOperand = operand.ToUnescapedString();
                    switch (modifier)
                    {
                        case Modifier.EXACT:
                            return Builders<BsonDocument>.Filter.Eq(parameterName, typedOperand);
                        case Modifier.CONTAINS:
                            return Builders<BsonDocument>.Filter.Regex(
                                parameterName,
                                new BsonRegularExpression(new Regex($".*{typedOperand}.*", RegexOptions.IgnoreCase)));
                        case Modifier.TEXT: //the same behaviour as :phonetic in previous versions.
                            return Builders<BsonDocument>.Filter.Regex(parameterName + "soundex", "^" + typedOperand);
                        //case Modifier.BELOW:
                        //    return Builders<BsonDocument>.Filter.Matches(parameterName, typedOperand + ".*")
                        case Modifier.NONE:
                        case null:
                            //partial from begin
                            return Builders<BsonDocument>.Filter.Regex(
                                parameterName,
                                new BsonRegularExpression("^" + typedOperand, "i"));
                        default:
                            throw new ArgumentException(
                                $"Invalid modifier {modifier} on string parameter {parameterName}");
                    }
                case Operator.IN: //We'll only handle choice like :exact
                    IEnumerable<ValueExpression> opMultiple = ((ChoiceValue) operand).Choices;
                    return SafeIn(parameterName, new BsonArray(opMultiple.Select(sv => sv.ToUnescapedString())));
                case Operator.ISNULL:
                    return Builders<BsonDocument>.Filter.Or(
                        Builders<BsonDocument>.Filter.Exists(parameterName, false),
                        Builders<BsonDocument>.Filter.Eq(
                            parameterName,
                            BsonNull
                                .Value)); //With only Builders<BsonDocument>.Filter.NotExists, that would exclude resources that have this field with an explicit null in it.
                case Operator.NOTNULL:
                    return
                        Builders<BsonDocument>.Filter.Ne(
                            parameterName,
                            BsonNull
                                .Value); //We don't use Builders<BsonDocument>.Filter.Exists, because that would include resources that have this field with an explicit null in it.
                default:
                    throw new ArgumentException($"Invalid operator {optor} on string parameter {parameterName}");
            }
        }

        //No modifiers allowed on number parameters, hence not in the method signature.
        private static FilterDefinition<BsonDocument> NumberQuery(
            string parameterName,
            Operator optor,
            ValueExpression operand)
        {
            string typedOperand;
            try
            {
                typedOperand = ((UntypedValue) operand).AsNumberValue().ToString();
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException($"Invalid number value {operand} on number parameter {parameterName}");
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Invalid number value {operand} on number parameter {parameterName}");
            }

            switch (optor)
            {
                case Operator.APPROX:
                //TODO
                case Operator.CHAIN:
                //Invalid in this context
                case Operator.EQ:
                    return Builders<BsonDocument>.Filter.Eq(parameterName, typedOperand);
                case Operator.GT:
                    return Builders<BsonDocument>.Filter.Gt(parameterName, typedOperand);
                case Operator.GTE:
                    return Builders<BsonDocument>.Filter.Gte(parameterName, typedOperand);
                case Operator.IN:
                    IEnumerable<ValueExpression> opMultiple = ((ChoiceValue) operand).Choices;
                    return SafeIn(parameterName, new BsonArray(opMultiple.Cast<NumberValue>().Select(nv => nv.Value)));
                case Operator.ISNULL:
                    return Builders<BsonDocument>.Filter.Eq(parameterName, BsonNull.Value);
                case Operator.LT:
                    return Builders<BsonDocument>.Filter.Lt(parameterName, typedOperand);
                case Operator.LTE:
                    return Builders<BsonDocument>.Filter.Lte(parameterName, typedOperand);
                case Operator.NOTNULL:
                    return Builders<BsonDocument>.Filter.Ne(parameterName, BsonNull.Value);
                default:
                    throw new ArgumentException($"Invalid operator {optor} on number parameter {parameterName}");
            }
        }

        private static FilterDefinition<BsonDocument> ExpressionQuery(string name, Operator optor, BsonValue value)
        {
            return optor switch
            {
                Operator.EQ => Builders<BsonDocument>.Filter.Eq(name, value),
                Operator.GT => Builders<BsonDocument>.Filter.Gt(name, value),
                Operator.GTE => Builders<BsonDocument>.Filter.Gte(name, value),
                Operator.ISNULL => Builders<BsonDocument>.Filter.Eq(name, BsonNull.Value),
                Operator.LT => Builders<BsonDocument>.Filter.Lt(name, value),
                Operator.LTE => Builders<BsonDocument>.Filter.Lte(name, value),
                Operator.NOTNULL => Builders<BsonDocument>.Filter.Ne(name, BsonNull.Value),
                _ => throw new ArgumentException($"Invalid operator {optor} on token parameter {name}")
            };
        }

        private static FilterDefinition<BsonDocument> QuantityQuery(
            string parameterName,
            Operator optor,
            ValueExpression operand)
        {
            //$elemMatch only works on array values. But the MongoIndexMapper only creates an array if there are multiple values for a given parameter.
            //So we also construct a query for when there is only one set of values in the searchIndex, hence there is no array.
            var quantity = operand.ToModelQuantity();
            var q = quantity.ToUnitsOfMeasureQuantity().Canonical();
            var decimals = q.SearchableString();
            BsonValue value = q.GetValueAsBson();

            var arrayQueries = new List<FilterDefinition<BsonDocument>>();
            var noArrayQueries = new List<FilterDefinition<BsonDocument>>
            {
                Builders<BsonDocument>.Filter.Not(Builders<BsonDocument>.Filter.Type(parameterName, BsonType.Array))
            };
            switch (optor)
            {
                case Operator.EQ:
                    arrayQueries.Add(
                        Builders<BsonDocument>.Filter.Regex("decimals", new BsonRegularExpression("^" + decimals)));
                    noArrayQueries.Add(
                        Builders<BsonDocument>.Filter.Regex(
                            parameterName + ".decimals",
                            new BsonRegularExpression("^" + decimals)));
                    break;

                default:
                    arrayQueries.Add(ExpressionQuery("value", optor, value));
                    noArrayQueries.Add(ExpressionQuery(parameterName + ".value", optor, value));
                    break;
            }

            if (quantity.System != null)
            {
                arrayQueries.Add(Builders<BsonDocument>.Filter.Eq("system", quantity.System));
                noArrayQueries.Add(Builders<BsonDocument>.Filter.Eq(parameterName + ".system", quantity.System));
            }

            arrayQueries.Add(Builders<BsonDocument>.Filter.Eq("unit", q.Metric.ToString()));
            noArrayQueries.Add(Builders<BsonDocument>.Filter.Eq(parameterName + ".unit", q.Metric.ToString()));

            var arrayQuery = Builders<BsonDocument>.Filter.ElemMatch(
                parameterName,
                Builders<BsonDocument>.Filter.And(arrayQueries));
            var noArrayQuery = Builders<BsonDocument>.Filter.And(noArrayQueries);

            var query = Builders<BsonDocument>.Filter.Or(arrayQuery, noArrayQuery);
            return query;
        }

        private static FilterDefinition<BsonDocument> TokenQuery(
            string parameterName,
            Operator optor,
            string modifier,
            ValueExpression operand)
        {
            //$elemMatch only works on array values. But the MongoIndexMapper only creates an array if there are multiple values for a given parameter.
            //So we also construct a query for when there is only one set of values in the searchIndex, hence there is no array.
            var systemfield = parameterName + ".system";
            var codefield = parameterName + ".code";
            var textfield = parameterName + ".text";

            switch (optor)
            {
                case Operator.EQ:
                    var typedEqOperand = ((UntypedValue) operand).AsTokenValue();
                    if (modifier == Modifier.TEXT)
                    {
                        return Builders<BsonDocument>.Filter.Regex(
                            textfield,
                            new BsonRegularExpression(typedEqOperand.Value, "i"));
                    }
                    else //Search on code and system
                    {
                        //Set up two variants of queries, for dealing with single token values in the index, and multiple (in an array).
                        var arrayQueries = new List<FilterDefinition<BsonDocument>>();
                        var noArrayQueries = new List<FilterDefinition<BsonDocument>>
                        {
                            Builders<BsonDocument>.Filter.Not(
                                Builders<BsonDocument>.Filter.Type(parameterName, BsonType.Array))
                        };
                        var plainStringQueries = new List<FilterDefinition<BsonDocument>>
                        {
                            Builders<BsonDocument>.Filter.Type(parameterName, BsonType.String)
                        };

                        if (modifier == Modifier.NOT) //NOT modifier only affects matching the code, not the system
                        {
                            noArrayQueries.Add(Builders<BsonDocument>.Filter.Exists(parameterName));
                            noArrayQueries.Add(Builders<BsonDocument>.Filter.Ne(codefield, typedEqOperand.Value));
                            arrayQueries.Add(Builders<BsonDocument>.Filter.Exists(parameterName));
                            arrayQueries.Add(Builders<BsonDocument>.Filter.Ne("code", typedEqOperand.Value));
                            plainStringQueries.Add(Builders<BsonDocument>.Filter.Exists(parameterName));
                            plainStringQueries.Add(
                                Builders<BsonDocument>.Filter.Ne(parameterName, typedEqOperand.Value));
                        }
                        else
                        {
                            noArrayQueries.Add(Builders<BsonDocument>.Filter.Eq(codefield, typedEqOperand.Value));
                            arrayQueries.Add(Builders<BsonDocument>.Filter.Eq("code", typedEqOperand.Value));
                            plainStringQueries.Add(
                                Builders<BsonDocument>.Filter.Eq(parameterName, typedEqOperand.Value));
                        }

                        //Handle the system part, if present.
                        if (!typedEqOperand.AnyNamespace)
                        {
                            if (string.IsNullOrWhiteSpace(typedEqOperand.Namespace))
                            {
                                arrayQueries.Add(Builders<BsonDocument>.Filter.Exists("system", false));
                                noArrayQueries.Add(Builders<BsonDocument>.Filter.Exists(systemfield, false));
                                plainStringQueries.Add(Builders<BsonDocument>.Filter.Exists("system", false));
                            }
                            else
                            {
                                arrayQueries.Add(Builders<BsonDocument>.Filter.Eq("system", typedEqOperand.Namespace));
                                noArrayQueries.Add(
                                    Builders<BsonDocument>.Filter.Eq(systemfield, typedEqOperand.Namespace));
                                plainStringQueries.Add(
                                    Builders<BsonDocument>.Filter.Eq("system", typedEqOperand.Namespace));
                            }
                        }

                        //Combine code and system
                        var arrayEqQuery = Builders<BsonDocument>.Filter.ElemMatch(
                            parameterName,
                            Builders<BsonDocument>.Filter.And(arrayQueries));
                        var noArrayEqQuery = Builders<BsonDocument>.Filter.And(noArrayQueries);
                        var plainStringQuery = Builders<BsonDocument>.Filter.And(plainStringQueries);
                        return Builders<BsonDocument>.Filter.Or(arrayEqQuery, noArrayEqQuery, plainStringQuery);
                    }
                case Operator.IN:
                    IEnumerable<ValueExpression> opMultiple = ((ChoiceValue) operand).Choices;
                    return Builders<BsonDocument>.Filter.Or(
                        opMultiple.Select(choice => TokenQuery(parameterName, Operator.EQ, modifier, choice)));
                case Operator.ISNULL:
                    return Builders<BsonDocument>.Filter.And(
                        Builders<BsonDocument>.Filter.Eq(parameterName, BsonNull.Value),
                        Builders<BsonDocument>.Filter.Eq(
                            textfield,
                            BsonNull
                                .Value)); //We don't use Builders<BsonDocument>.Filter.NotExists, because that would exclude resources that have this field with an explicit null in it.
                case Operator.NOTNULL:
                    return Builders<BsonDocument>.Filter.Or(
                        Builders<BsonDocument>.Filter.Ne(parameterName, BsonNull.Value),
                        Builders<BsonDocument>.Filter.Eq(
                            textfield,
                            BsonNull
                                .Value)); //We don't use Builders<BsonDocument>.Filter.Exists, because that would include resources that have this field with an explicit null in it.
                default:
                    throw new ArgumentException($"Invalid operator {optor} on token parameter {parameterName}");
            }
        }

        private static FilterDefinition<BsonDocument> UriQuery(
            string parameterName,
            Operator optor,
            string modifier,
            ValueExpression operand)
        {
            //CK: Ugly implementation by just using existing features on the StringQuery.
            //TODO: Implement :ABOVE.
            var localModifier = "";
            switch (modifier)
            {
                case Modifier.BELOW:
                    //Without a modifier the default string search is left partial, which is what we need for Uri:below :-)
                    break;
                case Modifier.ABOVE:
                    //Not supported by string search, still TODO.
                    throw new NotImplementedException(
                        $"Modifier {modifier} on Uri parameter {parameterName} not supported yet.");
                case Modifier.NONE:
                case null:
                    localModifier = Modifier.EXACT;
                    break;
                case Modifier.MISSING:
                    localModifier = Modifier.MISSING;
                    break;
                default:
                    throw new ArgumentException($"Invalid modifier {modifier} on Uri parameter {parameterName}");
            }

            return StringQuery(parameterName, optor, localModifier, operand);
        }

        //private static string GroomDate(string value)
        //{
        //    if (value != null)
        //    {
        //        var s = Regex.Replace(value, @"[T\s:\-]", "");
        //        var i = s.IndexOf('+');
        //        if (i > 0) s = s.Remove(i);
        //        return s;
        //    }
        //    else
        //        return null;
        //}

        private static FilterDefinition<BsonDocument> DateQuery(
            string parameterName,
            Operator optor,
            string modifier,
            ValueExpression operand)
        {
            if (optor == Operator.IN)
            {
                IEnumerable<ValueExpression> opMultiple = ((ChoiceValue) operand).Choices;
                return Builders<BsonDocument>.Filter.Or(
                    opMultiple.Select(choice => DateQuery(parameterName, Operator.EQ, modifier, choice)));
            }

            var start = parameterName + ".start";
            var end = parameterName + ".end";

            var fdtValue = ((UntypedValue) operand).AsDateTimeValue();
            var valueLower = BsonDateTime.Create(fdtValue.LowerBound());
            var valueUpper = BsonDateTime.Create(fdtValue.UpperBound());

            return optor switch
            {
                Operator.EQ => Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Gte(end, valueLower),
                    Builders<BsonDocument>.Filter.Lt(start, valueUpper)),
                Operator.GT => Builders<BsonDocument>.Filter.Gte(start, valueUpper),
                Operator.GTE => Builders<BsonDocument>.Filter.Gte(start, valueLower),
                Operator.LT => Builders<BsonDocument>.Filter.Lt(end, valueLower),
                Operator.LTE => Builders<BsonDocument>.Filter.Lt(end, valueUpper),
                Operator.ISNULL =>
                    Builders<BsonDocument>.Filter.Eq(
                        parameterName,
                        BsonNull.Value) //We don't use Builders<BsonDocument>.Filter.NotExists, because that would exclude resources that have this field with an explicit null in it.
                ,
                Operator.NOTNULL =>
                    Builders<BsonDocument>.Filter.Ne(
                        parameterName,
                        BsonNull.Value) //We don't use Builders<BsonDocument>.Filter.Exists, because that would include resources that have this field with an explicit null in it.
                ,
                _ => throw new ArgumentException($"Invalid operator {optor} on date parameter {parameterName}")
            };
        }

        private static FilterDefinition<BsonDocument> CompositeQuery(
            ModelInfo.SearchParamDefinition parameterDef,
            Operator optor,
            string modifier,
            ValueExpression operand)
        {
            if (optor == Operator.IN)
            {
                var choices = (ChoiceValue) operand;
                var queries = new List<FilterDefinition<BsonDocument>>();
                foreach (var choice in choices.Choices)
                {
                    queries.Add(CompositeQuery(parameterDef, Operator.EQ, modifier, choice));
                }

                return Builders<BsonDocument>.Filter.Or(queries);
            }

            if (optor == Operator.EQ)
            {
                var typedOperand = (CompositeValue) operand;
                var queries = new List<FilterDefinition<BsonDocument>>();
                var components = typedOperand.Components;
                var subParams = parameterDef.CompositeParams;

                if (components.Length != subParams.Length)
                {
                    throw new ArgumentException(
                        $"Parameter {parameterDef.Name} requires exactly {subParams.Length} composite values, not the currently provided {components.Length} values.");
                }

                for (var i = 0; i < subParams.Length; i++)
                {
                    var subCrit = new Criterium
                    {
                        Operator = Operator.EQ,
                        ParamName = subParams[i],
                        Operand = components[i],
                        Modifier = modifier
                    };
                    queries.Add(subCrit.ToFilter(parameterDef.Resource));
                }

                return Builders<BsonDocument>.Filter.And(queries);
            }

            throw new ArgumentException($"Invalid operator {optor} on composite parameter {parameterDef.Name}");
        }

        //internal static FilterDefinition<BsonDocument> _lastUpdatedFixedQuery(Criterium crit)
        //{
        //    if (crit.Operator == Operator.IN)
        //    {
        //        IEnumerable<ValueExpression> opMultiple = ((ChoiceValue)crit.Operand).Choices;
        //        IEnumerable<Criterium> criteria = opMultiple.Select<ValueExpression, Criterium>(choice => new Criterium() { ParamName = crit.ParamName, Modifier = crit.Modifier, Operator = Operator.EQ, Operand = choice });
        //        return Builders<BsonDocument>.Filter.Or(criteria.Select(criterium => _lastUpdatedFixedQuery(criterium)));
        //    }

        //    var typedOperand = ((UntypedValue)crit.Operand).AsDateTimeValue();

        //    DateTimeOffset searchPeriodStart = typedOperand.ToDateTimeOffset();
        //    DateTimeOffset searchPeriodEnd = typedOperand.ToDateTimeOffset();

        //    return DateQuery(InternalField.LASTUPDATED, crit.Operator, crit.Modifier, (ValueExpression)crit.Operand);
        //}

        //internal static FilterDefinition<BsonDocument> _tagFixedQuery(Criterium crit)
        //{
        //    return TagQuery(crit, new Uri(XmlNs.FHIRTAG, UriKind.Absolute));
        //}

        //internal static FilterDefinition<BsonDocument> _profileFixedQuery(Criterium crit)
        //{
        //    return TagQuery(crit, new Uri(XmlNs.TAG_PROFILE, UriKind.Absolute));
        //}

        //internal static FilterDefinition<BsonDocument> _securityFixedQuery(Criterium crit)
        //{
        //    return TagQuery(crit, new Uri(XmlNs.TAG_SECURITY, UriKind.Absolute));
        //}

        //private static FilterDefinition<BsonDocument> TagQuery(Criterium crit, Uri tagscheme)
        //{
        //    if (crit.Operator == Operator.IN)
        //    {
        //        IEnumerable<ValueExpression> opMultiple = ((ChoiceValue)crit.Operand).Choices;
        //        var optionQueries = new List<FilterDefinition<BsonDocument>>();
        //        foreach (var choice in opMultiple)
        //        {
        //            Criterium option = new Criterium();
        //            option.Operator = Operator.EQ;
        //            option.Operand = choice;
        //            option.Modifier = crit.Modifier;
        //            option.ParamName = crit.ParamName;
        //            optionQueries.Add(TagQuery(option, tagscheme));
        //        }
        //        return Builders<BsonDocument>.Filter.Or(optionQueries);
        //    }

        //    //From here there's only 1 operand.
        //    FilterDefinition<BsonDocument> schemeQuery = Builders<BsonDocument>.Filter.Eq(InternalField.TAGSCHEME, tagscheme.AbsoluteUri);
        //    FilterDefinition<BsonDocument> argQuery;

        //    var operand = (ValueExpression)crit.Operand;
        //    switch (crit.Modifier)
        //    {
        //        case Modifier.PARTIAL:
        //            argQuery = StringQuery(InternalField.TAGTERM, Operator.EQ, Modifier.NONE, operand);
        //            break;
        //        case Modifier.TEXT:
        //            argQuery = StringQuery(InternalField.TAGLABEL, Operator.EQ, Modifier.NONE, operand);
        //            break;
        //        case Modifier.NONE:
        //        case null:
        //            argQuery = StringQuery(InternalField.TAGTERM, Operator.EQ, Modifier.EXACT, operand);
        //            break;
        //        default:
        //            throw new ArgumentException(String.Format("Invalid modifier {0} in parameter {1}", crit.Modifier, crit.ParamName));
        //    }

        //    return Builders<BsonDocument>.Filter.ElemMatch(InternalField.TAG, Builders<BsonDocument>.Filter.And(schemeQuery, argQuery));
        //}

        internal static FilterDefinition<BsonDocument> internal_justidFixedQuery(Criterium crit) =>
            StringQuery(InternalField.JUSTID, crit.Operator, "exact", (ValueExpression) crit.Operand);

        //internal static FilterDefinition<BsonDocument> _idFixedQuery(Criterium crit)
        //{
        //    return StringQuery(InternalField.JUSTID, crit.Operator, "exact", (ValueExpression)crit.Operand);
        //}

        internal static FilterDefinition<BsonDocument> internal_idFixedQuery(Criterium crit) =>
            StringQuery(InternalField.ID, crit.Operator, "exact", (ValueExpression) crit.Operand);

        private static FilterDefinition<BsonDocument> FalseQuery()
        {
            return Builders<BsonDocument>.Filter.Where(w => false);
        }

        private static FilterDefinition<BsonDocument> SafeIn(string parameterName, BsonArray values) =>
            values.Any() ? Builders<BsonDocument>.Filter.In(parameterName, values) : FalseQuery();
    }
}