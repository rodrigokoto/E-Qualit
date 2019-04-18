using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Validacao.RegistroConformidades.GestaoDeRiscos;
using Dominio.Entidade;
using System.Collections.Generic;
using Dominio.Enumerado;

namespace TestProject.RegistroConformidades
{
    [TestClass]
    public class GestaoDeRiscoTest
    {
        #region Cenarios Etapa1
        private CamposObrigatoriosGestaoDeRiscoEtapa1Validation _validaCamposEtapa1 = new CamposObrigatoriosGestaoDeRiscoEtapa1Validation();
        private RegistroConformidade _grEtapa1Valido;
        private RegistroConformidade _grEtapa1Invalido;
        #endregion
        #region Cenarios Etapa2
        private CamposObrigatoriosGestaoDeRiscoEtapa2Validation _validaCamposEtapa2 = new CamposObrigatoriosGestaoDeRiscoEtapa2Validation();
        private RegistroAcaoImediata _acaoImediataEtapa2;
        private RegistroConformidade _grEtapa2Valido;
        private RegistroConformidade _grEtapa2Invalido;

        #endregion
        #region Cenario Etapa3

        #endregion

        [TestInitialize]
        public void IniciadorCenarios()
        {
            #region Cenarios Etapa1
            _grEtapa1Valido = new RegistroConformidade()
            {
                IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 12,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "gr",
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,
                IdSite = 5,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
                //Tags = new List<string>() { "Etapa1", "Nova Gestão de Risco" },
            };
            _grEtapa1Invalido = new RegistroConformidade()
            {

                IdRegistroConformidade = 0,
                //descricao invalida vazia, menos que 4 caracteres (2 erros)
                DescricaoRegistro = "",
                //Emissor null (1 erro)
                IdEmissor = 1,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "gr",
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,
                // site null (1 erro)
                IdSite = 1,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
                //Tags = new List<string>() { "Etapa1", "Nova Gestão de Risco" },
            };
            #endregion
            #region Cenarios Etapa2
            _acaoImediataEtapa2 = new RegistroAcaoImediata()
            {
                Descricao = "Descricao Acao Imediata",
                IdResponsavelImplementar = 2,
                DtPrazoImplementacao = DateTime.Today
            };
            _grEtapa2Valido = new RegistroConformidade()
            {
                #region _grEtapa1Valido
                IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 12,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "gr",
                StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao,
                IdSite = 5,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
                //Tags = new List<string>() { "Etapa1", "Nova Gestão de Risco" },
                #endregion

                IdResponsavelReverificador = 1,
                DtDescricaoAcao = DateTime.Now,
                AcoesImediatas = new List<RegistroAcaoImediata>() { _acaoImediataEtapa2 }
            };

            _grEtapa2Invalido = new RegistroConformidade()
            {
                #region _grEtapa1Valido
                IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 12,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "gr",
                StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao,
                IdSite = 5,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
                //Tags = new List<string>() { "Etapa1", "Nova Gestão de Risco" },
                #endregion

                IdResponsavelAcaoCorretiva = 1,
                DtDescricaoAcao = DateTime.Now,
                AcoesImediatas = new List<RegistroAcaoImediata>()
            };
            #endregion
            #region Cenario Etapa3

            #endregion
        }

        [TestMethod]
        [TestCategory("Gestao de Risco - Etapa 1 Identificação do Risco")]
        public void Cadastrar_Primeira_Etapa_True()
        {
            var result = _validaCamposEtapa1.Validate(_grEtapa1Valido);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Gestao de Risco - Etapa 1 Identificação do Risco")]
        public void Cadastrar_Primeira_Etapa_False()
        {
            var result = _validaCamposEtapa1.Validate(_grEtapa1Invalido);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Gestao de Risco - Etapa 2 Ação para Mitigar o Risco")]
        public void Cadastrar_Segunda_Etapa_True()
        {
            var result = _validaCamposEtapa2.Validate(_grEtapa2Valido);

            Assert.IsTrue(result.IsValid);
        }
        [TestMethod]
        [TestCategory("Gestao de Risco - Etapa 2 Ação para Mitigar o Risco")]
        public void Cadastrar_Segunda_Etapa_False()
        {
            var result = _validaCamposEtapa2.Validate(_grEtapa2Invalido);

            Assert.IsFalse(result.IsValid);
        }
    }
}
