using Dominio.Entidade;

namespace Dominio.Validacao.Usuarios.LoginView
{
    public class AlterarSenhaViewValidation : ValidaCamposUsuario<Usuario>
    {
        public AlterarSenhaViewValidation()
        {
            IdUsuarioNull();
            SenhaDeveSerIgualAConfirmaSenha();
        }
    }
}
