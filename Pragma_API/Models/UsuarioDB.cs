using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pragma_API.Models
{
    [Table("usuario", Schema = "api")]
    public partial class UsuarioDB
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("email")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Required]
        [Column("senha")]
        [StringLength(100)]
        [Unicode(false)]
        public string? SenhaHash { get; set; }
        [Column("ativo")]
        public bool Ativo { get; set; }
    }
}
