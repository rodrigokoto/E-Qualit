using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;
using Dominio.Servico;
using System;

namespace ApplicationService.Servico
{
    public class AnaliseCriticaAppServico : BaseServico<AnaliseCritica>, IAnaliseCriticaAppServico
    {

        private readonly IAnaliseCriticaRepositorio _analiseCriticaRepositorio;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteSiteServico;
        private readonly IAnaliseCriticaTemaRepositorio _analiseCriticaTemaRepositorio;

        public AnaliseCriticaAppServico(IAnaliseCriticaRepositorio analiseCriticaRepositorio,
                                     IUsuarioClienteSiteAppServico usuarioClienteSiteServico,
                                     IAnaliseCriticaTemaRepositorio analiseCriticaTemaRepositorio) : base(analiseCriticaRepositorio)
        {
            _analiseCriticaRepositorio = analiseCriticaRepositorio;
            _usuarioClienteSiteServico = usuarioClienteSiteServico;
            _analiseCriticaTemaRepositorio = analiseCriticaTemaRepositorio;
        }

        public List<AnaliseCritica> ObterPorIdSite(int idSite)
        {
            return _analiseCriticaRepositorio.Get(x => x.IdSite == idSite).ToList();
        }

        public List<AnaliseCritica> ListaTodosAtivos()
        {
            var analiseCriticas = new List<AnaliseCritica>();
      
                analiseCriticas.AddRange(_analiseCriticaRepositorio.Get(x => x.Ativo == true));

            return analiseCriticas;
        }

        public List<Usuario> ObterUsuariosPorAnaliseCritica(int idAnaliseCritica, int idSite)
        {
            var usuarios = new List<Usuario>();

            var analiseCritiva = GetById(idAnaliseCritica);

            usuarios.AddRange(_usuarioClienteSiteServico.ListarPorEmpresa(idSite));

            if (UtilsServico.EstaPreenchido(analiseCritiva))
            {
                foreach (var usuario in analiseCritiva.Funcionarios)
                {
                    usuarios.Add(usuario.Funcionario);
                }
            }

            return UsuarioAppServico.RetiraDuplicado(usuarios);
        }

        public void SalvarAnaliseCritica(AnaliseCritica analiseCritica)
        {
            _analiseCriticaRepositorio.SalvarAnaliseCritica(analiseCritica);
        }

        public void AtualizaAnaliseCritica(AnaliseCritica analiseCritica)
        {
            _analiseCriticaRepositorio.AtualizaAnaliseCriticaTema(analiseCritica);
            _analiseCriticaRepositorio.AtualizaAnaliseCriticaFuncionario(analiseCritica);
            //_analiseCriticaRepositorio.Update(analiseCritica);
        }
    }
}
