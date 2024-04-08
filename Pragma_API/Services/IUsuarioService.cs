using Pragma_API.Models;

namespace Pragma_API.Services
{
    public interface IUsuarioService
    {
        Task<bool> ChangePasswordAsync(UsuarioDB usuario);
        Task<Usuario> GetUsuario(Login login);
    }
}
