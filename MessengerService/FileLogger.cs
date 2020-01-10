using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWindowsService
{
    public static class FileLogger
    {
        static FileStream fileStream;
        

        public static void Log(string message, Exception ex = null)
        {
            if (fileStream == null)
            {
                var dirLog = ConfigurationManager.AppSettings["DiretorioLog"];
                var fileName = "Log_{0}.log";
                fileName = string.Format(fileName, DateTime.Now.ToString("yyyyMMddHH"));
                fileStream = new FileStream(dirLog + fileName, FileMode.Append, FileAccess.Write);
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.AutoFlush = true;
                var dualOutput = new ConsoleDualOutput(streamWriter, Console.Out);
                Console.SetOut(dualOutput);
            }

            Console.WriteLine(MontarMensagem(message, ex));
        }


        private static string MontarMensagem(string message, Exception ex = null)
        {
            string retorno = string.Empty;

            if (ex == null)
            {
                retorno = "[" + DateTime.Now.ToString("HH:mm:ss") + "] - " + message;
            }
            else
            {
                retorno = "[" + DateTime.Now.ToString("HH:mm:ss") + "] - " + message + Environment.NewLine + "ERRO: " + ex.Message + Environment.NewLine + "STACKTRACE: " + ex.StackTrace;
            }

            return retorno;
        }
      
    }
}
