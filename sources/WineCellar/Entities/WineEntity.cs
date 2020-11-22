
namespace WineCellar.Entities
{

    public class WineEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vintage { get; set; }
        public string WhenPurchased { get; set; }
        public int BottlesPurchased { get; set; } = 1;
        public int BottlesDrank { get; set; } = 0;

        // Foreign Keys
        public int BottleSizeEntityId { get; set; }
        public int VarietalEntityId { get; set; }
        public int WineryEntityId { get; set; }

        // Navigation Properties
        public virtual BottleSizeEntity BottleSize { get; set; }
        public virtual VarietalEntity Varietal { get; set; }
        public virtual WineryEntity Winery { get; set; }
    }

}