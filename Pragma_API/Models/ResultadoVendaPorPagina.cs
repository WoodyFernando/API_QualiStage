using Microsoft.EntityFrameworkCore;

namespace Pragma_API.Models
{
    [Keyless]
    public partial class ResultadoVendaPorPagina
    {
        public int TotalTransacoes { get; set; }

        public DateTime DataGeracao { get; set; }
        public DateTime DataInicioBusca { get; set; }
        public DateTime DatafinalBusca { get; set; }
        public decimal ValorTotalTransacao { get; set; }
        public int QtdTransacoesPorPagina { get; set; }
        public int PaginalAtual { get; set; }
        public int TotalPaginas { get; set; }
        public List<Venda>? Transacoes { get; set; }
    }
}
