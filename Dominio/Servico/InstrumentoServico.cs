using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Instrumentos;
using Dominio.Validacao.Instrumentos.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class InstrumentoServico : IInstrumentoServico
    {
        private readonly IInstrumentoRepositorio _instrumentoRepositorio;
        private readonly ICalibracaoRepositorio _calibracaoRepositorio;
        private readonly ICriterioAceitacaoRepositorio _criterioAceitacaoRepositorio;

        public InstrumentoServico(IInstrumentoRepositorio instrumentoRepositorio,
                                  ICalibracaoRepositorio calibracaoRepositorio,
                                  ICriterioAceitacaoRepositorio criterioAceitacaoRepositorio)
        {
            _instrumentoRepositorio = instrumentoRepositorio;
            _calibracaoRepositorio = calibracaoRepositorio;
            _criterioAceitacaoRepositorio = criterioAceitacaoRepositorio;
        }

        public void Valido(Instrumento instrumento, ref List<string> erros)
        {
            ValidaRegraTela(instrumento, ref erros);

            //if (erros.Count == 0)
            //    ValidaRegraNegocio(instrumento, ref erros);

        }

        private void ValidaRegraNegocio(Instrumento instrumento, ref List<string> erros)
        {
            instrumento.ValidationResult = instrumento.IdInstrumento == 0 ?
                new AptoParaCadastroInstrumentoValidation(_instrumentoRepositorio).Validate(instrumento) :
                new AptoParaEditarInstrumentoValidation(_instrumentoRepositorio).Validate(instrumento);

            if (!instrumento.ValidationResult.IsValid)
                erros.AddRange(UtilsServico.PopularErros(instrumento.ValidationResult));
        }

        private void ValidaRegraTela(Instrumento instrumento, ref List<string> erros)
        {

            var camposObrigatrios = instrumento.IdInstrumento == 0 ?
                new CriarInstrumentoViewValidation().Validate(instrumento) :
                new EditarInstrumentoViewValidation().Validate(instrumento);

            if (!camposObrigatrios.IsValid)
                erros.AddRange(UtilsServico.PopularErros(camposObrigatrios.Errors));
        }

        public bool DeletarInstrumentoEDependencias(int id)
        {
            try
            {
                _instrumentoRepositorio.RemoverComDelecaoDosRelacionamentos(id);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Instrumento> ObterPorIdSite(int idSite)
        {
            return _instrumentoRepositorio.Get(x => x.IdSite == idSite).ToList();
        }

        public decimal GeraProximoNumeroRegistro(int idSite, int? idProcesso = null, int? idSigla = null)
        {
            decimal saida = 0;

            var item = _instrumentoRepositorio.GetAll()
                .Where(x => x.IdSite == idSite &&
                        (x.IdProcesso == idProcesso || idProcesso == null) &&
                        (x.IdSigla == idSigla || idSigla == null)
                )
                .OrderByDescending(x => x.Numero).FirstOrDefault();
            if (item != null)
            {
                if (item.Numero != 0)
                {
                    saida = Convert.ToInt32(item.Numero + 1);
                }
                else
                {
                    saida = 1;
                }
            }
            else
            {
                saida = 1;
            }

            return saida;
        }
    }
}
