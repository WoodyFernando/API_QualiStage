using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pragma_API.Interface;
using Pragma_API.Models;
using Pragma_API.Services;
using System.Security.Claims;

namespace Pragma_API.Controllers
{
    [ApiController]
    [Route("api/v1/usuario")]
    public class PasswordController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly ILog _log;

        public PasswordController(IConfiguration configuration, IUsuarioService usuarioService,ILog log)
        {
            Configuration = configuration;
            _usuarioService = usuarioService;
            _log = log;
        }


        [Authorize]
        [HttpPost]
        [Route("AlterarSenha")]
        public async Task<IActionResult> ChangePassword([FromBody] LoginNovo login)
        {
            try
            {
                var emailUsuario = User.FindFirst(ClaimTypes.Email)?.Value;
                var usuarioID = User.FindFirst("UsuarioID")?.Value;

                var log = new Log()
                {
                    UsuarioId = Convert.ToInt32(usuarioID),
                    Email = emailUsuario,
                    Erro = false,
                    DataHora = DateTime.Now,
                    Url = HttpContext.Request.Path.Value.ToString()

                };

                if (login.Usuario is null || login.Senha is null
                || login.Usuario == "" || login.Senha is null
                || login.Usuario == "" || login.Senha is null
                || login.NovaSenha == "" || login.NovaSenha is null)
                {
                    log.Mensagem = "Parâmetro não pode ser vazio/null";
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return BadRequest("Parâmetro não pode ser vazio/null");
                }

                var login_ = new Login()
                {
                    Usuario = login.Usuario,
                    Senha = login.Senha
                };

                var usuarioResult = await _usuarioService.GetUsuario(login_);

                if (!usuarioResult.SenhaCorreta)
                {
                    log.Mensagem = "Senha inválidos";
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return BadRequest("Senha inválidos");
                }

                var novaSenha = GerarSenhaHash(login.NovaSenha);

                var novoUsuario = new UsuarioDB()
                {
                    Id = (int)usuarioResult.Id,
                    Email = usuarioResult.Email,
                    SenhaHash = novaSenha,
                    Ativo = (bool)usuarioResult.Ativo
                };

                if (await _usuarioService.ChangePasswordAsync(novoUsuario))
                {
                    log.Mensagem = "Senha alterada com sucesso";
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return Ok("Senha alterada com sucesso");
                }

                log.Mensagem = "Erro na alteração da senha";
                _log.IncluirLog(log);
                await _log.SaveAllAsync();

                return BadRequest("Erro na alteração da senha");
            }
            catch (Exception ex)
            {

                var log = new Log()
                {
                    Email = login.Usuario,
                    Erro = true,
                    DataHora = DateTime.Now,
                    Url = HttpContext.Request.Path.Value.ToString(),
                    Mensagem = ex.Message,
                };

                _log.IncluirLog(log);
                await _log.SaveAllAsync();

                return BadRequest("Erro: " + "entrar em contato com a Tickemaster");
            }

            

        }

        string GerarSenhaHash (string senha) 
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(senha);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

    }
}
