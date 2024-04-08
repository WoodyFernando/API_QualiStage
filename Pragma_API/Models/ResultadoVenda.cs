using Microsoft.EntityFrameworkCore;

namespace Pragma_API.Models
{
    [Keyless]
    public partial class ResultadoVenda
    {
        public int TotalTransacoes { get; set; }

        public DateTime DataGeracao { get; set; }
        public DateTime DataInicioBusca { get; set; }
        public DateTime DatafinalBusca { get; set; }

        public decimal ValorTotalTransacao { get; set; }

        public List<Venda>? Transacoes { get; set; }
    }
}
