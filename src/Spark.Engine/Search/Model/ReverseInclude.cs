﻿using System;
using System.Text.RegularExpressions;

namespace Spark.Engine.Search.Model
{
    public class ReverseInclude
    {
        private static readonly Regex _pattern = new Regex(@"(?<resourcetype>[^\.]+)\.(?<searchpath>.*)");

        public string ResourceType { get; set; }
        public string SearchPath { get; set; }

        /// <summary>
        /// Expected format: ResourceType.searchParameter[.searchParameter]*
        /// </summary>
        /// <param name="reverseInclude"></param>
        /// <returns>ReverseInclude instance with ResourceType is everything before the first dot, and SearchPath everything after it.</returns>
        public static ReverseInclude Parse(string reverseInclude)
        {
            //_revinclude should have the following format: ResourceType.searchParameter[.searchParameter]*
            //so we simply split in on the first dot.
            if (reverseInclude == null)
            {
                throw new ArgumentNullException(nameof(reverseInclude), "reverseInclude cannot be null");
            }
            var result = new ReverseInclude();
            var match = _pattern.Match(reverseInclude);
            if (match.Groups.Count < 2)
            {
                throw new ArgumentException(
                    $"reverseInclude '{reverseInclude}' does not adhere to the format 'ResourceType.searchParameter[.searchParameter]*'");
            }

            result.ResourceType = match.Groups["resourcetype"].Captures[0].Value;
            result.SearchPath = match.Groups["searchpath"].Captures[0].Value;

            return result;
        }
    }
}
