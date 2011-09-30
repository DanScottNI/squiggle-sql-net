using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    public class Parameter : Matchable
    {
        public void write(Output output)
        {
            output.print("?");
        }

        public void addReferencedTablesTo(HashSet<Table> tables)
        {
        }
    }

}
