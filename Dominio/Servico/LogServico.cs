using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;

namespace Dominio.Servico
{
    public class LogServico :  ILogServico
    {
        private readonly ILogRepositorio _logRepositorio;

        public LogServico(ILogRepositorio logRepositorio) 
        {
            _logRepositorio = logRepositorio;
        }
    }
}
