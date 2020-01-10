using ApplicationService.Entidade;
using DAL.Repository;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerWindowsService
{
    public partial class ServicoMensageiro : ServiceBase
    {
        private readonly IFilaEnvioRepositorio _filaEnvioRepositorio;
        private readonly ILogRepositorio _logRepositorio;
        private Thread ServicoMensageiroThread;

        public ServicoMensageiro()
        {
            _filaEnvioRepositorio = new FilaEnvioRepositorio();
            _logRepositorio = new LogRepositorio();
        
        }
        /// <summary>
        /// Realiza o envio dos e-mail agendados a cada 30 segundos
        /// </summary>
        public void Processar()
        {
            OnStart(null);
   
        }

        private void GravaLog(Exception ex)
        {

            var log = new Log(null,
                              Convert.ToInt32(Acao.ServicoMensageiro),
                              null,
                              null,
                              ex);

            _logRepositorio.Add(log);
        }

        private void Enviar(List<FilaEnvio> envios)
        {
            foreach (var item in envios)
            {
                try
                {
                    Email email = new Email();

                    email.Assunto = item.Assunto;
                    email.De = ConfigurationManager.AppSettings["EmailDE"];
                    email.Para = item.Destinatario;
                    email.Conteudo = item.Mensagem;
                    email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
                    email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
                    email.Enviar();
                    item.Enviado = true;
                    item.DataEnviado = DateTime.Now;
                    _filaEnvioRepositorio.Update(item);
                }
                catch (Exception ex)
                {
                    FileLogger.Log("Erro ao enviar e-mail id: " + item.Id.ToString(), ex);
                    GravaLog(ex);
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            ServicoMensageiroThread = new Thread(() => ServicoMensageiroService());
            ServicoMensageiroThread.Start();

        }

        private void ServicoMensageiroService()
        {
            while (true) {

                try
                {

                    var enviosAgendados = _filaEnvioRepositorio.Get(x => x.DataAgendado <= DateTime.Now && x.Enviado == false).ToList();

                    this.Enviar(enviosAgendados);

                }
                catch (Exception ex)
                {
                    FileLogger.Log("Erro durante o processamento ", ex);
                    GravaLog(ex);
                }

                Thread.Sleep(30000);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
