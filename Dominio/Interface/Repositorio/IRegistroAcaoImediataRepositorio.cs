using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface IRegistroAcaoImediataRepositorio : IBaseRepositorio<RegistroAcaoImediata>
    {
        void AtualizaAcaoImediataComAnexos2(RegistroAcaoImediata acaoImediata, ArquivoDeEvidenciaAcaoImediata arquivoAcaoImediata);
    }
}
