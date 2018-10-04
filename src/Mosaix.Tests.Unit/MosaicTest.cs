using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Mosaix.Tests.Unit
{
    public class MosaicTest
    {
        [Fact]
        public void Should_AddScreeen_When_AddOneScreeen()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(200, 200);
            mosaic.AddNeighbor(screen1);

            Assert.NotNull(mosaic);
            Assert.Single(mosaic.Items);
        }

        [Fact]
        public void Should_AddScreeen_When_AddTwoScreeen()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(200, 200);
            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);

            Assert.NotNull(mosaic);
            Assert.Equal(2, mosaic.Items.Count);
        }

        [Fact]
        public void Should_AddScreenNeighbor_When_AddScreenNeighborRight()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);

            Assert.NotNull(mosaic);
            Assert.Equal(2, mosaic.Items.Count);
        }

        [Fact]
        public void Should_ExistItemTrue_When_AScreenIsInMosaic()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            mosaic.AddNeighbor(screen1);
            var exist = mosaic.ExistItem(screen1);

            Assert.Single(mosaic.Items);
            Assert.True(exist);
        }

        [Fact]
        public void Should_ExistItemFalse_When_AScreenIsNotInMosaic()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);
            mosaic.AddNeighbor(screen1);
            var exist = mosaic.ExistItem(screen2);

            Assert.Single(mosaic.Items);
            Assert.False(exist);
        }

        [Fact]
        public void Should_GetDimensionCorrect_When_AddTwoScreeenSameHeight()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);

            Assert.NotNull(mosaic);
            Assert.Equal(2, mosaic.Items.Count);
            Assert.Equal(400, mosaic.Dimension.Width);
            Assert.Equal(200, mosaic.Dimension.Height);
        }

        [Fact]
        public void Should_GetDimensionCorrect_When_AddThreeScreeenDistinctHeight()
        {
            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            Assert.NotNull(mosaic);
            Assert.Equal(3, mosaic.Items.Count);
            Assert.Equal(500, mosaic.Dimension.Width);
            Assert.Equal(300, mosaic.Dimension.Height);

            Assert.Equal(0, mosaic.Items[0].DimensionMosaic.X);
            Assert.Equal(0, mosaic.Items[0].DimensionMosaic.Y);
            Assert.Equal(screen1.Dimension.Width, mosaic.Items[0].DimensionMosaic.Width);
            Assert.Equal(screen1.Dimension.Height, mosaic.Items[0].DimensionMosaic.Height);

            Assert.Equal(200, mosaic.Items[1].DimensionMosaic.X);
            Assert.Equal(0, mosaic.Items[1].DimensionMosaic.Y);
            Assert.Equal(screen2.Dimension.Width, mosaic.Items[1].DimensionMosaic.Width);
            Assert.Equal(screen2.Dimension.Height, mosaic.Items[1].DimensionMosaic.Height);

            Assert.Equal(300, mosaic.Items[2].DimensionMosaic.X);
            Assert.Equal(0, mosaic.Items[2].DimensionMosaic.Y);
            Assert.Equal(screen3.Dimension.Width, mosaic.Items[2].DimensionMosaic.Width);
            Assert.Equal(screen3.Dimension.Height, mosaic.Items[2].DimensionMosaic.Height);
        }

        [Fact]
        public void Should_GetScreens_When_AElementIsInScreen()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            element.Move(200, 50);

            var result = mosaic.IsIn(element);
            Assert.NotNull(mosaic);
            Assert.Single(result);
            Assert.Equal<Guid>(screen2.Id, result[0].Id);
        }


        [Fact]
        public void Should_GetScreens_When_AElementIsInScreenLast()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            element.Move(350, 150);

            var result = mosaic.IsIn(element);
            Assert.NotNull(mosaic);
            Assert.Single(result);
            Assert.Equal<Guid>(screen3.Id, result[0].Id);
        }


        [Fact]
        public void Should_GetScreensEmpty_When_AElementIsNotAnyScreen()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            element.Move(999, 999);

            var result = mosaic.IsIn(element);
            Assert.NotNull(mosaic);
            Assert.Empty(result);
        }


        [Fact]
        public void Should_GetElementLocation_When_AElementMoveIntoScreenItemOne()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            //Move element Screen 2 > Location > 20:100
            element.Move(75, 75);

            var result = mosaic.WhereAreYou(element, screen1);
            Assert.Equal<int>(75, result.X);
            Assert.Equal<int>(75, result.Y);
        }

        [Fact]
        public void Should_GetElementLocation_When_AElementMoveIntoScreenItemSecond()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            //Move element Screen 2 > Location > 20:100
            element.Move(20, 100);

            var result = mosaic.WhereAreYou(element, screen2);
            Assert.Equal<int>(220, result.X);
            Assert.Equal<int>(100, result.Y);
        }


        [Fact]
        public void Should_GetElementLocation_When_AElementMoveIntoScreenItemThree()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            //Move element Screen 2 > Location > 20:100
            element.Move(175, 75);

            var result = mosaic.WhereAreYou(element, screen3);
            Assert.Equal<int>(475, result.X);
            Assert.Equal<int>(75, result.Y);
        }


        [Fact]
        public void Should_GetAddScreen_When_AddScreen()
        {
            var element = new Element(20, 20);

            var mosaic = new Mosaic();

            var screens = new List<Screen>();

            for (int i = 1; i < 100; i++)
            {
                screens.Add(new Screen(300, 300));
            }

            mosaic.AddNeighbor(screens[0]); //First screen node
            for (int i = 1; i < screens.Count; i++)
            {
                mosaic.AddNeighbor(screens[i-1], screens[i], DirectionEnum.Right);
            }
          


            //Move element Screen 3 > Location > 620:100
            element.Move(620, 100);
            var result = mosaic.WhereAreYou(element, screens[2]);

            Assert.Equal<int>(screens.Count, mosaic.Items.Count);
            Assert.Equal<int>(screens.Count*300, mosaic.Dimension.Width);

            Assert.Equal<int>(1220, result.X);
            Assert.Equal<int>(100, result.Y);
        }




        [Theory]
        [InlineData(15, 15, 15, 15, 0)]
        [InlineData(50, 100, 50, 100, 0)]
        [InlineData(250, 100, 50, 100, 1)]
        [InlineData(230, 30, 30, 30, 1)]
        [InlineData(310, 100, 10, 100, 2)]
        public void Should_MosaicToScreenLocation_When_AElementMoveIntoScreen(
            int mosaicX, int mosaicY, int screenX, int screenY, int screenIndex)
        {
            var element = new Element(50, 50);

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 200);
            var screen3 = new Screen(200, 200);

            var mosaic = new Mosaic();

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            //Move element Screen 2 > Location > 250:100
            element.Move(mosaicX, mosaicY);

            var result = mosaic.MosaicToScreenLocation(element, mosaic.Items[screenIndex].Screen);
            Assert.Equal<int>(screenX, result.X);
            Assert.Equal<int>(screenY, result.Y);
        }

        
        [Theory]
        [InlineData(679, 226, 679, 226, 0)]
        [InlineData(679, 226, -1, 226, 1)]
        public void Should_MosaicToScreenLocation_When_AElementMoveIntoScreen2(
            int mosaicX, int mosaicY, int screenX, int screenY, int screenIndex)
        {
            var element = new Element(80, 80);

            var screen1 = new Screen(680, 627);
            var screen2 = new Screen(682, 627);

            var mosaic = new Mosaic();

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);

            element.Move(mosaicX, mosaicY);

            var result = mosaic.MosaicToScreenLocation(element, mosaic.Items[screenIndex].Screen);
            Assert.Equal<int>(screenX, result.X);
            Assert.Equal<int>(screenY, result.Y);
        }


        [Fact]
        public void Should_GetMosaicItemById_When_ExistItem()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);
            
            var result = mosaic.GetItem(screen2.Id.ToString());

            Assert.NotNull(result);
            Assert.Equal(result.Screen, screen2);
        }


        [Fact]
        public void Should_GetMosaicItemById_When_NotExistItem()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);
            var screen4 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            var result = mosaic.GetItem(screen4.Id.ToString());

            Assert.Null(result);
        }


        [Fact]
        public void Should_GetMosaicItemByScreen_When_ExistItem()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            var result = mosaic.GetItem(screen2);

            Assert.NotNull(result);
            Assert.Equal(result.Screen, screen2);
        }


        [Fact]
        public void Should_ResizeScreenIntoMosaic_When_ResizeScreen()
        {
            var element = new Element(50, 50);

            var mosaic = new Mosaic();

            var screen1 = new Screen(200, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            var result = mosaic.Resize(screen2, 100, 500);
            var item2Result = mosaic.GetItem(screen2.Id.ToString());

            Assert.NotNull(result);
            Assert.Equal<int>(100, item2Result.Screen.Dimension.Width);
            Assert.Equal<int>(500, item2Result.Screen.Dimension.Height);
        }
    }
}
