
namespace WineCellar.Entities
{

    public class BottleSizeEntity
    {
        public int Id { get; set; }
        public string BottleSize { get; set; }
        public string Volume { get; set; }
        public bool Default { get; set; } = false;
    }

}