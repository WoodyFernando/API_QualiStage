using Pragma_API.Interface;
using Pragma_API.Models;

namespace Pragma_API.Services
{
    public class VendaCupomService : IVendaCupomService
    {
        private readonly ICupomVendaRepository _cupomVendaRepository;

        public VendaCupomService(ICupomVendaRepository cupomVendaRepository)
        {
            _cupomVendaRepository = cupomVendaRepository;
        }
        public async Task<IEnumerable<VendaCupom>> buscarVendasPeriodo (DateTime dataInicio, DateTime dataFim) 
        {
            
            var vendasCupomResult = await _cupomVendaRepository.SelecionarPeriodo( dataInicio,  dataFim);

            var vendasCupomList = new List<VendaCupom>();

            foreach (var item in vendasCupomResult)
            {
                var vendaCupom = vendasCupomList.Find(v => v.Pedido == item.Pedido);

                if (vendaCupom == null)
                {
                    vendaCupom = new VendaCupom();
                    vendaCupom.Pedido = item.Pedido;
                    vendaCupom.Date = item.Date;
                    vendaCupom.Nome = item.Nome;
                    vendaCupom.TipoDocumento = item.TipoDocumento;
                    vendaCupom.Documento = item.Documento;
                    vendaCupom.Endereco = item.Endereco;
                    vendaCupom.Bairo = item.Bairo;
                    vendaCupom.Cep = item.Cep;
                    vendaCupom.Cidade = item.Cidade;
                    vendaCupom.Uf = item.Uf;
                    vendaCupom.Email = item.Email;
                    vendaCupom.Itens = new List<VendaCupomItem>();

                    vendasCupomList.Add(vendaCupom);
                }

                var vendaCupomItem = new VendaCupomItem();
                vendaCupomItem.ingressoId = item.ingressoID;
                vendaCupomItem.nomeEvento = item.nomeEvento;

                vendaCupom.Itens.Add(vendaCupomItem);



            }
            return vendasCupomList;
        }
    }
}
