using Dominio.Entidade;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class NCEProcedenteFalseViewValidation : ValidaCamposNC<RegistroConformidade>
    {
        public NCEProcedenteFalseViewValidation()
        {
            EProcedenteObrigatorioEDeveSerFalso();
            JustificatiVaObrigatorio();
            AEtapaDeveSerEncerrada();
        }
    }
}
