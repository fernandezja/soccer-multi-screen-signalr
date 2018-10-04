using Mosaix.Helpers;
using Ogyke.Core.Enumerations;
using Ogyke.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Ogyke.Core.Entities
{
    public class Mosaic : IMosaic
    {
        public Guid Id { get; private set; }
        private Rectangle _dimension;
        public Rectangle Dimension
        {
            get { return _dimension; }
            private set { _dimension = value; }
        }


        public List<IMosaicItem> Items { get; private set; }

        public Mosaic()
        {
            Items = new List<IMosaicItem>();
            Id = Guid.NewGuid();
        }

        public Mosaic AddNeighbor(Screen screenNew)
        {
            if (Items.Any())
            {
                throw new ApplicationException("Mosaic exist. ");
            }

            var item = new MosaicItem(screenNew);
            Items.Add(item);

            Maker();
            return this;
        }
        public Mosaic AddNeighbor(Screen screenInMosaic, Screen screenNew, DirectionEnum direction)
        {
            if (!ExistItem(screenInMosaic))
            {
                throw new ApplicationException("Screen is not in mosaic");
            }

            //TODO: Add(screenInMosaic, screenNew, direction);
            var itemInMosaic = GetItem(screenInMosaic);
            //if (itemInMosaic.NeighborRight!=null)
            //{
            //    //Clean tree node
            //}

            var itemNew = itemInMosaic.AddNeighbor(screenNew, direction);


            if (itemNew!=null)
            {
                Items.Add(itemNew);
            }

            Maker();

            return this;
        }

       
        public Mosaic Resize(Screen screen, int width, int height)
        {
            foreach (var item in Items)
            {
                if (item.Screen.Id == screen.Id)
                {
                    item.Resize(screen, width, height);
                    break;
                }
            }

            Maker();
            return this;
        }

        /// <summary>
        /// Make big screen.. the mosaiker
        /// </summary>
        private void Maker() {
            var maker = new Maker(Items);
            maker.Init();

            _dimension.Width = maker.Dimension.Width;
            _dimension.Height = maker.Dimension.Height;
        }


        public List<Screen> IsIn(Element element) {
            var screens = new List<Screen>();

            foreach (var item in Items)
            {
                if (item.DimensionMosaic.IntersectsWith(element.Dimension))
                {
                    screens.Add(item.Screen);
                }
            }

            return screens;
        }

        /// <summary>
        /// Get location into Mosaic
        /// </summary>
        /// <param name="element"></param>
        /// <param name="screen"></param>
        /// <returns></returns>
        public Point WhereAreYou(Element element, Screen screen)
        {
            var location = new Point(element.Dimension.X, element.Dimension.Y);
            foreach (var item in Items)
            {
               
                if (item.Screen.Id == screen.Id)
                {
                    location.Y += item.DimensionMosaic.Y;
                    break;
                }

                location.X += item.DimensionMosaic.Width;
                

            }

            return location;
        }

        /// <summary>
        /// Location in Mosaic transpolation to Screen
        /// TODO: Name WhereAreYouInScreen
        /// </summary>
        /// <param name="element"></param>
        /// <param name="screen"></param>
        /// <returns></returns>
        public Point MosaicToScreenLocation(Element element, Screen screen)
        {
            Point location;
            int width = 0;
            int height = 0;

            foreach (var item in Items)
            {

                

                if (item.Screen.Dimension.Height > height)
                {
                    height = item.Screen.Dimension.Height;
                }


                if (item.Screen.Id == screen.Id)
                {
                    location = new Point(0, 0);
                    location.X = element.Dimension.X - width;
                    location.Y = element.Dimension.Y; //TODO: Warning multi linea screen  mosaic
                    break;
                }

                width += item.Screen.Dimension.Width;

            }

            return location;
        }


        public IMosaicItem GetItem(Screen screen)
        {
            IMosaicItem itemResult = null;
            foreach (var item in Items)
            {
                if (item.Screen.Id == screen.Id)
                {
                    itemResult = item;
                    break;
                }
            }

            return itemResult;
        }


        public IMosaicItem GetItem(string screenId)
        {
            IMosaicItem itemResult = null;
            foreach (var item in Items)
            {
                if (item.Screen.Id.ToString() == screenId)
                {
                    itemResult = item;
                    break;
                }
            }

            return itemResult;
        }

        public bool ExistItem(Screen screen)
        {
            return ExistItem(screen.Id.ToString());
        }

        public bool ExistItem(string screenId)
        {
            return Items.Any(s => s.Screen.Id.ToString() == screenId);
        }

        public bool ExistItemByIdShort(string screenIdShort)
        {
            return Items.Any(s => s.Screen.IdShort == screenIdShort);
        }

    }
}
