using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.DocDocumentos;
using Dominio.Validacao.DocDocumentos.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class DocDocumentoServico : IDocDocumentoServico
    {
        private readonly IDocDocumentoRepositorio _documentoRepositorio;
        private readonly IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprovaRepositorio;
        private readonly IDocTemplateRepositorio _docTemplateRepositorio;
        private readonly IDocCargoRepositorio _docCargoRepositorio;
        private readonly INotificacaoServico _notificacaoServico;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly int NuMeroDiasDispatoComAntecedencia = 5;

        private string _statusVerificacao = "V";
        private string _statusAprovacao = "A";
        private string _categoriaCombo = "CATDOC";

        public DocDocumentoServico(IDocDocumentoRepositorio documentoRepositorio,
                                   IDocUsuarioVerificaAprovaRepositorio docUsuarioVerificaAprovaRepositorio,
                                   IDocTemplateRepositorio docTemplate,
                                   IDocCargoRepositorio docCargo,
                                   INotificacaoServico notificacaoServico)
        {
            _documentoRepositorio = documentoRepositorio;
            _docUsuarioVerificaAprovaRepositorio = docUsuarioVerificaAprovaRepositorio;
            _docTemplateRepositorio = docTemplate;
            _docCargoRepositorio = docCargo;
            _notificacaoServico = notificacaoServico;
        }

        public IEnumerable<DocDocumento> ListaDocumentosStatusProcessoSite(int idSite, int flStatus, int idProcesso)
        {
            var lista = _documentoRepositorio.Get(x => x.IdSite == idSite &&
                                                    x.FlStatus == flStatus &&
                                                    (x.IdProcesso == idProcesso || x.IdProcesso == null));



            return lista;
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

        public void SalvarDocumento(DocDocumento documento)
        {
            _documentoRepositorio.Add(documento);
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

        public void VerificarDocumentoPorUsuario(DocDocumento documento, int idUsuarioLogado)
        {
            documento.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuarioLogado && x.TpEtapa == _statusVerificacao).FirstOrDefault().FlVerificou = true;

            _documentoRepositorio.Update(documento);

            if (VerificadoPorTodos(documento))
                documento.FlStatus = (int)StatusDocumento.Aprovacao;

            _documentoRepositorio.Update(documento);
        }

        public bool VerificadoPorTodos(DocDocumento documento)
        {
            if (DocumentoFoiVerificadoPortodos(documento.DocUsuarioVerificaAprova))
                return true;

            return false;
        }

        public bool AprovadoPorTodos(DocDocumento documento)
        {
            if (DocumentoFoiAprovadoPortodos(documento.DocUsuarioVerificaAprova))
                return true;

            return false;
        }

        public void AprovarDocumento(DocDocumento documento, int idUsuarioLogado)
        {
            documento.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuarioLogado && x.TpEtapa == _statusAprovacao).FirstOrDefault().FlAprovou = true;

            if (AprovadoPorTodos(documento))
                documento.FlStatus = (int)StatusDocumento.Aprovado;

            _documentoRepositorio.Update(documento);
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

        public void EnviarDocumentoParaElaboracao(DocDocumento doc)
        {
            doc.FlStatus = (int)StatusDocumento.Elaboracao;

            _documentoRepositorio.Update(doc);
        }

        public DocDocumento CriarRevisaoDocumento(int idDocumentoAtual, int idUsuarioLogado)
        {
            var documentoPai = _documentoRepositorio.GetById(idDocumentoAtual);

            //documentoPai.DocTemplate.ForEach(x => x.IdDocumento = 0);
            //documentoPai.FlStatus = (int)StatusDocumento.Obsoleto;
            //_documentoRepositorio.Update(documentoPai);

            return CriarCloneRevisadoComMaisUm(documentoPai);
        }

        private DocDocumento CriarCloneRevisadoComMaisUm(DocDocumento documentoAtual)
        {
            //documentoAtual.DocCargo.ForEach(x => x.IdDocumento = 0);
            //documentoAtual.DocUsuarioVerificaAprova.ForEach(x => {
            //    x.IdDocumento = 0;
            //    x.DocDocumento = null;
            //});
            //documentoAtual.Assuntos.ForEach(x => x.IdDocumento = 0);
            //documentoAtual.Comentarios.ForEach(x => x.IdDocumento = 0);
            //documentoAtual.Registros.ForEach(x => x.IdDocumento = 0);
            //documentoAtual.Rotinas.ForEach(x => x.IdDocumento = 0);
            //documentoAtual.DocTemplate.ForEach(x => x.IdDocumento = 0);


            var documentoNovo = new DocDocumento
            {
                IdDocumento = 0,

                IdSite = documentoAtual.IdSite,
                IdDocIdentificador = documentoAtual.IdDocIdentificador,
                IdProcesso = documentoAtual.IdProcesso,
                IdCategoria = documentoAtual.IdCategoria,
                Titulo = documentoAtual.Titulo,
                IdSigla = documentoAtual.IdSigla,
                NumeroDocumento = documentoAtual.NumeroDocumento,
                IdElaborador = documentoAtual.IdElaborador,
                DtPedidoVerificacao = documentoAtual.DtPedidoVerificacao,
                DtVerificacao = documentoAtual.DtVerificacao,
                DtVencimento = documentoAtual.DtVencimento,
                DtEmissao = documentoAtual.DtEmissao,
                DtPedidoAprovacao = documentoAtual.DtPedidoAprovacao,
                DtAprovacao = documentoAtual.DtAprovacao,
                DtNotificacao = documentoAtual.DtNotificacao,
                XmlMetadata = documentoAtual.XmlMetadata,
                FlWorkFlow = documentoAtual.FlWorkFlow,
                FlRevisaoPeriodica = documentoAtual.FlRevisaoPeriodica,
                FlStatus = (int)StatusDocumento.Elaboracao,
                IdUsuarioIncluiu = documentoAtual.IdUsuarioIncluiu,
                DtInclusao = documentoAtual.DtInclusao,
                DtAlteracao = documentoAtual.DtAlteracao,
                Ativo = documentoAtual.Ativo,
                CorRisco = documentoAtual.CorRisco,
                PossuiGestaoRisco = documentoAtual.PossuiGestaoRisco,
                ConteudoDocumento = documentoAtual.ConteudoDocumento,
                IdGestaoDeRisco = documentoAtual.IdGestaoDeRisco,
                IdLicenca = documentoAtual.IdLicenca,
                IdDocExterno = documentoAtual.IdDocExterno,

                TextoDoc = documentoAtual.TextoDoc,
                FluxoDoc = documentoAtual.FluxoDoc,
                RecursoDoc = documentoAtual.RecursoDoc,
            };

            documentoAtual.DocCargo.ForEach(x =>
            {
                DocumentoCargo docCargo = new DocumentoCargo();
                docCargo.IdCargo = x.IdCargo;
                documentoNovo.DocCargo.Add(docCargo);
            });

            documentoAtual.DocUsuarioVerificaAprova.ForEach(x =>
            {
                DocUsuarioVerificaAprova docUsuarioVerificaAprova = new DocUsuarioVerificaAprova();
                docUsuarioVerificaAprova.Ativo = x.Ativo;
                docUsuarioVerificaAprova.FlAprovou = x.FlAprovou;
                docUsuarioVerificaAprova.FlVerificou = x.FlVerificou;
                docUsuarioVerificaAprova.deletar = x.deletar;
                docUsuarioVerificaAprova.IdDocUsuarioVerificaAprova = x.IdDocUsuarioVerificaAprova;
                docUsuarioVerificaAprova.IdUsuario = x.IdUsuario;
                docUsuarioVerificaAprova.TpEtapa = x.TpEtapa;
                documentoNovo.DocUsuarioVerificaAprova.Add(docUsuarioVerificaAprova);
            });

            documentoAtual.Assuntos.ForEach(x =>
            {
                DocumentoAssunto assunto = new DocumentoAssunto();
                assunto.DataAssunto = x.DataAssunto;
                assunto.Descricao = x.Descricao;
                assunto.Id = x.Id;
                assunto.Revisao = x.Revisao;
                documentoNovo.Assuntos.Add(assunto);
            });

            documentoAtual.Comentarios.ForEach(x =>
            {
                DocumentoComentario docComentario = new DocumentoComentario();
                docComentario.IdUsuario = x.IdUsuario;
                docComentario.DataComentario = x.DataComentario;
                docComentario.Descricao = x.Descricao;
                documentoNovo.Comentarios.Add(docComentario);
            });

            documentoAtual.Registros.ForEach(x =>
            {
                DocRegistro docRegistro = new DocRegistro();
                docRegistro.Armazenar = x.Armazenar;
                docRegistro.Disposicao = x.Disposicao;
                docRegistro.Identificar = x.Identificar;
                docRegistro.Proteger = x.Proteger;
                docRegistro.Recuperar = x.Recuperar;
                docRegistro.Retencao = x.Retencao;
                documentoNovo.Registros.Add(docRegistro);
            });

            documentoAtual.Rotinas.ForEach(x =>
            {
                DocRotina docRotina = new DocRotina();
                docRotina.Como = x.Como;
                docRotina.Item = x.Item;
                docRotina.OQue = x.OQue;
                docRotina.Quem = x.Quem;
                docRotina.Registro = x.Registro;
                documentoNovo.Rotinas.Add(docRotina);
            });

            documentoAtual.DocTemplate.ForEach(x =>
            {
                DocTemplate docTemplate = new DocTemplate();
                docTemplate.Ativo = x.Ativo;
                docTemplate.TpTemplate = x.TpTemplate;
                documentoNovo.DocTemplate.Add(docTemplate);
            });

            var revisao = documentoNovo.Assuntos.Max(x => Convert.ToInt32(x.Revisao));

            documentoAtual.NuRevisao = Convert.ToByte(revisao);

            if (PossuiRevisao(documentoAtual.NuRevisao))
            {
                documentoNovo.NuRevisao = documentoAtual.NuRevisao;
                documentoNovo.NuRevisao += 1;

            }
            else
            {
                documentoNovo.NuRevisao = 1;
            }

            documentoNovo.IdDocumentoPai = documentoAtual.IdDocumento;

            documentoNovo.Assuntos.Add(new DocumentoAssunto { Descricao = "", DataAssunto = DateTime.Now, Revisao = (Convert.ToInt32(documentoNovo.Assuntos.Max(x => Convert.ToInt32(x.Revisao))) + 1).ToString() });

            _documentoRepositorio.Add(documentoNovo);

            return documentoNovo;
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

        public void AtualizaPaiParaObsoleto(DocDocumento documento)
        {
            documento.FlStatus = (int)StatusDocumento.Obsoleto;
            _documentoRepositorio.Update(documento);
        }

        public void Valido(DocDocumento documento, ref List<string> erros)
        {
            var camposObrigatoriosDoc = new CamposObrigatoriosValidation();

            var validacao = camposObrigatoriosDoc.Validate(documento);

            List<string> errosAssuntos = new List<string>();

            documento.Assuntos.ForEach(x =>
            {
                if (x.Descricao != null && x.Descricao.Length > 1000)
                {
                    errosAssuntos.Add(Traducao.Resource.MsgMaxAssuntosDescricao);
                }
            });

            erros.AddRange(errosAssuntos);


            List<string> errosRegistro = new List<string>();

            documento.Registros.ForEach(x =>
            {



                if (x.Identificar.Length > 300)
                {
                    errosRegistro.Add(Traducao.Resource.MsgMaxRegistrosIdentificar);
                }

                if (x.Armazenar.Length > 300)
                {
                    errosRegistro.Add(Traducao.Resource.MsgMaxRegistrosArmazenar);
                }

                if (x.Proteger.Length > 300)
                {
                    errosRegistro.Add(Traducao.Resource.MsgMaxRegistrosProteger);
                }

                if (x.Recuperar.Length > 300)
                {
                    errosRegistro.Add(Traducao.Resource.MsgMaxRegistrosRecuperar);
                }

                if (x.Retencao.Length > 300)
                {
                    errosRegistro.Add(Traducao.Resource.MsgMaxRegistrosRetencao);
                }

                if (x.Disposicao.Length > 300)
                {
                    errosRegistro.Add(Traducao.Resource.MsgMaxRegistrosDisposicao);
                }

            });

            erros.AddRange(errosRegistro);

            List<string> errosRotina = new List<string>();

            documento.Rotinas.ForEach(x =>
            {

                if (x.OQue.Length > 500)
                {
                    errosRotina.Add(Traducao.Resource.MsgMaxRotinasOQue);
                }

                if (x.Quem.Length > 500)
                {
                    errosRotina.Add(Traducao.Resource.MsgMaxRotinasQuem);
                }

                if (x.Registro.Length > 500)
                {
                    errosRotina.Add(Traducao.Resource.MsgMaxRotinasRegistro);
                }

                if (x.Como.Length > 500)
                {
                    errosRotina.Add(Traducao.Resource.MsgMaxRotinasComo);
                }

            });

            erros.AddRange(errosRotina);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }

            var validoParaCadastro = new AptoParaCadastroAtualizarValidation(_docUsuarioVerificaAprovaRepositorio).Validate(documento);
            if (!validoParaCadastro.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validoParaCadastro));
            }
        }

        public void ValidoParaEtapaDeVerificacao(DocDocumento documento, ref List<string> erros)
        {
            var camposEtapaVerificacaoObrigatorios = new CamposObrigatoriosEtapaVerificacaoValidation()
                                    .Validate(documento);

            if (!camposEtapaVerificacaoObrigatorios.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(camposEtapaVerificacaoObrigatorios.Errors));
            }
        }

        public void AssuntoObrigatorioEditarRevisao(DocDocumento documento, ref List<string> erros)
        {
            bool hasNew = false;

            foreach (DocumentoAssunto item in documento.Assuntos)
            {
                var AssuntoObrigatorio = new CampoAssuntoObrigatorioRevisaoValidation()
                                    .Validate(item);

                if (!AssuntoObrigatorio.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(AssuntoObrigatorio.Errors));
                }

                hasNew = item.Id == 0 && !hasNew;
            }

            if(!hasNew)
            {
                erros.Add(Traducao.Resource.DocDocumento_msg_erro_required_DocAssunto);
            }
        }

        public void ValidoParaRevisao(DocDocumento documento, ref List<string> erros)
        {
            var validaResisao = new CamposObrigatoriosRevisaoViewValidation()
                                   .Validate(documento);

            if (!validaResisao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaResisao.Errors));
            }
        }


        private bool DocumentoFoiVerificadoPortodos(List<DocUsuarioVerificaAprova> verificadores)
        {
            foreach (var verificador in verificadores)
                if (verificador.FlVerificou == false)
                    return false;

            return true;
        }

        private bool DocumentoFoiAprovadoPortodos(List<DocUsuarioVerificaAprova> aprovados)
        {
            foreach (var aprovador in aprovados)
                if (aprovador.FlAprovou == false)
                    return false;

            return true;
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

        private bool DocumentoEstaEmVerificacao(int flStatus)
        {
            if (flStatus == (int)StatusDocumento.Verificacao)
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

        private bool UsuarioEAprovadorDocumento(List<DocUsuarioVerificaAprova> doc, int idUsuario)
        {
            int usuarioAprova = doc.Where(s => s.IdUsuario == idUsuario
                                            && s.TpEtapa == _statusAprovacao).Count();

            if (usuarioAprova > 0)
                return true;

            return false;
        }

        private bool UsuarioE_Elaborador_Documento(int idElaborador, int idUsuario)
        {
            if (idElaborador == idUsuario)
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

        private bool EPerfilColaborador(int idPerfil)
        {
            if (idPerfil == (int)PerfisAcesso.Colaborador)
                return true;

            return false;
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

        private void SalvarDocumentoColaborador(DocDocumento documento, int idUsuarioLogado)
        {
            documento.IdElaborador = idUsuarioLogado;
            _documentoRepositorio.Add(documento);
        }

        public DocDocumento ObterMaiorRevisao(int idProcesso)
        {
            var documentos = _documentoRepositorio.Get(x => x.IdProcesso == idProcesso);

            return documentos.Where(x => x.NuRevisao == documentos.Max(y => y.NuRevisao)).SingleOrDefault();
        }

        public void NotificacaoAprovadoresEmail(int idSite, int idProcesso, List<DocUsuarioVerificaAprova> aprovadores)
        {
            foreach (var aprovador in aprovadores)
            {
                var notificacao = new Notificacao("Você está recebendo uma notificação do sistema de gestão e-Qualit," +
                    "existe uma não conformidade no sistema a qual necessita sua intervenção." +
                    "Por favor, acesse seu sistema de gestão, no menu – Registros – módulo – Não Conformidade," +
                    "vá ao registro número 1.",
                    DateTime.Now,
                    DateTime.Now,
                    (int)Funcionalidades.ControlDoc,
                    idProcesso,
                    1,
                    idSite,
                    5,
                    "E", aprovador.IdUsuario);

                _notificacaoRepositorio.Add(notificacao);
            }
        }

        public void NotificacaoVerificadoresEmail(int idSite, int idProcesso, List<DocUsuarioVerificaAprova> aprovadores)
        {
            foreach (var aprovador in aprovadores)
            {
                var notificacao = new Notificacao("Você está recebendo uma notificação do sistema de gestão e-Qualit," +
                    "existe uma não conformidade no sistema a qual necessita sua intervenção." +
                    "Por favor, acesse seu sistema de gestão, no menu – Registros – módulo – Não Conformidade," +
                    "vá ao registro número 1.",
                    DateTime.Now,
                    DateTime.Now,
                    (int)Funcionalidades.ControlDoc,
                    idProcesso,
                    1,
                    idSite,
                    5,
                    "E", aprovador.IdUsuario);

                _notificacaoRepositorio.Add(notificacao);
            }
        }

        public void NotificacaoElaboradorEmail(int idSite, int idProcesso, int idElaborador, DateTime dataVencimento)
        {
            {
                var notificacao = new Notificacao("Você está recebendo uma notificação do sistema de gestão e-Qualit," +
                    "existe uma não conformidade no sistema a qual necessita sua intervenção." +
                    "Por favor, acesse seu sistema de gestão, no menu – Registros – módulo – Não Conformidade," +
                    "vá ao registro número 1.",
                    dataVencimento,
                    dataVencimento,
                    (int)Funcionalidades.ControlDoc,
                    idProcesso,
                    1,
                    idSite,
                    NuMeroDiasDispatoComAntecedencia,
                    "E",
                    idElaborador);

                _notificacaoRepositorio.Add(notificacao);
            }
        }

        public void NotificacaoColaboradores(List<Usuario> usuarios, int? idProcesso, int idSite)
        {
            var possuiProcesso = Convert.ToInt32(idProcesso);

            foreach (var usuario in usuarios)
            {
                var notificacao = new Notificacao("Você está recebendo uma notificação do sistema de gestão e-Qualit," +
                    "existe uma não conformidade no sistema a qual necessita sua intervenção." +
                    "Por favor, acesse seu sistema de gestão, no menu – Registros – módulo – Controlador Documento," +
                    "vá ao registro número 1.",
                    DateTime.Now,
                    DateTime.Now,
                    (int)Funcionalidades.ControlDoc,
                    possuiProcesso,
                    1,
                    idSite,
                    5,
                    "L",
                    usuario.IdUsuario);

                _notificacaoRepositorio.Add(notificacao);
            }


        }

        public void NotificacaoLicenca()
        {
            var notificacao = new Notificacao("Licenca", DateTime.Now, DateTime.Now, 1, 1, 1, 1, 5, "Teste", 1);

            _notificacaoRepositorio.Add(notificacao);
        }

        public IEnumerable<DocDocumento> ListaDocumentosVerificacaoPorVerificador(int processo, int site, int usuarioVerificador)
        {
            var listaVerificadores = _documentoRepositorio.Get(x => (x.IdProcesso == processo || x.IdProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Verificacao);
            listaVerificadores = listaVerificadores.Where(x => x.DocUsuarioVerificaAprova.Any(y => y.IdUsuario == usuarioVerificador && y.TpEtapa == _statusVerificacao));

            return listaVerificadores;
        }

        public IEnumerable<DocDocumento> ListaDocumentosEmVerificacaoPorProcessoESite(int processo, int site)
        {
            var listaDocumentoEmVerificacao = _documentoRepositorio.Get(x => (x.IdProcesso == processo || x.IdProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Verificacao);
            return listaDocumentoEmVerificacao;
        }

        public IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorAprovador(int processo, int site, int usuarioAprovador)
        {
            var listaDocumentosAprovacao = _documentoRepositorio.Get(x => (x.IdProcesso == processo || x.IdProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Aprovacao);

            listaDocumentosAprovacao = listaDocumentosAprovacao.Where(x => x.DocUsuarioVerificaAprova.Any(y => y.IdUsuario == usuarioAprovador && y.TpEtapa == _statusAprovacao));

            return listaDocumentosAprovacao;
        }

        public IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorProcessoESite(int processo, int site)
        {
            var listaDocumentoEmVerificacao = _documentoRepositorio.Get(x => (x.IdProcesso == processo || x.IdProcesso == null) &&
                                                    x.IdSite == site &&
                                                    x.FlStatus == (int)StatusDocumento.Aprovacao);
            return listaDocumentoEmVerificacao;
        }

        private DocDocumento ObterDocumentoComMaiorRevisao(int documento)
        {
            var documentos = _documentoRepositorio.Get(x => x.IdDocumento == documento);

            return documentos.Where(x => x.NuRevisao == documentos.Max(y => y.NuRevisao)).SingleOrDefault();
        }

        public IEnumerable<DocDocumento> ListaDocumentosAprovadosMaiorRevisao(int processo, int site)
        {

            var documentos = _documentoRepositorio.Get(x => x.IdSite == site && (x.IdProcesso == processo || x.IdProcesso == null) && x.FlStatus == (int)StatusDocumento.Aprovado).Select(x => x.IdDocumento);

            var listaDocumentos = new List<DocDocumento>();

            foreach (var doc in documentos)
                listaDocumentos.Add(ObterDocumentoComMaiorRevisao(doc));

            return listaDocumentos;
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
            documento.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == idUsuarioLogado && x.TpEtapa == _statusAprovacao).FirstOrDefault().FlAprovou = true;
            _documentoRepositorio.Update(documento);
        }

        public IEnumerable<DocDocumento> ListaDocumentosPorTemplateSiteEProcesso(int idsite, int idprocesso, string template)
        {
            var listaDocumentosComFluxo = _documentoRepositorio.Get(x => (x.IdProcesso == idprocesso) &&
                                                    x.IdSite == idsite && x.FlStatus == (int)StatusDocumento.Aprovado);
            return listaDocumentosComFluxo.Where(x => x.DocTemplate.Any(y => y.TpTemplate == template));
        }


        public decimal GeraProximoNumeroRegistro(int idSite, int? idProcesso = null, int? idSigla = null)
        {
            decimal saida = 0;

            var item = _documentoRepositorio.GetAll()
                .Where(x => x.IdSite == idSite &&
                        (x.IdProcesso == idProcesso || idProcesso == null) &&
                        (x.IdSigla == idSigla || idSigla == null)
                )
                .OrderByDescending(x => x.NumeroDocumento).FirstOrDefault();
            if (item != null)
            {
                if (item.NumeroDocumento != 0)
                {
                    saida = item.NumeroDocumento + 1;
                }
                else
                { saida = 1; }
            }
            else
            {
                saida = 1;
            }

            return saida;
        }



    }
}
