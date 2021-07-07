using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Servico
{
    public class RegistroAcaoImediataServico : IRegistroAcaoImediataServico
    {
        private readonly IRegistroAcaoImediataRepositorio _repositorio;

        public RegistroAcaoImediataServico(IRegistroAcaoImediataRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public void Add(RegistroAcaoImediata registroAcaoImediata)
        {
            _repositorio.Add(registroAcaoImediata);
        }

        public void Update(RegistroAcaoImediata registroAcaoImediata)
        {
            _repositorio.Update(registroAcaoImediata);
            
        }
        public RegistroAcaoImediata GetById(int idRegistroAcaoImediata)
        {
            return _repositorio.GetById(idRegistroAcaoImediata);

        }

        public void Remove(RegistroAcaoImediata registroAcaoImediata)
        {
            _repositorio.Remove(registroAcaoImediata);
        }
    }
}
