


namespace WineCellar.Helpers
{
    using WineCellar.Entities;
    using WineCellar.Models;

    public class BottleSizeConverter
    {
        private WineCellarDBContext _context;

        public BottleSizeConverter( WineCellarDBContext context ) => _context = context;

        public BottleSizeEntity Convert( BottleSizeModel source )
        {
            BottleSizeEntity destination = _context.BottleSizes.Find( source.Id );

            if (destination == null)
            {
                destination = new BottleSizeEntity( );
            }

            destination.Id = source.Id;
            destination.BottleSize = source.Size;
            destination.Volume = source.VolumeMeasure;

            return destination;
        }

        public BottleSizeModel Convert( BottleSizeEntity source )
        {

            BottleSizeModel destination = new BottleSizeModel( )
            {
                Id = source.Id,
                Size = source.BottleSize,
                VolumeMeasure = source.Volume,
                Default = source.Default
        };

            return destination;
        }
    }
}

