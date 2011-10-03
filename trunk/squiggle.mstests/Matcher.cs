using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace squiggle.mstests
{
    class Matcher
    {
        protected static readonly Regex normalizeSpace = new Regex(@"\s+", RegexOptions.Compiled);

        public static string Normalize(string s1)
        {
            return normalizeSpace.Replace(s1, " ").ToLower().Trim();
        }
    }
}
