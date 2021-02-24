using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Backup
{
    public class BackupModel
    {
        public string CaminhoTemplate { get; set; }
        public List<InformacaoBackupModel> Informacoes { get; set; }
    }
}