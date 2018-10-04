using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ogyke.Core.Enumerations
{
    /// <summary>
    /// Code enum for direction (screen and move swipe touch)
    /// The codes equal hammerJs constants
    /// https://hammerjs.github.io/api/#constants
    /// </summary>
    public enum DirectionEnum: byte
    {
        [Description("Up")]
        Up = 8,
        [Description("Right")]
        Right = 4,
        [Description("Down")]
        Down = 16,
        [Description("Left")]
        Left = 2
          

    }
}
