using Dominio.Entidade;

namespace Dominio.Validacao.Usuarios.LoginView
{
    public class EsqueciaSenhaViewValidation: ValidaCamposUsuario<Usuario>
    {
        public EsqueciaSenhaViewValidation()
        {
            DeveSerEmailValido();
        }
    }
}
