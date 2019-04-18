using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class LoginAppServico : BaseServico<Usuario>, ILoginAppServico
    {
        private readonly IUsuarioRepositorio _loginRepositorio;

        public LoginAppServico(IUsuarioRepositorio loginRepositorio) : base(loginRepositorio)
        {
            _loginRepositorio = loginRepositorio;
        }
    }
}
