    using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    id_usuario = 1,
                    nombreCompleto_usuario = "Administrador",
                    rol_usuario = "ADMIN",
                    email_usuario="admin@admin",
                    //Admin1234
                    passwd_usuario="eb31b34db2f22a0030aa4f9306b77bfbbda728967e9feb88ea79ac206e657d29",
                    registrado=true
                }
            );
            modelBuilder.Entity<Post>()
           .HasOne(p => p.Usuario)
           .WithMany(u => u.Posts)
           .HasForeignKey(p => p.UsuarioId);
        }
        //Entidades(dbSet)
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Token> tokens { get; set; }

    }
}
