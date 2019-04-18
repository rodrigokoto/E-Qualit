using DAL.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Dominio.Servico;

namespace Dominio.Entidade
{
    [TestClass]
    public class ControleImpressaoTest
    {
        private ControleImpressaoRepositorio _controleImpressaoRepositorio;

        private ControleImpressao _controleImpressao;

        public ControleImpressaoTest()
        {
            _controleImpressao = new ControleImpressao()
            {
                IdFuncionalidade = 2,
                CodigoReferencia = "1",
                CopiaControlada = true,
                IdUsuarioDestino = 1,
                DataInclusao = DateTime.Now,
                IdUsuarioIncluiu = 1,
                DataImpressao = DateTime.Now
            };
        }

        //[TestMethod]
        //public void Insert()
        //{
        //    _controleImpressaoRepositorio = new ControleImpressaoRepositorio();

        //    _controleImpressaoRepositorio.Add(_controleImpressao);
        //}

        [TestMethod]
        [TestCategory("ControleImpressao")]
        public void Validacao_true()
        {
            _controleImpressaoRepositorio = new ControleImpressaoRepositorio();
            var controleImpressaoServico = new ControleImpressaoServico(_controleImpressaoRepositorio);

            List<string> erros = new List<string>();

            controleImpressaoServico.Valido(_controleImpressao, ref erros);

            Assert.IsTrue(erros.Count == 0);
        }

        [TestMethod]
        [TestCategory("ControleImpressao")]
        public void Validacao_false()
        {
            _controleImpressaoRepositorio = new ControleImpressaoRepositorio();
            var controleImpressaoServico = new ControleImpressaoServico(_controleImpressaoRepositorio);

            List<string> erros = new List<string>();

            _controleImpressao.DataImpressao = DateTime.MinValue;

            controleImpressaoServico.Valido(_controleImpressao, ref erros);

            Assert.IsFalse(erros.Count == 0);
        }
    }
}
