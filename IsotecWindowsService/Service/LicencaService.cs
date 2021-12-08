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
    public class LicencaService : ILicencaService

    {
        private readonly ILicencaAppServico _licencaAppServico;
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IRegistroLicencaServico _registroLicencaServico;



        public LicencaService(ILicencaAppServico licencaAppServico,
            IFilaEnvioServico filaEnvioServico,
            IUsuarioAppServico usuarioAppServico,
            IRegistroLicencaServico registroLicencaServico
           )
        {

            _licencaAppServico = licencaAppServico;
            _filaEnvioServico = filaEnvioServico;
            _usuarioAppServico = usuarioAppServico;
            _registroLicencaServico = registroLicencaServico;


        }

        public void EnfileirarEmail()
        {
            EnfileirarEmailLicenca();


        }
        public void EnfileirarEmailLicenca()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Auditoria-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

                var data30d = DateTime.Now.AddDays(30);
                data30d = new DateTime(data30d.Year, data30d.Month, data30d.Day, 0, 0, 0);

                var AgendarHoje = _licencaAppServico.GetAll().Where(x => x.DataProximaNotificacao != null).ToList();

                AgendarHoje = AgendarHoje.Where(x => x.DataProximaNotificacao.Value.Date == DateTime.Now.Date).ToList();

                var avaHj = AgendarHoje.Select(x => new { x.IdLicenca, x.DataVencimento, x.IdResponsavel }).Distinct().ToList();


                List<Licenca> lstLicenca = new List<Licenca>();

                foreach (var Licenca in avaHj)
                {
                    var licenca = new Licenca();

                    licenca.IdLicenca = Licenca.IdLicenca;
                    licenca.DataVencimento = Licenca.DataVencimento;
                    licenca.IdResponsavel = (int)Licenca.IdResponsavel;

                    lstLicenca.Add(licenca);
                }

                AgendaEmail(ValidaAgendamento(lstLicenca), DateTime.Now);


            }
            catch (Exception ex)
            {
                FileLogger.Log("Erro ao enfileirar os e-mails", ex);
            }
        }

        public List<Licenca> ValidaAgendamento(List<Licenca> avaliaCriterioLicencas)
        {


            var retorno = new List<Licenca>();

            retorno.AddRange(avaliaCriterioLicencas);

            
            foreach (var item in retorno)
            {
                var reg = _registroLicencaServico.RetornaRegistro(item.IdLicenca);

                if (reg != null)
                {
                    if ((DateTime)reg.DtInclusao.Value.Date == DateTime.Now.Date)
                    {
                        avaliaCriterioLicencas.Remove(item);
                    }
                }
            }

            return avaliaCriterioLicencas;
        }

        public void AgendaEmail(IEnumerable<Licenca> ListEmail, DateTime date)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\Licenca-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            EnfileirarEmail(ListEmail, path);
        }

        public void EnfileirarEmail(IEnumerable<Licenca> ListEmail, string path)
        {

            string template = System.IO.File.ReadAllText(path);

            string conteudo = template;

            foreach (var licenca in ListEmail)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("<tr><td>{0}</td></tr>", _usuarioAppServico.GetById(licenca.IdResponsavel).NmCompleto);

                Licenca relation = _licencaAppServico.Get().Where(x => x.IdLicenca == licenca.IdLicenca).FirstOrDefault();

                var Responsavel = _usuarioAppServico.GetById((int)licenca.IdResponsavel);

                conteudo = conteudo.Replace("#Responsavel#", Responsavel.NmCompleto);
                conteudo = conteudo.Replace("#Titulo#", relation.Titulo.ToString());


                FilaEnvio filaEnvio = new FilaEnvio();

                filaEnvio.Assunto = "Notificação de Licença";
                filaEnvio.DataAgendado = DateTime.Now;
                filaEnvio.DataInclusao = DateTime.Now;
                filaEnvio.Destinatario = Responsavel.CdIdentificacao;
                filaEnvio.Enviado = false;
                filaEnvio.Mensagem = conteudo;

                _filaEnvioServico.Enfileirar(filaEnvio);

                RegistroLicenca registro = new RegistroLicenca();

                registro.GuidLicenca = licenca.IdLicenca.ToString();
                registro.IdFilaEnvio = filaEnvio.Id;
                registro.IdRegLicenca = licenca.IdLicenca;
                registro.DtInclusao = DateTime.Now;
                registro.IdResponsavel = (int)licenca.IdResponsavel;
                registro.IdUsuarioInclusao = (int)licenca.IdResponsavel;

                _registroLicencaServico.InserirEmail(registro);
            }
        }
    }
}
