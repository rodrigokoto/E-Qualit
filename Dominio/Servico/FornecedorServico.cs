using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Linq;
using Dominio.Validacao.Fornecedores;

namespace Dominio.Servico
{
    public class FornecedorServico : IFornecedorServico
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;

        public FornecedorServico(IFornecedorRepositorio fornecedorRepositorio) 
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }


        public List<Fornecedor> ObterPorSite(int idSite)
        {
            return _fornecedorRepositorio.Get(x => x.IdSite == idSite).ToList();
        }

        public void ValidaCampos(Fornecedor fornecedor, ref List<string> erros)
        {
            var validaCampos = new AptoParaCadastrarFornecedor().Validate(fornecedor);


            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }
    }
}
