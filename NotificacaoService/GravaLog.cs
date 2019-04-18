using System;
using System.Configuration;
using System.IO;

namespace NotificacaoWindowsService
{
    public static class GravaLog
    {
        public static void Log(Exception pException)
        {
            string sLog = ConfigurationManager.AppSettings["Log"];

            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(sLog + "\\log.txt", true);
                sw.WriteLine("\r\nData: " + string.Format("{0:dd/MM/yyyy}", DateTime.Now) + string.Format(" {0:hh:mm}", DateTime.Now));
                sw.WriteLine(pException.Message.ToString());
                sw.WriteLine(pException.StackTrace.ToString() + "\r\n");
            }
            catch
            { }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        public static void LogArquivo(Exception pException, string pArquivo)
        {
            string sLog = ConfigurationManager.AppSettings["Log"];

            StreamWriter sw = null;

            try
            {
                sw.WriteLine("\r\nData: " + string.Format("{0:dd/MM/yyyy}", DateTime.Now) + string.Format(" {0:hh:mm}", DateTime.Now));
                sw.WriteLine("Arquivo: " + pArquivo);
                sw.WriteLine(pException.Message.ToString());
                sw.WriteLine(pException.StackTrace.ToString() + "\r\n");
            }
            catch
            { }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
    }
}
