using System;
using System.Web;
using System.Web.Mvc;
using Dominio.Enumerado;
using ApplicationService.Interface;
using ApplicationService.Servico;
using Dominio.Interface.Repositorio;
using DAL.Repository;

namespace Web.UI.Helpers
{
    public class AutorizacaoUsuarioAttribute : AuthorizeAttribute
    {
        private readonly int _idFuncao;
        private readonly int _idFuncionalidade;
        private readonly ICargoProcessoRepositorio _cargoProcessoRepositorio = new CargoProcessoRepositorio();
        private readonly IUsuarioCargoRepositorio _usuarioCargoRepositorio = new UsuarioCargoRepositorio();


        public AutorizacaoUsuarioAttribute(int idFuncao, int idFuncionalidade)
        {
            _idFuncao = idFuncao;
            _idFuncionalidade = idFuncionalidade;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var idUsuario = Util.ObterCodigoUsuarioLogado();
                var idPerfil = Util.ObterPerfilUsuarioLogado();
                //var idProcessoSelecionado = Util.ObterProcessoSelecionado();
                
                if ((int)PerfisAcesso.Administrador == idPerfil)
                {
                    return true;
                }
                else if ((int)PerfisAcesso.Coordenador == idPerfil)
                {
                    return true;
                }
                else if ((int)PerfisAcesso.Suporte == idPerfil)
                {
                    return true;
                }
                else if ((int)PerfisAcesso.Colaborador == idPerfil)
                {
                    ServiceLocator.ServiceLocator.Register<ICargoProcessoAppServico>(new CargoProcessoAppServico(_cargoProcessoRepositorio, _usuarioCargoRepositorio));

                    var cargoProcessoAppService = ServiceLocator.ServiceLocator.GetService<ICargoProcessoAppServico>();

                    return cargoProcessoAppService.PossuiAcessoAFuncao(idUsuario, _idFuncao);
                }

                return false;
            }
            catch (Exception ex)
            {  
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Login/Index");
        }
    }
}