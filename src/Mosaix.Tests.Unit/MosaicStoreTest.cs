using Ogyke.Core;
using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mosaix.Tests.Unit
{
    public class MosaicStoreTest
    {
        public MosaicStore _mosaicStore { get; set; }
        public SwipeRelayer _swipeRelayer { get; set; }
        public MosaicStoreTest()
        {
            _mosaicStore = new MosaicStore();
            _swipeRelayer = (new SwipeRelayer()).SetMosaicStore(_mosaicStore);
        }


        [Fact]
        public void Should_CreateStore_WhenPassScreen()
        {
            _mosaicStore.Clear();

            var screenId1 = Guid.NewGuid();
            var connectionId1 = Guid.NewGuid();
            var screen1 = new Screen(screenId1, 100, 200, connectionId1.ToString());

            var result = _mosaicStore.Create(screen1);

            Assert.NotNull(result);
            Assert.Single(_mosaicStore.Apps);
        }


        [Fact]
        public void Should_CreateStore_WhenPassValuesOfScreen()
        {
            _mosaicStore.Clear();

            var screenId1 = Guid.NewGuid();
            var connectionId1 = Guid.NewGuid();
            var screen1 = new Screen(screenId1, 100, 200, connectionId1.ToString());

            var result = _mosaicStore.Create(screen1);

            Assert.NotNull(result);
            Assert.Single(_mosaicStore.Apps);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        public void Should_CreateStore_WhenPassManyScreen(int screenToAdd)
        {
            _mosaicStore.Clear();
            for (int i = 1; i <= screenToAdd; i++)
            {
                var screenId = Guid.NewGuid();
                var connectionId = Guid.NewGuid();
                var screen = new Screen(screenId, 100, 200, connectionId.ToString());
                _mosaicStore.Create(screen);
            }

            Assert.Equal<int>(screenToAdd, _mosaicStore.Apps.Count);
        }



        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_CreateOrUpdate_WhenPassScreenFirstTime(int screenToAdd)
        {
            _mosaicStore.Clear();
            for (int i = 1; i <= screenToAdd; i++)
            {
                var screenId = Guid.NewGuid();
                var connectionId = Guid.NewGuid();
                var screen = new Screen(screenId, 100, 200, connectionId.ToString());
                _mosaicStore.CreateOrUpdate(screen);
            }

            Assert.Equal<int>(screenToAdd, _mosaicStore.Apps.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(200)]
        [InlineData(500)]
        [InlineData(1500)]
        public void Should_CreateOrUpdate_WhenPassScreenAgainWithRedimension(int screenToAdd)
        {
            _mosaicStore.Clear();

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

            var mosaicLastScreen1 =
                _mosaicStore.GetByScreenId(screens[screenToAdd - 1].Id.ToString());

            mosaics.Clear();

            foreach (var screen in screens)
            {
                var screenNew = new Screen(screen.Id, 200, 400, screen.ConnectionId);
                var mosaic = _mosaicStore.CreateOrUpdate(screenNew);
                mosaics.Add(mosaic);
                screensNew.Add(screenNew);
            }

            var mosaicLastScreen2 =
                _mosaicStore.GetByScreenId(screensNew[screenToAdd - 1].Id.ToString());

            Assert.Equal<int>(screenToAdd, _mosaicStore.Apps.Count);
            Assert.Equal<Guid>(mosaicLastScreen1.Id, mosaicLastScreen2.Id);
        }

        [Theory]
        [InlineData(2, DirectionEnum.Left)]
        [InlineData(4, DirectionEnum.Right)]
        [InlineData(8, DirectionEnum.Up)]
        [InlineData(16, DirectionEnum.Down)]
        public void Should_DirectionToEnum_WhenPassValueIntDirection(
            int direction, DirectionEnum resultExpected)
        {
            var result = _mosaicStore.DirectionToEnum(direction);

            Assert.Equal<DirectionEnum>(resultExpected, result);
        }

        [Fact]
        public void Should_DirectionToEnumException_WhenPassValueNotDirection()
        {
            Assert.Throws<ApplicationException>(
                () => _mosaicStore.DirectionToEnum(666));

        }

        [Fact]
        public void Should_GetByScreenId_WhenPassScreen()
        {
            _mosaicStore.Clear();
            var mosaics = new List<Mosaic>();
            var screens = new List<Screen>();
            var screenToAdd = 10; // TODO: maybe pass parameters

            for (int i = 1; i <= screenToAdd; i++)
            {
                var screenId = Guid.NewGuid();
                var connectionId = Guid.NewGuid();
                var screen = new Screen(screenId, 100, 200, connectionId.ToString());
                var mosaic = _mosaicStore.Create(screen);

                mosaics.Add(mosaic);
                screens.Add(screen);
            }

            var indexToValidate = 5; //TODO: maybe pass parameters
            var result = _mosaicStore.GetByScreenId(screens[indexToValidate].Id.ToString());

            Assert.Equal<int>(screenToAdd, _mosaicStore.Apps.Count);
            Assert.Equal<Guid>(mosaics[indexToValidate].Id, result.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_RemoveScreen_WhenScreenIntoMosaic(int screenToAdd)
        {
            _mosaicStore.Clear();
            var mosaics = new List<Mosaic>();
            var screens = new List<Screen>();

            for (int i = 1; i <= screenToAdd; i++)
            {
                var screenId = Guid.NewGuid();
                var connectionId = Guid.NewGuid();
                var screen = new Screen(screenId, 100, 200, connectionId.ToString());
                var mosaic = _mosaicStore.CreateOrUpdate(screen);

                mosaics.Add(mosaic);
                screens.Add(screen);
            }

            Assert.Equal<int>(screenToAdd, _mosaicStore.Apps.Count);

            foreach (var screen in screens)
            {
                _mosaicStore.RemoveScreen(screen.Id.ToString());
            }

            Assert.Empty(_mosaicStore.Apps[0].Items);
        }


        [Fact]
        public void Should_Match_WhenPassScreensToMerge()
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

            var result = _mosaicStore.Match(
                            screen1.Id.ToString(),
                            screen2.Id.ToString(),
                            DirectionEnum.Right);

            //Get first item to compare neighbor
            var item1 = mosaic1.Items[0];

            Assert.NotNull(result);
            Assert.Equal(2, _mosaicStore.Apps.Count);    //Todas las apps
            Assert.Equal(2, _mosaicStore.Apps[0].Items.Count); //La app con los screens del mosaico
            Assert.Empty(_mosaicStore.Apps[1].Items); //La app con la antigua screen
            Assert.Equal<int>(2, mosaic1.Items.Count);
            Assert.Equal<Guid>(screen2.Id, item1.NeighborRight.Screen.Id);
        }
    }
}
