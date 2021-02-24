using MariGold.OpenXHTML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Web.UI.Backup
{
    public class BackupFactory : IBackupFactory
    {

        public byte[] GerarBackup(BackupModel model)
        {
            byte[] retorno = null;


            if (File.Exists(model.CaminhoTemplate))
            {
                var html = File.ReadAllText(model.CaminhoTemplate);

                var novoHtml = SubstituirTags(html, model.Informacoes);
                var arquivoTemp = Guid.NewGuid().ToString();

                using (MemoryStream mem = new MemoryStream())
                {
                    var parser = new HtmlParser(novoHtml);

                    WordDocument doc = new WordDocument(arquivoTemp);
                    doc.Process(parser);
                    doc.Save();
                }

                retorno = File.ReadAllBytes(arquivoTemp);

                File.Delete(arquivoTemp);
            }
            else
                throw new Exception("Template não encontrado");

            return retorno;
        }

        private string SubstituirTags(string html, List<InformacaoBackupModel> informacoes)
        {
            foreach (var item in informacoes)
            {
                html = html.Replace(item.Tag, item.Valor);
            }

            return html;
        }


    }
}