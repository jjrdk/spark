/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System.Collections.Generic;

namespace Spark.Mongo.Search.Common
{
    /*
    Ik heb deze class losgetrokken van SearchParamDefinition,
    omdat Definition onafhankelijk van Spark zou moeten kunnen bestaan.
    Er komt dus een converter voor in de plaats. -mh
    */

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