using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle.Criterias
{
    public class InCriteria : Criteria
    {
        private Matchable matched;
        private ValueSet valueSet;

        public InCriteria(Matchable matchable, ValueSet valueSet)
        {
            this.matched = matchable;
            this.valueSet = valueSet;
        }

        public InCriteria(Matchable column, params String[] values)
        {
            this.matched = column;
            this.valueSet = new LiteralValueSet(values);
        }

        public InCriteria(Matchable column, params int[] values)
        {
            this.matched = column;
            this.valueSet = new LiteralValueSet(values);
        }

        public InCriteria(Matchable column, params double[] values)
        {
            this.matched = column;
            this.valueSet = new LiteralValueSet(values);
        }

        public InCriteria(Table table, String columnname, ValueSet valueSet)
            :
                this(table.getColumn(columnname), valueSet)
        {
        }

        public InCriteria(Table table, String columnname, String[] values)
            :
               this(table.getColumn(columnname), values)
        {
        }

        public InCriteria(Table table, String columnname, double[] values)
            :
                this(table.getColumn(columnname), values)
        {
        }

        public InCriteria(Table table, String columnname, int[] values)
            :
                this(table.getColumn(columnname), values)
        {
        }

        public Matchable getMatched()
        {
            return matched;
        }

        public override void write(Output output)
        {
            matched.write(output);
            output.println(" IN (");
            output.indent();
            valueSet.write(output);
            output.println();
            output.unindent();
            output.print(")");
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
            matched.addReferencedTablesTo(tables);
        }
    }

}
