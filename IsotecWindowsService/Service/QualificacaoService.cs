using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Servico;
using IsotecWindowsService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsotecWindowsService.Service
{
    public class QualificacaoService : IQualificacaoService
    {
        private readonly IAvaliaCriterioAvaliacaoAppServico _avaliaCriterioAvaliacaoAppServico;
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IRegistroQualificacaoServico _registroQualificacaoServico;
        private readonly IFornecedorAppServico _fornecedorAppServico;
        private readonly IProdutoAppServico _produtoAppServico;
        private readonly IProdutoFornecedorAppServico _produtoFornecedorAppServico;


        public QualificacaoService(IAvaliaCriterioAvaliacaoAppServico avaliaCriterioAvaliacaoAppServico,
            IFilaEnvioServico filaEnvioServico,
            IUsuarioAppServico usuarioAppServico,
            IRegistroQualificacaoServico registroQualificacaoServico,
            IFornecedorAppServico fornecedorAppServico,
            IProdutoAppServico produtoAppServico,
            IProdutoFornecedorAppServico produtoFornecedorAppServico)
        {

            _avaliaCriterioAvaliacaoAppServico = avaliaCriterioAvaliacaoAppServico;
            _filaEnvioServico = filaEnvioServico;
            _usuarioAppServico = usuarioAppServico;
            _registroQualificacaoServico = registroQualificacaoServico;
            _fornecedorAppServico = fornecedorAppServico;
            _produtoAppServico = produtoAppServico;
            _produtoFornecedorAppServico = produtoFornecedorAppServico;
        }

        public void EnfileirarEmail()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Auditoria-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

                var data30d = DateTime.Now.AddDays(30);
                data30d = new DateTime(data30d.Year, data30d.Month, data30d.Day, 0, 0, 0);

                var AgendarHoje = _avaliaCriterioAvaliacaoAppServico.Get().Where(x => x.DtProximaAvaliacao.Date == DateTime.Now.Date).ToList();
                var Agendar30D = _avaliaCriterioAvaliacaoAppServico.Get(x => x.DtProximaAvaliacao == data30d).ToList();

                var avaHj = AgendarHoje.Select(x => new { x.IdFornecedor, x.DtProximaAvaliacao, x.IdUsuarioAvaliacao , x.GuidAvaliacao }).Distinct().ToList();

                var ava30d = Agendar30D.Select(x => new { x.IdFornecedor, x.DtProximaAvaliacao, x.IdUsuarioAvaliacao, x.GuidAvaliacao }).Distinct().ToList();


                List<Avaliacao> lstAvaliacao = new List<Avaliacao>();

                foreach (var avaliacao in avaHj)
                {
                    var ava = new Avaliacao();

                    ava.IdFornecedor = avaliacao.IdFornecedor;
                    ava.DtProximaAvaliacao = avaliacao.DtProximaAvaliacao;
                    ava.IdUsuarioAvaliacao = (int)avaliacao.IdUsuarioAvaliacao;
                    ava.GuidAvaliacao = avaliacao.GuidAvaliacao;

                    lstAvaliacao.Add(ava);
                }


                List<Avaliacao> lstAvaliacao30d = new List<Avaliacao>();

                foreach (var avaliacao in ava30d)
                {
                    var ava = new Avaliacao();

                    ava.IdFornecedor = avaliacao.IdFornecedor;
                    ava.DtProximaAvaliacao = avaliacao.DtProximaAvaliacao;
                    ava.IdUsuarioAvaliacao = (int)avaliacao.IdUsuarioAvaliacao;
                    ava.GuidAvaliacao = avaliacao.GuidAvaliacao;

                    lstAvaliacao30d.Add(ava);
                }


                AgendaEmail(ValidaAgendamento(lstAvaliacao), DateTime.Now);
                AgendaEmail(ValidaAgendamento(lstAvaliacao30d), data30d);

            }
            catch (Exception ex)
            {
                FileLogger.Log("Erro ao enfileirar os e-mails", ex);
            }

        }

        public List<Avaliacao> ValidaAgendamento(List<Avaliacao> avaliaCriterioAvaliacaos)
        {


            var retorno = new List<Avaliacao>();

            retorno.AddRange(avaliaCriterioAvaliacaos);

            foreach (var item in retorno)
            {
                var reg = _registroQualificacaoServico.RetornaRegistro(item.GuidAvaliacao);

                if (reg != null)
                {
                    if ((DateTime)reg.DtInclusao.Value.Date == DateTime.Now.Date)
                    {
                        avaliaCriterioAvaliacaos.Remove(item);
                    }
                }
            }

            return avaliaCriterioAvaliacaos;
        }

        public void AgendaEmail(IEnumerable<Avaliacao> ListEmail, DateTime date)
        {
            if (date == DateTime.Now)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Qualificacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

                EnfileirarEmail(ListEmail, path);
            }
            else
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Qualificacao30d-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

                EnfileirarEmail(ListEmail, path);

            }
        }

        public void EnfileirarEmail(IEnumerable<Avaliacao> ListEmail, string path)
        {

            string template = System.IO.File.ReadAllText(path);

            string conteudo = template;

            foreach (var critAva in ListEmail)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("<tr><td>{0}</td></tr>", _fornecedorAppServico.GetById(critAva.IdFornecedor).Nome);

                ProdutoFornecedor relation = _produtoFornecedorAppServico.Get().Where(x => x.IdFornecedor == critAva.IdFornecedor).FirstOrDefault();

                var Responsavel = _usuarioAppServico.GetById((int)critAva.IdUsuarioAvaliacao);

                conteudo = conteudo.Replace("#Responsavel#", Responsavel.NmCompleto);
                conteudo = conteudo.Replace("#Produto#", relation.Produto.Nome);
                conteudo = conteudo.Replace("#Fornecedor#", relation.Fornecedor.Nome);

                FilaEnvio filaEnvio = new FilaEnvio();

                filaEnvio.Assunto = "Agendamento Qualificação de Fornecedor";
                filaEnvio.DataAgendado = DateTime.Now;
                filaEnvio.DataInclusao = DateTime.Now;
                filaEnvio.Destinatario = Responsavel.CdIdentificacao;
                filaEnvio.Enviado = false;
                filaEnvio.Mensagem = conteudo;

                _filaEnvioServico.Enfileirar(filaEnvio);

                RegistroQualificacao registro = new RegistroQualificacao();

                registro.GuidAvaliacao = critAva.GuidAvaliacao;
                registro.IdFilaEnvio = filaEnvio.Id;
                registro.IdFornecedor = critAva.IdFornecedor;
                registro.DtInclusao = DateTime.Now;
                registro.IdUsuarioAvaliacao = (int)critAva.IdUsuarioAvaliacao;
                registro.IdUsuarioInclusao = (int)critAva.IdUsuarioAvaliacao;

                _registroQualificacaoServico.InserirEmail(registro);
            }
        }
    }
}
