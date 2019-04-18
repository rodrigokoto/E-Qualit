using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Linq;

namespace ApplicationService.Servico
{
    public class FornecedorAppServico : BaseServico<Fornecedor>, IFornecedorAppServico
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;

        public FornecedorAppServico(IFornecedorRepositorio fornecedorRepositorio) : base(fornecedorRepositorio)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }


        public List<Fornecedor> ObterPorSite(int idSite)
        {
            return _fornecedorRepositorio.Get(x => x.IdSite == idSite).ToList();
        }

    }
}
