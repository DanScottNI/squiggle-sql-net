using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle
{
    public enum JoinType
    {
        Inner,
        CrossJoin,
        LeftOuterJoin,
        RightOuterJoin
    }

    public class TableSet : Outputs.Outputable
    {
        private List<Table> tables = new List<Table>();
        private List<JoinOn> joins = new List<JoinOn>();

        public TableSet(List<Table> tables)
            : this(tables, null)
        {
        }

        public TableSet(List<Table> tables, List<JoinOn> joins)
        {
            if (tables != null)
            {
                this.tables = tables;
            }

            if (joins != null)
            {
                this.joins = joins;
            }
        }

        

        

        public void write(Outputs.Output output)
        {
            throw new NotImplementedException();
        }
    }
}