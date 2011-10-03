using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace squiggle.mstests
{
    [TestClass]
    public class Example006
    {
        [TestMethod]
        public void tableAliases()
        {
            Table employees = new Table("people", "employees");
            Table managers = new Table("people", "managers");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(employees, "firstname");
            select.addColumn(managers, "firstname");

            select.addJoin(employees, "managerID", managers, "id");

            Assert.AreEqual(Matcher.Normalize(select.ToString()), (Matcher.Normalize((
                    "SELECT " +
                    "    employees.firstname , " +
                    "    managers.firstname " +
                    "FROM " +
                    "    people AS employees , " +
                    "    people AS  managers " +
                    "WHERE " +
                    "    employees.managerID = managers.id"))));
        }
    }
}
