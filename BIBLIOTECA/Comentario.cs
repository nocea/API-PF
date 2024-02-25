using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA
{
    public class Comentario
    {
        [Key]
        public int? id_comentario { get; set; }
        public string? contenido_comentario { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // ID del post en el que se hizo el comentario
        public int? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
