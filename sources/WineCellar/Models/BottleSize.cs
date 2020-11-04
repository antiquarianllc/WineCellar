
namespace WineCellar.Models
{

    public class BottleSize
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string VolumeMeasure { get; set; }
        public bool Default { get; set; }


        public override string ToString( )
        {
            return Size + VolumeMeasure;
        }
    }

}