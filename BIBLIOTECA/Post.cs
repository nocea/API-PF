using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA
{
    public class Post
    {
        [Key]
        public int? id_post { get; set; }
        public string? titulo_post { get; set; }
        public string? pie_post { get; set; }
        public byte[]? imagen_post { get; set; }

        public int? UsuarioId { get; set; }

        // Propiedad de navegación al usuario que publicó el post
        public Usuario? Usuario { get; set; }

    }
}
