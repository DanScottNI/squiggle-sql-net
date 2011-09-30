using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace squiggle.tests
{
    [TestFixture]
    public class Example004
    {
        [Test]
        public void selectWildcardColumn()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addToSelection(people.getWildcard());

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
                    "SELECT " +
                     "    people.* " +
                     "FROM " +
                     "    people"))));
        }
    }
}
