using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using squiggle.Criterias;

namespace squiggle.tests
{
    [TestFixture]
    public class Example003
    {
        [Test]
        public void testNastyStrings()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addColumn(people, "firstname");

            select.addCriteria(
                    new MatchCriteria(people, "foo", MatchCriteria.GREATER, "I've got a quote"));

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo( Matcher.Normalize((
                    "SELECT\n" +
                    "    people.firstname\n" +
                    "FROM\n" +
                    "    people\n" +
                    "WHERE\n" +
                    "    people.foo > 'I\\'ve got a quote'"))));
        }
    }
}
