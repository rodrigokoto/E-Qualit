using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Plais;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class PaiServico : IPaiServico
    {
        private readonly IPaiRepositorio _paiRepositorio;
        private readonly IPlaiRepositorio _plaiRepositorio;

        public PaiServico(IPaiRepositorio paiRepositorio, IPlaiRepositorio plaiRepositorio) 
        {
            _paiRepositorio = paiRepositorio;
            _plaiRepositorio = plaiRepositorio;
        }
        
        public Pai ObterPorAno(int? ano)
        {
            return _paiRepositorio.Get(x => x.Ano == ano).FirstOrDefault();
        }


        public void Valido(Plai plai, ref List<string> erros)
        {
            var validacao = new AptoParaCadastroValidacao().Validate(plai);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }

    }
}
