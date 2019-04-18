using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface IProdutoRepositorio : IBaseRepositorio<Produto>
    {
        bool Excluir(int idProduto);
    }
}
