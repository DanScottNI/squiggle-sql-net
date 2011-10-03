using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /**
 * Something that can be part of a match expression in a where clause
 * 
 * @author Nat Pryce
 */
    public interface Matchable : Outputable
    {
        void addReferencedTablesTo(HashSet<Table> tables);
    }

}
