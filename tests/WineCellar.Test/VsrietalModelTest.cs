using System;
using Xunit;

namespace WineCellar.Tests
{
    using WineCellar.Models;

    public class VarietalModelTests : IDisposable
    {
        private VarietalModel _testVarietal;

        public VarietalModelTests( )
        {
            _testVarietal = new VarietalModel
            {
                Varietal = "Test Varietal"
            };
        }

        public void Dispose( )
        {
            _testVarietal = null;
        }

        [Fact]
        public void CanChangeVarietal( )
        {
            // Arrange

            // Act
            _testVarietal.Varietal = "New Unit Test Varietal";

            // Assert
            Assert.Equal( "New Unit Test Varietal", _testVarietal.Varietal );

        }
    }
}
