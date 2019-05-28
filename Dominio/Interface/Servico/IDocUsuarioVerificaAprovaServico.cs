using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IDocUsuarioVerificaAprovaServico : IBaseRepositorio<DocUsuarioVerificaAprova>
    {
        void AlterarUsuariosDoDocumento(int idDocumento, List<DocUsuarioVerificaAprova> lista);
        void AtualizarParaEstadoInicialDoDocumento(List<DocUsuarioVerificaAprova> lista);
        void RemoveAllById(int id);
    }
}
