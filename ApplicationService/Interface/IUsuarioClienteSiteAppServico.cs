using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IUsuarioClienteSiteAppServico : IBaseServico<UsuarioClienteSite>
    {
        List<UsuarioClienteSite> ListarPorUsuario(int idUsuario);
        List<Usuario> ListarPorEmpresa(int idEmpresa);
        List<UsuarioClienteSite> ObterSitesPorUsuario(int idUsuario);
        void EmpresaValidaLogin(Usuario usuario, ref List<string> erros);
    }
}
