


namespace WineCellar.Helpers
{
    using WineCellar.Entities;
    using WineCellar.Models;

    public class WineryConverter
    {
        private WineCellarDBContext _context;

        public WineryConverter( WineCellarDBContext context ) => _context = context;

        public WineryEntity Convert( Winery source )
        {
            WineryEntity destination = _context.Wineries.Find( source.Id );

            if (destination == null)
            {
                destination = new WineryEntity( );
            }

            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.WebSite = source.WebSite;
            destination.EMail = source.EMail;
            destination.Phone = source.Phone;

            return destination;
        }

        public Winery Convert( WineryEntity source )
        {

            Winery destination = new Winery( )
            {
                Id = source.Id,
                Name = source.Name,
                WebSite = source.WebSite,
                EMail = source.EMail,
                Phone = source.Phone
            };

            return destination;
        }
    }
}

