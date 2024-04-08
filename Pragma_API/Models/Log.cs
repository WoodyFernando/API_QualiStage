using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pragma_API.Models
{
    [Table("log", Schema = "api")]
    public partial class Log
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("usuario_id")]
        public int? UsuarioId { get; set; }
        [Column("email")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Column("data_hora", TypeName = "datetime")]
        public DateTime DataHora { get; set; }
        [Column("erro")]
        public bool Erro { get; set; }
        [Required]
        [Column("url")]
        [StringLength(500)]
        [Unicode(false)]
        public string? Url { get; set; }
        [Column("mensagem")]
        [Unicode(false)]
        public string? Mensagem { get; set; }
        public string? Parametros { get; set; }
    }
}