
using System.Collections.Generic;

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
        public ActionResult<IEnumerable<VarietalModel>> GetVarietals( )
        {
            _logger.LogTrace( "Get all varietals" );
            List<VarietalModel> varietals = new List<VarietalModel>( );;
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
        public ActionResult<VarietalModel> GetVarietal( int id )
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
/*
        // POST:    api/commands
        [HttpPost]
        public ActionResult<Command> PostCommandItem( Command command )
        {

            _context.CommandItems.Add( command );

            try
            {
                _context.SaveChanges( );
            }
            catch
            {
                return BadRequest( );
            }

            return CreatedAtAction( "GetCommandItem", new Command { Id = command.Id }, command );
        }

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

        // DELETE:     api/commands/{id}
        [HttpDelete]
        public ActionResult<Command> DeleteCommandItem( int id )
        {

            var commandItem = _context.CommandItems.Find( id );

            if ( commandItem == null )
            {
                return NotFound( );
            }

            _context.CommandItems.Remove( commandItem );
            _context.SaveChanges( );

            return commandItem;
        }
*/
    }

}


