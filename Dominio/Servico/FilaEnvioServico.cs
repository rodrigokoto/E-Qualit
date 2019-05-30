using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Servico
{
    public class FilaEnvioServico : IFilaEnvioServico
    {
        private readonly IFilaEnvioRepositorio _filaEnvioRepositorio;


        public FilaEnvioServico(IFilaEnvioRepositorio filaEnvioRepositorio)
        {
            _filaEnvioRepositorio = filaEnvioRepositorio;           
        }

        public void Enfileirar(FilaEnvio filaEnvio)
        {
            _filaEnvioRepositorio.Add(filaEnvio);
        }

    }
}
