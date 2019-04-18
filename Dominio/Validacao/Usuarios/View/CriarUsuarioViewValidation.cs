using Dominio.Entidade;

namespace Dominio.Validacao.Usuarios.View
{
    public class CriarUsuarioViewValidation : ValidaCamposUsuario<Usuario>
    {
        public CriarUsuarioViewValidation()
        {
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
