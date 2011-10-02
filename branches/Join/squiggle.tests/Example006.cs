using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace squiggle.tests
{
    [TestFixture]
    public class Example006
    {
        [Test]
        public void tableAliases()
        {
            Table employees = new Table("people", "employees");
            Table managers = new Table("people", "managers");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(employees, "firstname");
            select.addColumn(managers, "firstname");

            select.addJoin(employees, "managerID", managers, "id");

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo( Matcher.Normalize((
                    "SELECT " +
                    "    employees.firstname , " +
                    "    managers.firstname " +
                    "FROM " +
                    "    people employees , " +
                    "    people managers " +
                    "WHERE " +
                    "    employees.managerID = managers.id"))));
        }
    }
}
