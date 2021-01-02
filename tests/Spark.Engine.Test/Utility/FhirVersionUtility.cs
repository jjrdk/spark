﻿using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using System.Collections.Generic;

namespace Spark.Engine.Test.Utility
{
    using System;

    internal class FhirVersionUtility
    {
        public const string VERSION_R2 = "1.0";
        public const string VERSION_R3 = "3.0";
        public const string VERSION_R4 = "4.0";
        public const string VERSION_R5 = "4.4";

        public static Dictionary<FhirVersionMoniker, string> KnownFhirVersions = new Dictionary<FhirVersionMoniker, string>
        {
            { FhirVersionMoniker.None, string.Empty },
            { FhirVersionMoniker.R2, VERSION_R2 },
            { FhirVersionMoniker.R3, VERSION_R3 },
            { FhirVersionMoniker.R4, VERSION_R4 },
            { FhirVersionMoniker.R5, VERSION_R5 },
        };

        public static FhirVersionMoniker GetFhirVersionMoniker()
        {
            FhirVersionMoniker? fhirVersion = default;
            if (Version.TryParse(ModelInfo.Version, out var semanticVersion))
            {
                fhirVersion = EnumUtility.ParseLiteral<FhirVersionMoniker>($"{semanticVersion.Major}.{semanticVersion.Minor}");
            }

            return fhirVersion ?? FhirVersionMoniker.None;
        }
    }
}
