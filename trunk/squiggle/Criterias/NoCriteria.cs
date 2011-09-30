using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle.Criterias
{
    /**
 * Class NoCriteria is a Criteria that represents an absent operand in an SQL
 * predicate expression so that one may represent a unary operator (for example,
 * {@link NOT}) using a binary operator derived from a {@link BaseLogicGroup}).
 * 
 * @author <a href="mailto:derek@derekmahar.ca">Derek Mahar</a>
 */
    public class NoCriteria : Criteria
    {
        /**
         * Writes an empty criteria (single space) to the given output stream.
         * 
         * @see com.truemesh.squiggle.Criteria#write(com.truemesh.squiggle.output.Output)
         */
        public override void write(Output output)
        {
            output.print(' ');
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
        }
    }

}
