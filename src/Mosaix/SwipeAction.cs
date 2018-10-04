using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogyke.Core
{
    public class SwipeAction
    {
        public string ScreenId { get; set; }
        public Guid Id { get; private set; }
        public DirectionEnum Direction { get; set; }
        public DateTime Timestamp { get; private set; }

        public SwipeAction(string screenId, DirectionEnum direction)
        {
            Id = Guid.Parse(screenId);
            ScreenId = screenId;
            Direction = direction;
            Timestamp = DateTime.Now;
        }
    }
}
