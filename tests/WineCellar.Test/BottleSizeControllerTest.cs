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

    public class BottleSizeControllerTests : IDisposable
    {

        private DbContextOptionsBuilder<WineCellarDBContext> _optionsBuilder;
        private WineCellarDBContext _dbContext;
        private BottleSizeController _controller;

        public BottleSizeControllerTests()
        {

            // Create inMemory database for testing.
            _optionsBuilder = new DbContextOptionsBuilder<WineCellarDBContext>( );
            _optionsBuilder.UseInMemoryDatabase( "UnitTestInMemDB" );
            _dbContext = new WineCellarDBContext( _optionsBuilder.Options );

            // Create commands controller for testing.   
            _controller = new BottleSizeController( _dbContext, new NullLogger<BottleSizeController>( ) );

        }

        public void Dispose()
        {
            _optionsBuilder = null;

            // Cleanup any created database items.
            foreach (var BottleSize in _dbContext.BottleSizes)
            {
                _dbContext.BottleSizes.Remove( BottleSize );
            }
            _dbContext.SaveChanges( );
            _dbContext.Dispose( );

            _controller = null;
        }


        // ACTION 1 Tests : GET     /api/BottleSize

        // Test 1.1 : Request objects when none exist - Return "nothing"
        [Fact]
        public void GetBottleSizes_ReturnsZeroBottleSizes_WhenDBIsEmpty()
        {

            // Arrange

            // Act
            var results = _controller.GetBottleSizes( );

            //Assert
            Assert.Empty( results.Value );
        }

        // Test 1.2 : Request count of single object when only 1 exists in database
        [Fact]
        public void GetBottleSizes_ReturnsOneBottleSize_WhenDBHasOneBottleSize()
        {

            // Arrange
            var BottleSize = new BottleSizeEntity
            {
                BottleSize = "Test 1.2 BottleSize",
                Volume = "Test 1.2 Volume",
                Default = true
            };
            _dbContext.BottleSizes.Add( BottleSize );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetBottleSizes( );

            //Assert
            Assert.Single( results.Value );
        }

        // Test 1.3 : Request count of N objects when N exists in database
        [Fact]
        public void GetBottleSizes_ReturnsBottleSizes_WhenDBHasNBottleSizes()
        {

            // Arrange
            var BottleSize = new BottleSizeEntity
            {
                BottleSize = "Test 1.3 BottleSize 1",
                Volume = "Test 1.3 Volume 1",
                Default = false
        };
            var BottleSize2 = new BottleSizeEntity
            {
                BottleSize = "Test 1.3 BottleSize 2",
                Volume = "Test 1.3 Volume 2",
                Default = true
            };

            _dbContext.BottleSizes.Add( BottleSize );
            _dbContext.BottleSizes.Add( BottleSize2 );
            _dbContext.SaveChanges( );

            // Act
            var results = _controller.GetBottleSizes( );

            //Assert
            Assert.Equal( 2, results.Value.Count( ) );

        }

        // Test 1.4 : Request count of N objects when N exists in database
        [Fact]
        public void GetBottleSizes_ReturnBottleSizes_WithCorrectType()
        {

            // Arrange

            // Act
            var results = _controller.GetBottleSizes( );

            //Assert
            Assert.IsType<ActionResult<IEnumerable<BottleSize>>>( results );

        }

        // ACTION 2 Tests : GET     /api/BottleSize/{id}

        // Test 2.1 : Request object by ID when none exist - Return null object value
        [Fact]
        public void GetBottleSize_ReturnsNullValue_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetBottleSize( 0 );

            //Assert
            Assert.Null( result.Value );

        }

        // Test 2.2 : Request object by ID when none exist - Return 404 Not Found Return Code
        [Fact]
        public void GetBottleSize_Returns404NotFound_WhenUsingInvalidID()
        {

            // Arrange
            // DB should be empty.

            // Act
            var result = _controller.GetBottleSize( 0 );

            //Assert
            Assert.IsType<NotFoundResult>( result.Result );

        }

        // Test 2.3 : Request object by valid ID - Check Correct Return Type
        [Fact]
        public void GetBottleSize_ReturnsItemOfCorrectType_WhenUsingValidID()
        {

            // Arrange
            var BottleSize = new BottleSizeEntity
            {
                BottleSize = "Test 2.3 BottleSize",
                Volume = "Test 2.3 Volume",
                Default = false
            };
            _dbContext.BottleSizes.Add( BottleSize );
            _dbContext.SaveChanges( );

            var cmdId = BottleSize.Id;

            // Act
            var result = _controller.GetBottleSize( cmdId );

            //Assert
            Assert.IsType<ActionResult<BottleSize>>( result );

        }

        // Test 2.4 : Request object by valid ID - Check correct item returned
        [Fact]
        public void GetBottleSize_ReturnCorrectBottleSize_WhenUsingValidID()
        {

            // Arrange
            var BottleSize = new BottleSizeEntity
            {
                BottleSize = "Test 2.4 BottleSize",
                Volume = "Test 2.4 Volume",
                Default = false
            };
            _dbContext.BottleSizes.Add( BottleSize );
            _dbContext.SaveChanges( );

            var cmdId = BottleSize.Id;

            // Act
            var result = _controller.GetBottleSize( cmdId );

            //Assert
            Assert.Equal( cmdId, result.Value.Id );

        }


        // ACTION 3 Tests : POST     /api/BottleSize

        // Test 3.1 : Create new BottleSize database item - Database record count increased.
        [Fact]
        public void PostBottleSize_BottleSizeCountIncremented_WhenUsingValidBottleSize()
        {

            // Arrange
            var BottleSize = new BottleSize
            {
                Size = "Test 3.1 BottleSize",
                VolumeMeasure = "Test 3.1 Volume Measure",
                Default = false
            };

            var oldCount = _dbContext.BottleSizes.Count( );

            // Act
            var result = _controller.PostBottleSize( BottleSize );

            //Assert
            Assert.Equal( oldCount + 1, _dbContext.BottleSizes.Count( ) );

        }

        // Test 3.2 : Create new BottleSize database item - Return item of proper type.
        [Fact]
        public void PostBottleSize_ReturnsItemOfCorrectType_WhenUsingValidID()
        {

            // Arrange
            var BottleSize = new BottleSize
            {
                Size = "Test 3.2 BottleSize",
                VolumeMeasure = "Test 3.2 Volume Measure",
                Default = true
        };

            // Act
            var result = _controller.PostBottleSize( BottleSize );

            //Assert
            Assert.IsType<CreatedAtActionResult>( result.Result );

        }

        // Test 3.3 : Create duplicate BottleSize record in Databse - Returns 409 Conflict code
        [Fact]
        public void PostBottleSize_Returns409Conflict_WhenUsingDuplicateBottleSize()
        {

            // Arrange
            var BottleSize = new BottleSize
            {
                Size = "Test 3.3 BottleSize",
                VolumeMeasure = "Test 3.3 Volume Measure",
                Default = false
            };

            // Act
            var result = _controller.PostBottleSize( BottleSize );
            //--if ( Assert.IsType<CreatedAtActionResult>( result.Result ) )
            {
                // Attempt to add BottleSize a 2nd time.
                result = _controller.PostBottleSize( BottleSize );
            }

            //Assert
            Assert.IsType<ConflictResult>( result.Result );

        }


        // ACTION 5 Tests : DELETE          /api/BottleSize/{id}

        // Test 5.1 : Request valid object Id be deleted - Results in object count decremented by 1
        [Fact]
        public void DeleteBottleSize_ObjectCountDecrementedBy1_WhenUsingValidBottleSizeId()
        {

            // Arrange
            var BottleSize = new BottleSizeEntity
            {
                BottleSize = "Test 5.1 BottleSize",
                Volume = "Test 5.1 Volume",
                Default = false
            };
            _dbContext.BottleSizes.Add( BottleSize );
            _dbContext.SaveChanges( );

            var BottleSizeId = BottleSize.Id;
            var objCount = _dbContext.BottleSizes.Count( );

            // Act
            _controller.DeleteBottleSize( BottleSizeId );

            //Assert
            Assert.Equal( objCount - 1, _dbContext.BottleSizes.Count( ) );
        }

        // Test 5.2 : Request valid objectId  be deleted - Returns 200 OK code
        [Fact]
        public void DeleteBottleSize_Returns200OK_WhenUsingValidBottleSizeId()
        {

            // Arrange
            var BottleSize = new BottleSizeEntity
            {
                BottleSize = "Test 5.2 BottleSize",
                Volume = "Test 5.2 Volume",
                Default = false
            };
            _dbContext.BottleSizes.Add( BottleSize );
            _dbContext.SaveChanges( );

            var BottleSizeId = BottleSize.Id;

            // Act
            var result = _controller.DeleteBottleSize( BottleSizeId );

            //Assert
            Assert.Null( result.Result );
        }

        // Test 5.3 : Request invalid object Id be deleted - Returns 404 Not Found code
        [Fact]
        public void DeleteBottleSize_Returns404NotFound_WhenUsingInvalidBottleSizeId()
        {

            // Arrange

            // Act
            var result = _controller.DeleteBottleSize( -1 );

            //Assert
            Assert.IsType<NotFoundResult>( result.Result );

        }

        // Test 5.4 : Request invalid object Id be deleted - Object count unchanged
        [Fact]
        public void DeleteBottleSize_UnchangedObjectCount_WhenUsingInvalidBottleSizeId()
        {

            // Arrange
            var BottleSize = new BottleSizeEntity
            {
                BottleSize = "Test 5.4 BottleSize",
                Volume = "Test 5.4 Volume",
                Default = true
            };
            _dbContext.BottleSizes.Add( BottleSize );
            _dbContext.SaveChanges( );

            var BottleSizeId = BottleSize.Id;
            var objCount = _dbContext.BottleSizes.Count( );

            // Act
            var result = _controller.DeleteBottleSize( BottleSizeId + 1 );

            //Assert
            Assert.Equal( objCount, _dbContext.BottleSizes.Count( ) );

        }

    }

}