﻿// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Search.Common
{
    public static class UniversalField
    {
        public const string ID = "_id", TAG = "_tag";

        public static string[] All = {ID, TAG};
    }
}