﻿using Hl7.Fhir.Model;
using System.Collections.Generic;

namespace Spark.Engine.Model
{
    /// <summary>
    /// Class for holding information as present in a CompartmentDefinition resource.
    /// This is a (hopefully) temporary solution, since the Hl7.Fhir api does not containt CompartmentDefinition yet.
    /// </summary>
    public class CompartmentInfo
    {
        public ResourceType ResourceType { get; set; }

        private readonly List<string> _revIncludes = new List<string>();
        public List<string> ReverseIncludes => _revIncludes;

        public CompartmentInfo(ResourceType resourceType)
        {
            this.ResourceType = resourceType;
        }

        public void AddReverseInclude(string revInclude)
        {
            _revIncludes.Add(revInclude);
        }

        public void AddReverseIncludes(IEnumerable<string> revIncludes)
        {
            this._revIncludes.AddRange(revIncludes);
        }
    }
}
