using Ogyke.Core;
using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mosaix.Tests.Unit
{
    public class SwipeRelayerTest
    {
        public MosaicStore _mosaicStore { get; set; }
        public SwipeRelayer _swipeRelayer { get; set; }
        public SwipeRelayerTest()
        {
            _mosaicStore = new MosaicStore();
            _swipeRelayer = (new SwipeRelayer()).SetMosaicStore(_mosaicStore);
        }


        [Theory]
        [InlineData(DirectionEnum.Right)]
        public void Should_Add_WhenActionSwipeToDirection(DirectionEnum direction)
        {
            _mosaicStore.Clear();

            var screenId1 = Guid.NewGuid();
            var connectionId1 = Guid.NewGuid();
            var screen1 = new Screen(screenId1, 100, 200, connectionId1.ToString());

            var mosaic1 = _mosaicStore.CreateOrUpdate(screen1);


            var action = _swipeRelayer.Add(screen1.Id.ToString(), direction);

            Assert.NotNull(action);
            Assert.Equal<DirectionEnum>(direction, action.Direction);
            Assert.Equal(action.ScreenId, screen1.Id.ToString());

        }

        [Fact]
        public void Should_Match_WhenActionSwipeToDirection()
        {
            _mosaicStore.Clear();

            var screenId1 = Guid.NewGuid();
            var connectionId1 = Guid.NewGuid();
            var screen1 = new Screen(screenId1, 100, 200, connectionId1.ToString());

            var screenId2 = Guid.NewGuid();
            var connectionId2 = Guid.NewGuid();
            var screen2 = new Screen(screenId2, 100, 200, connectionId2.ToString());

            var mosaic1 = _mosaicStore.CreateOrUpdate(screen1);
            var mosaic2 = _mosaicStore.CreateOrUpdate(screen2);


            var action1 = _swipeRelayer.Add(screen1.Id.ToString(), DirectionEnum.Right);

            //TODO: Sleep Time

            var action2 = _swipeRelayer.Add(screen2.Id.ToString(), DirectionEnum.Left);


            var matchResult = _swipeRelayer.Match(action2, 1000);

            Assert.NotNull(matchResult);
            Assert.True(matchResult.IsMatch);
            Assert.Equal(screen1.Id.ToString(), matchResult.ItemFather.Screen.Id.ToString());
            Assert.Equal(screen2.Id.ToString(), matchResult.ItemSon.Screen.Id.ToString());
            Assert.Equal<DirectionEnum>(DirectionEnum.Right, matchResult.Direction);

            Assert.Single(_mosaicStore.Apps);


        }


        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Match_WhenActionSwipeToDirectionAllScreenInChain(int screenToAdd)
        {
            var mosaics = new List<Mosaic>();
            var screens = new List<Screen>();
            var screensNew = new List<Screen>();

            for (int i = 1; i <= screenToAdd; i++)
            {
                var screenId = Guid.NewGuid();
                var connectionId = Guid.NewGuid();
                var screen = new Screen(screenId, 100, 200, connectionId.ToString());
                var mosaic = _mosaicStore.CreateOrUpdate(screen);

                mosaics.Add(mosaic);
                screens.Add(screen);
            }

            for (int i = 0; i < screenToAdd-1; i++)
            {
                var fatherScreen = screens[i];
                var sonScreen = screens[i+1];

                var action1Father = _swipeRelayer.Add(fatherScreen.Id.ToString(), 
                                    DirectionEnum.Right);

                //TODO: Sleep Time

                var action2Son = _swipeRelayer.Add(sonScreen.Id.ToString(), 
                                            DirectionEnum.Left);


                var matchResult = _swipeRelayer.Match(action2Son, 20000);


                Assert.NotNull(matchResult);
                Assert.True(matchResult.IsMatch);
                Assert.Equal(fatherScreen.Id.ToString(), matchResult.ItemFather.Screen.Id.ToString());
                Assert.Equal(sonScreen.Id.ToString(), matchResult.ItemSon.Screen.Id.ToString());
                Assert.Equal<DirectionEnum>(DirectionEnum.Right, matchResult.Direction);

            }

            Assert.Single(_mosaicStore.Apps);

        }



        [Fact]
        public void Should_Match_WhenActionSwipeToScreen()
        {
            _mosaicStore.Clear();

            var screenId1 = Guid.NewGuid();
            var connectionId1 = Guid.NewGuid();
            var screen1 = new Screen(screenId1, 100, 200, connectionId1.ToString());

            var mosaic1 = _mosaicStore.CreateOrUpdate(screen1);

            var screenId2 = Guid.NewGuid();
            var connectionId2 = Guid.NewGuid();
            var screen2 = new Screen(screenId2, 100, 200, connectionId2.ToString());

            var mosaic2 = _mosaicStore.CreateOrUpdate(screen2);

            var swipeActionToScreen = new SwipeActionToScreen(screenId1.ToString(), 
                        screen2.IdShort, 
                        DirectionEnum.Right);


            var matchResult = _swipeRelayer.Match(swipeActionToScreen);

            Assert.NotNull(matchResult);
            Assert.True(matchResult.IsMatch);
            Assert.Equal(screen1.Id.ToString(), matchResult.ItemFather.Screen.Id.ToString());
            Assert.Equal(screen2.Id.ToString(), matchResult.ItemSon.Screen.Id.ToString());
            Assert.Equal<DirectionEnum>(DirectionEnum.Right, matchResult.Direction);

        }


        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Match_WhenActionSwipeToScreenInChain(int screenToAdd)
        {
            var mosaics = new List<Mosaic>();
            var screens = new List<Screen>();
            var screensNew = new List<Screen>();

            for (int i = 1; i <= screenToAdd; i++)
            {
                var screenId = Guid.NewGuid();
                var connectionId = Guid.NewGuid();
                var screen = new Screen(screenId, 100, 200, connectionId.ToString());
                var mosaic = _mosaicStore.CreateOrUpdate(screen);

                mosaics.Add(mosaic);
                screens.Add(screen);
            }

            for (int i = 0; i < screenToAdd - 1; i++)
            {
                var fatherScreen = screens[i];
                var sonScreen = screens[i + 1];


                var swipeActionToScreen = new SwipeActionToScreen(fatherScreen.Id.ToString(),
                       sonScreen.IdShort,
                       DirectionEnum.Right);


                var matchResult = _swipeRelayer.Match(swipeActionToScreen);

                Assert.NotNull(matchResult);
                Assert.True(matchResult.IsMatch);
                Assert.Equal(fatherScreen.Id.ToString(), matchResult.ItemFather.Screen.Id.ToString());
                Assert.Equal(sonScreen.Id.ToString(), matchResult.ItemSon.Screen.Id.ToString());
                Assert.Equal<DirectionEnum>(DirectionEnum.Right, matchResult.Direction);

            }

            Assert.Single(_mosaicStore.Apps);

        }

    }
}
