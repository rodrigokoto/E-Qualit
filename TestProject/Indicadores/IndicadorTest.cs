using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidade;
using Dominio.Validacao.Indicadores;
using Moq;
using System.Collections.Generic;
using DAL.Repository;
using ApplicationService.Servico;
using System.Linq;

namespace TestProject
{
    [TestClass]
    public class IndicadorTest
    {
        private Indicador _indicador = null;
        private IndicadorAppServico _indicadorServico;
        private IndicadorRepositorio _indicadorRepositorio;

        private void Construtor()
        {
            _indicador = new Indicador();

            _indicador.Descricao = "Descrição";
            _indicador.IdResponsavel = 1;
            _indicador.IdProcesso = 1;
            _indicador.PeriodicidaDeAnalises = new List<PeriodicidaDeAnalise> {
                new PeriodicidaDeAnalise
                {
                    Justificativa = "Apenas um teste",
                    Inicio = new DateTime(2017,01,01),
                    Fim = new DateTime(2017, 12, 31),
                    Metas = new List<Meta> {
                        new Meta() {
                            Valor = 100,
                            DataReferencia = new DateTime(2017,01,01)
                        },
                        new Meta() {
                            Valor = 200,
                            DataReferencia = new DateTime(2017,02,01)
                        },
                        new Meta() {
                            Valor = 420,
                            DataReferencia = new DateTime(2017,03,01)
                        },
                    },
                    PlanosDeVoo = new List<PlanoVoo> {
                        new PlanoVoo() {
                            Realizado = 175,
                            DataReferencia = new DateTime(2017,01,01)
                            ,DataAlteracao = DateTime.Now
                            , DataInclusao = DateTime.Now
                        },
                        new PlanoVoo() {
                            Realizado = 132,
                            DataReferencia = new DateTime(2017,02,01)
                            ,DataAlteracao = DateTime.Now
                            , DataInclusao = DateTime.Now
                        },
                        new PlanoVoo() {
                            Realizado = 458,
                            DataReferencia = new DateTime(2017,03,01)
                            ,DataAlteracao = DateTime.Now
                            , DataInclusao = DateTime.Now
                        },
                    }
                }
            };
            _indicador.Objetivo = "Objetivo";
            _indicador.Unidade = "%";
            _indicador.IdSite = 9;
            _indicador.DataAlteracao = DateTime.Now;
            _indicador.DataInclusao = DateTime.Now;
            _indicador.Maximo = 1;

            _indicador.IdUsuarioIncluiu = It.IsAny<int>();
        }

        [TestMethod]
        [TestCategory("Indicador")]
        public void Indicador_Validacao_True()
        {
            Construtor();

            var validacao = new AptoParaCadastroValidation().Validate(_indicador);

            Assert.IsTrue(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Indicador")]
        public void Indicador_Validacao_False()
        {

            _indicador = new Indicador();

            var validacao = new AptoParaCadastroValidation().Validate(_indicador);

            Assert.IsFalse(validacao.IsValid);
        }

        [TestMethod]
        [TestCategory("Indicador")]
        public void Indicador_BateuAMeta_seta_pra_cima()
        {
            _indicadorRepositorio = new IndicadorRepositorio();

            _indicadorServico = new IndicadorAppServico(_indicadorRepositorio);

            var indicador = _indicadorServico.GetById(1002);

            indicador.PeriodicidaDeAnalises.ForEach(p => {
                _indicadorServico.BateuAMeta("Cima", p.Metas, p.PlanosDeVoo);
            });

            var qtde = indicador.PeriodicidaDeAnalises.Select(p => p.PlanosDeVoo.Where(x => x.AtingiuAMeta)).Count();
        }

        [TestMethod]
        [TestCategory("Indicador")]
        public void Indicador_BateuAMeta_seta_pra_baixo()
        {
            _indicadorRepositorio = new IndicadorRepositorio();

            _indicadorServico = new IndicadorAppServico(_indicadorRepositorio);

            var indicador = _indicadorServico.GetById(1002);

            indicador.PeriodicidaDeAnalises.ForEach(p => {
                _indicadorServico.BateuAMeta("Baixo", p.Metas, p.PlanosDeVoo);
            });

            var qtde = indicador.PeriodicidaDeAnalises.Select(p => p.PlanosDeVoo.Where(x => x.AtingiuAMeta)).Count();
        }


        //[TestMethod]
        //public void Insert()
        //{
        //    Construtor();

        //    var repositorio = new IndicadorRepositorio();
        //    repositorio.Add(_indicador);
        //}
    }
}
