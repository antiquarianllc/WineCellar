
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
    public class VarietalController : ControllerBase
    {
        private readonly WineCellarDBContext _context;

        private readonly ILogger<VarietalController> _logger;

        private VarietalConverter _converter;

        public VarietalController( WineCellarDBContext context, ILogger<VarietalController> logger )
        {
            _context = context;
            _logger = logger;

            _converter = new VarietalConverter( _context );
        }

        // GET:  api/varietal
        [HttpGet]
        public ActionResult<IEnumerable<WineVarietal>> GetVarietals( )
        {
            _logger.LogTrace( "Get all varietals" );
            List<WineVarietal> varietals = new List<WineVarietal>( );;
            var varietalEntities = _context.Varietals;

            foreach ( VarietalEntity entity in varietalEntities )
            {
                varietals.Add( _converter.Convert( entity ) );
            }

            return varietals;
        }

        // GET:      api/varietal/{Id}
        //[Authorize]
        [HttpGet( "{id}" )]
        public ActionResult<WineVarietal> GetVarietal( int id )
        {
            _logger.LogTrace( "Get varietal with Id " + id );
            var varietalEntity = _context.Varietals.Find( id );

            if (varietalEntity == null)
            {
                _logger.LogInformation( "Cannot find varietal associated with Id " + id );
                return NotFound( );
            }
            
            _logger.LogTrace( "Found varietal for Id " + id );
            return _converter.Convert( varietalEntity );
        }

        // POST:    api/commands
        [HttpPost]
        public ActionResult<WineVarietal> PostVarietal( WineVarietal varietal )
        {
            _logger.LogTrace( "Create new varietal with name " + varietal.Varietal );

            // Convert model to entity.
            var varietalEntity = _converter.Convert( varietal );

            // Only allow one instance of each varietal.
            var checkVarietal = from v in _context.Varietals
                                where ( v.Varietal == varietalEntity.Varietal )
                                select v;

            if (checkVarietal.FirstOrDefault( ) == null)
            {
                _context.Varietals.Add( varietalEntity );

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


            return CreatedAtAction( "GetVarietal", varietalEntity.Id  );
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

        // DELETE:     api/varietal/{id}
        [HttpDelete]
        public ActionResult<WineVarietal> DeleteVarietal( int id )
        {
            _logger.LogTrace( "Delete varietal with Id " + id );
            var varietalEntity = _context.Varietals.Find( id );

            if ( varietalEntity == null )
            {
                return NotFound( );
            }

            _context.Varietals.Remove( varietalEntity );
            _context.SaveChanges( );

            return _converter.Convert( varietalEntity );
        }

    }

}


