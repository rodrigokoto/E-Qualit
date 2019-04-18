using Dominio.Entidade;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using Dominio.Interface.Repositorio;
using Dominio.Validacao.Processos;
using System.Linq;
using Dominio.Validacao.Processos.View;

namespace Dominio.Servico
{
    public class ProcessoServico : IProcessoServico
    {
        private ILogServico _logServico;
        private IProcessoRepositorio _processoRepositorio;
        private ICargoProcessoRepositorio _cargoProcessoRepositorio;
        private IUsuarioCargoRepositorio _usuarioCargoRepositorio;

        public ProcessoServico(ILogServico logServico,
            IProcessoRepositorio processoRepositorio,
            IUsuarioCargoRepositorio usuarioCargoRepositorio,
            ICargoProcessoRepositorio cargoProcessoRepositorio)
        {
            _logServico = logServico;
            _processoRepositorio = processoRepositorio;
            _usuarioCargoRepositorio = usuarioCargoRepositorio;
            _cargoProcessoRepositorio = cargoProcessoRepositorio;
        }

        public void Valido(Processo processo, ref List<string> erros)
        {
            ValidaView(processo, ref erros);
            if (erros.Count == 0)
            {
                ValidaRegraNegocioProcesso(processo, ref erros);
            }
        }

        private void ValidaView(Processo processo, ref List<string> erros)
        {
            var validaCampos = processo.IdProcesso == 0?
                new CriarProcessoViewValidation().Validate(processo):
                new EditarProcessoViewValidation().Validate(processo);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }

        }

        private void ValidaRegraNegocioProcesso(Processo processo, ref List<string> erros)
        {
            processo.ValidationResult = processo.IdProcesso == 0 ? 
                new AptoParaCadastraProcessoValidation(_processoRepositorio).Validate(processo):
                new AptoParaEditarProcessoValidation(_processoRepositorio).Validate(processo);

            if (!processo.ValidationResult.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(processo.ValidationResult));
            }


        }

        public List<Processo> ListaProcessosPorSiteComTracking(int idSite)
        {
            return _processoRepositorio.ListaProcessosPorSite(idSite);
        }

        public List<Processo> ListaProcessosPorSite(int idSite)
        {
            return _processoRepositorio.Get(x => x.IdSite == idSite).ToList();

        }

        public List<Processo> ListaProcessosPorUsuario(int idUsuario)
        {
            var listaProcesso = new List<Processo>();
            var idCargosUsuarios = new List<int>();
            var cargosProcesso = new List<CargoProcesso>();

            var usuarioCargo = _usuarioCargoRepositorio.Get(x => x.IdUsuario == idUsuario).ToList();

            foreach (var usuario in usuarioCargo)
            {
                foreach (var processo in usuario.Cargo.CargoProcessos.Select(x => x.Processo).Distinct())
                {
                    listaProcesso.Add(processo);
                }
            }

            return listaProcesso.Distinct().ToList();
        }

        private bool UsuarioPossuiCargo(List<UsuarioCargo> usuariosCargos)
        {
            if (usuariosCargos != null)
            {
                return true;
            }
            return false;
        }

        public Processo GetProcessoById(int IdProcesso)
        {
            Processo processoOut = _processoRepositorio.GetById(IdProcesso);
            return processoOut;  
        }

    }
}
