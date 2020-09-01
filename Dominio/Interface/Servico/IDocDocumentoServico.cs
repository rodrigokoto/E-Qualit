using Dominio.Entidade;
using System;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IDocDocumentoServico
    {
        void Valido(DocDocumento documento, ref List<string> erros);
        void ValidoParaEtapaDeVerificacao(DocDocumento documento, ref List<string> erros);
        void AssuntoObrigatorioEditarRevisao(DocDocumento documento, ref List<string> erros);
        void ValidoParaRevisao(DocDocumento documento, ref List<string> erros);

        DocDocumento DocumentoPerfilEtapaIdDocumento(int idUsuario, int etapaDocumento, int idSite, int idPerfilUsuario, int idDocumento);

        IEnumerable<DocDocumento> ListaDocumentosStatusProcessoSite(int idSite, int flStatus, int idProcesso);

        void SalvarDocumento(DocDocumento documento);

        bool VerificadoPorTodos(DocDocumento documento);
        bool AprovadoPorTodos(DocDocumento documento);
        void VerificarDocumentoPorUsuario(DocDocumento documento, int idUsuarioLogado);
        
        void AprovarDocumento(DocDocumento documento, int idUsuarioLogado);
        DocDocumento CriarRevisaoDocumento(int idDocumentoAtual, int idUsuarioLogado);
        DocDocumento ObterMaiorRevisao(int idProcesso);

        void NotificacaoAprovadoresEmail(int idSite, int idProcesso, List<DocUsuarioVerificaAprova> aprovadores);
        void NotificacaoVerificadoresEmail(int idSite, int idProcesso, List<DocUsuarioVerificaAprova> aprovadores);
        void NotificacaoElaboradorEmail(int idSite, int idProcesso, int idElaborador, DateTime dataVencimento);
        void NotificacaoColaboradores(List<Usuario> usuarios, int? idProcesso, int idSite);

        IEnumerable<DocDocumento> ListaDocumentosVerificacaoPorVerificador(int processo, int site, int usuarioVerificador);

        IEnumerable<DocDocumento> ListaDocumentosAprovadosMaiorRevisao(int processo, int site);
        IEnumerable<DocDocumento> ListaDocumentosEmVerificacaoPorProcessoESite(int processo, int site);

        // Etapas do documento
        void EnviarDocumentoParaAprovacao(DocDocumento documentoAprovacao);
        void EnviarDocumentoParaAprovado(DocDocumento documentoAprovacao);
        void EnviarDocumentoParaElaboracao(DocDocumento doc);

        // Atualizar Etapa Por Usuario
        void AprovarDocumento(DocDocumento documentoAprovado);
        void AprovarDocumentoPorUsuario(DocDocumento documento, int idUsuarioLogado);

        //Listas Por Etapa
        IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorAprovador(int processo, int site, int usuarioAprovador);
        IEnumerable<DocDocumento> ListaDocumentosEmAprovacaoPorProcessoESite(int processo, int site);

        // Listas Por Template
        IEnumerable<DocDocumento> ListaDocumentosPorTemplateSiteEProcesso(int idsite, int idprocesso, string template);

        string GeraProximoNumeroRegistro(int idSite, int? idProcesso = null, int? idSigla = null);

        void AtualizaPaiParaObsoleto(DocDocumento documento);
    }

}
