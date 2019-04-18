using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface IUsuarioCargoAppServico : IBaseServico<UsuarioCargo>
    {
        string ListarFuncaoConcatenadas(int idUsuario);
    }
}
