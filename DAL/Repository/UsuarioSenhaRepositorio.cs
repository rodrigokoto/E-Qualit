using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UsuarioSenhaRepositorio : BaseRepositorio<UsuarioSenha>, IUsuarioSenhaRepositorio
    {
        //public IEnumerable<UsuarioSenha> ListaUltimasSenhas(int IdUsuario, int Quantidade)
        //{
        //    return _usuarioSenhaRepositorio.ListaUltimasSenhas(IdUsuario, Quantidade);
        //}

    }
}
