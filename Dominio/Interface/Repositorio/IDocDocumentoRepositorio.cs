using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IDocDocumentoRepositorio : IBaseRepositorio<DocDocumento>
    {
        IEnumerable<DocDocumento> ListaDocumentosEtapa(int idSite, int etapaDocumento);
        IEnumerable<DocDocumento> ListaDocumentosSite(int idSite);

        IEnumerable<DocDocumento> ListaDocumentosElaboracaoColaborador(int idSite, int idUsuario);
        IEnumerable<DocDocumento> ListaDocumentosVerificacaoColaborador(int idSite, int idUsuario, string tpEtapa);
        IEnumerable<DocDocumento> ListaDocumentosAprovacaoColaborador(int idSite, int idUsuario, string tpEtapa);

        DocDocumento DocumentoElaboracaoColaborador(int idSite, int idUsuario, int idDocumento);
        DocDocumento DocumentoVerificacaoColaborador(int idSite, int idUsuario, string tpEtapa, int idDocumento);
        DocDocumento DocumentoAprovacaoColaborador(int idSite, int idUsuario, string tpEtapa, int idDocumento);
        DocDocumento DocumentoEtapa(int idSite, int etapaDocumento, int idDocumento);

        IEnumerable<DocDocumento> ListaDocumentosStatusProcessoSite(int idSite, int flStatus, int idProcesso);
        IEnumerable<DocDocumento> ListarAssuntosDoDocumentoERevisoes(int idDocumentoPai);
        void RemoverGenerico(object obj);
        void RemoverDocumento(DocDocumento obj);
        void SalvarDocumento(DocDocumento doc);
    }                          
}
