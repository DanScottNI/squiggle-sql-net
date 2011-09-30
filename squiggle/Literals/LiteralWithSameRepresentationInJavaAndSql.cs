using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle.Literals
{
    /// <remarks>
    /// Author: Nat Pryce
    /// </remarks>
    public abstract class LiteralWithSameRepresentationInJavaAndSql : Literal
    {
        private object literalValue;

        protected LiteralWithSameRepresentationInJavaAndSql(object literalValue)
        {
            this.literalValue = literalValue;
        }

        public override void write(Output output)
        {
            output.print(literalValue);
        }
    }
}