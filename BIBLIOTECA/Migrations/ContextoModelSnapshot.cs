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

            modelBuilder.Entity("BIBLIOTECA.Comentario", b =>
                {
                    b.Property<int?>("id_comentario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("id_comentario"));

                    b.Property<int?>("PostId")
                        .HasColumnType("integer");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("integer");

                    b.Property<string>("contenido_comentario")
                        .HasColumnType("text");

                    b.HasKey("id_comentario");

                    b.HasIndex("PostId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("BIBLIOTECA.Post", b =>
                {
                    b.Property<int?>("id_post")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("id_post"));

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("integer");

                    b.Property<byte[]>("imagen_post")
                        .HasColumnType("bytea");

                    b.Property<string>("pie_post")
                        .HasColumnType("text");

                    b.Property<string>("titulo_post")
                        .HasColumnType("text");

                    b.HasKey("id_post");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Posts");
                });

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

                    b.Property<byte[]>("imagen_usuario")
                        .HasColumnType("bytea");

                    b.Property<int?>("movil_usuario")
                        .HasColumnType("integer");

                    b.Property<string>("nombreCompleto_usuario")
                        .HasColumnType("text");

                    b.Property<string>("passwd_usuario")
                        .HasColumnType("text");

                    b.Property<bool?>("registrado")
                        .HasColumnType("boolean");

                    b.Property<string>("rol_usuario")
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
                            registrado = true,
                            rol_usuario = "ADMIN"
                        });
                });

            modelBuilder.Entity("BIBLIOTECA.Comentario", b =>
                {
                    b.HasOne("BIBLIOTECA.Post", "Post")
                        .WithMany("Comentarios")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BIBLIOTECA.Usuario", "Usuario")
                        .WithMany("Comentarios")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Post");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("BIBLIOTECA.Post", b =>
                {
                    b.HasOne("BIBLIOTECA.Usuario", "Usuario")
                        .WithMany("Posts")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("BIBLIOTECA.Post", b =>
                {
                    b.Navigation("Comentarios");
                });

            modelBuilder.Entity("BIBLIOTECA.Usuario", b =>
                {
                    b.Navigation("Comentarios");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
