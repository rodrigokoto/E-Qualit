using Dominio.Entidade;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class CriacaoNCViewValidation : ValidaCamposNC<RegistroConformidade>
    {
        public CriacaoNCViewValidation()
        {
            DescricaoRegistroObrigatoria();            
            EmissorObrigatorio();
            UsuarioQueIncluiuObrigatorio();
            ProcessoObrigatorio();
            SiteObrigatorio();
            TipoNaoConformidadeObrigatorio();
            ResponsavelPorDefinirAcaoImediata();
            ENaoConformidadeAuditoriaObrigatorio();
            TipoRegistro();
        }

    }
}
