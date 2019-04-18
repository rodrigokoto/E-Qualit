using Dominio.Entidade;
using Dominio.Interface.Servico;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using Dominio.Validacao.QualificaAvaliacaoCriticidades;
using Dominio.Validacao.CriteriosAvaliacao;

namespace Dominio.Servico
{
    public class CriterioAvaliacaoServico : ICriterioAvaliacaoServico
    {
        private ICriterioAvaliacaoRepositorio _criterioAvaliacaoRepositorio;

        public CriterioAvaliacaoServico(ICriterioAvaliacaoRepositorio criterioAvaliacaoRepositorio)
        {
            _criterioAvaliacaoRepositorio = criterioAvaliacaoRepositorio;
        }

        public void ValidaCamposCadastro(CriterioAvaliacao criterioAvaliacao, ref List<string> erros)
        {
            var validaCampos = new AptoParaCadastraCriterioAvaliacao().Validate(criterioAvaliacao);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }

        }

        public void ValidaCamposQualifica(CriterioAvaliacao criterioAvaliacao, ref List<string> erros)
        {
            var validaCampos = new AptoParaQualificaCriterioAvaliacao().Validate(criterioAvaliacao);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }
    }
}
