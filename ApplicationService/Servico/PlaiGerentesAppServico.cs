using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class PlaiGerentesAppServico : BaseServico<PlaiGerentes>, IPlaiGerentesAppServico
    {
        private readonly IPlaiGerentesRepositorio _plaiGerentesAppServico;

        public PlaiGerentesAppServico(IPlaiGerentesRepositorio plaiGerentesAppServico) : base(plaiGerentesAppServico)
        {
            _plaiGerentesAppServico = plaiGerentesAppServico;
        }

    }
}
