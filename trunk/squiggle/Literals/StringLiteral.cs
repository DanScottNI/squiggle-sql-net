using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle.Literals
{
    public class StringLiteral : Literal
    {
        private String literalValue;

        public StringLiteral(String literalValue)
        {
            this.literalValue = literalValue;
        }

        public override void write(Output output)
        {
            output.print(quote(literalValue));
        }

        protected String quote(String s)
        {
            if (s == null) return "null";

            StringBuilder str = new StringBuilder();
            str.Append('\'');
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\'
                        || s[i] == '\"'
                        || s[i] == '\'')
                {
                    str.Append('\\');
                }
                str.Append(s[i]);
            }
            str.Append('\'');
            return str.ToString();
        }
    }

}
