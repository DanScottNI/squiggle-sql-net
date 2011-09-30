using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle.Criterias
{
    public static class SQLOperator
    {
        /**
         * Identifier for the AND operator.
         */
        public static int AND = 0;

        /**
         * Identifier for the OR operator.
         */
        public static int OR = 1;
    }

    /**
 * Class CriteriaExpression is Criteria class extension that generates the SQL
 * syntax for a compound logic expression in an SQL Where clause. The logic
 * expression that the class generates is a list of criteria terms each
 * separated by an {@link Operator#AND AND} or {@link Operator#OR OR} operator.
 * Formally, we may express a logic expression in Backus-Naur Form (BNF) as
 * <p>
 * <code>
 * &lt;expression&gt; ::= &lt;term&gt; | &lt;term&gt; &lt;operator &lt;expression&gt;
 * <br/>&lt;term&gt; ::= &lt;criteria&gt;
 * <br/>&lt;operator&gt; ::= AND | OR
 * </code>
 * </p>
 * 
 * @author <a href="mailto:derek@derekmahar.ca">Derek Mahar</a>
 */
    public class CriteriaExpression : Criteria
    {
        /**
         * Operator identifiers
         */


        // Recursive reference to another expression.
        private CriteriaExpression expression;

        // Operator that joins the initial criteria term with the trailing criteria
        // expression.
        private int op;

        // Initial criteria term in the expression.
        private Criteria term;

        /**
         * Initializes a CriteriaExpression with a single criteria term, leaving the
         * trailing criteria expression set to null. This constructor definition
         * represents the rule
         * 
         * &lt;expression&gt; ::= &lt;term&gt;
         * 
         * in the formal criteria expression syntax.
         * 
         * @param term
         *            single criteria to assign to this criteria expression.
         */
        public CriteriaExpression(Criteria term)
        {
            this.term = term;
        }

        /**
         * Initializes a CriteriaExpression with an initial criteria term, an
         * operator, and a another trailing criteria expression. Note that this
         * constructor definition is recursive and represents the rule
         * 
         * &lt;expression&gt; ::= &lt;term&gt; &lt;operator &lt;expression&gt;
         * 
         * in the formal criteria expression syntax.
         * 
         * @param term
         *            the starting criteria to assign to the new criteria
         *            expression.
         * 
         * @param operator
         *            the infix operator, either {@link Operator#AND AND} or
         *            {@link Operator#OR OR}, that joins the initial criteria with
         *            the trailing criteria expression.
         * 
         * @param expression
         *            the trailing expression to assign to the new criteria
         *            expression.
         */
        public CriteriaExpression(Criteria term, int op,
                CriteriaExpression expression)
            : this(term)
        {
            this.op = op;
            this.expression = expression;
        }

        /**
         * Recursively generates a CriteriaExpression from a given list of criteria
         * and an infix operator to join each criteria with the next in the list.
         * 
         * @param terms
         *            the list of criteria terms from which the constructor
         *            generates the new criteria expression.
         * 
         * @param operator
         *            the infix operator, either {@link Operator#AND AND} or
         *            {@link Operator#OR OR} that joins each criteria term with the
         *            next in the list.
         */
        public CriteriaExpression(List<Criteria> terms, int op)
        {
            this.op = op;
            if (terms.Any())
            {
                this.term = terms[0];
                if (terms.Count > 1)
                    this.expression = new CriteriaExpression(terms.Skip(1).Take(terms.Count - 1).ToList(), op);
            }
        }

        /**
         * Returns the trailing expression in this criteria expression.
         * 
         * @return the trailing expression in this criteria expression.
         */
        public CriteriaExpression getExpression()
        {
            return expression;
        }

        /**
         * Returns the operator, either {@link Operator#AND AND} or
         * {@link Operator#OR OR}, in this criteria expression that joins the
         * initial criteria with the trailing criteria expression.
         * 
         * @return the operator, either {@link Operator#AND AND} or
         *         {@link Operator#OR OR}, in this criteria expression.
         */
        public int getOperator()
        {
            return op;
        }

        /**
         * Returns the initial criteria in this criteria expression.
         * 
         * @return the initial criteria in this criteria expression.
         */
        public Criteria getTerm()
        {
            return term;
        }

        /**
         * Writes this criteria expression to the given output destination. If both
         * term and expression are null, this method generates no output. If
         * expression is null, but term is defined, the method writes the term
         * criteria to the output. If both expression and term are defined, however,
         * the method creates a new logic operator object, either
         * {@link Operator#AND AND} or {@link Operator#OR OR}, assigns it the term
         * and expression, and writes this new operator to the output.
         * 
         * @param out
         *            the output destination to which to write this criteria
         *            expression.
         * 
         * @see com.truemesh.squiggle.Criteria#write(com.truemesh.squiggle.output.Output)
         */
        public override void write(Output output)
        {
            if (term == null && expression == null)
            {
                ;
            }
            else if (expression == null)
            {
                term.write(output);
            }
            else if (op == SQLOperator.AND)
            {
                new And(term, expression).write(output);
            }
            else if (op == SQLOperator.OR)
            {
                new Or(term, expression).write(output);
            }
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
            term.addReferencedTablesTo(tables);
            if (expression != null) expression.addReferencedTablesTo(tables);
        }
    }
}
