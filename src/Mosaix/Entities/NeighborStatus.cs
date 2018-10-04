using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaix.Entities
{
    public class NeighborStatus
    {
        public bool HasANeighborBottom { get; set; }
        public bool HasANeighborLeft { get; set; }
        public bool HasANeighborRight { get; set; }
        public bool HasANeighborTop { get; set; }
    }
}
