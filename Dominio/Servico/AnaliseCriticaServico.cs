using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.AnaliseCriticas;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class AnaliseCriticaServico : IAnaliseCriticaServico
    {

        private readonly IAnaliseCriticaRepositorio _analiseCriticaRepositorio;
        private readonly IUsuarioClienteSiteServico _usuarioClienteSiteServico;
        private readonly IAnaliseCriticaTemaRepositorio _analiseCriticaTemaRepositorio;

        public AnaliseCriticaServico(IAnaliseCriticaRepositorio analiseCriticaRepositorio,
                                     IUsuarioClienteSiteServico usuarioClienteSiteServico,
                                     IAnaliseCriticaTemaRepositorio analiseCriticaTemaRepositorio) 
        {
            _analiseCriticaRepositorio = analiseCriticaRepositorio;
            _usuarioClienteSiteServico = usuarioClienteSiteServico;
            _analiseCriticaTemaRepositorio = analiseCriticaTemaRepositorio;
        }

        public void Valido(AnaliseCritica analiseCritica, ref List<string> erros)
        {
            var validacao = new AptoParaCadastroValidation().Validate(analiseCritica);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
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

            var analiseCritiva = _analiseCriticaRepositorio.GetById(idAnaliseCritica);

            usuarios.AddRange(_usuarioClienteSiteServico.ListarPorEmpresa(idSite));

            if (UtilsServico.EstaPreenchido(analiseCritiva))
            {
                foreach (var usuario in analiseCritiva.Funcionarios)
                {
                    usuarios.Add(usuario.Funcionario);
                }
            }

            return UsuarioServico.RetiraDuplicado(usuarios);
        }

        public void Remove(List<AnaliseCritica> analiseCriticas)
        {
          
                foreach (var analiseCritica in analiseCriticas)
                {
                _analiseCriticaRepositorio.Remove(analiseCritica);
                }
       
        }
    }
}
