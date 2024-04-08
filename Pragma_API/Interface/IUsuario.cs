using Pragma_API.Models;

namespace Pragma_API.Interface
{
    public interface IUsuario
    {
        Task<UsuarioDB>GetUsuario(string Usuario, string SenhaHash);

        void AlterarUsuario(UsuarioDB usuario);

        Task<bool> SaveAllAsync();
    }
}
