using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using squiggle.Criterias;

namespace squiggle.mstests
{
    [TestClass]
    public class Example003
    {
        [TestMethod]
        public void testNastyStrings()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addColumn(people, "firstname");

            select.addCriteria(
                    new MatchCriteria(people, "foo", MatchCriteria.GREATER, "I've got a quote"));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), (Matcher.Normalize((
                    "SELECT\n" +
                    "    people.firstname \n" +
                    "FROM\n" +
                    "    people\n" +
                    "WHERE\n" +
                    "    people.foo > 'I\\'ve got a quote'"))));
        }
    }
}
