using Dominio.Enumerado;
using ApplicationService.Servico;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using DAL.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Dominio.Validacao.Notificacoes;
using Dominio.Validacao.NotificacoesMensagem;
using Dominio.Validacao.NotificacoesSmtp;

namespace TestProject.Notificacoes
{
    [TestClass]
    public class NotificacaoWindowsServiceTest
    {
        private INotificacaoRepositorio _notificacaoRepository;
        private IUsuarioRepositorio _usuarioRepository;
        private NotificacaoAppServico _notificacaoServico;

        private AptoParaCadastroNotificacaoValidation _validaCamposNotificacao;

        private INotificacaoMensagemRepositorio _notificacaoMensagemRepository;
        private INotificacaoRepositorio _notificacaoRepositorio;
        private IRegistroConformidadesRepositorio _registroConformidadesRepositorio;
        private NotificacaoMensagemAppServico _notificacaoMensagemServico;
        private NotificacaoMensagemRepositorio _notificacaoMensagemRepositorio;

        private AptoParaCadastroNotificacaoMensagemValidation _validaCamposMensagem;


        private INotificacaoSmtpRepositorio _notificacaoSmtpRepositorio;
        private NotificacaoSmtpAppServico _notificacaoSmtpServico;

        private AptoParaCadastroNotificacaoSmtp _validaCamposSmtp;



        [TestInitialize]
        public void Start()
        {
            _notificacaoRepository = new NotificacaoRepositorio();
            _usuarioRepository = new UsuarioRepositorio();
            //_notificacaoServico = new NotificacaoServico(_notificacaoRepository, _usuarioRepository);

            _validaCamposNotificacao = new AptoParaCadastroNotificacaoValidation();

            _notificacaoMensagemRepository = new NotificacaoMensagemRepositorio();
            _notificacaoRepositorio = new NotificacaoRepositorio();
            _registroConformidadesRepositorio = new RegistroConformidadesRepositorio();
            _notificacaoMensagemServico = new NotificacaoMensagemAppServico(_notificacaoMensagemRepository, _notificacaoRepositorio, _registroConformidadesRepositorio);

            _validaCamposMensagem = new AptoParaCadastroNotificacaoMensagemValidation();

            _notificacaoSmtpRepositorio = new NotificacaoSmtpRepositorio();
            _notificacaoSmtpServico = new NotificacaoSmtpAppServico(_notificacaoSmtpRepositorio);

            _validaCamposSmtp = new AptoParaCadastroNotificacaoSmtp();
        }

        #region Testes de CRUD da tabela NotificacaoSMTP

        [TestMethod]
        [TestCategory("NotificacaoSmtp - Cadastra")]
        public void Cadastra_NotificacaoSmtp_true()
        {
            var notificacaoSmtp = new NotificacaoSmtp()
            {
                CdSenha = "123456",
                CdUsuario = "teste",
                DsSmtp = "stmp4.g2it.com.br",
                FlAtivo = true,
                NmNome = "Teste",
                NuPorta = 587
            };

            var result = _validaCamposSmtp.Validate(notificacaoSmtp);

            //if (result.IsValid)
            //{
            //    _notificacaoSmtpServico.Add(notificacaoSmtp);
            //}

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("NotificacaoSmtp - Cadastra")]
        public void Cadastra_NotificacaoSmtp_false()
        {
            var notificacaoSmtp = new NotificacaoSmtp()
            {
                CdSenha = "123456",
                CdUsuario = "teste",
                FlAtivo = false,
                NuPorta = 587
            };

            var result = _validaCamposSmtp.Validate(notificacaoSmtp);

            //if (result.IsValid)
            //{
            //    _notificacaoSmtpServico.Add(notificacaoSmtp);
            //}

            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Objetivo do teste é testar todos os registro da tabela são lidos
        /// </summary>
        //[TestMethod]
        //public void LeDadosSMTP()
        //{
        //    var _notificacoesSmtp = _notificacaoSmtpServico.GetAll();
        //    Assert.IsTrue(_notificacoesSmtp.Count() > 0);
        //}

        /// <summary>
        /// Objetivo do teste é testar a atualiza de uma novo smpt
        /// </summary>
        //[TestMethod]
        //public void AtualizaSMTP()
        //{

        //    var notificacaoSmtp = _notificacaoSmtpServico.GetById(4);

        //    notificacaoSmtp.NmNome = "e-Qualit 444";
        //    _notificacaoSmtpServico.Update(notificacaoSmtp);
        //    notificacaoSmtp = _notificacaoSmtpServico.GetById(4);

        //    Assert.IsTrue(notificacaoSmtp.NmNome == "e-Qualit 444");
        //}

        /// <summary>
        /// Objetivo do teste é testar se a exclusão de um smpt
        /// </summary>
        //[TestMethod]
        //public void RemoveSMTP()
        //{
        //    var notificacaoSmtp = _notificacaoSmtpServico.GetAll().FirstOrDefault();

        //    _notificacaoSmtpServico.Remove(notificacaoSmtp);
        //    notificacaoSmtp = _notificacaoSmtpServico.GetById(notificacaoSmtp.IdSmptNotificacao);

        //    Assert.IsTrue(notificacaoSmtp == null);

        //}
        #endregion

        #region Teste de CRUD da tabela NotificacaoMensagem

        [TestMethod]
        [TestCategory("NotificacaoMensagem - Cadastro")]
        public void Cadastra_NotificacaoMensagem_true()
        {
            NotificacaoMensagem notificacaoMensagem = new NotificacaoMensagem()
            {
                DsAssunto = "Teste",
                DsMensagem = "teste",
                DtCadastro = DateTime.Now,
                DtEnvio = DateTime.Now,
                FlEnviada = false,
                IdSite = 1,
                IdSmtpNotificacao = 4,
                NmEmailNome = "Silvestre",
                NmEmailPara = "aciolecarmo@g2db.com.br"
            };

            var result = _validaCamposMensagem.Validate(notificacaoMensagem);

            //if (result.IsValid)
            //{
            //    _notificacaoMensagemServico.Add(notificacaoMensagem);
            //}

            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        [TestCategory("NotificacaoMensagem - Cadastro")]
        public void Cadastra_NotificacaoMensagem_False()
        {

            NotificacaoMensagem notificacaoMensagem = new NotificacaoMensagem()
            {
                DsAssunto = "Teste",
                DsMensagem = "teste",
                DtCadastro = DateTime.Now,
                DtEnvio = DateTime.Today.AddDays(-2),
                FlEnviada = false,
                IdNotificacaoMenssagem = 0,
                IdSmtpNotificacao = 4,
                NmEmailNome = "Silvestre",
                NmEmailPara = "aciolecarmo@g2db.com.br"
            };
            var result = _validaCamposMensagem.Validate(notificacaoMensagem);

            if (result.IsValid)
            {
                _notificacaoMensagemRepositorio.Add(notificacaoMensagem);
            }

            Assert.IsFalse(result.IsValid);

        }

        //[TestMethod]
        //public void LeDadosMensagem()
        //{
        //    var _registros = _notificacaoMensagemServico.GetAll();
        //    Assert.IsTrue(_registros.Count() > 0);
        //}
        /// <summary>
        /// Objetivo do teste é testar todas as mensagens não enviada
        /// </summary>
        //[TestMethod]
        //public void LeMensagensNaoEnviadas()
        //{
        //    var _registros = _notificacaoMensagemServico.ObterMensagensNaoEnviadas(30);
        //    Assert.IsTrue(_registros.Count() > 0);
        //}
        /// <summary>
        /// Objetivo do teste é testar a atualiza de uma novo smpt
        /// </summary>
        [TestMethod]
        public void AtualizaMensagem()
        {
            var notificacaoMensagem = _notificacaoMensagemRepositorio.GetAll().FirstOrDefault();

            notificacaoMensagem.DsAssunto = "e-Qualit 444";
            _notificacaoMensagemRepositorio.Update(notificacaoMensagem);

            //notificacaoMensagem = _notificacaoMensagemServico.GetById(notificacaoMensagem.IdNotificacaoMenssagem);

            Assert.IsTrue(notificacaoMensagem.DsAssunto == "e-Qualit 444");
        }

        /// <summary>
        /// Objetivo do teste é testar se a exclusão de um smpt
        /// </summary>
        //[TestMethod]
        //public void RemoveMensagem()
        //{
        //    var notificacaoMensagem = _notificacaoMensagemServico.GetAll().FirstOrDefault();

        //    _notificacaoMensagemServico.Remove(notificacaoMensagem);
        //    notificacaoMensagem = _notificacaoMensagemServico.GetById(notificacaoMensagem.IdNotificacaoMenssagem);

        //    Assert.IsTrue(notificacaoMensagem == null);

        //}
        #endregion

        #region Teste com a Tabela de Notificacao

        [TestMethod]
        [TestCategory("Notificacao - Cadastra")]
        public void Cadastra_Notificacao_True()
        {
            Notificacao notificacao = new Notificacao($"Não Conformidade #1",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                1, 108,
                                1, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                1);

            var result = _validaCamposNotificacao.Validate(notificacao);

            //if (result.IsValid)
            //{
            //    _notificacaoServico.Add(notificacao);
            //}
            Assert.IsTrue(result.IsValid);

        }

        [TestMethod]
        [TestCategory("Notificacao - Cadastra")]
        public void Cadastra_Notificacao_False()
        {
            Notificacao notificacao = new Notificacao($"Não Conformidade #1",
                                null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                1, 108,
                                1, 0,
                                ((char)TipoNotificacao.Leitura).ToString(),
                                0);

            notificacao.IdProcesso = 0;
            notificacao.IdSite = 0;

            var result = _validaCamposNotificacao.Validate(notificacao);
            Assert.IsFalse(result.IsValid);

        }



        /// <summary>
        /// Objetivo do teste é testar todos os registro da tabela são lidos
        /// </summary>
        //[TestMethod]
        //public void LeDadosNotificacao()
        //{
        //    var _registros = _notificacaoServico.ObterNotificacoesUsuario(1, 1);
        //    Assert.IsTrue(_registros.Count() > 0);
        //}
        #endregion

        #region Teste geração e fila para disparo
        /// <summary>
        /// Objetivo do teste é testar a inclusão de uma nova mensagem
        /// </summary>
        //[TestMethod]
        //public void GeraFilaEmail()
        //{
        //    _notificacaoMensagemServico.GeraFilaEmail();

        //}
        #endregion
    }
}
