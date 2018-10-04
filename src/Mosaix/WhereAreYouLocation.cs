using Ogyke.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ogyke.Core
{
    public class WhereAreYouLocation
    {
        public Location Get(Element element) {
            return new Location();
        }

        /// <summary>
        /// Use https://github.com/dotnet/corefx/blob/master/src/System.Drawing.Primitives/src/System/Drawing/Rectangle.cs
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool IsIn(Screen screen, Element element)
        {
            var isIn = false;
            if (screen.Dimension.IntersectsWith(element.Dimension))
            {
                isIn = true;
            }
            return isIn;
        }
    }
}
