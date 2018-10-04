using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using System.Collections.Generic;
using System.Drawing;

namespace Ogyke.Core.Interfaces
{
    public interface IMosaicItem
    {
        Rectangle DimensionMosaic { get; }
        IMosaicItem NeighborBottom { get; }
        IMosaicItem NeighborLeft { get; }
        IMosaicItem NeighborRight { get; }
        IMosaicItem NeighborTop { get; }
        Screen Screen { get; }

        void MoveIntoMosaic(int x, int y);
        void MoveIntoMosaic(Point location);
        void Resize(Screen screen);
        void Resize(Screen screen, int width, int height);
        IMosaicItem GetNeighbor(DirectionEnum direction);
        
        IMosaicItem Clean(DirectionEnum direction);

        IMosaicItem AddNeighbor(IMosaicItem neighbor, DirectionEnum direction);
        IMosaicItem AddNeighbor(Screen screen, DirectionEnum direction);

        List<IMosaicItem> GetNeighbors();

        Rectangle DimensionTop(bool includeMe);
        Rectangle DimensionRight(bool includeMe);
        Rectangle DimensionBottom(bool includeMe);
        Rectangle DimensionLeft(bool includeMe);

        bool HasANeighborBottom { get; }
        bool HasANeighborLeft { get; }
        bool HasANeighborRight { get; }
        bool HasANeighborTop { get; }
    }
}