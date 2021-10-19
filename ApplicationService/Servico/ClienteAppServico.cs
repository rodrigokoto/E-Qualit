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
        private readonly IUsuarioClienteSiteRepositorio _usuarioClienteSiteRepositorio;
        private readonly ISiteRepositorio _siteRepositorio;
        private readonly IFilaEnvioRepositorio _filaEnvio;

        public ClienteAppServico(IClienteRepositorio clienteRepositorio,
                                 IUsuarioRepositorio usuarioRepositorio,
                                 IUsuarioClienteSiteRepositorio usuarioClienteSiteRepositorio,
                                 ISiteRepositorio siteRepositorio,
                                 IFilaEnvioRepositorio filaEnvioRepositorio) :
            base(clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _usuarioClienteSiteRepositorio = usuarioClienteSiteRepositorio;
            _siteRepositorio = siteRepositorio;
            _filaEnvio = filaEnvioRepositorio;
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

                var UsuarioCliente = _usuarioClienteSiteRepositorio.Get(x => x.IdCliente == cliente.IdCliente).ToList();


                foreach (var usuariocliente in UsuarioCliente)
                {
                    var usuario = _usuarioRepositorio.GetById(usuariocliente.IdUsuario);

                    //Altera Usuaruios Ativo / Inativo
                    usuario.FlAtivo = !cliente.FlAtivo;
                    _usuarioRepositorio.Update(usuario);

                    var Fila = _filaEnvio.Get(x => x.Destinatario == usuario.CdIdentificacao).ToList();

                    foreach (var email in Fila)
                    {
                        _filaEnvio.Remove(email);
                    }
                }

                var siteId = (int)UsuarioCliente.FirstOrDefault().IdSite;

                var ClienteSite = _siteRepositorio.GetById(siteId);

                //Altera Site Ativo / Inativo
                ClienteSite.FlAtivo = !cliente.FlAtivo;
                _siteRepositorio.Update(ClienteSite);


                //Altera Cliente Ativo / Inativo
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
