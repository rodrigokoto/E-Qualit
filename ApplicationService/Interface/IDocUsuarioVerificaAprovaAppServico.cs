using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IDocUsuarioVerificaAprovaAppServico : IBaseServico<DocUsuarioVerificaAprova>
    {
        void AlterarUsuariosDoDocumento(List<DocUsuarioVerificaAprova> lista);
        void AtualizarParaEstadoInicialDoDocumento(List<DocUsuarioVerificaAprova> lista);
    }
}
