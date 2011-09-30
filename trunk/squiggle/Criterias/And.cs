using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Criterias
{
    public class And : BaseLogicGroup
    {
        public And(Criteria left, Criteria right)
            : base("AND", left, right)
        {

        }
    }
}
