using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace squiggle.mstests
{
    [TestClass]
    public class Example004
    {
        [TestMethod]
        public void selectWildcardColumn()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addToSelection(people.getWildcard());

            Assert.AreEqual(Matcher.Normalize(select.ToString()), (Matcher.Normalize((
                    "SELECT " +
                     "    people.* " +
                     "FROM " +
                     "    people"))));
        }
    }
}
