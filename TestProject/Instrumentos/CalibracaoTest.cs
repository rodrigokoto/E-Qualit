using DAL.Repository;
using Dominio.Entidade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Dominio.Servico;

namespace TestProject.Instrumentos
{
    [TestClass]
    public class CalibracaoTest
    {
        private CalibracaoRepositorio _calibracaoRepositorio;
        private CriterioAceitacaoRepositorio _criterioAceitacaoRepositorio;
        private InstrumentoRepositorio _instrumentoRepositorio;

        private CalibracaoServico _calibracaoServico;

        private Calibracao NovaInstancia()
        {
            return new Calibracao()
            {
                IdInstrumento = 2,
                Certificado = "Cerfificado",
                OrgaoCalibrador = "OrgaoCalibrador",
                Aprovado = 1,
                Aprovador = 1,
                Observacoes = "Observacoes",
                ArquivoCertificado = "ArquivoCertificado",
                DataAlteracao = DateTime.Now,
                DataCalibracao = DateTime.Now,
                DataCriacao = DateTime.Now,
                DataProximaCalibracao = DateTime.Now,
                DataRegistro = DateTime.Now,
                DataNotificacao = DateTime.Now,
                IdUsuarioIncluiu = 1,
                CriterioAceitacao = new List<CriterioAceitacao> {
                    new CriterioAceitacao{
                        DtAlteracao = DateTime.Now,
                        DtInclusao = DateTime.Now,
                        IdUsuarioIncluiu = 1,
                        Incerteza = 10,
                        Erro = 5
                    },

                    new CriterioAceitacao{
                        DtAlteracao = DateTime.Now,
                        DtInclusao = DateTime.Now,
                        IdUsuarioIncluiu = 1,
                        Incerteza = 10,
                        Erro = 4
                    },

                    new CriterioAceitacao{
                        DtAlteracao = DateTime.Now,
                        DtInclusao = DateTime.Now,
                        IdUsuarioIncluiu = 1,
                        Incerteza = 9,
                        Erro = 4
                    }
                }

            };
        }

        //[TestMethod]
        //[TestCategory("Insert CriterioAceitacao")]
        //public void Insert()
        //{
        //    var inserNovo = NovaInstancia();

        //    _calibracaoRepositorio = new CalibracaoRepositorio();
        //    _calibracaoRepositorio.Add(inserNovo);
        //}


        //[TestMethod]
        //[TestCategory("Update Calibracao")]
        //public void Update()
        //{
        //    _calibracaoRepositorio = new CalibracaoRepositorio();

        //    var atualizar = _calibracaoRepositorio.GetById(22);

        //    atualizar.OrgaoCalibrador = "OrgaoDeTestes EM EQUIPAMENTOS";

        //    _calibracaoRepositorio.Update(atualizar);
        //}

        [TestMethod]
        [TestCategory("Update Calibracao")]
        public void GetByID()
        {
            _calibracaoRepositorio = new CalibracaoRepositorio();

            var atualizar = _calibracaoRepositorio.GetById(22);

            atualizar.OrgaoCalibrador = "OrgaoDeTestes EM EQUIPAMENTOS";

            _calibracaoRepositorio.Update(atualizar);
        }

        [TestMethod]
        [TestCategory("CriterioAceitacao")]
        public void Validacao_true()
        {
            _calibracaoRepositorio = new CalibracaoRepositorio();
            _criterioAceitacaoRepositorio = new CriterioAceitacaoRepositorio();
            _instrumentoRepositorio = new InstrumentoRepositorio();

            _calibracaoServico = new CalibracaoServico(_calibracaoRepositorio, _instrumentoRepositorio);

            var calibracao = NovaInstancia();

            List<String> erros = new List<string>();

            _calibracaoServico.Valido(calibracao, ref erros);

            Assert.IsTrue(erros.Count == 0);
        }

        [TestMethod]
        [TestCategory("CriterioAceitacao")]
        public void Validacao_false()
        {
            _calibracaoRepositorio = new CalibracaoRepositorio();
            _criterioAceitacaoRepositorio = new CriterioAceitacaoRepositorio();
            _instrumentoRepositorio = new InstrumentoRepositorio();

            _calibracaoServico = new CalibracaoServico(_calibracaoRepositorio, _instrumentoRepositorio);

            var calibracao = NovaInstancia();

            calibracao.Certificado = String.Empty;

            List<String> erros = new List<string>();

            _calibracaoServico.Valido(calibracao, ref erros);

            Assert.IsTrue(erros.Count > 0);
        }
    }
}
