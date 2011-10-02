using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace squiggle.tests
{
    [TestFixture]
    public class Example011
    {
        [Test]
        public void InnerJoin()
        {
            Table people = new Table("people");
            Table departments = new Table("departments");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(people, "firstname");
            select.addColumn(departments, "director");

            select.addJoin(JoinType.Inner, people, "departmentID", departments, "id");

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
                    "SELECT " +
                    "    people.firstname , " +
                    "    departments.director " +
                    "FROM " +
                    "    people , " +
                    "    departments " +
                    "WHERE " +
                    "    people.departmentID = departments.id"))));
        }
    }
}
