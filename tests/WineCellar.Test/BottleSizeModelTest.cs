using System;
using Xunit;

namespace WineCellar.Tests
{
    using WineCellar.Models;

    public class BottleSizeModelTests : IDisposable
    {
        private BottleSizeModel _testBottleSize;

        public BottleSizeModelTests()
        {
            _testBottleSize = new BottleSizeModel
            {
                Size = "BaseSize",
                VolumeMeasure = " Base Volume Measure",
                Default = false
            };
        }

        public void Dispose()
        {
            _testBottleSize = null;
        }

        [Fact]
        public void CanChangeBottleSize()
        {
            // Arrange

            // Act
            _testBottleSize.Size = "New Size";

            // Assert
            Assert.Equal( "New Size", _testBottleSize.Size );

        }

        [Fact]
        public void CanChangeVolumeMeasure()
        {
            // Arrange

            // Act
            _testBottleSize.VolumeMeasure = "New Volume Measure";

            // Assert
            Assert.Equal( "New Volume Measure", _testBottleSize.VolumeMeasure );

        }

        [Fact]
        public void CanChangeDefaultBottleSize()
        {
            // Arrange

            // Act
            _testBottleSize.Default = true;

            // Assert
            Assert.True( _testBottleSize.Default );
        }

    }
}
