
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
        public ActionResult<IEnumerable<BottleSizeModel>> GetBottleSizes()
        {
            _logger.LogTrace( "Get all BottleSizes" );
            List<BottleSizeModel> BottleSizes = new List<BottleSizeModel>( ); ;
            var BottleSizeEntities = _context.BottleSizes;

            foreach (BottleSizeEntity entity in BottleSizeEntities)
            {
                BottleSizes.Add( _converter.Convert( entity ) );
            }

            return BottleSizes;
        }

        // GET:      api/BottleSize/{Id}
        //[Authorize]
        [HttpGet( "{id}" )]
        public ActionResult<BottleSizeModel> GetBottleSize( int id )
        {
            _logger.LogTrace( "Get BottleSize with Id " + id );
            var BottleSizeEntity = _context.BottleSizes.Find( id );

            if (BottleSizeEntity == null)
            {
                _logger.LogInformation( "Cannot find BottleSize associated with Id " + id );
                return NotFound( );
            }

            _logger.LogTrace( "Found BottleSize for Id " + id );
            return _converter.Convert( BottleSizeEntity );
        }

        // POST:    api/commands
        [HttpPost]
        public ActionResult<BottleSizeModel> PostBottleSize( BottleSizeModel BottleSizeModel )
        {
            _logger.LogTrace( "Create new BottleSize " + BottleSizeModel.ToString( ) );

            // Convert model to entity.
            var BottleSizeEntity = _converter.Convert( BottleSizeModel );

            // Only allow one instance of each BottleSize.
            var checkBottleSize = from v in _context.BottleSizes
                                where ( v.BottleSize == BottleSizeEntity.BottleSize )
                                select v;

            if (checkBottleSize.FirstOrDefault( ) == null)
            {
                _context.BottleSizes.Add( BottleSizeEntity );

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


            return CreatedAtAction( "GetBottleSize", BottleSizeEntity.Id );
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
        public ActionResult<BottleSizeModel> DeleteBottleSize( int id )
        {
            _logger.LogTrace( "Delete BottleSize with Id " + id );
            var BottleSizeEntity = _context.BottleSizes.Find( id );

            if (BottleSizeEntity == null)
            {
                return NotFound( );
            }

            _context.BottleSizes.Remove( BottleSizeEntity );
            _context.SaveChanges( );

            return _converter.Convert( BottleSizeEntity );
        }

    }

}

