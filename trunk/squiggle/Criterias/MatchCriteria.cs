using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Literals;
using squiggle.Outputs;

namespace squiggle.Criterias
{
    public class MatchCriteria : Criteria
    {
        public static String EQUALS = "=";
        public static String GREATER = ">";
        public static String GREATEREQUAL = ">=";
        public static String LESS = "<";
        public static String LESSEQUAL = "<=";
        public static String LIKE = "LIKE";
        public static String NOTEQUAL = "<>";

        protected Matchable left;
        protected String op;
        protected Matchable right;

        public MatchCriteria(Matchable left, String op, Matchable right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public MatchCriteria(Column column, String matchType, bool value)
            : this(column, matchType, new BooleanLiteral(value))
        {
        }

        /**
         * Initializes a MatchCriteria with a given column, comparison operator, and
         * date operand that the criteria will use to make a comparison between the
         * given column and the date.
         *
         * @param column   the column to use in the date comparison.
         * @param operator the comparison operator to use in the date comparison.
         * @param operand  the date literal to use in the comparison.
         */
        public MatchCriteria(Column column, String op, DateTime operand)
            : this(column, op, operand.ToString("yyyy-MM-dd HH:mm:ss.S"))
        {

        }

        public MatchCriteria(Column column, String matchType, double value)
            : this(column, matchType, new FloatLiteral(value))
        {
        }

        public MatchCriteria(Column column, String matchType, int value)
            : this(column, matchType, new IntegerLiteral(value))
        {

        }

        public MatchCriteria(Column column, String matchType, String value) :
            this(column, matchType, new StringLiteral(value))
        {
        }

        public MatchCriteria(Table table, String columnname, String matchType, bool value) :
            this(table.getColumn(columnname), matchType, value)
        {
        }

        /**
         * Initializes a MatchCriteria with a table, column name is this table,
         * comparison operator, and date operand that the criteria will use to make a
         * comparison between the given table column and the date.
         *
         * @param table      the table that contains a column having the given name to use in
         *                   the date comparison.
         * @param columnName the name of the column to use in the date comparison.
         * @param operator   the comparison operator to use in the date comparison.
         * @param operand    the date literal to use in the comparison.
         */
        public MatchCriteria(Table table, String columnName, String op, DateTime operand)
            : this(table.getColumn(columnName), op, operand)
        {

        }

        public MatchCriteria(Table table, String columnname, String matchType, double value) :
            this(table.getColumn(columnname), matchType, value)
        {
        }

        public MatchCriteria(Table table, String columnname, String matchType, long value) :
            this(table.getColumn(columnname), matchType, value)
        {
        }

        public MatchCriteria(Table table, String columnname, String matchType, String value) :
            this(table.getColumn(columnname), matchType, value)
        {
        }

        public Matchable getLeft()
        {
            return left;
        }

        public String getComparisonOperator()
        {
            return op;
        }

        public Matchable getRight()
        {
            return right;
        }

        public override void write(Output output)
        {
            left.write(output);
            output.print(' ').print(op).print(' ');
            right.write(output);
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
            left.addReferencedTablesTo(tables);
            right.addReferencedTablesTo(tables);
        }
    }
}
