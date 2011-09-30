using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using squiggle.Literals;
using squiggle.Outputs;

namespace squiggle
{
    public class LiteralValueSet : ValueSet
    {
        private List<Literal> literals;

        public LiteralValueSet(List<Literal> literals)
        {
            this.literals = literals;
        }

        public LiteralValueSet(params String[] values)
        {
            this.literals = new List<Literal>(values.Length);
            foreach (var value in values) literals.Add(new StringLiteral(value));
        }

        public LiteralValueSet(params int[] values)
        {
            this.literals = new List<Literal>(values.Length);
            foreach (var value in values) literals.Add(new IntegerLiteral(value));
        }

        public LiteralValueSet(params double[] values)
        {
            this.literals = new List<Literal>(values.Length);
            foreach (var value in values) literals.Add(new FloatLiteral(value));
        }

        public LiteralValueSet(params Decimal[] values)
        {
            this.literals = new List<Literal>(values.Length);
            foreach (var value in values) literals.Add(new BigDecimalLiteral(value));
        }

        public LiteralValueSet(params DateTime[] values)
        {
            this.literals = new List<Literal>(values.Length);
            foreach (var value in values) literals.Add(new DateTimeLiteral(value));
        }

        public void write(Output output)
        {
            for (int i = 0; i < literals.Count; i++)
            {
                Literal literal = literals[i];

                literal.write(output);

                if (i < literals.Count - 1)
                {
                    output.print(", ");
                }
            }

            //for (Iterator<Literal> it = literals.iterator(); it.hasNext();) {
            //    Literal literal = it.next();
            //    literal.write(output);
            //    if (it.hasNext()) {
            //        output.print(", ");
            //    }
            //}
        }
    }

}
