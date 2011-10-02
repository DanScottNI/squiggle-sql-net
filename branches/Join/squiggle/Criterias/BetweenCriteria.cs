using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;
using squiggle.Literals;

namespace squiggle.Criterias
{
    ///<summary>Class BetweenCriteria is a Criteria extension that 
    ///generates the SQL syntax for a BETWEEN operator in an SQL 
    ///Where clause.
    ///</summary>
    /// <remarks>
    /// Author: Nat Pryce, Derek Mahar
    /// </remarks>
    public class BetweenCriteria : Criteria
    {
        private Matchable column;
        private Matchable lower, upper;

        /**
         * Initializes a new BetweenCriteria with an operand and the upper
         * and lower bounds of the SQL BETWEEN operator.
         *
         * @param operand
         *            the first operand to the SQL BETWEEN operator that the
         *            operator uses to test whether the column falls within the
         *            given range. The SQL type of the column must be DECIMAL or
         *            NUMERIC.
         * @param lower
         *            the lower bound of the BETWEEN operator
         * @param upper
         *            the upper bound of the BETWEEN operator
         */
        public BetweenCriteria(Matchable operand, Matchable lower, Matchable upper)
        {
            this.column = operand;
            this.lower = lower;
            this.upper = upper;
        }

        public BetweenCriteria(Matchable operand, decimal lower, decimal upper)
            : this(operand, new BigDecimalLiteral(lower), new BigDecimalLiteral(upper))
        {

        }

        public BetweenCriteria(Matchable column, DateTime lower, DateTime upper)
            : this(column, new DateTimeLiteral(lower), new DateTimeLiteral(upper))
        {

        }

        public BetweenCriteria(Matchable column, double lower, double upper) :
            this(column, new FloatLiteral(lower), new FloatLiteral(upper))
        {
        }

        public BetweenCriteria(Matchable column, int lower, int upper)
            : this(column, new IntegerLiteral(lower), new IntegerLiteral(upper))
        {
        }

        public BetweenCriteria(Matchable column, String lower, String upper)
            : this(column, new StringLiteral(lower), new StringLiteral(upper))
        {
        }

        public override void write(Output output)
        {
            column.write(output);
            output.print(" BETWEEN ");
            lower.write(output);
            output.print(" AND ");
            upper.write(output);
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
            column.addReferencedTablesTo(tables);
        }
    }
}