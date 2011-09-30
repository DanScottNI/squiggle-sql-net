using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /// <remarks>
    /// Author: Nat Pryce, Joe Walnes
    /// </remarks>
    public abstract class Criteria : Outputable
    {
        public abstract void write(Output output);
        public abstract void addReferencedTablesTo(HashSet<Table> tables);
    }

}
