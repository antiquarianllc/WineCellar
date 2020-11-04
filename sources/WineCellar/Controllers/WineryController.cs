
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
    public class WineryController : ControllerBase
    {
        private readonly WineCellarDBContext _context;

        private readonly ILogger<WineryController> _logger;

        private WineryConverter _converter;

        public WineryController( WineCellarDBContext context, ILogger<WineryController> logger )
        {
            _context = context;
            _logger = logger;

            _converter = new WineryConverter( _context );
        }

        // GET:  api/Winery
        [HttpGet]
        public ActionResult<IEnumerable<Winery>> GetWinerys()
        {
            _logger.LogTrace( "Get all Winerys" );
            List<Winery> Winerys = new List<Winery>( ); ;
            var WineryEntities = _context.Wineries;

            foreach (WineryEntity entity in WineryEntities)
            {
                Winerys.Add( _converter.Convert( entity ) );
            }

            return Winerys;
        }

        // GET:      api/Winery/{Id}
        //[Authorize]
        [HttpGet( "{id}" )]
        public ActionResult<Winery> GetWinery( int id )
        {
            _logger.LogTrace( "Get Winery with Id " + id );
            var WineryEntity = _context.Wineries.Find( id );

            if (WineryEntity == null)
            {
                _logger.LogInformation( "Cannot find Winery associated with Id " + id );
                return NotFound( );
            }

            _logger.LogTrace( "Found Winery for Id " + id );
            return _converter.Convert( WineryEntity );
        }

        // POST:    api/Winery
        [HttpPost]
        public ActionResult<Winery> PostWinery( Winery Winery )
        {
            _logger.LogTrace( "Create new Winery " + Winery.ToString( ) );

            // Convert model to entity.
            var WineryEntity = _converter.Convert( Winery );

            // Only allow one instance of each Winery.
            var checkWinery = from v in _context.Wineries
                                  where ( v.Name == WineryEntity.Name )
                                  select v;

            if (checkWinery.FirstOrDefault( ) == null)
            {
                _context.Wineries.Add( WineryEntity );

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


            return CreatedAtAction( "GetWinery", WineryEntity.Id );
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

        // DELETE:     api/Winery/{id}
        [HttpDelete]
        public ActionResult<Winery> DeleteWinery( int id )
        {
            _logger.LogTrace( "Delete Winery with Id " + id );
            var WineryEntity = _context.Wineries.Find( id );

            if (WineryEntity == null)
            {
                return NotFound( );
            }

            _context.Wineries.Remove( WineryEntity );
            _context.SaveChanges( );

            return _converter.Convert( WineryEntity );
        }

    }

}

