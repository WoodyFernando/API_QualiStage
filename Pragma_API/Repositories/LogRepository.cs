using Pragma_API.Infraestrutura;
using Pragma_API.Interface;
using Pragma_API.Models;

namespace Pragma_API.Repositories
{
    public class LogRepository : ILog
    {
        private readonly IntegracaoTMContext _context;

        public LogRepository(IntegracaoTMContext context)
        {
            _context = context;
        }

        public void IncluirLog(Log log)
        {
            _context.Logs.Add(log);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
