using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Literals
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Author: Nat Pryce
    /// </remarks>
    public class BigDecimalLiteral : LiteralWithSameRepresentationInJavaAndSql
    {
        public BigDecimalLiteral(Decimal literalValue)
            : base(literalValue)
        {
        }
    }

}
