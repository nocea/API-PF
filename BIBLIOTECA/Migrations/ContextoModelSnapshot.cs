﻿// <auto-generated />
using System;
using BIBLIOTECA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BIBLIOTECA.Migrations
{
    [DbContext(typeof(Contexto))]
    partial class ContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BIBLIOTECA.Token", b =>
                {
                    b.Property<int>("id_token")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id_token"));

                    b.Property<string>("cadena_token")
                        .HasColumnType("text");

                    b.Property<DateTime>("fechaFin_token")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id_token");

                    b.ToTable("tokens");
                });

            modelBuilder.Entity("BIBLIOTECA.Usuario", b =>
                {
                    b.Property<int?>("id_usuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("id_usuario"));

                    b.Property<string>("alias_usuario")
                        .HasColumnType("text");

                    b.Property<string>("email_usuario")
                        .HasColumnType("text");

                    b.Property<int?>("movil_usuario")
                        .HasColumnType("integer");

                    b.Property<string>("nombreCompleto_usuario")
                        .HasColumnType("text");

                    b.Property<string>("passwd_usuario")
                        .HasColumnType("text");

                    b.Property<string>("rol_usuario")
                        .HasColumnType("text");

                    b.Property<string>("token_usuario")
                        .HasColumnType("text");

                    b.HasKey("id_usuario");

                    b.ToTable("usuarios");

                    b.HasData(
                        new
                        {
                            id_usuario = 1,
                            email_usuario = "admin@admin",
                            nombreCompleto_usuario = "Administrador",
                            passwd_usuario = "eb31b34db2f22a0030aa4f9306b77bfbbda728967e9feb88ea79ac206e657d29",
                            rol_usuario = "ADMIN"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
