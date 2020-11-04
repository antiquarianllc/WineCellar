
namespace WineCellar.Models
{

    public class BottleSizeModel
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Volume { get; set; }
        public bool Default { get; set; }


        public override string ToString( )
        {
            return Size + Volume;
        }
    }

}