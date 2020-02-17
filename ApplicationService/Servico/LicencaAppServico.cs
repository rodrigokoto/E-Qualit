using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Repository;

namespace ApplicationService.Servico
{
    public class LicencaAppServico : BaseServico<Licenca>, ILicencaAppServico
    {
        private readonly ILicencaRepositorio _licencaRepositorio;
        private readonly IAnexoAppServico _AnexoAppServico;
        private readonly IArquivoLicencaAnexoRepositorio _ArquivoLicencaAnexoRepositorio;

        public LicencaAppServico(ILicencaRepositorio licencaRepositorio , IAnexoAppServico anexoAppServico  , IArquivoLicencaAnexoRepositorio arquivoLicencaAnexoRepositorio) : base(licencaRepositorio)
        {
            _AnexoAppServico = anexoAppServico;
            _ArquivoLicencaAnexoRepositorio = arquivoLicencaAnexoRepositorio;
        }
        public void SalvarArquivoLicenca(Licenca licenca)
        {
            //se ainda nao salvou, esperamos apra processar e vamos ser chamados de novo depois que criar o registro
            if (licenca.IdLicenca == 0)
                return;

            //aqui o nc está vindo da tela
            if (licenca.ArquivosLicencaAnexos != null)
            {
                foreach (var licencaanexo in licenca.ArquivosLicencaAnexos)
                {
                    if (licencaanexo.ApagarAnexo == 1)
                    {
                        //apagamos deirtamente do anexo
                        //ninguem mais pode estar usando esse anexo

                        //tem que ser removida pelo servico, e não da lista
                        _ArquivoLicencaAnexoRepositorio.Remove(_ArquivoLicencaAnexoRepositorio.Get(x =>x.IdAnexo == licencaanexo.IdAnexo).FirstOrDefault());
                        //remover tb da tabela intermediaria
                        //naõ precisa, ele remove sozinho!
                        //foi testado! é que o ínidce no SQL está para cascatear a exclusão

                        /*
                        var existente = nc.ArquivosDeEvidencia.FirstOrDefault(r => r.IdAnexo == arquivoEvidencia.IdAnexo);
                        if (existente != null)
                            nc.ArquivosDeEvidencia.Remove(existente);
                            */
                        continue;
                    }

                    if (licencaanexo == null)
                        continue;
                    if (licencaanexo.Anexo == null)
                        continue;
                    if (string.IsNullOrEmpty(licencaanexo.Anexo.Extensao))
                        continue;
                    if (string.IsNullOrEmpty(licencaanexo.Anexo.ArquivoB64))
                        continue;

                    if (licencaanexo.Anexo.ArquivoB64 == "undefined")
                        continue;


                    Anexo anexoAtual = _AnexoAppServico.GetById(licencaanexo.IdAnexo);
                    if (anexoAtual == null)
                    {
                        licencaanexo.Anexo.TratarComNomeCerto();
                        licencaanexo.IdLicenca = licenca.IdLicenca;
                        _ArquivoLicencaAnexoRepositorio.Add(licencaanexo);
                    }

                }

                //limpar para nao reprocessar
                licenca.ArquivosLicencaAnexos = new List<ArquivoLicencaAnexo>();
            }
        }

    }
}
