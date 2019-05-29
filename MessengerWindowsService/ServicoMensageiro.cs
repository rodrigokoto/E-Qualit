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
using System.Threading.Tasks;

namespace MessengerWindowsService
{
    public partial class ServicoMensageiro : ServiceBase
    {
        private readonly IFilaEnvioRepositorio _filaEnvioRepositorio;
        private readonly ILogRepositorio _logRepositorio;

        public ServicoMensageiro()
        {
            _filaEnvioRepositorio = new FilaEnvioRepositorio();
            _logRepositorio = new LogRepositorio();
            InitializeComponent();
        }

        public void Processar()
        {
            try
            {
                FileLogger.Log("Início processamento");

                var diasCorte = Convert.ToInt32(ConfigurationManager.AppSettings["diasCorte"]);
                var dataCorte = DateTime.Now.AddDays(diasCorte*-1);

                FileLogger.Log("Parâmetro diasCorte: " + diasCorte);


                FileLogger.Log("Buscando envios não agendados");

                var enviosNaoAgedados = _filaEnvioRepositorio.Get(
                    x => x.DataAgendado == null
                    && x.Enviado == false
                    && (x.DataInclusao >= dataCorte || diasCorte == 0) /*SE O PARÂMETRO FOR DEFINIDO COMO ZERO, IGNORA A VALIDAÇÃO DE DATAINCLUSAO*/
                    ).ToList();


                FileLogger.Log(enviosNaoAgedados.Count.ToString() + " - envios NÃO AGENDADOS encontrados");

                this.Enviar(enviosNaoAgedados);

                FileLogger.Log("Processamento de envioS NÃO AGENDADOS realizado");

                throw new Exception("ERRO HAHAHAH");

                FileLogger.Log("Buscando envios AGENDADOS");

                var enviosAgendados = _filaEnvioRepositorio.Get(x => x.DataAgendado <= DateTime.Now && x.Enviado == false).ToList();

                FileLogger.Log(enviosAgendados.Count.ToString() + " - envios AGENDADOS encontrados");

                this.Enviar(enviosAgendados);

                FileLogger.Log("Processamento de envioS AGENDADOS realizado");

                FileLogger.Log("##########################################");
                FileLogger.Log("############ Final processamento #########");
                FileLogger.Log("##########################################");
            }
            catch (Exception ex)
            {
                FileLogger.Log("Erro durante o processamento ", ex);
                GravaLog(ex);
            }

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


        }

        protected override void OnStop()
        {
        }
    }
}
