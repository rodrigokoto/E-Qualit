namespace Web.UI.Backup
{
    public interface IBackupFactory
    {
        string GerarBackupArquivo(BackupModel model, string NmArquivo);
        byte[] GerarBackupBytes(BackupModel model);
    }
}