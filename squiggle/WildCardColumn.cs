using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /**
 * Special column to represent For SELECT * FROM ...
 * 
 * @author <a href="joe@truemesh.com">Joe Walnes</a>
 * @author Nat Pryce
 */
    public class WildCardColumn : Projection
    {
        public WildCardColumn(Table table) : base(table)
        {
        }

        public override void write(Output output)
        {
            output.print(getTable().getAlias()).print(".*");
        }
    }

}
