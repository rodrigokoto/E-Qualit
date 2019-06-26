using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Especificacao.Instrumentos;
using Dominio.Enumerado;

namespace ApplicationService.Servico
{
    public class CalibracaoAppServico : BaseServico<Calibracao>, ICalibracaoAppServico
    {
        private readonly ICalibracaoRepositorio _calibracaoRepositorio;
        private readonly IInstrumentoRepositorio _instrumentoRepositorio;
        private readonly IArquivoCertificadoAnexoRepositorio _arquivoCertificadoAnexoRepositorio;

        public CalibracaoAppServico(ICalibracaoRepositorio calibracao,
                                 IInstrumentoRepositorio instrumentoRepositorio) : base(calibracao)
        {
            _calibracaoRepositorio = calibracao;
            _instrumentoRepositorio = instrumentoRepositorio;
        }

        public void AlterarEstadoParaDeletado(Calibracao obj)
        {
            _calibracaoRepositorio.AlteraEstado(obj, EstadoObjetoEF.Deleted);
        }

        public void SalvarComCriteriosAceitacao(Calibracao calibracao)
        {
            try
            {
                var possuiCriteiosAceitacaoSpecification = new PossuiCriteriosAceitacaoSpecification();
                var possuiCriteiosAceitacao = possuiCriteiosAceitacaoSpecification.IsSatisfiedBy(calibracao);

                if (possuiCriteiosAceitacao)
                {
                    var valor = Convert.ToDecimal(calibracao.Instrumento.valorAceitacao);

                    List<CriterioAceitacao> itensAvaliados = AvaliarCriteriosAceitacao(valor, calibracao.CriterioAceitacao);

                    var qtdeAceitos = CriteriosAceitos(itensAvaliados);

                    var calibracaoAprovada = CalibracaoEstaAprovada(qtdeAceitos, calibracao.CriterioAceitacao.Count);

                    var instrumento = _instrumentoRepositorio.GetById(calibracao.Instrumento.IdInstrumento);

                    if (calibracaoAprovada)
                    {
                        instrumento.Status = (byte)EquipamentoStatus.Calibrado;
                        AtualizarInstrumento(instrumento);
                    }
                    else
                    {
                        instrumento.Status = (byte)EquipamentoStatus.NaoCalibrado;
                        AtualizarInstrumento(instrumento);
                    }
                }
                else
                {
                    if (calibracao.Instrumento != null)
                    {
                        var setaIntrumento = _instrumentoRepositorio.GetById(calibracao.Instrumento.IdInstrumento);
                        setaIntrumento.Status = calibracao.Aprovado;
                        AtualizarInstrumento(setaIntrumento);
                    }
                }

                calibracao.Instrumento = null;
                _calibracaoRepositorio.CriarCalibracao(calibracao);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SalvarCalibracao(Calibracao calibracao)
        {
            try
            {
                calibracao.Instrumento = null;

                _calibracaoRepositorio.CriarCalibracao(calibracao);

                var setaIntrumento = _instrumentoRepositorio.GetById(calibracao.IdInstrumento);

                if (setaIntrumento != null)
                {
                    setaIntrumento.Status = calibracao.Aprovado;
                    AtualizarInstrumento(setaIntrumento);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AtualizarCalibracao(Calibracao calibracao)
        {
            try
            {
                _calibracaoRepositorio.Atualizar(calibracao);

                var setaIntrumento = _instrumentoRepositorio.GetById(calibracao.IdInstrumento);
                setaIntrumento.Status = calibracao.Aprovado;

                if (calibracao.Aprovado == 1)
                {
                    setaIntrumento.FlagTravado = true;
                }
                else
                {
                    setaIntrumento.FlagTravado = false;
                }

                AtualizarInstrumento(setaIntrumento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AtualizarInstrumento(Instrumento instrumentoAtualizar)
        {
            _instrumentoRepositorio.Update(instrumentoAtualizar);
        }

        private List<CriterioAceitacao> AvaliarCriteriosAceitacao(decimal valorAceitacao, List<CriterioAceitacao> criterios)
        {
            List<CriterioAceitacao> listaCriteriosAvaliados = new List<CriterioAceitacao>();

            foreach (var criterio in criterios)
            {
                var resultado = Convert.ToDecimal(criterio.Erro) + Convert.ToDecimal(criterio.Incerteza);

                if (valorAceitacao >= resultado)
                {
                    criterio.Aceito = true;
                    listaCriteriosAvaliados.Add(criterio);
                }
                else
                {
                    criterio.Aceito = false;
                    listaCriteriosAvaliados.Add(criterio);
                }
            }

            return listaCriteriosAvaliados;
        }

        private int CriteriosAceitos(List<CriterioAceitacao> criterios)
        {
            return criterios.Where(s => s.Aceito == true).Count();
        }

        private bool CalibracaoEstaAprovada(int criteriosAceitos, int totalCriterios)
        {
            return (criteriosAceitos >= (totalCriterios / 2));
        }

        public bool RemoverComRelacionamentos(int id)
        {
            try
            {
                _calibracaoRepositorio.RemoverComDelecaoDosRelacionamentos(id);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

