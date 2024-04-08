using Microsoft.AspNetCore.Http.HttpResults;
using Pragma_API.Interface;
using Pragma_API.Model;
using Pragma_API.Models;
using Pragma_API.Repositories;

namespace Pragma_API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuario _usuario;

        public UsuarioService(IUsuario usuario)
        {
            _usuario = usuario;
        }

        async Task<Usuario> IUsuarioService.GetUsuario(Login login)
        {
            var usuarioResult = await _usuario.GetUsuario(login.Usuario, login.Senha);


            var usuario = new Usuario();


            if (usuarioResult == null || usuarioResult.Ativo == false)
            {
                return usuario;
            }

            usuario = new Usuario()
            {
                Email = usuarioResult.Email,
                Id = usuarioResult.Id,
                Ativo = usuarioResult.Ativo,
            };

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(login.Senha);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            if (usuarioResult.SenhaHash != sb.ToString())
            {
                usuario.SenhaCorreta = false;
                return usuario;
            }

            usuario.SenhaCorreta = true;



            return usuario;
        }

                        
        async Task<bool> IUsuarioService.ChangePasswordAsync(UsuarioDB usuario)
        {
            _usuario.AlterarUsuario(usuario);
            if (await _usuario.SaveAllAsync())
            {
                return true;
            }

            return false;
        }
    }
}
