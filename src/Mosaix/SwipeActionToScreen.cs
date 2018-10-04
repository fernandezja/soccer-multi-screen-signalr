using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogyke.Core
{
    public class SwipeActionToScreen: SwipeAction
    {
        public string ToScreenIdShort { get; set; }

        public SwipeActionToScreen(string screenId, string toScreenIdShort, DirectionEnum direction)
            :base(screenId, direction)
        {
            ToScreenIdShort = toScreenIdShort;
        }
    }
}
