using HtmlAgilityPack;
using MariGold.OpenXHTML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Web.UI.Backup
{
    public class BackupFactory : IBackupFactory
    {
        public byte[] GerarBackupBytes(BackupModel model)
        {
            byte[] retorno = null;
            var caminhoArquivoGerado = GerarBackupArquivo(model);
            retorno = File.ReadAllBytes(caminhoArquivoGerado);
            File.Delete(caminhoArquivoGerado);

            return retorno;
        }


        public string GerarBackupArquivo(BackupModel model)
        {
            string retorno = string.Empty;

            if (File.Exists(model.CaminhoTemplate))
            {
                var html = File.ReadAllText(model.CaminhoTemplate);
                var novoHtml = SubstituirTags(html, model.Informacoes);

                var arquivoBackup = Guid.NewGuid().ToString() + ".docx";

                using (MemoryStream mem = new MemoryStream())
                {
                    var parser = new HtmlParser(novoHtml);

                    if (!String.IsNullOrEmpty(model.CaminhoBackup) && !model.CaminhoBackup.EndsWith(@"\") && model.CaminhoBackup.Length > 0)
                        model.CaminhoBackup += @"\";

                    retorno = model.CaminhoBackup + arquivoBackup;

                    WordDocument doc = new WordDocument(retorno);
                    doc.Process(parser);
                    doc.Save();
                }
            }
            else
                throw new Exception("Template não encontrado");

            return retorno;
        }


        private string TratarHtml(string html)
        {
            string htmlRetorno = string.Empty;

            if (html != string.Empty)
            {
                using (var mem = new MemoryStream())
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    foreach (HtmlNode imgs in htmlDoc.DocumentNode.SelectNodes("//img"))
                    {
                        var src = imgs.Attributes.Where(a => a.Name == "src").FirstOrDefault();

                        if (src != null)
                        {
                            var url = src.Value;
                            var imagemBase64 = $"data:image/png;base64,{ObterBase64Url(url)}";
                            src.Value = imagemBase64;
                        }
                    }

                    htmlDoc.Save(mem);

                    htmlRetorno = Encoding.GetEncoding("iso-8859-1").GetString(mem.ToArray());
                }
            }
            return htmlRetorno;
        }

        private string ObterBase64Url(string url)
        {
            var retorno = string.Empty;
            Stream stream = null;
            byte[] buf;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();

                retorno = Convert.ToBase64String(buf, 0, buf.Length);
            }
            catch (Exception ex)
            {
                retorno = url;
            }

            return retorno;
        }

        private string SubstituirTags(string html, List<InformacaoBackupModel> informacoes)
        {
            foreach (var item in informacoes)
            {
                if (item.Tipo == TipoInformacaoBackup.CkEditor)
                {
                    var htmlCkEditor = TratarHtml(item.Valor);
                    item.Valor = htmlCkEditor;
                }

                html = html.Replace(item.Tag, item.Valor);
            }

            return html;
        }


    }
}