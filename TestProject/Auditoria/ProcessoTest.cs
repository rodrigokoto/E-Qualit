using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using System.Collections.Generic;
using Moq;
using Dominio.Validacao.Processos;
using Dominio.Interface.Repositorio;
using ApplicationService.Servico;
using ApplicationService.Interface;

namespace TestProject
{
    [TestClass]
    public class ProcessoAuditoriaTest
    {
        private Processo _processo;

        public void Construtor()
        {
            _processo = new Processo();

            _processo.IdSite = 1;
            _processo.IdUsuarioIncluiu = 1;
            _processo.Atividade = "Atividade";
            _processo.DocumentosAplicaveis = "Documentos";
            _processo.DataCadastro = DateTime.Now;
            _processo.DataAlteracao = DateTime.Now;
            _processo.FlAtivo = true;
            _processo.FlQualidade = true;
            _processo.Nome = "Processo 2";
        }


        [TestMethod]
        [TestCategory("Processo Auditoria")]
        public void Processo_Validacao_True()
        {
            Construtor();

            var validacao = new AptoParaCadastroValidacao().Validate(_processo);

            Assert.IsTrue(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Processo Auditoria")]
        public void Processo_Validacao_False()
        {
            _processo = new Processo();

            var validacao = new AptoParaCadastroValidacao().Validate(_processo);

            Assert.IsFalse(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Processo Auditoria")]
        public void Processo_Valido_True()
        {
            Construtor();
            var repositorio = new Mock<IProcessoRepositorio>();
            var usuarioCargoRepositorio = new Mock<IUsuarioCargoRepositorio>();
            var cargoProcessoRepositorio = new Mock<ICargoProcessoRepositorio>();
            var log = new Mock<ILogAppServico>();

            var servico = new ProcessoAppServico(log.Object, repositorio.Object, usuarioCargoRepositorio.Object, cargoProcessoRepositorio.Object);
            var erros = new List<string>();
            var valido = false;

            servico.Valido(_processo, ref erros);

            if (erros.Count == 0)
            {
                valido = true;
            }

            Assert.IsTrue(valido);
        }

        [TestMethod]
        [TestCategory("Processo Auditoria")]
        public void Processo_Valido_False()
        {
            _processo = new Processo();
            var repositorio = new Mock<IProcessoRepositorio>();
            var usuarioCargoRepositorio = new Mock<IUsuarioCargoRepositorio>();
            var cargoProcessoRepositorio = new Mock<ICargoProcessoRepositorio>();
            var log = new Mock<ILogAppServico>();

            var servico = new ProcessoAppServico(log.Object, repositorio.Object, usuarioCargoRepositorio.Object, cargoProcessoRepositorio.Object);
            var erros = new List<string>();
            var valido = false;

            servico.Valido(_processo, ref erros);

            if (erros.Count == 0)
            {
                valido = true;
            }

            Assert.IsFalse(valido);
        }

        //[TestMethod]
        //[TestCategory("Processo Auditoria")]
        //public void Insert()
        //{
        //    Construtor();

        //    var repositorio = new ProcessoRepositorio();

        //    _processo.Normas = new List<NormaProcesso>();

        //    _processo.Normas.Add(new NormaProcesso
        //    {
        //        Norma = new Norma
        //        {
        //            Titulo = "Teste de Entity",
        //            IdSite = 5,
        //            Item = "Teste de Item",
        //            Codigo = "001",
        //            Ativo = true,
        //            IdUsuarioIncluiu = 2,
        //            DataCadastro = DateTime.Now,

        //        }
        //    });

        //    repositorio.Add(_processo);
        //}

        //[TestMethod]
        //[TestCategory("Processo Auditoria")]
        //public void Insert()
        //{
        //    _processo = new Processo
        //    {
        //        NmProcesso = "Qualidade",
        //        FlAtivo = true,
        //        FlQualidade = true,
        //        DataCadastro = DateTime.Now
        //    };

        //    var repositorio = new ProcessoRepositorio();

        //    repositorio.Add(_processo);
        //}

        //[TestMethod]
        //[TestCategory("Processo Auditoria")]
        //public void Update()
        //{
        //    Construtor();
        //    _processoAuditoria.Titulo = "Titulo 2";
        //    var repositorio = new ProcessoAuditoriaRepositorio();

        //    repositorio.Update(_processoAuditoria);
        //}

        //[TestMethod]
        //[TestCategory("Processo Auditoria")]
        //public void Delete()
        //{
        //    Construtor();
        //    var repositorio = new ProcessoAuditoriaRepositorio();

        //    repositorio.Remove(_processoAuditoria);
        //}
    }
}
