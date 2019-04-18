using Dominio.Entidade;

namespace Dominio.Validacao.Instrumentos.View
{
    public class CriarInstrumentoViewValidation : ValidaCampoInstrumento<Instrumento>
    {
        public CriarInstrumentoViewValidation()
        {
            CriterioAceitacaoObrigatorio();
            DataAlteracaoObrigatorio();
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
