
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
        public ActionResult<IEnumerable<Winery>> GetWinery()
        {
            _logger.LogTrace( "Get all Wineries" );
            List<Winery> wineries = new List<Winery>( ); ;
            var wineEntities = _context.Wineries;

            foreach (WineryEntity entity in wineEntities)
            {
                wineries.Add( _converter.Convert( entity ) );
            }

            return wineries;
        }

        // GET:      api/Winery/{Id}
        //[Authorize]
        [HttpGet( "{id}" )]
        public ActionResult<Winery> GetWinery( int id )
        {
            _logger.LogTrace( "Get Winery with Id " + id );
            var wineryEntity = _context.Wineries.Find( id );

            if (wineryEntity == null)
            {
                _logger.LogInformation( "Cannot find Winery associated with Id " + id );
                return NotFound( );
            }

            _logger.LogTrace( "Found Winery for Id " + id );
            return _converter.Convert( wineryEntity );
        }

        // POST:    api/Winery
        [HttpPost]
        public ActionResult<Winery> PostWinery( Winery winery )
        {
            _logger.LogTrace( "Create new Winery " + winery.ToString( ) );

            // Convert model to entity.
            var wineryEntity = _converter.Convert( winery );

            // Only allow one instance of each Winery.
            var checkWinery = from v in _context.Wineries
                                  where ( v.Name == wineryEntity.Name )
                                  select v;

            if (checkWinery.FirstOrDefault( ) == null)
            {
                _context.Wineries.Add( wineryEntity );

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

            return CreatedAtAction( "GetWinery", wineryEntity.Id );
        }


        // PUT:     api/Winery/{id}
        [HttpPut]
        public ActionResult PutWinery( int id, Winery winery )
        {
            _logger.LogTrace( "Update Winery with Id " + id );
            var wineryEntity = _context.Wineries.Find( id );

            // Can only update an existing winery.
            if (wineryEntity == null)
            {
                return BadRequest( );
            }

            // Convert parameter model to entity.
            var paramWineryEntity = _converter.Convert( winery );

            // Generate an updated winery entity.
            wineryEntity.Name = paramWineryEntity.Name;
            wineryEntity.EMail = paramWineryEntity.EMail;
            wineryEntity.Phone = paramWineryEntity.Phone;
            wineryEntity.WebSite = paramWineryEntity.WebSite;

            // Flush change to database.
            _context.Entry( wineryEntity ).State = EntityState.Modified;
            _context.SaveChanges( );

            return NoContent( );
        }


        // DELETE:     api/Winery/{id}
        [HttpDelete]
        public ActionResult<Winery> DeleteWinery( int id )
        {
            _logger.LogTrace( "Delete Winery with Id " + id );
            var wineryEntity = _context.Wineries.Find( id );

            if (wineryEntity == null)
            {
                return NotFound( );
            }

            _context.Wineries.Remove( wineryEntity );
            _context.SaveChanges( );

            return _converter.Convert( wineryEntity );
        }

    }

}

