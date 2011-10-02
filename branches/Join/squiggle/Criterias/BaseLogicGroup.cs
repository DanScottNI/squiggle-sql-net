using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle.Criterias
{
    public abstract class BaseLogicGroup : Criteria
    {
        private String op;
        private Criteria left;
        private Criteria right;

        public BaseLogicGroup(String op, Criteria left, Criteria right)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }

        public override void write(Output output)
        {
            output.print("( ");
            left.write(output);
            output.print(' ')
               .print(op)
               .print(' ');
            right.write(output);
            output.print(" )");
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
            left.addReferencedTablesTo(tables);
            right.addReferencedTablesTo(tables);
        }
    }
}
