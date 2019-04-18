using Dominio.Entidade;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using Dominio.Interface.Repositorio;
using Dominio.Validacao.CriterioAceitacoes;

namespace Dominio.Servico
{
    public class CriterioAceitacaoServico : ICriterioAceitacaoServico
    {
        private readonly ICriterioAceitacaoRepositorio _criterioAceitacaoRepositorio;

        public CriterioAceitacaoServico(ICriterioAceitacaoRepositorio criterioAceitacaoRepositorio) 
        {
            _criterioAceitacaoRepositorio = criterioAceitacaoRepositorio;
        }

        public void Valido(CriterioAceitacao criterioAceitacao, ref List<string> erros)
        {
            var camposObrigatoriosDoc = new CamposObrigatoriosCriterioAceitacao();

            var validacao = camposObrigatoriosDoc.Validate(criterioAceitacao);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }

        public void AlterarEstadoParaDeletado(CriterioAceitacao obj)
        {
            _criterioAceitacaoRepositorio.AlteraEstado(obj, Enumerado.EstadoObjetoEF.Deleted);
        }

        public void Remove(List<CriterioAceitacao> criteriosAceitacao)
        {
            foreach (var criterioAceitacao in criteriosAceitacao)
            {
                _criterioAceitacaoRepositorio.Remove(criterioAceitacao);
            }
        }
    }
}
