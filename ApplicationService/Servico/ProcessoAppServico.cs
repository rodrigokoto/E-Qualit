using Dominio.Entidade;
using ApplicationService.Interface;
using System.Collections.Generic;
using Dominio.Interface.Repositorio;
using System.Linq;
using System;
using Dominio.Enumerado;

namespace ApplicationService.Servico
{
    public class ProcessoAppServico : BaseServico<Processo>, IProcessoAppServico
    {
        private ILogAppServico _logServico;
        private IProcessoRepositorio _processoRepositorio;
        private ICargoProcessoRepositorio _cargoProcessoRepositorio;
        private IUsuarioCargoRepositorio _usuarioCargoRepositorio;

        public ProcessoAppServico(ILogAppServico logServico,
            IProcessoRepositorio processoRepositorio,
            IUsuarioCargoRepositorio usuarioCargoRepositorio,
            ICargoProcessoRepositorio cargoProcessoRepositorio) : base(processoRepositorio)
        {
            _logServico = logServico;
            _processoRepositorio = processoRepositorio;
            _usuarioCargoRepositorio = usuarioCargoRepositorio;
            _cargoProcessoRepositorio = cargoProcessoRepositorio;
        }
             

        public List<Processo> ListaProcessosPorSiteComTracking(int idSite)
        {
            return _processoRepositorio.ListaProcessosPorSite(idSite);
        }
        
        public List<Processo> ListaProcessosPorSite(int idSite)
        {
            return _processoRepositorio.Get(x => x.IdSite == idSite && x.FlAtivo == true).ToList();

        }

        public List<Processo> ListaProcessosPorUsuario(int idUsuario)
        {
            var listaProcesso = new List<Processo>();
            var idCargosUsuarios = new List<int>();
            var cargosProcesso = new List<CargoProcesso>();

            try
            {
                var usuarioCargo = _usuarioCargoRepositorio.Get(x => x.IdUsuario == idUsuario).ToList();
                
                foreach (var usuario in usuarioCargo)
                {
                    foreach (var processo in usuario.Cargo.CargoProcessos.Select(x => x.Processo).Distinct())
                    {
                        if(processo != null)
                        {
                            listaProcesso.Add(processo);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                var log = new Log(Convert.ToInt32(Acao.ListaProcessosPorUsuario), ex);

                _logServico.Add(log);
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

    }
}
