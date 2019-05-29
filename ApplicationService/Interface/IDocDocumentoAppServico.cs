using Dominio.Entidade;
using System;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IDocDocumentoAppServico : IBaseServico<DocDocumento>
    {
        void RemoverGenerico(object obj);
        void SalvarDocumento(DocDocumento documento);
        void AprovarDocumento(DocDocumento documentoAprovado);
        // Atualizar Etapa Por Usuario
        void AprovarDocumento(DocDocumento documento, int idUsuarioLogado);
        void AprovarDocumentoPorUsuario(DocDocumento documento, int idUsuarioLogado);
        void VerificarDocumentoPorUsuario(DocDocumento documento, int idUsuarioLogado);

        void NotificacaoColaboradores(decimal NuDocumento, List<Usuario> usuarios, int idSite, int? idProcesso = null);

        void NotificacaoAprovadoresEmail(DocDocumento documento, int idSite, List<DocUsuarioVerificaAprova> aprovadores, int? IdPocesso = null);
        void NotificacaoVerificadoresEmail(DocDocumento documento, int idSite, List<DocUsuarioVerificaAprova> verificadores, int? IdPocesso = null);
        void NotificacaoElaboradorEmail(DocDocumento documento);

        // Etapas do documento
        void EnviarDocumentoParaAprovacao(DocDocumento documentoAprovacao);
        void EnviarDocumentoParaAprovado(DocDocumento documentoAprovacao);
        void EnviarDocumentoParaElaboracao(DocDocumento doc);

        bool VerificadoPorTodos(List<DocUsuarioVerificaAprova> verificadores);
        bool AprovadoPorTodos(List<DocUsuarioVerificaAprova> aprovadores);

        DocDocumento DocumentoPerfilEtapaIdDocumento(int idUsuario, int etapaDocumento, int idSite, int idPerfilUsuario, int idDocumento);
        DocDocumento ObterMaiorRevisao(int ? IdPocesso = null);

        // Listas Por Template
        IEnumerable<DocDocumento> ListaDocumentosPorTemplateSiteEProcesso(int idsite, string template, int? IdPocesso = null);

        //Listas Por Etapa
        IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorAprovador(int site, int usuarioAprovador, int ? processo = null);

        IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorProcessoESite(int site, int? idProcesso = null);

        IEnumerable<DocDocumento> ListaDocumentosVerificacaoPorVerificador(int site, int usuarioVerificador, int? idProcesso = null);

        IEnumerable<DocDocumento> ListaDocumentosEmVerificacaoPorProcessoESite(int site, int? idProcesso = null);

        IEnumerable<DocDocumento> ListaDocumentosStatusProcessoSite(int idSite, int flStatus, int ? IdPocesso = null);

        IEnumerable<DocDocumento> ListaDocumentosAprovadosMaiorRevisao(int site, int? idProcesso = null);

        IEnumerable<DocDocumento> ListaDocumentosObsoletosMaiorRevisao(int site, int? idProcesso = null);

        IEnumerable<DocDocumento> ListaTodosDocumentosProcessoSite(int idSite, int ? IdPocesso = null);

    }

}
