using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle
{
    public class JoinOn : Criteria
    {
        public JoinType JoinType { get; set; }
        public Column SourceColumn { get; set; }
        public Column DestColumn { get; set; }

        public JoinOn(JoinType joinType, Column sourceColumn, Column destColumn)
        {
            this.JoinType = joinType;
            this.SourceColumn = sourceColumn;
            this.DestColumn = destColumn;
        }

        public override void write(Outputs.Output output)
        {
            output.print(" ");
            switch (JoinType)
            {
                case JoinType.CrossJoin: output.print("CROSS JOIN"); break;
                case JoinType.Inner: output.print("INNER JOIN"); break;
                case JoinType.LeftOuterJoin: output.print("LEFT JOIN"); break;
                case JoinType.RightOuterJoin: output.print("RIGHT JOIN"); break;
            }
            output.print(" ");
            SourceColumn.table.write(output);
            output.print(" ON ");
            SourceColumn.write(output);
            output.print(" = ");
            DestColumn.write(output);
            output.print(' ');
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
            tables.Add(SourceColumn.table);
            tables.Add(DestColumn.table);
        }
    }
}
