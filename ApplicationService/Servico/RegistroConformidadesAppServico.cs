using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using Dominio.Validacao.RegistroConformidades.AcaoCorretivas;
using Dominio.Validacao.RegistroConformidades.GestaoDeRiscos;
using Dominio.Validacao.RegistroConformidades.NaoConformidades;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dominio.Servico;

namespace ApplicationService.Servico
{
    public class RegistroConformidadesAppServico : BaseServico<RegistroConformidade>, IRegistroConformidadesAppServico
    {
        private readonly IRegistroConformidadesRepositorio _registroConformidadesRepositorio;
        private readonly IRegistroAcaoImediataRepositorio _registroAcaoImediataRepositorio;
        private readonly IAnexoRepositorio _anexoRepositorio;
        private readonly IArquivoDeEvidenciaAcaoImediataRepositorio _arquivoDeEvidenciaAcaoImediataRepositorio;
        private readonly IArquivoDeEvidenciaRegistroConformidadeRepositorio _arquivoDeEvidenciaRegistroConformidadeRepositorio;

        private readonly INotificacaoAppServico _notificacaoServico;
        private readonly IAnexoAppServico _AnexoAppServico;
        private readonly IArquivoNaoConformidadeAnexoRepositorio _arquivoNaoConformidadeAnexoRepositorio;
        public RegistroConformidadesAppServico(
            IRegistroConformidadesRepositorio registroConformidadesRepositorio,
            IRegistroAcaoImediataRepositorio registroAcaoImediataRepositorio,
            INotificacaoAppServico notificacaoServico,
            IAnexoRepositorio anexoRepositorio,
            IArquivoDeEvidenciaAcaoImediataRepositorio arquivoDeEvidenciaAcaoImediataRepositorio,
            IArquivoDeEvidenciaRegistroConformidadeRepositorio arquivoDeEvidenciaRegistroConformidadeRepositorio,
            IAnexoAppServico anexoAppServico,
            IArquivoNaoConformidadeAnexoRepositorio arquivoNaoConformidadeAnexoRepositorio,
            IUsuarioRepositorio usuarioRepositorio)
            : base(registroConformidadesRepositorio)
        {
            _AnexoAppServico = anexoAppServico;
            _arquivoNaoConformidadeAnexoRepositorio = arquivoNaoConformidadeAnexoRepositorio;
            _registroConformidadesRepositorio = registroConformidadesRepositorio;
            _registroAcaoImediataRepositorio = registroAcaoImediataRepositorio;
            _arquivoDeEvidenciaAcaoImediataRepositorio = arquivoDeEvidenciaAcaoImediataRepositorio;
            _arquivoDeEvidenciaRegistroConformidadeRepositorio = arquivoDeEvidenciaRegistroConformidadeRepositorio;
            _notificacaoServico = notificacaoServico;
            _anexoRepositorio = anexoRepositorio;
        }

        public RegistroConformidade SalvarPrimeiraEtapa(RegistroConformidade registroConformidade)
        {
            if (registroConformidade.IdResponsavelInicarAcaoImediata.HasValue)
                registroConformidade.IdResponsavelEtapa = registroConformidade.IdResponsavelInicarAcaoImediata.Value;

            registroConformidade = _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(registroConformidade);
            _registroConformidadesRepositorio.Add(registroConformidade);
            SalvarArquivoNaoConformidade(registroConformidade);

            return registroConformidade;
        }

        private void SalvarArquivoNaoConformidade(RegistroConformidade nc)
        {
            //se ainda nao salvou, esperamos apra processar e vamos ser chamados de novo depois que criar o registro
            if (nc.IdRegistroConformidade == 0)
                return;

            //aqui o nc está vindo da tela
            if (nc.ArquivosNaoConformidadeAnexos != null)
            {
                foreach (var arquivoEvidencia in nc.ArquivosNaoConformidadeAnexos)
                {
                    if (arquivoEvidencia.ApagarAnexo == 1)
                    {
                        //apagamos deirtamente do anexo
                        //ninguem mais pode estar usando esse anexo

                        //tem que ser removida pelo servico, e não da lista
                        _AnexoAppServico.Remove(_AnexoAppServico.GetById(arquivoEvidencia.IdAnexo));
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

                    if (arquivoEvidencia == null)
                        continue;
                    if (arquivoEvidencia.Anexo == null)
                        continue;
                    if (string.IsNullOrEmpty(arquivoEvidencia.Anexo.Extensao))
                        continue;
                    if (string.IsNullOrEmpty(arquivoEvidencia.Anexo.ArquivoB64))
                        continue;

                    if (arquivoEvidencia.Anexo.ArquivoB64 == "undefined")
                        continue;


                    Anexo anexoAtual = _AnexoAppServico.GetById(arquivoEvidencia.IdAnexo);
                    if (anexoAtual == null)
                    {
                        arquivoEvidencia.Anexo.TratarComNomeCerto();
                        arquivoEvidencia.IdRegistroConformidade = nc.IdRegistroConformidade;
                        _arquivoNaoConformidadeAnexoRepositorio.Add(arquivoEvidencia);
                    }

                }

                //limpar para nao reprocessar
                nc.ArquivosNaoConformidadeAnexos = new List<ArquivoNaoConformidadeAnexo>();
            }
        }

        public RegistroConformidade SalvarSegundaEtapa(RegistroConformidade registroConformidade, Funcionalidades funcionalidade)
        {
            SalvarArquivoNaoConformidade(registroConformidade);
            registroConformidade = TrataRegistroConformidadeParaSerAtualizada(registroConformidade);

            registroConformidade.AcoesImediatas.ToList().ForEach(x =>
            {
                x.Registro = registroConformidade;
                x.IdRegistroConformidade = registroConformidade.IdRegistroConformidade;

            });

            _notificacaoServico.RemovePorFuncionalidade(funcionalidade, registroConformidade.IdRegistroConformidade);

            _registroConformidadesRepositorio.Update(registroConformidade);

            //para salvar so anexos
            SalvarArquivoNaoConformidade(registroConformidade);

            return registroConformidade;
        }

        private void GerarFilaEmailNotificacao(RegistroAcaoImediata x)
        {

        }

        private RegistroConformidade TrataAC(RegistroConformidade acaoCorretiva)
        {
            var objCtx = _registroConformidadesRepositorio.GetById(acaoCorretiva.IdRegistroConformidade);
            objCtx.Parecer = acaoCorretiva.Parecer;

            var listaAcaoImediataUpdate = acaoCorretiva.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
            var temAcoesImediataParaAtualizar = listaAcaoImediataUpdate.FirstOrDefault() != null;

            if (acaoCorretiva.OStatusEImplementacao() && temAcoesImediataParaAtualizar == false)
            {
                TrataRegistroQuandoEntraEmFaseDeImplementacao(acaoCorretiva, objCtx);
            }
            else if (acaoCorretiva.OStatusEImplementacao() && temAcoesImediataParaAtualizar == true)
            {
                TrataQuandoResponsavelPorAcaoImediataAtualizaADataDeImplementacao(acaoCorretiva, listaAcaoImediataUpdate, objCtx);
            }
            else if (acaoCorretiva.OStatusEReverificacao() && temAcoesImediataParaAtualizar == true)
            {
                TrataRegistroAprovacaoReverificador(acaoCorretiva, listaAcaoImediataUpdate.ToList(), objCtx);
            }
            else if (acaoCorretiva.OStatusEEncerrada())
            {
                TrataRegistroAprovacaoReverificador(acaoCorretiva, listaAcaoImediataUpdate.ToList(), objCtx);
            }
            else if (acaoCorretiva.OStatusAnulada())
            {
                TrataRegistroAnulada(acaoCorretiva, listaAcaoImediataUpdate.ToList(), objCtx);
            }

            return objCtx;
        }

        private RegistroConformidade TrataGR(RegistroConformidade gestaoDeRisco)
        {
            var objCtx = _registroConformidadesRepositorio.GetById(gestaoDeRisco.IdRegistroConformidade);
            objCtx.Parecer = gestaoDeRisco.Parecer;

            var listaAcaoImediataUpdate = gestaoDeRisco.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
            var temAcoesImediataParaAtualizar = listaAcaoImediataUpdate.FirstOrDefault() != null;

            if (gestaoDeRisco.OStatusEImplementacao() && temAcoesImediataParaAtualizar == false)
            {
                TrataRegistroQuandoEntraEmFaseDeImplementacao(gestaoDeRisco, objCtx);

            }
            else if (gestaoDeRisco.OStatusEImplementacao() && temAcoesImediataParaAtualizar == true)
            {
                TrataQuandoResponsavelPorAcaoImediataAtualizaADataDeImplementacao(gestaoDeRisco, listaAcaoImediataUpdate, objCtx);
            }
            else
            {
                TrataRegistroAprovacaoReverificador(gestaoDeRisco, listaAcaoImediataUpdate.ToList(), objCtx);
            }
            return objCtx;
        }
        private RegistroConformidade TrataGM(RegistroConformidade gestaoDeRisco)
        {
            var objCtx = _registroConformidadesRepositorio.GetById(gestaoDeRisco.IdRegistroConformidade);
            objCtx.Parecer = gestaoDeRisco.Parecer;

            var listaAcaoImediataUpdate = gestaoDeRisco.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
            var temAcoesImediataParaAtualizar = listaAcaoImediataUpdate.FirstOrDefault() != null;

            if (gestaoDeRisco.OStatusEImplementacao() && temAcoesImediataParaAtualizar == false)
            {
                TrataRegistroQuandoEntraEmFaseDeImplementacao(gestaoDeRisco, objCtx);

            }
            else if (gestaoDeRisco.OStatusEImplementacao() && temAcoesImediataParaAtualizar == true)
            {
                TrataQuandoResponsavelPorAcaoImediataAtualizaADataDeImplementacao(gestaoDeRisco, listaAcaoImediataUpdate, objCtx);
            }
            else if (gestaoDeRisco.OStatusEReverificacao() && temAcoesImediataParaAtualizar == true)
            {

                VerificaAtualizaAcaoCorretiva(objCtx, gestaoDeRisco);

                TrataRegistroQuandoEntraEmFaseDeImplementacao(gestaoDeRisco, objCtx);

                objCtx.ECorrecao = gestaoDeRisco.ECorrecao != null ? gestaoDeRisco.ECorrecao : objCtx.ECorrecao;
                objCtx.NecessitaAcaoCorretiva = gestaoDeRisco.NecessitaAcaoCorretiva != null ? gestaoDeRisco.NecessitaAcaoCorretiva : objCtx.NecessitaAcaoCorretiva;
                objCtx.DescricaoAnaliseCausa = gestaoDeRisco.DescricaoAnaliseCausa;
                objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva = gestaoDeRisco.IdResponsavelPorIniciarTratativaAcaoCorretiva;
                objCtx.DtEfetivaImplementacao = gestaoDeRisco.DtEfetivaImplementacao;
                objCtx.DtPrazoImplementacao = gestaoDeRisco.DtPrazoImplementacao;

                foreach (var objacao in objCtx.AcoesImediatas)
                {
                    foreach (var naoconfacao in gestaoDeRisco.AcoesImediatas)
                    {
                        if (objacao.IdAcaoImediata == naoconfacao.IdAcaoImediata)
                        {
                            objacao.DtPrazoImplementacao = naoconfacao.DtPrazoImplementacao;
                            objacao.Descricao = naoconfacao.Descricao;

                        }
                    }
                }

                objCtx.DescricaoAcao = gestaoDeRisco.DescricaoAcao;
                objCtx.DescricaoRegistro = gestaoDeRisco.DescricaoRegistro;

                TrataRegistroAprovacaoReverificador(gestaoDeRisco, listaAcaoImediataUpdate.ToList(), objCtx);

            }
            else
            {
                TrataRegistroAprovacaoReverificador(gestaoDeRisco, listaAcaoImediataUpdate.ToList(), objCtx);
            }
            return objCtx;
        }
        private RegistroConformidade TrataNC(RegistroConformidade naoConformidade)
        {
            var objCtx = _registroConformidadesRepositorio.GetById(naoConformidade.IdRegistroConformidade);

            var acaoImediataUpdate = naoConformidade.AcoesImediatas.Where(x => x.IdAcaoImediata == 0).ToList();

            if (objCtx.IdResponsavelReverificador != null)
                naoConformidade.IdResponsavelReverificador = objCtx.IdResponsavelReverificador;

            foreach (var item in acaoImediataUpdate)
            {
                objCtx.AcoesImediatas.Add(item);
            }

            objCtx.Parecer = naoConformidade.Parecer;

            var listaAcaoImediataUpdate = naoConformidade.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
            listaAcaoImediataUpdate.ToList().ForEach(x =>
            {
                x.Registro = naoConformidade;
                x.IdRegistroConformidade = naoConformidade.IdRegistroConformidade;
            });

            objCtx.StatusEtapa = naoConformidade.StatusEtapa;

            var temAcoesImediataParaAtualizar = listaAcaoImediataUpdate.FirstOrDefault() != null;

            if (naoConformidade.OStatusEEncerrada() && naoConformidade.EProcedente == false)
            {

                //var registroAcaoCorretiva = _registroConformidadesRepositorio.Get(x => x.IdRegistroPai == objCtx.IdRegistroConformidade).FirstOrDefault();
                //if (registroAcaoCorretiva != null)
                //{
                //    registroAcaoCorretiva.DescricaoRegistro = naoConformidade.DescricaoAnaliseCausa;
                //    registroAcaoCorretiva.StatusEtapa = 4;
                //    registroAcaoCorretiva.DtEnceramento = DateTime.Now;
                //    _registroConformidadesRepositorio.Update(registroAcaoCorretiva);
                //}
                VerificaAtualizaAcaoCorretiva(objCtx, naoConformidade);

                TrataRegistroQuandoEntraEmFaseDeImplementacao(naoConformidade, objCtx);

                objCtx.EProcedente = naoConformidade.EProcedente;
                objCtx.StatusEtapa = naoConformidade.StatusEtapa;
                objCtx.DtDescricaoAcao = naoConformidade.DtDescricaoAcao;
                objCtx.DescricaoAcao = naoConformidade.DescricaoAcao;
                objCtx.IdUsuarioAlterou = naoConformidade.IdUsuarioAlterou;
                objCtx.DtEnceramento = DateTime.Now;

            }
            else if (naoConformidade.OStatusEAcaoImediata() && naoConformidade.EProcedente == false)
            {

                VerificaAtualizaAcaoCorretiva(objCtx, naoConformidade);

                TrataRegistroQuandoEntraEmFaseDeImplementacao(naoConformidade, objCtx);

                objCtx.EProcedente = naoConformidade.EProcedente;
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtDescricaoAcao = naoConformidade.DtDescricaoAcao;
                objCtx.DescricaoAcao = naoConformidade.DescricaoAcao;
                objCtx.IdUsuarioAlterou = naoConformidade.IdUsuarioAlterou;
                objCtx.DtEnceramento = DateTime.Now;

            }
            else if (naoConformidade.OStatusEImplementacao() && naoConformidade.EProcedente == false)
            {

                //var registroAcaoCorretiva = _registroConformidadesRepositorio.Get(x => x.IdRegistroPai == objCtx.IdRegistroConformidade).FirstOrDefault();
                //if (registroAcaoCorretiva != null)
                //{
                //    registroAcaoCorretiva.DescricaoRegistro = naoConformidade.DescricaoAnaliseCausa;
                //    registroAcaoCorretiva.StatusEtapa = 4;
                //    registroAcaoCorretiva.DtEnceramento = DateTime.Now;
                //    _registroConformidadesRepositorio.Update(registroAcaoCorretiva);
                //}
                VerificaAtualizaAcaoCorretiva(objCtx, naoConformidade);

                TrataRegistroQuandoEntraEmFaseDeImplementacao(naoConformidade, objCtx);

                objCtx.EProcedente = naoConformidade.EProcedente;
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtDescricaoAcao = naoConformidade.DtDescricaoAcao;
                objCtx.DescricaoAcao = naoConformidade.DescricaoAcao;
                objCtx.IdUsuarioAlterou = naoConformidade.IdUsuarioAlterou;
                objCtx.DtEnceramento = DateTime.Now;

            }
            else if (naoConformidade.OStatusEImplementacao() && naoConformidade.EProcedente == true && temAcoesImediataParaAtualizar == false)
            {
                TrataRegistroQuandoEntraEmFaseDeImplementacao(naoConformidade, objCtx);

                objCtx.EProcedente = naoConformidade.EProcedente != null ? naoConformidade.EProcedente : objCtx.EProcedente;
                objCtx.ECorrecao = naoConformidade.ECorrecao != null ? naoConformidade.ECorrecao : objCtx.ECorrecao;
                objCtx.NecessitaAcaoCorretiva = naoConformidade.NecessitaAcaoCorretiva != null ? naoConformidade.NecessitaAcaoCorretiva : objCtx.NecessitaAcaoCorretiva;
                objCtx.DescricaoAnaliseCausa = naoConformidade.DescricaoAnaliseCausa;
                objCtx.Parecer = naoConformidade.Parecer;
                objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva = naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva;
                objCtx.DtEfetivaImplementacao = naoConformidade.DtEfetivaImplementacao;
                objCtx.DescricaoAcao = naoConformidade.DescricaoAcao;
                objCtx.DescricaoRegistro = naoConformidade.DescricaoRegistro;
                objCtx.IdResponsavelReverificador = naoConformidade.IdResponsavelReverificador;

                if (naoConformidade.NecessitaAcaoCorretiva == true && naoConformidade.IdNuRegistroFilho == null)
                {

                    var acaoImediata = naoConformidade.AcoesImediatas.FirstOrDefault();

                    var novaAc = CriarAcaoCorretivaApartirDeNaoConformidade(objCtx);

                    novaAc.DescricaoRegistro += $"\n\n Referênte a Não Conformidade({objCtx.NuRegistro})";
                    novaAc.DescricaoAcao = acaoImediata.Descricao;
                    novaAc.DtPrazoImplementacao = acaoImediata.DtPrazoImplementacao;

                    _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaAc);
                    _registroConformidadesRepositorio.Add(novaAc);

                    objCtx.IdNuRegistroFilho = novaAc.NuRegistro;
                }

            }
            else if (naoConformidade.OStatusEImplementacao() && temAcoesImediataParaAtualizar == true)
            {

                VerificaAtualizaAcaoCorretiva(objCtx, naoConformidade);

                var listaAcaoImediataNaoImplementadas = naoConformidade.AcoesImediatas.FirstOrDefault(x => x.DtEfetivaImplementacao == null) != null;

                TrataRegistroQuandoEntraEmFaseDeImplementacao(naoConformidade, objCtx);

                objCtx.EProcedente = naoConformidade.EProcedente != null ? naoConformidade.EProcedente : objCtx.EProcedente;
                objCtx.ECorrecao = naoConformidade.ECorrecao != null ? naoConformidade.ECorrecao : objCtx.ECorrecao;
                objCtx.NecessitaAcaoCorretiva = naoConformidade.NecessitaAcaoCorretiva != null ? naoConformidade.NecessitaAcaoCorretiva : objCtx.NecessitaAcaoCorretiva;
                objCtx.DescricaoAnaliseCausa = naoConformidade.DescricaoAnaliseCausa;
                objCtx.Parecer = naoConformidade.Parecer;
                objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva = naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva != 0 ? naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva : objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva;
                objCtx.DtEfetivaImplementacao = naoConformidade.DtEfetivaImplementacao;
                objCtx.DescricaoAcao = naoConformidade.DescricaoAcao;
                objCtx.DescricaoRegistro = naoConformidade.DescricaoRegistro;
                objCtx.IdResponsavelReverificador = naoConformidade.IdResponsavelReverificador;

                if (naoConformidade.NecessitaAcaoCorretiva.Value)
                {
                    if (objCtx.IdNuRegistroFilho == 0 || objCtx.IdNuRegistroFilho == null)
                    {
                        var acaoImediata = naoConformidade.AcoesImediatas.FirstOrDefault();

                        var novaAc = CriarAcaoCorretivaApartirDeNaoConformidade(objCtx);

                        novaAc.DescricaoRegistro += $"\n\n Referênte a Não Conformidade({objCtx.NuRegistro})";
                        novaAc.DescricaoAcao = acaoImediata.Descricao;
                        novaAc.DtPrazoImplementacao = acaoImediata.DtPrazoImplementacao;
                        //IdNuRegistroFilho

                        _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaAc);
                        _registroConformidadesRepositorio.Add(novaAc);

                        objCtx.IdNuRegistroFilho = novaAc.NuRegistro;
                    }
                }

                if (listaAcaoImediataNaoImplementadas == false)
                {
                    objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Reverificacao;
                }
                bool acoesImediatasIncolpletas = false;
                foreach (var item in naoConformidade.AcoesImediatas)
                {
                    if (item.DtEfetivaImplementacao == null)
                    {
                        acoesImediatasIncolpletas = true;
                        break;
                    }
                }
                //naoConformidade.EProcedente 
                if (naoConformidade.ECorrecao == false || naoConformidade.ECorrecao == null && !acoesImediatasIncolpletas)
                {
                    objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                    objCtx.DtEnceramento = DateTime.Now;
                }

            }
            else if (naoConformidade.OStatusEReverificacao() && temAcoesImediataParaAtualizar == true)
            {
                VerificaAtualizaAcaoCorretiva(objCtx, naoConformidade);

                TrataRegistroQuandoEntraEmFaseDeImplementacao(naoConformidade, objCtx);

                objCtx.EProcedente = naoConformidade.EProcedente != null ? naoConformidade.EProcedente : objCtx.EProcedente;
                objCtx.ECorrecao = naoConformidade.ECorrecao != null ? naoConformidade.ECorrecao : objCtx.ECorrecao;
                objCtx.NecessitaAcaoCorretiva = naoConformidade.NecessitaAcaoCorretiva != null ? naoConformidade.NecessitaAcaoCorretiva : objCtx.NecessitaAcaoCorretiva;
                objCtx.DescricaoAnaliseCausa = naoConformidade.DescricaoAnaliseCausa;
                objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva = naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva;
                objCtx.DtEfetivaImplementacao = naoConformidade.DtEfetivaImplementacao;
                objCtx.DtPrazoImplementacao = naoConformidade.DtPrazoImplementacao;

                foreach (var objacao in objCtx.AcoesImediatas)
                {
                    foreach (var naoconfacao in naoConformidade.AcoesImediatas)
                    {
                        if (objacao.IdAcaoImediata == naoconfacao.IdAcaoImediata)
                        {
                            objacao.DtPrazoImplementacao = naoconfacao.DtPrazoImplementacao;
                            objacao.Descricao = naoconfacao.Descricao;

                        }
                    }
                }

                objCtx.DescricaoAcao = naoConformidade.DescricaoAcao;
                objCtx.DescricaoRegistro = naoConformidade.DescricaoRegistro;

                TrataRegistroAprovacaoReverificador(naoConformidade, listaAcaoImediataUpdate.ToList(), objCtx);
            }
            else if (naoConformidade.OStatusEAcaoImediata())
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;


            }
            else
            {
                VerificaAtualizaAcaoCorretiva(objCtx, naoConformidade);

                AtualizaAcoesImediatas(listaAcaoImediataUpdate.ToList(), objCtx);

                TrataRegistroAprovacaoReverificador(naoConformidade, listaAcaoImediataUpdate.ToList(), objCtx);


                if (naoConformidade.NecessitaAcaoCorretiva.Value)
                {
                    if (objCtx.IdNuRegistroFilho == 0 || objCtx.IdNuRegistroFilho == null)
                    {
                        var acaoImediata = naoConformidade.AcoesImediatas.FirstOrDefault();

                        var novaAc = CriarAcaoCorretivaApartirDeNaoConformidade(objCtx);

                        novaAc.DescricaoRegistro += $"\n\n Referênte a Não Conformidade({objCtx.NuRegistro})";
                        novaAc.DescricaoAcao = acaoImediata.Descricao;
                        novaAc.DtPrazoImplementacao = acaoImediata.DtPrazoImplementacao;
                        //IdNuRegistroFilho

                        _registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaAc);
                        _registroConformidadesRepositorio.Add(novaAc);

                        objCtx.IdNuRegistroFilho = novaAc.NuRegistro;
                    }
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
                case "gm":
                    registroConformidade = TrataGM(registroConformidade);
                    break;

                default:
                    break;
            }

            registroConformidade.IdProcesso = registroConformidade.IdProcesso == 0 ? null : registroConformidade.IdProcesso;

            return registroConformidade;

        }

        private void TrataRegistroQuandoEntraEmFaseDeImplementacao(RegistroConformidade registroConformidade, RegistroConformidade objCtx)
        {
            //limpando tabela para atualização
            //objCtx.ArquivosDeEvidencia.ToList().ForEach(arquivo =>
            //{
            //    _arquivoDeEvidenciaRegistroConformidadeRepositorio.Remove(arquivo);
            //});

            objCtx.IdResponsavelImplementar = registroConformidade.IdResponsavelImplementar;
            objCtx.IdResponsavelReverificador = registroConformidade.IdResponsavelReverificador != null ? registroConformidade.IdResponsavelReverificador : objCtx.IdResponsavelReverificador;
            objCtx.DtPrazoImplementacao = registroConformidade.DtPrazoImplementacao != null ? registroConformidade.DtPrazoImplementacao : objCtx.DtPrazoImplementacao;
            objCtx.DtEfetivaImplementacao = registroConformidade.DtEfetivaImplementacao != null ? registroConformidade.DtEfetivaImplementacao : objCtx.DtEfetivaImplementacao;
            objCtx.DsAcao = registroConformidade.DsAcao != null ? registroConformidade.DsAcao : objCtx.DsAcao;
            objCtx.DtDescricaoAcao = registroConformidade.DtDescricaoAcao != null ? registroConformidade.DtDescricaoAcao : objCtx.DtDescricaoAcao;

            if (objCtx.AcoesImediatas.Count == 0)
            {
                objCtx.AcoesImediatas = registroConformidade.AcoesImediatas;
            }

            objCtx.IdUsuarioAlterou = registroConformidade.IdUsuarioAlterou;
            objCtx.Tags = registroConformidade.Tags;
            objCtx.DescricaoRegistro = registroConformidade.DescricaoRegistro;
            objCtx.IdEmissor = registroConformidade.IdEmissor;
            objCtx.IdProcesso = registroConformidade.IdProcesso != null ? registroConformidade.IdProcesso : objCtx.IdProcesso;
            objCtx.ENaoConformidadeAuditoria = registroConformidade.ENaoConformidadeAuditoria != null ? registroConformidade.ENaoConformidadeAuditoria : objCtx.ENaoConformidadeAuditoria;
            objCtx.IdTipoNaoConformidade = registroConformidade.IdTipoNaoConformidade != null ? registroConformidade.IdTipoNaoConformidade : objCtx.IdTipoNaoConformidade;
            objCtx.NecessitaAcaoCorretiva = registroConformidade.NecessitaAcaoCorretiva != null ? registroConformidade.NecessitaAcaoCorretiva : objCtx.NecessitaAcaoCorretiva;
            objCtx.IdResponsavelInicarAcaoImediata = registroConformidade.IdResponsavelInicarAcaoImediata != null ? registroConformidade.IdResponsavelInicarAcaoImediata : objCtx.IdResponsavelInicarAcaoImediata;
            objCtx.CriticidadeGestaoDeRisco = registroConformidade.CriticidadeGestaoDeRisco != null ? registroConformidade.CriticidadeGestaoDeRisco : objCtx.CriticidadeGestaoDeRisco;
            objCtx.DtEmissao = registroConformidade.DtEmissao != null ? registroConformidade.DtEmissao : objCtx.DtEmissao;
            objCtx.ArquivosDeEvidenciaAux = registroConformidade.ArquivosDeEvidenciaAux.ToList();
            objCtx.ArquivosDeEvidencia = registroConformidade.ArquivosDeEvidencia.ToList();
            objCtx.ArquivosDeEvidencia.ToList().ForEach(arquivo =>
            {
                arquivo.RegistroConformidade = null;

            });

            objCtx.FlEficaz = registroConformidade.FlEficaz != null ? registroConformidade.FlEficaz : objCtx.FlEficaz;
            objCtx.EProcedente = registroConformidade.EProcedente != null ? registroConformidade.EProcedente : objCtx.EProcedente;
            objCtx.IdRegistroConformidade = registroConformidade.IdRegistroConformidade != 0 ? registroConformidade.IdRegistroConformidade : objCtx.IdRegistroConformidade;
            objCtx.Causa = registroConformidade.Causa != null ? registroConformidade.Causa : objCtx.Causa;

            //if(registroConformidade.IdResponsavelReverificador == null)
            //{
            //    throw new Exception("Você deve escolher um responsável por verificação.");
            //}

            bool Implementacao = true;

            objCtx.AcoesImediatas.ToList().ForEach(acaoImediata =>
            {

                //RegistroAcaoImediata acaoUpdate = registroConformidade.AcoesImediatas.Where(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).FirstOrDefault();
                RegistroAcaoImediata acaoUpdate = registroConformidade.AcoesImediatas.Where(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata && x.IdAcaoImediata > 0).FirstOrDefault();
                acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                acaoImediata.ArquivoEvidencia = acaoUpdate != null ? acaoUpdate.ArquivoEvidencia : null;

                if (acaoUpdate != null && acaoUpdate.ArquivoEvidenciaAux != null && !string.IsNullOrEmpty(acaoUpdate.ArquivoEvidenciaAux.Extensao))
                {
                    acaoImediata.ArquivoEvidenciaAux = acaoUpdate != null ? acaoUpdate.ArquivoEvidenciaAux : null;
                }


                //acaoImediata.Descricao = objCtx.DsAcao;
                //acaoImediata.DtPrazoImplementacao = objCtx.DtPrazoImplementacao;

                if (acaoImediata.DtPrazoImplementacao == null)
                {
                    Implementacao = false;

                }

                acaoImediata.IdResponsavelImplementar = acaoUpdate != null && acaoUpdate.IdResponsavelImplementar != null ? acaoUpdate.IdResponsavelImplementar : acaoImediata.IdResponsavelImplementar;
                acaoImediata.DtEfetivaImplementacao = acaoUpdate != null && acaoUpdate.DtEfetivaImplementacao != null ? acaoUpdate.DtEfetivaImplementacao : acaoImediata.DtEfetivaImplementacao;
                acaoImediata.DtPrazoImplementacao = acaoUpdate != null && acaoUpdate.DtPrazoImplementacao != null ? acaoUpdate.DtPrazoImplementacao : acaoImediata.DtPrazoImplementacao;
                if (acaoUpdate != null && acaoUpdate.ComentariosAcaoImediata != null)
                    acaoImediata.ComentariosAcaoImediata = acaoUpdate.ComentariosAcaoImediata;
            });


            var primeiraAcaoImdediata = objCtx.AcoesImediatas.FirstOrDefault();

            if (primeiraAcaoImdediata != null && primeiraAcaoImdediata.DtEfetivaImplementacao != null && primeiraAcaoImdediata.DtEfetivaImplementacao != default(DateTime) || objCtx.StatusEtapa == 4 || objCtx.StatusEtapa == 2)
            {
                AtualizaAcoesImediatas(registroConformidade.AcoesImediatas.ToList(), objCtx);
            }

            if (registroConformidade.EProcedente == false)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
            }
            if (!Implementacao)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata;
            }
            else
            {
                objCtx.StatusEtapa = registroConformidade.StatusEtapa;
            }

        }
        private void TrataRegistroAnulada(RegistroConformidade registroConformidade, List<RegistroAcaoImediata> listaAcaoImediataUpdate, RegistroConformidade objCtx)
        {
            /*
			feito abaixo, mas nao funcinou, continua limpando em todas*/


            if (registroConformidade.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada)
            {
                //List<RegistroAcaoImediata> lista = new List<RegistroAcaoImediata>();
                foreach (var item in listaAcaoImediataUpdate)
                {

                    {
                        //estava limpando todas as ações imediatas
                        var acaoImediata = item;
                        {

                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Aprovado = acaoImediata.Aprovado;
                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).ComentariosAcaoImediata = acaoImediata.ComentariosAcaoImediata;
                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Descricao = acaoImediata.Descricao;

                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).DtPrazoImplementacao = acaoImediata.DtPrazoImplementacao;
                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).IdResponsavelImplementar = acaoImediata.IdResponsavelImplementar;
                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).DtEfetivaImplementacao = acaoImediata.DtEfetivaImplementacao;
                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Observacao = acaoImediata.Observacao;
                            objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).SubmitArquivoEvidencia = acaoImediata.SubmitArquivoEvidencia;

                        }
                    }

                    if (item.Aprovado == false)
                    {
                        //{
                        //    //estava limpando todas as ações imediatas
                        //    var acaoImediata = item;
                        //    {
                        //        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Aprovado = acaoImediata.Aprovado;
                        //        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).ComentariosAcaoImediata = acaoImediata.ComentariosAcaoImediata;
                        //    }
                        //}

                        //tem que apagar os anexos dessa linha
                        var acaoImediataparaLimpar = objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata);
                        if (acaoImediataparaLimpar != null)
                        {
                            foreach (var arquivoAcaoImediata in acaoImediataparaLimpar.ArquivoEvidencia)
                            {
                                //apagamos deirtamente do anexo
                                _AnexoAppServico.Remove(_AnexoAppServico.GetById(arquivoAcaoImediata.IdAnexo));
                            }
                        }

                        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).DtEfetivaImplementacao = null;

                        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).Observacao = string.Empty;

                        //_anexoRepositorio.Remove(objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).ArquivoEvidenciaAux);
                    }

                    //RegistroAcaoImediata local = new RegistroAcaoImediata();
                    //local = item;
                    //if (item.Aprovado != null && item.Aprovado == false)
                    //{
                    //local.DtEfetivaImplementacao = null;
                    ////local.ArquivoEvidencia = new List<ArquivoDeEvidenciaAcaoImediata>();
                    ////local.ArquivoEvidenciaAux = new Anexo();
                    //}
                    //lista.Add(local);
                }
                //objCtx.AcoesImediatas = lista;
            }

            //listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
            //{
            //    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata && acaoImediata.Aprovado == false).DtEfetivaImplementacao = null;
            //    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata && acaoImediata.Aprovado == false).ArquivoEvidencia = new List<ArquivoDeEvidenciaAcaoImediata>();
            //    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata && acaoImediata.Aprovado == false).ArquivoEvidenciaAux = null;
            //});
            objCtx.NecessitaAcaoCorretiva = registroConformidade.NecessitaAcaoCorretiva;
            objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva = registroConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva;
            objCtx.DescricaoAnaliseCausa = registroConformidade.DescricaoAnaliseCausa;

            objCtx.IdTipoNaoConformidade = registroConformidade.IdTipoNaoConformidade;
            objCtx.ECorrecao = registroConformidade.ECorrecao;


            objCtx.DtDescricaoAcao = registroConformidade.DtDescricaoAcao;
            objCtx.Tags = registroConformidade.Tags;
            objCtx.DescricaoRegistro = registroConformidade.DescricaoRegistro;
            objCtx.IdEmissor = registroConformidade.IdEmissor;
            objCtx.IdProcesso = registroConformidade.IdProcesso;
            objCtx.IdResponsavelInicarAcaoImediata = registroConformidade.IdResponsavelInicarAcaoImediata;
            objCtx.CriticidadeGestaoDeRisco = registroConformidade.CriticidadeGestaoDeRisco;
            objCtx.DtEmissao = registroConformidade.DtEmissao;
            objCtx.EProcedente = registroConformidade.EProcedente;
            objCtx.FlEficaz = registroConformidade.FlEficaz;
            objCtx.Causa = registroConformidade.Causa;

            objCtx.IdResponsavelImplementar = registroConformidade.IdResponsavelImplementar;
            objCtx.IdResponsavelReverificador = registroConformidade.IdResponsavelReverificador;
            objCtx.DtPrazoImplementacao = registroConformidade.DtPrazoImplementacao;
            objCtx.DtEfetivaImplementacao = registroConformidade.DtEfetivaImplementacao;
            objCtx.DsAcao = registroConformidade.DsAcao;
            objCtx.StatusRegistro = registroConformidade.StatusRegistro;
            objCtx.DsJustificativa = registroConformidade.DsJustificativa;


            registroConformidade.AcoesImediatas.ToList().ForEach(acaoImediata =>
            {
                acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;

                //acaoImediata.Descricao = objCtx.DsAcao;
                //acaoImediata.DtPrazoImplementacao = objCtx.DtPrazoImplementacao;
                //acaoImediata.IdResponsavelImplementar = objCtx.IdResponsavelImplementar;
                //acaoImediata.DtEfetivaImplementacao = objCtx.DtEfetivaImplementacao;
            });

            if (objCtx.StatusEtapa != (byte)EtapasRegistroConformidade.Anulada)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Anulada;
                objCtx.DtEnceramento = DateTime.Now;
            }

        }

        private void TrataRegistroAprovacaoReverificador(RegistroConformidade registroConformidade, List<RegistroAcaoImediata> listaAcaoImediataUpdate, RegistroConformidade objCtx)
        {
            /*
			feito abaixo, mas nao funcinou, continua limpando em todas*/


            if (registroConformidade.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada)
            {
                //List<RegistroAcaoImediata> lista = new List<RegistroAcaoImediata>();
                foreach (var item in listaAcaoImediataUpdate)
                {

                    {
                        //estava limpando todas as ações imediatas
                        var acaoImediata = item;
                        {
                            if (acaoImediata.IdAcaoImediata != 0)
                            {
                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Aprovado = acaoImediata.Aprovado;
                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).ComentariosAcaoImediata = acaoImediata.ComentariosAcaoImediata;
                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Descricao = acaoImediata.Descricao;

                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).DtPrazoImplementacao = acaoImediata.DtPrazoImplementacao;
                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).IdResponsavelImplementar = acaoImediata.IdResponsavelImplementar;
                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).DtEfetivaImplementacao = acaoImediata.DtEfetivaImplementacao;
                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Observacao = acaoImediata.Observacao;
                                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).SubmitArquivoEvidencia = acaoImediata.SubmitArquivoEvidencia;
                            }
                        }
                    }

                    if (item.Aprovado == false)
                    {
                        //{
                        //    //estava limpando todas as ações imediatas
                        //    var acaoImediata = item;
                        //    {
                        //        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).Aprovado = acaoImediata.Aprovado;
                        //        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata).ComentariosAcaoImediata = acaoImediata.ComentariosAcaoImediata;
                        //    }
                        //}

                        //tem que apagar os anexos dessa linha
                        var acaoImediataparaLimpar = objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata);
                        if (acaoImediataparaLimpar != null)
                        {
                            foreach (var arquivoAcaoImediata in acaoImediataparaLimpar.ArquivoEvidencia)
                            {
                                //apagamos deirtamente do anexo
                                _AnexoAppServico.Remove(_AnexoAppServico.GetById(arquivoAcaoImediata.IdAnexo));
                            }
                        }

                        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).DtEfetivaImplementacao = null;

                        objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).Observacao = string.Empty;

                        //_anexoRepositorio.Remove(objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).ArquivoEvidenciaAux);
                    }

                    //RegistroAcaoImediata local = new RegistroAcaoImediata();
                    //local = item;
                    //if (item.Aprovado != null && item.Aprovado == false)
                    //{
                    //local.DtEfetivaImplementacao = null;
                    ////local.ArquivoEvidencia = new List<ArquivoDeEvidenciaAcaoImediata>();
                    ////local.ArquivoEvidenciaAux = new Anexo();
                    //}
                    //lista.Add(local);
                }
                //objCtx.AcoesImediatas = lista;
            }

            //listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
            //{
            //    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata && acaoImediata.Aprovado == false).DtEfetivaImplementacao = null;
            //    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata && acaoImediata.Aprovado == false).ArquivoEvidencia = new List<ArquivoDeEvidenciaAcaoImediata>();
            //    objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == acaoImediata.IdAcaoImediata && acaoImediata.Aprovado == false).ArquivoEvidenciaAux = null;
            //});
            objCtx.NecessitaAcaoCorretiva = registroConformidade.NecessitaAcaoCorretiva;
            objCtx.IdResponsavelPorIniciarTratativaAcaoCorretiva = registroConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva;
            objCtx.DescricaoAnaliseCausa = registroConformidade.DescricaoAnaliseCausa;

            objCtx.IdTipoNaoConformidade = registroConformidade.IdTipoNaoConformidade;
            objCtx.ECorrecao = registroConformidade.ECorrecao;



            objCtx.Tags = registroConformidade.Tags;
            objCtx.DescricaoRegistro = registroConformidade.DescricaoRegistro;
            objCtx.IdEmissor = registroConformidade.IdEmissor;
            objCtx.IdProcesso = registroConformidade.IdProcesso;
            objCtx.IdResponsavelInicarAcaoImediata = registroConformidade.IdResponsavelInicarAcaoImediata;
            objCtx.CriticidadeGestaoDeRisco = registroConformidade.CriticidadeGestaoDeRisco;
            objCtx.DtEmissao = registroConformidade.DtEmissao;
            objCtx.EProcedente = registroConformidade.EProcedente;
            objCtx.FlEficaz = registroConformidade.FlEficaz;
            objCtx.Causa = registroConformidade.Causa;

            objCtx.IdResponsavelImplementar = registroConformidade.IdResponsavelImplementar;
            objCtx.IdResponsavelReverificador = registroConformidade.IdResponsavelReverificador;
            objCtx.DtPrazoImplementacao = registroConformidade.DtPrazoImplementacao;
            objCtx.DtEfetivaImplementacao = registroConformidade.DtEfetivaImplementacao;
            objCtx.DsAcao = registroConformidade.DsAcao;
            objCtx.StatusRegistro = registroConformidade.StatusRegistro;



            registroConformidade.AcoesImediatas.ToList().ForEach(acaoImediata =>
                    {
                        acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                        acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;

                        //acaoImediata.Descricao = objCtx.DsAcao;
                        //acaoImediata.DtPrazoImplementacao = objCtx.DtPrazoImplementacao;
                        //acaoImediata.IdResponsavelImplementar = objCtx.IdResponsavelImplementar;
                        //acaoImediata.DtEfetivaImplementacao = objCtx.DtEfetivaImplementacao;
                    });

            if (registroConformidade.EProcedente == false)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
            }

            if (objCtx.FlEficaz == false && objCtx.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada)
            {
                var novaRegistro = CriarNovoCasoEficaciaForFalsa(objCtx, objCtx.TipoRegistro);
                if (novaRegistro.TipoRegistro == "gr")
                {
                    if (objCtx.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada)
                    {
                        objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
                    }
                }
                else if (novaRegistro.TipoRegistro == "ac")
                {
                    if (objCtx.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada)
                    {
                        objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
                    }
                }
                else if (novaRegistro.TipoRegistro == "nc")
                {
                    //novaRegistro.DescricaoRegistro += $"\n \n Referênte a Não Conformidade({objCtx.NuRegistro})";

                    //[aqui]
                    //_registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaRegistro);
                    //_registroConformidadesRepositorio.Add(novaRegistro);

                    if (objCtx.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada)
                    {
                        //objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                        //objCtx.DtEnceramento = DateTime.Now;
                        objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;

                    }
                }

                else if (novaRegistro.TipoRegistro == "gm")
                {
                    //novaRegistro.DescricaoRegistro += $"\n \n Referênte a Não Conformidade({objCtx.NuRegistro})";

                    //[aqui]
                    //_registroConformidadesRepositorio.GerarNumeroSequencialPorSite(novaRegistro);
                    //_registroConformidadesRepositorio.Add(novaRegistro);

                    if (objCtx.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada)
                    {
                        //objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                        //objCtx.DtEnceramento = DateTime.Now;
                        objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;

                    }
                }
            }
            else if (objCtx.FlEficaz == true)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
            }
            else if (objCtx.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
            }
            else
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Reverificacao;
            }
        }

        private void TrataQuandoResponsavelPorAcaoImediataAtualizaADataDeImplementacao(RegistroConformidade registroConformidade, IEnumerable<RegistroAcaoImediata> listaAcaoImediataUpdate, RegistroConformidade objCtx)
        {
            if (registroConformidade.DtEfetivaImplementacao < registroConformidade.DtEmissao)
            {
                throw new Exception("Data Efetiva Implementação não pode ser menor que Data de Emissão.");
            }
            AtualizaAcoesImediatas(listaAcaoImediataUpdate.ToList(), objCtx);
            var listaAcaoImediataNaoImplementadas = registroConformidade.AcoesImediatas.FirstOrDefault(x => x.DtEfetivaImplementacao == null) != null;
            if (listaAcaoImediataNaoImplementadas == false)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Reverificacao;
            }
            if (registroConformidade.EProcedente == false)
            {
                objCtx.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada;
                objCtx.DtEnceramento = DateTime.Now;
            }

            objCtx.Tags = registroConformidade.Tags;
            objCtx.DescricaoRegistro = registroConformidade.DescricaoRegistro;
            objCtx.IdEmissor = registroConformidade.IdEmissor;
            objCtx.IdProcesso = registroConformidade.IdProcesso;
            objCtx.IdResponsavelInicarAcaoImediata = registroConformidade.IdResponsavelInicarAcaoImediata;
            objCtx.CriticidadeGestaoDeRisco = registroConformidade.CriticidadeGestaoDeRisco;
            objCtx.DtEmissao = registroConformidade.DtEmissao;
            objCtx.EProcedente = registroConformidade.EProcedente;
            objCtx.Causa = registroConformidade.Causa;

            foreach (var item in registroConformidade.AcoesImediatas)
            {
                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).ArquivoEvidenciaAux = item.ArquivoEvidenciaAux;
                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).ArquivoEvidencia = item.ArquivoEvidencia;

                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).Observacao = item.Observacao;
                objCtx.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).DtEfetivaImplementacao = item.DtEfetivaImplementacao;
                //item.ArquivoEvidencia.ToList().ForEach(arquivo =>
                //{
                //    arquivo.AcaoImediata = null;
                //});

            }

            objCtx.IdResponsavelImplementar = registroConformidade.IdResponsavelImplementar;
            objCtx.IdResponsavelReverificador = registroConformidade.IdResponsavelReverificador;
            objCtx.DtPrazoImplementacao = registroConformidade.DtPrazoImplementacao;
            objCtx.DtEfetivaImplementacao = registroConformidade.DtEfetivaImplementacao;
            objCtx.DsAcao = registroConformidade.DsAcao;

            registroConformidade.AcoesImediatas.ToList().ForEach(acaoImediata =>
            {
                acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;
                //acaoImediata.Descricao = objCtx.DsAcao;
                //acaoImediata.DtPrazoImplementacao = objCtx.DtPrazoImplementacao;
                //acaoImediata.IdResponsavelImplementar = objCtx.IdResponsavelImplementar;
                //acaoImediata.DtEfetivaImplementacao = objCtx.DtEfetivaImplementacao;
            });

        }

        private RegistroConformidade CriarNovoCasoEficaciaForFalsa(RegistroConformidade registroConformidade, string tipo)
        {
            var novoRegistro = new RegistroConformidade()
            {
                DescricaoRegistro = registroConformidade.DescricaoRegistro,// + $"\n Referênte a Não Conformidade({numeroRegistro})",
                IdEmissor = registroConformidade.IdUsuarioAlterou,
                IdUsuarioAlterou = registroConformidade.IdUsuarioAlterou,
                IdUsuarioIncluiu = registroConformidade.IdUsuarioAlterou,
                TipoRegistro = tipo,
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,
                IdResponsavelInicarAcaoImediata = registroConformidade.IdResponsavelInicarAcaoImediata,
                IdResponsavelEtapa = registroConformidade.IdResponsavelInicarAcaoImediata,
                IdSite = registroConformidade.IdSite,
                IdProcesso = registroConformidade.IdProcesso,
                NecessitaAcaoCorretiva = null,
                EProcedente = true,
                Tags = registroConformidade.Tags,
                TipoNaoConformidade = registroConformidade.TipoNaoConformidade,
                ENaoConformidadeAuditoria = registroConformidade.ENaoConformidadeAuditoria
            };


            registroConformidade.ArquivosDeEvidencia.ToList().ForEach(x =>
            {
                novoRegistro.ArquivosDeEvidencia.Add(new ArquivosDeEvidencia
                {
                    IdAnexo = x.IdAnexo,
                    RegistroConformidade = novoRegistro,
                    TipoRegistro = x.TipoRegistro
                });
            });



            return novoRegistro;
        }

        private void AtualizaAcoesImediatas(List<RegistroAcaoImediata> listaAcaoImediataUpdate, RegistroConformidade objCtx)
        {
            listaAcaoImediataUpdate.ToList().ForEach(acaoImediata =>
            {
                acaoImediata.IdUsuarioIncluiu = objCtx.IdResponsavelInicarAcaoImediata;
                acaoImediata.IdRegistroConformidade = objCtx.IdRegistroConformidade;

                foreach (var obj in objCtx.AcoesImediatas)
                {
                    if (obj.IdAcaoImediata == acaoImediata.IdAcaoImediata && obj.IdAcaoImediata != 0)
                    {

                        obj.Observacao = obj.Observacao != acaoImediata.Observacao ? acaoImediata.Observacao : obj.Observacao;
                        obj.Descricao = obj.Descricao != acaoImediata.Descricao ? acaoImediata.Descricao : obj.Descricao;
                        obj.DtPrazoImplementacao = obj.DtPrazoImplementacao != acaoImediata.DtPrazoImplementacao ? acaoImediata.DtPrazoImplementacao : obj.DtPrazoImplementacao;
                        obj.DtEfetivaImplementacao = obj.DtEfetivaImplementacao != acaoImediata.DtEfetivaImplementacao ? acaoImediata.DtEfetivaImplementacao : obj.DtEfetivaImplementacao;
                        //obj.IdRegistroConformidade = obj.IdRegistroConformidade != acaoImediata.IdRegistroConformidade ? acaoImediata.IdRegistroConformidade : obj.IdRegistroConformidade;
                        obj.IdResponsavelImplementar = obj.IdResponsavelImplementar != acaoImediata.IdResponsavelImplementar ? acaoImediata.IdResponsavelImplementar : obj.IdResponsavelImplementar;
                    }
                }
                if (acaoImediata.SubmitArquivoEvidencia != null)
                {
                    foreach (var arquivoAcaoImediata in acaoImediata.SubmitArquivoEvidencia)
                    {
                        if (arquivoAcaoImediata != null && arquivoAcaoImediata.Anexo.ArquivoB64 != null)
                        {
                            if (arquivoAcaoImediata.ApagarAnexo == 1)
                            {
                                //apagamos deirtamente do anexo
                                //ninguem mais pode estar usando esse anexo
                                _AnexoAppServico.Remove(_AnexoAppServico.GetById(arquivoAcaoImediata.IdAnexo));
                                continue;
                            }

                            if (arquivoAcaoImediata == null)
                                continue;
                            if (arquivoAcaoImediata.Anexo == null)
                                continue;
                            if (string.IsNullOrEmpty(arquivoAcaoImediata.Anexo.Extensao))
                                continue;
                            if (string.IsNullOrEmpty(arquivoAcaoImediata.Anexo.ArquivoB64))
                                continue;

                            if (arquivoAcaoImediata.Anexo.ArquivoB64 == "undefined")
                                arquivoAcaoImediata.Anexo.ArquivoB64 = string.Empty;

                            Anexo anexoAtual = _AnexoAppServico.GetById(arquivoAcaoImediata.IdAnexo);
                            if (anexoAtual == null)
                            {
                                arquivoAcaoImediata.Anexo.Tratar();
                                arquivoAcaoImediata.IdAcaoImediata = acaoImediata.IdAcaoImediata;

                                _arquivoDeEvidenciaAcaoImediataRepositorio.Add(arquivoAcaoImediata);
                            }
                        }
                    }
                }
                //_registroAcaoImediataRepositorio.AtualizaAcaoImediataComAnexos(acaoImediata);
                /*
                if (alteracaoAnexos)
                    _registroAcaoImediataRepositorio.Update(acaoImediata);
                    */
            });
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

        public List<RegistroConformidade> ObtemListaRegistroConformidadePorSite(int idSite, string tipoRegistro, ref int numeroUltimoRegistro, int? idProcesso = null)
        {
            var listaNC = _registroConformidadesRepositorio.Get(
                                     x => x.IdSite == idSite &&
                                     (x.IdProcesso == idProcesso || idProcesso == null)

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
                return _registroConformidadesRepositorio.Get(x => x.IdRegistroPai == naoConformidade.IdRegistroConformidade).FirstOrDefault();

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
            //_registroConformidadesRepositorio.Add(gestaoDeRiscoCTX);

            return gestaoDeRisco;
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
            else if (gestaoDeRisco.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao)
            {
                return new CamposObrigatoriosNaoConformidadeReverificacao();
            }
            return null;
        }

        private AbstractValidator<RegistroConformidade> DefineFluxoValidacaoNC(RegistroConformidade naoConformidade)
        {
            var acaoImediataUpdateIsValid = naoConformidade.AcoesImediatas.FirstOrDefault(x => x.Estado == EstadoObjetoEF.Modified) != null;
            if (naoConformidade.EProcedente == false && naoConformidade.OStatusEEncerrada() && acaoImediataUpdateIsValid == false)
            {
                return new NCEProcedenteFalseViewValidation();
            }
            else if (naoConformidade.EProcedente == true && naoConformidade.OStatusEImplementacao() && acaoImediataUpdateIsValid == false)
            {
                return new NCEProcedenteTrueViewValidation();
            }
            else if (naoConformidade.EProcedente == true && naoConformidade.OStatusEImplementacao() && acaoImediataUpdateIsValid == true)
            {
                return new NCEProcedenteTrueViewValidation();
            }
            else if (naoConformidade.OStatusEReverificacao())
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
                IdRegistroPai = naoConformidade.IdRegistroConformidade
            };
        }

        public void VerificaAtualizaAcaoCorretiva(RegistroConformidade objCtx, RegistroConformidade naoConformidade)
        {
            var registroAcaoCorretiva = _registroConformidadesRepositorio.Get(x => x.IdRegistroPai == objCtx.IdRegistroConformidade).FirstOrDefault();

            if (registroAcaoCorretiva != null)
            {
                if (!naoConformidade.NecessitaAcaoCorretiva.Value || !naoConformidade.EProcedente.Value)
                {
                    registroAcaoCorretiva.DescricaoRegistro = naoConformidade.DescricaoAnaliseCausa;
                    registroAcaoCorretiva.StatusEtapa = (byte)EtapasRegistroConformidade.Encerrada; ;
                    registroAcaoCorretiva.DtEnceramento = DateTime.Now;
                    _registroConformidadesRepositorio.Update(registroAcaoCorretiva);
                }
                else
                {
                    registroAcaoCorretiva.DescricaoRegistro = naoConformidade.DescricaoAnaliseCausa;
                    registroAcaoCorretiva.StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata;
                    registroAcaoCorretiva.DtEfetivaImplementacao = naoConformidade.DtEfetivaImplementacao;
                    registroAcaoCorretiva.DtEnceramento = null;
                    _registroConformidadesRepositorio.Update(registroAcaoCorretiva);
                }
            }
        }


        private bool TemAcaoImediata(RegistroConformidade registroConformidade) => (registroConformidade.AcoesImediatas.Count > 0);

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

        public void CarregarArquivosNaoConformidadeAnexos2(RegistroConformidade naoConformidade, bool carregarArquivosDeEvidenciaAux)
        {
            naoConformidade.ArquivosNaoConformidadeAnexos = _arquivoNaoConformidadeAnexoRepositorio.Get(r => r.IdRegistroConformidade == naoConformidade.IdRegistroConformidade);

            if (carregarArquivosDeEvidenciaAux)
            {
                naoConformidade.ArquivosDeEvidenciaAux.AddRange(naoConformidade.ArquivosNaoConformidadeAnexos.Select(x => x.Anexo));
            }
        }
    }
}
