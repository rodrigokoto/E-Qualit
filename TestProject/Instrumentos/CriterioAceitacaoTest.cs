using DAL.Repository;
using Dominio.Entidade;
using ApplicationService.Servico;
using Dominio.Validacao.CriterioAceitacoes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Dominio.Servico;

namespace TestProject.Instrumentos
{
    [TestClass]
    public class CriterioAceitacaoTest
    {
        private CamposObrigatoriosCriterioAceitacao _valido = new CamposObrigatoriosCriterioAceitacao();
        private CriterioAceitacaoRepositorio  _criterioAceitacaoRepositorio;
        private CriterioAceitacaoAppServico _criterioAceitacaoServico;

        private CriterioAceitacao InstanciaCriterioAceitacao()
        {
            return new CriterioAceitacao
            {
                IdCriterioAceitacao = 0,
                Incerteza = 10,
                Erro = 5,
                IdUsuarioIncluiu = 2,
                Aceito = true,
                DtAlteracao = DateTime.Now,
                DtInclusao = DateTime.Now,
                IdCalibracao = 1
            };
        }

        [TestMethod]
        [TestCategory("CamposObrigatorios CriterioAceitacao")]
        public void TodosOsCamposForamPreenchidos_True()
        {
            var objetoComTodosOsCampos = InstanciaCriterioAceitacao();

            var estaValido = _valido.Validate(objetoComTodosOsCampos).IsValid;

            Assert.IsTrue(estaValido);
        }

        [TestMethod]
        [TestCategory("CamposObrigatorios CriterioAceitacao")]
        public void TodosOsCamposForamPreenchidos_False()
        {
            var objetoComCamposVazios = InstanciaCriterioAceitacao();

            objetoComCamposVazios.Erro = null;

            var estaValido = _valido.Validate(new CriterioAceitacao()).IsValid;

            Assert.IsFalse(estaValido);
        }

        [TestMethod]
        [TestCategory("Servico Validate CriterioAceitacao")]
        public void ServicoValidate_False()
        {
            var novo = InstanciaCriterioAceitacao();
            novo.Incerteza = null;

            var repositorioCriterioAceitacao = new CriterioAceitacaoRepositorio();

            var servicoCriterioAceitacao = new CriterioAceitacaoServico(repositorioCriterioAceitacao);

            List<string> erros = new List<string>();

            servicoCriterioAceitacao.Valido(novo, ref erros);

            Assert.IsFalse(erros.Count == 0);
        }

        [TestMethod]
        [TestCategory("CamposObrigatorios CriterioAceitacao")]
        public void ServicoValidate_true()
        {
            var novo = InstanciaCriterioAceitacao();

            var repositorioCriterioAceitacao = new CriterioAceitacaoRepositorio();

            var servicoCriterioAceitacao = new CriterioAceitacaoServico(repositorioCriterioAceitacao);

            List<string> erros = new List<string>();

            servicoCriterioAceitacao.Valido(novo, ref erros);

            Assert.IsTrue(erros.Count == 0);
        }

        //[TestMethod]
        //[TestCategory("Insert CriterioAceitacao")]
        //public void Insert()
        //{
        //    var novo = InstanciaCriterioAceitacao();

        //    novo.ValorMaximo = 10;

        //    var repositorioCriterioAceitacao = new CriterioAceitacaoRepositorio();

        //    var servicoCriterioAceitacao = new CriterioAceitacaoAppServico(repositorioCriterioAceitacao);

        //    List<string> erros = new List<string>();

        //    servicoCriterioAceitacao.Valido(novo, ref erros);

        //    if (erros.Count == 0 )
        //        servicoCriterioAceitacao.Add(novo);
        //}

        //[TestMethod]
        //[TestCategory("Update CriterioAceitacao")]
        //public void Update()
        //{
        //    var atualizar = InstanciaCriterioAceitacao();
        //    atualizar.IdCriterioAceitacao = 1;
        //    atualizar.ValorMaximo = 20;

        //    _criterioAceitacaoRepositorio = new CriterioAceitacaoRepositorio();

        //    _criterioAceitacaoServico = new CriterioAceitacaoAppServico(_criterioAceitacaoRepositorio);

        //    _criterioAceitacaoServico.Update(atualizar);
        //}

        //[TestMethod]
        //[TestCategory("Delete CriterioAceitacao")]
        //public void Delete()
        //{
        //    var deletar = new CriterioAceitacao() { IdCriterioAceitacao = 1 };

        //    _criterioAceitacaoRepositorio = new CriterioAceitacaoRepositorio();

        //    _criterioAceitacaoServico = new CriterioAceitacaoAppServico(_criterioAceitacaoRepositorio);

        //    _criterioAceitacaoServico.Remove(deletar);
        //}
    }
}
