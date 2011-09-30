using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Literals
{
    /// <remarks>
    /// Author: Nat Pryce
    /// </remarks>
    public class BooleanLiteral : LiteralWithSameRepresentationInJavaAndSql
    {
        public static BooleanLiteral TRUE = new BooleanLiteral(true);
        public static BooleanLiteral FALSE = new BooleanLiteral(false);

        public BooleanLiteral(Boolean literalValue)
            : base(literalValue)
        {

        }
    }
}