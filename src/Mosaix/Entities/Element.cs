using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ogyke.Core.Entities
{
    public class Element
    {
        private Rectangle _dimension;
        public Rectangle Dimension
        {
            get { return _dimension; }
            private set { _dimension = value; }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Element(int width, int height)
        {
            Dimension = new Rectangle(0, 0, width, height);
        }

        public void Move(Point location)
        {
            _dimension.Location = location;
        }


        public void Move(int x, int y)
        {
            Move(new Point(x, y));
        }

    }
}
