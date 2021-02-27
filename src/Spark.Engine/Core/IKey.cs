// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Core
{
    public interface IKey
    {
        string Base { get; set; }
        string TypeName { get; set; }
        string ResourceId { get; set; }
        string VersionId { get; set; }
    }
}