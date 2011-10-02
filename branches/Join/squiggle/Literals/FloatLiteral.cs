using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Literals
{
    /// <remarks>
    /// Author: Nat Pryce
    /// </remarks>
    public class FloatLiteral : LiteralWithSameRepresentationInJavaAndSql
    {
        public FloatLiteral(double literalValue)
            : base(literalValue)
        {
        }
    }
}
