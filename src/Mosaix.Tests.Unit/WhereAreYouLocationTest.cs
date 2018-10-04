using Ogyke.Core;
using Ogyke.Core.Entities;
using System;
using Xunit;

namespace Mosaix.Tests.Unit
{
    public class WhereAreYouLocationTest
    {
        private WhereAreYouLocation _location = new WhereAreYouLocation();

        [Fact]
        public void Should_GetInitLocation_WhenInit()
        {
            var element = new Element(50, 50);
            var result = _location.Get(element);

            Assert.NotNull(result);
            Assert.Equal<long>(0, result.X);
            Assert.Equal<long>(0, result.Y);
        }


        [Fact]
        public void Should_IsInTrue_When_ElemIsInScreenTopLeftCorner()
        {
            var element = new Element(50, 50);
            var screen = new Screen(200, 200);
            element.Move(0, 0);

            var result = _location.IsIn(screen, element);

            Assert.True(result);
        }

        [Fact]
        public void Should_IsInTrue_When_ElemIsInScreenCenter()
        {
            var element = new Element(50, 50);
            var screen = new Screen(200, 200);
            element.Move(100, 100);

            var result = _location.IsIn(screen, element);

            Assert.True(result);
        }

        [Fact]
        public void Should_IsInTrue_When_ElemIsInRightCornerOnePixelInside()
        {
            var element = new Element(50, 50);
            var screen = new Screen(200, 200);
            element.Move(screen.Dimension.Width-1, screen.Dimension.Height-1);

            var result = _location.IsIn(screen, element);

            Assert.True(result);
        }



        [Fact]
        public void Should_IsInFalse_When_ElemIsNotInScreen()
        {
            var element = new Element(50, 50);
            var screen = new Screen(200, 200);
            element.Move(500, 500);

            var result = _location.IsIn(screen, element);

            Assert.False(result);
        }

        [Fact]
        public void Should_IsInFalse_When_ElemIsInScreenBottomRightCorner()
        {
            var element = new Element(50, 50);
            var screen = new Screen(200, 200);
            element.Move(200, 200);

            var result = _location.IsIn(screen, element);

            Assert.False(result);
        }

    }
}
