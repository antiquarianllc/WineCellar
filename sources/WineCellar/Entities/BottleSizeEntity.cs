
namespace WineCellar.Entities
{

    public class BottleSizeEntity
    {
        public int Id { get; set; }
        public string BottleSize { get; set; } = "750";
        public string Volume { get; set; } = "ml";
        public bool Default { get; set; } = false;
    }

}