using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface IUsuarioRepositorio : IBaseRepositorio<Usuario>
    {
        bool Excluir(int id, int idUsuarioMigracao);
        bool AtualizaSenha(Usuario usuario);
    }
}
