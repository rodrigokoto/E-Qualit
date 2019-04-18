using Entidade;
using Servico.Validacao.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Servico.Validacao
{
    public class CargoValidacao : IBaseValidacao<Cargo>
    {
        private ProcessoValidacao _validaProcesso = new ProcessoValidacao();
        private ModuloValidacao _validaModulo = new ModuloValidacao();
        private FuncaoValidacao _validaFuncao = new FuncaoValidacao();
        private List<ValidationResult> erros = new List<ValidationResult>();


        public List<ValidationResult> CargoValido(Cargo cargo)
        {
            var erroOrigem = new List<string>();

            ObjetoValido(cargo);

            if (!_validaProcesso.PossuiPermissaoProcesso(cargo.LvPermissao))
            {
                erroOrigem.Add("Permissão Processo");
                erros.Add(new ValidationResult("O cadastro não pode ser realizado sem um processo cadastrado", erroOrigem));
            }
            else
            {
                foreach (PermissaoProcesso processo in cargo.LvPermissao)
                {
                    if (!_validaModulo.PossuiPermissaoModulo(processo.LvModulos))
                    {
                        erroOrigem.Add("Modulo");
                        erros.Add(new ValidationResult("O processo: " + processo.NmProcesso + " necessita de pelo menos um módulo", erroOrigem));
                    }
                    else
                    {
                        foreach (var modulo in processo.LvModulos)
                        {
                            if (!_validaFuncao.PossuiFuncao(modulo.LvFuncao))
                            {
                                erroOrigem.Add("Função");
                                erros.Add(new ValidationResult("O módulo : " + modulo.NmModulo + " necessita de pelo menos uma função", erroOrigem));
                            }
                        }
                    }
                }
            }

            return erros;
        }

        public void ObjetoValido(Cargo cargo)
        {
            if (!cargo.IsValid())
            {
                erros.AddRange(cargo.ModelErros);
            }
        }
    }
}
