using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IDocUsuarioVerificaAprovaServico 
    {
        void AlterarUsuariosDoDocumento(int idDocumento, List<DocUsuarioVerificaAprova> lista);
        void AtualizarParaEstadoInicialDoDocumento(List<DocUsuarioVerificaAprova> lista);
    }
}
