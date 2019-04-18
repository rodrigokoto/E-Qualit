using Dominio.Entidade;

namespace Dominio.Validacao.Instrumentos.View
{
    public class EditarInstrumentoViewValidation : ValidaCampoInstrumento<Instrumento>
    {
        public EditarInstrumentoViewValidation()
        {
            CriterioAceitacaoObrigatorio();            
            DataCriacaoObrigatorio();
            EquipamentoObrigatorio();
            EscalaObrigatorio();
            IdUsuarioObrigatorio();
            LocalDeUsoObrigatorio();
            MenorDivisaoObrigatorio();
            ModeloObrigatorio();
            NumeroObrigatorio();
            ResponsavelObrigatorio();

        }
    }
}
