using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using squiggle.Criterias;

namespace squiggle.mstests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Example002
    {
        [TestMethod]
        public void whereCriteria()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addColumn(people, "firstname");
            select.addColumn(people, "lastname");

            select.addCriteria(
                    new MatchCriteria(people, "height", MatchCriteria.GREATER, 1.8));
            select.addCriteria(
                    new InCriteria(people, "department", new String[] { "I.T.", "Cooking" }));
            select.addCriteria(
                    new BetweenCriteria(people.getColumn("age"), 18, 30));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), (Matcher.Normalize((
                    "SELECT " +
                    "    people.firstname , " +
                    "    people.lastname " +
                    "FROM " +
                    "    people " +
                    "WHERE " +
                    "    people.height > 1.8 AND " +
                    "    people.department IN ( " +
                    "        'I.T.', 'Cooking' " +
                    "    ) AND" +
                    "    people.age BETWEEN 18 AND 30"))));

        }

        [TestMethod]
        public void nullCriteria()
        {
            Table people = new Table("people");

            SelectQuery select = new SelectQuery();

            select.addToSelection(people.getWildcard());

            select.addCriteria(
                    new IsNullCriteria(people.getColumn("name")));
            select.addCriteria(
                    new IsNotNullCriteria(people.getColumn("age")));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), (Matcher.Normalize((
                    "SELECT " +
                    "    people.* " +
                    "FROM " +
                    "    people " +
                    "WHERE " +
                    "    people.name IS NULL AND " +
                    "    people.age IS NOT NULL"))));
        }

        [TestMethod]
        public void betweenCriteriaWithColumns()
        {
            Table rivers = new Table("rivers");

            SelectQuery select = new SelectQuery();

            select.addColumn(rivers, "name");
            select.addColumn(rivers, "level");

            select.addCriteria(
                    new BetweenCriteria(rivers.getColumn("level"), rivers.getColumn("lower_limit"), rivers.getColumn("upper_limit")));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), (Matcher.Normalize((
                    "SELECT " +
                    "    rivers.name , " +
                    "    rivers.level " +
                    "FROM " +
                    "    rivers " +
                    "WHERE " +
                    "    rivers.level BETWEEN rivers.lower_limit AND rivers.upper_limit"))));
        }
    }
}
