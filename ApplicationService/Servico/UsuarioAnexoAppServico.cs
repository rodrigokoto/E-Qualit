using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class UsuarioAnexoAppServico : BaseServico<UsuarioAnexo>, IUsuarioAnexoAppServico
    {
        private readonly IUsuarioAnexoRepositorio _usuarioAnexoRepositorio;
        public UsuarioAnexoAppServico(IUsuarioAnexoRepositorio usuarioAnexoRepositorio) : base(usuarioAnexoRepositorio)
        {
            _usuarioAnexoRepositorio = usuarioAnexoRepositorio;
        }
    }
}
