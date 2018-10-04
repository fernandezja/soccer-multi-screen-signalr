using Ogyke.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Ogyke.Core
{
    public class Maker
    {
        private Rectangle _dimension;
        public Rectangle Dimension
        {
            get { return _dimension; }
            private set { _dimension = value; }
        }

        public List<IMosaicItem> Items { get; private set; }

        public Maker(List<IMosaicItem> items)
        {
            Items = items;
            Dimension = new Rectangle(0, 0, 0, 0);
        }

        public void Init()
        {
            NodeMaker(Items.First());
            NodeDimensionMosaicMaker();
        }

        public void NodeMaker(IMosaicItem item) {

            int width = item.Screen.Dimension.Width;
            int height = item.Screen.Dimension.Height;

            //Top
            var dimensionTop = item.DimensionTop(includeMe: false);
            height += dimensionTop.Height;
            if (dimensionTop.Width > width)
            {
                width = dimensionTop.Width;
            }


            //Right
            var dimensionRight = item.DimensionRight(includeMe: false);
            width += dimensionRight.Width;
            if (dimensionRight.Height > height)
            {
                height = dimensionRight.Height;
            }

            //Bottom
            var dimensionBottom = item.DimensionBottom(includeMe: false);
            height += dimensionBottom.Height;
            if (dimensionBottom.Width > width)
            {
                width = dimensionBottom.Width;
            }

            //Left
            var dimensionLeft = item.DimensionLeft(includeMe: false);
            width += dimensionLeft.Width;
            if (dimensionLeft.Height > height)
            {
                height = dimensionLeft.Height;
            }

            _dimension.Width = width;
            _dimension.Height = height;
        }

        public List<IMosaicItem> GetNodesTreePath() {
            var nodes = new List<IMosaicItem>();
            var root = Items.First();

            nodes.Add(root);
            nodes.AddRange(root.GetNeighbors());

            return nodes;
        }

        public void NodeDimensionMosaicMaker() {
            var nodes = GetNodesTreePath();

            var x = 0;
            var y = 0;
            ///nodes[0].Screen.Dimension.Width //nodes[0].Screen.Dimension.Height
            ///
            foreach (var n in nodes)
            {
                n.MoveIntoMosaic(x, y);
                x += n.Screen.Dimension.Width;
                //y += n.Screen.Dimension.Height;
            }
        }
    }
}
