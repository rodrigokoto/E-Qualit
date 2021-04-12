using ApplicationService.Interface;
using Dominio.Entidade;
using IsotecWindowsService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsotecWindowsService.Service
{
    public class CalibracaoService : ICalibracaoService
    {
        private readonly IInstrumentoAppServico _instrumentoAppServico;
        private readonly ICalibracaoAppServico _calibracaoAppServico;

        public CalibracaoService(IInstrumentoAppServico instrumentoAppServico, ICalibracaoAppServico calibracaoAppServico)
        {
            _instrumentoAppServico = instrumentoAppServico;
            _calibracaoAppServico = calibracaoAppServico;
        }

        public void AtualizaCalibracao()
        {
            var Instrumentos = _instrumentoAppServico.GetAll().Where(x => x.Status != 2).ToList();

            foreach (var instrumento in Instrumentos)
            {
                var calibracoes = _calibracaoAppServico.GetAll().Where(x => x.IdInstrumento == instrumento.IdInstrumento).ToList();

                if (calibracoes.Count() == 0)
                {
                    instrumento.Status = 0;
                    _instrumentoAppServico.Update(instrumento);
                }
                else
                {
                    if (instrumento.Status != 2 || instrumento.Status != 3)
                    {
                        var calibracao = _calibracaoAppServico.Get().Where(x => x.IdInstrumento == instrumento.IdInstrumento).OrderByDescending(x => x.DataProximaCalibracao).FirstOrDefault();

                        if (calibracao.Aprovado == 0)
                        {
                            if (instrumento.Status != 0)
                            {
                                instrumento.Status = 0;
                                _instrumentoAppServico.Update(instrumento);
                            }
                        }
                        else if (calibracao.Aprovado == 1)
                        {

                            if (calibracao.DataProximaCalibracao <= DateTime.Now.AddDays(1))
                            {
                                instrumento.Status = 0;
                                _instrumentoAppServico.Update(instrumento);
                            }
                            else if (calibracao.DataProximaCalibracao >= DateTime.Now) {
                                instrumento.Status = 1;
                                _instrumentoAppServico.Update(instrumento);
                            }
                        }
                    }
                }
            }
        }
    }
}
