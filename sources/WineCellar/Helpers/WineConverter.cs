

namespace WineCellar.Helpers
{
    using WineCellar.Entities;
    using WineCellar.Models;

    public class WineConverter
    {
        private WineCellarDBContext _context;

        public WineConverter( WineCellarDBContext context ) => _context = context;

        public WineEntity Convert( Wine source )
        {
            WineEntity destination = _context.Wines.Find( source.Id );

            if ( destination == null )
            {
                destination = new WineEntity( );
            }

            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Vintage = source.Vintage;
            destination.WhenPurchased = source.WhenPurchased;
            destination.BottlesPurchased = source.BottlesPurchased;
            destination.BottlesDrank = source.BottlesPurchased;

            destination.BottleSizeEntityId = source.BottleSizeId;
            destination.VarietalEntityId = source.VarietalId;
            destination.WineryEntityId = source.WineryId;

            return destination;
        }

        public Wine Convert( WineEntity source )
        {

            Wine destination = new Wine( )
            {
                Id = source.Id,
                Name = source.Name,
                Vintage = source.Vintage,
                WhenPurchased = source.WhenPurchased,
                BottlesPurchased = source.BottlesPurchased,
                BottlesDrank = source.BottlesDrank
            };

            // Fill-in fields from linked BottleSize tabe

            if ( source.BottleSize != null )
            {
                destination.BottleSizeId = source.BottleSizeEntityId;
                destination.FullBottleSize = source.BottleSize.ToString();
                destination.BottleVolume = source.BottleSize.BottleSize;
                destination.BottleMeasure = source.BottleSize.Volume;
            }

            if ( source.Varietal != null )
            {
                destination.VarietalId = source.VarietalEntityId;
                destination.Varietal = source.Varietal.Varietal;
            }

            if ( source.Winery != null )
            {
                destination.WineryId = source.WineryEntityId;
                destination.WineryName = source.Winery.Name;
            }

            return destination;
        }
    }
}

