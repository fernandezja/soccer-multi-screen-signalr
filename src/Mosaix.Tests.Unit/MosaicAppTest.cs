using Ogyke.Core;
using Ogyke.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mosaix.Tests.Unit
{
    public class MosaicAppTest
    {
        [Fact]
        public void Should_CreateStore_WhenPassScreen()
        {
            MosaicApp.Clear();

            var screenId1 = Guid.NewGuid();
            var connectionId1 = Guid.NewGuid();
            var screen1 = new Screen(screenId1, 100, 200, connectionId1.ToString());

            var result = MosaicApp.Add(screen1);

            Assert.NotNull(result);
            Assert.NotNull(MosaicApp.Mosaic);
        }


        [Fact]
        public void Should_CreateStore_WhenPassScreenWithNewDimensions()
        {
            MosaicApp.Clear();

            var screenId1 = Guid.NewGuid();
            var connectionId1 = Guid.NewGuid();
            var screen1 = new Screen(screenId1, 100, 200, connectionId1.ToString());

            var result1 = MosaicApp.Add(screen1);

            var screen1New = new Screen(screenId1, 200, 300, connectionId1.ToString());

            var result2 = MosaicApp.Add(screen1New);

            Assert.NotNull(result2);
            Assert.NotNull(MosaicApp.Mosaic);
            Assert.Equal<int>(200, MosaicApp.Mosaic.Items[0].Screen.Dimension.Width);
            Assert.Equal<int>(300, MosaicApp.Mosaic.Items[0].Screen.Dimension.Height);


        }
    }
}
