
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace WineCellar.Controllers
{
    using WineCellar.Entities;
    using WineCellar.Helpers;
    using WineCellar.Models;

    [Route( "api/[controller]" )]
    [ApiController]
    public class WineController : ControllerBase
    {
        private readonly WineCellarDBContext _context;

        private readonly ILogger<WineController> _logger;

        private WineConverter _converter;


        public WineController( WineCellarDBContext context, ILogger<WineController> logger )
        {
            _context = context;
            _logger = logger;

            _converter = new WineConverter( _context );
        }

        // GET:  api/Wine
        [HttpGet]
        public ActionResult<IEnumerable<Wine>> GetWine()
        {
            _logger.LogTrace( "Get all Wines" );
            List<Wine> wines = new List<Wine>( ); ;
            var wineEntities = _context.Wines;

            foreach (WineEntity entity in wineEntities)
            {
                wines.Add( _converter.Convert( entity ) );
            }

            return wines;
        }

        // GET:      api/Wine/{Id}
        //[Authorize]
        [HttpGet( "{id}" )]
        public ActionResult<Wine> GetWine( int id )
        {
            _logger.LogTrace( "Get Wine with Id " + id );
            var wineEntity = _context.Wines.Find( id );

            if (wineEntity == null)
            {
                _logger.LogInformation( "Cannot find Wine associated with Id " + id );
                return NotFound( );
            }

            _logger.LogTrace( "Found Wine for Id " + id );
            return _converter.Convert( wineEntity );
        }

        // POST:    api/Wine
        [HttpPost]
        public ActionResult<Wine> PostWine( Wine wine )
        {
            _logger.LogTrace( "Create new Wine " + wine.ToString( ) );

            // Convert model to entity.
            var wineEntity = _converter.Convert( wine );

            // BottleSize must be specified.
            var bottleSizeEntity = _context.BottleSizes.Find( wineEntity.BottleSizeEntityId );
            var varietalEntity = _context.Varietals.Find( wineEntity.VarietalEntityId );
            if ( ( bottleSizeEntity == null ) || ( varietalEntity == null ) )
            {
                return NotFound( );
            }

            _context.Wines.Add( wineEntity );

            try
            {
                _context.SaveChanges( );
            }
            catch
            {
                return BadRequest( );
            }

            return CreatedAtAction( "GetWine", wineEntity.Id );
        }


        // PUT:     api/Wine/{id}
        [HttpPut]
        public ActionResult PutWine( int id, Wine wine )
        {
            _logger.LogTrace( "Update Wine with Id " + id );
            var wineEntity = _context.Wines.Find( id );

            // Can only update an existing wine.
            if ( wineEntity == null )
            {
                return NotFound( );
            }

            // Convert parameter model to entity.
            var paramWineEntity = _converter.Convert( wine );

            // Generate an updated wine entity.
            wineEntity.Name = paramWineEntity.Name;
            wineEntity.Vintage = paramWineEntity.Vintage;
            wineEntity.WhenPurchased = paramWineEntity.WhenPurchased;
            wineEntity.BottlesPurchased = paramWineEntity.BottlesPurchased;
            wineEntity.BottlesDrank = paramWineEntity.BottlesDrank;

            wineEntity.BottleSizeEntityId = paramWineEntity.BottleSizeEntityId;
            wineEntity.VarietalEntityId = paramWineEntity.VarietalEntityId;
            wineEntity.WineryEntityId = paramWineEntity.WineryEntityId;

            // Flush change to database.
            _context.Entry( wineEntity ).State = EntityState.Modified;
            _context.SaveChanges( );

            return NoContent( );
        }


        // DELETE:     api/Wine/{id}
        [HttpDelete]
        public ActionResult<Wine> DeleteWine( int id )
        {
            _logger.LogTrace( "Delete Wine with Id " + id );
            var wineEntity = _context.Wines.Find( id );

            if (wineEntity == null)
            {
                return NotFound( );
            }

            _context.Wines.Remove( wineEntity );
            _context.SaveChanges( );

            return _converter.Convert( wineEntity );
        }

    }

}

