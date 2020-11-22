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

    public class WineryControllerTests : IDisposable
    {

        private DbContextOptionsBuilder<WineCellarDBContext> _optionsBuilder;
        private WineCellarDBContext _dbContext;
        private WineryController _controller;

        public WineryControllerTests()
        {

            // Create inMemory database for testing.
            _optionsBuilder = new DbContextOptionsBuilder<WineCellarDBContext>( );
            _optionsBuilder.UseInMemoryDatabase( "WineryUnitTestInMemDB" );
            _dbContext = new WineCellarDBContext( _optionsBuilder.Options );

            // Create commands controller for testing.   
            _controller = new WineryController( _dbContext, new NullLogger<WineryController>( ) );

        }

        public void Dispose()
        {
            _optionsBuilder = null;

            // Cleanup any created database items.
            foreach (var Winery in _dbContext.Wineries)
            {
                _dbContext.Wineries.Remove( Winery );
            }
            _dbContext.SaveChanges( );
            _dbContext.Dispose( );

            _controller = null;
        }


        // ACTION 1 Tests : GET     /api/Winery

        // Test 1.1 : Request objects when none exist - Return "nothing"
        [Fact]
        public void GetWineries_ReturnsZeroWineries_WhenDBIsEmpty()
        {

            // Arrange

            // Act
            var results = _controller.GetWinery( );

            // Assert
            Assert.Empty( results.Value );
        }

        // Test 1.2 : Request count of single object when only 1 exists in database
        [Fact]
        public void GetWineries_ReturnsOneWinery_WhenDBHasOneWinery()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Winery Name",
                WebSite = "Winery Web Site",
                EMail = "Winery EMail",
                Phone = "Winery Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetWinery( );

            // Assert
            Assert.Single( results.Value );
        }

        // Test 1.3 : Request count of N objects when N exists in database
        [Fact]
        public void GetWinery_ReturnsWineries_WhenDBHasMultipleWioneries()
        {

            // Arrange
            var winery1 = new WineryEntity
            {
                Name = "Winery1 Name",
                WebSite = "Winery1 Web Site",
                EMail = "Winery1 EMail",
                Phone = "Winery1 Phone"
            };
            var winery2 = new WineryEntity
            {
                Name = "Winery2 Name",
                WebSite = "Winery2 Web Site",
                EMail = "Winery2 EMail",
                Phone = "Winery2 Phone"
            };
            _dbContext.Wineries.Add( winery1 );
            _dbContext.Wineries.Add( winery2 );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetWinery( );

            // Assert
            Assert.Equal( 2, results.Value.Count( ) );
        }

        // Test 1.4 : Request count of N objects when N exists in database
        [Fact]
        public void GetWinery_ReturnWinery_WithCorrectType()
        {

            // Arrange

            // Act
            var results = _controller.GetWinery( );

            // Assert
            Assert.IsType<ActionResult<IEnumerable<Winery>>>( results );
        }

        // ACTION 2 Tests : GET     /api/Winery/{id}

        // Test 2.1 : Request object by ID when none exist - Return null object value
        [Fact]
        public void GetWinery_ReturnsNullValue_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetWinery( 0 );

            // Assert
            Assert.Null( result.Value );
        }

        // Test 2.2 : Request object by ID when none exist - Return 404 Not Found Return Code
        [Fact]
        public void GetWinery_Returns404NotFound_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetWinery( 0 );

            // Assert
            Assert.IsType<NotFoundResult>( result.Result );
        }

        // Test 2.3 : Request object by valid ID - Check Correct Return Type
        [Fact]
        public void GetWinery_ReturnsItemOfCorrectType_WhenUsingValidID()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 2.3 Winery",
                WebSite = "Test 2.3 Web Site",
                EMail = "Test 2.3 EMail",
                Phone = "Test 2.3 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            var wineryEntityId = wineryEntity.Id;

            // Act
            var result = _controller.GetWinery( wineryEntityId );

            // Assert
            Assert.IsType<ActionResult<Winery>>( result );
        }

        // Test 2.4 : Request object by valid ID - Check correct item returned
        [Fact]
        public void GetWinery_ReturnCorrectWinery_WhenUsingValidID()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 2.4 Winery",
                WebSite = "Test 2.4 Web Site",
                EMail = "Test 2.4 EMail",
                Phone = "Test 2.4 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            var cmdId = wineryEntity.Id;

            // Act
            var result = _controller.GetWinery( cmdId );

            // Assert
            Assert.Equal( cmdId, result.Value.Id );
        }


        // ACTION 3 Tests : POST     /api/Winery

        // Test 3.1 : Create new Winery database item - Database record count increased.
        [Fact]
        public void PostWinery_WineryCountIncremented_WhenUsingValidWinery()
        {

            // Arrange
            var winery = new Winery
            {
                Name = "Test 3.1 Winery",
                WebSite = "Test 3.1 Web Site",
                EMail = "Test 3.1 EMail",
                Phone = "Test 3.1 Phone"
            };

            var oldCount = _dbContext.Wineries.Count( );

            // Act
            var result = _controller.PostWinery( winery );

            // Assert
            Assert.Equal( oldCount + 1, _dbContext.Wineries.Count( ) );
        }

        // Test 3.2 : Create new Winery database item - Return item of proper type.
        [Fact]
        public void PostWinery_ReturnsItemOfCorrectType_WhenUsingValidID()
        {

            // Arrange
            var winery = new Winery
            {
                Name = "Test 3.2 Winery",
                WebSite = "Test 3.2 Web Site",
                EMail = "Test 3.2 EMail",
                Phone = "Test 3.2 Phone"
            };

            // Act
            var result = _controller.PostWinery( winery );

            // Assert
            Assert.IsType<CreatedAtActionResult>( result.Result );
        }

        // Test 3.3 : Create duplicate Winery record in Databse - Returns 409 Conflict code
        [Fact]
        public void PostWinery_Returns409Conflict_WhenUsingDuplicateWinery()
        {

            // Arrange
            var winery = new Winery
            {
                Name = "Test 2.4 Winery",
                WebSite = "Test 2.4 Web Site",
                EMail = "Test 2.4 EMail",
                Phone = "Test 2.4 Phone"
            };

            // Act
            var result = _controller.PostWinery( winery );
            //--if ( Assert.IsType<CreatedAtActionResult>( result.Result ) )
            {
                // Attempt to add Winery a 2nd time.
                result = _controller.PostWinery( winery );
            }

            // Assert
            Assert.IsType<ConflictResult>( result.Result );
        }


        // ACTION 4 Tests : PUT         /api/Winery/{id}

        // Test 4.1 : Request attribute in valid object be updated - Return attribute updated
        [Fact]
        public void PutWinery_AttributesUpdated_WhenUsingValidWineryId()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.1 Winery",
                WebSite = "Test 4.1 Web Site",
                EMail = "Test 4.1 EMail",
                Phone = "Test 4.1 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );
            var wineryEntityId = wineryEntity.Id;

            var winery = new Winery
            {
                Name = "Test 4.1 Winery UPDATED",
                WebSite = "Test 4.1 Web Site UPDATED",
                EMail = "Test 4.1 EMail UPDATED",
                Phone = "Test 4.1 Phone UPDATED"
            };

            // Act
            _controller.PutWinery( wineryEntityId, winery );
            var result = _dbContext.Wineries.Find( wineryEntityId );

            // Assert
            Assert.Equal( winery.Name, result.Name );
            Assert.Equal( winery.WebSite, result.WebSite );
            Assert.Equal( winery.EMail, result.EMail );
            Assert.Equal( winery.Phone, result.Phone );
        }

        // Test 4.2 : Request attribute in valid object be updated - Return 204 return code
        [Fact]
        public void PutWinery_ReturnsItemOfCorrectType_WhenUsingValidWineryId()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.2 Winery",
                WebSite = "Test 4.2 Web Site",
                EMail = "Test 4.2 EMail",
                Phone = "Test 4.2 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );
            var wineryEntityId = wineryEntity.Id;

            var winery = new Winery
            {
                Name = "Test 4.2 Winery",
                WebSite = "Test 4.2 Web Site",
                EMail = "Test 4.2 EMail",
                Phone = "Test 4.2 Phone UPDATED"
            };

            // Act
            var result = _controller.PutWinery( wineryEntityId, winery );

            // Assert
            Assert.IsType<NoContentResult>( result );
        }

        // Test 4.3 : Request attribute in invalid object be updated - Return 400 return code
        [Fact]
        public void PutWinery_Returns404NotFound_WhenUsingInvalidIWineryId()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.3 Winery",
                WebSite = "Test 4.3 Web Site",
                EMail = "Test 4.3 EMail",
                Phone = "Test 4.3 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            var wineryId = wineryEntity.Id + 1;         // force an invalid record Id

            var winery = new Winery
            {
                Name = "Test 4.3 Winery",
                WebSite = "Test 4.3 Web Site UPDATED",
                EMail = "Test 4.3 EMail",
                Phone = "Test 4.3 Phone"
            };

            // Act
            var result = _controller.PutWinery( wineryId, winery );

            // Assert
            Assert.IsType<NotFoundResult>( result );
        }

        // Test 4.4 : Request attribute in invalid object be updated - Return original object unchanged
        [Fact]
        public void PutWinery_AttributesUnchanged_WhenUsingInvalidWineryId()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 4.4 Winery",
                WebSite = "Test 4.4 Web Site",
                EMail = "Test 4.4 EMail",
                Phone = "Test 4.4 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            var winery = new Winery
            {
                Name = "Test 4.4 Winery UPDATE",
                WebSite = "Test 4.4 Web Site UPDATE",
                EMail = "Test 4.4 EMail UPDATE",
                Phone = "Test 4.4 Phone UPDATE"
            };

            // Act
            _controller.PutWinery( wineryEntity.Id + 1, winery );
            var result = _dbContext.Wineries.Find( wineryEntity.Id );

            // Assert
            Assert.Equal( wineryEntity.Name, result.Name );
        }


        // ACTION 5 Tests : DELETE          /api/Winery/{id}

        // Test 5.1 : Request valid object Id be deleted - Results in object count decremented by 1
        [Fact]
        public void DeleteWinery_ObjectCountDecrementedBy1_WhenUsingValidWineryId()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 5.1 Winery",
                WebSite = "Test 5.1 Web Site",
                EMail = "Test 5.1 EMail",
                Phone = "Test 5.1 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            var wineryId = wineryEntity.Id;
            var objCount = _dbContext.Wineries.Count( );

            // Act
            _controller.DeleteWinery( wineryId );

            // Assert
            Assert.Equal( objCount - 1, _dbContext.Wineries.Count( ) );
        }

        // Test 5.2 : Request valid objectId  be deleted - Returns 200 OK code
        [Fact]
        public void DeleteWinery_Returns200OK_WhenUsingValidWineryId()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 5.2 Winery",
                WebSite = "Test 5.2 Web Site",
                EMail = "Test 5.2 EMail",
                Phone = "Test 5.2 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            var wineryId = wineryEntity.Id;

            // Act
            var result = _controller.DeleteWinery( wineryId );

            // Assert
            Assert.Null( result.Result );
        }

        // Test 5.3 : Request invalid object Id be deleted - Returns 404 Not Found code
        [Fact]
        public void DeleteWinery_Returns404NotFound_WhenUsingInvalidWineryId()
        {

            // Arrange

            // Act
            var result = _controller.DeleteWinery( -1 );

            // Assert
            Assert.IsType<NotFoundResult>( result.Result );
        }

        // Test 5.4 : Request invalid object Id be deleted - Object count unchanged
        [Fact]
        public void DeleteWinery_UnchangedObjectCount_WhenUsingInvalidWineryId()
        {

            // Arrange
            var wineryEntity = new WineryEntity
            {
                Name = "Test 5.4 Winery",
                WebSite = "Test 5.4 Web Site",
                EMail = "Test 5.4 EMail",
                Phone = "Test 5.4 Phone"
            };
            _dbContext.Wineries.Add( wineryEntity );
            _dbContext.SaveChanges( );

            var wineryId = wineryEntity.Id;
            var objCount = _dbContext.Wineries.Count( );

            // Act
            var result = _controller.DeleteWinery( wineryId + 1 );

            // Assert
            Assert.Equal( objCount, _dbContext.Wineries.Count( ) );
        }

    }

}