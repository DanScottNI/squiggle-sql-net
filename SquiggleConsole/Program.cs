using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Criterias;
using squiggle;

namespace SquiggleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // basic query
            SelectQuery select = new SelectQuery();

            // add columns
            Table orders = new Table("orders_table");
            select.addColumn(orders, "id");
            select.addColumn(orders, "total_price");

            // matches
            select.addCriteria(new MatchCriteria(orders, "status", MatchCriteria.EQUALS, "processed"));
            select.addCriteria(new MatchCriteria(orders, "items", MatchCriteria.LESS, 5));

            // IN...
            select.addCriteria(new InCriteria(orders, "delivery",
                new String[] { "post", "fedex", "goat" }));

            // join
            Table warehouses = new Table("warehouses_table");
            select.addJoin(orders, "warehouse_id", warehouses, "id");

            // use joined table
            select.addColumn(warehouses, "location");
            select.addCriteria(new MatchCriteria(warehouses, "size", MatchCriteria.EQUALS, "big"));

            // build subselect query
            SelectQuery subSelect = new SelectQuery();
            Table offers = new Table("offers_table");
            subSelect.addColumn(offers, "location");
            subSelect.addCriteria(new MatchCriteria(offers, "valid", MatchCriteria.EQUALS, true));

            // add subselect to original query
            select.addCriteria(new InCriteria(warehouses, "location", subSelect));

            Console.WriteLine(select.ToString());
            Console.ReadLine();
        }
    }
}
