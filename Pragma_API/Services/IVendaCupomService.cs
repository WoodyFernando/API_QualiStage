using Pragma_API.Models;

namespace Pragma_API.Services
{
    public interface IVendaCupomService
    {
        Task<IEnumerable<VendaCupom>> buscarVendasPeriodo(DateTime dataInicio, DateTime dataFim);
    }
}
