using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using Dominio.Servico;
using System.Net.Mail;

namespace NotificacaoWindowsService
{
    public class Email
    {
        public Email()
        {
            Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            Usuario = ConfigurationManager.AppSettings["SMTPUser"];
            Senha = ConfigurationManager.AppSettings["SMTPPassword"];
        }

        #region Propriedade Privadas

        private List<string> _anexos;

        #endregion

        #region Propriedades Publicas

        public string Servidor;
        public string De;
        public string Para;
        public string Copia;
        public string Assunto;
        public string Conteudo;
        public string Usuario;
        public string Senha;
        public int Porta;

        #endregion

        #region Metodos Publicos

        public void Enviar()
        {
            //configuracoes do email
            MailMessage mess = new MailMessage();
            mess.From = new MailAddress(De);

            if (Para != null && Para.Length > 0)
            {
                foreach (string address in Para.Split(';'))
                {
                    mess.To.Add(new MailAddress(address));
                }
            }

            if (Copia != null && Copia.Length > 0)
            {
                foreach (string address in Copia.Split(';'))
                {
                    mess.CC.Add(new MailAddress(address));
                }
            }

            mess.Subject = Assunto;

            //formato do email
            mess.IsBodyHtml = true;

            //corpo do email						
            mess.Body = Conteudo;

            //prioridade
            mess.Priority = MailPriority.High;

            //anexos
            if (_anexos != null)
            {
                foreach (string path in _anexos)
                {
                    mess.Attachments.Add(new Attachment(path));
                }
            }

            //define servidor SMTP
            SmtpClient emailClient;
            if (Porta == 0)
                emailClient = new SmtpClient(Servidor);
            else
                emailClient = new SmtpClient(Servidor, Porta);

            //se for envio autenticado
            if (Usuario != null && Usuario.Trim() != "" &&
                Senha != null && Senha.Trim() != "")
            {
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = new System.Net.NetworkCredential(Usuario, Senha);
            }

            //envia email            	
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["EscreverEmailArquivo"]))
                emailClient.Send(mess);
            else
                EscreveArquivo(mess, ConfigurationManager.AppSettings["EscreverEmailArquivo"]);
        }

        public void AdicionarAnexo(string nomeArquivo_)
        {
            if (_anexos == null)
                _anexos = new List<string>();

            _anexos.Add(nomeArquivo_);
        }

        private void EscreveArquivo(MailMessage mess, string pathArquivo)
        {
            System.IO.StreamWriter mWriter = new System.IO.StreamWriter(pathArquivo, true);
            mWriter.WriteLine("Assunto:" + mess.Subject);
            mWriter.WriteLine("De:" + mess.From.ToString());
            mWriter.WriteLine("Para:" + mess.To.ToString());
            mWriter.WriteLine("Copia:" + mess.CC.ToString());
            mWriter.WriteLine("Copia Oculta:" + mess.Bcc.ToString());
            mWriter.WriteLine("Data:" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            mWriter.WriteLine("----------------------------------------");
            mWriter.WriteLine(mess.Body);
            mWriter.WriteLine("----------------------------------------");
            mWriter.Close();
            mWriter.Dispose();
        }

        #endregion

    }

    public static class SendNotificacao
    {

        public static void Enviar()
        {
            Dominio.Interface.Repositorio.INotificacaoSmtpRepositorio _smtpRepository = new DAL.Repository.NotificacaoSmtpRepositorio();
            Dominio.Interface.Repositorio.INotificacaoMensagemRepositorio _mensagemRepository = new DAL.Repository.NotificacaoMensagemRepositorio();
            Dominio.Interface.Repositorio.INotificacaoRepositorio _notificacaoRepositorio = new DAL.Repository.NotificacaoRepositorio();
            Dominio.Interface.Repositorio.IRegistroConformidadesRepositorio _registroConformidadeRepositorio = new DAL.Repository.RegistroConformidadesRepositorio();

            Dominio.Interface.Repositorio.IPaiRepositorio _paiRepositorio = new DAL.Repository.PaiRepositorio();
            Dominio.Interface.Repositorio.IPlaiRepositorio _plaiRepositorio = new DAL.Repository.PlaiRepositorio();
            Dominio.Interface.Repositorio.ILogRepositorio _logRepositorio = new DAL.Repository.LogRepositorio();
            NotificacaoMensagemServico _mensagem = new NotificacaoMensagemServico(_mensagemRepository, _notificacaoRepositorio, _registroConformidadeRepositorio);

            Dominio.Interface.Servico.IPlaiServico _plaiServico = new PlaiServico(_plaiRepositorio, _paiRepositorio, _logRepositorio, _mensagem);
            _plaiServico.InsereMensagemPlaisVencidos();

            try
            {
                List<Dominio.Entidade.NotificacaoSmtp> _listServidores = _smtpRepository.GetAll().Where(w => w.FlAtivo).ToList();

                foreach (Dominio.Entidade.NotificacaoSmtp server in _listServidores)
                {
                    //Mando um número limitado de mensagem por servidor
                    int iMsg = int.Parse(ConfigurationManager.AppSettings["NumRegistos"]);

                    //Leio apenas uma parte das menssagens
                    List<Dominio.Entidade.NotificacaoMensagem> _listaMsg = _mensagem.ObterMensagensNaoEnviadas(iMsg).ToList();

                    //Processo as menssagens
                    foreach (Dominio.Entidade.NotificacaoMensagem item in _listaMsg)
                    {
                        try
                        {
                            Email _email = new Email();

                            _email.Servidor = server.DsSmtp;
                            _email.Porta = server.NuPorta;
                            _email.Usuario = server.CdUsuario;
                            _email.Senha = server.CdSenha;

                            _email.Copia = string.Empty;
                            _email.De = server.NmNome + " <" + server.CdUsuario + ">";
                            _email.Para = item.NmEmailNome + " <" + item.NmEmailPara + ">";
                            _email.Assunto = item.DsAssunto;
                            _email.Conteudo = item.DsMensagem;

                            _email.Enviar();

                            item.DtEnvio = DateTime.Now;
                            item.FlEnviada = true;
                            item.IdSmtpNotificacao = server.IdSmptNotificacao;

                            _mensagem.Update(item);
                        }
                        catch (Exception ex)
                        {
                            GravaLog.Log(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GravaLog.Log(ex);
            }
            finally
            {
                _smtpRepository.Dispose();
                _mensagemRepository.Dispose();
                _notificacaoRepositorio.Dispose();
                _registroConformidadeRepositorio.Dispose();

                _mensagem.Dispose();
            }
        }
    }
}
