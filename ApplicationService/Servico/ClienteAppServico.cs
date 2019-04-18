using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class ClienteAppServico : BaseServico<Cliente>, IClienteAppServico
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public ClienteAppServico(IClienteRepositorio clienteRepositorio, IUsuarioRepositorio usuarioRepositorio) : 
            base(clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Cliente ObterPorUrl(string url)
        {
            try
            {
                return _clienteRepositorio.Get(x => x.NmUrlAcesso.Equals(url)).FirstOrDefault();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public IEnumerable<Cliente> ObterClientesPorUsuario(int idUsuario)
        {
            var usuario = _usuarioRepositorio.GetById(idUsuario);

            if (EAdministrador(usuario.IdPerfil))
            {
                return _clienteRepositorio.GetAll();
            }

            else if (ECoordenadorOuSuporte(usuario.IdPerfil))
            {
                return _clienteRepositorio.ListarClientesPorUsuario(idUsuario);
            }            
            else
            {
                return _clienteRepositorio.ListarClientesPorUsuario(idUsuario).Take(1).ToList();
            }            
        }

        private bool EAdministrador(int perfil)
        {
            if (perfil == (int)PerfisAcesso.Administrador)
                return true;

            return false;
        }

        private bool ECoordenadorOuSuporte(int perfil)
        {
            if (perfil == (int)PerfisAcesso.Coordenador || perfil == (int)PerfisAcesso.Suporte)
                return true;

            return false;
        }

        public bool AtivarInativar(int id)
        {
            try
            {
                var cliente = _clienteRepositorio.GetById(id);

                cliente.FlAtivo = !cliente.FlAtivo;
                _clienteRepositorio.Update(cliente);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
