using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Validacao
{
    public class DocDocumentoXMLValidacao: BaseValidation
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
            //if (IsFluxo(codigoTemplate.TpTemplate))
            //    //ValidarFluxo(dcXML.Fluxo);
            //else if (IsTexto(codigoTemplate.TpTemplate))
            //    //ValidarTexto(dcXML.Texto);
            //else if (IsRiscos(codigoTemplate.TpTemplate))
            //    ValidarRiscos(dcXML.Riscos);
            //else if (IsRecursos(codigoTemplate.TpTemplate))
            //    //ValidarRecursos(dcXML.Recursos);
            ////else if (IsLicenca(codigoTemplate.TpTemplate))
            ////    ValidarLicenca(dcXML.Licenca);
            //else if (IsRegistros(codigoTemplate.TpTemplate))
                //ValidarRegistros(dcXML.Registros);
        }

        private bool ValidarRegistros(List<DocRegistro> registros)
        {
            foreach (var reg in registros)
            {
                if (String.IsNullOrWhiteSpace(reg.Identificar) || String.IsNullOrWhiteSpace(reg.Armazenar) || String.IsNullOrWhiteSpace(reg.Disposicao)
                    || String.IsNullOrWhiteSpace(reg.Proteger) || String.IsNullOrWhiteSpace(reg.Recuperar) || String.IsNullOrWhiteSpace(reg.Retencao))
                {
                    erroOrigem.Add(Traducao.Resource.RegistrosResponsavelAcaoCorretiva);
                    erros.Add(new ValidationResult(Traducao.Resource.ValidaTodosCamposAcaoCorretiva, erroOrigem));
                    return false;
                }
            }

            return true;
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
            if (tp != DocTemplate.GestaoDeRiscoTemplate)
                return false;

            return true;
        }
           
        private bool IsLicenca(string tp)
        {
            if (tp != DocTemplate.LicencaTemplate)
                return false;

            return true;
        }

        private bool IsDocExternos(string tp)
        {
            if (tp != DocTemplate.DocExternoTemplate)
                return false;

            return true;
        }

        private bool IsRegistros(string tp)
        {
            if (tp != DocTemplate.RegistrosTemplate)
                return false;

            return true;
        }

        private bool ValidarRecursos(Recurso recurso)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(recurso.Texto))
                {
                    erroOrigem.Add(Traducao.Resource.Recursos);
                    erros.Add(new ValidationResult(Traducao.Resource.ValidaRecursoPreenchidos, erroOrigem));
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
                if (String.IsNullOrWhiteSpace(fluxo))
                {
                    erroOrigem.Add(Traducao.Resource.DocDocumento_lbl_Fluxo);
                    erros.Add(new ValidationResult(Traducao.Resource.ValidaCriarFluxo, erroOrigem));
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
                if (String.IsNullOrWhiteSpace(texto))
                {
                    erroOrigem.Add(Traducao.Resource.DocDocumento_lbl_Texto);
                    erros.Add(new ValidationResult(Traducao.Resource.ValidaCriarTexto, erroOrigem));
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
                    if (String.IsNullOrWhiteSpace(risk.Texto) || String.IsNullOrWhiteSpace(risk.Classificacao))
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

        private bool ValidarUpload(List<String> upload)
        {
            if (upload == null || upload.Count == 0)
            {
                erroOrigem.Add(Traducao.Resource.DocDocumento_lbl_Upload);
                erros.Add(new ValidationResult(Traducao.Resource.ValidaTodosCamposAcaoCorretiva, erroOrigem));
                return false;
            }
            return true;
        }

        private bool ValidarLicenca(Licenca licenca)
        {
            if (licenca.DataEmissao == DateTime.MinValue)
            {
                erroOrigem.Add(Traducao.Resource.Licenca);
                erros.Add(new ValidationResult(Traducao.Resource.ValidaDataLicenca, erroOrigem));
                return false;
            }

            if (licenca.DataVencimento == DateTime.MinValue)
            {
                erroOrigem.Add(Traducao.Resource.Licenca);
                erros.Add(new ValidationResult(Traducao.Resource.ValidaDataVencimentoLicenca, erroOrigem));
                return false;
            }

            //if (String.IsNullOrWhiteSpace(licenca.Arquivos))
            //{
            //    erroOrigem.Add("Licença");
            //    erros.Add(new ValidationResult("É incluir arquivos para a licença", erroOrigem));
            //    return false;
            //}

            return true;
        }

        private bool ValidarDocExternos(String docExterno)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(docExterno))
                {
                    erroOrigem.Add(Traducao.Resource.DocumentosExternos);
                    erros.Add(new ValidationResult(Traducao.Resource.ValidaDocumentosExternos, erroOrigem));
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
