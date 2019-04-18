using Dominio.Entidade;
using Dominio.Especificacao;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Cargos;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class CargoServico : ICargoServico
    {
        private readonly ICargoRepositorio _cargoRepositorio;
        private readonly ISiteModuloRepositorio _siteModuloRepositorio;
        private readonly IFuncionalidadeRepositorio _moduloRepositorio;
        private readonly ICargoProcessoRepositorio _cargoProcessoRepositorio;

        public CargoServico(ICargoRepositorio cargoRepositorio, 
                            ISiteModuloRepositorio siteModuloRepositorio,
                            IFuncionalidadeRepositorio moduloRepositorio,
                            ICargoProcessoRepositorio cargoProcessoRepositorio) 
        {
            _cargoRepositorio = cargoRepositorio;
            _siteModuloRepositorio = siteModuloRepositorio;
            _moduloRepositorio = moduloRepositorio;
            _cargoProcessoRepositorio = cargoProcessoRepositorio;
        }

        public List<Funcionalidade> ObtemModulosPermitidos(int idCargo)
        {
            var moduloServico = new FuncionalidadeServico(_moduloRepositorio);
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

        public void Valida(Cargo cargo, ref List<string> erros)
        {
            var validadorCampos = new AptoParaCadastroValidation()
                                    .Validate(cargo);

            if (!validadorCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validadorCampos.Errors));
            }

            //cargo.ValidationResult = new DeveTerNomeUnicoValidation(_cargoRepositorio)
            //                                .Validate(cargo);

            //if (!cargo.ValidationResult.IsValid)
            //{
            //    erros.AddRange(UtilsServico.PopularErros(cargo.ValidationResult));               
            //}

        }

        public bool Excluir(int id)
        {
            return _cargoRepositorio.Excluir(id);
        }
    }
}
