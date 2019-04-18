using Dominio.Entidade;

namespace Dominio.Validacao.Clientes.View
{
    public class EdiarClienteViewValidation : ValidaCampos<Cliente>
    {
        public EdiarClienteViewValidation()
        {
            IdClienteObrigatorio();
            LogoObrigatorio();
            DataValidadeContrato();
            NomeEmpresa();
            UrlAcesso();
        }
    }
}
