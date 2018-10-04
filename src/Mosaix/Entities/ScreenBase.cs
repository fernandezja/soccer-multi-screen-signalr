using Mosaix.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ogyke.Core.Entities
{
    public abstract class ScreenBase
    {
        public Guid Id { get; protected set; }
        public string ConnectionId { get; protected set; }
        public string Name { get; protected set; }
        private Rectangle _dimension;
        public Rectangle Dimension
        {
            get { return _dimension; }
            protected set { _dimension = value; }
        }

        public ScreenBase()
        {
            Id = Guid.NewGuid();
        }

        public string IdShort{
            get {
                return GuidHelper.Shorter(Id);
            }
        }

    }
}
