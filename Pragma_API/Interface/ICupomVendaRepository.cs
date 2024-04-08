using Pragma_API.Models;

namespace Pragma_API.Interface
{
    public interface ICupomVendaRepository
    {
        List<VCupomVenda> get();

        Task<IEnumerable<VCupomVenda>> SelecionarPeriodo(DateTime dataInicio, DateTime dataFim);
    }
}
