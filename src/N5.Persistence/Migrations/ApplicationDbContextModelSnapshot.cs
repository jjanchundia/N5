﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using N5.Persistence;

#nullable disable

namespace N5.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("N5.Domain.Permiso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApellidoEmpleado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaPermiso")
                        .HasColumnType("datetime2");

                    b.Property<string>("NombreEmpleado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoPermisoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TipoPermisoId");

                    b.ToTable("Permiso");
                });

            modelBuilder.Entity("N5.Domain.TipoPermiso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TipoPermiso");
                });

            modelBuilder.Entity("N5.Domain.Permiso", b =>
                {
                    b.HasOne("N5.Domain.TipoPermiso", "TipoPermiso")
                        .WithMany("Permisos")
                        .HasForeignKey("TipoPermisoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TipoPermiso");
                });

            modelBuilder.Entity("N5.Domain.TipoPermiso", b =>
                {
                    b.Navigation("Permisos");
                });
#pragma warning restore 612, 618
        }
    }
}