using Microsoft.AspNetCore.Mvc;
using Pragma_API.Interface;
using Pragma_API.Models;
using Pragma_API.Repositories;
using Pragma_API.Services;

namespace Pragma_API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILog _log;

        public AuthController(IUsuarioService usuarioService, ILog log)
        {
            _usuarioService = usuarioService;
            _log = log;
        }

        [HttpPost]
        public async Task<IActionResult> Auth(Login login)
        {
            if (login.Usuario is null || login.Senha is null
                || login.Usuario == "" || login.Senha == "")
            {
                return BadRequest("Parâmetro não pode ser vazio/null");
            }

            try
            {
                var usuarioResult = await _usuarioService.GetUsuario(login);

                if (usuarioResult.Id == null)
                {
                    return BadRequest("Usuário não existe.");
                }

                var log = new Log()
                {
                    UsuarioId = (int)usuarioResult.Id,
                    Email = login.Usuario,
                    Erro = false,
                    DataHora = DateTime.Now,
                    Url = HttpContext.Request.Path.Value.ToString()
                    
                };

                if (usuarioResult.Email == "" || usuarioResult.SenhaCorreta == false)
                {
                    log.Mensagem = "Usuário e/ou senha inválidos";
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return BadRequest("Usuário e/ou senha inválidos");
                }

                var token = TokenService.GerarToken(usuarioResult);

                log.Mensagem = "Token gerado";



                _log.IncluirLog(log);
                await _log.SaveAllAsync();

                return Ok(token);
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
    }
}
