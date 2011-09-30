using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace squiggle.tests
{
    [TestFixture]
    public class Example001
    {
        [Test]
        public void simpleSelect()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addColumn(people, "firstname");
            select.addColumn(people, "lastname");

            select.addOrder(people, "age", Order.DESCENDING);

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize(
                    "SELECT  people.firstname , people.lastname " +
                    "FROM people " +
                    "ORDER BY people.age DESC")));
        }
    }
}
