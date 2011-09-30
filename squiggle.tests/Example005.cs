using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using squiggle.Criterias;

namespace squiggle.tests
{
    [TestFixture]
    public class Example005
    {
        [Test]
        public void joinOnForeignKeyRelationship()
        {
            Table people = new Table("people");
            Table departments = new Table("departments");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(people, "firstname");
            select.addColumn(departments, "director");

            select.addJoin(people, "departmentID", departments, "id");

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
                    "SELECT " +
                    "    people.firstname , " +
                    "    departments.director " +
                    "FROM " +
                    "    people , " +
                    "    departments " +
                    "WHERE " +
                    "    people.departmentID = departments.id"))));
        }

        [Test]
        public void joinOnComparison()
        {
            Table invoices = new Table("invoices");
            Table taxPaymentDate = new Table("tax_payment_date");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(invoices, "number");

            select.addJoin(invoices, "date", MatchCriteria.GREATER, taxPaymentDate, "date");

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
                    "SELECT " +
                    "    invoices.number " +
                    "FROM " +
                    "    invoices , " +
                    "    tax_payment_date " +
                    "WHERE " +
                    "    invoices.date > tax_payment_date.date"))));
        }

        [Test]
        public void doNotHaveToExplicitlyJoinTables()
        {
            Table invoices = new Table("invoices");
            Table taxPaymentDate = new Table("tax_payment_date");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(invoices, "number");
            select.addCriteria(new MatchCriteria(invoices.getColumn("date"), MatchCriteria.GREATER, taxPaymentDate.getColumn("date")));

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
                    "SELECT " +
                    "    invoices.number " +
                    "FROM " +
                    "    invoices , " +
                    "    tax_payment_date " +
                    "WHERE " +
                    "    invoices.date > tax_payment_date.date"))));
        }
    }
}
