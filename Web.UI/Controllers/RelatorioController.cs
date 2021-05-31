using System;
using System.Linq;
using System.Web.Mvc;
using Dominio.Enumerado;
using ApplicationService.Interface;
using Web.UI.Helpers;
using Dominio.Entidade;
using System.Collections.Generic;
using Highsoft.Web.Mvc.Charts;
using System.Globalization;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class RelatorioController : BaseController
    {
        private readonly ILogAppServico _logAppServico;
        private readonly IRegistroConformidadesAppServico _naoConformidade;
        private readonly IRelatorioAppServico _relatorioAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IRegistroConformidadesAppServico _registroConformidadesAppServico;


        public RelatorioController(ILogAppServico logAppServico,
                                   IRegistroConformidadesAppServico naoConformidade,
                                   IUsuarioAppServico usuarioAppServico,
                                   IProcessoAppServico processoAppServico,
                                   IPendenciaAppServico pendenciaAppServico,
                                   IControladorCategoriasAppServico controladorCategoriasServico,
                                   IRelatorioAppServico relatorioAppServico,
                                   IRegistroConformidadesAppServico registroConformidadesAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {
            _logAppServico = logAppServico;
            _naoConformidade = naoConformidade;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _relatorioAppServico = relatorioAppServico;
            _registroConformidadesAppServico = registroConformidadesAppServico;
        }

        public ActionResult DashBoard()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();
        }

        public JsonResult NaoConformidade()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                var naoConformidades = _naoConformidade.GetAll();

                var naoConformidade = new
                {
                    Total = naoConformidades.Count(),
                    Encerrada = naoConformidades.Where(x => x.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada).Count(),
                    Aberta = naoConformidades.Where(x => x.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada).Count()
                };

                return Json(new { StatusCode = 200, Relatorio = naoConformidade }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);

                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index(int idRelatorio)
        {
            var relatorio = _relatorioAppServico.GetById(idRelatorio);
            var idSite = Util.ObterSiteSelecionado();
            relatorio = GetParameters(relatorio);
            return View(relatorio);
        }

        public Relatorio GetParameters(Relatorio relatorio)
        {

            switch (relatorio.Url)
            {
                case "Abertas":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Mês", "");
                    break;
                case "AbertasAno":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Ano", "");
                    break;
                case "Fechadas":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Mês", "");
                    break;
                case "FechadasAno":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Ano", "");
                    break;
                default:
                    break;
            }

            return relatorio;
        }



        [Authorize]
        public ViewResult Abertas(Relatorio relatorio)
        {
            DateTime monthParam = new DateTime();

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Meses = new Dictionary<int, int?>();

            Meses.Add(monthParam.Month, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }


            if (relatorio.Parametros.ContainsKey("Mês"))
            {
                var param1 = "01/" + relatorio.Parametros["Mês"];
                monthParam = Convert.ToDateTime(param1);


            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Month == monthParam.Month && x.DtInclusao.Year == monthParam.Year && x.TipoRegistro == tpRegistro && x.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada).ToList();

            Meses[monthParam.Month] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Meses[monthParam.Month].Value
            };
            

            List<BarSeriesData> MesesData = new List<BarSeriesData>();


            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthParam.Month));

            ViewData["AbertaMes"] = MesesData;
            ViewData["Categories"] = Categories;
            
            return View("Abertas", relatorio);
        }
        public ViewResult AbertasAno(Relatorio relatorio)
        {
            var YearParam = 0;

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Year = new Dictionary<int, int?>();

            Year.Add(YearParam, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }


            if (relatorio.Parametros.ContainsKey("Ano"))
            {
                var param1 = relatorio.Parametros["Ano"];
                YearParam = int.Parse(param1);


            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Year == YearParam && x.TipoRegistro == tpRegistro && x.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata).ToList();

            Year[YearParam] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Year[YearParam].Value
            };


            List<BarSeriesData> MesesData = new List<BarSeriesData>();


            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(YearParam.ToString());

            ViewData["AbertasAno"] = MesesData;
            ViewData["Categories"] = Categories;

            return View("AbertasAno", relatorio);
        }
        public ViewResult FechadasAno(Relatorio relatorio)
        {
            var YearParam = 0;

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Year = new Dictionary<int, int?>();

            Year.Add(YearParam, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }


            if (relatorio.Parametros.ContainsKey("Ano"))
            {
                var param1 = relatorio.Parametros["Ano"];
                YearParam = int.Parse(param1);


            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Year == YearParam && x.TipoRegistro == tpRegistro && x.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada).ToList();

            Year[YearParam] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Year[YearParam].Value
            };

            List<BarSeriesData> MesesData = new List<BarSeriesData>();

            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(YearParam.ToString());

            ViewData["FechadasAno"] = MesesData;
            ViewData["Categories"] = Categories;

            return View("FechadasAno", relatorio);
        }
        public ViewResult Fechadas(Relatorio relatorio)
        {
            DateTime monthParam = new DateTime();

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Meses = new Dictionary<int, int?>();

            Meses.Add(monthParam.Month, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }

            if (relatorio.Parametros.ContainsKey("Mês"))
            {
                var param1 = "01/" + relatorio.Parametros["Mês"];
                monthParam = Convert.ToDateTime(param1);

            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Month == monthParam.Month && x.DtInclusao.Year == monthParam.Year && x.TipoRegistro == tpRegistro && x.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada).ToList();

            Meses[monthParam.Month] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Meses[monthParam.Month].Value
            };

            List<BarSeriesData> MesesData = new List<BarSeriesData>();

            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthParam.Month));

            ViewData["FechadasMes"] = MesesData;
            ViewData["Categories"] = Categories;

            return View("Fechadas", relatorio);
        }
    }
}