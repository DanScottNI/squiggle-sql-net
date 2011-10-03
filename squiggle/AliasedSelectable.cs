using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    class AliasedSelectable : Selectable
    {
        private Selectable selectable;
        private String alias;

        public AliasedSelectable(Selectable selectable, String alias)
        {
            this.selectable = selectable;
            this.alias = alias;
        }

        public void addReferencedTablesTo(HashSet<Table> tables)
        {
            if (this.selectable != null)
            {
                this.selectable.addReferencedTablesTo(tables);
            }
        }

        public void write(Output output)
        {
            if (this.selectable != null)
            {
                this.selectable.write(output);
            }
            else
            {
                output.print("NULL");
            }

            output.print(" AS ").print(this.alias);
        }
    }
}
