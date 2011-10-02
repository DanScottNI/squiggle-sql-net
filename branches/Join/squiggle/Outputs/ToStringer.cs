using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace squiggle.Outputs
{
    public class ToStringer
    {
        public static String toString(Outputable outputable)
        {
            Output output = new Output("    ");
            outputable.write(output);

            return output.ToString();
        }
    }
}
