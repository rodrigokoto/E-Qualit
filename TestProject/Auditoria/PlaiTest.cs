using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using Dominio.Validacao.Plais;
using System.Collections.Generic;
using Dominio.Interface.Repositorio;
using Moq;
using ApplicationService.Servico;
using ApplicationService.Interface;

namespace TestProject.Auditoria
{
    [TestClass]
    public class PlaiTest
    {
        private Plai _plai;

        public void Construtor()
        {
            _plai = new Plai();

            _plai.IdPai = 1;
            _plai.IdSite = 5;
            _plai.IdRepresentanteDaDirecao = 2;
            _plai.Endereco = "Teste Endereco";
            _plai.Escopo = "Teste de Escopo";
            _plai.Gestores = "Gestores";
            _plai.DataReuniaoAbertura = DateTime.Now;
            _plai.DataReuniaoEncerramento = DateTime.Now;
            _plai.DataCadastro = DateTime.Now;

        }

        [TestMethod]
        [TestCategory("Plai")]
        public void Plai_Validacao_True()
        {
            Construtor();

            var validacao = new AptoParaCadastroValidacao().Validate(_plai);

            Assert.IsTrue(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Plai")]
        public void Plai_Validacao_False()
        {
            _plai = new Plai();

            var validacao = new AptoParaCadastroValidacao().Validate(_plai);

            Assert.IsFalse(validacao.IsValid);
        }


        [TestMethod]
        [TestCategory("Plai")]
        public void Plai_Valido_True()
        {
            Construtor();
            var repositorioPai = new Mock<IPaiRepositorio>();
            var repositorio = new Mock<IPlaiRepositorio>();
            var log = new Mock<ILogRepositorio>();
            var mensagemServico = new Mock<INotificacaoMensagemAppServico>();
                var servico = new PlaiAppServico(repositorio.Object, repositorioPai.Object, 
                                        log.Object, mensagemServico.Object);
            var erros = new List<string>();
            var valido = false;

            servico.Valido(_plai, ref erros);

            if (erros.Count == 0)
            {
                valido = true;
            }
            
            Assert.IsTrue(valido);
        }

        [TestMethod]
        [TestCategory("Plai")]
        public void Plai_Valido_False()
        {
            _plai = new Plai();
            var repositorioPai = new Mock<IPaiRepositorio>();
            var repositorio = new Mock<IPlaiRepositorio>();
            var log = new Mock<ILogRepositorio>();
            var mensagemServico = new Mock<INotificacaoMensagemAppServico>();
            var servico = new PlaiAppServico(repositorio.Object, repositorioPai.Object, 
                                          log.Object, mensagemServico.Object);
            var erros = new List<string>();
            var valido = false;


            servico.Valido(_plai, ref erros);

            if (erros.Count == 0)
            {
                valido = true;
            }

            Assert.IsFalse(valido);
        }


        //[TestMethod]
        //[TestCategory("Plai Processo")]
        //public void Insert()
        //{
        //    Construtor();
        //    _plai.PlaiProcessoNorma  = new List<PlaiProcessoNorma>();
        //    _plai.PlaiProcessoNorma .Add(new PlaiProcessoNorma
        //    {
        //        IdProcessoAuditoria = 5,
        //        IdNorma = 3,
        //        Data = DateTime.Now
        //    });

        //    var repositorio = new PlaiRepositorio();

        //    repositorio.Add(_plai);
        //}


        //[TestMethod]
        //[TestCategory("Plai")]
        //public void Insert()
        //{
        //    Construtor();
        //    var repositorio = new PlaiRepositorio();

        //    repositorio.Add(_plai);
        //}

        //[TestMethod]
        //[TestCategory("Plai")]
        //public void Update()
        //{
        //    Construtor();
        //    _plai.IdPlai = 3;
        //    _plai.Endereco = "Endereço 2";
        //    var repositorio = new PlaiRepositorio();

        //    repositorio.Update(_plai);
        //}

        //[TestMethod]
        //[TestCategory("Norma")]
        //public void Delete()
        //{
        //    Construtor();
        //    _plai.IdPlai = 3;
        //    var repositorio = new PlaiRepositorio();

        //    repositorio.Remove(_plai);
        //}
    }
}
