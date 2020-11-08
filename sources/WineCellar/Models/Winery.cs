
namespace WineCellar.Models
{

    public class Winery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WebSite { get; set; } = null;
        public string EMail { get; set; } = null;
        public string Phone { get; set; } = null;
    }

}