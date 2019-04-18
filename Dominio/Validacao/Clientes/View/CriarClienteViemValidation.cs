using Dominio.Entidade;

namespace Dominio.Validacao.Clientes.View
{
    public class CriarClienteViewValidation : ValidaCampos<Cliente>
    {
        public CriarClienteViewValidation()
        {
            LogoObrigatorio();
            NomeEmpresa();
            UrlAcesso();
            // NumeroDiasParaTrocasSenha();
            ClienteDeveEstaAtivo();
            DataValidadeContrato();
            SiteModuloObrigatorio();

            UsuarioObrigatorioNaCriacao();
        }
    }
}
