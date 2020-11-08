
namespace WineCellar.Entities
{

    public class WineEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vintage { get; set; }
        public string WhenPurchased { get; set; }
        public string BottlesPurchased { get; set; }
        public string BottlesDrank { get; set; }

        // Foreign Keys
        public int BottleSizeId { get; set; }
        public int VarietalId { get;set; }
        public int WineryId { get; set; }
   }

}