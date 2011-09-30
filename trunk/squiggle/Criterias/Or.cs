using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Criterias
{
    public class Or : BaseLogicGroup
    {
        public Or(Criteria left, Criteria right)
            : base("OR", left, right)
        {

        }
    }
}
