﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spark.Engine.Extensions
{
    public static class RegexExtensions
    {

        public static string ReplaceGroup(this Regex regex, string input, string groupName, string replacement)
        {
            return ReplaceGroups(regex, input, new Dictionary<string, string> { { groupName, replacement } });
        }

        public static string ReplaceGroups(this Regex regex, string input, Dictionary<string, string> replacements)
        {
            return regex.Replace(input, m => ReplaceNamedGroups(m, replacements));
        }

        private static string ReplaceNamedGroups(Match m, Dictionary<string, string> replacements)
        {
            var result = m.Value;
            foreach (var (groupName, replaceWith) in replacements)
            {
                foreach (Capture cap in m.Groups[groupName]?.Captures)
                {
                    result = result.Remove(cap.Index - m.Index, cap.Length);
                    result = result.Insert(cap.Index - m.Index, replaceWith);
                }
            }
            return result;
        }
    }
}
