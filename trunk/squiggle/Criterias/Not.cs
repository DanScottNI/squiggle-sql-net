using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Criterias
{
    public class Not : BaseLogicGroup
    {
        /**
         * Initializes an SQL NOT operator with the given criteria that appears to the
         * right of the operator.
         * 
         * @param right
         *            the criteria or operand to which the NOT operator applies.
         */
        public Not(Criteria right)
            : base("NOT", new NoCriteria(), right)
        {

        }
    }

}
