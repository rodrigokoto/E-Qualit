namespace Web.UI.Backup
{
    public interface IBackupFactory
    {
        byte[] GerarBackup(BackupModel model);
    }
}