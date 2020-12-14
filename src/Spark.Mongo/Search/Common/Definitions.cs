/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Spark.Engine.Core;

namespace Spark.Mongo.Search.Common
{
    /*
    Ik heb deze class losgetrokken van SearchParamDefinition,
    omdat Definition onafhankelijk van Spark zou moeten kunnen bestaan.
    Er komt dus een converter voor in de plaats. -mh
    */

    public class Definition
    {
        public Argument Argument { get; set; }
        public string Resource { get; set; }
        public string ParamName { get; set; }
        public string Description { get; set; }
        public SearchParamType ParamType { get; set; }
        public ElementQuery Query { get; set; }

        public override string ToString()
        {
            return $"{Resource.ToLower()}.{ParamName.ToLower()}->{Query}";
        }
    }

    public class Definitions
    {
        private readonly List<Definition> definitions = new List<Definition>();

        public void Add(Definition definition)
        {
            this.definitions.Add(definition);
        }
        public void Replace(Definition definition)
        {
            definitions.RemoveAll(d => (d.Resource == definition.Resource) && (d.ParamName == definition.ParamName));
            definitions.Add(definition);
            // for manual correction
        }
    }
}