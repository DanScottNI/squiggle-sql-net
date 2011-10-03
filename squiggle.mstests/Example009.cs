using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using squiggle.Criterias;

namespace squiggle.mstests
{
    [TestClass]
    public class Example009
    {
        [TestMethod]
        public void orAnd()
        {
            Table user = new Table("user");

            SelectQuery select = new SelectQuery();

            select.addToSelection(new WildCardColumn(user));

            Criteria name = new MatchCriteria(user, "name", MatchCriteria.LIKE, "a%");
            Criteria id = new MatchCriteria(user, "id", MatchCriteria.EQUALS, 12345);
            Criteria feet = new MatchCriteria(user, "feet", MatchCriteria.EQUALS, "smelly");

            select.addCriteria(new Or(name, new And(id, feet)));


            Assert.AreEqual(Matcher.Normalize(select.ToString()),
                (Matcher.Normalize(@"SELECT user.*  
FROM  user  
WHERE ( user.name LIKE 'a%' OR ( user.id = 12345 AND user.feet = 'smelly' ) )")));
        }
    }
}
