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
            IFilaEnvioServico filaEnvioServico , 
            IUsuarioAppServico usuarioAppServico , 
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


                AgendaEmail(ValidaAgendamento(AgendarHoje));
                AgendarEmail30Dias(ValidaAgendamento(Agendar30D));
            }
            catch (Exception ex)
            {
                FileLogger.Log("Erro ao enfileirar os e-mails", ex);
            }
            
        }

        public List<AvaliaCriterioAvaliacao> ValidaAgendamento(List<AvaliaCriterioAvaliacao> avaliaCriterioAvaliacaos)
        {

            var retorno = new List<AvaliaCriterioAvaliacao>();

            retorno.AddRange(avaliaCriterioAvaliacaos);

            foreach (var item in retorno)
            {
                var reg = _registroQualificacaoServico.RetornaRegistro(item.IdAvaliaCriterioAvaliacao);
                
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

        public void AgendarEmail30Dias(IEnumerable<AvaliaCriterioAvaliacao> ListEmail)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Qualificacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            EnfileirarEmail(ListEmail, path);
        }



        public void AgendaEmail(IEnumerable<AvaliaCriterioAvaliacao> ListEmail)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Qualificacao30d-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            EnfileirarEmail(ListEmail, path);
        }

        public void EnfileirarEmail(IEnumerable<AvaliaCriterioAvaliacao> ListEmail , string path) {

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

                RegistroQualificacao registro = new RegistroQualificacao();

                _filaEnvioServico.Enfileirar(filaEnvio);

                registro.IdAvaliaCriterio = critAva.IdAvaliaCriterioAvaliacao;
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
