
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
    public class BottleSizeController : ControllerBase
    {
        private readonly WineCellarDBContext _context;

        private readonly ILogger<BottleSizeController> _logger;

        private BottleSizeConverter _converter;

        public BottleSizeController( WineCellarDBContext context, ILogger<BottleSizeController> logger )
        {
            _context = context;
            _logger = logger;

            _converter = new BottleSizeConverter( _context );
        }

        // GET:  api/BottleSize
        [HttpGet]
        public ActionResult<IEnumerable<BottleSize>> GetBottleSizes()
        {
            _logger.LogTrace( "Get all BottleSizes" );
            List<BottleSize> bottleSizes = new List<BottleSize>( ); ;
            var bottleSizeEntities = _context.BottleSizes;

            foreach (BottleSizeEntity entity in bottleSizeEntities)
            {
                bottleSizes.Add( _converter.Convert( entity ) );
            }

            return bottleSizes;
        }

        // GET:      api/BottleSize/{Id}
        //[Authorize]
        [HttpGet( "{id}" )]
        public ActionResult<BottleSize> GetBottleSize( int id )
        {
            _logger.LogTrace( "Get BottleSize with Id " + id );
            var bottleSizeEntity = _context.BottleSizes.Find( id );

            if (bottleSizeEntity == null)
            {
                _logger.LogInformation( "Cannot find BottleSize associated with Id " + id );
                return NotFound( );
            }

            _logger.LogTrace( "Found BottleSize for Id " + id );
            return _converter.Convert( bottleSizeEntity );
        }

        // POST:    api/commands
        [HttpPost]
        public ActionResult<BottleSize> PostBottleSize( BottleSize bottleSize )
        {
            _logger.LogTrace( "Create new BottleSize " + bottleSize.ToString( ) );

            // Convert model to entity.
            var bottleSizeEntity = _converter.Convert( bottleSize );

            // Only allow one instance of each BottleSize.
            var checkBottleSize = from v in _context.BottleSizes
                                where ( v.BottleSize == bottleSizeEntity.BottleSize )
                                select v;

            if (checkBottleSize.FirstOrDefault( ) == null)
            {
                _context.BottleSizes.Add( bottleSizeEntity );

                try
                {
                    _context.SaveChanges( );
                }
                catch
                {
                    return BadRequest( );
                }
            }
            else
            {
                return Conflict( );
            }

            return CreatedAtAction( "GetBottleSize", bottleSizeEntity.Id );
        }

        /*
                // PUT:     api/commands/{id}
                [HttpPut]
                public ActionResult PutCommandItem( int id, Command command )
                {

                    if ( id != command.Id )
                    {
                        return BadRequest( );
                    }

                    _context.Entry( command ).State = EntityState.Modified;
                    _context.SaveChanges( );

                    return NoContent( );
                }
        */

        // DELETE:     api/BottleSize/{id}
        [HttpDelete]
        public ActionResult<BottleSize> DeleteBottleSize( int id )
        {
            _logger.LogTrace( "Delete BottleSize with Id " + id );
            var bottleSizeEntity = _context.BottleSizes.Find( id );

            if (bottleSizeEntity == null)
            {
                return NotFound( );
            }

            _context.BottleSizes.Remove( bottleSizeEntity );
            _context.SaveChanges( );

            return _converter.Convert( bottleSizeEntity );
        }

    }

}

