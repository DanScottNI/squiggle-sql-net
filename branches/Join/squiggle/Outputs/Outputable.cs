using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Outputs
{
    /// <summary>
    /// Any object that can output itself as part of the final query should implement this interface.
    /// </summary>
    public interface Outputable
    {
        void write(Output output);
    }

}
