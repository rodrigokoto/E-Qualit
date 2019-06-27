using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Usuarios;
using Dominio.Validacao.Usuarios.MeusDadosView;
using Dominio.Validacao.Usuarios.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class UsuarioServico :  IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUsuarioCargoRepositorio _usuarioCargoRepositorio;
        private readonly IUsuarioClienteSiteRepositorio _usuarioClienteSiteRepositorio;
        private readonly ICargoProcessoRepositorio _cargoProcessoRepositorio;
        private long _administrador = (int)PerfisAcesso.Administrador;
        private long _suporte = (int)PerfisAcesso.Suporte;
        private long _coordenador = (int)PerfisAcesso.Coordenador;
        private long _colaborador = (int)PerfisAcesso.Colaborador;


        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio,
                              IUsuarioCargoRepositorio usuarioCargoRepositorio,
                              IUsuarioClienteSiteRepositorio usuarioClienteSiteRepositorio,
                              ICargoProcessoRepositorio cargoProcessoRepositorio) 
        {
            _usuarioRepositorio = usuarioRepositorio;
            _usuarioCargoRepositorio = usuarioCargoRepositorio;
            _usuarioClienteSiteRepositorio = usuarioClienteSiteRepositorio;
            _cargoProcessoRepositorio = cargoProcessoRepositorio;
        }

        private List<Usuario> PopularCargoProcesso(IEnumerable<CargoProcesso> cargoProcessos, int idSite)
        {

            var usuarios = new List<Usuario>();

            foreach (var cargoProcesso in cargoProcessos)
            {
                foreach (var usuarioCargo in cargoProcesso.Cargo.UsuarioCargos)
                {
                    usuarios.Add(new Usuario
                    {
                        IdUsuario = usuarioCargo.Usuario.IdUsuario,
                        NmCompleto = usuarioCargo.Usuario.NmCompleto
                    });
                }
            }

            var coordenadores = cargoProcessos.Select(
                                      x => x.Processo.Site.UsuarioClienteSites
                                         .Where(y => y.Usuario.IdPerfil == (byte)PerfisAcesso.Coordenador && y.IdSite == x.Processo.IdSite))
                                         .SelectMany(x => x.Select(b => b.Usuario)).ToList();

            if (coordenadores.Count() == 0)
            {
                coordenadores = _usuarioClienteSiteRepositorio.Get(x => x.IdSite == idSite)
                    .Select(y => y.Usuario).Where(b => b.IdPerfil == (byte)PerfisAcesso.Coordenador).ToList();

            }

            coordenadores.ForEach(coordenador =>
            {
                usuarios.Add(new Usuario
                {
                    IdUsuario = coordenador.IdUsuario,
                    NmCompleto = coordenador.NmCompleto
                });
            });

            usuarios = RetiraDuplicado(usuarios);

            return RetiraDuplicado(usuarios);
        }

        public static List<Usuario> RetiraDuplicado(List<Usuario> usuarios)
        {
            return usuarios.GroupBy(x => x.IdUsuario)
                                 .Select(y => y.First())
                                 .ToList();
        }

        public bool PossuiAcesso(int idUsuario, int idfuncionalidade, int idFuncao)
        {
            try
            {
                var usuarios = new List<Usuario>();

                var cargoProcessos = _cargoProcessoRepositorio.Get(x => x.Funcao.IdFuncionalidade == idfuncionalidade &&
                                                                        x.Funcao.IdFuncao == idFuncao).ToList();

                foreach (var cargoProcesso in cargoProcessos)
                {
                    foreach (var usuarioCargo in cargoProcesso.Cargo.UsuarioCargos.Where(x => x.Usuario.IdUsuario.Equals(idUsuario)))
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = usuarioCargo.Usuario.IdUsuario,
                            NmCompleto = usuarioCargo.Usuario.NmCompleto
                        });
                    }
                }

                if (usuarios.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                //TODO: Adicionar a tratativa para log
            }
            return false;
        }

        public List<Usuario> ObterUsuariosPorPerfil(int idPerfil)
        {
            var usuarios = new List<Usuario>();

            try
            {

                if (EAdministrador(idPerfil))
                {
                    usuarios = _usuarioRepositorio.Get(x => x.IdPerfil == _administrador ||
                                                       x.IdPerfil == _suporte).ToList();
                }
                else
                {
                    usuarios = _usuarioRepositorio.Get(x => x.IdPerfil == _colaborador ||
                                                       x.IdPerfil == _coordenador).ToList();
                }

                return usuarios;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AlterarSenha(int idUsuario, string novaSenha)
        {
            try
            {
                var usuarioAtualizar = _usuarioRepositorio.GetById(idUsuario);

                usuarioAtualizar.CdSenha = UtilsServico.Sha1Hash(novaSenha);
                _usuarioRepositorio.Update(usuarioAtualizar);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PodeAlterarSenha(Usuario entidade, ref List<string> erros)
        {
            var validation = new AlterarSenhaValidation(_usuarioRepositorio);

            var erro = validation.Validate(entidade);

            erros.AddRange(UtilsServico.PopularErros(erro));
        }

        public Usuario ObterUsuarioPorCdIdentificacao(string cdIdentificacao)
        {
            return _usuarioRepositorio.Get(x => x.CdIdentificacao == cdIdentificacao).FirstOrDefault();
        }

        public List<Usuario> ObterUsuariosPorCargo(int idCargo)
        {
            var cargos = _usuarioCargoRepositorio.Get(x => x.IdCargo == idCargo).ToList();
            var usuarios = new List<Usuario>();

            foreach (var cargo in cargos)
            {
                usuarios.Add(cargo.Usuario);
            }

            return usuarios;
        }

        public List<Funcionalidade> ObterFuncionalidadesPermitidas(int idUsuario)
        {
            var listaProcesso = new List<CargoProcesso>();
            var idCargosUsuarios = new List<int>();
            var cargosProcesso = new List<CargoProcesso>();
            var funcionalidades = new List<Funcionalidade>();

            var usuarioCargo = _usuarioCargoRepositorio.Get(x => x.IdUsuario == idUsuario).ToList();

            foreach (var usuario in usuarioCargo)
            {
                foreach (var processo in usuario.Cargo.CargoProcessos)
                {
                    funcionalidades.Add(processo.Funcao.Funcionalidade);
                }
            }

            return funcionalidades.Where(x => x.Ativo == true).Distinct().ToList();
        }

        public bool EAdministrador(int idPerfil)
        {
            if (idPerfil != (int)PerfisAcesso.Administrador)
            {
                return false;
            }
            return true;
        }

        public void Valido(Usuario usuario, ref List<string> erros)
        {
            ValidarCamposView(usuario, ref erros);

            if (erros.Count == 0)
                ValidarRegrasNegocio(usuario, ref erros);

        }

        private void ValidarCamposView(Usuario usuario, ref List<string> erros)
        {
            var validaCampos = usuario.IdUsuario == 0 ?
                new CriarUsuarioViewValidation().Validate(usuario) :
                new EditarUsuarioViewValidation().Validate(usuario);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }

        private void ValidarRegrasNegocio(Usuario usuario, ref List<string> erros)
        {

            if (usuario.IdUsuario == 0)
            {
                usuario.ValidationResult = new AptoParaCadastroValidation(_usuarioRepositorio).Validate(usuario);

                if (!usuario.ValidationResult.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(usuario.ValidationResult));
                }
            }
            else
            {
                usuario.ValidationResult = new AptoParaEditarValidation(_usuarioRepositorio).Validate(usuario);

                if (!usuario.ValidationResult.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(usuario.ValidationResult));
                }
            }

        }

        public void ValidoAtualizarMeusDados(Usuario usuario, ref List<string> erros)
        {
            ValidarCamposMeusDadosView(usuario, ref erros);

            if (erros.Count == 0) { }
            ValidarRegrasNegocio(usuario, ref erros);
        }

        private void ValidarCamposMeusDadosView(Usuario usuario, ref List<string> erros)
        {
            var validaCampos = new EditarUsuarioMeusDadosViewValidation().Validate(usuario);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
        }

        public bool Excluir(int id, int idUsuarioMigracao)
        {
            return _usuarioRepositorio.Excluir(id, idUsuarioMigracao);
        }
    }
}