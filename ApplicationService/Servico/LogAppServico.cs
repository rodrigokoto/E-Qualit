using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class LogAppServico : BaseServico<Log>, ILogAppServico
    {
        private readonly ILogRepositorio _logRepositorio;

        public LogAppServico(ILogRepositorio logRepositorio) : base(logRepositorio)
        {
            _logRepositorio = logRepositorio;
        }
    }
}
