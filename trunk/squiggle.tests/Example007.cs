using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using squiggle.Criterias;

namespace squiggle.tests
{
    [TestFixture]
    public class Example007
    {
        [Test]
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

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
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
