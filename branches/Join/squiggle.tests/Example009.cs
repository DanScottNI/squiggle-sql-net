using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using squiggle.Criterias;

namespace squiggle.tests
{
    [TestFixture]
    public class Example009 
    {
        [Test]
        public void orAnd()
        {
            Table user = new Table("user");

            SelectQuery select = new SelectQuery();

            select.addToSelection(new WildCardColumn(user));

            Criteria name = new MatchCriteria(user, "name", MatchCriteria.LIKE, "a%");
            Criteria id = new MatchCriteria(user, "id", MatchCriteria.EQUALS, 12345);
            Criteria feet = new MatchCriteria(user, "feet", MatchCriteria.EQUALS, "smelly");

            select.addCriteria(new Or(name, new And(id, feet)));

            
            Assert.That(Matcher.Normalize(select.ToString()), 
                Is.EqualTo(Matcher.Normalize(@"SELECT user.*  
FROM  user  
WHERE ( user.name LIKE 'a%' OR ( user.id = 12345 AND user.feet = 'smelly' ) )")));
        }
    }
}