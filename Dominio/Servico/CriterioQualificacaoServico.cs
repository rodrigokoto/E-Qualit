using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.CriteriosQualificacao;

namespace Dominio.Servico
{
    public class CriterioQualificacaoServico : ICriterioQualificacaoServico
    {
        private readonly ICriterioQualificacaoRepositorio _criterioQualificacaoRepositorio;

        public CriterioQualificacaoServico(ICriterioQualificacaoRepositorio criterioQualificacaoRepositorio) 
        {
            _criterioQualificacaoRepositorio = criterioQualificacaoRepositorio;
        }

        public void SalvarQualificacao(CriterioQualificacao criterioQualificacao)
        {
            _criterioQualificacaoRepositorio.Update(criterioQualificacao);
        }

        public void ValidaCampos(CriterioQualificacao criterioQualificacao, ref List<string> erros)
        {
            var validaCampos = new AptoParaCadastroCriterioQualificacao().Validate(criterioQualificacao);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }

        public void ValidaCamposQualificacao(AvaliaCriterioQualificacao criterioQualificacao, ref List<string> erros)
        {
            var validaCampos = new AptoParaQualificarCriterioQualificacao().Validate(criterioQualificacao);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }

        public void ValidaCamposQualificacaoSemControleVencimento(AvaliaCriterioQualificacao criterioQualificacao, ref List<string> erros)
        {
            var validaCampos = new AptoParaQualificarCriterioQualificacaoSemControleVencimento().Validate(criterioQualificacao);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }
    }
}
