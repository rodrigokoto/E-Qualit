using ApplicationService.Interface;
using Dominio.Entidade;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Http;

namespace Web.API.Controllers
{
    [RoutePrefix("api/Indicador")]
    public class IndicadorController : ApiController
    {
        private readonly IIndicadorAppServico _servicoIndicador;

        public IndicadorController(IIndicadorAppServico servicoIndicador)
        {
            _servicoIndicador = servicoIndicador;
        }

        /// <summary>
        /// Obtem todos os indicadores
        /// </summary>
        /// <param name="indicador">Traz todos indicadores</param>
        /// <remarks>Traz todos indicadores com suas dependencias</remarks>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        ///  [AllowAnonymous]
        [Route("GET")]
        [HttpGet]
        public String Get()
        {
            var indicadores = _servicoIndicador.GetAll();

            //// Serializer settings
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            //settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //settings.Formatting = Formatting.Indented;

            //// Do the serialization and output to the console
            //string json = JsonConvert.SerializeObject(indicadores, settings);
            //Console.WriteLine(json);


            //List<Indicador> indicadorRetorno = new List<Indicador>();

            //foreach (var indicador in indicadores)
            //{
            //    var semReferenciaCircular = new Indicador();

            //    semReferenciaCircular.Id = indicador.Id;


            //    indicadorRetorno.Add(semReferenciaCircular);
            //}

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            string json = JsonConvert.SerializeObject(indicadores.ToList(), Formatting.Indented, settings);

            return json;
        }

        // POST: api/Indicadores
        /// <summary>
        /// Registra um novo indicador na aplicação
        /// </summary>
        /// <param name="indicador">Novo indicador para registrar</param>
        /// <remarks>Adiciona um indicador com seus devidos objetos filhos</remarks>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [AllowAnonymous]
        public void Post(Indicador indicador)
        {
            //_servicoIndicador.Add(indicador);
        }

    }
}
