using Dominio.Interface.Servico;
using System.Web.Http;

namespace Web.API.Controllers
{
    public class TesteController : ApiController
    {
        private readonly IIndicadorServico _servicoIndicador;

        public TesteController(IIndicadorServico servicoIndicador)
        {
            _servicoIndicador = servicoIndicador;
        }

        // GET: api/Teste/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Teste
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Teste/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Teste/5
        public void Delete(int id)
        {
        }
    }
}
