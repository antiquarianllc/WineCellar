using Microsoft.EntityFrameworkCore;

namespace WineCellar.Models
{
    
    public class WineCellarDBContext : DbContext
    {

        public WineCellarDBContext( DbContextOptions<WineCellarDBContext> options ) : base( options )
        {

        }   

        public DbSet<WineVarietal> Varietals { get; set; }
        
    }

}