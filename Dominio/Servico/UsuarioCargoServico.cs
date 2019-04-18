using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;

namespace Dominio.Servico
{
    public class UsuarioCargoServico : IUsuarioCargoServico
    {
        private readonly IUsuarioCargoRepositorio _usuarioCargoRepositorio;

        public UsuarioCargoServico(IUsuarioCargoRepositorio usuarioCargoRepositorio) 
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
                cargosConcatenados += "," + usuario.Cargo.NmNome.ToString();
            }

            return cargosConcatenados;
        }
    }
}
