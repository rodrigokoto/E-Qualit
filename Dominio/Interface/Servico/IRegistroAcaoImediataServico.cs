using Dominio.Entidade;
using Dominio.Enumerado;
using System;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IRegistroAcaoImediataServico
    {
       void Add(RegistroAcaoImediata registroAcaoImediata);
       void Update(RegistroAcaoImediata registroAcaoImediata);
    }
}
