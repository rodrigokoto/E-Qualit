using Entidade;
using Servico.Validacao.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Servico.Validacao
{
    public class ModuloValidacao : IBaseValidacao<Funcionalidade>
    {
        private List<ValidationResult> erros = new List<ValidationResult>();

        public List<ValidationResult> ModuloValido(List<Funcionalidade> modulos)
        {
            var erroOrigem = new List<string>();
            
            if (!PossuiModuloSelecionado(modulos))
            {
                erroOrigem.Add("");
                erros.Add(new ValidationResult("Deve obrigatóriamente possuir um módulo.", erroOrigem));
            }

            return erros;
        }

        public bool PossuiModuloSelecionado(List<Funcionalidade> modulos)
        {
            foreach (var modulo in modulos)
            {
                if (modulo.FlSelecionado)
                {
                    return true;
                }
            }

            return false;
        }

        public bool PossuiPermissaoModulo(List<PermissaoModulo> modulos)
        {
            if (modulos == null || modulos.Count == 0)
            {
                return false;
            }
            return true;
        }

        public void ObjetoValido(Funcionalidade modulo)
        {
            if (!modulo.IsValid())
            {
                erros.AddRange(modulo.ModelErros);
            }
        }
    }
}
