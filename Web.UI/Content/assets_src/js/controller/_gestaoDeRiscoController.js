﻿/*
|--------------------------------------------------------------------------
| Controlador Gestao de Risco
|--------------------------------------------------------------------------
*/

APP.controller.GestaoDeRiscoController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexGestaoDeRisco") {
            this.indexGestaoDeRisco();
        }
        if (page == "CriarGestaoDeRisco") {
            this.acoesGestaoDeRisco();

        }
    },

    setup: function () {
        //Index Gestao De Risco
        this.buttonDelGestaoDeRisco = $(".del-gestao-de-risco");

        //Criar Gestao De Risco
        this.buttonSalvar = $(".btn-salvar");

        //Editar Gestao De Risco
        this.buttonAddAcaoImediata = $('.add-acao-imediata');

        //Criar Gestao De Risco
        this.buttonDestravar = $(".btn-destravar");
    },

    //Models
    models: {
        AnexoModel: APP.model.Anexo,
    },

    //Index Gestao De Risco
    indexGestaoDeRisco: function () {

        APP.component.DataTable.setDataTableParam('#tb-index-gestao-de-risco', 0, "desc", "Tags");
        this.delGestaoDeRisco();

    },

    delGestaoDeRisco: function () {

        var tabelaGestaoRisco = $('#tb-index-gestao-de-risco').DataTable();

        this.buttonDelGestaoDeRisco.on('click', function (event) {
            event.preventDefault();

            var IdRegistroConformidade = $(this).data('idGestaoDeRisco');
            var $AtualRow = $(this).parents('tr');

            console.log("IdRegistroConformidade:= ", IdRegistroConformidade);

            bootbox.confirm("Deseja excluir o registro ? ", function (result) {
                if (result) {
                    $.get('/GestaoDeRisco/Excluir?idGestaoDeRisco=' + IdRegistroConformidade, function (data, status) {
                    }).done(function (data) {
                        if (data.StatusCode == "200") {
                            tabelaGestaoRisco.row($AtualRow).remove().draw();
                            bootbox.alert("Registro excluído com sucesso");
                        }
                    });
                }
            });
        });

        //    var msgIconeDeleteNC = $('[name=msgIconeDeleteGR]').val();

        //    bootbox.confirm(msgIconeDeleteNC, function (result) {
        //        if (result == true) {
        //            APP.controller.GestaoDeRiscoController.setDelGestaoDeRisco();
        //        }
        //    });

        //});

    },

    setDelGestaoDeRisco: function () {

        var idGestaoDeRisco = $('.del-gestao-de-risco').data("id-gestao-de-risco");
        var data = {
            "idGestaoDeRisco": idGestaoDeRisco,
            "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()
        };

        var erro = "";

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: "/GestaoDeRisco/RemoverComAcaoConformidade",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.reload([true]);
                } else if (result.StatusCode == 505) {
                    erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                } else if (result.StatusCode == 500) {
                    erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                }

            },
            error: function (result) {
                erro = APP.component.ResultErros.init(result.Erro);
                bootbox.alert(erro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

    //Acoes Gestao De Risco
    acoesGestaoDeRisco: function () {
        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        APP.component.BarRating.setBarRating('[name=formCriarNaoConformidadeCriticidade]', 'bars-1to10');

        if ($('[name=IdRegistroConformidade]').val() > 0) {
            APP.controller.GestaoDeRiscoController.getTemasCores();
        }


        this.setAndHide();
        this.setValidateForms();

        this.setDestravarCamposGestaoRisco();
        this.sendFormCriarNaoConformidade();

        if (habilitaLoad == 'sim') {

            this.HabilitaCamposGestaoRisco(perfil);
        }





    },

    setAndHide: function () {

        //Verifica Status Etapa
        var statusEtapa = parseInt($('[name=StatusEtapa]').val());
        switch (statusEtapa) {
            case 0:
                this.setShowAndHideStatusEtapa0();
                this.formCriarNaoConformidade();
                break;
            case 1:
                this.setShowAndHideStatusEtapa1();
                this.formAcaoImediata();
                break;
            case 2:
                this.setShowAndHideStatusEtapa2();
                this.formAcaoImediata();
                break;
            case 3:
                this.setShowAndHideStatusEtapa3();
                this.formAcaoImediata();
                break;
            case 4:
                this.setShowAndHideStatusEtapa4();
                break;
        }

    },

    setValidateForms: function () {

        var ObjFormCriarNaoConformidadeValidate = APP.controller.GestaoDeRiscoController.getObjObjFormCriarNaoConformidadeValidate();
        APP.component.ValidateForm.init(ObjFormCriarNaoConformidadeValidate, '#form-criar-gestaoderisco');
        APP.component.ValidateForm.init(ObjFormCriarNaoConformidadeValidate, '#form-acaoimediata');

    },

    //Interacao de Tela - StatusEtapa 0
    setShowAndHideStatusEtapa0: function () {

        $('#panel-acaoimediata').hide();
        $('[name=formCriarNaoConformidadeNmRegistro]').closest('div').hide();
        $('#lblformCriarNaoConformidadeNmRegistro').closest('div').hide();
        $('[name=formCriarNaoConformidadeResponsavel]').closest('div').hide();
        $('[name=formCriarNaoConformidadeCriticidade]').closest('div').hide();
        $('[name=formCausa]').closest('div').hide();



    },

    //Interacao de Tela - StatusEtapa 1
    setShowAndHideStatusEtapa1: function () {

        this.setDisabledStatusEtapa1(true);
        this.setHideStatusEtapa1();

    },

    setDisabledStatusEtapa1: function (_disabled) {
        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);
        $('[name=formCausa]').prop('disabled', _disabled);

    },

    setHideStatusEtapa1: function () {

        //Formulario Acao Imediata
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();
        APP.controller.GestaoDeRiscoController.checkSizeTableAcaoImediata();
        APP.controller.GestaoDeRiscoController.setHideRowAcaoImediata();

    },

    setHideRowAcaoImediata: function () {

        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').closest('div').hide();
        $('[name=formAcaoImadiataTbEvidencia]').closest('div').hide();

    },

    checkSizeTableAcaoImediata: function () {

        var sizeTable = $('#tb-acao-imediata tbody tr').size();

        if (sizeTable > 0) {
            $('[name=formAcaoImadiataResponsavelReverificacao]').closest('[class^=col]').show();
        } else {
            $('[name=formAcaoImadiataResponsavelReverificacao]').closest('[class^=col]').hide();
        }

    },

    //Interacao de Tela - StatusEtapa 2
    setShowAndHideStatusEtapa2: function () {

        this.setHideStatusEtapa2();
        this.setDisabledStatusEtapa2(true);
        this.setShowInputsEtapa2();

    },

    setHideStatusEtapa2: function () {

        //Botoes Acoes
        $('#tb-acao-imediata tbody tr').each(function () {
            $(this).find('td').last().hide();
        });
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();
        $('.add-acao-imediata').hide();

    },

    setDisabledStatusEtapa2: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeCriticidade]').prop('disabled', _disabled);

        //Formulario Acao Imediata
        $('[name=formAcaoImadiataDtDescricaoAcao]').prop('disabled', _disabled);
        //Table Acao Imediata
        $('[name=formAcaoImadiataTbDescricao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtPrazoImplementacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbResponsavelImplementar]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', _disabled);
        $('.upload-arq form-control').prop('disabled', _disabled);
        //Upload Changes
        $('[class^=btn-upload-form-acaoimediata-tb-evidencia]').closest('div').css('background-color', '#eee');
        $('[name=formAcaoImadiataTbEvidencia]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelReverificacao]').prop('disabled', _disabled);
        $('[name=formCausa]').prop('disabled', _disabled);

    },

    setShowInputsEtapa2: function () {

        var idUsuarioLogado = $('[name=UsuarioLogado]').val();
        var idPerfil = $('[name=IdPerfil]').val();
        $('#tb-acao-imediata tbody tr').each(function () {
            var idResponsavelImplementar = $(this).find('[name=formAcaoImadiataTbResponsavelImplementar]').val();
            if ((idResponsavelImplementar == idUsuarioLogado) || idPerfil != 4) {
                $(this).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', false);
                $(this).find('[name=formAcaoImadiataTbEvidencia]').prop('disabled', false);
                $(this).find('[class^=btn-upload-form-acaoimediata-tb-evidencia]').closest('div').css('background-color', '#fff');
            }
        });

    },

    //Interacao de Tela - StatusEtapa 3
    setShowAndHideStatusEtapa3: function () {

        this.setHideStatusEtapa3();
        this.setDisabledStatusEtapa3(true);
        this.setShowInputsEtapa3();
        this.setCheckAcaoImediataOk();
        this.setCheckAcaoImediataNotOk();
        this.setBackChecksOnPage();

    },

    setHideStatusEtapa3: function () {

        //Botoes Acoes
        $('#tb-acao-imediata tbody tr').each(function () {
            $(this).find('td').last().hide();
        });
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();
        $('.add-acao-imediata').hide();

    },

    setDisabledStatusEtapa3: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);

        //Formulario Acao Imediata
        $('[name=formAcaoImadiataDtDescricaoAcao]').prop('disabled', _disabled);
        //Table Acao Imediata
        $('[name=formAcaoImadiataTbDescricao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtPrazoImplementacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbResponsavelImplementar]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', _disabled);
        $('.upload-arq form-control').prop('disabled', _disabled);
        //Upload Changes
        $('[name^=formCriarNaoConformidadeEvidencia]').closest('div').css('background-color', '#eee');
        $('[name=formAcaoImadiataTbEvidencia]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelReverificacao]').prop('disabled', _disabled);
        $('[name=formCausa]').prop('disabled', _disabled);
    },

    setShowInputsEtapa3: function () {

        var idUsuarioLogado = $('[name=UsuarioLogado]').val();
        var idPerfil = $('[name=IdPerfil]').val();

        var idResponsavelReverificacao = $('[name=formAcaoImadiataResponsavelReverificacao]').val();
        if ((idResponsavelReverificacao == idUsuarioLogado) || idPerfil != 4) {
            $('#tb-acao-imediata tbody tr').each(function () {
                $(this).find('td').last().show();
                $(this).find('.btn-delete-acao-imediata').hide();
            });

            $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').show();

        }

    },

    setCheckAcaoImediataOk: function () {

        $('.btn-confirm-acao-imediata').on('click', function () {
            $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(true);
            APP.controller.GestaoDeRiscoController.getChecksAcaoImediata();
        });

    },

    setCheckAcaoImediataNotOk: function () {

        $('.btn-denied-acao-imediata').on('click', function () {
            $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(false);
            APP.controller.GestaoDeRiscoController.getChecksAcaoImediata();
        });

    },

    getChecksAcaoImediata: function () {
        var checkSize = 0;
        var checkAcaoImediata = $('#tb-acao-imediata tbody tr').find('[name=formAcaoImadiataTbAprovado]');
        for (i = 0; i < checkAcaoImediata.size(); i++) {
            var valueAprovador = $(checkAcaoImediata[i]).val();
            if (valueAprovador == "true") {
                checkSize++;
            }
        }
        if (checkSize == checkAcaoImediata.size()) {
            $('[name=formAcaoImadiataFoiEficaz').filter('[value=true]').prop('checked', true);
        } else {
            $('[name=formAcaoImadiataFoiEficaz').filter('[value=false]').prop('checked', true);
        }

    },

    setBackChecksOnPage: function () {
        $('.fa-check-circle, .fa-times-circle').unbind('click');
        $('.fa-check-circle, .fa-times-circle').on('click', function () {
            $(this).closest('td').find('.btn-ok-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-confirm-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val('');
            APP.controller.GestaoDeRiscoController.getChecksAcaoImediata();

        });
    },

    //Interacao de Tela - StatusEtapa 4
    setShowAndHideStatusEtapa4: function () {

        this.setHideStatusEtapa4();
        this.setTriggerRadioButtonsEtapa4('change');
        this.setDisabledStatusEtapa4(true);
        this.setShowInputsEtapa4();

    },

    setHideStatusEtapa4: function () {

        //Botoes Acoes
        $('#tb-acao-imediata tbody tr').each(function () {
            $(this).find('.btn-delete-acao-imediata').hide();
            $(this).find('.btn-confirm-acao-imediata').hide();
            $(this).find('.btn-denied-acao-imediata').hide();
        });
        $('.add-acao-imediata').hide();
        APP.controller.GestaoDeRiscoController.setNecessitaAcao();

    },

    setTriggerRadioButtonsEtapa4: function (_change) {

        $('[name=formAcaoImadiataEProcedente]').trigger(_change);
        $('[name=formAcaoImadiataECorrecao]').trigger(_change);
        $('[name=formAcaoImadiataNecessitaAC]').trigger(_change);

    },

    setDisabledStatusEtapa4: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);

        //Formulario Acao Imediata
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataDtDescricaoAcao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataJustificativa]').prop('disabled', _disabled);
        //Table Acao Imediata
        $('[name=formAcaoImadiataTbDescricao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtPrazoImplementacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbResponsavelImplementar]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', _disabled);
        $('.upload-arq form-control').prop('disabled', _disabled);
        //Upload Changes
        $('[name^=formCriarNaoConformidadeEvidencia]').closest('div').css('background-color', '#eee');
        $('[name=formAcaoImadiataTbEvidencia]').prop('disabled', _disabled);
        $('.btn-delete-acao-imediata').prop('disabled', _disabled);
        $('.btn-confirm-acao-imediata').prop('disabled', _disabled);
        $('.btn-denied-acao-imediata').prop('disabled', _disabled);
        $('.btn-ok-acao-imediata').prop('disabled', _disabled);
        $('.btn-notok-acao-imediata').prop('disabled', _disabled);
        $('[name=formAcaoImadiataECorrecao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataNecessitaAC]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelReverificacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataFoiEficaz]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelTratativa]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataAnaliseCausa]').prop('disabled', _disabled);
        $('[name=formCausa]').prop('disabled', _disabled);

    },

    setShowInputsEtapa4: function () {


        $('#tb-acao-imediata tbody tr').each(function () {
            var aprovado = $(this).find('[name=formAcaoImadiataTbAprovado]');
            if (aprovado == "true") {
                $(this).find('.btn-ok-acao-imediata').show();
            } else {
                $(this).find('.btn-notok-acao-imediata').show();
            }

        });

    },

    //Auxiliares
    getFoiEficaz: function () {

        var FoiEficaz = APP.component.Radio.init('formAcaoImadiataFoiEficaz');
        return FoiEficaz;

    },

    getNecessitaAcao: function () {

        var NecessitaAcao = APP.component.Radio.init('formAcaoImadiataEProcedente');
        return NecessitaAcao;

    },

    //Rules
    setRulesNecessitaAcao: function () {

        var necessitaAcao = APP.controller.GestaoDeRiscoController.getNecessitaAcao();
        if (necessitaAcao == "false") {
            $('[name=formCriarNaoConformidadeResponsavel]').closest('[class^=col]').hide();
            $('[name=formCriarNaoConformidadeNmRegistro]').closest('[class^=col]').hide();
            $('#lblformCriarNaoConformidadeNmRegistro').closest('div').hide();
            $('[name=formCriarNaoConformidadeCriticidade]').closest('div').hide();
            $('[name=formCausa]').closest('div').hide();
            $('#form-acaoimediata').hide();
        } else {
            $('[name=formCriarNaoConformidadeResponsavel]').closest('[class^=col]').show();
            $('[name=formCriarNaoConformidadeNmRegistro]').closest('[class^=col]').show();
            $('#lblformCriarNaoConformidadeNmRegistro').closest('div').show();
            $('[name=formCriarNaoConformidadeCriticidade]').closest('div').show();
            $('[name=formCausa]').closest('div').show();
            $('#form-acaoimediata').show();
        }

    },

    //Change
    setNecessitaAcao: function () {

        $('[name^=formAcaoImadiataEProcedente]').on('change', function () {

            APP.controller.GestaoDeRiscoController.setRulesNecessitaAcao();

        });

    },

    //Formulario Criar Nao Conformidade
    formCriarNaoConformidade: function () {

        this.setAndHideCriarNaoConformidade();
        this.getformCriarNaoConformidadeDtEmissao();
        this.getProcessosPorSite();
        this.getEmissorPorSite();
        this.setResponsavelAnaliseDefinicaoAC();
        this.setEProcedente();
        APP.controller.GestaoDeRiscoController.setNecessitaAcao();

    },

    setAndHideCriarNaoConformidade: function () {

        //

    },

    getformCriarNaoConformidadeDtEmissao: function () {

        $('[name=formCriarNaoConformidadeDtEmissao]').val(APP.component.Datatoday.init());

    },

    getProcessosPorSite: function () {

        var idSite = $('#nao-conformidade-site').val();
        $.get('/Processo/ListaProcessosPorSite?idSite=' + idSite, function (result) {
            $.each(result.Lista, (key, val) => {
                var $option = $('<option></option>');
                $('[name=formCriarNaoConformidadeProcesso]').append(
                    $option.val(val.IdProcesso).text(val.Nome)
                );
            });
        });

    },

    getEmissorPorSite: function () {

        var idSite = $('#nao-conformidade-site').val();
        var idFuncao = 89; // Funcionalidade(Cadastrar) que permite usuario criar nc
        $.get(`/Usuario/ObterUsuariosPorFuncao?idSite=${idSite}&idFuncao=${idFuncao}`, (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarNaoConformidadeEmissor] option'), $('[name=formCriarNaoConformidadeEmissor]'), 'IdUsuario', 'NmCompleto');
            }
        });

    },

    setResponsavelAnaliseDefinicaoAC: function () {

        $('[name=formCriarNaoConformidadeProcesso]').on('change', function () {
            var idSite = $('#nao-conformidade-site').val();
            processoSelecionado = $(this).find(':selected').val();
            var idFuncao = 90; // Funcionalidade(Define aÃ§Ã£o)
            $.get(`/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=${processoSelecionado}&idSite=${idSite}&idFuncao=${idFuncao}`, (result) => {
                if (result.StatusCode == 200) {
                    $('[name=formCriarNaoConformidadeResponsavel] option').not(':first-child').remove();
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name="formCriarNaoConformidadeResponsavel"] option'), $('[name="formCriarNaoConformidadeResponsavel"]'), 'IdUsuario', 'NmCompleto');
                }
            });

        });

    },

    setEProcedente: function () {

        $('[name=formAcaoImadiataEProcedente]').on('change', function () {

            if ($(this).val() == "true") {
                $('[name=formCriarNaoConformidadeResponsavel]').closest('div').show();
                $('[name=formCriarNaoConformidadeNmRegistro]').closest('div').show();
                $('#lblformCriarNaoConformidadeNmRegistro').closest('div').show();

            } else {
                $('[name=formCriarNaoConformidadeResponsavel]').closest('div').hide();
                $('[name=formCriarNaoConformidadeNmRegistro]').closest('div').hide();
                $('#lblformCriarNaoConformidadeNmRegistro').closest('div').hide();
            }
        });

    },

    getUltimaDataEmissao: function (_fluxo) {

        var dtEmissao = $('[name=formCriarNaoConformidadeDtEmissao]').val();
        var idSite = $('#nao-conformidade-site').val();

        $.ajax({
            url: "/NaoConformidade/ObtemUltimaDataEmissao/",
            type: "GET",
            data: {
                "site": idSite
            },
            async: false,
            success: function (result) {
                if (result.StatusCode == 200) {
                    var dataCompare = APP.component.Datatoday.getCompareDate(dtEmissao, result.UltimaDataEmissao);
                    var labelDesejaCriarNcDtEmissaoAnteriorAUltima = $('[name=labelDesejaCriarNcDtEmissaoAnteriorAUltima]').val();
                    if (!true) {
                        bootbox.confirm(labelDesejaCriarNcDtEmissaoAnteriorAUltima, function (result) {
                            if (result) {
                                var naoConformidadeObj = APP.controller.GestaoDeRiscoController.getCriarNaoConformidadeObj(_fluxo);
                                APP.controller.GestaoDeRiscoController.saveFormCriarNaoConformidade(naoConformidadeObj, _fluxo);
                            }
                        });
                    } else {
                        var naoConformidadeObj = APP.controller.GestaoDeRiscoController.getCriarNaoConformidadeObj(_fluxo);
                        APP.controller.GestaoDeRiscoController.saveFormCriarNaoConformidade(naoConformidadeObj, _fluxo);
                    }
                }
            }
        });

    },

    getObjObjFormCriarNaoConformidadeValidate: function () {

        var acoesNaoConformidadeoFormCriarNaoConformidadeObj = {
            //formCriarNaoConformidadeNmRegistro: 'required',
            formCriarNaoConformidadeDsRegistro: {
                'required': true,
                'minlength': 4
            },
            formCriarNaoConformidadeDtEmissao: { 'required': true },
            formCriarNaoConformidadeProcesso: { 'required': true },
            formCriarNaoConformidadeEmissor: 'required',
            //formCriarNaoConformidadeResponsavel: 'required',
            //formCriarNaoConformidadeTags: 'required',

            //Form Acao Imediata
            // formAcaoImadiataEProcedente: {'required': true},
            // formAcaoImadiataDtDescricaoAcao: {'required': true},
            // formAcaoImadiataJustificativa: {'required': true},
            // formAcaoImadiataTbDescricao: {'required': true},
            // formAcaoImadiataTbDtPrazoImplementacao: {'required': true},
            // formAcaoImadiataTbResponsavelImplementar: {'required': true},
            // formAcaoImadiataTbDtEfetivaImplementacao: {'required': true},
            // formAcaoImadiataTbEvidencia: {'required': true},
            // formAcaoImadiataECorrecao: {'required': true},
            // formAcaoImadiataNecessitaAC: {'required': true},
            // formAcaoImadiataResponsavelReverificacao: {'required': true},
            // formAcaoImadiataFoiEficaz: {'required': true},
            // formAcaoImadiataResponsavelTratativa: {'required': true},
            // formAcaoImadiataNumeroAC: {'required': true},
            // formAcaoImadiataAnaliseCausa: {'required': true},
        };

        return acoesNaoConformidadeoFormCriarNaoConformidadeObj;

    },

    getObjFormCriarNaoConformidade: function (_fluxo) {

        var acoesNaoConformidadeFormCriarNaoConformidadeObj = {};

        switch (_fluxo) {
            case "fluxo-00":
                //Obj enviado no fluxo de criacao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    IdSite: $('[name=IdSite]').val(),
                    StatusEtapa: parseInt($('[name=StatusEtapa]').val()),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoDeRiscoController.getAnexosEvidencias(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    CriticidadeGestaoDeRisco: $('[name=formCriarNaoConformidadeCriticidade] :selected').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    Causa: $('[name=formCausa]').val(),
                };

                break;

            case "fluxo-02":
                //Obj enviado no fluxo 02 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: 2,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    AcoesImediatas: APP.controller.GestaoDeRiscoController.getObjFormAcaoImediata(),
                    ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelPorIniciarTratativaAcaoCorretiva: $('[name=formAcaoImadiataResponsavelTratativa]').val(),
                    DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    CriticidadeGestaoDeRisco: $('[name=formCriarNaoConformidadeCriticidade] :selected').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoDeRiscoController.getAnexosEvidencias(),
                    Causa: $('[name=formCausa]').val(),
                };

                break;
            case "fluxo-03":
                //Obj enviado no fluxo 03 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: 2,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    AcoesImediatas: APP.controller.GestaoDeRiscoController.getObjFormAcaoImediata(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    CriticidadeGestaoDeRisco: $('[name=formCriarNaoConformidadeCriticidade] :selected').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoDeRiscoController.getAnexosEvidencias(),
                    Causa: $('[name=formCausa]').val(),
                };
                break;
            case "fluxo-04":
                //Obj enviado no fluxo 04 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: $('[name=StatusEtapa]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    AcoesImediatas: APP.controller.GestaoDeRiscoController.getObjFormAcaoImediata(),
                    FlEficaz: APP.controller.GestaoDeRiscoController.getFoiEficaz(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    CriticidadeGestaoDeRisco: $('[name=formCriarNaoConformidadeCriticidade] :selected').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoDeRiscoController.getAnexosEvidencias(),
                    Causa: $('[name=formCausa]').val(),
                };
                break;
        }

        return acoesNaoConformidadeFormCriarNaoConformidadeObj;

    },

    getAnexosEvidencias: function () {

        var anexoEvidenciaModel = APP.controller.ClienteController.models.AnexoModel;
        var arrayAnexoEvidencia = [];

        $('.dashed li a:first-child').each(function () {

            var nameImg = $(this).text();
            var anexoEvidenciaNC = anexoEvidenciaModel.constructor(
                $(this).closest('li').find('[name=formCriarNaoConformidadeEvidenciaIdAnexo]').val(),
                nameImg,
                $(this).data('b64')
            );

            arrayAnexoEvidencia.push(anexoEvidenciaNC);
        });

        return arrayAnexoEvidencia;

    },

    //Formulario Acao Imediata
    formAcaoImediata: function () {

        this.setAndHideAcaoImediata();
        this.getformCriarNaoConformidadeDtDescricaoAcao();
        this.setAcaoImediata();
        this.delAcaoImediata();
        this.getResponsavelReverificarAcaoImediata();

    },

    setAndHideAcaoImediata: function () {

        var necessitaAcao = APP.controller.GestaoDeRiscoController.getNecessitaAcao();
        if (necessitaAcao == "false") {
            $('[name=formCriarNaoConformidadeResponsavel]').closest('[class^=col]').hide();
            $('[name=formCriarNaoConformidadeNmRegistro]').closest('[class^=col]').hide();
            $('#form-acaoimediata').hide();
        }

    },

    getformCriarNaoConformidadeDtDescricaoAcao: function () {

        $('[name=formAcaoImadiataDtDescricaoAcao]').val(APP.component.Datatoday.init());

    },

    setAcaoImediata: function () {

        this.buttonAddAcaoImediata.unbind('click');
        this.buttonAddAcaoImediata.bind('click', function () {

            var TraducaoDropNameSelect = 'Selecione';
            var index = $('#tb-acao-imediata tbody tr').size();

            var html = '';
            html += '<tr role="row" class="odd">';
            html += '<td>';
            html += '<textarea type="text" id="form-acaoimediata-tb-descricao" name="formAcaoImadiataTbDescricao" class="form-control" ';
            html += 'placeholder="DescriÃ§Ã£o" ';
            html += 'data-msg-required="" ';
            html += 'value=""';
            html += 'rows="3"></textarea>';
            html += '<input type="hidden" name="formAcaoImadiataTbIdAcaoImediata" class="form-control input-data" value="0"/>';
            html += '<input type="hidden" name="formAcaoImadiataTbEstado" value="4"/>';
            html += '</td>';
            html += '<td>';
            html += '<div class="input-group">';
            html += '<input type="text" name="formAcaoImadiataTbDtPrazoImplementacao" id="form-acaoimediata-tb-dt-prazo-implementacao" class="form-control data datepicker" ';
            html += 'data-msg-required="" ';
            html += 'value="" />';
            html += '<span class="input-group-addon">';
            html += '<i class="fa fa-calendar" aria-hidden="true"></i>';
            html += '</span>';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<select id="form-acaoimediata-tb-responsavel-implementar" name="formAcaoImadiataTbResponsavelImplementar" class="form-control" ';
            html += 'data-msg-required="">';
            html += '<option value="">' + TraducaoDropNameSelect + '</option>';
            html += '</select>';
            html += '</td>';
            html += '<td>';
            html += '<div class="input-group">';
            html += '<input type="text" name="formAcaoImadiataTbDtEfetivaImplementacao" id="form-acaoimediata-tb-dt-efetiva-implementacao" class="form-control data datepicker" ';
            html += 'data-msg-required="" ';
            html += 'value="" />';
            html += '<span class="input-group-addon">';
            html += '<i class="fa fa-calendar" aria-hidden="true"></i>';
            html += '</span>';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<div class="upload-arq form-control">';
            html += '<a class="btn-upload-form-acaoimediata-tb-evidencia-' + index + '">';
            html += '<i class="fa fa-paperclip fa-1x" aria-hidden="true"></i>Anexar';
            html += '</a>';
            html += '<input type="file" name="formAcaoImadiataTbEvidencia" id="form-acaoimediata-tb-evidencia-' + index + '" class="" data-msg-required="" data-b64="">';
            html += '</div>';
            html += '<ul></ul>';
            html += '</td>';
            html += '<td>';
            html += '<a href="#" class="btn-delete-acao-imediata icon-cliente trash-color">';
            html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" data-original-title="@Traducao.Resource.btn_lbl_excluir"></i>';
            html += '</a>';
            html += '</td>';
            html += '</tr>';

            $('#tb-acao-imediata tbody').append(html);
            $('.add-acao-imediata').removeClass('show').addClass('hide');
            APP.controller.GestaoDeRiscoController.bind();

        });

    },

    delAcaoImediata: function () {

        $('.btn-delete-acao-imediata').unbind('click');
        $('.btn-delete-acao-imediata').on('click', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
        });

    },

    getResponsavelImplementarAcaoImediata: function () {
        var idSite = $('#nao-conformidade-site').val();
        var idFuncao = 91; // Funcionalidade(Implementar aÃ§Ã£o) que permite usuario Implementar aÃ§Ã£o NC
        var idProcesso = $('[name=IdProcesso]').val();
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: `/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=${idProcesso}&idSite=${idSite}&idFuncao=${idFuncao}`,
            beforeSend: function () {
                $('.add-acao-imediata').removeClass('show').addClass('hide');
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    APP.component.SelectListCompare.selectList(result.Lista, $('#tb-acao-imediata tbody tr:last-child [name="formAcaoImadiataTbResponsavelImplementar"] option'), $('#tb-acao-imediata tbody tr:last-child [name="formAcaoImadiataTbResponsavelImplementar"]'), 'IdUsuario', 'NmCompleto');
                }
            },
            error: function (result) {
                bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {
                $('.add-acao-imediata').removeClass('hide').addClass('show');
            }
        });

    },

    getResponsavelReverificarAcaoImediata: function () {
        var idSite = $('#nao-conformidade-site').val();
        var idProcesso = $('[name=IdProcesso]').val();
        var idFuncao = 92; // Funcionalidade(ReverificaÃ§Ã£o) que permite usuario Reverifique NC
        $.get(`/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=${idProcesso}&idSite=${idSite}&idFuncao=${idFuncao}`, function (result) {
            if (result.StatusCode == 200) {

                $(result.Lista).each(function (key, value) {
                    var $option = $('<option>');
                    $('[name=formAcaoImadiataResponsavelReverificacao]').append(
                        $option.val(value.IdUsuario).text(value.NmCompleto)
                    );
                });
            }
        });

    },

    bind: function () {

        APP.component.Datapicker.init();
        APP.controller.GestaoDeRiscoController.setup();
        APP.controller.GestaoDeRiscoController.setAcaoImediata();
        APP.controller.GestaoDeRiscoController.getResponsavelImplementarAcaoImediata();
        APP.component.FileUpload.init();
        APP.controller.GestaoDeRiscoController.delAcaoImediata();
        APP.controller.GestaoDeRiscoController.setHideStatusEtapa1();

    },

    getObjObjFormAcaoImediataValidate: function () {

        var acoesNaoConformidadeoFormAcaoImediataObj = {

            formCriarNaoConformidadeTags: 'required',
        };

        return acoesNaoConformidadeoFormAcaoImediataObj;

    },

    getObjFormAcaoImediata: function () {

        var idUsuarioLogado = $('[name=UsuarioLogado]').val();
        var statusEtapa = parseInt($('[name=StatusEtapa]').val());

        var getObjFormAcaoImediataArray = [];
        var acoesNaoConformidadeFormAcaoImediataObj = {};

        var anexoEvidenciaModel = APP.controller.ClienteController.models.AnexoModel;

        $('#tb-acao-imediata tbody tr').each(function (index, tr) {
            if (statusEtapa == 2) {
                acoesNaoConformidadeFormAcaoImediataObj = {
                    Descricao: $(tr).find('[name=formAcaoImadiataTbDescricao]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $(tr).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    ArquivoEvidenciaAux: anexoEvidenciaModel.constructor(
                        $(tr).find('[name^=formCriarNaoConformidadeEvidencia]').closest('td').find('[name^=formCriarNaoConformidadeEvidenciaIdAnexo]').val(),
                        $(tr).find('[name^=formCriarNaoConformidadeEvidencia]').closest('td').find('i').text(),
                        $(tr).find('[id^=form-acaoimediata-tb-evidencia-]').data('b64')
                    ),
                    Estado: $(tr).find('[name=formAcaoImadiataTbEstado]').val() != 0 ? 16 : 0,
                    IdAcaoImediata: $(tr).find('[name=formAcaoImadiataTbIdAcaoImediata]').val(),
                };
            } else if (statusEtapa == 3) {
                acoesNaoConformidadeFormAcaoImediataObj = {
                    Aprovado: $(tr).find('[name=formAcaoImadiataTbAprovado]').val(),
                    IdAcaoImediata: $(tr).find('[name=formAcaoImadiataTbIdAcaoImediata]').val(),
                    Estado: $(tr).find('[name=formAcaoImadiataTbEstado]').val() != 0 ? 16 : 0,
                };
            } else {
                acoesNaoConformidadeFormAcaoImediataObj = {
                    Descricao: $(tr).find('[name=formAcaoImadiataTbDescricao]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $(tr).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    Estado: $(tr).find('[name=formAcaoImadiataTbEstado]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                };
            }

            getObjFormAcaoImediataArray.push(acoesNaoConformidadeFormAcaoImediataObj);
        });

        return getObjFormAcaoImediataArray;

    },

    //Edit
    getTemasCores: function () {

        var divBarRating = $('.barraRating');
        var corRisco = $('[name=formGestaoDeRiscoCriticidadeCor]').val();
        var lastCores = $("[data-rating-value='" + corRisco + "']").last().index();
        for (i = 0; i <= lastCores; i++) {
            $($('.br-theme-bars-1to10').find('.br-widget a')[i]).addClass('br-selected');
        }
        $($('.br-theme-bars-1to10').find('.br-widget a')[lastCores]).trigger("click");
        $('.br-theme-bars-1to10').find("[data-rating-value='" + corRisco + "']").addClass('br-current');
        $('.br-widget').addClass('barRating-disabled');

    },

    //Todos
    sendFormCriarNaoConformidade: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.GestaoDeRiscoController.validateForms();

            //validate = true;
            if (validate == true) {

                var statusEtapa = parseInt($('[name=StatusEtapa]').val());
                var IdRegistroConformidade = $('[name=IdRegistroConformidade]').val();
                var eProcedente = $('[name=formAcaoImadiataEProcedente]:checked').val();

                var naoConformidade = {};

                var fluxos = {
                    //Criar Gestao de Risco
                    _000: ['fluxo-00'],
                    //Status 1 - AÃ§Ã£o Corretiva
                    _002: ['fluxo-02'],
                    //Status 2 - ImplementaÃ§Ã£o
                    _003: ['fluxo-03'],
                    //Status 3 - ReverificaÃ§Ã£o
                    _004: ['fluxo-04']
                };

                switch (statusEtapa) {
                    case 0:
                        APP.controller.GestaoDeRiscoController.getUltimaDataEmissao('fluxo-00');
                        break;
                    case 1:
                        naoConformidade = APP.controller.GestaoDeRiscoController.getCriarNaoConformidadeObj("fluxo-02");
                        APP.controller.GestaoDeRiscoController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-02");
                        break;
                    case 2:
                        naoConformidade = APP.controller.GestaoDeRiscoController.getCriarNaoConformidadeObj("fluxo-03");
                        APP.controller.GestaoDeRiscoController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-03");
                        break;
                    case 3:
                        naoConformidade = APP.controller.GestaoDeRiscoController.getCriarNaoConformidadeObj("fluxo-04");
                        APP.controller.GestaoDeRiscoController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-04");
                        break;
                    case 4:

                        break;
                }
            }

        });

    },

    validateForms: function () {

        var valid = true;
        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            if (isVisible) {
                var validate = $(this).closest('form').valid();
                if (validate != true) {
                    valid = false;
                }

            }
        });

        return valid;

    },

    getCriarNaoConformidadeObj: function (_fluxo) {

        var naoConformidadeObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "gestaoderisco":
                        naoConformidadeObj = APP.controller.GestaoDeRiscoController.getObjFormCriarNaoConformidade(_fluxo);
                        break;
                    //case "acaoimediata":
                    //acaoimediataObj = APP.controller.GestaoDeRiscoController.getObjFormAcaoImediata();
                    //break;
                }
            }

        });

        return naoConformidadeObj;

    },

    saveFormCriarNaoConformidade: function (naoConformidadeObj, _fluxo) {
        var url = "";

        switch (_fluxo) {
            case "fluxo-00":
                url = "/GestaoDeRisco/SalvarPrimeiraEtapa";
                break;
            case "fluxo-02":
                url = "/GestaoDeRisco/SalvarSegundaEtapa";
                break;
            case "fluxo-03":
                url = "/GestaoDeRisco/SalvarSegundaEtapa";
                break;
            case "fluxo-04":
                url = "/GestaoDeRisco/SalvarSegundaEtapa";
                break;
        }

        $.ajax({
            type: "POST",
            data: naoConformidadeObj,
            dataType: 'json',
            url: url,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/GestaoDeRisco/Index";
                    });
                } else if (result.StatusCode == 505) {
                    erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                } else if (result.StatusCode == 500) {
                    erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                }
            },
            error: function (result) {
                erro = APP.component.ResultErros.init(result.Erro);
                bootbox.alert(erro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },


    HabilitaCamposGestaoRisco: function (perfil) {

        if (perfil == '4') {

            $('#main').find('input, textarea, button, select').removeAttr('disabled');
            $("#form-criar-nao-conformidade-processo").attr("disabled", true);
            $("#form-criar-nao-conformidade-emissor").attr("disabled", true);
            $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
            $('.br-widget').removeClass('barRating-disabled');


        }
        else {

            $('#main').find('input, textarea, button, select').removeAttr('disabled');
            $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
            $('.br-widget').removeClass('barRating-disabled');



        }
    },


    DestravaDocumento: function () {

        var idGestaoDeRisco = $('[name=IdRegistroConformidade]').val();  // $('#form-criar-nao-conformidade-nm-registro').val();
        var data = { "idGestaoDeRisco": idGestaoDeRisco };

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: "/GestaoDeRisco/DestravarDocumento",
            success: function (result) {
                //alert('foi');
                //if (result.StatusCode == 200) {
                //    window.location.reload([true]);
                //} else if (result.StatusCode == 505) {
                //    erro = APP.component.ResultErros.init(result.Erro);
                //    bootbox.alert(erro);
                //} else if (result.StatusCode == 500) {
                //    erro = APP.component.ResultErros.init(result.Erro);
                //    bootbox.alert(erro);
                //}

            },
            error: function (result) {
                //erro = APP.component.ResultErros.init(result.Erro);
                //bootbox.alert(erro);
            },
            //complete: function (result) {
            //    APP.component.Loading.hideLoading();
        });
    },


    setDestravarCamposGestaoRisco: function () {

        this.buttonDestravar.on('click', function () {


            if (perfil == '4') {

                $('#main').find('input, textarea, button, select').removeAttr('disabled');
                $("#form-criar-nao-conformidade-processo").attr("disabled", true);
                $("#form-criar-nao-conformidade-emissor").attr("disabled", true);
                $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
                $('.br-widget').removeClass('barRating-disabled');



            }
            else {

                $('#main').find('input, textarea, button, select').removeAttr('disabled');
                $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
                $('.br-widget').removeClass('barRating-disabled');

            }

            var idGestaoDeRisco = $("[name=IdRegistroConformidade]").val();
            var data = { "idGestaoDeRisco": idGestaoDeRisco };

            $.ajax({
                type: "POST",
                dataType: 'json',
                data: data,
                url: "/GestaoDeRisco/DestravarDocumento",
                success: function (result) {
                    //alert('foi');
                    //if (result.StatusCode == 200) {
                    //    window.location.reload([true]);
                    //} else if (result.StatusCode == 505) {
                    //    erro = APP.component.ResultErros.init(result.Erro);
                    //    bootbox.alert(erro);
                    //} else if (result.StatusCode == 500) {
                    //    erro = APP.component.ResultErros.init(result.Erro);
                    //    bootbox.alert(erro);
                    //}

                },
                error: function (result) {
                    //erro = APP.component.ResultErros.init(result.Erro);
                    //bootbox.alert(erro);
                },
                //complete: function (result) {
                //    APP.component.Loading.hideLoading();
            });




            //this.HabilitaCamposGestaoRisco(perfil);

            //this.DestravaDocumento();

        });
    },

}