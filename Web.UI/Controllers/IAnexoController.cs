using System.Web.Mvc;

namespace Web.UI.Controllers
{
    public interface IAnexoController
    {
        void BackuAuditoria();
        ActionResult BackupAnexo(int idCliente, int Modulo, int IDDoumento);
        void BackupDocdocumento();
        void BackupInstrumento();
        void BackupLicencas();
        void BackupRegistros(string TpRegistro);
    }
}