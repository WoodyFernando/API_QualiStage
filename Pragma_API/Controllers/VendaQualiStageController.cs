using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Pragma_API.Models;
using System.Security.Claims;
using Pragma_API.Services;
using System.Collections.Generic;
using System.Data;
using Pragma_API.Interface;

namespace Pragma_API.Controllers
{
    [ApiController]
    [Route("api/v1/venda")]
    public class VendaQualiStageController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly ILog _log;

        public VendaQualiStageController(IConfiguration configuration, ILog log)
        {
            Configuration = configuration;
            _log = log;
        }

        [Authorize]
        [HttpGet]
        [Route("qualistage")]
        public async Task<ActionResult<IEnumerable<ResultadoVenda>>> GetCupomVenda(DateTime dataInicio, DateTime dataFim)
        {

            var emailUsuario = User.FindFirst(ClaimTypes.Email)?.Value;
            var usuarioID = User.FindFirst("UsuarioID")?.Value;

            string mensagem = "Entrou no método [GetCupomVenda]";

            var log = new Log()
            {
                UsuarioId = Convert.ToInt32(usuarioID),
                Email = emailUsuario,
                Erro = false,
                DataHora = DateTime.Now,
                Url = HttpContext.Request.Path.Value.ToString(),
                Parametros = " DataInicio: " + dataInicio.ToString() + " | DataFim: " + dataFim.ToString()

            };

            if (dataInicio == DateTime.MinValue
                || dataFim == DateTime.MinValue)
            {
                mensagem = "Preencha uma data válida.";
                log.Mensagem = mensagem;
                _log.IncluirLog(log);
                await _log.SaveAllAsync();

                return BadRequest(mensagem);
            }

            try
            {
                if (dataFim < dataInicio)
                {
                    mensagem = "Parâmetro inválido, data final inferior ao período inicial.";
                    log.Mensagem = mensagem;
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return BadRequest(mensagem);
                }

                var result = (dataFim - dataInicio).Days;
                int periodoPermitidoVenda = Convert.ToInt32(Configuration["PeriodoPermitidoVenda"]);

                if (result > periodoPermitidoVenda)
                {
                    mensagem = ("O período excedeu o limite de @dias dias.").Replace("@dias", periodoPermitidoVenda.ToString());
                    log.Mensagem = mensagem;
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return BadRequest(mensagem);
                }

                var sql = "exec [listar_vendas_Qualistage] @DataInicio, @DataFim ";

                var parameters = new { DataInicio = dataInicio, DataFim = dataFim };

                log.Mensagem = "Passou pelas validações. Iniciando conexão com o banco.";

                using (var sqlConnection = new SqlConnection(Configuration["ConnectionStrings:DefaultConnection"]))
                {
                    var lista = await sqlConnection.QueryAsync<VendaDB>(sql, parameters);

                    log.Mensagem = "Finalizou a chamada no banco.";

                    if (!lista.Any())
                    {
                        mensagem = "Neste período, não há registro de vendas.";
                        log.Mensagem = mensagem;
                        _log.IncluirLog(log);
                        await _log.SaveAllAsync();

                        return BadRequest(mensagem);
                    }

                    var vendasList = new List<Venda>();

                    log.Mensagem = "Criando a lista de objeto [Venda]";

                    foreach (var item in lista)
                    {
                        var venda = vendasList.Find(v => v.Pedido == item.Pedido
                                                    && v.Movimento == item.Movimento);

                        if (venda == null)
                        {
                            venda = new Venda();
                            venda.Pedido = item.Pedido;
                            venda.EventoID = item.EventoID;
                            venda.NomeEvento = item.NomeEvento;
                            venda.CategoriaEvento = item.CategoriaEvento;
                            venda.DataTransacao = item.DataTransacao;
                            venda.QtdTotal = item.QtdTotal;
                            venda.ValorTransacao = item.ValorTransacao;
                            venda.NomeCliente = item.NomeCliente;
                            venda.Email = item.Email;
                            venda.TipoDocumento = item.TipoDocumento;
                            venda.Documento = item.Documento;
                            venda.Endereco = item.Endereco;
                            venda.Bairro = item.Bairro;
                            venda.Cep = item.Cep;
                            venda.Cidade = item.Cidade;
                            venda.Uf = item.Uf;
                            venda.Movimento = item.Movimento;
                            venda.Itens = new List<VendaItem>();

                            vendasList.Add(venda);
                        }

                        var vendaItem = new VendaItem();
                        vendaItem.DataShow = item.DataShow;
                        vendaItem.Setor = item.Setor;
                        vendaItem.NomePreco = item.NomePreco;
                        vendaItem.QtdIngressos = item.QtdIngressos;
                        vendaItem.ValorUnitario = item.ValorUnitario;
                        vendaItem.ValorTotal = item.ValorTotal;
                        vendaItem.TipoPreco = item.TipoPreco;

                        venda.Itens.Add(vendaItem);

                    }


                    var resultadoVendasList = new List<ResultadoVenda>
                {
                    new ResultadoVenda()
                    {
                        TotalTransacoes = vendasList.Count(),
                        DataGeracao = DateTime.Now,
                        DataInicioBusca = dataInicio,
                        DatafinalBusca = dataFim,
                        ValorTotalTransacao = vendasList.Sum(item => item.ValorTransacao),
                        Transacoes = vendasList
                    }
                };

                    
                    log.Mensagem = "Gerado com sucesso";
                    
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return resultadoVendasList;
                }
            }
            catch (Exception ex)
            {
                log.Mensagem = mensagem + " | " + ex.Message;
                _log.IncluirLog(log);
                await _log.SaveAllAsync();

                return BadRequest("Erro: " + ex.Message);
            }     
        }

        [Authorize]
        [HttpGet]
        [Route("qualistage_por_pagina")]
        public async Task<ActionResult<IEnumerable<ResultadoVendaPorPagina>>> GetCupomVendaPorPagina(DateTime dataInicio, DateTime dataFim, int qtdVendas = 0, int pagina = 0)
        {
            var emailUsuario = User.FindFirst(ClaimTypes.Email)?.Value;
            var usuarioID = User.FindFirst("UsuarioID")?.Value;

            string mensagem = "Entrou no método [GetCupomVenda]";

            var log = new Log()
            {
                UsuarioId = Convert.ToInt32(usuarioID),
                Email = emailUsuario,
                Erro = false,
                DataHora = DateTime.Now,
                Url = HttpContext.Request.Path.Value.ToString(),
                Parametros = " DataInicio: " + dataInicio.ToString() + " | DataFim: " + dataFim.ToString()

            };

            if (dataInicio == DateTime.MinValue
                || dataFim == DateTime.MinValue)
            {

                mensagem = "Preencha uma data válida.";
                log.Mensagem = mensagem;
                _log.IncluirLog(log);
                await _log.SaveAllAsync();

                return BadRequest(mensagem);
            }

            try
            {
                if (dataFim < dataInicio)
                {
                    mensagem = "Parâmetro inválido, data final inferior ao período inicial.";
                    log.Mensagem = mensagem;
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return BadRequest(mensagem);
                }

                var result = (dataFim - dataInicio).Days;
                int periodoPermitidoVenda = Convert.ToInt32(Configuration["PeriodoPermitidoVendaPorPagina"]);

                if (result > periodoPermitidoVenda)
                {
                    mensagem = ("O período excedeu o limite de @dias dias.").Replace("@dias", periodoPermitidoVenda.ToString());
                    log.Mensagem = mensagem;
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return BadRequest(mensagem);
                }

                var sql = "exec [listar_vendas_Qualistage_por_pagina] @DataInicio, @DataFim , @QtdVendas, @Pagina";

                var parameters = new { DataInicio = dataInicio, DataFim = dataFim, QtdVendas = qtdVendas, Pagina = pagina };

                log.Mensagem = "Passou pelas validações. Iniciando conexão com o banco.";

                var paginaAtual = 0;
                var qdtVendaAtual = 0;
                var totalPagina = 0;


                using (var sqlConnection = new SqlConnection(Configuration["ConnectionStrings:DefaultConnection"]))
                {
                    var lista = await sqlConnection.QueryAsync<VendaDB>(sql, parameters);

                    log.Mensagem = "Finalizou a chamada no banco.";

                    if (!lista.Any())
                    {
                        mensagem = "Neste período, não há registro de vendas.";
                        log.Mensagem = mensagem;
                        _log.IncluirLog(log);
                        await _log.SaveAllAsync();

                        return BadRequest(mensagem);
                    }

                    var vendasList = new List<Venda>();

                    foreach (var item in lista)
                    {
                        var venda = vendasList.Find(v => v.Pedido == item.Pedido
                                                    && v.Movimento == item.Movimento);
                        if (venda == null)
                        {
                            venda = new Venda();
                            venda.Pedido = item.Pedido;
                            venda.EventoID = item.EventoID;
                            venda.NomeEvento = item.NomeEvento;
                            venda.CategoriaEvento = item.CategoriaEvento;
                            venda.DataTransacao = item.DataTransacao;
                            venda.QtdTotal = item.QtdTotal;
                            venda.ValorTransacao = item.ValorTransacao;
                            venda.NomeCliente = item.NomeCliente;
                            venda.Email = item.Email;
                            venda.TipoDocumento = item.TipoDocumento;
                            venda.Documento = item.Documento;
                            venda.Endereco = item.Endereco;
                            venda.Bairro = item.Bairro;
                            venda.Cep = item.Cep;
                            venda.Cidade = item.Cidade;
                            venda.Uf = item.Uf;
                            venda.Movimento = item.Movimento;
                            venda.Itens = new List<VendaItem>();

                            vendasList.Add(venda);

                            paginaAtual = item.PaginaAtual;
                            qdtVendaAtual = item.QtdVendaPagina;
                            totalPagina = item.TotalPaginas;
                        }

                        var vendaItem = new VendaItem();
                        vendaItem.DataShow = item.DataShow;
                        vendaItem.Setor = item.Setor;
                        vendaItem.NomePreco = item.NomePreco;
                        vendaItem.QtdIngressos = item.QtdIngressos;
                        vendaItem.ValorUnitario = item.ValorUnitario;
                        vendaItem.ValorTotal = item.ValorTotal;
                        vendaItem.TipoPreco = item.TipoPreco;


                        venda.Itens.Add(vendaItem);
                    }


                    var resultadoVendasList = new List<ResultadoVendaPorPagina>
                {
                    new ResultadoVendaPorPagina()
                    {
                        TotalTransacoes = vendasList.Count(),
                        DataGeracao = DateTime.Now,
                        DataInicioBusca = dataInicio,
                        DatafinalBusca = dataFim,
                        ValorTotalTransacao = vendasList.Sum(item => item.ValorTransacao),
                        PaginalAtual = paginaAtual,
                        QtdTransacoesPorPagina = qdtVendaAtual,
                        TotalPaginas = totalPagina,
                        Transacoes = vendasList
                    }
                };

                    log.Mensagem = "Gerado com sucesso";
                    log.Parametros = " DataInicio: " + dataInicio.ToString() + " | DataFim: " + dataFim.ToString();
                    _log.IncluirLog(log);
                    await _log.SaveAllAsync();

                    return resultadoVendasList;
                }
            }
            catch (Exception ex)
            {

                log.Mensagem = mensagem + " | " + ex.Message;
                _log.IncluirLog(log);
                await _log.SaveAllAsync();

                return BadRequest("Erro: " + ex.Message);
            }
        }
    }
}
