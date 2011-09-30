using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle
{
    public class TableSet
    {
        private List<Table> tables = new List<Table>();
        private List<JoinOn> joins = new List<JoinOn>();

        public TableSet(List<Table> tables)
        {
        }

        public TableSet(List<Table> tables, List<JoinOn> joins)
        {
            this.tables = tables;
            this.joins = joins;
        }
    }
}
