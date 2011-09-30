using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle
{
    /// <summary>
    /// What can be selected from a table.
    /// </summary>
    public abstract class Projection : Selectable
    {
        public Table table { get; private set; }

        public Projection(Table table)
        {
            this.table = table;
        }

        public Table getTable()
        {
            return table;
        }

        public void addReferencedTablesTo(HashSet<Table> tables)
        {
            tables.Add(table);
        }

        public virtual void write(Outputs.Output output)
        {
            throw new NotImplementedException();
        }
    }

}
