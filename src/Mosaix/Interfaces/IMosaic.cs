using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using System.Collections.Generic;
using System.Drawing;

namespace Ogyke.Core.Interfaces
{
    public interface IMosaic
    {
        Rectangle Dimension { get; }
        List<IMosaicItem> Items { get; }
        Mosaic AddNeighbor(Screen screenInMosaic, Screen screenNew, DirectionEnum direction);
        IMosaicItem GetItem(Screen screen);
        IMosaicItem GetItem(string screenId);

        bool ExistItem(Screen screen);
        bool ExistItem(string screenId);
        List<Screen> IsIn(Element element);
        Point MosaicToScreenLocation(Element element, Screen screen);
        Mosaic Resize(Screen screen, int width, int height);
        Point WhereAreYou(Element element, Screen screen);
    }
}