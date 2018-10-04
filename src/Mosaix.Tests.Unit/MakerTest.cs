using Ogyke.Core;
using Ogyke.Core.Entities;
using Ogyke.Core.Enumerations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Mosaix.Tests.Unit
{
    public class MakerTest
    {
        [Fact]
        public void Should_AddScreenRight_When_ScreenNeighborRightExist()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
           
            var maker = new Maker(mosaic.Items);
            maker.Init();


            Assert.NotNull(mosaic);
            Assert.Equal(2, mosaic.Items.Count);
            Assert.Equal(200, maker.Dimension.Width);
            Assert.Equal(200, maker.Dimension.Height);
        }


        [Fact]
        public void Should_AddScreen3Right_When_Screen3NeighborRightExist()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 300);
            var screen3 = new Screen(200, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            var maker = new Maker(mosaic.Items);
            maker.Init();


            Assert.NotNull(mosaic);
            Assert.Equal(3, mosaic.Items.Count);
            Assert.Equal(400, maker.Dimension.Width);
            Assert.Equal(300, maker.Dimension.Height);
        }

        [Fact]
        public void Should_AddScreenTop_When_ScreenNeighborTopExist()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Up);

            var maker = new Maker(mosaic.Items);
            maker.Init();


            Assert.NotNull(mosaic);
            Assert.Equal(2, mosaic.Items.Count);
            Assert.Equal(100, maker.Dimension.Width);
            Assert.Equal(400, maker.Dimension.Height);
        }

        [Fact]
        public void Should_AddScreenBottom_When_ScreenNeighborBottomExist()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Down);

            var maker = new Maker(mosaic.Items);
            maker.Init();


            Assert.NotNull(mosaic);
            Assert.Equal(2, mosaic.Items.Count);
            Assert.Equal(100, maker.Dimension.Width);
            Assert.Equal(400, maker.Dimension.Height);
        }

        [Fact]
        public void Should_AddScreenLeft_When_ScreenNeighborLeftExist()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Left);

            var maker = new Maker(mosaic.Items);
            maker.Init();


            Assert.NotNull(mosaic);
            Assert.Equal(2, mosaic.Items.Count);
            Assert.Equal(200, maker.Dimension.Width);
            Assert.Equal(200, maker.Dimension.Height);
        }


        [Fact]
        public void Should_AddNeighborOnlyOne_When_FormASquare()
        {
            var mosaic = new Mosaic();
            var screen0 = new Screen(100, 200);
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);
            var screen3 = new Screen(100, 200);
            var screen4 = new Screen(100, 200);

            mosaic.AddNeighbor(screen0);
            mosaic.AddNeighbor(screen0, screen1, DirectionEnum.Up);
            mosaic.AddNeighbor(screen0, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen0, screen3, DirectionEnum.Down);
            mosaic.AddNeighbor(screen0, screen4, DirectionEnum.Left);

            var maker = new Maker(mosaic.Items);
            maker.Init();


            Assert.NotNull(mosaic);
            Assert.Equal(5, mosaic.Items.Count);
            Assert.Equal(300, maker.Dimension.Width);
            Assert.Equal(600, maker.Dimension.Height);
        }


        [Fact]
        public void Should_AddNeighborTwo_When_FormAStar()
        {
            var mosaic = new Mosaic();
            var screen0 = new Screen(100, 200);
            var screen11 = new Screen(100, 200);
            var screen12 = new Screen(100, 200);
            var screen21 = new Screen(100, 200);
            var screen22 = new Screen(100, 200);
            var screen31 = new Screen(100, 200);
            var screen32 = new Screen(100, 200);
            var screen41 = new Screen(100, 200);
            var screen42 = new Screen(100, 200);

            mosaic.AddNeighbor(screen0);
            mosaic.AddNeighbor(screen0, screen11, DirectionEnum.Up);
            mosaic.AddNeighbor(screen11, screen12, DirectionEnum.Up);
            mosaic.AddNeighbor(screen0, screen21, DirectionEnum.Right);
            mosaic.AddNeighbor(screen21, screen22, DirectionEnum.Right);
            mosaic.AddNeighbor(screen0, screen31, DirectionEnum.Down);
            mosaic.AddNeighbor(screen31, screen32, DirectionEnum.Down);
            mosaic.AddNeighbor(screen0, screen41, DirectionEnum.Left);
            mosaic.AddNeighbor(screen41, screen42, DirectionEnum.Left);

            var maker = new Maker(mosaic.Items);
            maker.Init();


            Assert.NotNull(mosaic);
            Assert.Equal(9, mosaic.Items.Count);
            Assert.Equal(500, maker.Dimension.Width);
            Assert.Equal(1000, maker.Dimension.Height);
        }



        [Fact]
        public void Should_GetNodesTreePath_When_ExistNodesRightOnly()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200);
            var screen2 = new Screen(100, 200);
            var screen3 = new Screen(100, 200);

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screen2, DirectionEnum.Right);
            mosaic.AddNeighbor(screen2, screen3, DirectionEnum.Right);

            var maker = new Maker(mosaic.Items);
            maker.Init();

            var nodes = maker.GetNodesTreePath();

            Assert.NotNull(mosaic);
            Assert.Equal(3, mosaic.Items.Count);
            Assert.NotNull(nodes);
            Assert.Equal(screen1, nodes[0].Screen);
            Assert.Equal(screen2, nodes[1].Screen);
            Assert.Equal(screen3, nodes[2].Screen);
        }


        [Fact]
        public void Should_GetNodesTreePath_When_ExistNodesTopAndRight()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200, "screen1");
            var screenTop1 = new Screen(100, 200, "screenTop1");
            var screenTop2 = new Screen(100, 200, "screenTop2");
            var screenRight1 = new Screen(100, 200, "screenRight1");
            var screenRight2 = new Screen(100, 200, "screenRight2");

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screenRight1, DirectionEnum.Right);
            mosaic.AddNeighbor(screenRight1, screenRight2, DirectionEnum.Right);

            mosaic.AddNeighbor(screen1, screenTop1, DirectionEnum.Up);
            mosaic.AddNeighbor(screenTop1, screenTop2, DirectionEnum.Up);


            var maker = new Maker(mosaic.Items);
            maker.Init();

            var nodes = maker.GetNodesTreePath();

            Assert.NotNull(mosaic);
            Assert.Equal(5, mosaic.Items.Count);
            Assert.NotNull(nodes);
            Assert.Equal(screen1, nodes[0].Screen);

            Assert.Equal(screenTop1, nodes[1].Screen);
            Assert.Equal(screenTop2, nodes[2].Screen);

            Assert.Equal(screenRight1, nodes[3].Screen);
            Assert.Equal(screenRight2, nodes[4].Screen);
        }


        [Fact]
        public void Should_GetDimensionMosaic_When_ExisteRightScreen()
        {
            var mosaic = new Mosaic();
            var screen1 = new Screen(100, 200, "screen1");
            var screenRight1 = new Screen(100, 200, "screenRight1");
            var screenRight2 = new Screen(100, 200, "screenRight2");

            mosaic.AddNeighbor(screen1);
            mosaic.AddNeighbor(screen1, screenRight1, DirectionEnum.Right);
            mosaic.AddNeighbor(screenRight1, screenRight2, DirectionEnum.Right);


            var maker = new Maker(mosaic.Items);
            maker.Init();
            
            Assert.NotNull(mosaic);
            Assert.Equal(3, mosaic.Items.Count);

            Assert.Equal(0, mosaic.Items[0].DimensionMosaic.X);
            Assert.Equal(0, mosaic.Items[0].DimensionMosaic.Y);
            Assert.Equal(screen1.Dimension.Width, mosaic.Items[0].DimensionMosaic.Width);
            Assert.Equal(screen1.Dimension.Height, mosaic.Items[0].DimensionMosaic.Height);

            Assert.Equal(100, mosaic.Items[1].DimensionMosaic.X);
            Assert.Equal(0, mosaic.Items[1].DimensionMosaic.Y);
            Assert.Equal(screenRight1.Dimension.Width, mosaic.Items[1].DimensionMosaic.Width);
            Assert.Equal(screenRight1.Dimension.Height, mosaic.Items[1].DimensionMosaic.Height);

            Assert.Equal(200, mosaic.Items[2].DimensionMosaic.X);
            Assert.Equal(0, mosaic.Items[2].DimensionMosaic.Y);
            Assert.Equal(screenRight2.Dimension.Width, mosaic.Items[2].DimensionMosaic.Width);
            Assert.Equal(screenRight2.Dimension.Height, mosaic.Items[2].DimensionMosaic.Height);

        }
    }
}
