
namespace WineCellar.Models
{

    public class Wine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vintage { get; set; }
        public string WhenPurchased { get; set; }
        public int BottlesPurchased { get; set; } = 1;
        public int BottlesDrank { get; set; } = 0;

        // Foreign Keys
        public int BottleSizeId { get; set; }
        public int VarietalId { get; set; }
        public int WineryId { get; set; }

        // Combined data members fro BottleSize model
        public string FullBottleSize { get; set; }
        public string BottleVolume { get; set; }
        public string BottleMeasure { get; set; }

        // Combined data memebers from WineVarietal model
        public string Varietal { get; set; }

        // Combined data members for Winery model
        public string WineryName { get; set; }

    }

}