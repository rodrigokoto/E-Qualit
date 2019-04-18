
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Clientes;
using Dominio.Validacao.Clientes.View;
using Dominio.Validacao.Sites.View;
using Dominio.Validacao.Usuarios.View;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class ClienteServico : IClienteServico
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISiteRepositorio _siteRepositorio;
        private readonly ISubModuloRepositorio _subModuloRepositorio;
        private readonly IClienteContratoRepositorio _ClienteContratoRepositorio;
        private readonly IClienteLogoRepositorio _ClienteLogoRepositorio;
        private readonly ISiteAnexoRepositorio _SiteAnexoRepositorio;
        private readonly ISiteModuloRepositorio _SiteModuloRepositorio;
        private readonly ICargoRepositorio _CargoRepositorio;
        private readonly IUsuarioCargoRepositorio _UsuarioCargoRepositorio;
        private readonly ICargoProcessoRepositorio _CargoProcessoRepositorio;
        private readonly IUsuarioClienteSiteRepositorio _UsuarioClienteSiteRepositorio;
        private readonly IUsuarioAnexoRepositorio _UsuarioAnexoRepositorio;
        private readonly IAnexoRepositorio _AnexoRepositorio;


        public ClienteServico(IClienteRepositorio clienteRepositorio, IUsuarioRepositorio usuarioRepositorio, ISiteRepositorio siteRepositorio, ISubModuloRepositorio subModuloRepositorio, IClienteContratoRepositorio ClienteContratoRepositorio, IClienteLogoRepositorio ClienteLogoRepositorio, ISiteAnexoRepositorio SiteAnexoRepositorio, ISiteModuloRepositorio SiteModuloRepositorio, ICargoRepositorio CargoRepositorio, IUsuarioCargoRepositorio UsuarioCargoRepositorio, ICargoProcessoRepositorio CargoProcessoRepositorio, IUsuarioClienteSiteRepositorio UsuarioClienteSiteRepositorio, IUsuarioAnexoRepositorio UsuarioAnexoRepositorio, IAnexoRepositorio anexoRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _siteRepositorio = siteRepositorio;
            _subModuloRepositorio = subModuloRepositorio;
            _ClienteContratoRepositorio = ClienteContratoRepositorio;
            _ClienteLogoRepositorio = ClienteLogoRepositorio;
            _SiteAnexoRepositorio = SiteAnexoRepositorio;
            _SiteModuloRepositorio = SiteModuloRepositorio;
            _CargoRepositorio = CargoRepositorio;
            _UsuarioCargoRepositorio = UsuarioCargoRepositorio; 
            _CargoProcessoRepositorio = CargoProcessoRepositorio;
            _UsuarioClienteSiteRepositorio = UsuarioClienteSiteRepositorio;
            _UsuarioAnexoRepositorio = UsuarioAnexoRepositorio;
            _AnexoRepositorio = anexoRepositorio;
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

        public bool Excluir(int id)
        {
            return _clienteRepositorio.Excluir(id);
        }

        public void ValidaCriacao(Cliente cliente, ref List<string> erros)
        {
            ValidarCamposCriacaoView(cliente, ref erros);

            if (erros.Count == 0)           
            {
                ValidarRegrasNegocioCriacao(cliente, ref erros);
            }

        }

        private void ValidarCamposCriacaoView(Cliente cliente, ref List<string> erros)
        {
            var validaCamposCliente = new CriarClienteViewValidation().Validate(cliente);
            var validaCamposSite = new CriarSiteViewValidation().Validate(cliente.Site);
            var validaCamposUsuario = new CriarUsuarioViewValidation().Validate(cliente.Usuario);

            if (!validaCamposCliente.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCamposCliente.Errors));
            }

            if (!validaCamposSite.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCamposSite.Errors));
            }

            if (!validaCamposUsuario.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCamposUsuario.Errors));
            }
        }

        public Cliente ObjterClienteById(int IdCliente)
        {
            return _clienteRepositorio.Get(x => x.IdCliente == IdCliente).FirstOrDefault();
        }

        private void ValidarRegrasNegocioCriacao(Cliente cliente, ref List<string> erros)
        {
            cliente.Usuario.ValidationResult = new Validacao.Usuarios.AptoParaCadastroValidation(_usuarioRepositorio).Validate(cliente.Usuario);

            if (!cliente.Usuario.ValidationResult.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(cliente.Usuario.ValidationResult));
            }
        }

        public void ValidaEdicao(Cliente cliente, ref List<string> erros)
        {
            var validaCampos = new EdiarClienteViewValidation().Validate(cliente);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
            else
            {
                cliente.ValidationResult = new AptoParaEdicaoValidation(_clienteRepositorio).Validate(cliente);

                if (!cliente.ValidationResult.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(cliente.ValidationResult));
                }
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
    }
}
