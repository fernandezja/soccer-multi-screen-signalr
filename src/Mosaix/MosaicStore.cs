using Ogyke.Core;
using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using Ogyke.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogyke.Core
{
    public class MosaicStore
    {
        
        private List<Mosaic> _apps;

        public List<Mosaic> Apps
        {
            get
            {
                return GetStore();
            }
        }
        
        private List<Mosaic> GetStore()
        {
            if (_apps == null)
            {
                _apps = new List<Mosaic>();
            }
            return _apps;
        }

        public Mosaic Create(string screenId, int width, int height, string connectionId)
        {
            var id = Guid.Parse(screenId);
            Screen screen = null;

            var mosaic = new Mosaic();

            if (screen == null)
            {
                screen = new Screen(id, width, height);
                screen.SetConnectionId(connectionId);
                mosaic.AddNeighbor(screen);
                Apps.Add(mosaic);
            }

            return mosaic;
        }

        public Mosaic Create(Screen screen) {
            return Create(screen.Id.ToString(), 
                            screen.Dimension.Width, 
                            screen.Dimension.Height, 
                            screen.ConnectionId);
        }

        public IMatchResult Match(
            string screenIdFrom, string screenIdTo, DirectionEnum direction)
        {
            var mosaic = GetByScreenId(screenIdFrom);
            var item = mosaic.GetItem(screenIdFrom);

            var mosaicOld = GetByScreenId(screenIdTo);
            var itemOld = mosaicOld.GetItem(screenIdTo);

            mosaic.AddNeighbor(item.Screen, itemOld.Screen, direction);

            RemoveScreen(screenIdTo);

            var result = new MatchResult()
            {
                ItemFather = item,
                ItemSon = itemOld,
                Direction = direction,
                IsMatch = true
            };

            return result;
        }


        public IMatchResult Match(SwipeActionToScreen swipeActionToScreen)
        {
            var mosaicItem = GetItemByScreenIdShort(swipeActionToScreen.ToScreenIdShort.ToLower());

            return Match(swipeActionToScreen.ScreenId,
                            mosaicItem.Screen.Id.ToString(),
                            swipeActionToScreen.Direction);
        }

        public DirectionEnum DirectionToEnum(int direction)
        {
            switch (direction)
            {
                case 2:
                    return DirectionEnum.Left;
                case 4:
                    return DirectionEnum.Right;
                case 8:
                    return DirectionEnum.Up;
                case 16:
                    return DirectionEnum.Down;
            }

            throw new ApplicationException("Direction not found");
        }

        public Mosaic GetByScreenId(string screenId)
        {
            var id = Guid.Parse(screenId);
            foreach (var mosaic in Apps)
            {
                if (mosaic.ExistItem(screenId))
                {
                    return mosaic;
                }
            }
            return null;
        }

        public Mosaic GetByScreenIdShort(string screenIdShort)
        {
            foreach (var mosaic in Apps)
            {
                if (mosaic.ExistItemByIdShort(screenIdShort))
                {
                    return mosaic;
                }
            }
            return null;
        }

        public IMosaicItem GetItemByScreenIdShort(string screenIdShort)
        {

            foreach (var mosaic in Apps)
            {
                var query = from m in mosaic.Items
                            where m.Screen.IdShort.ToLower() == screenIdShort.ToLower()
                            select m;

                if (query.Any())
                {
                    return query.FirstOrDefault();
                }

            }
            return null;
        }


        public Mosaic CreateOrUpdate(string screenId, int width, int height, string connectionId)
        {
            var id = Guid.Parse(screenId);
            Mosaic mosaic = GetByScreenId(screenId);
            if (mosaic == null)
            {
                //Create new mosaic
                mosaic = Create(screenId, width, height, connectionId);
            }
            else
            {
                //Update screen into mosaic
                foreach (var item in mosaic.Items)
                {
                    if (item.Screen.Id.Equals(id))
                    {
                        var screenNew = new Screen(id, width, height);
                        screenNew.SetConnectionId(connectionId);
                        item.Resize(screenNew);
                        break;
                    }
                }
            }

            return mosaic;
        }


        public Mosaic CreateOrUpdate(Screen screen) {
            return CreateOrUpdate(screen.Id.ToString(),
                                    screen.Dimension.Width,
                                    screen.Dimension.Height,
                                    screen.ConnectionId);
        }

        public void RemoveScreen(string screenId)
        {
            var id = Guid.Parse(screenId);
            Mosaic mosaic = null;
            foreach (var m in Apps)
            {
                if (m.ExistItem(screenId))
                {
                    mosaic = m;
                }
            }

            if (mosaic != null)
            {
                var item = mosaic.GetItem(screenId);
                mosaic.Items.Remove(item);

                if (mosaic.Items.Any())
                {
                    Apps.Remove(mosaic);
                }
            }

            return;
        }

        public void Clear()
        {
            _apps = null;
        }

        public void ClearAppsEmpty()
        {
            Apps.RemoveAll(a => !a.Items.Any());
        }

    }
}
