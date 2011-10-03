using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squiggle.Outputs;

namespace squiggle
{
    /**
 * @author <a href="joe@truemesh.com">Joe Walnes</a>
 * @author Nat Pryce
 */
    public class Table : Outputable
    {
        private String name;
        private String alias;

        public Table(String name)
            : this(name, null)
        {

        }

        public Table(String name, String alias)
        {
            this.name = name;
            this.alias = alias;
        }

        /**
         * Name of table
         */
        public String getName()
        {
            return name;
        }

        /**
         * Whether this table has an alias assigned.
         */
        private bool hasAlias()
        {
            return alias != null;
        }

        /**
         * Short alias of table
         */
        public String getAlias()
        {
            return alias != null ? alias : name;
        }

        /**
         * Get a column for a particular table.
         */
        public Column getColumn(String columnName)
        {
            return new Column(this, columnName);
        }

        public WildCardColumn getWildcard()
        {
            return new WildCardColumn(this);
        }

        public override bool Equals(Object o)
        {
            if (this == o)
                return true;
            if (o == null)
                return false;
            if (GetType() != o.GetType())
                return false;

            Table that = (Table)o;

            return getAlias() == that.getAlias();
        }

        public override int GetHashCode()
        {
            return getAlias().GetHashCode();
        }

        public void write(Output output)
        {
            output.print(getName());
            if (hasAlias())
            {
                output.print(" AS ");
                output.print(getAlias());
            }
        }
    }

}
