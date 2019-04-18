using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Especificacao.DocDocumentos;
using Dominio.Interface.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace TestProject.ControlDoc
{
    [TestClass]
    public class ControlDocTest
    {
        private DocDocumento _documento;

        public DocDocumentoXML gerarNovoDocXml()
        {
            DocDocumentoXML dcxml = new DocDocumentoXML();
            dcxml.Fluxo = "Fluxo";
            dcxml.Licenca = new Licenca() { DataEmissao = DateTime.Now, DataVencimento = DateTime.Now };
            dcxml.Recursos = new Recurso() { Texto = String.Empty };
            dcxml.Residuo = new Residuo() { DataCriacao = DateTime.Now, Processo = String.Empty, Revisao = String.Empty };
            dcxml.Texto = "Texto";

            List<Risco> risco = new List<Risco>();
            risco.Add(new Risco() { Classificacao = "Classificacao", Texto = "Texto" });
            risco.Add(new Risco() { Classificacao = "Classificacao2", Texto = "Texto2" });
            risco.Add(new Risco() { Classificacao = "Classificacao3", Texto = "Texto3" });

            List<RegistroDocXML> registro = new List<RegistroDocXML>();
            registro.Add(new RegistroDocXML()
            {
                Armazenar = "Armazenar",
                Disposicao = "Disposicao",
                Identificar = "Indentificar",
                Proteger = "Proteger",
                Recuperar = "Recuperar",
                Retencao = "Retencao"
            });

            dcxml.Riscos = risco;
            dcxml.Registros = registro;

            dcxml.Licenca.Arquivos = "Arquivos";

            return dcxml;
        }

        public string gerarXML()
        {
            string x = File.ReadAllText(@"C:/Projetos/testeDoc.txt");
            return x;
        }

        public DocTemplate gerarTemplateDocDocumento(String template)
        {
            return new DocTemplate() { TpTemplate = template };
        }

        public DocDocumento Nova_Instancia_DocDocumento()
        {
            //List<DocumentoCargo> lstDocCargo = new List<DocumentoCargo>();
            //lstDocCargo.Add(new DocumentoCargo { IdCargo = 28 });
            //lstDocCargo.Add(new DocumentoCargo { IdCargo = 14 });
            //lstDocCargo.Add(new DocumentoCargo { IdCargo = 18 });
            //lstDocCargo.Add(new DocumentoCargo { IdCargo = 19 });
            //lstDocCargo.Add(new DocumentoCargo { IdCargo = 20 });

            List<DocTemplate> lstDocTemplate = new List<DocTemplate>();
            lstDocTemplate.Add(new DocTemplate { TpTemplate = DocTemplate.FluxoTemplate });

            List<DocUsuarioVerificaAprova> listaUAprovadores = new List<DocUsuarioVerificaAprova>();
            listaUAprovadores.Add(new DocUsuarioVerificaAprova { IdUsuario = 1, TpEtapa = "A" });
            listaUAprovadores.Add(new DocUsuarioVerificaAprova { IdUsuario = 1, TpEtapa = "A" });
            listaUAprovadores.Add(new DocUsuarioVerificaAprova { IdUsuario = 1, TpEtapa = "A" });

            List<DocUsuarioVerificaAprova> listaUVerificadores = new List<DocUsuarioVerificaAprova>();
            listaUVerificadores.Add(new DocUsuarioVerificaAprova { IdUsuario = 1, TpEtapa = "V", FlAprovou = null });
            listaUVerificadores.Add(new DocUsuarioVerificaAprova { IdUsuario = 1, TpEtapa = "V", FlAprovou = null });


            List<DocUsuarioVerificaAprova> listaUsuarios = new List<DocUsuarioVerificaAprova>();
            listaUsuarios.Add(new DocUsuarioVerificaAprova { IdUsuario = 1, TpEtapa = "V", FlAprovou = null });
            listaUsuarios.Add(new DocUsuarioVerificaAprova { IdUsuario = 1, TpEtapa = "A", FlAprovou = null });

            //List<DocumentoAssunto> assuntos = new List<DocumentoAssunto>();
            //assuntos.Add(new DocumentoAssunto { DataAssunto = DateTime.Now, Descricao = "Teste Assunto 001", Revisao = "Revisao 001" });
            //assuntos.Add(new DocumentoAssunto { DataAssunto = DateTime.Now, Descricao = "Teste Assunto 002", Revisao = "Revisao 021" });

            //List<DocumentoComentario> comentarios = new List<DocumentoComentario>();
            //comentarios.Add(new DocumentoComentario { Descricao = "Teste de Comentario 001", DataComentario = DateTime.Now, IdUsuario = 1 });
            //comentarios.Add(new DocumentoComentario { Descricao = "Teste de Comentario 002", DataComentario = DateTime.Now, IdUsuario = 1 });

            return new DocDocumento
            {
                IdDocumentoPai = 1,
                IdSite = 1,
                IdDocIdentificador = 2,
                IdProcesso = 1,
                IdCategoria = 13,
                Titulo = "NmTitulo",
                IdSigla = 13,
                NumeroDocumento = "CdNumero",
                NuRevisao = 0,
                IdElaborador = 1,
                DtPedidoVerificacao = DateTime.Now,
                DtVerificacao = DateTime.Now,
                DtVencimento = DateTime.Now,
                DtEmissao = DateTime.Now,
                DtPedidoAprovacao = DateTime.Now,
                DtAprovacao = DateTime.Now,
                DtNotificacao = DateTime.Now,
                DtAlteracao = DateTime.Now,
                FlWorkFlow = false,
                FlRevisaoPeriodica = false,
                FlStatus = 0,
                IdUsuarioIncluiu = 1,
                DocTemplate = lstDocTemplate,
                DocUsuarioVerificaAprova = listaUsuarios,
                Verificadores = listaUsuarios,
                Aprovadores = listaUsuarios,
                //DocCargo = lstDocCargo,
                //Assuntos = assuntos,
                //Comentarios = comentarios
            };
        }

        public DocDocumento gerarDocDocumento()
        {
            List<DocumentoCargo> lstDocCargo = new List<DocumentoCargo>();
            lstDocCargo.Add(new DocumentoCargo() { Id = 1 });

            List<DocTemplate> lstDocTemplate = new List<DocTemplate>();
            lstDocTemplate.Add(new DocTemplate { IdDocTemplate = 1, TpTemplate = DocTemplate.FluxoTemplate });

            return new DocDocumento
            {
                IdDocumento = 1,
                IdDocumentoPai = 1,
                IdSite = 5,
                IdDocIdentificador = 2,
                IdCategoria = 3,
                Titulo = "NmTitulo",
                IdSigla = 12,
                NumeroDocumento = "CdNumero",
                DtAlteracao = DateTime.Now,
                FlWorkFlow = false,
                IdElaborador = 5,
                FlRevisaoPeriodica = false,
                FlStatus = 0,
                Elaborador = new Usuario { IdUsuario = 34 },
                DocCargo = lstDocCargo,
                Processo = new Processo() { IdProcesso = 7 },
                DocTemplate = lstDocTemplate
            };
        }

        private void Inicializador()
        {
            _documento = new DocDocumento
            {
                IdDocumento = 37,
                IdElaborador = 5
            };
        }

        [TestMethod]
        public void Deve_Ter_Uma_Unica_Copia_Com_Status_Diferente_De_Revisar_True()
        {

            var repositorio = new Mock<IDocDocumentoRepositorio>();


            var documentos = new List<DocDocumento>();
            documentos.Add(new DocDocumento
            {
                FlStatus = (int)StatusDocumento.Aprovado
            });


            repositorio.Setup(x => x.Get(It.IsAny<Expression<Func<DocDocumento, bool>>>(), null, null))
                                    .Returns(documentos);


            var especification = new DeveTerUmaUnicaCopiaComStatusDiferenteDeRevisar(repositorio.Object);



            var retorno = especification.IsSatisfiedBy(documentos.FirstOrDefault());

            Assert.IsTrue(retorno);
        }
        //[TestMethod]
        //public void InserirDocumento()
        //{

        //    IDocDocumentoRepositorio _docDocumentoServico = new DocDocumentoRepositorio();
        //    IRegistroConformidadesRepositorio _registroConformidadeServico = new RegistroConformidadesRepositorio();
        //    IDocTemplateRepositorio _docTemplate = new DocTemplateRepositorio();
        //    IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprova = new DocUsuarioVerificaAprovaRepositorio();
        //    IControladorCategoriasRepositorio registroConformidade = new ControladorCategoriasRepositorio();



        //    INotificacaoRepositorio notificacaoRepositorio = new NotificacaoRepositorio();
        //    IUsuarioRepositorio usuario = new UsuarioRepositorio();

        //    IDocCargoRepositorio docCargoRepositorio = new DocCargoRepositorio();

        //    INotificacaoServico notificacao = new NotificacaoServico(notificacaoRepositorio, usuario);

        //    var docServico = new DocDocumentoServico(_docDocumentoServico,
        //                                             registroConformidade,
        //                                             _docUsuarioVerificaAprova,
        //                                             _docTemplate, docCargoRepositorio, notificacao);

        //    var novoDocumento = Nova_Instancia_DocDocumento();

        //    docServico.Add(novoDocumento);
        //}

        //[TestMethod]
        //public void setarDocDocumentoXML()
        //{
        //    DocDocumentoXML dcdocxml = gerarNovoDocXml();

        //    DocDocumento dc = new DocDocumento();
        //    dc.XmlMetadata = Biblioteca.ConversorXML.EscreveXML(dcdocxml);

        //    Assert.IsTrue(!String.IsNullOrWhiteSpace(dc.XmlMetadata));
        //}

        //[TestMethod]
        //public void ValidarDocXMLTemplateFluxo()
        //{
        //    List<DocTemplate> listaTemplate = new List<DocTemplate>();
        //    listaTemplate.Add(gerarTemplateDocDocumento(DocTemplate.FluxoTemplate));
        //    listaTemplate.Add(gerarTemplateDocDocumento(DocTemplate.GestaoDeRiscoTemplate));
        //    listaTemplate.Add(gerarTemplateDocDocumento(DocTemplate.RegistrosTemplate));
        //    listaTemplate.Add(gerarTemplateDocDocumento(DocTemplate.TextoTemplate));
        //    listaTemplate.Add(gerarTemplateDocDocumento(DocTemplate.LicencaTemplate));
        //    listaTemplate.Add(gerarTemplateDocDocumento(DocTemplate.UploadTemplate));

        //    DocDocumentoXML xml = gerarNovoDocXml();

        //    DocDocumentoXMLValidacao val = new DocDocumentoXMLValidacao();

        //    Assert.IsTrue(val.IsValid(xml, listaTemplate));
        //}

        //[TestMethod]
        //public void ValidarDocDocumento_True()
        //{
        //    DocDocumento dc = gerarDocDocumento();

        //    Assert.IsTrue(true);
        //}

        //[TestMethod]
        //public void Validar_DocDocumento_Sem_CdLetra_False()
        //{
        //    DocDocumento dc = gerarDocDocumento();

        //    dc.CdLetra = string.Empty;
        //}

        //[TestMethod]
        //public void Valida_Etapa_Do_Documento_Elaboracao_Perfil_Colaborador_True()
        //{
        //    Inicializador();

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();

        //    docDocumentoRepositorio.Setup(x => x.DocumentoElaboracaoColaborador(
        //            It.IsAny<int>(),
        //            It.IsAny<int>(),
        //            It.IsAny<int>())).
        //        Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);

        //    var retorno = docServico.DocumentoPerfilEtapaIdDocumento(1, (int)StatusDocumento.Elaboracao, 1, (int)PerfisAcesso.Colaborador, 1);

        //    Assert.IsNotNull(retorno);
        //}

        //[TestMethod]
        //public void Valida_Etapa_Do_Documento_Elaboracao_Perfil_Administrador_True()
        //{
        //    Inicializador();

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();

        //    docDocumentoRepositorio.Setup(x => x.DocumentoEtapa(
        //            It.IsAny<int>(),
        //            It.IsAny<int>(),
        //            It.IsAny<int>())).
        //        Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);


        //    var retorno = docServico.DocumentoPerfilEtapaIdDocumento(1, (int)StatusDocumento.Elaboracao, 1, (int)PerfisAcesso.Administrador, 1);

        //    Assert.IsNotNull(retorno);
        //}

        //[TestMethod]
        //public void Valida_Etapa_Do_Documento_SemEtapa_Perfil_SemPerfil_True()
        //{
        //    Inicializador();

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();

        //    docDocumentoRepositorio.Setup(x => x.DocumentoEtapa(
        //            It.IsAny<int>(),
        //            It.IsAny<int>(),
        //            It.IsAny<int>())).
        //        Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);


        //    var retorno = docServico.DocumentoPerfilEtapaIdDocumento(1, 0, 1, 0, 1);

        //    Assert.IsNull(retorno);
        //}

        //[TestMethod]
        //public void Valida_Etapa_Do_Documento_SemEtapa_Perfil_Administrador_True()
        //{
        //    Inicializador();

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();

        //    docDocumentoRepositorio.Setup(x => x.DocumentoEtapa(
        //                It.IsAny<int>(),
        //                It.IsAny<int>(),
        //                It.IsAny<int>())).
        //            Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);

        //    var retorno = docServico.DocumentoPerfilEtapaIdDocumento(1, 10, 1, (int)PerfisAcesso.Administrador, 1);

        //    Assert.IsNull(retorno);
        //}

        //[TestMethod]
        //public void Validar_Remove_Servico_Colaborador()
        //{
        //    int _idusuario = 5;

        //    Inicializador();

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();

        //    docDocumentoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_documento);
        //    docDocumentoRepositorio.Setup(x => x.RemoverDocumento(It.IsAny<DocDocumento>()));

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                            controladorCategriaRepositorio.Object,
        //                                            docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);


        //    var retorno = docServico.RemoverDocumentoPorPerfilUsuario(It.IsAny<int>(), _idusuario, (int)PerfisAcesso.Colaborador);

        //    Assert.IsTrue(retorno);

        //}

        //[TestMethod]
        //public void Validar_EnviarAprovado_Servico_Colaborador()
        //{
        //    int _idusuario = 34;

        //    Inicializador();

        //    _documento = Nova_Instancia_DocDocumento();

        //    _documento.FlStatus = (int)StatusDocumento.Aprovado;

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();

        //    docDocumentoRepositorio.Setup(x => x.Update(It.IsAny<DocDocumento>()));

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);

        //    var retorno = docServico.EnviarDocumentoParaAprovado(It.IsAny<int>(), _idusuario);

        //    Assert.IsTrue(retorno);

        //}

        //[TestMethod]
        //public void Validar_EnviarAprovado_Servico_Colaborador_AprovadoPorTodos()
        //{
        //    int _idusuario = 34;

        //    Inicializador();

        //    _documento = Nova_Instancia_DocDocumento();

        //    _documento.FlStatus = (int)StatusDocumento.Aprovado;

        //    _documento.DocUsuarioVerificaAprova.ForEach(a => a.FlAprovou = true);

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();



        //    docDocumentoRepositorio.Setup(x => x.Update(It.IsAny<DocDocumento>()));

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);


        //    var retorno = docServico.AprovarDocumento(It.IsAny<int>(), _idusuario);

        //    Assert.IsTrue(retorno);

        //}

        //[TestMethod]
        //public void Validar_EnviarAprovacao_Servico_Colaborador()
        //{
        //    int _idusuario = 34;

        //    Inicializador();

        //    _documento = Nova_Instancia_DocDocumento();

        //    _documento.FlStatus = (int)StatusDocumento.Aprovacao;

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();

        //    docDocumentoRepositorio.Setup(x => x.Update(It.IsAny<DocDocumento>()));
        //    docDocumentoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);

        //    var retorno = docServico.AprovarDocumento(It.IsAny<int>(), _idusuario);

        //    Assert.IsTrue(retorno);
        //}

        //[TestMethod]
        //public void Validar_EnviarVerificacao_Servico_Colaborador()
        //{
        //    int _idusuario = 34;

        //    Inicializador();

        //    _documento = Nova_Instancia_DocDocumento();

        //    _documento.FlStatus = (int)StatusDocumento.Aprovacao;

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();

        //    docDocumentoRepositorio.Setup(x => x.Update(It.IsAny<DocDocumento>()));
        //    docDocumentoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);


        //    var retorno = docServico.VerificarDocumento(_documento, _idusuario);

        //    Assert.IsTrue(retorno);
        //}

        //[TestMethod]
        //public void Validar_CriarRevisaoDocumento_Servico_Colaborador()
        //{
        //    int _idusuario = 34;

        //    Inicializador();

        //    _documento = Nova_Instancia_DocDocumento();

        //    _documento.FlStatus = (int)StatusDocumento.Aprovado;

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();


        //    docDocumentoRepositorio.Setup(x => x.Update(It.IsAny<DocDocumento>()));
        //    docDocumentoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);

        //    var retorno = docServico.CriarRevisaoDocumento(It.IsAny<int>(), _idusuario);

        //    Assert.IsNotNull(retorno);
        //}

        //[TestMethod]
        //public void Validar_EnviarElaboracao_Servico_Colaborador()
        //{
        //    int _idusuario = 34;

        //    Inicializador();

        //    _documento = Nova_Instancia_DocDocumento();

        //    _documento.FlStatus = (int)StatusDocumento.Aprovacao;

        //    var docDocumentoRepositorio = new Mock<IDocDocumentoRepositorio>();
        //    var controladorCategriaRepositorio = new Mock<IControladorCategoriasRepositorio>();
        //    var processoRepositorio = new Mock<IProcessoRepositorio>();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var docTemplate = new Mock<IDocTemplateRepositorio>();
        //    var usuario = new Mock<IUsuarioServico>();

        //    var docCargoRepositorio = new Mock<IDocCargoRepositorio>();

        //    docDocumentoRepositorio.Setup(x => x.Update(It.IsAny<DocDocumento>()));
        //    docDocumentoRepositorio.Setup(x => x.GetById(It.IsAny<int>())).Returns(_documento);

        //    var docServico = new DocDocumentoServico(docDocumentoRepositorio.Object,
        //                                             controladorCategriaRepositorio.Object,
        //                                             docUsuarioVerificaAprovaRepositorio.Object,
        //                                             docTemplate.Object,
        //                                             docCargoRepositorio.Object);

        //    var retorno = docServico.EnviarDocumentoParaElaboracao(It.IsAny<int>(), _idusuario, (int)PerfisAcesso.Colaborador);

        //    Assert.IsTrue(retorno);
        //}

        //[TestMethod]
        //public void Validar_Campos_Obrigatorios_True()
        //{
        //    var documento = Nova_Instancia_DocDocumento();

        //    var camposObrigatoriosDoc = new CamposObrigatoriosValidation();
        //    var validacao = camposObrigatoriosDoc.Validate(documento);

        //    Assert.IsTrue(validacao.IsValid);
        //}

        //[TestMethod]
        //public void Validar_Campos_Obrigatorios_False()
        //{
        //    var documento = Nova_Instancia_DocDocumento();

        //    documento.IdSite = 0;
        //    var camposObrigatoriosDoc = new CamposObrigatoriosValidation();
        //    var validacao = camposObrigatoriosDoc.Validate(documento);

        //    Assert.IsFalse(validacao.IsValid);
        //}

        //[TestMethod]
        //public void Validar_Apto_Para_Cadastro_Com_Gestao_De_Risco_Validacao_True()
        //{
        //    var documento = Nova_Instancia_DocDocumento();
        //    documento.GestaoDeRisco = new RegistroConformidade
        //    {
        //        IdRegistroConformidade = 1
        //    };

        //    var validacao = new AptoParaCadastroComGestaoDeRiscoValidacao()
        //                                                   .Validate(documento.GestaoDeRisco);


        //    Assert.IsTrue(validacao.IsValid);
        //}

        //[TestMethod]
        //public void Validar_Apto_Para_Cadastro_Atualizar_Validacao_False()
        //{
        //    var documento = Nova_Instancia_DocDocumento();
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    documento.FlStatus = (int)StatusDocumento.Aprovacao;

        //    var validacao = new AptoParaCadastroAtualizarValidation(docUsuarioVerificaAprovaRepositorio.Object)
        //                                                   .Validate(documento);

        //    Assert.IsFalse(validacao.IsValid);
        //}

        //[TestMethod]
        //public void Validar_Apto_Para_Cadastro_Atualizar_Validacao_True()
        //{
        //    var docUsuarioVerificaAprovaRepositorio = new Mock<IDocUsuarioVerificaAprovaRepositorio>();
        //    var documento = Nova_Instancia_DocDocumento();
        //    documento.FlStatus = (int)StatusDocumento.Elaboracao;

        //    var validacao = new AptoParaCadastroAtualizarValidation(docUsuarioVerificaAprovaRepositorio.Object).Validate(documento);

        //    Assert.IsTrue(validacao.IsValid);
        //}

        [TestMethod]
        public void Nao_Possui_WorkFlow_SemAprovador_True()
        {
            var especification = new PossuiWorkFlowDeveTerAprovadoresEspecification();
            var controlDoc = new DocDocumento();

            var retorno = especification.IsSatisfiedBy(controlDoc);

            Assert.IsTrue(retorno);

        }

        //[TestMethod]
        //public void Possui_WorkFlow_SemAprovador_False()
        //{
        //    var especification = new PossuiWorkFlowDeveTerAprovadoresEspecification();
        //    var controlDoc = new DocDocumento();
        //    controlDoc.FlWorkFlow = true;

        //    var retorno = especification.IsSatisfiedBy(controlDoc);

        //    Assert.IsFalse(retorno);
        //}

        //[TestMethod]
        //public void Possui_WorkFlow_ComAprovador_True()
        //{
        //    var especification = new PossuiWorkFlowDeveTerAprovadoresEspecification();
        //    var controlDoc = new DocDocumento();
        //    controlDoc.FlWorkFlow = true;
        //    controlDoc.Aprovadores = new List<DocUsuarioVerificaAprova>();
        //    controlDoc.Aprovadores.Add(new DocUsuarioVerificaAprova
        //    {
        //        IdUsuario = 1
        //    });

        //    var retorno = especification.IsSatisfiedBy(controlDoc);

        //    Assert.IsTrue(retorno);
        //}

        //[TestMethod]
        //public void Possui_WorkFlow_SemAprovador_True()
        //{
        //    var especification = new PossuiWorkFlowDeveTerAprovadoresEspecification();
        //    var controlDoc = new DocDocumento();
        //    controlDoc.FlWorkFlow = true;

        //    var retorno = especification.IsSatisfiedBy(controlDoc);

        //    Assert.IsFalse(retorno);

        //}

        //[TestMethod]
        //public void Nao_Possui_WorkFlow_SemVerificador_True()
        //{
        //    var especification = new PossuiWorkFlowDeveTerVerificadoresEspecification();
        //    var controlDoc = new DocDocumento();

        //    var retorno = especification.IsSatisfiedBy(controlDoc);

        //    Assert.IsTrue(retorno);

        //}

        //[TestMethod]
        //public void Possui_WorkFlow_SemVerificador_False()
        //{
        //    var especification = new PossuiWorkFlowDeveTerVerificadoresEspecification();
        //    var controlDoc = new DocDocumento();
        //    controlDoc.FlWorkFlow = true;

        //    var retorno = especification.IsSatisfiedBy(controlDoc);

        //    Assert.IsFalse(retorno);
        //}

        //[TestMethod]
        //public void Possui_WorkFlow_ComVerificador_True()
        //{
        //    var especification = new PossuiWorkFlowDeveTerVerificadoresEspecification();
        //    var controlDoc = new DocDocumento();
        //    controlDoc.FlWorkFlow = true;
        //    controlDoc.Verificadores = new List<DocUsuarioVerificaAprova>();
        //    controlDoc.Verificadores.Add(new DocUsuarioVerificaAprova
        //    {
        //        IdUsuario = 1
        //    });

        //    var retorno = especification.IsSatisfiedBy(controlDoc);

        //    Assert.IsTrue(retorno);
        //}

        //[TestMethod]
        //public void Possui_WorkFlow_SemVerificador_True()
        //{
        //    var especification = new PossuiWorkFlowDeveTerVerificadoresEspecification();
        //    var controlDoc = new DocDocumento();
        //    controlDoc.FlWorkFlow = true;

        //    var retorno = especification.IsSatisfiedBy(controlDoc);

        //    Assert.IsFalse(retorno);

        //}

        //[TestMethod]
        //[TestCategory("ControlDoc")]
        //public void CriarRevisaoDocumento_NaoENulo()
        //{

        //    IDocDocumentoRepositorio _docDocumentoServico = new DocDocumentoRepositorio();
        //    IRegistroConformidadesRepositorio _registroConformidadeServico = new RegistroConformidadesRepositorio();
        //    IDocTemplateRepositorio _docTemplate = new DocTemplateRepositorio();
        //    IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprova = new DocUsuarioVerificaAprovaRepositorio();
        //    IControladorCategoriasRepositorio registroConformidade = new ControladorCategoriasRepositorio();

        //    IDocCargoRepositorio docCargoRepositorio = new DocCargoRepositorio();

        //    var docServico = new DocDocumentoServico(_docDocumentoServico,
        //                                             registroConformidade,
        //                                             _docUsuarioVerificaAprova,
        //                                             _docTemplate, docCargoRepositorio);

        //    var novoDocumento = Nova_Instancia_DocDocumento();

        //    //docServico.Add(novoDocumento);

        //    int antigo = novoDocumento.IdDocumento;

        //    var novo = docServico.CriarRevisaoDocumento(novoDocumento.IdDocumento, 2);

        //    Assert.IsNotNull(novo);

        //}

        //[TestMethod]
        //[TestCategory("ControlDoc")]
        //public void ListarAssuntosDoDocumento_ConsultaRealizadaComSucesso()
        //{
        //    IDocDocumentoRepositorio _docDocumentoRepositorio = new DocDocumentoRepositorio();
        //    IRegistroConformidadesRepositorio _registroConformidadeServico = new RegistroConformidadesRepositorio();
        //    IDocTemplateRepositorio _docTemplate = new DocTemplateRepositorio();
        //    IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprova = new DocUsuarioVerificaAprovaRepositorio();
        //    IControladorCategoriasRepositorio registroConformidade = new ControladorCategoriasRepositorio();
        //    IDocCargoRepositorio docCargoRepositorio = new DocCargoRepositorio();


        //    var docServico = new DocDocumentoServico(_docDocumentoRepositorio,
        //                                             registroConformidade,
        //                                             _docUsuarioVerificaAprova,
        //                                             _docTemplate, docCargoRepositorio);

        //    var dadosretorno = _docDocumentoRepositorio.ListarAssuntosDoDocumentoERevisoes(100).ToList();

        //    List<String> assuntos = new List<string>();

        //    foreach (var item in dadosretorno)
        //    {
        //        assuntos.Add(item.DsAssunto);
        //    }

        //    Assert.IsNotNull(dadosretorno);
        //}

        //[TestMethod]
        //[TestCategory("ControlDoc Cargos")]
        //public void Validar_EnviarDocumento_Verificacao_Com_Atualizacao_Dos_Cargos()
        //{
        //    IDocDocumentoRepositorio _docDocumentoRepositorio = new DocDocumentoRepositorio();
        //    IRegistroConformidadesRepositorio _registroConformidadeServico = new RegistroConformidadesRepositorio();
        //    IDocTemplateRepositorio _docTemplate = new DocTemplateRepositorio();
        //    IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprova = new DocUsuarioVerificaAprovaRepositorio();
        //    IControladorCategoriasRepositorio registroConformidade = new ControladorCategoriasRepositorio();
        //    IDocCargoRepositorio docCargoRepositorio = new DocCargoRepositorio();


        //    var docServico = new DocDocumentoServico(_docDocumentoRepositorio,
        //                                             registroConformidade,
        //                                             _docUsuarioVerificaAprova,
        //                                             _docTemplate, docCargoRepositorio);

        //    var alterar = Nova_Instancia_DocDocumento();

        //    alterar.IdDocumento = 44;

        //    Assert.IsTrue(docServico.VerificarDocumento(alterar, 5));
        //}

        //[TestMethod]
        //public void GetDocumentoByID()
        //{
        //    IDocDocumentoRepositorio _documentoRepositorio = new DocDocumentoRepositorio();
        //    IRegistroConformidadesRepositorio _registroConformidadeServico = new RegistroConformidadesRepositorio();
        //    IDocTemplateRepositorio _docTemplate = new DocTemplateRepositorio();
        //    IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprova = new DocUsuarioVerificaAprovaRepositorio();
        //    IControladorCategoriasRepositorio registroConformidade = new ControladorCategoriasRepositorio();
        //    IDocCargoRepositorio docCargoRepositorio = new DocCargoRepositorio();
        //    INotificacaoRepositorio _notificacaoRepositorio = new NotificacaoRepositorio ();
        //    IUsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();

        //    var notificacaoServico = new NotificacaoServico(_notificacaoRepositorio, _usuarioRepositorio);

        //    var docServico = new DocDocumentoServico(_documentoRepositorio,
        //                                             _docUsuarioVerificaAprova,
        //                                             _docTemplate,
        //                                             docCargoRepositorio
        //                                             , notificacaoServico);

        //    var documento = docServico.GetById(2026);
        //}
    }
}
