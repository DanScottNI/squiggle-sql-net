using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    public class FunctionCall : Matchable, Selectable
    {
        private String functionName;
        private Matchable[] arguments;
        private string columnName;
        public FunctionCall(String functionName, params Matchable[] arguments)
        {
            this.functionName = functionName;
            this.arguments = arguments;
        }

        public FunctionCall(String functionName, string columnName, params Matchable[] arguments)
            : this(functionName, arguments)
        {
            this.columnName = columnName;
        }

        public void write(Output output)
        {
            output.print(functionName).print("(");

            if (arguments != null)
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    if (i > 0) output.print(", ");
                    arguments[i].write(output);
                }
            }

            output.print(")");

            if (!string.IsNullOrWhiteSpace(columnName))
            {
                output.print(" AS " + columnName);
            }
        }

        public void addReferencedTablesTo(HashSet<Table> tables)
        {
            foreach (var argument in arguments)
            {
                argument.addReferencedTablesTo(tables);
            }
        }
    }

}
