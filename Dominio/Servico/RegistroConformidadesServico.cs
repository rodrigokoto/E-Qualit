using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.AcoesImediatas;
using Dominio.Validacao.RegistroConformidades.AcaoCorretivas;
using Dominio.Validacao.RegistroConformidades.GestaoDeRiscos;
using Dominio.Validacao.RegistroConformidades.NaoConformidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Servico
{
    public class RegistroConformidadesServico : IRegistroConformidadesServico
    {
        private readonly IRegistroConformidadesRepositorio _registroConformidadesRepositorio;
        private readonly IRegistroAcaoImediataRepositorio _registroAcaoImediataRepositorio;

        private readonly INotificacaoServico _notificacaoServico;

        public RegistroConformidadesServico(
            IRegistroConformidadesRepositorio registroConformidadesRepositorio,
            IRegistroAcaoImediataRepositorio registroAcaoImediataRepositorio,
            INotificacaoServico notificacaoServico,
            IUsuarioRepositorio usuarioRepositorio)
        {
            _registroConformidadesRepositorio = registroConformidadesRepositorio;
            _registroAcaoImediataRepositorio = registroAcaoImediataRepositorio;
            _notificacaoServico = notificacaoServico;
        }
        public void DeletaRegistroConformidade(RegistroConformidade registroConformidade)
        {
            registroConformidade.AcoesImediatas = new List<RegistroAcaoImediata>();

            _registroConformidadesRepositorio.Remove(registroConformidade);
        }
        public RegistroConformidade SalvarPrimeiraEtapa(RegistroConformidade registroConformidade)
        {
            registroConformidade.IdResponsavelEtapa = registroConformidade.IdResponsavelInicarAcaoImediata.Value;
            registroConformidade = _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(registroConformidade);
            _registroConformidadesRepositorio.Add(registroConformidade);

            return registroConformidade;
        }

        public RegistroConformidade SalvarSegundaEtapa(RegistroConformidade registroConformidade, Funcionalidades funcionalidade)
        {
            registroConformidade = TrataRegistroConformidadeParaSerAtualizada(registroConformidade);

            _registroConformidadesRepositorio.Update(registroConformidade);
            _notificacaoServico.RemovePorFuncionalidade((int)funcionalidade, registroConformidade.IdRegistroConformidade);

            return registroConformidade;
        }

        private RegistroConformidade TrataAC(RegistroConformidade acaoCorretiva)
        {
            var listaAcaoImediataUpdate = acaoCorretiva.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);

            var efalso = listaAcaoImediataUpdate.FirstOrDefault() != null;
            var objCtx = _registroConformidadesRepositorio.GetById(acaoCorretiva.IdRegistroConformidade);

            if (acaoCorretiva.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && efalso == false)
            {
                objCtx.StatusEtapa = acaoCorretiva.StatusEtapa;
                objCtx.DtDescricaoAcao = acaoCorretiva.DtDescricaoAcao;
                objCtx.AcoesImediatas = acaoCorretiva.AcoesImediatas;
                objCtx.IdResponsavelReverificador = acaoCorretiva.IdResponsavelReverificador;
                objCtx.IdUsuarioAlterou = acaoCorretiva.IdUsuarioAlterou;
                objCtx.Causa = acaoCorretiva.Causa;
                objCtx.StatusRegistro = acaoCorretiva.StatusRegistro;


                acaoCorretiva.AcoesImediatas.ToList().ForEach(acaoImediata =>
                {
                    acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                    acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                });

            }
            else if (acaoCorretiva.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && efalso == true)
            {

                listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
                {
                    acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                    acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                    _registroAcaoImediataRepositorio.Update(acaoImediata);
                });

                var listaAcaoImediataNaoImplementadas = acaoCorretiva.AcoesImediatas.FirstOrDefault(x => x.DtEfetivaImplementacao == null) != null;
                if (listaAcaoImediataNaoImplementadas == false)
                {
                    objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Reverificacao;
                }

            }
            else if (acaoCorretiva.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && efalso == true)
            {
                listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
                {
                    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Aprovado = acaoImediata.Aprovado;
                });

                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
                objCtx.FlEficaz = acaoCorretiva.FlEficaz;

                if (objCtx.FlEficaz == false)
                {
                    var novaAC = CriarAcaoCorretivaSeFlEficazFalse(objCtx);
                    novaAC.DescricaoRegistro += $"\n \n Referênte a Ação Corretiva({objCtx.NuRegistro})";
                    _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaAC);
                    _registroConformidadesRepositorio.Add(novaAC);
                }
            }

            return objCtx;
        }

        private RegistroConformidade TrataGR(RegistroConformidade gestaoDeRisco)
        {
            var listaAcaoImediataUpdate = gestaoDeRisco.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);

            var efalso = listaAcaoImediataUpdate.FirstOrDefault() != null;
            var objCtx = _registroConformidadesRepositorio.GetById(gestaoDeRisco.IdRegistroConformidade);

            if (gestaoDeRisco.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && efalso == false)
            {
                objCtx.StatusEtapa = gestaoDeRisco.StatusEtapa;
                objCtx.DtDescricaoAcao = gestaoDeRisco.DtDescricaoAcao;
                objCtx.AcoesImediatas = gestaoDeRisco.AcoesImediatas;
                objCtx.IdResponsavelReverificador = gestaoDeRisco.IdResponsavelReverificador;
                objCtx.IdUsuarioAlterou = gestaoDeRisco.IdUsuarioAlterou;
                objCtx.Causa = gestaoDeRisco.Causa;
                objCtx.StatusRegistro = gestaoDeRisco.StatusRegistro;

                gestaoDeRisco.AcoesImediatas.ToList().ForEach(acaoImediata =>
                {
                    acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                    acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                });


            }
            else if (gestaoDeRisco.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && efalso == true)
            {

                listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
                {
                    acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                    acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                    _registroAcaoImediataRepositorio.Update(acaoImediata);
                });

                var listaAcaoImediataNaoImplementadas = gestaoDeRisco.AcoesImediatas.FirstOrDefault(x => x.DtEfetivaImplementacao == null) != null;
                if (listaAcaoImediataNaoImplementadas == false)
                {
                    objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Reverificacao;
                }

            }
            else if (gestaoDeRisco.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && efalso == true)
            {
                listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
                {
                    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Aprovado = acaoImediata.Aprovado;
                });

                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
                objCtx.FlEficaz = gestaoDeRisco.FlEficaz;

                if (objCtx.FlEficaz == false)
                {
                    var novaGR = CriarGestaoDeRiscoSeFlEficazFalse(objCtx);
                    novaGR.DescricaoRegistro += $"\n \n Referênte a Gestão de Risco({objCtx.NuRegistro})";
                    _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaGR);
                    _registroConformidadesRepositorio.Add(novaGR);
                }
            }
            return objCtx;
        }

        private RegistroConformidade TrataNC(RegistroConformidade naoConformidade)
        {
            var objCtx = _registroConformidadesRepositorio.GetById(naoConformidade.IdRegistroConformidade);

            var listaAcaoImediataUpdate = naoConformidade.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
            var temAcoesImediataParaAtualizar = listaAcaoImediataUpdate.FirstOrDefault() != null;

            if (naoConformidade.OStatusEEncerrada() && naoConformidade.EProcedente == false)
            {
                objCtx.EProcedente = naoConformidade.EProcedente;
                objCtx.StatusEtapa = naoConformidade.StatusEtapa;
                objCtx.DtDescricaoAcao = naoConformidade.DtDescricaoAcao;
                objCtx.DescricaoAcao = naoConformidade.DescricaoAcao;
                objCtx.IdUsuarioAlterou = naoConformidade.IdUsuarioAlterou;
                objCtx.DtEnceramento = DateTime.Now;
                objCtx.Causa = naoConformidade.Causa;
                objCtx.StatusRegistro = naoConformidade.StatusRegistro;

            }
            else if (naoConformidade.OStatusEImplementacao() && naoConformidade.EProcedente == true && temAcoesImediataParaAtualizar == false)
            {
                objCtx.EProcedente = naoConformidade.EProcedente;
                objCtx.StatusEtapa = naoConformidade.StatusEtapa;
                objCtx.DtDescricaoAcao = naoConformidade.DtDescricaoAcao;
                objCtx.ECorrecao = naoConformidade.ECorrecao;
                objCtx.NecessitaAcaoCorretiva = naoConformidade.NecessitaAcaoCorretiva;
                objCtx.AcoesImediatas = naoConformidade.AcoesImediatas;
                objCtx.IdResponsavelReverificador = naoConformidade.IdResponsavelReverificador;
                objCtx.DescricaoAnaliseCausa = naoConformidade.DescricaoAnaliseCausa;
                objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva = naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva;
                objCtx.IdUsuarioAlterou = naoConformidade.IdUsuarioAlterou;
                objCtx.Causa = naoConformidade.Causa;
                objCtx.StatusRegistro = naoConformidade.StatusRegistro;

                naoConformidade.AcoesImediatas.ToList().ForEach(acaoImediata =>
                {
                    acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                    acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                });

                if (naoConformidade.NecessitaAcaoCorretiva == true && naoConformidade.IdNuRegistroFilho == null)
                {
                    var novaAc = CriarAcaoCorretivaApartirDeNaoConformidade(objCtx);

                    novaAc.DescricaoRegistro += $"\n\n Referênte a Não Conformidade({objCtx.NuRegistro})";
                    _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaAc);
                    _registroConformidadesRepositorio.Add(novaAc);

                    objCtx.IdNuRegistroFilho = novaAc.NuRegistro;
                }

            }
            else if (naoConformidade.OStatusEImplementacao() && temAcoesImediataParaAtualizar == true)
            {

                listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
                {
                    acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                    acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                    _registroAcaoImediataRepositorio.Update(acaoImediata);
                });

                var listaAcaoImediataNaoImplementadas = naoConformidade.AcoesImediatas.FirstOrDefault(x => x.DtEfetivaImplementacao == null) != null;
                if (listaAcaoImediataNaoImplementadas == false)
                {
                    objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Reverificacao;
                }

            }
            else if (naoConformidade.OStatusEReverificacao() && temAcoesImediataParaAtualizar == true)
            {
                listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
                {
                    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Aprovado = acaoImediata.Aprovado;
                });

                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
                objCtx.FlEficaz = naoConformidade.FlEficaz;

                if (objCtx.FlEficaz == false)
                {
                    var novaNC = CriarNaoConformidadeSeFlEficazFalse(objCtx);
                    novaNC.DescricaoRegistro += $"\n \n Referênte a Não Conformidade({objCtx.NuRegistro})";
                    _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaNC);
                    if (objCtx.NecessitaAcaoCorretiva == true)
                    {
                        novaNC.EProcedente = true;
                        novaNC.NecessitaAcaoCorretiva = true;
                        novaNC.IdNuRegistroFilho = objCtx.IdNuRegistroFilho;
                    }

                    _registroConformidadesRepositorio.Add(novaNC);
                }
            }
            return objCtx;


        }

        private RegistroConformidade TrataRegistroConformidadeParaSerAtualizada(RegistroConformidade registroConformidade)
        {

            switch (registroConformidade.TipoRegistro)
            {
                case "nc":
                    registroConformidade = TrataNC(registroConformidade);
                    break;

                case "ac":
                    registroConformidade = TrataAC(registroConformidade);
                    break;

                case "gr":
                    registroConformidade = TrataGR(registroConformidade);
                    break;

                default:
                    break;
            }

            return registroConformidade;

        }

        public void ConsisteItegridadeAcaoImediata(RegistroAcaoImediata acaoImediata)
        {
            if (acaoImediata.Estado == EstadoObjetoEF.Added)
            {
                _registroAcaoImediataRepositorio.AlteraEstado(acaoImediata, EstadoObjetoEF.Added);
            }
            else if (acaoImediata.Estado == EstadoObjetoEF.Deleted)
            {
                RegistroAcaoImediata filhoQueSeraDeletado = new RegistroAcaoImediata()
                {
                    IdRegistroConformidade = acaoImediata.IdRegistroConformidade,
                    Descricao = acaoImediata.Descricao,
                    DtInclusao = acaoImediata.DtInclusao,
                    DtPrazoImplementacao = acaoImediata.DtPrazoImplementacao,
                    IdUsuarioIncluiu = acaoImediata.IdUsuarioIncluiu,
                    IdResponsavelImplementar = acaoImediata.IdResponsavelImplementar

                };
                _registroAcaoImediataRepositorio.AlteraEstado(acaoImediata, EstadoObjetoEF.Deleted);
            }

            else if (acaoImediata.Estado == EstadoObjetoEF.Modified)
            {
                _registroAcaoImediataRepositorio.AlteraEstado(acaoImediata, EstadoObjetoEF.Modified);
            }

        }

        public List<RegistroConformidade> ObtemListaRegistroConformidadePorSite(int idSite, string tipoRegistro, ref int numeroUltimoRegistro)
        {
            var listaNC = _registroConformidadesRepositorio.Get(
                                     x => x.IdSite == idSite
                                     && x.TipoRegistro == tipoRegistro).OrderByDescending(x => x.NuRegistro)
                                     .ToList();

            numeroUltimoRegistro = listaNC.Count > 0 ? listaNC.Select(x => x.NuRegistro).Max() : 0;
            return listaNC;
        }

        public RegistroConformidade ObtemAcaoConformidadePorNaoConformidade(RegistroConformidade naoConformidade)
        {
            var acaoConformidade = new RegistroConformidade();
            try
            {
                return _registroConformidadesRepositorio.Get(x => x.IdSite == naoConformidade.IdSite &&
                                                 x.NuRegistro == naoConformidade.NuRegistro &&
                                                 x.TipoRegistro == "ac").FirstOrDefault();

            }
            catch (Exception)
            {
                return null;
            }

        }

        public DateTime ObtemUltimaDataEmissao(int idSite, string tipoRegistro)
        {
            try
            {
                return _registroConformidadesRepositorio
                    .Get(
                        x => x.IdSite == idSite
                        && x.TipoRegistro == tipoRegistro
                    ).Max(x => x.DtEmissao);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public void ValidoParaExclusaoNaoConformidade(RegistroConformidade naoConformidade, ref List<string> erros)
        {
            var validacao = new AptoParaExclusaoNaoConformidadeValidation(_registroConformidadesRepositorio).Validate(naoConformidade);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao));
            }
        }

        public void ValidoParaExclusaoAcaoCorretiva(RegistroConformidade acaoCorretiva, ref List<string> erros)
        {
            var validacao = new AptoParaExclusaoAcaoCorretivaValidation(_registroConformidadesRepositorio).Validate(acaoCorretiva);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao));
            }
        }

        public void GestaoDeRiscoValida(RegistroConformidade gestaoDeRisco, ref List<string> erros)
        {
            var validacao = new CamposObrigatoriosGestaoDeRiscoEtapa1Validation().Validate(gestaoDeRisco);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }

        public RegistroConformidade SalvarGestaoDeRisco(RegistroConformidade gestaoDeRisco)
        {
            var gestaoDeRiscoCTX = _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(gestaoDeRisco);
            _registroConformidadesRepositorio.Add(gestaoDeRiscoCTX);

            return gestaoDeRisco;
        }

        public void ValidarCampos(RegistroConformidade naoConformidade, ref List<string> erros)
        {
            var validaCamposPorEtapa = DefineEtapa(naoConformidade).Validate(naoConformidade);

            if (!validaCamposPorEtapa.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCamposPorEtapa.Errors));
            }

        }

        public void ValidaAcaoCorretiva(RegistroConformidade acaoCorretiva, int idUsuarioLogado, ref List<string> erros)
        {
            var acaoImediataUpdateIsValid = acaoCorretiva.AcoesImediatas.FirstOrDefault(x => x.Estado == EstadoObjetoEF.Modified) != null;
            var validaCampos = DefineFluxoValidacaoAC(acaoCorretiva).Validate(acaoCorretiva);
            if (!validaCampos.IsValid && validaCampos != null)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }

            if (acaoCorretiva.AcoesImediatas.Count > 0 && acaoCorretiva.StatusEtapa != (byte)EtapasRegistroConformidade.Reverificacao)
            {
                var erroAux = new List<string>();

                Parallel.ForEach(acaoCorretiva.AcoesImediatas, acaoImediata =>
                {
                    erroAux = TrataAcaoImediata(acaoImediata, acaoCorretiva, idUsuarioLogado, erroAux);
                });
                erros.AddRange(erroAux);
            }

        }

        public void ValidaGestaoDeRisco(RegistroConformidade gestaoDeRisco, int idUsuarioLogado, ref List<string> erros)
        {
            var acaoImediataUpdateIsValid = gestaoDeRisco.AcoesImediatas.FirstOrDefault(x => x.Estado == EstadoObjetoEF.Modified) != null;
            var validaCampos = DefineFluxoValidacaoGR(gestaoDeRisco).Validate(gestaoDeRisco);

            if (gestaoDeRisco.NecessitaAcaoCorretiva != null)
            {
                if (!validaCampos.IsValid && validaCampos != null)
                {
                    erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
                }
                if (gestaoDeRisco.AcoesImediatas.Count > 0 && gestaoDeRisco.StatusEtapa != (byte)EtapasRegistroConformidade.Reverificacao)
                {
                    var erroAux = new List<string>();

                    Parallel.ForEach(gestaoDeRisco.AcoesImediatas, acaoImediata =>
                    {
                        erroAux = TrataAcaoImediata(acaoImediata, gestaoDeRisco, idUsuarioLogado, erroAux);
                    });
                    erros.AddRange(erroAux);
                }
            }
        }

        public void ValidaNaoConformidade(RegistroConformidade naoConformidade, int idUsuarioLogado, ref List<string> erros)
        {

            var acaoImediataUpdateIsValid = naoConformidade.AcoesImediatas.FirstOrDefault(x => x.Estado == EstadoObjetoEF.Modified) != null;



            try
            {

                var validaCampos = DefineFluxoValidacaoNC(naoConformidade).Validate(naoConformidade);
                if (!validaCampos.IsValid && validaCampos != null)
                {
                    erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
                }

                if (naoConformidade.AcoesImediatas.Count > 0 && naoConformidade.StatusEtapa != (byte)EtapasRegistroConformidade.Reverificacao)
                {
                    var erroAux = new List<string>();

                    Parallel.ForEach(naoConformidade.AcoesImediatas, acaoImediata =>
                    {
                        erroAux = TrataAcaoImediata(acaoImediata, naoConformidade, idUsuarioLogado, erroAux);
                    });
                    erros.AddRange(erroAux);

                }

            }
            catch
            {
            }
        }

        private List<string> TrataAcaoImediata(RegistroAcaoImediata acaoImediata, RegistroConformidade registro, int idUsuarioLogado, List<string> erroAux)
        {


            acaoImediata.IdUsuarioIncluiu = idUsuarioLogado;
            acaoImediata.IdRegistroConformidade = registro.IdRegistroConformidade;


            //registro.DtEmissao = registro.DtEmissao.AddHours(-registro.DtEmissao.Hour);
            //registro.DtEmissao = registro.DtEmissao.AddMinutes(-registro.DtEmissao.Minute);
            //registro.DtEmissao = registro.DtEmissao.AddSeconds(-registro.DtEmissao.Second);
            //registro.DtEmissao = registro.DtEmissao.AddMilliseconds(-registro.DtEmissao.Millisecond);
            acaoImediata.DtInclusao = registro.DtEmissao.Date;
            acaoImediata.Registro = registro;

            var dataNulaPrazo = false;
            if (acaoImediata.DtPrazoImplementacao == null)
            {
                dataNulaPrazo = true;
                acaoImediata.DtPrazoImplementacao = DateTime.Now;
            }

            var dataNula = false;
            if (acaoImediata.DtEfetivaImplementacao == null)
            {
                dataNula = true;
                acaoImediata.DtEfetivaImplementacao = DateTime.Now;
            }


            var CamposObrigatoriosAcaoImediata = new CamposObrigatoriosAcaoImediata()
                                            .Validate(acaoImediata);

            if (dataNula)
            {
                acaoImediata.DtEfetivaImplementacao = null;
            }

            if (dataNulaPrazo)
            {
                acaoImediata.DtPrazoImplementacao = null;
                CamposObrigatoriosAcaoImediata.Errors.Add(new FluentValidation.Results.ValidationFailure("DtPrazoImplementacao", Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_DtrPrazoImplementacao_required));
            }


            if (!CamposObrigatoriosAcaoImediata.IsValid)
            {
                erroAux.AddRange(UtilsServico.PopularErros(CamposObrigatoriosAcaoImediata.Errors));
            }


            acaoImediata.Registro = null;
            return erroAux;
        }

        private AbstractValidator<RegistroConformidade> DefineFluxoValidacaoAC(RegistroConformidade acaoCorretiva)
        {
            var acaoImediataUpdateIsValid = acaoCorretiva.AcoesImediatas.FirstOrDefault(x => x.Estado == EstadoObjetoEF.Modified) != null;
            if (acaoCorretiva.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && acaoImediataUpdateIsValid == false)
            {
                return new CamposObrigatoriosAcaoCorretivaEtapa2Validation();
            }
            else if (acaoCorretiva.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && acaoImediataUpdateIsValid == true)
            {
                return new CamposObrigatoriosGestaoDeRiscoImplementacaoDtEfetivaImplementacao();
            }
            else if (acaoCorretiva.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao)
            {
                return new CamposObrigatoriosAcaoCorretivaReverificacao();
            }
            return null;
        }

        private AbstractValidator<RegistroConformidade> DefineFluxoValidacaoGR(RegistroConformidade gestaoDeRisco)
        {
            var acaoImediataUpdateIsValid = gestaoDeRisco.AcoesImediatas.FirstOrDefault(x => x.Estado == EstadoObjetoEF.Modified) != null;
            if (gestaoDeRisco.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && acaoImediataUpdateIsValid == false)
            {
                return new CamposObrigatoriosGestaoDeRiscoEtapa2Validation();
            }
            else if (gestaoDeRisco.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && acaoImediataUpdateIsValid == true)
            {
                return new CamposObrigatoriosGestaoDeRiscoImplementacaoDtEfetivaImplementacao();
            }
            //else if (gestaoDeRisco.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao)
            else
            {
                return new CamposObrigatoriosNaoConformidadeReverificacao();
            }
            //return null;
        }

        private AbstractValidator<RegistroConformidade> DefineFluxoValidacaoNC(RegistroConformidade naoConformidade)
        {
            var acaoImediataUpdateIsValid = naoConformidade.AcoesImediatas.FirstOrDefault(x => x.Estado == EstadoObjetoEF.Modified) != null;
            if (!naoConformidade.EProcedente.Value && naoConformidade.OStatusEEncerrada() && acaoImediataUpdateIsValid == false)
            {
                return new NCEProcedenteFalseViewValidation();
            }
            else if (naoConformidade.EProcedente.Value && naoConformidade.OStatusEImplementacao() && acaoImediataUpdateIsValid == false)
            {
                return new NCEProcedenteTrueViewValidation();
            }
            else if (naoConformidade.EProcedente.Value && naoConformidade.OStatusEImplementacao() && acaoImediataUpdateIsValid == true)
            {
                return new CamposObrigatoriosSegundaEtapaAtaulizacaoAcaoImediata();
            }
            else if (naoConformidade.OStatusEReverificacao() && naoConformidade.EProcedente.Value)
            {
                return new CamposObrigatoriosNaoConformidadeReverificacao();
            }
            return null;
        }

        public RegistroConformidade CriarAcaoCorretivaApartirDeNaoConformidade(RegistroConformidade naoConformidade)
        {

            return new RegistroConformidade()
            {
                DescricaoRegistro = naoConformidade.DescricaoAnaliseCausa,// + $"\n Referênte a Não Conformidade({numeroRegistro})",
                IdEmissor = naoConformidade.IdUsuarioAlterou,
                IdUsuarioAlterou = naoConformidade.IdUsuarioAlterou,
                IdUsuarioIncluiu = naoConformidade.IdUsuarioAlterou,
                TipoRegistro = "ac",
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,
                IdResponsavelInicarAcaoImediata = naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva,
                IdResponsavelEtapa = naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva,
                IdSite = naoConformidade.IdSite,
                IdProcesso = naoConformidade.IdProcesso,
                Causa = naoConformidade.Causa,
                StatusRegistro = naoConformidade.StatusRegistro
                //,DsJustificativa = naoConformidade.DsJustificativa

            };
        }

        public RegistroConformidade CriarAcaoCorretivaSeFlEficazFalse(RegistroConformidade gestaoDeRisco)
        {
            return new RegistroConformidade()
            {
                DescricaoRegistro = gestaoDeRisco.DescricaoRegistro,// + $"\n Referênte a Não Conformidade({numeroRegistro})",
                IdEmissor = gestaoDeRisco.IdUsuarioAlterou,
                IdUsuarioAlterou = gestaoDeRisco.IdUsuarioAlterou,
                IdUsuarioIncluiu = gestaoDeRisco.IdUsuarioAlterou,
                TipoRegistro = "ac",
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,
                IdResponsavelInicarAcaoImediata = gestaoDeRisco.IdResponsavelInicarAcaoImediata,
                IdResponsavelEtapa = gestaoDeRisco.IdResponsavelInicarAcaoImediata.Value,
                IdSite = gestaoDeRisco.IdSite,
                IdProcesso = gestaoDeRisco.IdProcesso,
                Causa = gestaoDeRisco.Causa,
                StatusRegistro = gestaoDeRisco.StatusRegistro
            };
        }

        public RegistroConformidade CriarGestaoDeRiscoSeFlEficazFalse(RegistroConformidade gestaoDeRisco)
        {
            return new RegistroConformidade()
            {
                DescricaoRegistro = gestaoDeRisco.DescricaoRegistro,// + $"\n Referênte a Não Conformidade({numeroRegistro})",
                IdEmissor = gestaoDeRisco.IdUsuarioAlterou,
                IdUsuarioAlterou = gestaoDeRisco.IdUsuarioAlterou,
                IdUsuarioIncluiu = gestaoDeRisco.IdUsuarioAlterou,
                TipoRegistro = "gr",
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,
                IdResponsavelInicarAcaoImediata = gestaoDeRisco.IdResponsavelInicarAcaoImediata,
                IdResponsavelEtapa = gestaoDeRisco.IdResponsavelInicarAcaoImediata.Value,
                IdSite = gestaoDeRisco.IdSite,
                IdProcesso = gestaoDeRisco.IdProcesso,
                Causa = gestaoDeRisco.Causa,
                StatusRegistro = gestaoDeRisco.StatusRegistro
            };
        }

        public RegistroConformidade CriarNaoConformidadeSeFlEficazFalse(RegistroConformidade naoConformidade)
        {
            if (naoConformidade.NecessitaAcaoCorretiva == true)
            {

            }
            return new RegistroConformidade()
            {
                DescricaoRegistro = naoConformidade.DescricaoRegistro,// + $"\n Referênte a Não Conformidade({numeroRegistro})",
                IdEmissor = naoConformidade.IdUsuarioAlterou,
                IdUsuarioAlterou = naoConformidade.IdUsuarioAlterou,
                IdUsuarioIncluiu = naoConformidade.IdUsuarioAlterou,
                IdTipoNaoConformidade = naoConformidade.IdTipoNaoConformidade,
                TipoRegistro = "nc",
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,
                IdResponsavelInicarAcaoImediata = naoConformidade.IdResponsavelInicarAcaoImediata,
                IdResponsavelEtapa = naoConformidade.IdResponsavelInicarAcaoImediata.Value,
                IdSite = naoConformidade.IdSite,
                IdProcesso = naoConformidade.IdProcesso,
                Causa = naoConformidade.Causa,
                StatusRegistro = naoConformidade.StatusRegistro
            };
        }

        private bool TemAcaoImediata(RegistroConformidade registroConformidade) => (registroConformidade.AcoesImediatas.Count > 0);

        public void ValidaUsuarioPorSegundaEtapa(RegistroConformidade naoConformidade, Usuario usuarioLogado, ref List<string> erros)
        {
            var validacaoResponsavel = new AptoParaSalvarSegundaEtapaResponsavelPorEtapaValidacao()
                                    .Validate(naoConformidade);

            var validacaroPerfil = new AptoParaSalvarSegundaEtapaPerfilCoordenadorPorEtapaValidacao()
                                            .Validate(usuarioLogado);

            if (validacaoResponsavel.IsValid || validacaroPerfil.IsValid)
            {
            }
            else
            {
                if (validacaoResponsavel.Errors.Count > 0)
                {
                    erros.AddRange(UtilsServico.PopularErros(validacaoResponsavel.Errors));
                }
                if (validacaroPerfil.Errors.Count > 0)
                {
                    erros.AddRange(UtilsServico.PopularErros(validacaroPerfil.Errors));
                }

            }

        }

        public void ValidaDestravamento(int idPerfil, ref List<string> erros)
        {

            if (idPerfil == (int)PerfisAcesso.Colaborador)
            {
                var erro = new DomainValidation.Validation.ValidationResult();
                erro.Add(new DomainValidation.Validation.ValidationError(Traducao.Resource.NaoConformidade_msg_erro_Destravar));
                erros.AddRange(UtilsServico.PopularErros(erro));
            }
        }

        public void ValidarCamposAcoesImediata(ICollection<RegistroAcaoImediata> acoesImediatas, RegistroConformidade registro, int idUsuarioLogado, ref List<string> erros)
        {
            var erroAux = new List<string>();
            Parallel.ForEach(acoesImediatas, acaoImediata =>
            {
                erroAux = TrataAcaoImediata(acaoImediata, registro, idUsuarioLogado, erroAux);
            });
            erroAux.AddRange(erroAux);
        }

        private AbstractValidator<RegistroConformidade> DefineEtapa(RegistroConformidade registroConformidade)
        {
            var objCtx = _registroConformidadesRepositorio.GetById(registroConformidade.IdRegistroConformidade) != null;
            if (objCtx == false && registroConformidade.StatusEtapa == 1)
            {
                switch (registroConformidade.TipoRegistro)
                {
                    case "nc":
                        var camposObrigatoriosEtapa1 = new CriacaoNCViewValidation();
                        return camposObrigatoriosEtapa1;

                    case "ac":
                        var camposObrigatoriosAcaoCorretivaEtapa1 = new CamposObrigatoriosAcaoCorretivaEtapa1Validation();
                        return camposObrigatoriosAcaoCorretivaEtapa1;

                    case "gr":
                        var camposObrigatoriosGestaoDeRiscoEtapa1 = new CamposObrigatoriosGestaoDeRiscoEtapa1Validation();
                        return camposObrigatoriosGestaoDeRiscoEtapa1;
                }
            }
            else if (objCtx == true && registroConformidade.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata)
            {
                switch (registroConformidade.TipoRegistro)
                {
                    case "nc":
                        var camposObrigatoriosEtapa1 = new CriacaoNCViewValidation();
                        return camposObrigatoriosEtapa1;

                    case "ac":
                        var camposObrigatoriosNaoConformidadeEtapa1 = new CamposObrigatoriosAcaoCorretivaEtapa1Validation();
                        return camposObrigatoriosNaoConformidadeEtapa1;

                    case "gr":
                        var camposObrigatoriosAcaoCorretivaEtapa1 = new CamposObrigatoriosGestaoDeRiscoEtapa1Validation();
                        return camposObrigatoriosAcaoCorretivaEtapa1;
                }
            }
            else if (objCtx == true && registroConformidade.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao)
            {
                switch (registroConformidade.TipoRegistro)
                {
                    case "nc":
                        var camposObrigatoriosEtapa2 = new CamposObrigatoriosNaoConformidadeEtapa2Validation();
                        return camposObrigatoriosEtapa2;

                    case "ac":
                        var camposObrigatoriosNaoConformidadeEtapa2 = new CamposObrigatoriosAcaoCorretivaEtapa2Validation();
                        return camposObrigatoriosNaoConformidadeEtapa2;

                    case "gr":
                        var camposObrigatoriosAcaoCorretivaEtapa2 = new CamposObrigatoriosGestaoDeRiscoEtapa2Validation();
                        return camposObrigatoriosAcaoCorretivaEtapa2;
                }

            }
            else if (objCtx == true && registroConformidade.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada)
            {
                switch (registroConformidade.TipoRegistro)
                {
                    case "nc":
                        var camposObrigatoriosNaoConformidadeEtapa4 = new CamposObrigatoriosNaoConformidadeEtapa4Validation();
                        return camposObrigatoriosNaoConformidadeEtapa4;

                    case "ac":
                        var camposObrigatoriosAcaoCorretivaEtapa4 = new CamposObrigatoriosAcaoCorretivaEtapa4Validation();
                        return camposObrigatoriosAcaoCorretivaEtapa4;

                    case "gr":
                        var camposObrigatoriosGestaoDeRiscoEtapa4 = new CamposObrigatoriosGestaoDeRiscoEtapa4Validation();
                        return camposObrigatoriosGestaoDeRiscoEtapa4;
                }
            }
            return null;
        }

        public RegistroConformidade DestravarEtapa(RegistroConformidade registroConformidade)
        {
            registroConformidade.FlDesbloqueado = 1;
            _registroConformidadesRepositorio.Update(registroConformidade);

            return registroConformidade;
        }

        public Int64 GeraProximoNumeroRegistro(string tipoRegistro, int idSite, int? idProcesso = null)
        {
            RegistroConformidade registroConformidade = _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(new RegistroConformidade { TipoRegistro = tipoRegistro, IdSite = idSite, IdProcesso = idProcesso });
            return registroConformidade.NuRegistro;
        }


        public DataTable RetornarDadosGrafico(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int? idCliente, int idSite, int tipoGrafico)
        {
            var dtDados = new DataTable();

            switch (tipoGrafico)
            {
                case 1:
                    dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsMes(dtDe, dtAte, idTipoNaoConformidade, idSite);
                    break;
                case 2:
                    dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsAbertasFechadas(dtDe, dtAte, idTipoNaoConformidade, idSite);
                    break;
                case 3:
                    dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsTipo(dtDe, dtAte, idTipoNaoConformidade, idSite);
                    break;
                case 4:
                    dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsAcaoCorretiva(dtDe, dtAte, idTipoNaoConformidade, idSite);
                    break;
                case 5:
                    dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsProcesso(dtDe, dtAte, idTipoNaoConformidade, idSite);
                    break;
                case 6:
                    dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsSite(dtDe, dtAte, idTipoNaoConformidade, idCliente.Value);
                    break;

                default:
                    break;
            }

            return dtDados;
        }


        private DataTable RetornarDadosGraficoNcsMes(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite)
        {
            var dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsMes(dtDe, dtAte, idTipoNaoConformidade, idSite);

            return dtDados;
        }

        private DataTable RetornarDadosGraficoNcsTipo(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int idSite)
        {
            var dtDados = _registroConformidadesRepositorio.RetornarDadosGraficoNcsTipo(dtDe, dtAte, idTipoNaoConformidade, idSite);

            return dtDados;
        }



    }
}
