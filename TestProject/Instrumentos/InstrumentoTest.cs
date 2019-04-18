using DAL.Repository;
using Dominio.Entidade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Dominio.Servico;

namespace TestProject.Instrumentos
{
    [TestClass]
    public class InstrumentoTest
    {
        private InstrumentoRepositorio _instrumentoRepositorio;
        private CriterioAceitacaoRepositorio _criterioAceitacaoRepositorio;
        private CalibracaoRepositorio _calibracaoRepositorio;

        private InstrumentoServico _instrumentoServico;

        public Instrumento InstanciaInstrumento()
        {
            return new Instrumento()
            {
                valorAceitacao = "CriterioAceitacao",
                DataAlteracao = DateTime.Now,
                DataCriacao = DateTime.Now,
                Equipamento = "Equipamento",
                Escala = "Escala",
                IdProcesso = 1,
                IdResponsavel = 1,
                IdSite = 1,
                IdUsuarioIncluiu = 1,
                LocalDeUso = "LocalDeUso",
                Marca = "Marca",
                MenorDivisao = "MenorDivisao",
                Modelo = "Modelo",
                Numero = "NUmero",
                Periodicidade = 1
            };
        }

        //[TestMethod]
        //[TestCategory("Insert Instrumento")]
        //public void Insert()
        //{
        //    var instrumento = InstanciaInstrumento();

        //    instrumento.Calibracao = new List<Calibracao>();
        //    instrumento.Calibracao.Add(
        //        new Calibracao()
        //        {
        //            Certificado = "Cerfificado",
        //            OrgaoCalibrador = "OrgaoCalibrador",
        //            Aprovado = 1,
        //            Aprovador = 1,
        //            Observacoes = "Observacoes",
        //            ArquivoCertificado = "ArquivoCertificado",
        //            DataAlteracao = DateTime.Now,
        //            DataCalibracao = DateTime.Now,
        //            DataCriacao = DateTime.Now,
        //            DataProximaCalibracao = DateTime.Now,
        //            IdUsuarioIncluiu = 1,
        //            CriterioAceitacao = new List<CriterioAceitacao>
        //             {
        //                new CriterioAceitacao
        //                {
        //                    Incerteza = 10,
        //                    Erro = 5,
        //                    IdUsuarioIncluiu = 1,
        //                    Aceito = true,
        //                    DtAlteracao = DateTime.Now,
        //                    DtInclusao = DateTime.Now
        //                },
        //                new CriterioAceitacao
        //                {
        //                    Incerteza = 9,
        //                    Erro = 4,
        //                    IdUsuarioIncluiu = 1,
        //                    Aceito = true,
        //                    DtAlteracao = DateTime.Now,
        //                    DtInclusao = DateTime.Now
        //                }
        //            }
        //        }
        //    );

        //    var repositorioInstrumento = new InstrumentoRepositorio();

        //    repositorioInstrumento.Add(instrumento);
        //}

        [TestMethod]
        [TestCategory("Validar TRUE Instrumento")]
        public void Valido_true()
        {
            _instrumentoRepositorio = new InstrumentoRepositorio();
            _calibracaoRepositorio = new CalibracaoRepositorio();
            _criterioAceitacaoRepositorio = new CriterioAceitacaoRepositorio();

            _instrumentoServico = new InstrumentoServico(_instrumentoRepositorio, _calibracaoRepositorio, _criterioAceitacaoRepositorio);

            var instrumentoValido = InstanciaInstrumento();

            List<string> Erros = new List<string>();

            _instrumentoServico.Valido(instrumentoValido, ref Erros);

            Assert.IsTrue(Erros.Count == 0);
        }

        [TestMethod]
        [TestCategory("ValidarCampos FALSE Instrumento")]
        public void Valido_false()
        {
            _instrumentoRepositorio = new InstrumentoRepositorio();
            _calibracaoRepositorio = new CalibracaoRepositorio();
            _instrumentoRepositorio = new InstrumentoRepositorio();
            _instrumentoServico = new InstrumentoServico(_instrumentoRepositorio, _calibracaoRepositorio, _criterioAceitacaoRepositorio);

            var instrumentoValido = InstanciaInstrumento();
            instrumentoValido.IdResponsavel = null;

            List<string> Erros = new List<string>();

            _instrumentoServico.Valido(instrumentoValido, ref Erros);

            Assert.IsFalse(Erros.Count == 0);
        }

        //[TestMethod]
        //[TestCategory("Update Instrumento")]
        //public void Update()
        //{
        //    var instrumento = InstanciaInstrumento();

        //    instrumento.LocalDeUso = "Bruno";
        //    instrumento.IdInstrumento = 4;


        //    instrumento.Calibracao = new List<Calibracao>();
        //    instrumento.Calibracao.Add(
        //        new Calibracao()
        //        {
        //            Certificado = "Cerfificado",
        //            OrgaoCalibrador = "OrgaoCalibradorDeTestes",
        //            Aprovado = 1,
        //            Aprovador = 2,
        //            Observacoes = "Observacoes",
        //            ArquivoCertificado = "CertificadoEmTestesMuitoLokos",
        //            DataAlteracao = DateTime.Now,
        //            DataCalibracao = DateTime.Now,
        //            DataCriacao = DateTime.Now,
        //            DataProximaCalibracao = DateTime.Now,
        //            IdUsuarioIncluiu = 2,
        //            IdCalibracao = 4,
        //            IdInstrumento = 4
        //        }
        //    );

        //    var repositorioInstrumento = new InstrumentoRepositorio();

        //    repositorioInstrumento.Update(instrumento);
        //}

        //[TestMethod]
        //[TestCategory("Delete Instrumento")]
        //public void Delete()
        //{
        //    _instrumentoRepositorio = new InstrumentoRepositorio();
        //    _calibracaoRepositorio = new CalibracaoRepositorio();
        //    _criterioAceitacaoRepositorio = new CriterioAceitacaoRepositorio();

        //    var instrumentoRemover = _instrumentoRepositorio.GetById(30);

        //    List<int> ids = new List<int>();
        //    List<int> idsCriterio = new List<int>();

        //    foreach (var calibracao in instrumentoRemover.Calibracao)
        //    {
        //        foreach (var criterio in calibracao.criterioAceitacao)
        //            idsCriterio.Add(criterio.IdCriterioAceitacao);

        //        ids.Add(calibracao.IdCalibracao);
        //    }

        //    _instrumentoRepositorio.RemoverComDelecaoDosRelacionamentos(30);

        //    for (int i = 0; i < ids.Count; i++)
        //    {
        //        _calibracaoRepositorio.RemoverComDelecaoDosRelacionamentos(ids[i]);
        //    }

        //    for (int i = 0; i < idsCriterio.Count; i++)
        //    {
        //        _criterioAceitacaoRepositorio.Remove(new CriterioAceitacao() { IdCriterioAceitacao = idsCriterio[i] });
        //    }
        //}

        //[TestMethod]
        //[TestCategory("Delete InstrumentoServico")]
        //public void DeleteServico()
        //{
        //    _instrumentoRepositorio = new InstrumentoRepositorio();
        //    _calibracaoRepositorio = new CalibracaoRepositorio();
        //    _criterioAceitacaoRepositorio = new CriterioAceitacaoRepositorio();

        //    _instrumentoServico = new InstrumentoServico(_instrumentoRepositorio, _calibracaoRepositorio, _criterioAceitacaoRepositorio);

        //    var instrumentoRemover = _instrumentoRepositorio.GetById(8);

        //    var executouComSucesso = _instrumentoServico.DeletarInstrumentoEDependencias(instrumentoRemover);

        //    Assert.IsTrue(executouComSucesso);

        //}
    }
}
