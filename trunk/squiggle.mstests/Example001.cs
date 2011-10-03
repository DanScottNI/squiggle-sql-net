using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace squiggle.mstests
{
    /// <summary>
    /// Summary description for Example001
    /// </summary>
    [TestClass]
    public class Example001
    {
        [TestMethod]
        public void SimpleSelect()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addColumn(people, "firstname");
            select.addColumn(people, "lastname");

            select.addOrder(people, "age", Order.DESCENDING);

            Assert.AreEqual(Matcher.Normalize(select.ToString()), Matcher.Normalize(
                    "SELECT  people.firstname , people.lastname " +
                    "FROM people " +
                    "ORDER BY people.age DESC"), true);
        }
    }
}
