using System.Linq;
using System.Web.Mvc;
using Dominio.Enumerado;
using Web.UI.Helpers;
using ApplicationService.Interface;
using System.Threading;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class NotificacaoController : BaseController
    {
        private readonly INotificacaoAppServico _notificacaoAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public NotificacaoController(INotificacaoAppServico notificacaoAppServico, 
                                     ILogAppServico logAppServico,
                                     IUsuarioAppServico usuarioAppServico,
                                     IProcessoAppServico processoAppServico,
                                     IPendenciaAppServico pendenciaAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico,  pendenciaAppServico)
        {
            _notificacaoAppServico = notificacaoAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        // GET: Notificacao
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idUsuario = Util.ObterCodigoUsuarioLogado();
            var idPerfil = Util.ObterPerfilUsuarioLogado();
            var idSite = Util.ObterSiteSelecionado();

            var _notificacoes = _notificacaoAppServico.ObterNotificacoesUsuario(idUsuario, idPerfil, idSite).Where(x => x.IdRelacionado != 1).ToList();

            return PartialView(_notificacoes);
        }

        public ActionResult RedirecionaNotificacao(int idNotificacao)
        {
            var notificacao = _notificacaoAppServico.GetById(idNotificacao);

            var urlFuncionalidade = notificacao.Funcionalidade.Url.ToString();

            if (notificacao.TpNotificacao == ((char)TipoNotificacao.Leitura).ToString())
            {
                _notificacaoAppServico.Remove(notificacao);
            }

            return RedirectToAction("Editar", urlFuncionalidade, new { id = notificacao.IdRelacionado });
        }
    }
}