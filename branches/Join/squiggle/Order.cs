using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /**
 * ORDER BY clause. See SelectQuery.addOrder(Order).
 * 
 * @author <a href="joe@truemesh.com">Joe Walnes</a>
 */
    public class Order : Outputable
    {
        public static bool ASCENDING = true;
        public static bool DESCENDING = false;

        private Column column;
        private bool ascending;

        /**
         * @param column    Column to order by.
         * @param ascending Order.ASCENDING or Order.DESCENDING
         */
        public Order(Column column, bool ascending)
        {
            this.column = column;
            this.ascending = ascending;
        }

        public Projection getColumn()
        {
            return column;
        }

        public void write(Output output)
        {
            column.write(output);
            if (!ascending)
            {
                output.print(" DESC");
            }
        }

        public void addReferencedTablesTo(HashSet<Table> tables)
        {
            column.addReferencedTablesTo(tables);
        }
    }

}
