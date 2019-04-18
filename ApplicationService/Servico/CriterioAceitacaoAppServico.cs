using Dominio.Entidade;
using ApplicationService.Interface;
using System.Collections.Generic;
using Dominio.Interface.Repositorio;
using Dominio.Enumerado;

namespace ApplicationService.Servico
{
    public class CriterioAceitacaoAppServico : BaseServico<CriterioAceitacao>, ICriterioAceitacaoAppServico
    {
        private readonly ICriterioAceitacaoRepositorio _criterioAceitacaoRepositorio;

        public CriterioAceitacaoAppServico(ICriterioAceitacaoRepositorio criterioAceitacaoRepositorio) : base(criterioAceitacaoRepositorio)
        {
            _criterioAceitacaoRepositorio = criterioAceitacaoRepositorio;
        }

        public void AlterarEstadoParaDeletado(CriterioAceitacao obj)
        {
            _criterioAceitacaoRepositorio.AlteraEstado(obj, EstadoObjetoEF.Deleted);
        }

        public void Remove(List<CriterioAceitacao> criteriosAceitacao)
        {
            foreach (var criterioAceitacao in criteriosAceitacao)
            {
                Remove(criterioAceitacao);
            }
        }
    }
}
