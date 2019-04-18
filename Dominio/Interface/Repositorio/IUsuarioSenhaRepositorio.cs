using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interface.Repositorio
{
    public interface IUsuarioSenhaRepositorio : IBaseRepositorio<UsuarioSenha>
    {
        //IEnumerable<UsuarioSenha> ListaUltimasSenhas(int IdUsuario, int Quantidade);
    }
}
