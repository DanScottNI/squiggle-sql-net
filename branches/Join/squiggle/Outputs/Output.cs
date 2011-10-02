using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Outputs
{
    /// <summary>
    /// The Output is where the elements of the query output their bits of SQL to.
    /// </summary>
    public class Output
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indent">String to be used for indenting (e.g. "", "  ", "       ", "\t")</param>
        public Output(String indent)
        {
            this.indentText = indent;
        }

        private StringBuilder result = new StringBuilder();
        private StringBuilder currentIndent = new StringBuilder();
        private bool newLineComing;

        private String indentText;

        public override String ToString()
        {
            return result.ToString();
        }

        public Output print(Object o)
        {
            writeNewLineIfNeeded();
            result.Append(o);
            return this;
        }

        public Output print(char c)
        {
            writeNewLineIfNeeded();
            result.Append(c);
            return this;
        }

        public Output println(Object o)
        {
            writeNewLineIfNeeded();
            result.Append(o);
            newLineComing = true;
            return this;
        }

        public Output println()
        {
            newLineComing = true;
            return this;
        }

        public void indent()
        {
            currentIndent.Append(indentText);
        }

        public void unindent()
        {
            currentIndent.Length = (currentIndent.Length - indentText.Length);
        }

        private void writeNewLineIfNeeded()
        {
            if (newLineComing)
            {
                result.Append(Environment.NewLine).Append(currentIndent);
                newLineComing = false;
            }
        }
    }

}
