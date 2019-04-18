using System;
using System.ServiceProcess;
using System.Configuration;
using Dominio.Servico;
using System.ServiceProcess;

namespace NotificacaoWindowsService
{
    public partial class NotificacaoWindowsService : ServiceBase
    {
        public System.Timers.Timer tTimerEnvioEmail = new System.Timers.Timer();
        public System.Timers.Timer tTimerGeraFila = new System.Timers.Timer();

        public NotificacaoWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            tTimerEnvioEmail.Interval = 1000; //Da 1ª vez espera um segundo para enviar
            tTimerEnvioEmail.Elapsed += new System.Timers.ElapsedEventHandler(tTimerEnvioEmail_Elapsed);
            tTimerEnvioEmail.Enabled = true;
            tTimerEnvioEmail.Start();

            tTimerGeraFila.Interval = new TimeSpan(0, 0, 30).TotalMilliseconds; 
            tTimerGeraFila.Elapsed += new System.Timers.ElapsedEventHandler(tTimerGeraFila_Elapsed);
            tTimerGeraFila.Enabled = true;
            tTimerGeraFila.Start();
        }

        protected override void OnStop()
        {
            tTimerEnvioEmail.Enabled = false;
            tTimerEnvioEmail.Stop();

            tTimerGeraFila.Enabled = false;
            tTimerGeraFila.Stop();
        }

        #region tTimer_Elapsed
        /// <summary>
        /// Evento do timer com o código para enviar os emails para os usuários
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tTimerEnvioEmail_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Os próximos envio conforme o definido no parâmetro
            try
            {
                tTimerEnvioEmail.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]) * 1000;
                tTimerEnvioEmail.Enabled = false;
                tTimerEnvioEmail.Stop();

                SendNotificacao.Enviar();
            }
            catch (Exception ex)
            {
                GravaLog.Log(ex);
            }
            finally
            {
                tTimerEnvioEmail.Enabled = true;
                tTimerEnvioEmail.Start();
            }
        }

        /// <summary>
        /// Evento do timer com o código para inclusão das notificações na fila de envio de email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tTimerGeraFila_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dominio.Interface.Repositorio.INotificacaoMensagemRepositorio _notificacaoMensagemRepositorio = null;
            Dominio.Interface.Repositorio.INotificacaoRepositorio _notificacaoRepositorio = null;
            Dominio.Interface.Repositorio.IRegistroConformidadesRepositorio _registroConformidadeRepositorio = null;

            NotificacaoMensagemServico _mensagem = new NotificacaoMensagemServico(_notificacaoMensagemRepositorio, _notificacaoRepositorio, _registroConformidadeRepositorio);
            try
            {
                //Desabilita o timer
                tTimerGeraFila.Enabled = false;
                tTimerGeraFila.Stop();

                _notificacaoMensagemRepositorio = new DAL.Repository.NotificacaoMensagemRepositorio();
                _notificacaoRepositorio = new DAL.Repository.NotificacaoRepositorio();
                _registroConformidadeRepositorio = new DAL.Repository.RegistroConformidadesRepositorio();

                _mensagem = new NotificacaoMensagemServico(_notificacaoMensagemRepositorio, _notificacaoRepositorio, _registroConformidadeRepositorio);
                _mensagem.GeraFilaEmail();
            }
            catch (Exception ex)
            {
                GravaLog.Log(ex);
            }
            finally
            {
                if (_notificacaoMensagemRepositorio != null) _notificacaoMensagemRepositorio.Dispose();
                if (_notificacaoRepositorio != null) _notificacaoRepositorio.Dispose();
                if (_registroConformidadeRepositorio != null) _registroConformidadeRepositorio.Dispose();

                if (_mensagem != null) _mensagem.Dispose();

                tTimerGeraFila.Enabled = true;
                tTimerGeraFila.Start();
            }
        }
        #endregion

    }
}
