using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /**
 * Something that can be returned from a select query
 * 
 * @author Nat Pryce
 */
    public interface Selectable : Outputable
    {
        void addReferencedTablesTo(HashSet<Table> tables);
    }

}
