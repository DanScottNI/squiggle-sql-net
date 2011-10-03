using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using squiggle.Literals;
using squiggle.Criterias;

namespace squiggle.mstests
{
    [TestClass]
    public class Example010
    {
        [TestMethod]
        public void functions()
        {
            SelectQuery select = new SelectQuery();

            Table table = new Table("t");

            select.addToSelection(new FunctionCall("sheep"));
            select.addToSelection(new FunctionCall("cheese", new IntegerLiteral(10)));
            select.addToSelection(new FunctionCall("tomato", new StringLiteral("red"), table.getColumn("c")));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), Matcher.Normalize((
                    "SELECT " +
                    "    sheep() , " +
                    "    cheese(10) , " +
                    "    tomato('red', t.c) " +
                    "FROM " +
                    "    t ")));

        }

        [TestMethod]
        public void usingFunctionsInMatchCriteria()
        {
            Table cards = new Table("credit_cards");

            SelectQuery select = new SelectQuery();

            select.addToSelection(cards.getColumn("number"));
            select.addToSelection(cards.getColumn("issue"));

            select.addCriteria(
                    new BetweenCriteria(new FunctionCall("getDate"),
                            cards.getColumn("issue_date"), cards.getColumn("expiry_date")));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), Matcher.Normalize((
                    "SELECT " +
                            "    credit_cards.number , " +
                            "    credit_cards.issue " +
                            "FROM " +
                            "    credit_cards " +
                            "WHERE " +
                            "    getDate() BETWEEN credit_cards.issue_date AND credit_cards.expiry_date")));
        }

        [TestMethod]
        public void selectingFunctionThatDoesNotReferToTables()
        {
            SelectQuery select = new SelectQuery();
            select.addToSelection(new FunctionCall("getdate"));

            Assert.AreEqual(Matcher.Normalize(select.ToString()), Matcher.Normalize((
                    "SELECT" +
                    "    getdate()")));
        }
    }
}
