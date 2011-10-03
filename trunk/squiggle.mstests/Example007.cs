using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using squiggle.Criterias;

namespace squiggle.mstests
{
    [TestClass]
    public class Example007
    {
        [TestMethod]
        public void testSubSelect()
        {
            Table people = new Table("people");
            Table taxcodes = new Table("taxcodes");

            SelectQuery select = new SelectQuery();
            select.addColumn(people, "firstname");

            SelectQuery subSelect = new SelectQuery();
            subSelect.addColumn(taxcodes, "id");
            subSelect.addCriteria(new MatchCriteria(taxcodes, "valid", MatchCriteria.EQUALS, true));

            select.addCriteria(new InCriteria(people, "taxcode", subSelect));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), (Matcher.Normalize((
                    "SELECT " +
                    "    people.firstname " +
                    "FROM " +
                    "    people " +
                    "WHERE " +
                    "    people.taxcode IN ( " +
                    "        SELECT " +
                    "            taxcodes.id " +
                    "        FROM " +
                    "            taxcodes " +
                    "        WHERE " +
                    "            taxcodes.valid = true " +
                    "    )"))));
        }
    }
}
