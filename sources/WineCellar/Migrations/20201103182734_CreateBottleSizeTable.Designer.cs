﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WineCellar.Entities;

namespace WineCellar.Migrations
{
    [DbContext(typeof(WineCellarDBContext))]
    [Migration("20201103182734_CreateBottleSizeTable")]
    partial class CreateBottleSizeTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WineCellar.Entities.BottleSizeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Default")
                        .HasColumnType("boolean");

                    b.Property<string>("BottleSize")
                        .HasColumnType("text");

                    b.Property<string>("Volume")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BottleSizes");
                });

            modelBuilder.Entity("WineCellar.Entities.VarietalEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Varietal")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Varietals");
                });
#pragma warning restore 612, 618
        }
    }
}
