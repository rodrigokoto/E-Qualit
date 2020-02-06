using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Repository;

namespace ApplicationService.Servico
{
    public class LicencaAppServico : BaseServico<Licenca>, ILicencaAppServico
    {
        private readonly ILicencaRepositorio _licencaRepositorio;

        public LicencaAppServico(ILicencaRepositorio licencaRepositorio) : base(licencaRepositorio)
        {
        }
    }
}
