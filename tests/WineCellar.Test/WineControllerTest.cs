using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

using Xunit;


namespace WineCellar.Tests
{
    using WineCellar.Controllers;
    using WineCellar.Entities;
    using WineCellar.Models;

    public class WineControllerTests : IDisposable
    {

        private DbContextOptionsBuilder<WineCellarDBContext> _optionsBuilder;
        private WineCellarDBContext _dbContext;
        private WineController _controller;

        public WineControllerTests()
        {

            // Create inMemory database for testing.
            _optionsBuilder = new DbContextOptionsBuilder<WineCellarDBContext>( );
            _optionsBuilder.UseInMemoryDatabase( "WineUnitTestInMemDB" );
            _dbContext = new WineCellarDBContext( _optionsBuilder.Options );

            // Create commands controller for testing.   
            _controller = new WineController( _dbContext, new NullLogger<WineController>( ) );

        }

        public void Dispose()
        {
            _optionsBuilder = null;

            // Cleanup any created database items.
            foreach ( var aWine in _dbContext.Wines )
            {
                _dbContext.Wines.Remove( aWine );
            }
            foreach ( var aVarietal in _dbContext.Varietals )
            {
                _dbContext.Varietals.Remove( aVarietal );
            }
            foreach ( var aBottleSize in _dbContext.BottleSizes )
            {
                _dbContext.BottleSizes.Remove( aBottleSize );
            }
            foreach ( var aWinery in _dbContext.Wineries )
            {
                _dbContext.Wineries.Remove( aWinery );
            }
            _dbContext.SaveChanges( );
            _dbContext.Dispose( );

            _controller = null;
        }


        // ACTION 1 Tests : GET     /api/Wine

        // Test 1.1 : Request objects when none exist - Return "nothing"
        [Fact]
        public void GetWine_ReturnsZeroWines_WhenDBIsEmpty()
        {

            // Arrange

            // Act
            var results = _controller.GetWine( );

            // Assert
            Assert.Empty( results.Value );
        }

        // Test 1.2 : Request count of single object when only 1 exists in database
        [Fact]
        public void GetWine_ReturnsOneWine_WhenDBHasOneWine()
        {

            // Arrange
            var wineEntity = new WineEntity
            {
                Name = "Wine Name",
                Vintage = "Wine Vintage",
                WhenPurchased = "Wine WhenPurchased",
                BottlesDrank = 0,
                BottlesPurchased = 2
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetWine( );

            // Assert
            Assert.Single( results.Value );
        }

        // Test 1.3 : Request count of N objects when N exists in database
        [Fact]
        public void GetWine_ReturnsWines_WhenDBHasMultipleWines()
        {

            // Arrange
            var wine1 = new WineEntity
            {
                Name = "Wine1 Name",
                Vintage = "Wine1 Vintage",
                WhenPurchased = "Wine1 WhenPurchased",
                BottlesDrank = 1,
                BottlesPurchased = 8
            };
            var wine2 = new WineEntity
            {
                Name = "Wine2 Name",
                Vintage = "Wine2 Vintage",
                WhenPurchased = "Wine2 WhenPurchased",
                BottlesDrank = 0,
                BottlesPurchased = 6
            };
            _dbContext.Wines.Add( wine1 );
            _dbContext.Wines.Add( wine2 );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetWine( );

            // Assert
            Assert.Equal( 2, results.Value.Count( ) );
        }

        // Test 1.4 : Request count of N objects when N exists in database
        [Fact]
        public void GetWine_ReturnWine_WithCorrectType()
        {

            // Arrange

            // Act
            var results = _controller.GetWine( );

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Wine>>>( results );
        }

        // ACTION 2 Tests : GET     /api/Wine/{id}

        // Test 2.1 : Request object by ID when none exist - Return null object value
        [Fact]
        public void GetWine_ReturnsNullValue_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetWine( 0 );

            // Assert
            Assert.Null( result.Value );
        }

        // Test 2.2 : Request object by ID when none exist - Return 404 Not Found Return Code
        [Fact]
        public void GetWine_Returns404NotFound_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetWine( 0 );

            // Assert
            Assert.IsType<NotFoundResult>( result.Result );
        }

        // Test 2.3 : Request object by valid ID - Check Correct Return Type
        [Fact]
        public void GetWine_ReturnsItemOfCorrectType_WhenUsingValidID()
        {

            // Arrange
            var wineEntity = new WineEntity
            {
                Name = "Test 2.3 Wine",
                Vintage = "Test 2.3 Vintage",
                WhenPurchased = "Test 2.3 WhenPurchased",
                BottlesDrank = 0,
                BottlesPurchased = 4
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );

            var wineEntityId = wineEntity.Id;

            // Act
            var result = _controller.GetWine( wineEntityId );

            // Assert
            Assert.IsType<ActionResult<Wine>>( result );
        }

        // Test 2.4 : Request object by valid ID - Check correct item returned
        [Fact]
        public void GetWine_ReturnCorrectWine_WhenUsingValidID()
        {

            // Arrange
            var wineEntity = new WineEntity
            {
                Name = "Test 2.4 Wine",
                Vintage = "Test 2.4 Vintage",
                WhenPurchased = "Test 2.4 WhenPurchased",
                BottlesDrank = 0,
                BottlesPurchased = 3
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );

            var wineId = wineEntity.Id;

            // Act
            var result = _controller.GetWine( wineId );

            // Assert
            Assert.Equal( wineId, result.Value.Id );
        }


        // ACTION 3 Tests : POST     /api/Wine

        // Test 3.1 : Create new Winery database item - Database record count increased.
        [Fact]
        public void PostWine_WineryCountIncremented_WhenUsingValidWine()
        {

            // Arrange
                // Set up associated object - Varietal.
            var varietalEntity = new VarietalEntity
            {
                Varietal = "Test 3.1 Varietal"
            };
            _dbContext.Varietals.Add( varietalEntity );
 
                // Set up associated object - BottleSize.
            var bottleSizeEntity = new BottleSizeEntity
            {
                // Just use default values.
            };
            _dbContext.BottleSizes.Add( bottleSizeEntity );

            // Set up associated object - Winery.
            var wineryEntity = new WineryEntity
            {
                Name = "Test 3.1 Winery"
            };
            _dbContext.Wineries.Add( wineryEntity );

            _dbContext.SaveChanges( );

            var wine = new Wine
            {
                Name = "Test 3.1 Wine",
                WhenPurchased = "Test 3.1 WhenPurchased",
                BottlesDrank = 0,
                BottlesPurchased = 6,
                VarietalId = varietalEntity.Id,
                BottleSizeId = bottleSizeEntity.Id,
                WineryId = wineryEntity.Id,
                Vintage = "Test 3.1 Vintage"
            };

            var oldCount = _dbContext.Wines.Count( );

            // Act
            var result = _controller.PostWine( wine );

            // Assert
            Assert.Equal( oldCount + 1, _dbContext.Wines.Count( ) );
        }

        // Test 3.2 : Create new Winery database item - Return item of proper type.
        [Fact]
        public void PostWine_ReturnsItemOfCorrectType_WhenUsingValidID()
        {
            // Set up associated object - Varietal.
            var varietalEntity = new VarietalEntity
            {
                Varietal = "Test 3.2 Varietal"
            };
            _dbContext.Varietals.Add( varietalEntity );

            // Set up associated object - BottleSize.
            var bottleSizeEntity = new BottleSizeEntity
            {
                // Just use default values.
            };
            _dbContext.BottleSizes.Add( bottleSizeEntity );

            // Set up associated object - Winery.
            var wineryEntity = new WineryEntity
            {
                Name = "Test 3.2 Winery"
            };
            _dbContext.Wineries.Add( wineryEntity );

            _dbContext.SaveChanges( );

            // Arrange
            var wine = new Wine
            {
                Name = "Test 3.2 Wine",
                WhenPurchased = "Test 3.2 WhenPurchased",
                //--BottlesDrank = 4,
                BottlesPurchased = 4,
                VarietalId = varietalEntity.Id,
                BottleSizeId = bottleSizeEntity.Id,
                WineryId = wineryEntity.Id,
                Vintage = "Test 3.2 Vintage"
            };

            // Act
            var result = _controller.PostWine( wine );

            // Assert
            Assert.IsType<CreatedAtActionResult>( result.Result );
        }


        // ACTION 4 Tests : PUT         /api/Wine/{id}

        // Test 4.1 : Request attribute in valid object be updated - Return attribute updated
        [Fact]
        public void PutWine_AttributesUpdated_WhenUsingValidWineId()
        {

            // Arrange
            // Set up associated object - Varietal.
            var varietalEntity = new VarietalEntity
            {
                Varietal = "Test 4.1 Varietal"
            };
            _dbContext.Varietals.Add( varietalEntity );

            // Set up associated object - BottleSize.
            var bottleSizeEntity = new BottleSizeEntity
            {
                // Just use default values.
            };
            _dbContext.BottleSizes.Add( bottleSizeEntity );

            // Set up associated object - Winery.
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.1 Winery"
            };
            _dbContext.Wineries.Add( wineryEntity );

            _dbContext.SaveChanges( );

            var wineEntity = new WineEntity
            {
                Name = "Test 4.1 Wine",
                WhenPurchased = "Test 4.1 WhenPurchased",
                //--BottlesDrank = 4,
                BottlesPurchased = 8,
                VarietalEntityId = varietalEntity.Id,
                BottleSizeEntityId = bottleSizeEntity.Id,
                WineryEntityId = wineryEntity.Id,
                Vintage = "Test 4.1 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );
            var wineEntityId = wineEntity.Id;

            var wine = new Wine
            {
                Name = "Test 4.1 Wine UPDATED",
                WhenPurchased = "Test 4.1 WhenPurchased UPDATED",
                //--BottlesPurchased = 8,
                BottlesDrank = 6,
                Vintage = "Test 4.1 Vintage UPDATED"
            };

            // Act
            _controller.PutWine( wineEntityId, wine );
            var result = _dbContext.Wines.Find( wineEntityId );

            // Assert
            Assert.Equal( wine.Name, result.Name );
            Assert.Equal( wine.WhenPurchased, result.WhenPurchased );
            Assert.Equal( wine.BottlesPurchased, result.BottlesPurchased );
            Assert.Equal( wine.BottlesDrank, result.BottlesDrank );
            Assert.Equal( wine.Vintage, result.Vintage );
            Assert.Equal( wine.VarietalId, result.VarietalEntityId );
            Assert.Equal( wine.BottleSizeId, result.BottleSizeEntityId );
            Assert.Equal( wine.WineryId, result.WineryEntityId );
        }

        // Test 4.2 : Request attribute in valid object be updated - Return 204 return code
        [Fact]
        public void PutWine_ReturnsItemOfCorrectType_WhenUsingValidWineId()
        {

            // Arrange
            // Set up associated object - Varietal.
            var varietalEntity = new VarietalEntity
            {
                Varietal = "Test 4.2 Varietal"
            };
            _dbContext.Varietals.Add( varietalEntity );

            // Set up associated object - BottleSize.
            var bottleSizeEntity = new BottleSizeEntity
            {
                // Just use default values.
            };
            _dbContext.BottleSizes.Add( bottleSizeEntity );

            // Set up associated object - Winery.
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.2 Winery"
            };
            _dbContext.Wineries.Add( wineryEntity );

            _dbContext.SaveChanges( );

            var wineEntity = new WineEntity
            {
                Name = "Test 4.2 Wine",
                WhenPurchased = "Test 4.2 WhenPurchased",
                //--BottlesDrank = 4,
                BottlesPurchased = 3,
                VarietalEntityId = varietalEntity.Id,
                BottleSizeEntityId = bottleSizeEntity.Id,
                WineryEntityId = wineryEntity.Id,
                Vintage = "Test 4.2 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );
            var wineEntityId = wineEntity.Id;

            var wine = new Wine
            {
                Name = "Test 4.2 Wine UPDATED",
                Vintage = "Test 4.2 Vintage UPDATED"
            };

            // Act
            var result = _controller.PutWine( wineEntityId, wine );

            // Assert
            Assert.IsType<NoContentResult>( result );
        }

        // Test 4.3 : Request attribute in invalid object be updated - Return 400 return code
        [Fact]
        public void PutWine_Returns404NotFound_WhenUsingInvalidIWineId()
        {

            // Arrange
            // Set up associated object - Varietal.
            var varietalEntity = new VarietalEntity
            {
                Varietal = "Test 4.3 Varietal"
            };
            _dbContext.Varietals.Add( varietalEntity );

            // Set up associated object - BottleSize.
            var bottleSizeEntity = new BottleSizeEntity
            {
                // Just use default values.
            };
            _dbContext.BottleSizes.Add( bottleSizeEntity );

            // Set up associated object - Winery.
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.3 Winery"
            };
            _dbContext.Wineries.Add( wineryEntity );

            _dbContext.SaveChanges( );

            var wineEntity = new WineEntity
            {
                Name = "Test 4.3 Wine",
                WhenPurchased = "Test 4.3 WhenPurchased",
                //--BottlesDrank = 4,
                BottlesPurchased = 3,
                VarietalEntityId = varietalEntity.Id,
                BottleSizeEntityId = bottleSizeEntity.Id,
                WineryEntityId = wineryEntity.Id,
                Vintage = "Test 4.3 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );
            var wineEntityId = wineEntity.Id;
            var invalidWineId = wineEntityId + 10;

            var wine = new Wine
            {
                Name = "Test 4.3 Wine UPDATE",
                Vintage = "Test 4.2 Vintage UPDATED"
            };

            // Act
            var result = _controller.PutWine( invalidWineId, wine );

            // Assert
            Assert.IsType<NotFoundResult>( result );
        }

        // Test 4.4 : Request attribute in invalid object be updated - Return original object unchanged
        [Fact]
        public void PutWine_AttributesUnchanged_WhenUsingInvalidWineId()
        {

            // Arrange
            // Set up associated object - Varietal.
            var varietalEntity = new VarietalEntity
            {
                Varietal = "Test 4.4 Varietal"
            };
            _dbContext.Varietals.Add( varietalEntity );

            // Set up associated object - BottleSize.
            var bottleSizeEntity = new BottleSizeEntity
            {
                // Just use default values.
            };
            _dbContext.BottleSizes.Add( bottleSizeEntity );

            // Set up associated object - Winery.
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.4 Winery"
            };
            _dbContext.Wineries.Add( wineryEntity );

            _dbContext.SaveChanges( );

            var wineEntity = new WineEntity
            {
                Name = "Test 4.4 Wine",
                WhenPurchased = "Test 4.4 WhenPurchased",
                //--BottlesDrank = 4,
                BottlesPurchased = 3,
                VarietalEntityId = varietalEntity.Id,
                BottleSizeEntityId = bottleSizeEntity.Id,
                WineryEntityId = wineryEntity.Id,
                Vintage = "Test 4.4 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );
            var wineEntityId = wineEntity.Id;

            var wine = new Wine
            {
                Name = "Test 4.4 Wine UPDATE",
                WhenPurchased = "Test 4.4 WhenPurchased UPDTED",
                BottlesDrank = 2,
                Vintage = "Test 4.4 Vintage UPDATED"
            };

            // Act
            _controller.PutWine( wineEntity.Id + 1, wine );
            var result = _dbContext.Wines.Find( wineEntity.Id );

            // Assert
            Assert.Equal( wineEntity.Name, result.Name );
        }

        // Test 4.5 : Request linked data updated in base object be updated - Return attribute updated
        [Fact]
        public void PutWine_LinkedInfoUpdated_WhenUsingValidWineId()
        {

            // Arrange
            // Set up associated object - Varietal.
            var varietalEntity = new VarietalEntity
            {
                Varietal = "Test 4.5 Varietal"
            };
            var varietalEntity2 = new VarietalEntity
            {
                Varietal = "Test 4.5 Varietal2"
            };
            _dbContext.Varietals.Add( varietalEntity );
            _dbContext.Varietals.Add( varietalEntity2 );

            // Set up associated object - BottleSize.
            var bottleSizeEntity = new BottleSizeEntity
            {
                // Just use default values.
            };
            var bottleSizeEntity2 = new BottleSizeEntity
            {
                BottleSize = "375"         
            };
            _dbContext.BottleSizes.Add( bottleSizeEntity );
            _dbContext.BottleSizes.Add( bottleSizeEntity2 );

            // Set up associated object - Winery.
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.1 Winery"
            };
            var wineryEntity2 = new WineryEntity
            {
                Name = "Test 4.5 Winery2"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.Wineries.Add( wineryEntity2 );

            _dbContext.SaveChanges( );

            var wineEntity = new WineEntity
            {
                Name = "Test 4.5 Wine",
                WhenPurchased = "Test 4.5 WhenPurchased",
                //--BottlesDrank = 4,
                BottlesPurchased = 8,
                VarietalEntityId = varietalEntity.Id,
                BottleSizeEntityId = bottleSizeEntity.Id,
                WineryEntityId = wineryEntity.Id,
                Vintage = "Test 4.5 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );
            var wineEntityId = wineEntity.Id;

            var wine = new Wine
            {
                // Update Ids of linked records.
                BottleSizeId = bottleSizeEntity2.Id,
                VarietalId = varietalEntity2.Id,
                WineryId = wineryEntity2.Id
            };

            // Act
            _controller.PutWine( wineEntityId, wine );
            var result = _dbContext.Wines.Find( wineEntityId );

            // Assert
            Assert.Equal( wine.VarietalId, result.VarietalEntityId );
            Assert.Equal( wine.BottleSizeId, result.BottleSizeEntityId );
            Assert.Equal( wine.WineryId, result.WineryEntityId );
            Assert.Equal( bottleSizeEntity2.BottleSize, result.BottleSize.BottleSize );
            Assert.Equal( varietalEntity2.Varietal, result.Varietal.Varietal );
            Assert.Equal( wineryEntity2.Name, result.Winery.Name );
        }


        // ACTION 5 Tests : DELETE          /api/Wine/{id}

        // Test 5.1 : Request valid object Id be deleted - Results in object count decremented by 1
        [Fact]
        public void DeleteWine_ObjectCountDecrementedBy1_WhenUsingValidWineId()
        {

            // Arrange
            var wineEntity = new WineEntity
            {
                Name = "Test 5.1 Wine",
                WhenPurchased = "Test 5.1 WhenPurchased",
                //--BottlesDrank = 4,
                //--BottlesPurchased = 6,
                Vintage = "Test 5.1 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );

            var wineId = wineEntity.Id;
            var objCount = _dbContext.Wines.Count( );

            // Act
            _controller.DeleteWine( wineId );

            // Assert
            Assert.Equal( objCount - 1, _dbContext.Wines.Count( ) );
        }

        // Test 5.2 : Request valid objectId  be deleted - Returns 200 OK code
        [Fact]
        public void DeleteWine_Returns200OK_WhenUsingValidWineId()
        {

            // Arrange
            var wineEntity = new WineEntity
            {
                Name = "Test 5.2 Wine",
                WhenPurchased = "Test 5.2 WhenPurchased",
                //--BottlesDrank = 4,
                //--BottlesPurchased = 6,
                Vintage = "Test 5.2 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );

            var wineId = wineEntity.Id;

            // Act
            var result = _controller.DeleteWine( wineId );

            // Assert
            Assert.Null( result.Result );
        }

        // Test 5.3 : Request invalid object Id be deleted - Returns 404 Not Found code
        [Fact]
        public void DeleteWine_Returns404NotFound_WhenUsingInvalidWineId()
        {

            // Arrange

            // Act
            var result = _controller.DeleteWine( -1 );

            // Assert
            Assert.IsType<NotFoundResult>( result.Result );
        }

        // Test 5.4 : Request invalid object Id be deleted - Object count unchanged
        [Fact]
        public void DeleteWine_UnchangedObjectCount_WhenUsingInvalidWineId()
        {

            // Arrange
            var wineEntity = new WineEntity
            {
                Name = "Test 5.4 Wine",
                WhenPurchased = "Test 5.4 WhenPurchased",
                //--BottlesDrank = 4,
                //--BottlesPurchased = 6,
                Vintage = "Test 5.4 Vintage"
            };
            _dbContext.Wines.Add( wineEntity );
            _dbContext.SaveChanges( );

            var wineId = wineEntity.Id;
            var objCount = _dbContext.Wines.Count( );

            // Act
            var result = _controller.DeleteWine( wineId + 1 );

            // Assert
            Assert.Equal( objCount, _dbContext.Wines.Count( ) );
        }

    }

}