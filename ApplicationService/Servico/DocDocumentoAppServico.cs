using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationService.Entidade;
using System.Configuration;

namespace ApplicationService.Servico
{
    public class DocDocumentoAppServico : BaseServico<DocDocumento>, IDocDocumentoAppServico
    {
        private readonly IDocDocumentoRepositorio _documentoRepositorio;
        private readonly IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprovaRepositorio;
        private readonly IDocTemplateRepositorio _docTemplateRepositorio;
        private readonly IDocCargoRepositorio _docCargoRepositorio;
        private readonly INotificacaoAppServico _notificacaoServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly int NuMeroDiasDispatoComAntecedencia = 5;

        private string _statusVerificacao = "V";
        private string _statusAprovacao = "A";
        private string _categoriaCombo = "CATDOC";

        public DocDocumentoAppServico(IDocDocumentoRepositorio documentoRepositorio,
                                   IDocUsuarioVerificaAprovaRepositorio docUsuarioVerificaAprovaRepositorio,
                                   IDocTemplateRepositorio docTemplate,
                                   IDocCargoRepositorio docCargo,
                                   IUsuarioAppServico usuarioAppServico,
                                   INotificacaoAppServico notificacaoServico) : base(documentoRepositorio)
        {
            _documentoRepositorio = documentoRepositorio;
            _docUsuarioVerificaAprovaRepositorio = docUsuarioVerificaAprovaRepositorio;
            _docTemplateRepositorio = docTemplate;
            _docCargoRepositorio = docCargo;
            _notificacaoServico = notificacaoServico;
            _usuarioAppServico = usuarioAppServico;
        }

        public void RemoverGenerico(object obj)
        {
            _documentoRepositorio.RemoverGenerico(obj);
        }

        public void SalvarDocumento(DocDocumento documento)
        {
            //_documentoRepositorio.Add(documento);
            _documentoRepositorio.SalvarDocumento(documento);
        }

        public void VerificarDocumentoPorUsuario(DocDocumento documento, int idUsuarioLogado)
        {
            var alguemVerificou = documento.DocUsuarioVerificaAprova.FirstOrDefault(x => x.IdUsuario == idUsuarioLogado && x.TpEtapa == _statusVerificacao);
            //var docmentoCtx = _documentoRepositorio.GetById(documento.IdDocumento);
            if (alguemVerificou != null)
            {

                documento.DocUsuarioVerificaAprova
                    .FirstOrDefault(x => x.IdDocUsuarioVerificaAprova == alguemVerificou.IdDocUsuarioVerificaAprova)
                    .FlVerificou = true;
            }
            if (documento.IdLicenca == null)
            {
                documento.Licenca = null;
            }


            if (documento.IdDocExterno == null)
            {
                documento.DocExterno = null;
            }


            _documentoRepositorio.Update(documento);
        }

        public void AprovarDocumento(DocDocumento documento, int idUsuarioLogado)
        {
            documento.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuarioLogado && x.TpEtapa == _statusAprovacao).FirstOrDefault().FlAprovou = true;

            if (AprovadoPorTodos(documento))
                documento.FlStatus = (int)StatusDocumento.Aprovado;

            _documentoRepositorio.Update(documento);
        }

        public void EnviarDocumentoParaElaboracao(DocDocumento doc)
        {
            doc.FlStatus = (int)StatusDocumento.Elaboracao;
            if (doc.GestaoDeRisco != null)
            {
                doc.IdGestaoDeRisco = doc.GestaoDeRisco.IdRegistroConformidade;
            }

            _documentoRepositorio.Update(doc);
        }


        public bool RemoverDocumentoPorPerfilUsuario(int idDocumento, int idUsuarioLogado, int idPerfilUsuario)
        {
            DocDocumento doc = null;
            doc = _documentoRepositorio.GetById(idDocumento);

            if (DocumentoEstaEmElaboracao(doc.FlStatus))
            {
                if (EPerfilColaborador(idPerfilUsuario))
                {
                    if (UsuarioE_Elaborador_Documento(doc.IdElaborador, idUsuarioLogado))
                    {
                        _documentoRepositorio.RemoverDocumento(doc);

                        return true;
                    }
                }
                else if (EPerfilAdiministrativo(idPerfilUsuario))
                {
                    _documentoRepositorio.RemoverDocumento(doc);

                    return true;
                }
            }

            return false;
        }

        public bool VerificadoPorTodos(List<DocUsuarioVerificaAprova> verificadores)
        {
            if (DocumentoFoiVerificadoPortodos(verificadores))
                return true;

            return false;
        }

        public bool AprovadoPorTodos(DocDocumento documento)
        {
            var apenasAprovadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").ToList();
            if (DocumentoFoiAprovadoPortodos(apenasAprovadores))
                return true;

            return false;
        }

        public bool EnviarDocumentoParaAprovado(int idDocumento, int idUsuarioLogado)
        {
            DocDocumento doc = null;
            doc = _documentoRepositorio.GetById(idDocumento);

            if (DocumentoEstaEmAprovacao(doc.FlStatus))
            {
                if (UsuarioEAprovadorDocumento(doc.DocUsuarioVerificaAprova, idUsuarioLogado))
                {
                    if (DocumentoFoiAprovadoPortodos(doc.DocUsuarioVerificaAprova))
                    {
                        doc.FlStatus = (int)StatusDocumento.Aprovado;
                        _documentoRepositorio.Update(doc);

                        return true;
                    }
                }
            }

            return false;
        }

        public void NotificacaoAprovadoresEmail(DocDocumento documento, int idSite, List<DocUsuarioVerificaAprova> aprovadores, int? idProcesso = null)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NotificacaoAprovacaoDocumento-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);

            template = template.Replace("#nroDoc#", documento.NumeroDocumento.ToString());
            template = template.Replace("#titulo#", documento.Titulo);
            var textoEmail = template.Replace("#linkAcesso#", GerarLinkEdicaoDocumento(documento));

            foreach (var aprovador in aprovadores)
            {

                var notificacao = new Notificacao(textoEmail,
                        DateTime.Now,
                        DateTime.Now,
                        (int)Funcionalidades.ControlDoc,
                        idProcesso,
                        1,
                        idSite,
                        5,
                        "E", aprovador.IdUsuario);

                var usuario = _usuarioAppServico.GetById(aprovador.IdUsuario);
                _notificacaoServico.Add(notificacao);

                EnviaEmailSemTemplate(notificacao, usuario);
            }


        }

        public void NotificacaoVerificadoresEmail(DocDocumento documento, int idSite, List<DocUsuarioVerificaAprova> verificadores, int? idProcesso = null)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NotificacaoVerificacaoDocumento-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);

            template = template.Replace("#nroDoc#", documento.NumeroDocumento.ToString());
            template = template.Replace("#titulo#", documento.Titulo);
            var textoEmail = template.Replace("#linkAcesso#", GerarLinkEdicaoDocumento(documento));

            foreach (var verificador in verificadores)
            {

                var notificacao = new Notificacao(textoEmail,
                        DateTime.Now,
                        DateTime.Now,
                        (int)Funcionalidades.ControlDoc,
                        idProcesso,
                        1,
                        idSite,
                        5,
                        "E", verificador.IdUsuario);

                var usuario = _usuarioAppServico.GetById(verificador.IdUsuario);
                _notificacaoServico.Add(notificacao);

                EnviaEmailSemTemplate(notificacao, usuario);
            }
        }

        private string GerarLinkEdicaoDocumento(DocDocumento documento)
        {
            var prefixo = "http://" + ConfigurationManager.AppSettings["Dominio"].ToString();
            return prefixo + "ControlDoc/Editar/" + documento.IdDocumento.ToString();
        }

        public void NotificacaoElaboradorEmail(DocDocumento documento)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NotificacaoElaboracaoDocumento-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);

            template = template.Replace("#nroDoc#", documento.NumeroDocumento.ToString());
            template = template.Replace("#titulo#", documento.Titulo);
            var textoEmail = template.Replace("#linkAcesso#", GerarLinkEdicaoDocumento(documento));


            var notificacao = new Notificacao(textoEmail,
                DateTime.Now,
                DateTime.Now,
                (int)Funcionalidades.ControlDoc,
                documento.IdProcesso,
                1,
                documento.IdSite,
                NuMeroDiasDispatoComAntecedencia,
                "E",
                documento.IdElaborador);

            var usuario = _usuarioAppServico.GetById(documento.IdElaborador);
            _notificacaoServico.Add(notificacao);
            
            EnviaEmailSemTemplate(notificacao, usuario);
        }

        private void EnviaEmailSemTemplate(Notificacao notificacao, Usuario usuario)
        {
            Email _email = new Email();

            _email.Assunto = Traducao.ResourceNotificacaoMensagem.msgNotificacaoControlDoc;
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = usuario.CdIdentificacao;
            _email.Conteudo = notificacao.Descricao;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }


        private void EnviaEmail(Notificacao notificacao)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NotificacaoDocumento-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            notificacao.Descricao = conteudo.Replace("#Corpo#", notificacao.Descricao);

            this.EnviaEmailSemTemplate(notificacao, notificacao.Usuario);
        }

        public void NotificacaoColaboradores(decimal NuDocumento, List<Usuario> usuarios, int idSite, int? idProcesso)
        {


            foreach (var usuario in usuarios)
            {
                var notificacao = new Notificacao("Você está recebendo uma notificação do sistema de gestão e-Qualit," +
                    "existe uma não conformidade no sistema a qual necessita sua intervenção.<br>" +
                    "Por favor, acesse seu sistema de gestão, no menu – Registros – módulo – Controlador Documento," +
                    "vá ao registro número " + NuDocumento + ".",
                    DateTime.Now,
                    DateTime.Now,
                    (int)Funcionalidades.ControlDoc,
                    idProcesso,
                    1,
                    idSite,
                    5,
                    "L",
                    usuario.IdUsuario);

                _notificacaoServico.Add(notificacao);
                var usuarioEmail = _usuarioAppServico.GetById(usuario.IdUsuario);
                notificacao.Usuario = usuarioEmail;
                EnviaEmail(notificacao);
            }


        }

        public void NotificacaoLicenca()
        {
            var notificacao = new Notificacao("Licenca", DateTime.Now, DateTime.Now, 1, 1, 1, 1, 5, "Teste", 1);

            _notificacaoServico.Add(notificacao);
        }

        public void EnviarDocumentoParaAprovacao(DocDocumento documentoAprovacao)
        {
            documentoAprovacao.FlStatus = (int)StatusDocumento.Aprovacao;
            _documentoRepositorio.Update(documentoAprovacao);
        }

        public void EnviarDocumentoParaAprovado(DocDocumento documentoAprovacao)
        {
            documentoAprovacao.FlStatus = (int)StatusDocumento.Aprovado;
            _documentoRepositorio.Update(documentoAprovacao);
        }

        public void AprovarDocumento(DocDocumento documentoAprovado)
        {
            documentoAprovado.FlStatus = (int)StatusDocumento.Aprovado;
            _documentoRepositorio.Update(documentoAprovado);
        }

        public void AprovarDocumentoPorUsuario(DocDocumento documento, int idUsuarioLogado)
        {

            var usuarioValido = documento.DocUsuarioVerificaAprova.FirstOrDefault(x => x.IdUsuario == idUsuarioLogado && x.TpEtapa == _statusAprovacao);
            if (usuarioValido != null)
            {
                documento.DocUsuarioVerificaAprova.FirstOrDefault(x => x.IdUsuario == idUsuarioLogado && x.TpEtapa == _statusAprovacao).FlAprovou = true;
            }
            if (idUsuarioLogado == 1)
            {
                if (documento.DocUsuarioVerificaAprova.FirstOrDefault() != null)
                {
                    documento.DocUsuarioVerificaAprova.FirstOrDefault().FlAprovou = true;
                }
            }

            if (documento.IdDocumentoPai != null)
            {
                var documentoPai = _documentoRepositorio.Get(x => x.IdDocumento == documento.IdDocumentoPai).First();

                if (documentoPai != null)
                {
                    AtualizaParaObsoleto(documentoPai);
                }
            }

            _documentoRepositorio.Update(documento);
        }



        public DocDocumento ObterMaiorRevisao(int? idProcesso = null)
        {
            var documentos = _documentoRepositorio.Get(x => x.IdProcesso == idProcesso);

            return documentos.Where(x => x.NuRevisao == documentos.Max(y => y.NuRevisao)).SingleOrDefault();
        }

        public DocDocumento DocumentoPerfilEtapaIdDocumento(int idUsuario, int etapaDocumento, int idSite, int idPerfilUsuario, int idDocumento)
        {
            if (EPerfilColaborador(idPerfilUsuario))
            {
                return RegrasDocumentoColaboradorEtapa(idUsuario, etapaDocumento, idSite, idDocumento);
            }
            else if (EPerfilAdiministrativo(idPerfilUsuario))
            {
                return RegrasDocumentoAdministrativoEtapa(etapaDocumento, idSite, idDocumento);
            }

            return null;
        }

        public IEnumerable<DocDocumento> ListaDocumentosStatusProcessoSite(int idSite, int flStatus, int? idProcesso = null)
        {
            var lista = _documentoRepositorio.Get(x => x.IdSite == idSite &&
                                                    x.FlStatus == flStatus &&
                                                    (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null));

            return lista;
        }


        public IEnumerable<DocDocumento> ListaTodosDocumentosProcessoSite(int idSite, int? idProcesso = null)
        {
            var lista = _documentoRepositorio.Get(x => x.IdSite == idSite &&
                                           (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null));
            return lista;
        }



        public IEnumerable<DocDocumento> ListaDocumentosVerificacaoPorVerificador(int site, int usuarioVerificador, int? idProcesso = null)
        {
            var listaVerificadores = _documentoRepositorio.Get(x => (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Verificacao);

            listaVerificadores = listaVerificadores.Where(x => x.DocUsuarioVerificaAprova.Any(y => y.IdUsuario == usuarioVerificador && y.TpEtapa == _statusVerificacao));

            return listaVerificadores;
        }

        public IEnumerable<DocDocumento> ListaDocumentosEmVerificacaoPorProcessoESite(int site, int? idProcesso = null)
        {
            var listaDocumentoEmVerificacao = _documentoRepositorio.Get(x => (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Verificacao);
            return listaDocumentoEmVerificacao;
        }

        public IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorAprovador(int site, int usuarioAprovador, int? idProcesso = null)
        {
            var listaDocumentosAprovacao = _documentoRepositorio.Get(x => (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Aprovacao);

            listaDocumentosAprovacao = listaDocumentosAprovacao.Where(x => x.DocUsuarioVerificaAprova.Any(y => y.Usuario.IdUsuario == usuarioAprovador)).ToList();

            return listaDocumentosAprovacao;
        }

        public IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorProcessoESite(int site, int? idProcesso = null)
        {
            var listaDocumentoEmVerificacao = _documentoRepositorio.Get(x => (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Aprovacao);
            return listaDocumentoEmVerificacao;
        }

        public IEnumerable<DocDocumento> ListaDocumentosAprovadosMaiorRevisao(int site, int? idProcesso = null)
        {

            var documentos = _documentoRepositorio.Get(x => x.IdSite == site && (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null) && x.FlStatus == (int)StatusDocumento.Aprovado).Select(x => x.IdDocumento);

            var listaDocumentos = new List<DocDocumento>();

            foreach (var doc in documentos)
                listaDocumentos.Add(ObterDocumentoComMaiorRevisao(doc));

            return listaDocumentos;
        }

        public IEnumerable<DocDocumento> ListaDocumentosObsoletosMaiorRevisao(int site, int? idProcesso = null)
        {

            var documentos = _documentoRepositorio.Get(x => x.IdSite == site && (x.IdProcesso == idProcesso || x.IdProcesso == null || idProcesso == null) && x.FlStatus == (int)StatusDocumento.Obsoleto).Select(x => x.IdDocumento);

            var listaDocumentos = new List<DocDocumento>();

            foreach (var doc in documentos)
                listaDocumentos.Add(ObterDocumentoComMaiorRevisao(doc));

            return listaDocumentos;
        }

        public IEnumerable<DocDocumento> ListaDocumentosPorTemplateSiteEProcesso(int idsite, string template, int? idProcesso = null)
        {
            var listaDocumentosComFluxo = _documentoRepositorio.Get(x => (x.IdProcesso == idProcesso) &&
                                                    x.IdSite == idsite && x.FlStatus == (int)StatusDocumento.Aprovado);
            return listaDocumentosComFluxo.Where(x => x.DocTemplate.Any(y => y.TpTemplate == template));
        }


        private IEnumerable<DocDocumento> RegrasListaDocumentoAdministrativoEtapa(int etapaDocumento, int idSite)
        {
            if (etapaDocumento == (int)StatusDocumento.Elaboracao)
                return _documentoRepositorio.ListaDocumentosEtapa(idSite, etapaDocumento);

            if (etapaDocumento == (int)StatusDocumento.Verificacao)
                return _documentoRepositorio.ListaDocumentosEtapa(idSite, etapaDocumento);

            if (etapaDocumento == (int)StatusDocumento.Aprovacao)
                return _documentoRepositorio.ListaDocumentosEtapa(idSite, etapaDocumento);

            if (etapaDocumento == (int)StatusDocumento.Aprovado)
                return _documentoRepositorio.ListaDocumentosEtapa(idSite, etapaDocumento);

            return null;
        }
        private void AtualizaParaObsoleto(DocDocumento documento)
        {
            documento.FlStatus = (int)StatusDocumento.Obsoleto;
            _documentoRepositorio.Update(documento);
        }

        private void SalvarDocumentoColaborador(DocDocumento documento, int idUsuarioLogado)
        {
            documento.IdElaborador = idUsuarioLogado;
            _documentoRepositorio.Add(documento);
        }


        private bool PossuiRevisao(byte? revisao)
        {
            if (revisao == null)
            {
                return false;
            }
            else if (revisao == 0)
            {
                return false;
            }

            return true;
        }

        private bool EPerfilColaborador(int idPerfil)
        {
            if (idPerfil == (int)PerfisAcesso.Colaborador)
                return true;

            return false;
        }

        private bool EPerfilAdiministrativo(int idPerfil)
        {
            if (idPerfil == (int)PerfisAcesso.Administrador ||
                idPerfil == (int)PerfisAcesso.Coordenador ||
                idPerfil == (int)PerfisAcesso.Suporte)
                return true;

            return false;
        }

        private bool DocumentoEstaEmAprovacao(int status)
        {
            if (status == (int)StatusDocumento.Aprovacao)
                return true;

            return false;
        }

        private bool DocumentoEstaEmElaboracao(int status)
        {
            if (status == (int)StatusDocumento.Elaboracao)
                return true;

            return false;
        }

        private bool DocumentoEstaEmVerificacao(int flStatus)
        {
            if (flStatus == (int)StatusDocumento.Verificacao)
                return true;

            return false;
        }

        private bool UsuarioE_Elaborador_Documento(int idElaborador, int idUsuario)
        {
            if (idElaborador == idUsuario)
                return true;

            return false;
        }

        private bool DocumentoFoiAprovadoPortodos(List<DocUsuarioVerificaAprova> aprovados)
        {
            foreach (var aprovador in aprovados)
                if (aprovador.FlAprovou == false || aprovador.FlAprovou == null)
                    return false;

            return true;
        }

        private bool DocumentoFoiVerificadoPortodos(List<DocUsuarioVerificaAprova> verificadores)
        {
            foreach (var verificador in verificadores.Where(x => x.TpEtapa == _statusVerificacao))
                if (verificador.FlVerificou != true || verificador.FlVerificou == null)
                    return false;

            return true;
        }

        private bool UsuarioEAprovadorDocumento(List<DocUsuarioVerificaAprova> aprovadores, int idUsuario)
        {
            int usuarioAprova = aprovadores.Where(s => s.IdUsuario == idUsuario
                                            && s.TpEtapa == _statusAprovacao).Count();

            if (usuarioAprova > 0)
                return true;

            return false;
        }

        private DocDocumento ObterDocumentoComMaiorRevisao(int documento)
        {
            return _documentoRepositorio.Get(x => x.IdDocumento == documento).Select(x => { x.NuRevisao = Convert.ToByte(x.Assuntos.Max(y => y.Revisao)); return x; }).FirstOrDefault();


        }

        private DocDocumento NovaVersao(DocDocumento old)
        {
            DocDocumento novo = old;

            novo.IdDocumento = 0;
            novo.IdDocumentoPai = old.IdDocumento;
            novo.NuRevisao = Convert.ToByte(old.NuRevisao + 1);
            novo.DocUsuarioVerificaAprova.ForEach(f => f.FlAprovou = false && f.TpEtapa == _statusVerificacao);
            novo.FlStatus = (int)StatusDocumento.Elaboracao;

            return novo;
        }

        private DocDocumento RegrasDocumentoColaboradorEtapa(int idUsuario, int etapaDocumento, int idSite, int idDocumento)
        {
            if (etapaDocumento == (int)StatusDocumento.Elaboracao)
                return _documentoRepositorio.DocumentoElaboracaoColaborador(idSite, idUsuario, idDocumento);

            if (etapaDocumento == (int)StatusDocumento.Verificacao)
                return _documentoRepositorio.DocumentoVerificacaoColaborador(idSite, idUsuario, _statusVerificacao, idDocumento);

            if (etapaDocumento == (int)StatusDocumento.Aprovacao)
                return _documentoRepositorio.DocumentoAprovacaoColaborador(idSite, idUsuario, _statusAprovacao, idDocumento);

            if (etapaDocumento == (int)StatusDocumento.Aprovado)
                return _documentoRepositorio.DocumentoEtapa(idSite, etapaDocumento, idDocumento);

            return null;
        }

        private DocDocumento RegrasDocumentoAdministrativoEtapa(int etapaDocumento, int idSite, int idDocumento)
        {
            if (etapaDocumento == (int)StatusDocumento.Elaboracao)
                return _documentoRepositorio.DocumentoEtapa(idSite, etapaDocumento, idDocumento);

            if (etapaDocumento == (int)StatusDocumento.Verificacao)
                return _documentoRepositorio.DocumentoEtapa(idSite, etapaDocumento, idDocumento);

            if (etapaDocumento == (int)StatusDocumento.Aprovacao)
                return _documentoRepositorio.DocumentoEtapa(idSite, etapaDocumento, idDocumento);

            if (etapaDocumento == (int)StatusDocumento.Aprovado)
                return _documentoRepositorio.DocumentoEtapa(idSite, etapaDocumento, idDocumento);

            return null;
        }


    }
}
