using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface IAnexoAppServico : IBaseServico<Anexo>
    {
        void BackupAnexo(int idCliente, string pathBackup);
        void BackupRegistros(int idCliente, string pathBackup);
        void BackupDocdocumento(int idCliente, string pathBackup);
        void BackuAuditoria(int idCliente, string pathBackup);
        void BackupLicencas(int idCliente, string pathBackup);
        void BackupInstrumento(int idCliente, string pathBackup);
    }

   

}
