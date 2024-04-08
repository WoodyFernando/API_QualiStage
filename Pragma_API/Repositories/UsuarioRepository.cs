using Microsoft.EntityFrameworkCore;
using Pragma_API.Infraestrutura;
using Pragma_API.Interface;
using Pragma_API.Models;
using System.Reflection.Metadata;

namespace Pragma_API.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        private readonly IntegracaoTMContext _context;

        public UsuarioRepository(IntegracaoTMContext context)
        {
            _context = context;
        }
        public async Task<UsuarioDB> GetUsuario(string email, string senhaHash)
        {
            return await _context.Usuarios.Where(x => x.Email.ToLower() == email.ToLower()).AsNoTracking().FirstOrDefaultAsync();
        }

        public void AlterarUsuario(UsuarioDB usuario)
        {
            _context.Entry(usuario).State = usuario.Id == 0 ?
                                   EntityState.Added :
                                   EntityState.Modified;
            //_context.SaveChanges();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
