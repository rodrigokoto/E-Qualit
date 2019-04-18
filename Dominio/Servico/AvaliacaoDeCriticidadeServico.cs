using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.AvaliacaoDeCriticidades;

namespace Dominio.Servico
{
    public class AvaliacaoCriticidadeServico :  IAvaliacaoCriticidadeServico
    {
        private IAvaliacaoCriticidadeRepositorio _avaliacaoDeCriticidadeRepositorio;

        public AvaliacaoCriticidadeServico(IAvaliacaoCriticidadeRepositorio avaliacaoDeCriticidadeRepositorio) 
        {
            _avaliacaoDeCriticidadeRepositorio = avaliacaoDeCriticidadeRepositorio;
        }

        public void ValidaCampos(AvaliacaoCriticidade avaliacaoCriticidade, ref List<string> erros)
        {
            var validaCampos = new AptoParaCadastrarAvaliacaoDeCriticidade()
                                            .Validate(avaliacaoCriticidade);
            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }
    }
}
