using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace ApplicationService.Servico
{
    public class CargoProcessoAppServico : BaseServico<CargoProcesso>, ICargoProcessoAppServico
    {
        private readonly ICargoProcessoRepositorio _cargoProcessoRepositorio;
        private readonly IUsuarioCargoRepositorio _usuarioCargoRepositorio;

        public CargoProcessoAppServico(ICargoProcessoRepositorio cargoProcessoRepositorio,
            IUsuarioCargoRepositorio usuarioCargoRepositorio
            ) : base(cargoProcessoRepositorio)
        {
            _cargoProcessoRepositorio = cargoProcessoRepositorio;
            _usuarioCargoRepositorio = usuarioCargoRepositorio;
        }

        public bool PossuiAcessoAFuncao(int idUsuarioLogado, int idFuncao, int ? idProcessoSelecionado = null)
        {
            var cargoProcessos = _usuarioCargoRepositorio.Get(x=>x.IdUsuario == idUsuarioLogado).FirstOrDefault();

            return cargoProcessos.Cargo.CargoProcessos.Any(x=>x.IdFuncao == idFuncao && (x.IdProcesso == idProcessoSelecionado || idProcessoSelecionado == null));
        }
    }
}
