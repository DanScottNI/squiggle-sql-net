using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;
using squiggle.Criterias;
using System.Collections.ObjectModel;

namespace squiggle
{
    /**
  * @author <a href="joe@truemesh.com">Joe Walnes</a>
  * @author Nat Pryce
  */
    public class SelectQuery : Outputable, ValueSet
    {
        public static int indentSize = 4;

        private List<Selectable> selection = new List<Selectable>();
        private List<Criteria> criteria = new List<Criteria>();
        private List<JoinOn> joins = new List<JoinOn>();
        private List<Selectable> groupby = new List<Selectable>();
        private List<Criteria> having = new List<Criteria>();
        private List<Order> order = new List<Order>();

        public bool isDistinct { get; set; }

        public List<Table> listTables()
        {
            HashSet<Table> tables = new HashSet<Table>();
            addReferencedTablesTo(tables);
            return new List<Table>(tables);
        }

        /**
	 * Syntax sugar for addToGroupBy(Column).
	 */
        public void addGroupByColumn(Table table, String columname)
        {
            addToGroupBy(table.getColumn(columname));
        }

        public void addToGroupBy(Selectable selectable)
        {
            groupby.Add(selectable);
        }

        public void addHaving(Criteria criteria)
        {
            this.having.Add(criteria);
        }

        public void addToSelection(Selectable selectable)
        {
            selection.Add(selectable);
        }

        public void addToSelection(Selectable selectable, String alias)
        {
            selection.Add(new AliasedSelectable(selectable, alias));
        }

        /**
         * Syntax sugar for addToSelection(Column).
         */
        public void addColumn(Table table, String columname)
        {
            addToSelection(table.getColumn(columname));
        }

        /**
      * Syntax sugar for addToSelection(Column,Alias).
      */
        public void addColumn(Table table, String columname, String columnAlias)
        {
            addToSelection(table.getColumn(columname), columnAlias);
        }

        public void removeFromSelection(Selectable selectable)
        {
            selection.Remove(selectable);
        }

        /**
         * @return a list of {@link Selectable} objects.
         */
        public ReadOnlyCollection<Selectable> listSelection()
        {
            return selection.AsReadOnly();
        }



        public void addCriteria(Criteria criteria)
        {
            this.criteria.Add(criteria);
        }

        public void removeCriteria(Criteria criteria)
        {
            this.criteria.Remove(criteria);
        }

        public ReadOnlyCollection<Criteria> listCriteria()
        {
            return criteria.AsReadOnly();
        }

        /**
         * Syntax sugar for addCriteria(JoinCriteria)
         */
        public void addJoin(Table srcTable, String srcColumnname, Table destTable, String destColumnname)
        {
            addCriteria(new MatchCriteria(srcTable.getColumn(srcColumnname), MatchCriteria.EQUALS, destTable.getColumn(destColumnname)));
        }

        /**
         * Syntax sugar for addCriteria(JoinCriteria)
         */
        public void addJoin(Table srcTable, String srcColumnName, String op, Table destTable, String destColumnName)
        {
            addCriteria(new MatchCriteria(srcTable.getColumn(srcColumnName), op, destTable.getColumn(destColumnName)));
        }

        public void addJoin(JoinType joinType, Table srcTable, String srcColumnname, Table destTable, String destColumnname)
        {
            addJoin(new JoinOn(joinType, srcTable.getColumn(srcColumnname), destTable.getColumn(destColumnname)));
        }

        private void addJoin(JoinOn joinOn)
        {
            this.joins.Add(joinOn);
        }

        public void addOrder(Order order)
        {
            this.order.Add(order);
        }

        /**
         * Syntax sugar for addOrder(Order).
         */
        public void addOrder(Table table, String columnname, bool ascending)
        {
            addOrder(new Order(table.getColumn(columnname), ascending));
        }

        public void removeOrder(Order order)
        {
            this.order.Remove(order);
        }

        public ReadOnlyCollection<Order> listOrder()
        {
            return order.AsReadOnly();
        }

        public override String ToString()
        {
            return ToStringer.toString(this);
        }

        public void write(Output output)
        {
            output.print("SELECT");
            if (isDistinct)
            {
                output.print(" DISTINCT");
            }
            output.println();

            appendIndentedList(output, selection, ",");

            HashSet<Table> tables = findAllUsedTables();

            if (tables.Any() || joins.Any())
            {
                output.println("FROM");
            }

            if (tables.Any() && !joins.Any())
            {
                appendIndentedList(output, tables.ToList(), ",");
            }

            if (joins.Any())
            {
                appendIndentedList(output, joins, " ");
            }

            // Add criteria
            if (criteria.Any())
            {
                output.println("WHERE");
                appendIndentedList(output, criteria, "AND");
            }

            // Add GroupBy
            if (groupby.Any())
            {
                output.println("GROUP BY");
                appendIndentedList(output, groupby, ",");
            }

            // Add Having
            if (having.Any())
            {
                output.println("HAVING");
                appendIndentedList(output, having, "AND");
            }

            // Add order
            if (order.Any())
            {
                output.println("ORDER BY");
                appendIndentedList(output, order, ",");
            }
        }

        protected void appendIndentedList<T>(Output output, List<T> things, String seperator) where T : Outputable
        {
            output.indent();
            appendList(output, things, seperator);
            output.unindent();
        }

        /**
         * Iterate through a Collection and append all entries (using .toString()) to
         * a StringBuffer.
         */
        protected void appendList<T>(Output output, List<T> collection, String seperator) where T : Outputable
        {
            if (collection.Any())
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    Outputable curr = collection[i];

                    curr.write(output);
                    output.print(' ');

                    if (i < collection.Count - 1)
                    {
                        output.print(seperator);
                    }

                    output.println();
                }
            }

            //Iterator<? extends Outputable> i = collection.iterator();
            //boolean hasNext = i.hasNext();

            //while (hasNext) {
            //    Outputable curr = (Outputable) i.next();
            //    hasNext = i.hasNext();
            //    curr.write(out);
            //    out.print(' ');
            //    if (hasNext) {
            //        out.print(seperator);
            //    }
            //    out.println();
            //}
        }

        /**
         * Find all the tables used in the query (from columns, criteria and order).
         *
         * @return Set of {@link com.truemesh.squiggle.Table}s
         */
        protected HashSet<Table> findAllUsedTables()
        {
            HashSet<Table> tables = new HashSet<Table>();
            addReferencedTablesTo(tables);
            return tables;
        }

        public void addReferencedTablesTo(HashSet<Table> tables)
        {
            foreach (Selectable s in selection)
            {
                s.addReferencedTablesTo(tables);
            }
            foreach (Criteria c in criteria)
            {
                c.addReferencedTablesTo(tables);
            }
            foreach (Order o in order)
            {
                o.addReferencedTablesTo(tables);
            }
        }
    }

}
