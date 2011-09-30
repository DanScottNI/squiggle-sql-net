using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Literals
{
    /// <remarks>
    /// Author: Nat Pryce
    /// </remarks>
    public class DateTimeLiteral : StringLiteral
    {
        private static String FORMAT = "yyyy-MM-dd HH:mm:ss.S";

        public DateTimeLiteral(DateTime literalValue)
            : base(literalValue.ToString(FORMAT))
        {
        }
    }
}
