﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShortUrlGen.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShortUrlGen.UrlMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LongUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("ShortUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("UrlMappings");
                });
#pragma warning restore 612, 618
        }
    }
}
