using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;
using Dominio.Servico;

namespace ApplicationService.Servico
{
    public class CargoAppServico : BaseServico<Cargo>, ICargoAppServico
    {
        private readonly ICargoRepositorio _cargoRepositorio;
        private readonly ISiteModuloRepositorio _siteModuloRepositorio;
        private readonly IFuncionalidadeRepositorio _moduloRepositorio;
        private readonly ICargoProcessoRepositorio _cargoProcessoRepositorio;

        public CargoAppServico(ICargoRepositorio cargoRepositorio, 
                            ISiteModuloRepositorio siteModuloRepositorio,
                            IFuncionalidadeRepositorio moduloRepositorio,
                            ICargoProcessoRepositorio cargoProcessoRepositorio) : base(cargoRepositorio)
        {
            _cargoRepositorio = cargoRepositorio;
            _siteModuloRepositorio = siteModuloRepositorio;
            _moduloRepositorio = moduloRepositorio;
            _cargoProcessoRepositorio = cargoProcessoRepositorio;
        }

        public List<Funcionalidade> ObtemModulosPermitidos(int idCargo)
        {
            var moduloServico = new FuncionalidadeAppServico(_moduloRepositorio);
            var modulos = new List<Funcionalidade>();
            var sites = new List<SiteFuncionalidade>();
            var cargo = _cargoRepositorio.GetById(idCargo);

            if (UtilsServico.EstaPreenchido(cargo))
            {
                sites = _siteModuloRepositorio.ListarSiteModuloPorSite(cargo.IdSite);
            }

            var funcionalidades = moduloServico.CriarFuncionalidadesPorSiteModulos(sites);

            return funcionalidades;
        }

        public List<Cargo> ObtemCargosPorSiteEProcesso(int idSite, int idProcesso)
        {
            var cargos = new List<Cargo>();

            var cargosCTX = _cargoRepositorio.Get(x => x.IdSite == idSite && x.CargoProcessos.Where(s => s.IdProcesso == idProcesso).Count() > 0);

            foreach (var cargoCTX in cargosCTX)
            {
                cargos.Add(new Cargo
                {
                    IdCargo = cargoCTX.IdCargo,
                    NmNome = cargoCTX.NmNome
                });
            }

            return cargos;
        }

        public List<Cargo> ObtemCargosPorSite(int idSite)
        {
            return _cargoRepositorio.Get(x => x.IdSite == idSite).ToList();
        }

        public bool AtivarInativar(int id)
        {
            try
            {
                var cargo = _cargoRepositorio.GetById(id);

                cargo.Ativo = !cargo.Ativo;
                _cargoRepositorio.Update(cargo);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
