using System;
using Xunit;

namespace WineCellar.Tests
{
    using WineCellar.Models;

    public class WineryModelTests : IDisposable
    {
        private Winery _testWinery;

        public WineryModelTests()
        {
            _testWinery = new Winery
            {
                Name = "Base Winery",
                WebSite = "Base WebSite",
                EMail = "Base EMail",
                Phone = "Base Phone"
            };
        }

        public void Dispose()
        {
            _testWinery = null;
        }

        [Fact]
        public void CanChangeWineryName()
        {
            // Arrange

            // Act
            _testWinery.Name = "New Winery Name";

            // Assert
            Assert.Equal( "New Winery Name", _testWinery.Name );

        }

        [Fact]
        public void CanChangeWineryWebSite()
        {
            // Arrange

            // Act
            _testWinery.WebSite = "New Winery WebSitee";

            // Assert
            Assert.Equal( "New Winery WebSitee", _testWinery.WebSite );

        }

        [Fact]
        public void CanChangeWineryEMail()
        {
            // Arrange

            // Act
            _testWinery.EMail = "New Winery EMail";

            // Assert
            Assert.Equal( "New Winery EMail", _testWinery.EMail );
        }

        [Fact]
        public void CanChangeWineryPhone()
        {
            // Arrange

            // Act
            _testWinery.Phone = "New Winery Phone";

            // Assert
            Assert.Equal( "New Winery Phone", _testWinery.Phone );
        }

    }
}
