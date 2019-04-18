﻿using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Plais;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class PlaiServico : IPlaiServico
    {
        private readonly IPaiRepositorio _paiRepositorio;
        private readonly IPlaiRepositorio _plaiRepositorio;
        private readonly ILogRepositorio _logRepositorio;
        private readonly INotificacaoMensagemServico _notificacaoMensagemServico;
        private readonly INotificacaoMensagemRepositorio _notificacaoMensagemRepositorio;

        public PlaiServico(IPlaiRepositorio plaiRepositorio, IPaiRepositorio paiRepositorio, 
                          ILogRepositorio logRepositorio, INotificacaoMensagemServico notificacaoMensagemServico) 
        {
            _plaiRepositorio = plaiRepositorio;
            _paiRepositorio = paiRepositorio;
            _logRepositorio = logRepositorio;
            _notificacaoMensagemServico = notificacaoMensagemServico;
        }

        public void Valido(Plai plai, ref List<string> erros)
        {
            var validacao = new AptoParaCadastroValidacao().Validate(plai);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }

        public void Atualizar(Plai plai)
        {
            if (PossuiDocumento(plai.Arquivo.Arquivo))
            {
                plai.Bloqueado = true;
            }
            else
            {
                plai.Bloqueado = false;
            }

            _plaiRepositorio.Update(plai);
        }

        private bool PossuiDocumento(byte[] arquivo)
        {
            if (arquivo.Length == 0)
            {
                return false;
            }

            return true;
        }

        public List<Plai> ObterPorPai(int idPai)
        {
            var plais = _plaiRepositorio.Get(x => x.IdPai == idPai).ToList();

            return plais;
        }

        public Plai ObterPorPaiMesAno(int idPai, int mes, int ano)
        {
            return _plaiRepositorio.Get(x => x.IdPai == idPai && x.Mes == mes).FirstOrDefault();
        }

        public void InsereMensagemPlaisVencidos()
        {
            var plais = Expira15Dias();
            
            foreach (var plai in plais)
            {
                NotificacaoMensagem _registro = new NotificacaoMensagem()
                {
                    DsAssunto = "Expirado",
                    DsMensagem = "O PLAI do mês :" + plai.Mes + "está para expirar.",
                    DtCadastro = DateTime.Now,
                    DtEnvio = DateTime.Now,
                    FlEnviada = false,
                    IdNotificacaoMenssagem = 0,
                    IdSite = 5,
                    IdSmtpNotificacao = 4,
                    NmEmailNome = plai.Pai.Usuario.NmCompleto,
                    NmEmailPara = plai.Pai.Usuario.CdIdentificacao
                };

                _notificacaoMensagemRepositorio.Add(_registro);
            }

            AtualizarEmailEnviados(plais);
        }


        public List<Plai> Expira15Dias()
        {
            var anoCorrente = DateTime.Now.Year;
            var mesCorrente = DateTime.Now.AddDays(15).Month;

            var pai = _paiRepositorio.Get(x => x.Ano == anoCorrente).FirstOrDefault();

            pai.Plais = new List<Plai>();

            pai.Plais.AddRange(_plaiRepositorio.Get(x => x.IdPai == pai.IdPai &&
                                                    x.Mes == mesCorrente &&
                                                    x.EnviouEmail == false &&
                                                    x.Agendado == false));

            return pai.Plais;
        }

        public void AtualizarEmailEnviados(List<Plai> plais)
        {
            try
            {
                plais.ForEach(x => x.EnviouEmail = true);

                plais.ForEach(x => _plaiRepositorio.Update(x));
            }
            catch (Exception ex)
            {
                var log = new Log(Convert.ToInt32(Acao.AtualizarEmailEnviado), ex);
                _logRepositorio.Add(log);
            }

        }
    }
}
