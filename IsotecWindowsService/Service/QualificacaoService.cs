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


        public QualificacaoService(IAvaliaCriterioAvaliacaoAppServico avaliaCriterioAvaliacaoAppServico, 
            IFilaEnvioServico filaEnvioServico , 
            IUsuarioAppServico usuarioAppServico , 
            IRegistroQualificacaoServico registroQualificacaoServico,
            IFornecedorAppServico fornecedorAppServico)
        {

            _avaliaCriterioAvaliacaoAppServico = avaliaCriterioAvaliacaoAppServico;
            _filaEnvioServico = filaEnvioServico;
            _usuarioAppServico = usuarioAppServico;
            _registroQualificacaoServico = registroQualificacaoServico;
            _fornecedorAppServico = fornecedorAppServico;
        }

        public void EnfileirarEmail()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Auditoria-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            var data30d = DateTime.Now.AddDays(30);
            data30d = new DateTime(data30d.Year, data30d.Month, data30d.Day, 0, 0, 0);

            var AgendarHoje = _avaliaCriterioAvaliacaoAppServico.Get(x => x.DtProximaAvaliacao == DateTime.Now);
            var Agendar30D = _avaliaCriterioAvaliacaoAppServico.Get(x => x.DtProximaAvaliacao == data30d);



            AgendaEmail(AgendarHoje);
            AgendarEmail30Dias(Agendar30D);
        }

        public void ValidarAgendamento()
        {


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


            //registro.DtInclusao = filaEnvio.DataInclusao;
            //registro.IdGestor = plai.Pai.IdGestor;
            //registro.IdPlai = plai.IdPlai;
            //registro.IdUsuarioInclusao = int.Parse(Util.ObterUsuario().IdUsuario);

            //StringBuilder sb = new StringBuilder();

            //foreach (var norma in plai.PlaiProcessoNorma)
            //{
            //    sb.AppendFormat("<tr><td>{0} - {1}</td></tr>", norma.Norma.Titulo, norma.Norma.Codigo);
            //}

            //conteudo = conteudo.Replace("#NomeGestor#", gestor.NmCompleto);
            //conteudo = conteudo.Replace("#PlaiAgendada#", sb.ToString());


        }

        public void EnfileirarEmail(IEnumerable<AvaliaCriterioAvaliacao> ListEmail , string path) {

            string template = System.IO.File.ReadAllText(path);

            string conteudo = template;

            foreach (var critAva in ListEmail)
            {

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("<tr><td>{0}</td></tr>", _fornecedorAppServico.GetById(critAva.IdFornecedor).Nome);

                conteudo = conteudo.Replace("#Qualificacao#", sb.ToString());


                FilaEnvio filaEnvio = new FilaEnvio();

                filaEnvio.Assunto = "Agendamento Qualificação de Fornecedor";
                filaEnvio.DataAgendado = DateTime.Now;
                filaEnvio.DataInclusao = DateTime.Now;
                filaEnvio.Destinatario = _usuarioAppServico.GetById((int)critAva.IdUsuarioAvaliacao).CdIdentificacao;
                filaEnvio.Enviado = false;
                filaEnvio.Mensagem = conteudo;

                RegistroQualificacao registro = new RegistroQualificacao();

                registro.IdAvaliaCriterio = critAva.IdAvaliaCriterioAvaliacao;
                registro.IdFilaEnvio = filaEnvio.Id;
                registro.IdFornecedor = critAva.IdFornecedor;
                registro.DtInclusao = DateTime.Now;
                registro.IdUsuarioAvaliacao = (int)critAva.IdUsuarioAvaliacao;
                registro.IdUsuarioInclusao = (int)critAva.IdUsuarioAvaliacao;

                _registroQualificacaoServico.InserirEmail(registro);

                _filaEnvioServico.Enfileirar(filaEnvio);
            }
        }
    }
}
