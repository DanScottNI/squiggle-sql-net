using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace squiggle.tests
{
    public class BaseTest
    {
        protected static readonly Regex normalizeSpace = new Regex(@"\s+", RegexOptions.Compiled);
    }
}
