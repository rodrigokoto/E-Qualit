using Dominio.Entidade;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class NCEProcedenteTrueViewValidation : ValidaCamposNC<RegistroConformidade>
    {
        public NCEProcedenteTrueViewValidation()
        {
            EProcedenteObrigatorioEDeveSerTrue();
            DeveconterAcaoImediata();
            AEtapaDeveSerImplementacao();
            NecessitaAcaoCorretivaObrigatorio();
            DescricaoAnaliseCausaOgrigatoria();
            ResponsavelPorIniciarTratativaAcaoCorretivaObrigatorio();
            ResponsavelReverificadorObrigatorio();
            ECorrecaoObrigatorio();
        }
    }
}
