using Microsoft.EntityFrameworkCore;
using Pragma_API.Interface;
using Pragma_API.Models;

namespace Pragma_API.Repositories
{
    public class CupomVendaRepository : ICupomVendaRepository
    {
        private readonly PragmaConciliaTMContext _context;

        public CupomVendaRepository(PragmaConciliaTMContext context)
        {
            _context = context;
        }
        public List<VCupomVenda> get()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VCupomVenda>> SelecionarPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.VCupomVendas.Where(x => x.Date >= dataInicio && x.Date <= dataFim.AddHours(23).AddMinutes(59).AddSeconds(59)).ToListAsync();
        }
    }
}
