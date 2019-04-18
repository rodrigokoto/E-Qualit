using Dominio.Entidade;

namespace Dominio.Validacao.Usuarios.View
{
    public class EditarUsuarioViewValidation : ValidaCamposUsuario<Usuario>
    {
        public EditarUsuarioViewValidation()
        {
            IdUsuarioNull();
            DeveTerNomeValido();
            DeveSerEmailValido();
            DeveTerCPFValido();
            DeveTerPerfilValido();
            DevePossuirRelacionamentoComUsuarioClienteSite();
            DevePossuirCargo();
            ValidaLogo();
        }
    }
}
