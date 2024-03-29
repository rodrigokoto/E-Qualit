﻿

/*
|--------------------------------------------------------------------------
| Controlador Nao Conformidade
|--------------------------------------------------------------------------
*/

soNumeros = function (valor) {

    return valor.replace(/[^\d.-]/g, '');
};

removeNoNumerics = (function (el) {
    el.value = soNumeros(el.value);
});

dateFormat = (function (el) {
    value = el.value;

    var month = value.substring(0, 2);
    if (month < 1 || month > 12) {
        el.value = '';
        return;
    }

    var year = value.substring(2, 6);
    if (year <= 1900 || year >= 2500) {
        el.value = '';
        return;
    }

    el.value = month + '/' + year;
});

abrirModalGrafico = (function () {
    $('#dtDe').datepicker({
        dateFormat: "mm/yy",
        startView: "months",
        minViewMode: "months"
    })
        .datepicker("option", "changeMonth", true)
        .datepicker("option", "changeYear", true)
        .datepicker("option", "showButtonPanel", true);


    $('#dtAte').datepicker({
        dateFormat: "mm/yy",
        startView: "months",
        minViewMode: "months"
    })
        .datepicker("option", "changeMonth", true)
        .datepicker("option", "changeYear", true)
        .datepicker("option", "showButtonPanel", true);


    $("#modalGrafico").modal();
});

abrirGrafico = (function (url) {

    var idTipoGrafico = $("#ddlTipoGrafico").val();
    var idTipoNaoConformidade = $("#ddlTipoNaoConformidade").val();
    var dtDe = soNumeros($("#dtDe").val());
    var dtAte = soNumeros($("#dtAte").val());
    var estiloGrafico = $('input[name=estiloGrafico]:checked').val();
    var msg = '';

    if (idTipoGrafico.length == 0)
        msg += '- Tipo de Gráfico<br>';

    if (dtDe.length == 0)
        msg += '- De<br>';

    if (dtAte.length == 0)
        msg += '- Até<br>';

    if (msg.length > 0) {
        bootbox.alert('Os seguintes campos são obrigatórios:<br><br>' + msg);
    }
    else {

        var parametros = '?tipoGrafico=' + idTipoGrafico;
        parametros += '&dtDe=' + dtDe;
        parametros += '&dtAte=' + dtAte;
        parametros += '&estiloGrafico=' + estiloGrafico;
        parametros += '&tipoNaoConformidade=' + idTipoNaoConformidade;

        window.open(url + parametros, '_blank');
    }
});

$("#ddlTipoGrafico").change(function myFunction() {
    var idTipoGrafico = $("#ddlTipoGrafico").val();

    if (idTipoGrafico == 2 || idTipoGrafico == 4) {
        $('#graficoPizza').hide().next().hide();
        $('#graficoPizza').prop('checked', false);
        $('#graficoBarra').prop('checked', true);
    }
    else {
        $('#graficoPizza').show().next().show();
    }

});


APP.controller.NaoConformidadeController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexNaoConformidade") {
            this.indexNaoConformidade();
        }
        if (page == "CriarNaoConformidade") {
            this.acoesNaoConformidade();
        }
        //typeof (destavar) != "undefined" && 
        if (destravar === 'True') {


            this.setDestravarCamposNaoConformidade();
            this.HabilitaCamposNaoConformidade(perfil);
            this.formEditarNaoConformidade();
            this.formAcaoImediata();
            this.setCheckDtRevisao();
            //this.setAcaoImediata();
            $('.botaouploadarquivos').removeAttr('disabled');

            var idSite = $('#nao-conformidade-site').val();
            processoSelecionado = $('[name=formCriarNaoConformidadeProcesso]').find(':selected').val();
            var idFuncao = 14; // Funcionalidade(Define aÃ§Ã£o)
            $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + processoSelecionado + '&idSite=' + idSite + '&idFuncao=' + idFuncao, (result) => {
                if (result.StatusCode == 200) {
                    //$('[name=formCriarNaoConformidadeResponsavel] option').not(':first-child').remove();
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name="formCriarNaoConformidadeResponsavel"] option'), $('[name="formCriarNaoConformidadeResponsavel"]'), 'IdUsuario', 'NmCompleto');
                }
            });
            //this.setResponsavelAnaliseDefinicaoAC();
            //var valor = $('[name=formCriarNaoConformidadeResponsavel]').val();
            //$('[name=formCriarNaoConformidadeProcesso]').trigger('change');
            //$('[name=formCriarNaoConformidadeResponsavel]').val(valor);

            //this.setResponsavelAnaliseDefinicaoAC();
            APP.controller.NaoConformidadeController.getResponsavelImplementarAcaoImediata();

        }

    },

    setup: function () {
        //Index Nao Conformidade
        this.buttonDelNaoConformidade = $(".del-nao-conformidade");

        //Criar Nao Conformidade
        this.buttonSalvar = $(".btn-salvar");
        this.buttonAnular = $(".btn-anular");
        //Editar Nao Conformidade
        this.buttonAddAcaoImediata = $('.add-acao-imediata');

        this.buttonDestravar = $(".btn-destravar");

        this.buttonImprimir = $("#btn-imprimir");
        this.setCheckDtRevisao();

    },

    //Models
    models: {
        AnexoModel: APP.model.Anexo,
    },

    //Index Nao Conformidade
    indexNaoConformidade: function () {

        APP.component.DataTable.setDataTableParam('#tb-index-nao-conformidade', 0, "desc", "Tags");
        this.delNaoConformidade();

    },

    eventoAuditoria: function () {

        $('[name=formCriarNaoConformidadeEAuditoria]').on('change', function () {

            var EhAuditoria = $(this).val();

            if (EhAuditoria == "true") {
                var idCategoria = $('[name=formCriarNaoConformidadeTipo] option:contains("Auditoria")').val();

                $('[name=formCriarNaoConformidadeTipo]').val(idCategoria);
            }

        });

    },

    delNaoConformidade: function () {
        this.buttonDelNaoConformidade.on('click', function () {

            var msgIconeDeleteNC = $('[name=msgIconeDeleteNC]').val();

            bootbox.confirm(msgIconeDeleteNC, function (result) {
                if (result == true) {
                    APP.controller.NaoConformidadeController.setDelNaoConformidade();
                }
            });

        });

    },

    setDelNaoConformidade: function () {

        var idNaoConformidade = $('.del-nao-conformidade').data("id-nao-conformidade");
        var data = {
            "idNaoConformidade": idNaoConformidade,
            "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()
        };

        var erro = "";

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: "/NaoConformidade/RemoverComAcaoConformidade",
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

    //Acoes Nao Conformidade
    acoesNaoConformidade: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        this.setEProcedente();
        this.setECorrecao();
        this.setNecessitaAC();
        this.setAndHide();
        this.setValidateForms();
        this.eventoAuditoria();
        this.eventoImprimir();
        this.eventoAnular();

        this.sendFormCriarNaoConformidade();

        this.setDestravarCamposNaoConformidade();

        //if (habilitaLoad == 'sim') {

        //    this.HabilitaCamposNaoConformidade(perfil);
        //}
    },

    eventoAnular: function () {
        this.buttonAnular.on('click', function () {

            var dialog = bootbox.dialog({
                title: 'Anular Não Conformidade',
                message: "<p>Gostaria de anular essa Não Conformidade?.</p>",
                size: 'small',
                buttons: {
                    cancel: {
                        label: "Não",
                        className: 'btn-danger',
                        callback: function () {
                            $("#painel-acao-corretiva-nao").hide();
                            $('[name=StatusEtapa]').val('1');
                            $('[name=formCriarNaoConformidadeDsJustificativa]').hide();

                            $('.add-acao-imediata').show();
                            $('.pnl-anular').show();
                        }
                    },

                    ok: {
                        label: "Sim",
                        className: 'btn-info',
                        callback: function () {
                            $("#painel-acao-corretiva-nao").show();
                            $('[name=StatusEtapa]').val('5');
                            $('[name=formCriarNaoConformidadeDsJustificativa]').show();
                            $('[name=formCriarNaoConformidadeDsJustificativa]').prop('disabled', false);

                            $('.pnl-anular').hide();
                        }
                    }
                }
            });
        });
    },

    eventoImprimir: function () {

        this.buttonImprimir.on('click', function () {

            var IdNaoConformidade = $(".IdNaoConformidade").val();
            APP.controller.NaoConformidadeController.imprimir(IdNaoConformidade);

        });

    },

    imprimir: function (IdNaoConformidade) {


        if (IdNaoConformidade != null) {

            APP.component.Loading.showLoading();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/NaoConformidade/PDF?id=' + IdNaoConformidade, true);
            xhr.responseType = 'arraybuffer';
            xhr.onload = function (e) {
                if (this.status == 200) {
                    var blob = new Blob([this.response], { type: "application/pdf" });
                    var pdfUrl = URL.createObjectURL(blob);
                    printJS(pdfUrl);
                }

                APP.component.Loading.hideLoading();

            };

            xhr.send();

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
                this.formEditarNaoConformidade();
                this.formAcaoImediata();
                this.setEProcedenteInicializacao();
                break;
            case 2:
                this.setShowAndHideStatusEtapa2();
                this.formEditarNaoConformidade();
                this.formAcaoImediata();
                break;
            case 3:
                this.setShowAndHideStatusEtapa3();
                this.formEditarNaoConformidade();
                this.formAcaoImediata();
                break;
            case 4:
                this.setShowAndHideStatusEtapa4();
                $('.btn-anular').prop('disabled', true);
                break;
            case 5:
                this.setShowAndHideStatusEtapa5();
                $('.btn-anular').prop('disabled', true);
                break;
        }

    },

    setValidateForms: function () {

        var ObjFormCriarNaoConformidadeValidate = APP.controller.NaoConformidadeController.getObjObjFormCriarNaoConformidadeValidate();
        APP.component.ValidateForm.init(ObjFormCriarNaoConformidadeValidate, '#form-criar-naoconformidade');
        var ObjFormAcaoImediataValidate = APP.controller.NaoConformidadeController.getObjObjFormAcaoImediataValidate();
        APP.component.ValidateForm.init(ObjFormAcaoImediataValidate, '#form-acaoimediata');

    },

    //Interacao de Tela - StatusEtapa 0
    setShowAndHideStatusEtapa0: function () {

        $('#panel-acaoimediata').hide();
        //$('[name=formCriarNaoConformidadeNmRegistro]').closest('div').hide();

    },

    //Interacao de Tela - StatusEtapa 1
    setShowAndHideStatusEtapa1: function () {

        this.setDisabledStatusEtapa1(true);
        this.setHideStatusEtapa1();
        var EAuditoria = this.getEAuditoria();
        this.setRulesEAuditoria(EAuditoria);

    },

    setDisabledStatusEtapa1: function (_disabled) {
        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDsJustificativa]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);
        $('.botaouploadarquivos').prop('disabled', _disabled);

    },

    setHideStatusEtapa1: function () {

        //Formulario Acao Imediata
        this.setHidePanelEProcedenteNao();
        this.setHidePanelEProcedenteSim();

    },

    setHidePanelEProcedenteNao: function () {

        $('[name=formAcaoImadiataJustificativa]').closest('[class^=col]').hide();

    },

    setHidePrazoReverificacao: function () {
        var checkSize = 0;
        debugger;
        var checkAcaoImediata = $('#tb-acao-imediata tbody tr').find('[name=formAcaoImadiataTbDtEfetivaImplementacao]');
        for (i = 0; i < checkAcaoImediata.size(); i++) {
            var valueAprovador = $(checkAcaoImediata[i]).val();
            if (valueAprovador == "") {
                checkSize++;
            }
        }

        if (checkAcaoImediata.size() > 0) {

            if (checkSize !== 0) {
                $('.dtReverificacao').removeClass('show').addClass('hide');
            }
            else {
                $('.dtReverificacao').removeClass('hide').addClass('show');
                let _disabled = $('[name=formAcaoImadiataDtPrazoReverificacao]').val() !== "" ? true : false;
                $('[name=formAcaoImadiataDtPrazoReverificacao]').prop('disabled', _disabled);
            }
        }
        else
            $('.dtReverificacao').removeClass('show').addClass('hide');
    },


    setHidePanelEProcedenteSim: function () {

        $('#tb-acao-imediata').closest('div').hide();
        $('[name=formAcaoImadiataECorrecao]').closest('[class^=col]').hide();
        $('[name=formAcaoImadiataNecessitaAC]').closest('[class^=col]').hide();
        $('[name=formAcaoImadiataResponsavelReverificacao]').closest('[class^=col]').hide();
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();
        $('[name=formAcaoImadiataResponsavelTratativa]').closest('[class^=col]').hide();
        $('[name=formAcaoImadiataNumeroAC]').closest('[class^=col]').hide();
        $('[name=formAcaoImadiataAnaliseCausa]').closest('[class^=col]').hide();

    },

    setHideRowAcaoImediata: function () {
        //debugger;
        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').closest('div').hide();
        $('[name=formAcaoImadiataTbObservacao]').closest('div').hide();
        $('[name=formAcaoImadiataTbEvidencia]').closest('div').hide();

    },

    //Interacao de Tela - StatusEtapa 2
    setShowAndHideStatusEtapa2: function () {

        this.setHideStatusEtapa2();
        this.setTriggerRadioButtonsEtapa2('change');
        this.setDisabledStatusEtapa2(true);
        this.setShowInputsEtapa2();
        this.setCheckHistorico();
        this.setHidePrazoReverificacao();
    },

    setHideStatusEtapa2: function () {

        //Botoes Acoes
        $('#tb-acao-imediata tbody tr').each(function () {
            //$(this).find('td').last().hide();
            $('.botoesTd').hide();
        });
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();

    },

    setDisabledStatusEtapa2: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDsJustificativa]').prop('disabled', _disabled);

        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);

        //Formulario Acao Imediata
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataDtDescricaoAcao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataJustificativa]').prop('disabled', _disabled);
        //Table Acao Imediata
        $('[name=formAcaoImadiataTbDescricao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtPrazoImplementacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbResponsavelImplementar]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbObservacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', _disabled);
        $('.upload-arq').prop('disabled', _disabled);
        if (_disabled) {
            $('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "none");
        } else {
            $('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "visible");
        }
        //Upload Changes
        $('[class^=btn-upload-form-acaoimediata-tb-evidencia]').closest('div').css('background-color', '#eee');
        $('[name=formAcaoImadiataTbEvidencia]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataECorrecao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataNecessitaAC]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelReverificacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataDtPrazoReverificacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelTratativa]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataAnaliseCausa]').prop('disabled', _disabled);


        $('.botaouploadarquivos').prop('disabled', _disabled);
    },

    setTriggerRadioButtonsEtapa2: function (_change) {

        $('[name=formAcaoImadiataEProcedente]').trigger(_change);
        $('[name=formAcaoImadiataECorrecao]').trigger(_change);
        $('[name=formAcaoImadiataNecessitaAC]').trigger(_change);

    },

    setShowInputsEtapa2: function () {

        var idUsuarioLogado = $('[name=UsuarioLogado]').val();
        var idPerfil = $('[name=IdPerfil]').val();
        $('#tb-acao-imediata tbody tr').each(function () {
            var idResponsavelImplementar = $(this).find('[name=formAcaoImadiataTbResponsavelImplementar]').val();
            if ((idResponsavelImplementar == idUsuarioLogado) || idPerfil != 4) {

                //[novo]
                if ($(this).find('[name=desabilitaData]').val() == "true") {
                    $(this).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', true);
                    $(this).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', true);
                    $(this).find('[name=formAcaoImadiataTbObservacao]').prop('disabled', true);

                    $(this).find('.botaouploadarquivos').prop('disabled', true);
                    $("#" + $(this).find('.botaouploadarquivos').data('outrobotaodesab')).find('.botaouploadarquivos').prop('disabled', false);

                    //$('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "none");
                    $(this).find('[name=anxFile]').find('a').css("pointer-events", "none");
                    $(this).find('[name=anxFile]').find('a').prop('disabled', true);

                    ///$(this).find('[name=Ianexo]').show();
                    ///$(this).find('[name=IanexoAnexar]').show();
                    //$(this).find('[name=IanexoLi]').hide();
                }
                else {
                    $(this).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', false);
                    $(this).find('[name=formAcaoImadiataTbObservacao]').prop('disabled', false);
                    $(this).find('.botaouploadarquivos').prop('disabled', false);
                    $("#" + $(this).find('.botaouploadarquivos').data('outrobotaodesab')).find('.botaouploadarquivos').prop('disabled', false);

                    //$(this).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val("");
                    //$('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "none");
                    //$(this).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', false);

                    $(this).find('[name=anxFile]').find('a').prop('disabled', false);
                    $(this).find('[name=anxFile]').find('a').css("pointer-events", "visible");

                    ///$(this).find('[name=Ianexo]').hide();
                    //if ($(this).find('[name=Ianexo]').val() == "") {
                    ///$(this).find('[name=IanexoAnexar]').show();
                    //}
                    //$(this).find('[name=IanexoLi]').hide();

                }
                $(this).find('[name=formAcaoImadiataTbEvidencia]').prop('disabled', false);
                $(this).find('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', false);
                $(this).find('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', false);
                $(this).find('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', false);
                $(this).find('[class^=btn-upload-form-acaoimediata-tb-evidencia]').closest('div').css('background-color', '#fff');
            }
        });

    },

    //Interacao de Tela - StatusEtapa 3
    setShowAndHideStatusEtapa3: function () {

        this.setHideStatusEtapa3();
        this.setTriggerRadioButtonsEtapa3('change');
        this.setDisabledStatusEtapa3(true);
        this.setShowInputsEtapa3();
        this.setCheckAcaoImediataOk();
        this.setCheckDtRevisao();
        this.setCheckAcaoImediataNotOk();
        //[novo]
        this.setCheckHistorico();
        this.setIncluirComentario();
        this.setBackChecksOnPage();
        this.setHidePrazoReverificacao();

    },

    setHideStatusEtapa3: function () {

        //Botoes Acoes
        $('#tb-acao-imediata tbody tr').each(function () {
            //$(this).find('td').last().hide();
            $('.botoesTd').hide();

            //$(this).find('td')[6].hide();
        });
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();


    },

    setDisabledStatusEtapa3: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDsJustificativa]').prop('disabled', _disabled);

        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);

        //Formulario Acao Imediata
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataDtDescricaoAcao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataJustificativa]').prop('disabled', _disabled);
        //Table Acao Imediata
        $('[name=formAcaoImadiataTbDescricao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtPrazoImplementacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbResponsavelImplementar]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbObservacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', _disabled);
        $('.upload-arq').prop('disabled', _disabled);
        if (_disabled) {
            $('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "none");
        } else {
            $('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "visible");
        }

        //Upload Changes
        $('[name^=formCriarNaoConformidadeEvidencia]').closest('div').css('background-color', '#eee');
        $('[name=formAcaoImadiataTbEvidencia]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataECorrecao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataNecessitaAC]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelReverificacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataDtPrazoReverificacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelTratativa]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataAnaliseCausa]').prop('disabled', _disabled);



        $('.botaouploadarquivos').prop('disabled', _disabled);
    },

    setTriggerRadioButtonsEtapa3: function (_change) {

        $('[name=formAcaoImadiataEProcedente]').trigger(_change);
        $('[name=formAcaoImadiataECorrecao]').trigger(_change);
        $('[name=formAcaoImadiataNecessitaAC]').trigger(_change);

    },

    setShowInputsEtapa3: function () {

        var idUsuarioLogado = $('[name=UsuarioLogado]').val();
        var idPerfil = $('[name=IdPerfil]').val();

        var idResponsavelReverificacao = $('[name=formAcaoImadiataResponsavelReverificacao]').val();
        if ((idResponsavelReverificacao == idUsuarioLogado) || idPerfil != 4) {
            $('#tb-acao-imediata tbody tr').each(function () {
                //$(this).find('td').last().show();
                $('.botoesTd').show();
            });

            $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').show();
            $('[name=formAcaoImadiataDtPrazoReverificacao]').closest('[class^=col]').show();

        }

    },
    setCheckDtRevisao: function () {


        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').change(function () {

            var checkSize = 0;
            debugger;
            var checkAcaoImediata = $('#tb-acao-imediata tbody tr').find('[name=formAcaoImadiataTbDtEfetivaImplementacao]');
            for (i = 0; i < checkAcaoImediata.size(); i++) {
                var valueAprovador = $(checkAcaoImediata[i]).val();
                if (valueAprovador == "") {
                    checkSize++;
                }
            }

            if (checkAcaoImediata.size() > 0) {

                if (checkSize !== 0) {
                    $('.dtReverificacao').removeClass('show').addClass('hide');
                }
                else {
                    $('.dtReverificacao').removeClass('hide').addClass('show');
                    let _disabled = $('[name=formAcaoImadiataDtPrazoReverificacao]').val() !== "" ? true : false;
                    $('[name=formAcaoImadiataDtPrazoReverificacao]').prop('disabled', _disabled);
                }
            }
            else 
                $('.dtReverificacao').removeClass('show').addClass('hide');
        });

    },
    setCheckAcaoImediataOk: function () {

        //$('.btn-confirm-acao-imediata').on('click', function () {
        //    

        //    $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
        //    $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);

        //    $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeIn(300);
        //    $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(true);
        //    APP.controller.NaoConformidadeController.getChecksAcaoImediata();
        //});



        $('.btn-confirm-acao-imediata').on('click', function () {

            $(this).closest('td').find('.btn-confirm-acao-imediata').delay(400).fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').delay(400).fadeOut(300);
            $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').delay(400).fadeOut(300);

            $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(true);
            APP.controller.NaoConformidadeController.getChecksAcaoImediata();
        });

    },

    setIncluirComentario: function () {
        $('#btn-new-comentario').on('click', function () {
            var motivo = $('[name=formNaoConformidadeComentarioMotivo]').val();
            var orientacao = $('[name=formNaoConformidadeComentarioOrientacao]').val();
            $('#' + $('#acaoImediataAtual').val()).closest('tr').find('[name=formAcaoImediataComentarioMotivo]').val(motivo);
            $('#' + $('#acaoImediataAtual').val()).closest('tr').find('[name=formAcaoImediataComentarioOrientacao]').val(orientacao);
            //alert($('#acaoImediataAtual').val());
            $('#modal-panel-form-cargos').modal("hide");
        });
    },

    setCheckAcaoImediataNotOk: function () {

        $('.btn-denied-acao-imediata').on('click', function () {

            $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeOut(300);


            $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(false);
            APP.controller.NaoConformidadeController.getChecksAcaoImediata();
            //modal - panel - form - cargos
            var atual = $($(this).closest('tr')[0]).find('[name=formAcaoImadiataTbDescricao]').attr('id');
            $('#acaoImediataAtual').val(atual);
            $('[name=formNaoConformidadeComentarioMotivo').val("");
            $('[name=formNaoConformidadeComentarioOrientacao').val("");


            $('#modal-panel-form-cargos').modal("show");
            //alert(atual);
        });

    },


    setCheckHistorico: function () {

        $('.btn-historico').on('click', function () {

            //$(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
            //$(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);
            //$(this).closest('td').find('.btn-notok-acao-imediata').delay(400).fadeIn(300);
            //var idAcaoImediata = $(this).closest('td').find('[name=formAcaoImadiataTbIdAcaoImediata]').val();
            var idAcaoImediata = $($(this).closest('tr')[0]).find('[name=formAcaoImadiataTbIdAcaoImediata]').val();
            //APP.controller.NaoConformidadeController.getChecksAcaoImediata();
            //var atual = $($(this).closest('tr')[0]).find('[name=formAcaoImadiataTbDescricao]').attr('id');
            //$('#acaoImediataAtual').val(atual);
            var data = {
                "idAcaoImediata": idAcaoImediata
            };


            $.ajax({
                type: "POST",
                dataType: 'json',
                data: data,
                url: "/NaoConformidade/ListarAcaoImediataComentarios",
                success: function (result) {
                    var teste = result;


                    var html = "";
                    $('#painelComentarios').html("");
                    if (result.Comentarios.length > 0) {
                        $(result.Comentarios).each(function () {

                            //APP.component.Datatoday.getCompareDate(dtEmissao, result.UltimaDataEmissao);
                            html += '<div class="row" >';
                            html += '    <div class="col-md-12 ">';
                            html += '        <div class="form-group" style="margin-bottom: 3px;">';
                            html += '            <label class=".input-data">Data/Horario: ' + this.DataComentario + '</label>';
                            html += '   <br>';
                            html += '            <label>Autor: ' + this.UsuarioComentario + '</label>';
                            html += '        </div>';
                            html += '    </div>';
                            html += '    <div class="col-md-12 ">';
                            html += '        <div class="form-group" style="margin-bottom: 10px;">';
                            html += '            <label for="form-naoconformidade-comentario-motivo">Motivo</label>';
                            html += '            <textarea rows="4" name="formNaoConformidadeComentarioMotivo" id="form-naoconformidade-comentario-motivo" class="form-control" value="" disabled>' + this.Motivo + '</textarea>';
                            html += '        </div>';
                            html += '    </div>';
                            html += '    <div class="col-md-12 ">';
                            html += '        <div class="form-group">';
                            html += '            <label for="form-naoconformidade-comentario-orientacao">Orientação</label>';
                            html += '            <textarea rows="4" name="formNaoConformidadeComentarioOrientacao" id="form-naoconformidade-comentario-orientacao" class="form-control" value="" disabled>' + this.Orientacao + '</textarea>';
                            html += '        </div>';
                            html += '    </div>';
                            html += '</div>';

                            $('#painelComentarios').html(html);

                            //var hasItem = APP.controller.AuditoriaController.getProcessoMesAno(this);

                            //if (hasItem) {

                            //    var formAuditoriaAnoObj = {
                            //        formAuditoriaAno: $(this).find('.pai-ano span').text(),
                            //        formAuditoriaMeses: APP.controller.AuditoriaController.getObjFormAuditoriaMeses(this),
                            //    };

                            //    arrayFormAuditoriaAnoObj.push(formAuditoriaAnoObj);
                            //}
                        });
                    } else {
                        $('#painelComentarios').html("Sem registros no histórico.");
                    }



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
                    var erro = result;
                    //erro = APP.component.ResultErros.init(result.Erro);
                    //bootbox.alert(erro);
                },
                //complete: function (result) {
                //    APP.component.Loading.hideLoading();
            });



            $('#modal-panel-form-cargos2').modal("show");

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
            $('[name=formAcaoImadiataParecer]').show();
            $('label[for=form-acaoimediata-parecer]').show();
        } else {
            $('[name=formAcaoImadiataFoiEficaz').filter('[value=false]').prop('checked', true);
            $('[name=formAcaoImadiataParecer]').hide();
            $('label[for=form-acaoimediata-parecer]').hide();
        }

    },

    setBackChecksOnPage: function () {
        $('.fa-check-circle, .fa-times-circle').unbind('click');
        $('.fa-check-circle, .fa-times-circle').on('click', function () {
            //$(this).closest('td').find('.btn-ok-acao-imediata').fadeOut(300);
            //$(this).closest('td').find('.btn-notok-acao-imediata').fadeOut(300);
            //$(this).closest('td').find('.btn-confirm-acao-imediata').delay(400).fadeIn(300);
            //$(this).closest('td').find('.btn-denied-acao-imediata').delay(400).fadeIn(300);
            //$(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val('');
            //APP.controller.NaoConformidadeController.getChecksAcaoImediata();

        });
    },

    //Interacao de Tela - StatusEtapa 4
    setShowAndHideStatusEtapa4: function () {

        this.setHideStatusEtapa4();
        this.setTriggerRadioButtonsEtapa4('change');
        this.setDisabledStatusEtapa4(true);
        this.setShowInputsEtapa4();
        this.setHidePrazoReverificacao();
        this.setCheckDtRevisao();
    },

    setShowAndHideStatusEtapa5: function () {


        $("#painel-acao-corretiva-nao").show();
        $('[name=formCriarNaoConformidadeDsJustificativa]').prop('disabled', false);

        $('.pnl-anular').hide();

        this.setHideStatusEtapa4();
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

    },

    setTriggerRadioButtonsEtapa4: function (_change) {

        $('[name=formAcaoImadiataEProcedente]').trigger(_change);
        $('[name=formAcaoImadiataECorrecao]').trigger(_change);
        $('[name=formAcaoImadiataNecessitaAC]').trigger(_change);

    },

    setDisabledStatusEtapa4: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarNaoConformidadeDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDsJustificativa]').prop('disabled', _disabled);

        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarNaoConformidadeEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeProcesso]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEmissor]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarNaoConformidadeTags]').prop('disabled', _disabled);

        //Formulario Acao Imediata
        $('[name=formAcaoImadiataEProcedente]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataDtDescricaoAcao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataJustificativa]').prop('disabled', _disabled);
        //Table Acao Imediata
        $('[name=formAcaoImadiataTbDescricao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtPrazoImplementacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbResponsavelImplementar]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbObservacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').prop('disabled', _disabled);
        $('.upload-arq form-control').prop('disabled', _disabled);
        if (_disabled) {
            $('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "none");
        } else {
            $('.upload-arq.form-control').not('.box-upload-arq').find('a').css("pointer-events", "visible");
        }
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
        $('[name=formAcaoImadiataDtPrazoReverificacao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataFoiEficaz]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelTratativa]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataAnaliseCausa]').prop('disabled', _disabled);


        $('.botaouploadarquivos').prop('disabled', _disabled);
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

    //Rules
    setRulesEAuditoria: function (_EAuditoria) {

        if (_EAuditoria == "true") {
            $('#tb-acao-imediata').closest('div').show();
            $('#form-acaoimediata-e-procedente-sim').prop('checked', true);
            $('[name=formAcaoImadiataEProcedente]').prop('disabled', true);
            $('#form-acaoimediata-necessita-ac-sim').prop('checked', true);
            $('[name=formAcaoImadiataNecessitaAC]').prop('disabled', true);
        } else {

        }

    },

    setRulesEProcedente: function (_EProcedente, _checkTbAcaoCorretiva) {

        if (_EProcedente == "true" && _checkTbAcaoCorretiva > 0) {
            $('[name=formAcaoImadiataJustificativa]').closest('[class^=col]').hide();
            $('#tb-acao-imediata').closest('div').show();

            APP.controller.NaoConformidadeController.setShowPanelEProcedenteSim();

            $('[name=formAcaoImadiataECorrecao]').trigger("change");

            $('[name=formAcaoImadiataNecessitaAC]').trigger("change");
        } else if (_EProcedente == "true") {
            $('[name=formAcaoImadiataJustificativa]').closest('[class^=col]').hide();
            $('#tb-acao-imediata').closest('div').show();
        } else if ((_EProcedente == undefined || _EProcedente == null) && _checkTbAcaoCorretiva == 0) {

            $('[name=formAcaoImadiataJustificativa]').closest('[class^=col]').hide();

        }
        else {
            $('[name=formAcaoImadiataJustificativa]').closest('[class^=col]').show();
            APP.controller.NaoConformidadeController.setHidePanelEProcedenteSim();
            APP.controller.NaoConformidadeController.setValidateEProcedente(_EProcedente);
        }

    },

    setRulesECorrecao: function (_ECorrecao) {

        if (_ECorrecao == "true") {
            $('[name=formAcaoImadiataResponsavelReverificacao]').closest('[class^=col]').show();
        } else {
            $('[name=formAcaoImadiataResponsavelReverificacao]').closest('[class^=col]').hide();
        }

    },

    setRulesNecessitaAC: function (_NecessitaAC) {

        if (_NecessitaAC == "true") {
            $('[name=formAcaoImadiataResponsavelTratativa]').closest('[class^=col]').show();
            $('[name=formAcaoImadiataAnaliseCausa]').closest('[class^=col]').show();

        } else if (_NecessitaAC == "false") {
            $('[name=formAcaoImadiataResponsavelTratativa]').closest('[class^=col]').hide();
            $('[name=formAcaoImadiataNumeroAC]').closest('[class^=col]').hide();
            $('[name=formAcaoImadiataAnaliseCausa]').closest('[class^=col]').hide();

        } else {
            $('[name=formAcaoImadiataResponsavelTratativa]').closest('[class^=col]').hide();
            $('[name=formAcaoImadiataNumeroAC]').closest('[class^=col]').hide();
            $('[name=formAcaoImadiataAnaliseCausa]').closest('[class^=col]').hide();

        }

    },

    setShowPanelEProcedenteSim: function () {

        $('[name=formAcaoImadiataECorrecao]').closest('[class^=col]').show();
        $('[name=formAcaoImadiataNecessitaAC]').closest('[class^=col]').show();
        $('[name=formAcaoImadiataResponsavelTratativa]').closest('[class^=col]').show();
        $('[name=formAcaoImadiataAnaliseCausa]').closest('[class^=col]').show();


        $('[name=formAcaoImadiataECorrecao]').trigger("change");
        $('[name=formAcaoImadiataNecessitaAC]').trigger("change");
        //APP.controller.NaoConformidadeController.setRulesNecessitaAC();

    },

    //Changes
    setEProcedente: function () {

        $('[name=formAcaoImadiataEProcedente]').on('change', function () {

            //Verifica se possui AC na tabela - Sim: libera panel AC - Nao: trava panel AC
            var checkTbAcaoCorretiva = $('#tb-acao-imediata tbody tr').size();

            var EProcedente = APP.controller.NaoConformidadeController.getEProcedente('formAcaoImadiataEProcedente');
            APP.controller.NaoConformidadeController.setRulesEProcedente(EProcedente, checkTbAcaoCorretiva);

        });

    },

    setEProcedenteInicializacao: function () {

        var checkTbAcaoCorretiva = $('#tb-acao-imediata tbody tr').size();

        var EProcedente = APP.controller.NaoConformidadeController.getEProcedente('formAcaoImadiataEProcedente');
        APP.controller.NaoConformidadeController.setRulesEProcedente(EProcedente, checkTbAcaoCorretiva);

    },

    setECorrecao: function () {

        $('[name=formAcaoImadiataECorrecao]').on('change', function () {

            var ECorrecao = APP.controller.NaoConformidadeController.getECorrecao('formAcaoImadiataECorrecao');
            APP.controller.NaoConformidadeController.setRulesECorrecao(ECorrecao);

        });

    },

    setNecessitaAC: function () {

        $('[name=formAcaoImadiataNecessitaAC]').on('change', function () {

            var NecessitaAC = APP.controller.NaoConformidadeController.getNecessitaAC('formAcaoImadiataNecessitaAC');
            APP.controller.NaoConformidadeController.setRulesNecessitaAC(NecessitaAC);

        });

    },

    //Auxiliares
    getEAuditoria: function () {

        var EAuditoria = APP.component.Radio.init('formCriarNaoConformidadeEAuditoria');
        return EAuditoria;

    },

    getEProcedente: function () {

        var EProcedente = APP.component.Radio.init('formAcaoImadiataEProcedente');
        return EProcedente;

    },

    getECorrecao: function () {

        var ECorrecao = APP.component.Radio.init('formAcaoImadiataECorrecao');
        return ECorrecao;

    },

    getNecessitaAC: function () {

        var NecessitaAC = APP.component.Radio.init('formAcaoImadiataNecessitaAC');
        return NecessitaAC;

    },

    getFoiEficaz: function () {

        var FoiEficaz = APP.component.Radio.init('formAcaoImadiataFoiEficaz');
        return FoiEficaz;

    },

    //Validates
    setValidateEProcedente: function (_EProcedente) {

        var ObjFormAcaoImediataEProcedenteValidate = {};
        if (_EProcedente == "true") {
            ObjFormAcaoImediataEProcedenteValidate = {
                formAcaoImadiataJustificativa: { 'required': false },
            };
            APP.component.ValidateForm.init(ObjFormAcaoImediataEProcedenteValidate, '#form-acaoimediata');
        } else {
            ObjFormAcaoImediataEProcedenteValidate = {
                formAcaoImadiataJustificativa: { 'required': true },
            };

            APP.component.ValidateForm.init(ObjFormAcaoImediataEProcedenteValidate, '#form-acaoimediata');
        }

    },

    //Formulario Editar Nao Conformidade
    formEditarNaoConformidade: function () {

        this.setAndHideEditarNaoConformidade();
        //this.getformCriarNaoConformidadeDtEmissao();
        this.getProcessosPorSite();
        this.getEmissorPorSite();
        this.getTipoNaoConformidade();
        this.setAddTipoNaoConformidade();

        this.setResponsavelAnaliseDefinicaoAC();

    },

    //Formulario Criar Nao Conformidade
    formCriarNaoConformidade: function () {

        this.setAndHideCriarNaoConformidade();
        this.getformCriarNaoConformidadeDtEmissao();
        this.getProcessosPorSite();
        this.getEmissorPorSite();
        this.getTipoNaoConformidade();
        this.setAddTipoNaoConformidade();

        this.setResponsavelAnaliseDefinicaoAC();

    },

    setAndHideEditarNaoConformidade: function () {
        var idPerfil = $('[name=IdPerfil]').val();
        if (idPerfil == 4)
            this.buttonDestravar.hide();
        else
            this.buttonDestravar.show();
    },

    setAndHideCriarNaoConformidade: function () {
        $('#numeroRegistro').hide();
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
        var idFuncao = 12; // Funcionalidade(Cadastrar) que permite usuario criar nc
        $.get('/Usuario/ObterUsuariosPorFuncao?idSite=' + idSite + '&idFuncao=' + idFuncao, (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarNaoConformidadeEmissor] option'), $('[name=formCriarNaoConformidadeEmissor]'), 'IdUsuario', 'NmCompleto');
            }
        });

    },

    getTipoNaoConformidade: function () {

        var idSite = $('#nao-conformidade-site').val();
        $.get('/ControladorCategorias/ListaAtivos?tipo=tnc&site=' + idSite, function (result) {
            $.each(result.Lista, (key, val) => {
                if (result.StatusCode == 202) {
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarNaoConformidadeTipo] option'), $('[name=formCriarNaoConformidadeTipo]'), 'IdControladorCategorias', 'Descricao');
                }
            });
        });

    },

    setAddTipoNaoConformidade: function () {

        $('.add-tipo-nao-conformidade').on('click', function () {

            var idSite = $('#nao-conformidade-site').val();
            APP.controller.ControladorCategoriasController.init(idSite, 'tnc', APP.controller.NaoConformidadeController.getTipoNaoConformidade, ".add-tipo-nao-conformidade");

        });

    },

    setResponsavelAnaliseDefinicaoAC: function () {

        $('[name=formCriarNaoConformidadeProcesso]').on('change', function () {
            var idSite = $('#nao-conformidade-site').val();
            processoSelecionado = $(this).find(':selected').val();
            var idFuncao = 14; // Funcionalidade(Define aÃ§Ã£o)
            $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + processoSelecionado + '&idSite=' + idSite + '&idFuncao=' + idFuncao, (result) => {
                if (result.StatusCode == 200) {
                    $('[name=formCriarNaoConformidadeResponsavel] option').not(':first-child').remove();
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name="formCriarNaoConformidadeResponsavel"] option'), $('[name="formCriarNaoConformidadeResponsavel"]'), 'IdUsuario', 'NmCompleto');
                }
            });

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
                                var naoConformidadeObj = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj(_fluxo);
                                APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidadeObj, _fluxo);
                            }
                        });
                    } else {
                        var naoConformidadeObj = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj(_fluxo);
                        APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidadeObj, _fluxo);
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
            formCriarNaoConformidadeEAuditoria: 'required',
            formCriarNaoConformidadeTipo: 'required',
            formCriarNaoConformidadeResponsavel: 'required',
            //formCriarNaoConformidadeTags: 'required',

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
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.NaoConformidadeController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.NaoConformidadeController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    ENaoConformidadeAuditoria: APP.component.Radio.init('formCriarNaoConformidadeEAuditoria'),
                    IdTipoNaoConformidade: $('[name=formCriarNaoConformidadeTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };

                break;

            case "fluxo-01":
                //Obj enviado no fluxo 01 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: 1,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    EProcedente: APP.component.Radio.init('formAcaoImadiataEProcedente'),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.NaoConformidadeController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.NaoConformidadeController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    ENaoConformidadeAuditoria: APP.component.Radio.init('formCriarNaoConformidadeEAuditoria'),
                    IdTipoNaoConformidade: $('[name=formCriarNaoConformidadeTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };

                break;

            case "fluxo-02":
                //Obj enviado no fluxo 02 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: 2,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    EProcedente: APP.component.Radio.init('formAcaoImadiataEProcedente'),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    AcoesImediatas: APP.controller.NaoConformidadeController.getObjFormAcaoImediata(),
                    ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao] :selected').val(),
                    DtRevisao: $('[name=formAcaoImadiataDtPrazoReverificacao]').val(),
                    IdResponsavelPorIniciarTratativaAcaoCorretiva: $('[name=formAcaoImadiataResponsavelTratativa]').val(),
                    DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
                    Parecer: $('[name=formAcaoImadiataParecer]').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.NaoConformidadeController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.NaoConformidadeController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    ENaoConformidadeAuditoria: APP.component.Radio.init('formCriarNaoConformidadeEAuditoria'),
                    IdTipoNaoConformidade: $('[name=formCriarNaoConformidadeTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };

                break;
            case "fluxo-03":
                //Obj enviado no fluxo 03 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: 2,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    EProcedente: APP.component.Radio.init('formAcaoImadiataEProcedente'),
                    AcoesImediatas: APP.controller.NaoConformidadeController.getObjFormAcaoImediata(),
                    ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
                    NumeroAcaoCorretiva: $('[name=formAcaoImadiataNumeroAC]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao] :selected').val(),
                    DtRevisao: $('[name=formAcaoImadiataDtPrazoReverificacao]').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.NaoConformidadeController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.NaoConformidadeController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    ENaoConformidadeAuditoria: APP.component.Radio.init('formCriarNaoConformidadeEAuditoria'),
                    IdTipoNaoConformidade: $('[name=formCriarNaoConformidadeTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    IdResponsavelPorIniciarTratativaAcaoCorretiva: $('[name=formAcaoImadiataResponsavelTratativa]').val(),
                    DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
                    Parecer: $('[name=formAcaoImadiataParecer]').val(),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };
                break;
            case "fluxo-04":
                //Obj enviado no fluxo 04 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: $('[name=StatusEtapa]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    AcoesImediatas: APP.controller.NaoConformidadeController.getObjFormAcaoImediata(),
                    ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    FlEficaz: APP.controller.NaoConformidadeController.getFoiEficaz(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    IdTipoNaoConformidade: $('[name=formCriarNaoConformidadeTipo] :selected').val(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    ENaoConformidadeAuditoria: APP.component.Radio.init('formCriarNaoConformidadeEAuditoria'),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    CriticidadeGestaoDeRisco: $('[name=formCriarNaoConformidadeCriticidade] :selected').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao] :selected').val(),
                    DtRevisao: $('[name=formAcaoImadiataDtPrazoReverificacao]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    Observacao: $('[name=formAcaoImadiataTbObservacao]').val(),
                    DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    ArquivosDeEvidenciaAux: APP.controller.NaoConformidadeController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.NaoConformidadeController.getAnexosArquivosNaoConformidadeAnexos(),
                    Causa: $('[name=formCausa]').val(),
                    DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
                    Parecer: $('[name=formAcaoImadiataParecer]').val(),
                };
                break;
            case "fluxo-05":

                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: $('[name=StatusEtapa]').val(),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    AcoesImediatas: APP.controller.NaoConformidadeController.getObjFormAcaoImediata(),
                    FlEficaz: APP.controller.NaoConformidadeController.getFoiEficaz(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    IdTipoNaoConformidade: $('[name=formCriarNaoConformidadeTipo] :selected').val(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    ENaoConformidadeAuditoria: APP.component.Radio.init('formCriarNaoConformidadeEAuditoria'),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    CriticidadeGestaoDeRisco: $('[name=formCriarNaoConformidadeCriticidade] :selected').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao] :selected').val(),
                    DtRevisao: $('[name=formAcaoImadiataDtPrazoReverificacao]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    Observacao: $('[name=formAcaoImadiataTbObservacao]').val(),
                    DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    ArquivosDeEvidenciaAux: APP.controller.NaoConformidadeController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.NaoConformidadeController.getAnexosArquivosNaoConformidadeAnexos(),
                    Causa: $('[name=formCausa]').val(),
                    DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
                    Parecer: $('[name=formAcaoImadiataParecer]').val(),

                    ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),

                    NumeroAcaoCorretiva: $('[name=formAcaoImadiataNumeroAC]').val(),
                    IdResponsavelPorIniciarTratativaAcaoCorretiva: $('[name=formAcaoImadiataResponsavelTratativa]').val(),

                };
                break;
            case "fluxo-06":
                //Obj enviado no fluxo 04 de edicao
                acoesNaoConformidadeFormCriarNaoConformidadeObj = {
                    StatusEtapa: $('[name=StatusEtapa]').val(),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    NuRegistro: $("#form-criar-nao-conformidade-nm-registro").val(),
                    AcoesImediatas: APP.controller.NaoConformidadeController.getObjFormAcaoImediata(),
                    FlEficaz: APP.controller.NaoConformidadeController.getFoiEficaz(),
                    Tags: $('[name=formCriarNaoConformidadeTags]').val(),
                    IdEmissor: $('[name=formCriarNaoConformidadeEmissor] :selected').val(),
                    IdProcesso: $('[name=formCriarNaoConformidadeProcesso] :selected').val(),
                    DtEmissao: $('[name=formCriarNaoConformidadeDtEmissao]').val(),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarNaoConformidadeResponsavel] :selected').val(),
                    CriticidadeAcaoCorretiva: $('[name=formCriarNaoConformidadeCriticidade] :selected').val(),
                    DescricaoRegistro: $('[name=formCriarNaoConformidadeDsRegistro]').val(),
                    DsJustificativa: $('[name=formCriarNaoConformidadeDsJustificativa]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
                    DtRevisao: $('[name=formAcaoImadiataDtPrazoReverificacao]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    Observacao: $('[name=formAcaoImadiataTbObservacao]').val(),
                    DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    ArquivosDeEvidenciaAux: APP.controller.NaoConformidadeController.getAnexosEvidencias(),
                    Causa: $('[name=formCausa]').val(),
                    Parecer: $('[name=formAcaoImadiataParecer]').val(),
                };
                break;
        }

        return acoesNaoConformidadeFormCriarNaoConformidadeObj;

    },


    getAnexosAcaoImediata(identificador) {
        let raiz = $("#modal-rai" + identificador)[0];
        let ret = FileUploadGlobal_getArrArquivoRaiz(raiz, "IdArquivoDeEvidenciaAcaoImediata", "IdAcaoImediata");
        return ret;
    },

    getAnexosArquivosNaoConformidadeAnexos() {
        let raiz = $("#modal-rai" + "ncabeca")[0];
        let ret = FileUploadGlobal_getArrArquivoRaiz(raiz, "IdArquivoNaoConformidadeAnexo", "IdRegistroConformidade");
        return ret;
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
        this.getResponsavelTratativaAcaoImediata();

    },

    setAndHideAcaoImediata: function () {

        //

    },

    getformCriarNaoConformidadeDtDescricaoAcao: function () {
        var statusEtapa = parseInt($('[name=StatusEtapa]').val());

        if (statusEtapa <= 1)
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
            html += '<textarea type="text" id="form-acaoimediata-tb-descricao' + _options.NumeroAcaoImediataGrid + '" name="formAcaoImadiataTbDescricao" class="form-control" ';
            html += 'placeholder="Descrição" ';
            html += 'data-msg-required="" ';
            html += 'value=""';
            html += 'rows="3"></textarea>';
            html += '<input type="hidden" name="formAcaoImadiataTbIdAcaoImediata" class="form-control input-data" value="0"/>';
            html += '<input type="hidden" name="formAcaoImadiataTbEstado" value="4"/>';
            html += '</td>';




            html += '<td>';
            html += '<div class="input-group input-group-datepicker">';
            html += '<input type="text" name="formAcaoImadiataTbDtPrazoImplementacao" id="form-acaoimediata-tb-dt-prazo-implementacao' + _options.NumeroAcaoImediataGrid + '" class="form-control data datepicker largura-calendario" ';
            html += 'data-msg-required="" ';
            html += 'value="" />';
            html += '<span class="input-group-addon">';
            html += '<i class="fa fa-calendar" onclick="AbreCalendario(this)" aria-hidden="true"></i>';
            html += '</span>';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<select id="form-acaoimediata-tb-responsavel-implementar" name="formAcaoImadiataTbResponsavelImplementar" class="form-control" ';
            html += 'data-msg-required="">';
            html += '<option value="">' + TraducaoDropNameSelect + '</option>';
            html += '</select>';
            html += '</td>';

            html += '   <td>';
            html += '<div class="input-group input-group-datepicker">';
            html += '<input type="text" name="formAcaoImadiataTbDtEfetivaImplementacao" id="form-acaoimediata-tb-dt-efetiva-implementacao' + _options.NumeroAcaoImediataGrid + '" class="form-control data datepicker dataEfetivaImplementacaoDatePicker" ';
            html += 'data-msg-required="" ';
            html += 'value="" />';
            html += '<span class="input-group-addon">';
            html += '<i class="fa fa-calendar" aria-hidden="true"></i>';
            html += '</span>';
            html += '</div>';
            html += '</td>';


            //html += '<td>';
            //html += '<textarea type="text" name="formAcaoImadiataTbObservacao" class="form-control"></textarea>';
            //html += '</td>';


            html += '   <td>';



            html += '<div class="upload-arq form-control">';
            html += '<a class="btn-upload-form-acaoimediata-tb-evidencia-' + index + '">';
            html += '<i class="fa fa-paperclip fa-1x" aria-hidden="true"></i><br>Anexar';
            html += '</a>';
            html += '<input type="file" name="formAcaoImadiataTbEvidencia" id="form-acaoimediata-tb-evidencia-' + index + '" class="" data-msg-required="" data-b64="">';
            html += '</div>';
            html += '<ul></ul>';
            html += '</td>';
            html += '<td>';
            html += '</td>';
            html += '<td>';
            html += '</td>';
            html += '<td>';
            html += '<a href="#" class="btn-delete-acao-imediata icon-cliente trash-color">';
            html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</a>';
            html += '</td>';
            html += '</tr>';

            $('#tb-acao-imediata tbody').append(html);
            $('[name=formAcaoImadiataResponsavelReverificacao]').prop('disabled', false);
            $('[name=formAcaoImadiataResponsavelTratativa]').prop('disabled', false);

            $('[name=formAcaoImadiataNecessitaAC]').prop('disabled', false);
            $('[name=formAcaoImadiataECorrecao]').prop('disabled', false);

            $('[name=formAcaoImadiataResponsavelReverificacao]').removeClass('hide').add('show');
            $('[name=formAcaoImadiataDtPrazoReverificacao]').removeClass('hide').add('show');
            $('[name=formAcaoImadiataResponsavelTratativa]').removeClass('hide').add('show');
            $('.add-acao-imediata').removeClass('show').addClass('hide');
            APP.controller.NaoConformidadeController.bind();


            //if ($('[name=StatusEtapa]').val() == 1) {
            //	APP.controller.NaoConformidadeController.bind();
            //} else {
            //	APP.controller.NaoConformidadeController.bindAcao();
            //	//$('[name=formAcaoImadiataTbDtEfetivaImplementacao]').closest('div').hide();
            //	//$('[name=formAcaoImadiataTbObservacao]').closest('div').hide();
            //	$('[name=formAcaoImadiataTbEvidencia]').closest('div').hide();
            //}
            _options.NumeroAcaoImediataGrid++;

        });

    },

    delAcaoImediata: function () {

        $('.btn-delete-acao-imediata').unbind('click');
        $('.btn-delete-acao-imediata').on('click', function (event) {
            event.preventDefault();

            var idAcaoImediata = $(this).closest('tr').find('[name="formAcaoImadiataTbIdAcaoImediata"]').val();
            var tableRow = $(this).closest('tr');

            var modal = bootbox.dialog({
                title: 'Excluir ação imediata.',
                message: 'Deseja excluir está ação imediata ?',
                buttons: [
                    {
                        label: _options.labelButtonYes,
                        className: "btn btn-primary pull-right",
                        callback: function () {

                            $.ajax({
                                type: "POST",
                                dataType: 'json',
                                url: '/AcaoImediata/ExcluirAcaoImediata?IdAcaoImediata=' + idAcaoImediata,
                                beforeSend: function () {
                                    APP.component.Loading.showLoading();
                                },
                                success: function (result) {
                                    if (result.StatusCode == 200) {
                                        bootbox.alert("Ação imediata excluida com sucesso!");
                                    }
                                },
                                error: function (result) {
                                    bootbox.alert(_options.MsgOcorreuErro);
                                },
                                complete: function (result) {
                                    tableRow.remove();
                                    APP.component.Loading.hideLoading();
                                }
                            });
                        }
                    },
                    {
                        label: _options.botao_cancelar,
                        className: "btn btn-default pull-right",
                        callback: function () {
                            modal.modal("hide");
                        }
                    }
                ],
                show: false,
                onEscape: function () {
                    modal.modal("hide");
                }
            });

            modal.modal("show");

        });
    },

    getResponsavelImplementarAcaoImediata: function () {
        var idSite = $('#nao-conformidade-site').val();
        var idFuncao = 15; // Funcionalidade(Implementar aÃ§Ã£o) que permite usuario Implementar aÃ§Ã£o NC
        var idProcesso = $('[name=IdProcesso]').val();
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: '/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=idProcesso&idSite=' + idSite + '&idFuncao=' + idFuncao,
            beforeSend: function () {
                $('.add-acao-imediata').removeClass('hide').addClass('show');
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    //debugger;
                    //APP.component.SelectListCompare.selectList(result.Lista, $('#tb-acao-imediata tbody tr:last-child [name="formAcaoImadiataTbResponsavelImplementar"] option'), $('#tb-acao-imediata tbody tr:last-child [name="formAcaoImadiataTbResponsavelImplementar"]'), 'IdUsuario', 'NmCompleto');
                    $('[name="formAcaoImadiataTbResponsavelImplementar"]').each(function () {
                        APP.component.SelectListCompare.selectList(result.Lista, $(this).find('option'), $(this), 'IdUsuario', 'NmCompleto');
                    });
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {
                if ($('[name=StatusEtapa]').val() == "2" || $('[name=StatusEtapa]').val() == "1") {
                    $('.add-acao-imediata').removeClass('hide').addClass('show');
                }
            }
        });

    },

    getResponsavelReverificarAcaoImediata: function () {
        var idSite = $('#nao-conformidade-site').val();
        var idProcesso = $('[name=IdProcesso]').val();
        var idFuncao = 16; // Funcionalidade(ReverificaÃ§Ã£o) que permite usuario Reverifique NC
        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=idProcesso&idSite=' + idSite + '&idFuncao=' + idFuncao, function (result) {
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

    getResponsavelTratativaAcaoImediata: function () {

        var idSite = $('#nao-conformidade-site').val();
        var idProcesso = $('[name=IdProcesso]').val();
        var idFuncao = 20; // Funcionalidade(AnÃ¡lise da Causa) que permite usuario Criar nova AC
        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=idProcesso&idSite=' + idSite + '&idFuncao=' + idFuncao + '', function (result) {
            if (result.StatusCode == 200) {
                $(result.Lista).each(function (key, value) {
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name="formAcaoImadiataResponsavelTratativa"] option'), $('[name="formAcaoImadiataResponsavelTratativa"]'), 'IdUsuario', 'NmCompleto');
                });
            }

        });

    },

    bind: function () {

        APP.component.Datapicker.init();
        APP.controller.NaoConformidadeController.setup();
        APP.controller.NaoConformidadeController.setAcaoImediata();
        APP.controller.NaoConformidadeController.getResponsavelImplementarAcaoImediata();
        APP.component.FileUpload.init();
        APP.controller.NaoConformidadeController.setShowPanelEProcedenteSim();
        APP.controller.NaoConformidadeController.setHideRowAcaoImediata();
        APP.controller.NaoConformidadeController.delAcaoImediata();

    },

    bindAcao: function () {

        APP.component.Datapicker.init();
        APP.controller.NaoConformidadeController.setup();
        APP.controller.NaoConformidadeController.setAcaoImediata();
        APP.controller.NaoConformidadeController.getResponsavelImplementarAcaoImediata();
        APP.component.FileUpload.init();
        APP.controller.NaoConformidadeController.setShowPanelEProcedenteSim();
        //APP.controller.NaoConformidadeController.setHideRowAcaoImediata();
        APP.controller.NaoConformidadeController.delAcaoImediata();

    },

    getObjObjFormAcaoImediataValidate: function () {

        var acoesNaoConformidadeoFormAcaoImediataObj = {

            //Form Acao Imediata
            formAcaoImadiataEProcedente: { 'required': true },
            formAcaoImadiataDtDescricaoAcao: { 'required': true },
            //formAcaoImadiataJustificativa: {'required': true},
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

        return acoesNaoConformidadeoFormAcaoImediataObj;

    },

    getObjFormAcaoImediata: function () {

        var idUsuarioLogado = $('[name=UsuarioLogado]').val();
        var statusEtapa = parseInt($('[name=StatusEtapa]').val());

        var getObjFormAcaoImediataArray = [];
        var acoesNaoConformidadeFormAcaoImediataObj = {};
        var acoesImediatasComentarios = {};

        var anexoEvidenciaModel = APP.controller.ClienteController.models.AnexoModel;


        $('#tb-acao-imediata tbody tr').each(function (index, tr) {
            if (statusEtapa == 2) {
                acoesNaoConformidadeFormAcaoImediataObj = {
                    Aprovado: $(tr).find('[name=formAcaoImadiataTbAprovado]').val(),
                    Descricao: $(tr).find('[name=formAcaoImadiataTbDescricao]').val(),
                    Observacao: $(tr).find('[name=formAcaoImadiataTbObservacao]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=formAcaoImadiataTbResponsavelImplementar]').val(),

                    DtEfetivaImplementacao: $(tr).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    IdFilaEnvio: $(tr).find('[name=formAcaoImediataIdFilaEnvio]').val(),
                    ArquivoEvidenciaAux: anexoEvidenciaModel.constructor(
                        $(tr).find('[name^=formCriarNaoConformidadeEvidencia]').closest('td').find('[name^=formCriarNaoConformidadeEvidenciaIdAnexo]').val(),
                        $(tr).find('[name^=formCriarNaoConformidadeEvidencia]').closest('td').find('i').text(),
                        $(tr).find('[id^=form-acaoimediata-tb-evidencia-]').data('b64')
                    ),
                    SubmitArquivoEvidencia: APP.controller.NaoConformidadeController.getAnexosAcaoImediata($(tr).find(".IdentificadorInicialupload").data("identificador")),
                    Estado: $(tr).find('[name=formAcaoImadiataTbEstado]').val() != 0 ? 16 : 0,
                    IdAcaoImediata: $(tr).find('[name=formAcaoImadiataTbIdAcaoImediata]').val(),
                };
            } else if (statusEtapa == 3) {

                acoesNaoConformidadeFormAcaoImediataObj = {
                    Observacao: $(tr).find('[name=formAcaoImadiataTbObservacao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $(tr).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),

                    Aprovado: $(tr).find('[name=formAcaoImadiataTbAprovado]').val(),
                    IdAcaoImediata: $(tr).find('[name=formAcaoImadiataTbIdAcaoImediata]').val(),
                    Estado: $(tr).find('[name=formAcaoImadiataTbEstado]').val() != 0 ? 16 : 0,
                    IdFilaEnvio: $(tr).find('[name=formAcaoImediataIdFilaEnvio]').val(),
                    Motivo: $(tr).find('[name=formAcaoImediataComentarioMotivo]').val(),
                    Orientacao: $(tr).find('[name=formAcaoImediataComentarioOrientacao]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    Descricao: $(tr).find('[name=formAcaoImadiataTbDescricao]').val()
                    //ComentariosAcaoImediata: acoesImediatasComentarios = {
                    //    Motivo: $(tr).find('[name=formAcaoImediataComentarioMotivo]').val(),
                    //    Orientacao: $(tr).find('[name=formAcaoImediataComentarioOrientacao]').val()
                    //}

                };

            } else {

                acoesNaoConformidadeFormAcaoImediataObj = {




                    //Aprovado: $(tr).find('[name=formAcaoImadiataTbAprovado]').val(),
                    Descricao: $(tr).find('[name=formAcaoImadiataTbDescricao]').val(),
                    Observacao: $(tr).find('[name=formAcaoImadiataTbObservacao]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $(tr).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    Estado: $(tr).find('[name=formAcaoImadiataTbEstado]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    IdAcaoImediata: $(tr).find('[name=formAcaoImadiataTbIdAcaoImediata]').val(),
                    IdFilaEnvio: $(tr).find('[name=formAcaoImediataIdFilaEnvio]').val(),
                    ArquivoEvidenciaAux: anexoEvidenciaModel.constructor(
                        $(tr).find('[name^=formCriarNaoConformidadeEvidencia]').closest('td').find('[name^=formCriarNaoConformidadeEvidenciaIdAnexo]').val(),
                        $(tr).find('[name^=formCriarNaoConformidadeEvidencia]').closest('td').find('i').text(),
                        $(tr).find('[id^=form-acaoimediata-tb-evidencia-]').data('b64')
                    ),
                    SubmitArquivoEvidencia: APP.controller.NaoConformidadeController.getAnexosAcaoImediata($(tr).find(".IdentificadorInicialupload").data("identificador")),






                    //ComentarioMotivo: $(tr).find('[name=formAcaoImediataComentarioMotivo]').val(),
                    //ComentarioOrientacao: $(tr).find('[nameformAcaoImediataComentarioOrientacaoformAcaoImadiataTbIdAcaoImediata]').val()

                };
            }

            getObjFormAcaoImediataArray.push(acoesNaoConformidadeFormAcaoImediataObj);
        });

        return getObjFormAcaoImediataArray;

    },

    //Todos
    sendFormCriarNaoConformidade: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            //var validate = APP.controller.NaoConformidadeController.validateForms();
            var validate = true;

            //validate = true;
            if (validate == true) {

                var statusEtapa = parseInt($('[name=StatusEtapa]').val());
                var IdRegistroConformidade = $('[name=IdRegistroConformidade]').val();
                var eProcedente = $('[name=formAcaoImadiataEProcedente]:checked').val();

                var naoConformidade = {};

                var fluxos = {
                    //Criar Nao Conformidade
                    _000: ['fluxo-00'],
                    //Ã‰ Procedente = false
                    _001: ['fluxo-01'],
                    //Ã‰ Procedente = true
                    _002: ['fluxo-02'],
                    //Status 2 - ImplementaÃ§Ã£o
                    _003: ['fluxo-03'],
                    //Status 3 - ReverificaÃ§Ã£o
                    _004: ['fluxo-04'],
                    //Status 3 - Desbloquear
                    _005: ['fluxo-05'],
                    //Anular
                    _006: ['fluxo-06']


                };

                switch (statusEtapa) {
                    case 0:
                        APP.controller.NaoConformidadeController.getUltimaDataEmissao('fluxo-00');
                        break;
                    case 1:
                        if (eProcedente == "false") {
                            naoConformidade = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj("fluxo-01");
                            APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-01");
                        } else {
                            naoConformidade = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj("fluxo-02");
                            APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-02");
                        }

                        break;
                    case 2:
                        naoConformidade = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj("fluxo-03");
                        APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-03");

                        break;
                    case 3:
                        naoConformidade = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj("fluxo-04");
                        APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-04");
                        break;
                    case 4:
                        naoConformidade = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj("fluxo-05");
                        APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-05");
                        break;
                    case 5:
                        naoConformidade = APP.controller.NaoConformidadeController.getCriarNaoConformidadeObj("fluxo-06");
                        APP.controller.NaoConformidadeController.saveFormCriarNaoConformidade(naoConformidade, "fluxo-06");
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
                    case "naoconformidade":
                        naoConformidadeObj = APP.controller.NaoConformidadeController.getObjFormCriarNaoConformidade(_fluxo);
                        break;
                    //case "acaoimediata":
                    //acaoimediataObj = APP.controller.NaoConformidadeController.getObjFormAcaoImediata();
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
                url = "/NaoConformidade/SalvarPrimeiraEtapa";
                break;
            case "fluxo-01":
                url = "/NaoConformidade/SalvarSegundaEtapa";
                break;
            case "fluxo-02":
                url = "/NaoConformidade/SalvarSegundaEtapa";
                break;
            case "fluxo-03":
                url = "/NaoConformidade/SalvarSegundaEtapa";
                break;
            case "fluxo-04":
                url = "/NaoConformidade/SalvarSegundaEtapa";
                break;
            case "fluxo-05":
                url = "/NaoConformidade/SalvarSegundaEtapa";
                break;
            case "fluxo-06":
                url = "/NaoConformidade/SalvarSegundaEtapa";
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
                        window.location.href = "/NaoConformidade/Index";
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

    HabilitaCamposNaoConformidade: function (perfil) {
        if (perfil == '4') {

            $('#main').find('input, textarea, button, select').removeAttr('disabled');
            $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
            $("#form-criar-nao-conformidade-processo").attr("disabled", true);
            $("#form-criar-nao-conformidade-emissor").attr("disabled", true);

        }
        else {

            $('#main').find('input, textarea, button, select').removeAttr('disabled');
            $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
            $("#form-criar-nao-conformidade-dt-emissao").attr("disabled", true);
        }
    },


    DestravaDocumento: function () {

        var idNaoConformidade = $('[name=IdRegistroConformidade]').val();  // $('#form-criar-nao-conformidade-nm-registro').val();
        var data = { "idNaoConformidade": idNaoConformidade };

        APP.controller.NaoConformidadeController.getResponsavelImplementarAcaoImediata();

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: "/NaoConformidade/DestravarDocumento",
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


    setDestravarCamposNaoConformidade: function () {

        this.buttonDestravar.on('click', function () {



            $('#form-criar-nao-conformidade-ds-registro').removeAttr('disabled');
            $('#form-criar-nao-conformidade-processo').removeAttr('disabled');
            $('#form-criar-nao-conformidade-responsavel').removeAttr('disabled');
            $('#form-acaoimediata').find('input, textarea, button, select').removeAttr('disabled');


            var idNaoConformidade = $('[name=IdRegistroConformidade]').val();  //$('#form-criar-nao-conformidade-nm-registro').val();
            var data = { "idNaoConformidade": idNaoConformidade };


            APP.controller.NaoConformidadeController.getResponsavelImplementarAcaoImediata();



        });
    }

};