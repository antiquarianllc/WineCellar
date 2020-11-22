
namespace WineCellar.Models
{

    public class BottleSize
    {
        public int Id { get; set; }
        public string Size { get; set; } = "750";
        public string VolumeMeasure { get; set; } = "ml";
        public bool Default { get; set; } = false;


        public override string ToString( )
        {
            return Size + VolumeMeasure;
        }
    }

}