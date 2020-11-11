using System;
using Xunit;

namespace WineCellar.Tests
{
    using WineCellar.Models;

    public class WineModelTests : IDisposable
    {
        private Wine _testWine;

        public WineModelTests()
        {
            _testWine = new Wine
            {
                Name = "Base Winy",
                Vintage = "Base Vintage",
                WhenPurchased = "Base Purchased Date",
                BottlesPurchased = 6,
                BottlesDrank = 0,

                // Info from BottleSize
                BottleSizeId = 100,
                FullBottleSize = "750ml",
                BottleVolume = "750",
                BottleMeasure = "ml",

                // Info from Varietal
                VarietalId = 200,
                Varietal = "Base Varietal",
                
                // Infop from Winery
                WineryId = 300,
                WineryName = "Base Winery Name"
            };
        }

        public void Dispose()
        {
            _testWine = null;
        }

        [Fact]
        public void CanChangeWneyName()
        {
            // Arrange

            // Act
            _testWine.Name = "New Wine Name";

            // Assert
            Assert.Equal( "New Wine Name", _testWine.Name );
        }

        [Fact]
        public void CanChangeWineVintage()
        {
            // Arrange

            // Act
            _testWine.Vintage = "New Wine Vintage";

            // Assert
            Assert.Equal( "New Wine Vintage", _testWine.Vintage );
        }

        [Fact]
        public void CanChangeWineWhenPurchased()
        {
            // Arrange

            // Act
            _testWine.WhenPurchased = "New Wine Purchase Date";

            // Assert
            Assert.Equal( "New Wine Purchase Date", _testWine.WhenPurchased );
        }

        [Fact]
        public void CanChangeWineBottlesPurchased()
        {
            // Arrange

            // Act
            _testWine.BottlesPurchased = 12;

            // Assert
            Assert.Equal( 12, _testWine.BottlesPurchased );
        }

        [Fact]
        public void CanChangeWineBottlesDrank()
        {
            // Arrange

            // Act
            _testWine.BottlesDrank = 2;

            // Assert
            Assert.Equal( 2, _testWine.BottlesDrank );
        }

        [Fact]
        public void CanChangeWineBottleSizeId()
        {
            // Arrange

            // Act
            _testWine.BottleSizeId = 125;

            // Assert
            Assert.Equal( 125, _testWine.BottleSizeId );
        }

        [Fact]
        public void CanChangeWineVarietalId()
        {
            // Arrange

            // Act
            _testWine.VarietalId = 225;

            // Assert
            Assert.Equal( 225, _testWine.VarietalId );
        }

        [Fact]
        public void CanChangeWineWineryId()
        {
            // Arrange

            // Act
            _testWine.WineryId = 325;

            // Assert
            Assert.Equal( 325, _testWine.WineryId );
        }

    }
}
