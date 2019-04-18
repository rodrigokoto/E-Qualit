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
    public class UsuarioSenhaServico : IUsuarioSenhaServico
    {
        private readonly IUsuarioSenhaRepositorio _usuarioSenhaRepositorio;

        public UsuarioSenhaServico(IUsuarioSenhaRepositorio usuarioSenhaRepositorio)
        {
            _usuarioSenhaRepositorio = usuarioSenhaRepositorio;
           
        }

        public IEnumerable<UsuarioSenha> ListaUltimasSenhas(int IdUsuario, int Quantidade)
        {
            return _usuarioSenhaRepositorio.Get(x=> x.IdUsuario == IdUsuario).OrderByDescending(x=> x.DtInclusaoSenha).Take(Quantidade).ToList();
        }
    }
}
