using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA
{
    public class Token
    {
        [Key]
        public int id_token { get; set; }
        public string? cadena_token { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime fechaFin_token { get; set; }
    }
}
