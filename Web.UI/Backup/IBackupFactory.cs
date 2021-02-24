namespace Web.UI.Backup
{
    public interface IBackupFactory
    {
        string GerarBackupArquivo(BackupModel model);
        byte[] GerarBackupBytes(BackupModel model);
    }
}