﻿/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/ewoutkramer/fhir-net-api/master/LICENSE
 */

namespace Spark.Engine.Search.ValueExpressionTypes
{
    using System;
    using Support;

    public class TokenValue : ValueExpression
    {
        public string Namespace { get; private set; }

        public string Value { get; private set; }

        public bool AnyNamespace { get; private set; }

        public TokenValue(string value, bool matchAnyNamespace)
        {
            Value = value;
            AnyNamespace = matchAnyNamespace;
        }

        public TokenValue(string value, string ns)
        {
            Value = value;
            AnyNamespace = false;
            Namespace = ns;
        }

        public override string ToString()
        {
            if (!AnyNamespace)
            {
                var ns = Namespace ?? string.Empty;
                return StringValue.EscapeString(ns) + "|" +
                                    StringValue.EscapeString(Value);
            }
            else
            {
                return StringValue.EscapeString(Value);
            }
        }

        public static TokenValue Parse(string text)
        {
            if (text == null)
            {
                throw Error.ArgumentNull("text");
            }

            var pair = text.SplitNotEscaped('|');

            if (pair.Length > 2)
            {
                throw Error.Argument("text", "Token cannot have more than two parts separated by '|'");
            }

            var hasNamespace = pair.Length == 2;

            var pair0 = StringValue.UnescapeString(pair[0]);

            if (hasNamespace)
            {
                if(pair[1] == string.Empty)
                {
                    throw new FormatException("Token query parameters should at least specify a value after the '|'");
                }

                var pair1 = StringValue.UnescapeString(pair[1]);

                return pair0 == string.Empty ? new TokenValue(pair1, matchAnyNamespace: false ) : new TokenValue(pair1, pair0);
            }
            else
            {
                return new TokenValue(pair0, matchAnyNamespace: true);
            }            
        }     
    }



}