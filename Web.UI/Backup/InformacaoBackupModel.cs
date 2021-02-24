using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Backup
{
    public enum TipoInformacaoBackup
    {
        Texto = 1,
        Imagem = 2,
        CkEditor = 3
    }

    public class InformacaoBackupModel
    {
        public TipoInformacaoBackup Tipo { get; set; }
        public string Tag { get; set; }
        public string Valor { get; set; }

    }
}