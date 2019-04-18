using Dominio.Entidade;

namespace Dominio.Validacao.Usuarios.MeusDadosView
{
    public class EditarUsuarioMeusDadosViewValidation: ValidaCamposUsuario<Usuario>
    {
        public EditarUsuarioMeusDadosViewValidation()
        {
            IdUsuarioNull();
            DeveTerNomeValido();
            DeveSerEmailValido();
            DeveTerCPFValido();
            DeveTerPerfilValido();
        }
    }
}
