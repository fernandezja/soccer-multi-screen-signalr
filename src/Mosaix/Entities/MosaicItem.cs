using Ogyke.Core.Enumerations;
using Ogyke.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ogyke.Core.Entities
{
    public class MosaicItem : IMosaicItem
    {
        public Screen Screen { get; protected set; }
        public IMosaicItem NeighborTop { get; private set; }
        public IMosaicItem NeighborRight { get; private set; }
        public IMosaicItem NeighborBottom { get; private set; }
        public IMosaicItem NeighborLeft { get; private set; }

        private Rectangle _dimensionIntoMosaic;
        public Rectangle DimensionMosaic
        {
            get { return _dimensionIntoMosaic; }
            protected set { _dimensionIntoMosaic = value; }
        }

        public MosaicItem(Screen screen)
        {
            Screen = screen;
        }

        public void MoveIntoMosaic(Point location)
        {
            _dimensionIntoMosaic.Location = location;
            _dimensionIntoMosaic.Width = Screen.Dimension.Width;
            _dimensionIntoMosaic.Height = Screen.Dimension.Height;
        }

        public void MoveIntoMosaic(int x, int y)
        {
            MoveIntoMosaic(new Point(x, y));
        }

        public void Resize(Screen screen)
        {
            Screen = screen;
        }

        public void Resize(Screen screen, int width, int height)
        {
            Screen = new Screen(screen.Id, width, height, screen.ConnectionId);
        }

        public IMosaicItem GetNeighbor(DirectionEnum direction) {

            switch (direction)
            {
                case DirectionEnum.Up:
                    return NeighborTop;
                case DirectionEnum.Right:
                    return NeighborRight;
                case DirectionEnum.Down:
                    return NeighborBottom;
                case DirectionEnum.Left:
                    return NeighborLeft;
                default:
                    return null;
            }
        }

        public IMosaicItem AddNeighbor(IMosaicItem neighbor, DirectionEnum direction)
        {
            if (GetNeighbor(direction) != null)
            {
                return null;
            }

            switch (direction)
            {
                case DirectionEnum.Up:
                    NeighborTop = neighbor;
                    break;
                case DirectionEnum.Right:
                    NeighborRight = neighbor;
                    break;
                case DirectionEnum.Down:
                   NeighborBottom = neighbor;
                    break;
                case DirectionEnum.Left:
                    NeighborLeft = neighbor;
                    break;
                default:
                    return null;
            }

            return neighbor;
        }


        public IMosaicItem Clean(DirectionEnum direction) {
            //Clean tree node
            throw new NotImplementedException();
        }

        public IMosaicItem AddNeighbor(Screen screen, DirectionEnum direction) {

            var neighbor = GetNeighbor(direction);
            if (neighbor != null)
            {
                throw new ApplicationException("Neighbor Exist. Not is posible add node");
            }

            var item = new MosaicItem(screen);
            return AddNeighbor(item, direction); 
        }

        public Rectangle DimensionTop(bool includeMe)
        {

            int width = 0;
            int height = 0;


            //Only the top dimension neighbor exists, the value of the caller is considered
            if (includeMe)
            {
                width = Screen.Dimension.Width;
                height = Screen.Dimension.Height;
            }

            if (NeighborTop != null)
            {
                var dimensionTop = NeighborTop.DimensionTop(includeMe: true);

                if (dimensionTop.Width > width)
                {
                    width = dimensionTop.Width;
                }

                height += dimensionTop.Height;
            }

            var dimension = new Rectangle(0, 0, width, height);
            return dimension;
        }

        public Rectangle DimensionRight(bool includeMe) {

            int width = 0;
            int height = 0;
            if (includeMe)
            {
                width = Screen.Dimension.Width;
                height = Screen.Dimension.Height;
            }

            if (NeighborRight != null)
            {
                var dimensionRight = NeighborRight.DimensionRight(includeMe:true);
                width += dimensionRight.Width;
                if (dimensionRight.Height > height)
                {
                    height = dimensionRight.Height;
                }
            }
            var dimension = new Rectangle(0,0, width, height);
            return dimension;
        }

        public Rectangle DimensionBottom(bool includeMe)
        {

            int width = 0;
            int height = 0;

            //Only the bottom dimension neighbor exists, the value of the caller is considered
            if (includeMe)
            {
                width = Screen.Dimension.Width;
                height = Screen.Dimension.Height;
            }

            if (NeighborBottom != null)
            {
                var dimensionBottom = NeighborBottom.DimensionBottom(includeMe: true);

                if (dimensionBottom.Width > width)
                {
                    width = dimensionBottom.Width;
                }

                height += dimensionBottom.Height;
            }
            var dimension = new Rectangle(0, 0, width, height);
            return dimension;
        }

        public Rectangle DimensionLeft(bool includeMe)
        {
            int width = 0;
            int height = 0;
            if (includeMe)
            {
                width = Screen.Dimension.Width;
                height = Screen.Dimension.Height;
            }

            if (NeighborLeft != null)
            {
                var dimensionLeft = NeighborLeft.DimensionLeft(includeMe: true);

                width += dimensionLeft.Width;

                if (dimensionLeft.Height > height)
                {
                    height = dimensionLeft.Height;
                }
            }
            var dimension = new Rectangle(0, 0, width, height);
            return dimension;
        }


        public List<IMosaicItem> GetNeighbors()
        {
            var nodes = new List<IMosaicItem>();

            if (NeighborTop != null)
            {
                nodes.Add(NeighborTop);
                nodes.AddRange(NeighborTop.GetNeighbors());
            }
            if (NeighborRight != null)
            {
                nodes.Add(NeighborRight);
                nodes.AddRange(NeighborRight.GetNeighbors());
            }
            if (NeighborBottom != null)
            {
                nodes.Add(NeighborBottom);
                nodes.AddRange(NeighborBottom.GetNeighbors());
            }
            if (NeighborLeft != null)
            {
                nodes.Add(NeighborLeft);
                nodes.AddRange(NeighborLeft.GetNeighbors());
            }

            return nodes;
        }


        public bool HasANeighborBottom {
            get
            {
                return NeighborBottom != null;
            }
        }
        public bool HasANeighborLeft
        {
            get
            {
                return NeighborLeft != null;
            }
        }
        public bool HasANeighborRight
        {
            get
            {
                return NeighborRight != null;
            }
        }
        public bool HasANeighborTop
        {
            get
            {
                return NeighborTop != null;
            }
        }
    }
}
