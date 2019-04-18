using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Servico.Validacao
{
    public class DocDocumentoXMLValidacao
    {
        private List<ValidationResult> erros = new List<ValidationResult>();
        List<String> erroOrigem = new List<String>();

        public bool IsValid(DocDocumentoXML doc, List<DocTemplate> template)
        {
            try
            {
                foreach (var item in template)
                {
                    ValidarTemplate(item, doc);
                }

                if (erros.Count > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidarTemplate(DocTemplate codigoTemplate, DocDocumentoXML dcXML)
        {
            if (IsFluxo(codigoTemplate.TpTemplate))
                ValidarFluxo(dcXML.Fluxo);
            else if (IsRecursos(codigoTemplate.TpTemplate))
                ValidarTexto(dcXML.Fluxo);
            else if (IsTexto(codigoTemplate.TpTemplate))
                ValidarRiscos(dcXML.Riscos);
            else if (IsRiscos(codigoTemplate.TpTemplate))
                ValidarRecursos(dcXML.Recursos);
            else if (IsUpload(codigoTemplate.TpTemplate))
                ValidarUpload(dcXML.Upload);
            else if (IsLicenca(codigoTemplate.TpTemplate))
                ValidarLicenca(dcXML.Licenca);
            else if (IsDocExternos(codigoTemplate.TpTemplate))
                ValidarLicenca(dcXML.Licenca);
        }

        private bool IsFluxo(string tp)
        {
            if (tp != DocTemplate.FluxoTemplate)
                return false;
            return true;
        }

        private bool IsRecursos(string tp)
        {
            if (tp != DocTemplate.RecursosTemplate)
                return false;

            return true;
        }

        private bool IsTexto(string tp)
        {
            if (tp != DocTemplate.TextoTemplate)
                return false;

            return true;
        }

        private bool IsRiscos(string tp)
        {
            if (tp != DocTemplate.TextoTemplate)
                return false;

            return true;
        }

        private bool IsUpload(string tp)
        {
            if (tp != DocTemplate.TextoTemplate)
                return false;

            return true;
        }

        private bool IsLicenca(string tp)
        {
            if (tp != DocTemplate.TextoTemplate)
                return false;

            return true;
        }

        private bool IsDocExternos(string tp)
        {
            if (tp != DocTemplate.TextoTemplate)
                return false;

            return true;
        }

        private bool ValidarRecursos(Recurso recurso)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(recurso.Texto))
                {
                    erroOrigem.Add("Recursos");
                    erros.Add(new ValidationResult("Os recursos devem ser preenchidos", erroOrigem));
                    return false;
                }
                
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValidarFluxo(String fluxo)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(fluxo))
                {
                    erroOrigem.Add("Fluxo");
                    erros.Add(new ValidationResult("É necessário criar um fluxo", erroOrigem));
                    return false;
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValidarTexto(String texto)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(texto))
                {
                    erroOrigem.Add("Texto");
                    erros.Add(new ValidationResult("É necessário criar um texto para o documento", erroOrigem));
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ValidarRiscos(List<Risco> riscos)
        {
            try
            {
                foreach (var risk in riscos)
                {
                    if (!String.IsNullOrWhiteSpace(risk.Texto) && !String.IsNullOrWhiteSpace(risk.Classificacao))
                    {
                        erroOrigem.Add("Risco");
                        erros.Add(new ValidationResult("É necessário preencher os riscos", erroOrigem));
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValidarUpload(List<Upload> upload)
        {
            if (upload == null || upload.Count == 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidarLicenca(Licenca licenca)
        {
            if (licenca.DataEmissao != DateTime.MinValue)
            {
                erroOrigem.Add("Licença");
                erros.Add(new ValidationResult("É necessario definir uma data de emissão para a licença", erroOrigem));
                return false;
            }

            if (licenca.DataVencimento != DateTime.MinValue)
            {
                erroOrigem.Add("Licença");
                erros.Add(new ValidationResult("É necessario definir uma data de vencimento para a licença", erroOrigem));
                return false;
            }

            if (String.IsNullOrWhiteSpace(licenca.Arquivos))
            {
                erroOrigem.Add("Licença");
                erros.Add(new ValidationResult("É necessario definir uma data de emissão para a licença", erroOrigem));
                return false;
            }

            return true;
        }

        private bool ValidarDocExternos(String docExterno)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(docExterno))
                {
                    erroOrigem.Add("DocumentosExternos");
                    erros.Add(new ValidationResult("É necessário indicar o caminho do documento externo", erroOrigem));
                    return false;
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
