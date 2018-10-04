using Mosaix.Entities;
using Ogyke.Core.Enumerations;
using Ogyke.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ogyke.Core.Entities
{
    public class MatchResult : IMatchResult
    {
        public IMosaicItem ItemFather { get; set; }
        public IMosaicItem ItemSon { get; set; }
        public DirectionEnum Direction { get; set; }
        public bool IsMatch { get; set; }

        public NeighborStatus ItemFatherNeighborStatus {
            get {
                var status = new NeighborStatus()
                {
                    HasANeighborBottom = ItemFather.HasANeighborBottom,
                    HasANeighborLeft = ItemFather.HasANeighborLeft,
                    HasANeighborRight = ItemFather.HasANeighborRight,
                    HasANeighborTop = ItemFather.HasANeighborTop,
                };

                return status;
            }
        }
        public NeighborStatus ItemSonNeighborStatus {
            get
            {
                var status = new NeighborStatus()
                {
                    HasANeighborBottom = ItemSon.HasANeighborBottom,
                    HasANeighborLeft = ItemSon.HasANeighborLeft,
                    HasANeighborRight = ItemSon.HasANeighborRight,
                    HasANeighborTop = ItemSon.HasANeighborTop,
                };

                //Fix a Neighbor with direction
                switch (Direction)
                {
                    case DirectionEnum.Up:
                        status.HasANeighborBottom = true;
                        break;
                    case DirectionEnum.Right:
                        status.HasANeighborLeft = true;
                        break;
                    case DirectionEnum.Down:
                        status.HasANeighborTop = true;
                        break;
                    case DirectionEnum.Left:
                        status.HasANeighborRight = true;
                        break;
                    default:
                        break;
                }

                return status;
            }
        }


    }
}
