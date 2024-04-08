using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pragma_API.Models
{
    public partial class Usuario
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public bool? Ativo { get; set; }
        public bool SenhaCorreta { get; set; }
    }
}
