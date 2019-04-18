using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Validacao.Normas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Dominio.Servico;

namespace TestProject
{
    [TestClass]
    public class NormaTest
    {
        private Norma _norma;

        public void Construtor()
        {
            _norma = new Norma();

            _norma.IdNorma = 2;
            _norma.IdSite = 5;
            _norma.IdUsuarioIncluiu = 2;
            _norma.Codigo = "123546";
            _norma.Titulo = "Titulo";
            _norma.DataCadastro = DateTime.Now;
            _norma.DataAlteracao = DateTime.Now;
            _norma.Ativo = true;
        }

        [TestMethod]
        [TestCategory("Norma")]
        public void Norma_Validacao_True()
        {
            Construtor();

            var validacao = new AptoParaCadastroValidation().Validate(_norma);

            Assert.IsTrue(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Norma")]
        public void Norma_Validacao_False()
        {
            _norma = new Norma();

            var validacao = new AptoParaCadastroValidation().Validate(_norma);

            Assert.IsFalse(validacao.IsValid);
        }
        

        [TestMethod]
        [TestCategory("Norma")]
        public void Norma_Valido_True()
        {
            Construtor();
            var repositorio = new Mock<INormaRepositorio>();
            var servico = new NormaServico(repositorio.Object);
            var erros = new List<string>();
            var valido = false;

            servico.Valido(_norma, ref erros);

            if (erros.Count == 0)
            {
                valido = true;
            }

            Assert.IsTrue(valido);
        }

        [TestMethod]
        [TestCategory("Norma")]
        public void Norma_Valido_False()
        {
            _norma = new Norma();
            var repositorio = new Mock<INormaRepositorio>();
            var servico = new NormaServico(repositorio.Object);
            var erros = new List<string>();
            var valido = false;

            servico.Valido(_norma, ref erros);

            if (erros.Count == 0)
            {
                valido = true;
            }

            Assert.IsFalse(valido);
        }

        //[TestMethod]
        //[TestCategory("Norma")]
        //public void Insert()
        //{
        //    Construtor();
        //    var repositorio = new NormaRepositorio();

        //    repositorio.Add(_norma);
        //}

        //[TestMethod]
        //[TestCategory("Norma")]
        //public void Update()
        //{
        //    Construtor();
        //    _norma.Item = "Item3";
        //    var repositorio = new NormaRepositorio();

        //    repositorio.Update(_norma);
        //}

        //[TestMethod]
        //[TestCategory("Norma")]
        //public void Delete()
        //{
        //    Construtor();
        //    var repositorio = new NormaRepositorio();

        //    repositorio.Remove(_norma);
        //}
    }
}
