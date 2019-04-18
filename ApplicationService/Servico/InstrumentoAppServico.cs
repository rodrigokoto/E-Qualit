using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class InstrumentoAppServico : BaseServico<Instrumento>, IInstrumentoAppServico
    {
        private readonly IInstrumentoRepositorio _instrumentoRepositorio;
        private readonly ICalibracaoRepositorio _calibracaoRepositorio;
        private readonly ICriterioAceitacaoRepositorio _criterioAceitacaoRepositorio;

        public InstrumentoAppServico(IInstrumentoRepositorio instrumentoRepositorio,
                                  ICalibracaoRepositorio calibracaoRepositorio,
                                  ICriterioAceitacaoRepositorio criterioAceitacaoRepositorio) : base(instrumentoRepositorio)
        {
            _instrumentoRepositorio = instrumentoRepositorio;
            _calibracaoRepositorio = calibracaoRepositorio;
            _criterioAceitacaoRepositorio = criterioAceitacaoRepositorio;
        }

        public bool DeletarInstrumentoEDependencias(int id)
        {
            try
            {
                _instrumentoRepositorio.RemoverComDelecaoDosRelacionamentos(id);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Instrumento> ObterPorIdSite(int idSite)
        {
            return _instrumentoRepositorio.Get(x => x.IdSite == idSite).ToList();
        }
    }
}
