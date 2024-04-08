using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pragma_API.Model
{
    public class CupomVenda
    {
        
        public int? Pedido { get; set; }
        
        public DateTime? Date { get; set; }
      
        public string? Nome { get; set; }
       
        public string? TipoDocumento { get; set; }
       
        public string? Documento { get; set; }
        
        public string? Endereco { get; set; }
        
        public string? Bairo { get; set; }
        
        public string? Cep { get; set; }
       
        public string? Cidade { get; set; }
       
        public string? Uf { get; set; }
        
        public string? Email { get; set; }
    }
}
