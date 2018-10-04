using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaix.Helpers
{
    public abstract class DirectionHelper
    {
        public static DirectionEnum OppositeDirection(DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Left:
                    return DirectionEnum.Right;
                case DirectionEnum.Right:
                    return DirectionEnum.Left;
                case DirectionEnum.Up:
                    return DirectionEnum.Down;
                case DirectionEnum.Down:
                    return DirectionEnum.Up;
            }
            return 0;
        }
    }
}
