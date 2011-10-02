using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /**
 * A literal value, such as a number, string or boolean.
 * 
 * @author Nat Pryce
 * 
 */
    public abstract class Literal : Outputable, Matchable, Selectable
    {
        public void addReferencedTablesTo(HashSet<Table> tables)
        {
        }

        public virtual void write(Output output)
        {
            throw new NotImplementedException();
        }
    }

}
