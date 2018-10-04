

using Mosaix.Entities;
using Ogyke.Core.Enumerations;

namespace Ogyke.Core.Interfaces
{
    public interface IMatchResult
    {
        DirectionEnum Direction { get; set; }
        bool IsMatch { get; set; }
        IMosaicItem ItemFather { get; set; }
        IMosaicItem ItemSon { get; set; }

        NeighborStatus ItemFatherNeighborStatus { get; }
        NeighborStatus ItemSonNeighborStatus { get; }
    }
}