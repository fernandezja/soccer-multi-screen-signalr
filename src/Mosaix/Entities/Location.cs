using System;
using System.Collections.Generic;
using System.Text;

namespace Ogyke.Core.Entities
{
    public class Location
    {
        public long X { get; private set; }
        public long Y { get; private set; }

        public Location()
        {
            X = 0;
            Y = 0;
        }

        public Location(long x, long y)
        {
            X = x;
            Y = y;
        }
    }
}
