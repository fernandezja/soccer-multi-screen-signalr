using Mosaix.Helpers;
using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using Ogyke.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogyke.Core
{
    public class SwipeRelayer
    {
       
        private List<SwipeAction> _actions;
        public List<SwipeAction> Actions
        {
            get
            {
                return GetSwipeRelayer();
            }
        }

        private MosaicStore _mosaicStore;
        public SwipeRelayer SetMosaicStore(MosaicStore value)
        {
            _mosaicStore = value;
            return this;
        }

        private List<SwipeAction> GetSwipeRelayer()
        {
            if (_actions == null)
            {
                _actions = new List<SwipeAction>();
            }
            return _actions;
        }

        public SwipeAction Add(string screenId, DirectionEnum direction)
        {
            var id = Guid.Parse(screenId);
            var action = new SwipeAction(screenId, direction);
            Actions.Add(action);
            return action;
        }

        public void Remove(int milliseconds)
        {
            var now = DateTime.Now;

            Actions.RemoveAll(a => now.Subtract(a.Timestamp).TotalMilliseconds > milliseconds);

        }

        public void Remove(IMatchResult matchResult)
        {
            if (!matchResult.IsMatch)
                return; 
           
            Actions.RemoveAll(a => a.ScreenId == matchResult.ItemFather.Screen.Id.ToString());

            Actions.RemoveAll(a => a.ScreenId == matchResult.ItemSon.Screen.Id.ToString());

            
        }
        

        public IMatchResult Match(SwipeAction action, int milliseconds)
        {
            IMatchResult matchResult = null;
            var actionOpposite = GetOppositeAction(action);


            if (actionOpposite != null)
            {
                SwipeAction actionAux;
                //TODO: Thinking order
                if (actionOpposite.Timestamp.Subtract(action.Timestamp).TotalMilliseconds < 0)
                {
                    actionAux = actionOpposite;
                    actionOpposite = action;
                    action = actionAux;
                }

                var mosaic = _mosaicStore.GetByScreenId(action.ScreenId);

                if (mosaic.Items.Count == 1)
                {

                    matchResult = _mosaicStore.Match(action.ScreenId, 
                                                actionOpposite.ScreenId, 
                                                action.Direction);

                    
                }
                else
                {
                    matchResult = _mosaicStore.Match(action.ScreenId,
                                                actionOpposite.ScreenId,
                                                action.Direction);
                }

                if (matchResult.IsMatch)
                {
                    Remove(matchResult);
                }
                
                Remove(milliseconds);
            }

            return matchResult;
        }

        public IMatchResult Match(SwipeActionToScreen swipeActionToScreen)
        {
            IMatchResult matchResult = null;
            
            matchResult = _mosaicStore.Match(swipeActionToScreen);

            if (matchResult.IsMatch)
            {
                _mosaicStore.ClearAppsEmpty();
            }

            return matchResult;
        }

        public SwipeAction GetOppositeAction(SwipeAction action)
        {
            var directionOpposite = DirectionHelper.OppositeDirection(action.Direction);

            var query = from a in Actions
                        where a.Direction == directionOpposite
                             && !a.Id.Equals(action.Id)
                        select a;

            var actionOpposite = query.FirstOrDefault();
            return actionOpposite;
        }

        

       
    }
}
