using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ogyke.Core.Entities
{
    public class Screen: ScreenBase
    {

        public Screen(Guid id, int width, int height, string connectionId)
        {
            Id = id;
            Dimension = new Rectangle(0, 0, width, height);
            ConnectionId = connectionId;
        }

        public Screen(Guid id, int width, int height)
        {
            Id = id;
            Dimension = new Rectangle(0, 0, width, height);
        }

        public Screen(int width, int height)
        {
            Dimension = new Rectangle(0, 0, width, height);
        }

        public Screen(int width, int height, string name)
        {
            Dimension = new Rectangle(0, 0, width, height);
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Screen(int x, int y, int width, int height)
        {
            Dimension = new Rectangle(x, y, width, height);
        }

        public Screen SetConnectionId(string connectionId)
        {
            ConnectionId = connectionId;
            return this;
        }

        public override bool Equals(object screen)
        {
            return Id.Equals(((Screen)screen).Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
