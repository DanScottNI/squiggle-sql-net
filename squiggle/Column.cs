using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /// <remarks>
    /// Author: Nat Pryce, Joe Walnes
    /// </remarks>
    public class Column : Projection, Matchable
    {
        private String name;

        public Column(Table table, String name)
            : base(table)
        {

            this.name = name;
        }

        public String getName()
        {
            return name;
        }

        public override void write(Output output)
        {
            output.print(getTable().getAlias()).print('.').print(getName());
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = getTable().GetHashCode();
            result = prime * result + name.GetHashCode();
            return result;
        }

        public override bool Equals(Object o)
        {
            if (this == o)
                return true;
            if (o == null)
                return false;
            if (GetType() != o.GetType())
                return false;

            Column that = (Column)o;

            return this.name == that.name
                && this.getTable().Equals(that.getTable());
        }
    }

}
