using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IUsuarioClienteSiteServico 
    {
        List<UsuarioClienteSite> ListarPorUsuario(int idUsuario);
        List<Usuario> ListarPorEmpresa(int idEmpresa);
        List<UsuarioClienteSite> ObterSitesPorUsuario(int idUsuario);
    }
}
