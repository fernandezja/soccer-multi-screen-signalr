using Ogyke.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogyke.Core
{
    public class MosaicApp
    {
        private static Mosaic _mosaic;
        public static Mosaic Mosaic {
            get {
                if (_mosaic==null)
                {
                    _mosaic = new Mosaic();
                }
                return _mosaic;
            }
        }

        public static Screen Add(string screenId, int width, int height, string connectionId) {

            var id = Guid.Parse(screenId);
            Screen screen = null;

            foreach (var item in Mosaic.Items)
            {
                if (item.Screen.Id==id)
                {
                    screen = new Screen(id, width, height);
                    screen.SetConnectionId(connectionId);
                    item.Resize(screen);
                    break;
                }
            }

            if (screen==null)
            {
                screen = new Screen(id, width, height);
                screen.SetConnectionId(connectionId);
                Mosaic.AddNeighbor(screen);
            }

            return screen;
        }



        public static Screen Add(Screen screen)
        {
            return Add(screen.Id.ToString(),
                    screen.Dimension.Width,
                    screen.Dimension.Height,
                    screen.ConnectionId.ToString());
        }


        public static List<Screen> WhereAreYou(string screenId, int x, int y) {
            var element = new Element(80, 80);
            element.Move(x, y);

            var id = Guid.Parse(screenId);
            Screen screen = null;

            foreach (var item in Mosaic.Items)
            {
                if (item.Screen.Id == id)
                {
                    screen = item.Screen;
                    break;
                }
            }


            var locationOnMosaic = Mosaic.WhereAreYou(element, screen);
            element.Move(locationOnMosaic);

            var screens = Mosaic.IsIn(element);


            return screens;
        }


        public static void Clear()
        {
            _mosaic = null;
        }
    }
}
