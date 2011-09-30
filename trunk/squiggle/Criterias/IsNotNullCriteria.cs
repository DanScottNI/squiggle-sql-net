﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle.Criterias
{
    public class IsNotNullCriteria : Criteria
    {
        private Matchable matched;

        public IsNotNullCriteria(Matchable matched)
        {
            this.matched = matched;
        }


        public override void write(Output output)
        {
            matched.write(output);
            output.print(" IS NOT NULL");
        }

        public override void addReferencedTablesTo(HashSet<Table> tables)
        {
            matched.addReferencedTablesTo(tables);
        }
    }

}
