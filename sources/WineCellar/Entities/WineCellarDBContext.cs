using System;
//--using System.Data.Entity;
using Microsoft.EntityFrameworkCore;


namespace WineCellar.Entities
{

    public class WineCellarDBContext : DbContext
    {

        public WineCellarDBContext( DbContextOptions<WineCellarDBContext> options ) : base( options )
        {

        }

        public DbSet<VarietalEntity> Varietals { get; set; }

        public DbSet<BottleSizeEntity> BottleSizes { get; set; }

        public DbSet<WineryEntity> Wineries { get; set; }

        public DbSet<WineEntity> Wines { get; set; }

    }

}
