using Dominio.Entidade;

namespace Dominio.Validacao.Processos.View
{
    public class CriarProcessoViewValidation : ValidaCamposProcesso<Processo>
    {
        public CriarProcessoViewValidation()
        {
            NomeObrigatorio();
            SiteObrigatorio();
        }
    }
}
