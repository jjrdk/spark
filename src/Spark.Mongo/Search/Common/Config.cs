// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Mongo.Search.Common
{
    using System;

    public static class Config
    {
        public const string PARAM_TRUE = "true", PARAM_FALSE = "false";

        public const int PARAM_NOLIMIT = -1;

        public static int MaxSearchResults = 5000;

        public static string LuceneIndexPath = @"C:\Index", Mongoindexcollection = "searchindex";

        public static bool Equal(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }
}