using Dominio.Entidade;

namespace Dominio.Validacao.Instrumentos.View
{
    public class CriarLicencaViewValidation : ValidaCampoLicenca<Licenca>
    {
        public CriarLicencaViewValidation()
        {
            TituloObrigatorio();
            ValidarDataEmissao();
            ValidarDataProximaNotificacao();
            ValidarDataVencimento();
            
        }
    }
}
