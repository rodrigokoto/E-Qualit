using Dominio.Entidade;

namespace Dominio.Validacao.DocDocumentos.View
{
    public class CamposObrigatoriosValidation : ValidaCamposDocDocumento<DocDocumento>
    {
        public CamposObrigatoriosValidation()
        {
            SiteObrigatorio();
            DocumentoIndentificadorObrigatorio();
            CategoriaObrigtoria();
            ElaboradorObrigatorio();
            TemWorkFlowObrigatorio();
            TemRevisaoPeriodica();
            StatusObrigatorio();
            DataAlteracaoObrigatorio();
            TempleteObrigatorio();
            SiglaObrigatorio();
            NumeroDocumentoObrigatorio();
            TituloObrigatorio();
            UsuarioInclusorObrigatorio();
            VerificadoresObrigatorio();
            AprovadoresObrigatorio();
            DataNotificacaoObrigatoria();
            ValidaDocTemplate();
            ValidaQuantidadeCaracteresCampos();
        }
    }
}
