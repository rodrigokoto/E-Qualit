using Dominio.Entidade;

namespace Dominio.Validacao.Processos.View
{
    public class EditarProcessoViewValidation : ValidaCamposProcesso<Processo>
    {
        public EditarProcessoViewValidation()
        {
            NomeObrigatorio();
            SiteObrigatorio();
            IdProcessoObrigatorio();
        }
    }
}
