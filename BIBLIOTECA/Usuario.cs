using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA
{
    public class Usuario
    {
        [Key]
        public int? id_usuario { get; set; }
        public string? nombreCompleto_usuario { get; set; }
        public string? rol_usuario { get; set; }
        public int? movil_usuario { get; set; }
        public string? alias_usuario { get; set; }
        public string? email_usuario { get; set; }
        public string? passwd_usuario { get; set; }
        public byte[]? imagen_usuario { get; set; }
    }
}
