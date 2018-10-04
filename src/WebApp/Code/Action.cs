using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Mosaix.Entities;
using Mosaix.Helpers;
using Ogyke.Core;
using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Code
{
    public class Action: Hub
    {
        private MosaicStoreManager _mosaicStoreManager;

        public Action(MosaicStoreManager mosaicStoreManager)
        {
            _mosaicStoreManager = mosaicStoreManager;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task Init(string screenId, int width, int height)
        {
            var mosaicStore = _mosaicStoreManager.MosaicStore;

            var mosaic = mosaicStore.CreateOrUpdate(screenId, 
                                                    width, 
                                                    height, 
                                                    Context.ConnectionId);

            var screenIdToHuman = GuidHelper.Shorter(screenId);

            var message = $"[Init]  {Context.ConnectionId} > Create Mosaic {mosaic.Id} > Screen {screenIdToHuman} > dimension {width} x {height}";
            await Log(message);

            var mosaicMessage = $"[Mosaix] {MosaicApp.Mosaic.Items.Count} items > dimension {MosaicApp.Mosaic.Dimension.Width} x {MosaicApp.Mosaic.Dimension.Height}";
            await Log(mosaicMessage);
        }

        public async Task Match(string screenIdFrom, string screenIdTo, DirectionEnum direction)
        {
            var mosaicStore = _mosaicStoreManager.MosaicStore;

            var matchResult = mosaicStore.Match(screenIdFrom, screenIdTo, direction);
            var mosaic = mosaicStore.GetByScreenId(screenIdFrom);

            var message = $"[Match] Error";
            if (matchResult.IsMatch)
            {
                var directionToHuman = EnumHelper.GetEnumDescription(direction);
                var screenIdFromToHuman = GuidHelper.Shorter(screenIdFrom);
                var screenIdToToHuman = GuidHelper.Shorter(screenIdTo);
                message = $"[Match] {Context.ConnectionId} > Create Match > Screen {screenIdFromToHuman} > to > {screenIdToToHuman} > direction {directionToHuman}";
            }
            await Log(message);

            
            var mosaicMessage = $"[Mosaix] {mosaic.Items.Count} items > dimension {mosaic.Dimension.Width} x {mosaic.Dimension.Height}";
            await Log(mosaicMessage);
        }


        public async Task Swipe(string screenId, DirectionEnum direction)
        {
            var mosaicStore = _mosaicStoreManager.MosaicStore;
            var swipeRelayer = _mosaicStoreManager.SwipeRelayer;


            var action = swipeRelayer.Add(screenId, direction);

            var directionToHuman = EnumHelper.GetEnumDescription(direction);
            var screenIdToHuman = GuidHelper.Shorter(screenId);

            var message = $"[Match] Swipe action > ScreenId {screenIdToHuman} > direction {directionToHuman}";
            await Log(message);

            var matchResult = swipeRelayer.Match(action, 5000);
            var matchMessage = string.Empty;

            if (matchResult.IsMatch)
            {
                mosaicStore.ClearAppsEmpty();

                matchMessage = $"[Mosaix] Match > {matchResult.IsMatch} > Father (Screen {matchResult.ItemFather.Screen.ConnectionId}) > Son (Screen) {matchResult.ItemSon.Screen.ConnectionId}";

                await Clients.Client(matchResult.ItemFather.Screen.ConnectionId.ToString()).SendAsync("match", message);
                await Clients.Client(matchResult.ItemSon.Screen.ConnectionId.ToString()).SendAsync("match", message);
            }
            if (!string.IsNullOrEmpty(matchMessage))
            {
                await Log(matchMessage);
            }
            
        }

        
        public async Task SwipeDirect(SwipeActionToScreen swipeActionToScreen)
        {
            var mosaicStore = _mosaicStoreManager.MosaicStore;
            var swipeRelayer = _mosaicStoreManager.SwipeRelayer;

            var directionToHuman = EnumHelper.GetEnumDescription(swipeActionToScreen.Direction);
            var screenIdToHuman = GuidHelper.Shorter(swipeActionToScreen.ScreenId);

            var message = $"[Match] Swipe Direct > ScreenId {screenIdToHuman} > To {swipeActionToScreen.ToScreenIdShort} > direction {directionToHuman}";

            await Log(message);

            var matchResult = swipeRelayer.Match(swipeActionToScreen);

            var matchMessage = string.Empty;

            if (matchResult.IsMatch)
            {
                mosaicStore.ClearAppsEmpty();

                matchMessage = $"[Mosaix] Match > {matchResult.IsMatch} > Father (Screen {matchResult.ItemFather.Screen.ConnectionId}) > Son (Screen) {matchResult.ItemSon.Screen.ConnectionId}";

             
                await Clients.Client(matchResult.ItemFather.Screen.ConnectionId.ToString())
                        .SendAsync("match", matchResult.ItemFatherNeighborStatus);

                await Clients.Client(matchResult.ItemSon.Screen.ConnectionId.ToString())
                        .SendAsync("match", matchResult.ItemSonNeighborStatus);
            }

            if (!string.IsNullOrEmpty(matchMessage))
            {
                await Log(matchMessage);
            }

        }



        public async Task Resize(string screenId, int width, int height)
        {
            //MosaicApp.Add(Resize, width, height);
            var screenIdToHuman = GuidHelper.Shorter(screenId);
            var message = $"[Init] {Context.ConnectionId} > Screen {screenIdToHuman} > dimension {width} x {height}";
            await Log(message);
        }

        public async Task Log(string message)
        {
            
            await Clients.Groups("Log").SendAsync("log", message);
        }

        public async Task DragTo(string screenId, int x, int y)
        {
            var mosaicStore = _mosaicStoreManager.MosaicStore;

            //await Log($"[DragTo][Origin] ScreenId {screenIdToHuman} > Location x:{x} y:{y}");
            var mosaic = mosaicStore.GetByScreenId(screenId);
            var element = new Element(80, 80);
            element.Move(x, y);

            //Location
            var mosaicLocation = mosaic.WhereAreYou(element, mosaic.GetItem(screenId).Screen);
            var elementInMosaic = new Element(80, 80);
            elementInMosaic.Move(mosaicLocation.X, mosaicLocation.Y);
            var screenIdToHuman = GuidHelper.Shorter(screenId);

            await Log($"[DragTo][Origin] ScreenId {screenIdToHuman} > Location x:{x} y:{y} > Mosaic Location x:{mosaicLocation.X} y:{mosaicLocation.Y}");

            foreach (var item in mosaic.Items)
            {
                var locationInScreen = mosaic.MosaicToScreenLocation(elementInMosaic, item.Screen);
                await Log($"[DragTo] Screen > {item.Screen.Id} > Location x:{ locationInScreen.X} y: { locationInScreen.Y}");

                if (Context.ConnectionId != item.Screen.ConnectionId.ToString())
                {
                    //Only Screen in... 
                    var moveToItem = new MoveToItem()
                    {
                        ScreenIdOrigin = screenId,
                        X = locationInScreen.X,
                        Y = locationInScreen.Y
                    };
                    await Clients.Client(item.Screen.ConnectionId.ToString()).SendAsync("dragTo", moveToItem);
                }
            }

            //foreach (var s in screens)
            //{
            //    var locationInScreen = MosaicApp.Mosaic.MosaicToScreenLocation(elementInMosaic, s);
            //    await Log($"[WhereAreYou] Screen > {s.Id} > Location x:{ locationInScreen.X} y: { locationInScreen.Y}");

            //    //var item = MosaicApp.Mosaic.GetItem(s);

            //    if (Context.ConnectionId != s.ConnectionId.ToString())
            //    {

            //        //await Log($"[DragTo] ScreenId {screenIdToHuman} > Location x:{locationInScreen.X} y:{locationInScreen.Y}");

            //        //Only Screen in... 
            //        await Clients.Client(s.ConnectionId.ToString()).InvokeAsync("dragTo", locationInScreen.X, locationInScreen.Y);

            //    }

            //}

            //await Clients.AllExcept(new List<string> { Context.ConnectionId }).InvokeAsync("dragTo", x, y);
        }


        public async Task JoinLog()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Log");
        }
    }
}
