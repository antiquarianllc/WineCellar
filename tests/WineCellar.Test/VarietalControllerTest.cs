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

    public class VarietalControllerTests : IDisposable
    {

        private DbContextOptionsBuilder<WineCellarDBContext> _optionsBuilder;
        private WineCellarDBContext _dbContext;
        private VarietalController _controller;

        public VarietalControllerTests()
        {

            // Create inMemory database for testing.
            _optionsBuilder = new DbContextOptionsBuilder<WineCellarDBContext>( );
            _optionsBuilder.UseInMemoryDatabase( "UnitTestInMemDB" );
            _dbContext = new WineCellarDBContext( _optionsBuilder.Options );

            // Create commands controller for testing.   
            _controller = new VarietalController( _dbContext, new NullLogger<VarietalController>( ) );

        }

        public void Dispose()
        {
            _optionsBuilder = null;

            // Cleanup any created database items.
            foreach (var varietal in _dbContext.Varietals)
            {
                _dbContext.Varietals.Remove( varietal );
            }
            _dbContext.SaveChanges( );
            _dbContext.Dispose( );

            _controller = null;
        }


        // ACTION 1 Tests : GET     /api/varietal

        // Test 1.1 : Request objects when none exist - Return "nothing"
        [Fact]
        public void GetVarietals_ReturnsZeroVarietals_WhenDBIsEmpty()
        {

            // Arrange

            // Act
            var results = _controller.GetVarietals( );

            //Assert
            Assert.Empty( results.Value );
        }

        // Test 1.2 : Request count of single object when only 1 exists in database
        [Fact]
        public void GetVarietals_ReturnsOneVarietal_WhenDBHasOneVarietal()
        {

            // Arrange
            var varietal = new VarietalEntity
            {
                Varietal = "Test 1.2 Varietal"
            };

            _dbContext.Add( varietal );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetVarietals( );

            //Assert
            Assert.Single( results.Value );

        }

        // Test 1.3 : Request count of N objects when N exists in database
        [Fact]
        public void GetVarietals_ReturnsVarietals_WhenDBHasNVarietals()
        {

            // Arrange
            var varietal = new VarietalEntity
            {
                Varietal = "Test 1.3 Red Variwtal"
            };
            var varietal2 = new VarietalEntity
            {
                Varietal = "Test 1.3 White Varietal" 
            };

            _dbContext.Add( varietal );
            _dbContext.Add( varietal2 );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetVarietals( );

            //Assert
            Assert.Equal( 2, results.Value.Count( ) );

        }

        // Test 1.4 : Request count of N objects when N exists in database
        [Fact]
        public void GetVarietals_ReturnVarietals_WithCorrectType()
        {

            // Arrange

            // Act
            var results = _controller.GetVarietals( );

            //Assert
            Assert.IsType<ActionResult<IEnumerable<VarietalModel>>>( results );

        }

        // ACTION 2 Tests : GET     /api/varietal/{id}

        // Test 2.1 : Request object by ID when none exist - Return null object value
        [Fact]
        public void GetVarietal_ReturnsNullValue_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetVarietal( 0 );

            //Assert
            Assert.Null( result.Value );

        }

        // Test 2.2 : Request object by ID when none exist - Return 404 Not Found Return Code
        [Fact]
        public void GetVarietal_Returns404NotFound_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetVarietal( 0 );

            //Assert
            Assert.IsType<NotFoundResult>( result.Result );

        }

        // Test 2.3 : Request object by valid ID - Check Correct Return Type
        [Fact]
        public void GetVarietal_ReturnsItemOfCorrectType_WhenUsingValidID()
        {

            // Arrange
            var varietal = new VarietalEntity
            {
                Varietal = "Test 2.3 Varietal"
            };

            _dbContext.Add( varietal );
            _dbContext.SaveChanges( );

            var cmdId = varietal.Id;

            // Act
            var result = _controller.GetVarietal( cmdId );

            //Assert
            Assert.IsType<ActionResult<VarietalModel>>( result );

        }

        // Test 2.4 : Request object by valid ID - Check correct item returned
        [Fact]
        public void GetVarietal_ReturnCorrectVarietal_WhenUsingValidID()
        {

            // Arrange
            var varietal = new VarietalEntity
            {
                Varietal = "Test 2.4 Varietal"
            };

            _dbContext.Add( varietal );
            _dbContext.SaveChanges( );

            var cmdId = varietal.Id;

            // Act
            var result = _controller.GetVarietal( cmdId );

            //Assert
            Assert.Equal( cmdId, result.Value.Id );

        }


        // ACTION 3 Tests : POST     /api/varietal

        // Test 3.1 : Create new varietal database item - Database record count increased.
        [Fact]
        public void PostVarietal_VarietalCountIncremented_WhenUsingValidVarietal()
        {

            // Arrange
            var varietal = new VarietalModel
            {
                Varietal = "Test 3.1 Varietal"
            };

            var oldCount = _dbContext.Varietals.Count( );

            // Act
            var result = _controller.PostVarietal( varietal );

            //Assert
            Assert.Equal( oldCount + 1, _dbContext.Varietals.Count( ) );

        }

        // Test 3.2 : Create new varietal database item - Return item of proper type.
        [Fact]
        public void PostVarietal_ReturnsItemOfCorrectType_WhenUsingValidID()
        {

            // Arrange
            var varietal = new VarietalModel
            {
                Varietal = "Test 3.2 Varietal"
            };

            // Act
            var result = _controller.PostVarietal( varietal );

            //Assert
            Assert.IsType<CreatedAtActionResult>( result.Result );

        }

        // Test 3.3 : Create duplicate varietal record in Databse - Returns 409 Conflict code
        [Fact]
        public void PostVarietal_Returns409Conflict_WhenUsingDuplicateVarietal()
        {

            // Arrange
            var varietal = new VarietalModel
            {
                Varietal = "Test 3.3 Varietal"
            };

            // Act
            var result = _controller.PostVarietal( varietal );
            //--if ( Assert.IsType<CreatedAtActionResult>( result.Result ) )
            {
                // Attempt to add varietal a 2nd time.
                result = _controller.PostVarietal( varietal );
            }

            //Assert
            Assert.IsType<ConflictResult>( result.Result );

        }


        // ACTION 5 Tests : DELETE          /api/varietal/{id}

        // Test 5.1 : Request valid object Id be deleted - Results in object count decremented by 1
        [Fact]
        public void DeleteVarietal_ObjectCountDecrementedBy1_WhenUsingValidVarietalId()
        {

            // Arrange
            var varietal = new VarietalEntity
            {
                Varietal = "Test 5.1 Varietal"
            };
            _dbContext.Varietals.Add( varietal );
            _dbContext.SaveChanges( );

            var varietalId = varietal.Id;
            var objCount = _dbContext.Varietals.Count( );

            // Act
            _controller.DeleteVarietal( varietalId );

            //Assert
            Assert.Equal( objCount - 1, _dbContext.Varietals.Count( ) );
        }

        // Test 5.2 : Request valid objectId  be deleted - Returns 200 OK code
        [Fact]
        public void DeleteVarietal_Returns200OK_WhenUsingValidVarietalId()
        {

            // Arrange
            var varietal = new VarietalEntity
            {
                Varietal = "Test 5.2 Varietal"
            };
            _dbContext.Varietals.Add( varietal );
            _dbContext.SaveChanges( );

            var varietalId = varietal.Id;

            // Act
            var result = _controller.DeleteVarietal( varietalId );

            //Assert
            Assert.Null( result.Result );
        }

        // Test 5.3 : Request invalid object Id be deleted - Returns 404 Not Found code
        [Fact]
        public void DeleteVarietal_Returns404NotFound_WhenUsingInvalidVarietalId()
        {

            // Arrange

            // Act
            var result = _controller.DeleteVarietal( -1 );

            //Assert
            Assert.IsType<NotFoundResult>( result.Result );

        }

        // Test 5.4 : Request invalid object Id be deleted - Object count unchanged
        [Fact]
        public void DeleteVarietal_UnchangedObjectCount_WhenUsingInvalidVarietalId()
        {

            // Arrange
            var varietal = new VarietalEntity
            {
                Varietal = "Test 5.4 Varietal"
            };
            _dbContext.Varietals.Add( varietal );
            _dbContext.SaveChanges( );

            var varietalId = varietal.Id;
            var objCount = _dbContext.Varietals.Count( );

            // Act
            var result = _controller.DeleteVarietal( varietalId + 1 );

            //Assert
            Assert.Equal( objCount, _dbContext.Varietals.Count( ) );

        }

    }

}