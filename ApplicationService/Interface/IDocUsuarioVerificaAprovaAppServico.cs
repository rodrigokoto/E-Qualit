using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IDocUsuarioVerificaAprovaAppServico : IBaseServico<DocUsuarioVerificaAprova>
    {
        void AlterarUsuariosDoDocumento(int idDocumento, List<DocUsuarioVerificaAprova> lista);
        void AtualizarParaEstadoInicialDoDocumento(List<DocUsuarioVerificaAprova> lista);
    }
}
