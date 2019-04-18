using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Validacao.RegistroConformidades.AcaoCorretivas;
using Dominio.Entidade;
using System.Collections.Generic;
using Dominio.Enumerado;

namespace TestProject.RegistroConformidades
{
    [TestClass]
    public class AcaoCorretivaTest
    {
        #region Cenarios Etapa1
        private CamposObrigatoriosAcaoCorretivaEtapa1Validation _validaCamposEtapa1 = new CamposObrigatoriosAcaoCorretivaEtapa1Validation();
        private RegistroConformidade _acEtapa1Valido;
        private RegistroConformidade _acEtapa1Invalido;
        #endregion
        #region Cenarios Etapa2
        private CamposObrigatoriosAcaoCorretivaEtapa2Validation _validaCamposEtapa2 = new CamposObrigatoriosAcaoCorretivaEtapa2Validation();
        private RegistroConformidade _acEtapa2NaoProcedenteValido;
        private RegistroConformidade _acEtapa2NaoProcedenteInvalido;

        private RegistroConformidade _acEtapa2ComProcedenteValido;
        private RegistroConformidade _acEtapa2ComProcedenteInvalido;

        private RegistroAcaoImediata _acaoImediataEtapa2;
        #endregion

        [TestInitialize]
        public void IniciadorCenarios()
        {
            #region Cenarios Etapa1
            _acEtapa1Valido = new RegistroConformidade
            {
                //IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 1,
                TipoRegistro = "ac",
                StatusEtapa = (int)EtapasRegistroConformidade.AcaoImediata,
                IdSite = 1,
                IdProcesso = 1,
                EvidenciaImg = "imgEtapa1",
                DtEmissao = DateTime.Now,      
                DtInclusao = DateTime.Now,          
                //Tags = new List<string>() { "Etapa1", "Nova Ação Corretiva" },
                IdResponsavelAcaoCorretiva = 2,
                IdResponsavelEtapa = 2,
                IdUsuarioAlterou = 2,
                IdUsuarioIncluiu = 2,
            };
            _acEtapa1Invalido = new RegistroConformidade
            {
                IdRegistroConformidade = 0,
                DescricaoRegistro = "",
                IdEmissor = 1,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "ac",
                StatusEtapa = (int)EtapasRegistroConformidade.AcaoImediata,
                IdSite = 1,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
                //Tags = new List<string>() { "Etapa1", "Nova Ação Corretiva" },
                IdResponsavelAcaoCorretiva = 2,
            };
            #endregion
            #region Cenarios Etapa2
            _acaoImediataEtapa2 = new RegistroAcaoImediata()
            {
                Descricao = "Descricao Acao Imediata",
                IdResponsavelImplementar = 2,
                DtPrazoImplementacao = DateTime.Today,
                IdRegistroConformidade = 0,
            };
            _acEtapa2NaoProcedenteValido = new RegistroConformidade
            {
                #region _acEtapa1Valido
                IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 12,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "ac",
                StatusEtapa = (int)EtapasRegistroConformidade.Implementacao,
                IdSite = 5,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
               // Tags = new List<string>() { "Etapa1", "Nova Ação Corretiva" },
                #endregion

                DtDescricaoAcao = DateTime.Now,
                EProcedente = false,
                DescricaoAcao = "Justificativa",
               
            };
            _acEtapa2NaoProcedenteInvalido = new RegistroConformidade
            {
                #region _acEtapa1Valido
                IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 12,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "ac",
                StatusEtapa = (int)EtapasRegistroConformidade.Implementacao,
                IdSite = 5,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
                //Tags = new List<string>() { "Etapa1", "Nova Ação Corretiva" },
                #endregion

                DtDescricaoAcao = DateTime.Now,
                EProcedente = false,
                DescricaoAcao = null,
            };

            _acEtapa2ComProcedenteValido = new RegistroConformidade
            {
                #region _acEtapa1Valido
                IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 12,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "ac",
                StatusEtapa = (int)EtapasRegistroConformidade.Implementacao,
                IdSite = 5,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
                //Tags = new List<string>() { "Etapa1", "Nova Ação Corretiva" },
                #endregion

                DtDescricaoAcao = DateTime.Now,
                EProcedente = true,
                DescricaoAcao = "Justificativa",
                IdResponsavelAnalisar = 2,
                AcoesImediatas = new List<RegistroAcaoImediata>() { _acaoImediataEtapa2 }

            };
            _acEtapa2ComProcedenteInvalido = new RegistroConformidade
            {
                #region _acEtapa1Valido
                IdRegistroConformidade = 0,
                DescricaoRegistro = "O que Falhou",
                IdEmissor = 12,
                DtEmissao = DateTime.Now,
                 TipoRegistro = "ac",
                StatusEtapa = (int)EtapasRegistroConformidade.Implementacao,
                IdSite = 5,
                IdProcesso = 2,
                EvidenciaImg = "imgEtapa1",
               // Tags = new List<string>() { "Etapa1", "Nova Ação Corretiva" },
                #endregion

                DtDescricaoAcao = DateTime.Now,
                EProcedente = true,
                DescricaoAcao = null,
            };
            #endregion
        }

        //[TestMethod]
        //[TestCategory("Ação Corretiva - Etapa 1 Causa")]
        //public void Cadastrar_Primeira_Etapa_True()
        //{
        //    var result = _validaCamposEtapa1.Validate(_acEtapa1Valido);

        //    Assert.IsTrue(result.IsValid);
        //}

        [TestMethod]
        [TestCategory("Ação Corretiva - Etapa 1 Causa")]
        public void Cadastrar_Primeira_Etapa_False()
        {
            var result = _validaCamposEtapa1.Validate(_acEtapa1Invalido);

            Assert.IsFalse(result.IsValid);
        }

        //[TestMethod]
        //[TestCategory("Ação Corretiva - Etapa 2 Identificação da Ação")]
        //public void Cadastrar_Segunda_Etapa_Nao_Procedente_True()
        //{

        //    var result = _validaCamposEtapa2.Validate(_acEtapa2NaoProcedenteValido);

        //    Assert.IsTrue(result.IsValid);
        //}

        [TestMethod]
        [TestCategory("Ação Corretiva - Etapa 2 Identificação da Ação")]
        public void Cadastrar_Segunda_Etapa_Nao_Procedente_False()
        {

            var result = _validaCamposEtapa2.Validate(_acEtapa2NaoProcedenteInvalido);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        [TestCategory("Ação Corretiva - Etapa 2 Identificação da Ação")]
        public void Cadastrar_Segunda_Etapa_Com_Procedente_False()
        {

            var result = _validaCamposEtapa2.Validate(_acEtapa2ComProcedenteInvalido);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [TestCategory("Ação Corretiva - Etapa 3 Reverificação")]
        public void Cadastrar_Terceira_Etapa__True()
        {

        }

        [TestMethod]
        [TestCategory("Ação Corretiva - Etapa 3 Reverificação")]
        public void Cadastrar_Terceira_Etapa__False()
        {

        }
    }
}
