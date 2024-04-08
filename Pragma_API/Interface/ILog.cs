using Pragma_API.Models;

namespace Pragma_API.Interface
{
    public interface ILog
    {
        void IncluirLog(Log log);

        Task<bool> SaveAllAsync();
    }
}
