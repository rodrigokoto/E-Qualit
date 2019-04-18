using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;

namespace ApplicationService.Servico
{
    public class UsuarioCargoAppServico : BaseServico<UsuarioCargo>, IUsuarioCargoAppServico
    {
        private readonly IUsuarioCargoRepositorio _usuarioCargoRepositorio;

        public UsuarioCargoAppServico(IUsuarioCargoRepositorio usuarioCargoRepositorio) : base(usuarioCargoRepositorio)
        {
            _usuarioCargoRepositorio = usuarioCargoRepositorio;
        }

        public List<Usuario> ListarPorEmpresa()
        {
            return new List<Usuario>();
        }

        public string ListarFuncaoConcatenadas(int idUsuario)
        {
            var usuarioCargos = _usuarioCargoRepositorio.ObterPorIdUsuario(idUsuario);
            var cargosConcatenados = string.Empty;

            foreach (var usuario in usuarioCargos)
            {
                if(string.IsNullOrEmpty(cargosConcatenados))
                {
                    cargosConcatenados += usuario.Cargo.NmNome.ToString();
                }
                else
                {
                    cargosConcatenados += "," + usuario.Cargo.NmNome.ToString();
                }
                
            }

            return cargosConcatenados;
        }
    }
}
