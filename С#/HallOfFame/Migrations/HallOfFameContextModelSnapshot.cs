﻿// <auto-generated />
using System;
using HallOfFame.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HallOfFame.Migrations
{
    [DbContext(typeof(HallOfFameContext))]
    partial class HallOfFameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HallOfFame.Models.Person", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("displayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("HallOfFame.Models.Skill", b =>
                {
                    b.Property<string>("name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("Personid")
                        .HasColumnType("bigint");

                    b.Property<byte>("level")
                        .HasColumnType("tinyint");

                    b.HasKey("name");

                    b.HasIndex("Personid");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("HallOfFame.Models.Skill", b =>
                {
                    b.HasOne("HallOfFame.Models.Person", null)
                        .WithMany("skills")
                        .HasForeignKey("Personid");
                });

            modelBuilder.Entity("HallOfFame.Models.Person", b =>
                {
                    b.Navigation("skills");
                });
#pragma warning restore 612, 618
        }
    }
}
