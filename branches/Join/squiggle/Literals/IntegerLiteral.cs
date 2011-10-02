using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Literals
{
    /// <remarks>
    /// Author: Nat Pryce
    /// </remarks>
    public class IntegerLiteral : LiteralWithSameRepresentationInJavaAndSql
    {
        public IntegerLiteral(int literalValue)
            : base(literalValue)
        {

        }
    }

}
