

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
    var idTipoGestaoMelhoria = $("#ddlTipoNaoConformidade").val();
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
        parametros += '&tipoGestaoMelhoria=' + idTipoGestaoMelhoria;
        var openUrl = url + parametros;
        window.open(openUrl, '_blank');
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


APP.controller.GestaoMelhoriaController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexGestaoMelhoria") {
            this.indexGestaoMelhoria();
        }
        if (page == "CriarGestaoMelhoria") {
            this.acoesGestaoMelhoria();
		}
		//typeof (destavar) != "undefined" && 
		if (destravar === 'true') {

			
			this.setDestravarCamposGestaoMelhoria();
			this.HabilitaCamposGestaoMelhoria(perfil);
			this.formEditarGestaoMelhoria();
			this.formAcaoImediata();
			//this.setAcaoImediata();
			$('.botaouploadarquivos').removeAttr('disabled');

			var idSite = $('#nao-conformidade-site').val();
			processoSelecionado = $('[name=formCriarGestaoMelhoriaProcesso]').find(':selected').val();
			var idFuncao = 121; // Funcionalidade(Define aÃ§Ã£o)
			$.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + processoSelecionado + '&idSite=' + idSite + '&idFuncao=' + idFuncao, (result) => {
				if (result.StatusCode == 200) {
					//$('[name=formCriarGestaoMelhoriaResponsavel] option').not(':first-child').remove();
					APP.component.SelectListCompare.selectList(result.Lista, $('[name="formCriarGestaoMelhoriaResponsavel"] option'), $('[name="formCriarGestaoMelhoriaResponsavel"]'), 'IdUsuario', 'NmCompleto');
				}
			});
			//this.setResponsavelAnaliseDefinicaoAC();
			//var valor = $('[name=formCriarGestaoMelhoriaResponsavel]').val();
			//$('[name=formCriarGestaoMelhoriaProcesso]').trigger('change');
			//$('[name=formCriarGestaoMelhoriaResponsavel]').val(valor);

			//this.setResponsavelAnaliseDefinicaoAC();
			APP.controller.GestaoMelhoriaController.getResponsavelImplementarAcaoImediata();

        }

    },

    setup: function () {
        //Index Nao Conformidade
        this.buttonDelGestaoMelhoria = $(".del-nao-conformidade");

        //Criar Nao Conformidade
        this.buttonSalvar = $(".btn-salvar");

        //Editar Nao Conformidade
        this.buttonAddAcaoImediata = $('.add-acao-imediata');

        this.buttonDestravar = $(".btn-destravar");

        this.buttonImprimir = $("#btn-imprimir");

    },

    //Models
    models: {
        AnexoModel: APP.model.Anexo,
    },

    //Index Nao Conformidade
    indexGestaoMelhoria: function () {

        APP.component.DataTable.setDataTableParam('#tb-index-nao-conformidade', 0, "desc", "Tags");
        this.delGestaoMelhoria();

    },

    eventoAuditoria: function () {

        $('[name=formCriarGestaoMelhoriaEAuditoria]').on('change', function () {

            var EhAuditoria = $(this).val();

            if (EhAuditoria == "true") {
                var idCategoria = $('[name=formCriarGestaoMelhoriaTipo] option:contains("Auditoria")').val();

                $('[name=formCriarGestaoMelhoriaTipo]').val(idCategoria);
            }

        });

    },

    delGestaoMelhoria: function () {
        this.buttonDelGestaoMelhoria.on('click', function () {

            var msgIconeDeleteNC = $('[name=msgIconeDeleteNC]').val();

            bootbox.confirm(msgIconeDeleteNC, function (result) {
                if (result == true) {
                    APP.controller.GestaoMelhoriaController.setDelGestaoMelhoria();
                }
            });

        });

    },

    setDelGestaoMelhoria: function () {

        var idGestaoMelhoria = $('.del-nao-conformidade').data("id-nao-conformidade");
        var data = {
            "idGestaoMelhoria": idGestaoMelhoria,
            "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()
        };

        var erro = "";

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: "/GestaoMelhoria/RemoverComAcaoConformidade/",
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
    acoesGestaoMelhoria: function () {

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

        this.sendFormCriarGestaoMelhoria();

        this.setDestravarCamposGestaoMelhoria();

        //if (habilitaLoad == 'sim') {

        //    this.HabilitaCamposGestaoMelhoria(perfil);
        //}
    },

    eventoImprimir: function () {

        this.buttonImprimir.on('click', function () {

            var IdGestaoMelhoria = $(".IdNaoConformidade").val();
            APP.controller.GestaoMelhoriaController.imprimir(IdGestaoMelhoria);

        });

    },

    imprimir: function (IdGestaoMelhoria) {


        if (IdGestaoMelhoria != null) {

            APP.component.Loading.showLoading();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/GestaoMelhoria/PDF?id=' + IdGestaoMelhoria, true);
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
                this.formCriarGestaoMelhoria();
                break;
            case 1:
                this.setShowAndHideStatusEtapa1();
                this.formEditarGestaoMelhoria();
                this.formAcaoImediata();
                this.setEProcedenteInicializacao();
                break;
            case 2:
                this.setShowAndHideStatusEtapa2();
                this.formEditarGestaoMelhoria();
                this.formAcaoImediata();
                break;
            case 3:
                this.setShowAndHideStatusEtapa3(); 
                this.formEditarGestaoMelhoria();
                this.formAcaoImediata();
                break;
            case 4:
                this.setShowAndHideStatusEtapa4();
                break;
        }

    },

    setValidateForms: function () {

        var ObjFormCriarGestaoMelhoriaValidate = APP.controller.GestaoMelhoriaController.getObjObjFormCriarGestaoMelhoriaValidate();
        APP.component.ValidateForm.init(ObjFormCriarGestaoMelhoriaValidate, '#form-criar-naoconformidade');
        var ObjFormAcaoImediataValidate = APP.controller.GestaoMelhoriaController.getObjObjFormAcaoImediataValidate();
        APP.component.ValidateForm.init(ObjFormAcaoImediataValidate, '#form-acaoimediata');

    },

    //Interacao de Tela - StatusEtapa 0
    setShowAndHideStatusEtapa0: function () {

        $('#panel-acaoimediata').hide();
        //$('[name=formCriarGestaoMelhoriaNmRegistro]').closest('div').hide();

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
        $('[name=formCriarGestaoMelhoriaDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDsJustificativa]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaProcesso]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEmissor]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTags]').prop('disabled', _disabled);
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

    setHidePanelEProcedenteSim: function () {

        
        $('[name=formAcaoImadiataECorrecao]').closest('[class^=col]').hide();
        $('[name=formAcaoImadiataNecessitaAC]').closest('[class^=col]').hide();
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
    },

    setHideStatusEtapa2: function () {

        //Botoes Acoes
        $('#tb-acao-imediata tbody tr').each(function () {
            //$(this).find('td').last().hide();
            $('.botoesTd').hide();
        });
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();
        $('.add-acao-imediata').hide();

    },

    setDisabledStatusEtapa2: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarGestaoMelhoriaDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDsJustificativa]').prop('disabled', _disabled);

        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaProcesso]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEmissor]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTags]').prop('disabled', _disabled);

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
                $(this).find('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').attr('disabled', false);
                $(this).find('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').find('a').attr('disabled', false);
                $(this).find('[name=formCriarGestaoMelhoriaEvidencia]').prop('disabled', false);
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
        this.setCheckAcaoImediataNotOk();
        //[novo]
        this.setCheckHistorico();
        this.setIncluirComentario();
        this.setBackChecksOnPage();
        this.setShowInputsEtapa4

    },

    setHideStatusEtapa3: function () {

        //Botoes Acoes
        $('#tb-acao-imediata tbody tr').each(function () {
            //$(this).find('td').last().hide();
            $('.botoesTd').hide();

            //$(this).find('td')[6].hide();
        });
        $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').hide();
        $('.add-acao-imediata').hide();

    },

    setDisabledStatusEtapa3: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarGestaoMelhoriaDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDsJustificativa]').prop('disabled', _disabled);

        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaProcesso]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEmissor]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTags]').prop('disabled', _disabled);

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
        $('[name^=formCriarGestaoMelhoriaEvidencia]').closest('div').css('background-color', '#eee');
        $('[name=formAcaoImadiataTbEvidencia]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataECorrecao]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataNecessitaAC]').prop('disabled', _disabled);
        $('[name=formAcaoImadiataResponsavelReverificacao]').prop('disabled', _disabled);
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
                $(this).find('.btn-delete-acao-imediata').hide();
            });

            $('[name=formAcaoImadiataFoiEficaz]').closest('[class^=col]').show();

        }

    },

    setCheckAcaoImediataOk: function () {

        //$('.btn-confirm-acao-imediata').on('click', function () {
        //    

        //    $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
        //    $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);

        //    $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeIn(300);
        //    $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(true);
        //    APP.controller.GestaoMelhoriaController.getChecksAcaoImediata();
        //});



        $('.btn-confirm-acao-imediata').on('click', function () {

            $(this).closest('td').find('.btn-confirm-acao-imediata').delay(400).fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').delay(400).fadeOut(300);
            $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').delay(400).fadeOut(300);

            $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(true);
            APP.controller.GestaoMelhoriaController.getChecksAcaoImediata();
        });

    },

    setIncluirComentario: function () {
        $('#btn-new-comentario').on('click', function () {
            $('#modal-panel-form-cargos').modal("hide");
        });
        $('#modal-panel-form-cargos').on('hidden.bs.modal', function () {
            var motivo = $('[name=formNaoConformidadeComentarioMotivo]').val();
            var orientacao = $('[name=formNaoConformidadeComentarioOrientacao]').val();
            $('#' + $('#acaoImediataAtual').val()).closest('tr').find('[name=formAcaoImediataComentarioMotivo]').val(motivo);
            $('#' + $('#acaoImediataAtual').val()).closest('tr').find('[name=formAcaoImediataComentarioOrientacao]').val(orientacao);
            //alert($('#acaoImediataAtual').val());
        });
    },

    setCheckAcaoImediataNotOk: function () {

        $('.btn-denied-acao-imediata').on('click', function () {

            $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeOut(300);


            $(this).closest('td').find('[name=formAcaoImadiataTbAprovado]').val(false);
            APP.controller.GestaoMelhoriaController.getChecksAcaoImediata();
            //modal - panel - form - cargos
            var atual = $($(this).closest('tr')[0]).find('[name=formAcaoImadiataTbDescricao]').attr('id');
            $('#acaoImediataAtual').val(atual);
            $('[name=formGestaoMelhoriaComentarioMotivo').val("");
            $('[name=formGestaoMelhoriaComentarioOrientacao').val("");


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
            //APP.controller.GestaoMelhoriaController.getChecksAcaoImediata();
            //var atual = $($(this).closest('tr')[0]).find('[name=formAcaoImadiataTbDescricao]').attr('id');
            //$('#acaoImediataAtual').val(atual);
            var data = {
                "idAcaoImediata": idAcaoImediata
            };


            $.ajax({
                type: "POST",
                dataType: 'json',
                data: data,
                url: "/GestaoMelhoria/ListarAcaoImediataComentarios",
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
                            html += '            <textarea rows="4" name="formGestaoMelhoriaComentarioMotivo" id="form-naoconformidade-comentario-motivo" class="form-control" value="" disabled>' + this.Motivo + '</textarea>';
                            html += '        </div>';
                            html += '    </div>';
                            html += '    <div class="col-md-12 ">';
                            html += '        <div class="form-group">';
                            html += '            <label for="form-naoconformidade-comentario-orientacao">Orientação</label>';
                            html += '            <textarea rows="4" name="formGestaoMelhoriaComentarioOrientacao" id="form-naoconformidade-comentario-orientacao" class="form-control" value="" disabled>' + this.Orientacao + '</textarea>';
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
        } else {
            $('[name=formAcaoImadiataFoiEficaz').filter('[value=false]').prop('checked', true);
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
            //APP.controller.GestaoMelhoriaController.getChecksAcaoImediata();

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

    },

    setTriggerRadioButtonsEtapa4: function (_change) {

        $('[name=formAcaoImadiataEProcedente]').trigger(_change);
        $('[name=formAcaoImadiataECorrecao]').trigger(_change);
        $('[name=formAcaoImadiataNecessitaAC]').trigger(_change);

    },

    setDisabledStatusEtapa4: function (_disabled) {

        //Formulario Nao Conformidade
        $('[name=formCriarGestaoMelhoriaDsRegistro]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDsJustificativa]').prop('disabled', _disabled);

        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').closest('div').find('a').attr('disabled', 'disabled');
        $('[name=formCriarGestaoMelhoriaEvidencia]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaDtEmissao]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaProcesso]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEmissor]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaEAuditoria]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTipo]').prop('disabled', _disabled);
        $('#add-tipo-nao-conformidade').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaResponsavel]').prop('disabled', _disabled);
        $('[name=formCriarGestaoMelhoriaTags]').prop('disabled', _disabled);

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
        $('[name^=formCriarGestaoMelhoriaEvidencia]').closest('div').css('background-color', '#eee');
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

            APP.controller.GestaoMelhoriaController.setShowPanelEProcedenteSim();

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
            APP.controller.GestaoMelhoriaController.setHidePanelEProcedenteSim();
            APP.controller.GestaoMelhoriaController.setValidateEProcedente(_EProcedente);
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
        //APP.controller.GestaoMelhoriaController.setRulesNecessitaAC();

    },

    //Changes
    setEProcedente: function () {

        $('[name=formAcaoImadiataEProcedente]').on('change', function () {

            //Verifica se possui AC na tabela - Sim: libera panel AC - Nao: trava panel AC
            var checkTbAcaoCorretiva = $('#tb-acao-imediata tbody tr').size();

            var EProcedente = APP.controller.GestaoMelhoriaController.getEProcedente('formAcaoImadiataEProcedente');
            APP.controller.GestaoMelhoriaController.setRulesEProcedente(EProcedente, checkTbAcaoCorretiva);

        });

    },

    setEProcedenteInicializacao: function () {

        var checkTbAcaoCorretiva = $('#tb-acao-imediata tbody tr').size();

        var EProcedente = APP.controller.GestaoMelhoriaController.getEProcedente('formAcaoImadiataEProcedente');
        APP.controller.GestaoMelhoriaController.setRulesEProcedente(EProcedente, checkTbAcaoCorretiva);

    },

    setECorrecao: function () {

        $('[name=formAcaoImadiataECorrecao]').on('change', function () {

            var ECorrecao = APP.controller.GestaoMelhoriaController.getECorrecao('formAcaoImadiataECorrecao');
            APP.controller.GestaoMelhoriaController.setRulesECorrecao(ECorrecao);

        });

    },

    setNecessitaAC: function () {

        $('[name=formAcaoImadiataNecessitaAC]').on('change', function () {

            var NecessitaAC = APP.controller.GestaoMelhoriaController.getNecessitaAC('formAcaoImadiataNecessitaAC');
            APP.controller.GestaoMelhoriaController.setRulesNecessitaAC(NecessitaAC);

        });

    },

    //Auxiliares
    getEAuditoria: function () {

        var EAuditoria = APP.component.Radio.init('formCriarGestaoMelhoriaEAuditoria');
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
    formEditarGestaoMelhoria: function () {

        this.setAndHideEditarGestaoMelhoria();
        //this.getformCriarGestaoMelhoriaDtEmissao();
        this.getProcessosPorSite();
        this.getEmissorPorSite();
        this.getTipoGestaoMelhoria();
        this.setAddTipoGestaoMelhoria();

        this.setResponsavelAnaliseDefinicaoAC();

    },

    //Formulario Criar Nao Conformidade
    formCriarGestaoMelhoria: function () {

        this.setAndHideCriarGestaoMelhoria();
        this.getformCriarGestaoMelhoriaDtEmissao();
        this.getProcessosPorSite();
        this.getEmissorPorSite();
        this.getTipoGestaoMelhoria();
        this.setAddTipoGestaoMelhoria();

        this.setResponsavelAnaliseDefinicaoAC();

    },

    setAndHideEditarGestaoMelhoria: function () {
        var idPerfil = $('[name=IdPerfil]').val();
        if (idPerfil == 4)
            this.buttonDestravar.hide();
        else
            this.buttonDestravar.show();
    },

    setAndHideCriarGestaoMelhoria: function () {
        $('#numeroRegistro').hide();
    },

    getformCriarGestaoMelhoriaDtEmissao: function () {

        $('[name=formCriarGestaoMelhoriaDtEmissao]').val(APP.component.Datatoday.init());

    },

    getProcessosPorSite: function () {

        var idSite = $('#nao-conformidade-site').val();
        $.get('/Processo/ListaProcessosPorSite?idSite=' + idSite, function (result) {
            $.each(result.Lista, (key, val) => {
                var $option = $('<option></option>');
                $('[name=formCriarGestaoMelhoriaProcesso]').append(
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
                APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarGestaoMelhoriaEmissor] option'), $('[name=formCriarGestaoMelhoriaEmissor]'), 'IdUsuario', 'NmCompleto');
            }
        });

    },

    getTipoGestaoMelhoria: function () {

        var idSite = $('#nao-conformidade-site').val();
        $.get('/ControladorCategorias/ListaAtivos?tipo=tgm&site=' + idSite, function (result) {
            $.each(result.Lista, (key, val) => {
                if (result.StatusCode == 202) {
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarGestaoMelhoriaTipo] option'), $('[name=formCriarGestaoMelhoriaTipo]'), 'IdControladorCategorias', 'Descricao');
                }
            });
        });

    },

    setAddTipoGestaoMelhoria: function () {

        $('.add-tipo-nao-conformidade').on('click', function () {

            var idSite = $('#nao-conformidade-site').val();
            APP.controller.ControladorCategoriasController.init(idSite, 'tgm', APP.controller.GestaoMelhoriaController.getTipoGestaoMelhoria, ".add-tipo-nao-conformidade");

        });

    },

    setResponsavelAnaliseDefinicaoAC: function () {

        $('[name=formCriarGestaoMelhoriaProcesso]').on('change', function () {
            var idSite = $('#nao-conformidade-site').val();
            processoSelecionado = $(this).find(':selected').val();
            var idFuncao = 121; // Funcionalidade(Define aÃ§Ã£o)
            $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + processoSelecionado + '&idSite=' + idSite + '&idFuncao=' + idFuncao, (result) => {
                if (result.StatusCode == 200) {
                    $('[name=formCriarGestaoMelhoriaResponsavel] option').not(':first-child').remove();
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name="formCriarGestaoMelhoriaResponsavel"] option'), $('[name="formCriarGestaoMelhoriaResponsavel"]'), 'IdUsuario', 'NmCompleto');
                }
            });

        });

    },

    getUltimaDataEmissao: function (_fluxo) {

        var dtEmissao = $('[name=formCriarGestaoMelhoriaDtEmissao]').val();
        var idSite = $('#nao-conformidade-site').val();

        $.ajax({
            url: "/GestaoMelhoria/ObtemUltimaDataEmissao/",
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
                                var naoConformidadeObj = APP.controller.GestaoMelhoriaController.getCriarGestaoMelhoriaObj(_fluxo);
                                APP.controller.GestaoMelhoriaController.saveFormCriarGestaoMelhoria(naoConformidadeObj, _fluxo);
                            }
                        });
                    } else {
                        var naoConformidadeObj = APP.controller.GestaoMelhoriaController.getCriarGestaoMelhoriaObj(_fluxo);
                        APP.controller.GestaoMelhoriaController.saveFormCriarGestaoMelhoria(naoConformidadeObj, _fluxo);
                    }
                }
            }
        });

    },

    getObjObjFormCriarGestaoMelhoriaValidate: function () {

        var acoesGestaoMelhoriaoFormCriarGestaoMelhoriaObj = {
            //formCriarGestaoMelhoriaNmRegistro: 'required',
            formCriarGestaoMelhoriaDsRegistro: {
                'required': true,
                'minlength': 4
            },
            formCriarGestaoMelhoriaDtEmissao: { 'required': true },
            formCriarGestaoMelhoriaProcesso: { 'required': true },
            formCriarGestaoMelhoriaEmissor: 'required',
            formCriarGestaoMelhoriaEAuditoria: 'required',
            formCriarGestaoMelhoriaTipo: 'required',
            formCriarGestaoMelhoriaResponsavel: 'required',
            //formCriarGestaoMelhoriaTags: 'required',

        };

        return acoesGestaoMelhoriaoFormCriarGestaoMelhoriaObj;

    },

    getObjFormCriarGestaoMelhoria: function (_fluxo) {

        var acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj = {};
        switch (_fluxo) {
            case "fluxo-00":
                //Obj enviado no fluxo de criacao
                acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj = {
                    IdSite: $('[name=IdSite]').val(),
                    StatusEtapa: parseInt($('[name=StatusEtapa]').val()),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarGestaoMelhoriaDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoMelhoriaController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.GestaoMelhoriaController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarGestaoMelhoriaDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarGestaoMelhoriaProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarGestaoMelhoriaEmissor] :selected').val(),
                    EGestaoMelhoriaAuditoria: APP.component.Radio.init('formCriarGestaoMelhoriaEAuditoria'),
                    IdTipoNaoConformidade: $('[name=formCriarGestaoMelhoriaTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarGestaoMelhoriaResponsavel] :selected').val(),
                    Tags: $('[name=formCriarGestaoMelhoriaTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };

                break;

            case "fluxo-01":
                //Obj enviado no fluxo 01 de edicao
                acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj = {
                    StatusEtapa: 1,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    EProcedente: APP.component.Radio.init('formAcaoImadiataEProcedente'),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarGestaoMelhoriaDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoMelhoriaController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.GestaoMelhoriaController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarGestaoMelhoriaDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarGestaoMelhoriaProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarGestaoMelhoriaEmissor] :selected').val(),
                    EGestaoMelhoriaAuditoria: APP.component.Radio.init('formCriarGestaoMelhoriaEAuditoria'),
                    IdTipoNaoConformidade: $('[name=formCriarGestaoMelhoriaTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarGestaoMelhoriaResponsavel] :selected').val(),
                    Tags: $('[name=formCriarGestaoMelhoriaTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };

                break;

            case "fluxo-02":
                //Obj enviado no fluxo 02 de edicao
                acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj = {
                    StatusEtapa: 2,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    EProcedente: APP.component.Radio.init('formAcaoImadiataEProcedente'),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    AcoesImediatas: APP.controller.GestaoMelhoriaController.getObjFormAcaoImediata(),
                    ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
                    IdResponsavelPorIniciarTratativaAcaoCorretiva: $('[name=formAcaoImadiataResponsavelTratativa]').val(),
                    DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarGestaoMelhoriaDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoMelhoriaController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.GestaoMelhoriaController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarGestaoMelhoriaDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarGestaoMelhoriaProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarGestaoMelhoriaEmissor] :selected').val(),
                    EGestaoMelhoriaAuditoria: APP.component.Radio.init('formCriarGestaoMelhoriaEAuditoria'),
                    IdTipoNaoConformidade: $('[name=formCriarGestaoMelhoriaTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarGestaoMelhoriaResponsavel] :selected').val(),
                    Tags: $('[name=formCriarGestaoMelhoriaTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };

                break;
            case "fluxo-03":
                //Obj enviado no fluxo 03 de edicao
                acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj = {
                    StatusEtapa: 2,
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    EProcedente: APP.component.Radio.init('formAcaoImadiataEProcedente'),
                    AcoesImediatas: APP.controller.GestaoMelhoriaController.getObjFormAcaoImediata(),
                    ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
                    NumeroAcaoCorretiva: $('[name=formAcaoImadiataNumeroAC]').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarGestaoMelhoriaDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoMelhoriaController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.GestaoMelhoriaController.getAnexosArquivosNaoConformidadeAnexos(),
                    DtEmissao: $('[name=formCriarGestaoMelhoriaDtEmissao]').val(),
                    IdProcesso: $('[name=formCriarGestaoMelhoriaProcesso] :selected').val(),
                    IdEmissor: $('[name=formCriarGestaoMelhoriaEmissor] :selected').val(),
                    EGestaoMelhoriaAuditoria: APP.component.Radio.init('formCriarGestaoMelhoriaEAuditoria'),
                    Observacao: $('[name=formAcaoImadiataTbObservacao]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    IdTipoNaoConformidade: $('[name=formCriarGestaoMelhoriaTipo] :selected').val(),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarGestaoMelhoriaResponsavel] :selected').val(),
                    IdResponsavelPorIniciarTratativaAcaoCorretiva: $('[name=formAcaoImadiataResponsavelTratativa]').val(),
                    DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    Tags: $('[name=formCriarGestaoMelhoriaTags]').val(),
                    Causa: $('[name=formCausa]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val()
                };
                break;
			case "fluxo-04":
				//Obj enviado no fluxo 04 de edicao
				acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj = {
					StatusEtapa: $('[name=StatusEtapa]').val(),
					IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
					AcoesImediatas: APP.controller.GestaoMelhoriaController.getObjFormAcaoImediata(),
					ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
					DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
					FlEficaz: APP.controller.GestaoMelhoriaController.getFoiEficaz(),
					Tags: $('[name=formCriarGestaoMelhoriaTags]').val(),
					IdEmissor: $('[name=formCriarGestaoMelhoriaEmissor] :selected').val(),
					IdProcesso: $('[name=formCriarGestaoMelhoriaProcesso] :selected').val(),
					IdTipoNaoConformidade: $('[name=formCriarGestaoMelhoriaTipo] :selected').val(),
					DtEmissao: $('[name=formCriarGestaoMelhoriaDtEmissao]').val(),
					EGestaoMelhoriaAuditoria: APP.component.Radio.init('formCriarGestaoMelhoriaEAuditoria'),
					NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
					IdResponsavelInicarAcaoImediata: $('[name=formCriarGestaoMelhoriaResponsavel] :selected').val(),
					CriticidadeGestaoDeRisco: $('[name=formCriarGestaoMelhoriaCriticidade] :selected').val(),
					DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
					DescricaoRegistro: $('[name=formCriarGestaoMelhoriaDsRegistro]').val(),
					DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
					IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
					IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
					DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
					Observacao: $('[name=formAcaoImadiataTbObservacao]').val(),
					DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
					DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
					EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
					ArquivosDeEvidenciaAux: APP.controller.GestaoMelhoriaController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.GestaoMelhoriaController.getAnexosArquivosNaoConformidadeAnexos(),
					Causa: $('[name=formCausa]').val(),
					DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),
				};
				break;
            case "fluxo-05":
                
                acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj = {
                    StatusEtapa: $('[name=StatusEtapa]').val(),
                    DtDescricaoAcao: $('[name=formAcaoImadiataDtDescricaoAcao]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    AcoesImediatas: APP.controller.GestaoMelhoriaController.getObjFormAcaoImediata(),
                    FlEficaz: APP.controller.GestaoMelhoriaController.getFoiEficaz(),
                    Tags: $('[name=formCriarGestaoMelhoriaTags]').val(),
                    IdEmissor: $('[name=formCriarGestaoMelhoriaEmissor] :selected').val(),
                    IdProcesso: $('[name=formCriarGestaoMelhoriaProcesso] :selected').val(),
                    IdTipoNaoConformidade: $('[name=formCriarGestaoMelhoriaTipo] :selected').val(),
                    DtEmissao: $('[name=formCriarGestaoMelhoriaDtEmissao]').val(),
                    EGestaoMelhoriaAuditoria: APP.component.Radio.init('formCriarGestaoMelhoriaEAuditoria'),
                    NecessitaAcaoCorretiva: APP.component.Radio.init('formAcaoImadiataNecessitaAC'),
                    IdResponsavelInicarAcaoImediata: $('[name=formCriarGestaoMelhoriaResponsavel] :selected').val(),
                    CriticidadeGestaoDeRisco: $('[name=formCriarGestaoMelhoriaCriticidade] :selected').val(),
                    DescricaoAcao: $('[name=formAcaoImadiataJustificativa]').val(),
                    DescricaoRegistro: $('[name=formCriarGestaoMelhoriaDsRegistro]').val(),
                    DsJustificativa: $('[name=formAcaoImadiataJustificativa]').val(),
                    IdResponsavelReverificador: $('[name=formAcaoImadiataResponsavelReverificacao]').val(),
                    IdResponsavelImplementar: $('[name=formAcaoImadiataTbResponsavelImplementar]').val(),
                    DtEfetivaImplementacao: $('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    Observacao: $('[name=formAcaoImadiataTbObservacao]').val(),
                    DtPrazoImplementacao: $('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    DsAcao: $('[name=formAcaoImadiataTbDescricao]').val(),
                    EProcedente: $('[name=formAcaoImadiataEProcedente]:checked').val(),
                    ArquivosDeEvidenciaAux: APP.controller.GestaoMelhoriaController.getAnexosEvidencias(),
                    ArquivosNaoConformidadeAnexos: APP.controller.GestaoMelhoriaController.getAnexosArquivosNaoConformidadeAnexos(),
                    Causa: $('[name=formCausa]').val(),
					DescricaoAnaliseCausa: $('[name=formAcaoImadiataAnaliseCausa]').val(),

					ECorrecao: APP.component.Radio.init('formAcaoImadiataECorrecao'),
					
					NumeroAcaoCorretiva: $('[name=formAcaoImadiataNumeroAC]').val(),
					IdResponsavelPorIniciarTratativaAcaoCorretiva: $('[name=formAcaoImadiataResponsavelTratativa]').val(),

                };
                break;
        }

        return acoesGestaoMelhoriaFormCriarGestaoMelhoriaObj;

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
        this.getformCriarGestaoMelhoriaDtDescricaoAcao();
        this.setAcaoImediata();
        this.delAcaoImediata();
        this.getResponsavelReverificarAcaoImediata();
        this.getResponsavelTratativaAcaoImediata();

    },

    setAndHideAcaoImediata: function () {

        //

    },

    getformCriarGestaoMelhoriaDtDescricaoAcao: function () {
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
			$('.add-acao-imediata').removeClass('show').addClass('hide');
			APP.controller.GestaoMelhoriaController.bind();

			
			//if ($('[name=StatusEtapa]').val() == 1) {
			//	APP.controller.GestaoMelhoriaController.bind();
			//} else {
			//	APP.controller.GestaoMelhoriaController.bindAcao();
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
            $(this).closest('tr').remove();
        });

    },

    getResponsavelImplementarAcaoImediata: function () {
        var idSite = $('#nao-conformidade-site').val();
        var idFuncao = 122; // Funcionalidade(Implementar aÃ§Ã£o) que permite usuario Implementar aÃ§Ã£o NC
        var idProcesso = $('[name=IdProcesso]').val();
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: '/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=idProcesso&idSite=' + idSite + '&idFuncao=' + idFuncao,
            beforeSend: function () {
                $('.add-acao-imediata').removeClass('show').addClass('hide');
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
        var idFuncao = 123; // Funcionalidade(ReverificaÃ§Ã£o) que permite usuario Reverifique NC
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
        var idFuncao = 120; // Funcionalidade(AnÃ¡lise da Causa) que permite usuario Criar nova AC
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
        APP.controller.GestaoMelhoriaController.setup();
        APP.controller.GestaoMelhoriaController.setAcaoImediata();
        APP.controller.GestaoMelhoriaController.getResponsavelImplementarAcaoImediata();
        APP.component.FileUpload.init();
        APP.controller.GestaoMelhoriaController.setShowPanelEProcedenteSim();
        APP.controller.GestaoMelhoriaController.setHideRowAcaoImediata();
        APP.controller.GestaoMelhoriaController.delAcaoImediata();

	},

	bindAcao: function () {

		APP.component.Datapicker.init();
		APP.controller.GestaoMelhoriaController.setup();
		APP.controller.GestaoMelhoriaController.setAcaoImediata();
		APP.controller.GestaoMelhoriaController.getResponsavelImplementarAcaoImediata();
		APP.component.FileUpload.init();
		APP.controller.GestaoMelhoriaController.setShowPanelEProcedenteSim();
		//APP.controller.GestaoMelhoriaController.setHideRowAcaoImediata();
		APP.controller.GestaoMelhoriaController.delAcaoImediata();

	},

    getObjObjFormAcaoImediataValidate: function () {

        var acoesGestaoMelhoriaoFormAcaoImediataObj = {

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

        return acoesGestaoMelhoriaoFormAcaoImediataObj;

    },

    getObjFormAcaoImediata: function () {

        var idUsuarioLogado = $('[name=UsuarioLogado]').val();
        var statusEtapa = parseInt($('[name=StatusEtapa]').val());

        var getObjFormAcaoImediataArray = [];
        var acoesGestaoMelhoriaFormAcaoImediataObj = {};
        var acoesImediatasComentarios = {};

        var anexoEvidenciaModel = APP.controller.ClienteController.models.AnexoModel;


        $('#tb-acao-imediata tbody tr').each(function (index, tr) {
            if (statusEtapa == 2) {
                acoesGestaoMelhoriaFormAcaoImediataObj = {
                    Aprovado: $(tr).find('[name=formAcaoImadiataTbAprovado]').val(),
                    Descricao: $(tr).find('[name=formAcaoImadiataTbDescricao]').val(),
                    Observacao: $(tr).find('[name=formAcaoImadiataTbObservacao]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=formAcaoImadiataTbDtPrazoImplementacao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=formAcaoImadiataTbResponsavelImplementar]').val(),

                    DtEfetivaImplementacao: $(tr).find('[name=formAcaoImadiataTbDtEfetivaImplementacao]').val(),
                    IdFilaEnvio: $(tr).find('[name=formAcaoImediataIdFilaEnvio]').val(),
                    ArquivoEvidenciaAux: anexoEvidenciaModel.constructor(
                        $(tr).find('[name^=formCriarGestaoMelhoriaEvidencia]').closest('td').find('[name^=formCriarNaoConformidadeEvidenciaIdAnexo]').val(),
                        $(tr).find('[name^=formCriarGestaoMelhoriaEvidencia]').closest('td').find('i').text(),
                        $(tr).find('[id^=form-acaoimediata-tb-evidencia-]').data('b64')
                    ),
                    SubmitArquivoEvidencia: APP.controller.GestaoMelhoriaController.getAnexosAcaoImediata($(tr).find(".IdentificadorInicialupload").data("identificador")),
                    Estado: $(tr).find('[name=formAcaoImadiataTbEstado]').val() != 0 ? 16 : 0,
                    IdAcaoImediata: $(tr).find('[name=formAcaoImadiataTbIdAcaoImediata]').val(),
                };
            } else if (statusEtapa == 3) {

				acoesGestaoMelhoriaFormAcaoImediataObj = {
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

				acoesGestaoMelhoriaFormAcaoImediataObj = {


					

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
                        $(tr).find('[name^=formCriarGestaoMelhoriaEvidencia]').closest('td').find('[name^=formCriarNaoConformidadeEvidenciaIdAnexo]').val(),
                        $(tr).find('[name^=formCriarGestaoMelhoriaEvidencia]').closest('td').find('i').text(),
                        $(tr).find('[id^=form-acaoimediata-tb-evidencia-]').data('b64')
                    ),
                    SubmitArquivoEvidencia: APP.controller.GestaoMelhoriaController.getAnexosAcaoImediata($(tr).find(".IdentificadorInicialupload").data("identificador")),


					

					
					
                    //ComentarioMotivo: $(tr).find('[name=formAcaoImediataComentarioMotivo]').val(),
                    //ComentarioOrientacao: $(tr).find('[nameformAcaoImediataComentarioOrientacaoformAcaoImadiataTbIdAcaoImediata]').val()

                };
            }

            getObjFormAcaoImediataArray.push(acoesGestaoMelhoriaFormAcaoImediataObj);
        });

        return getObjFormAcaoImediataArray;

    },

    //Todos
    sendFormCriarGestaoMelhoria: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            //var validate = APP.controller.GestaoMelhoriaController.validateForms();
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
                    _005: ['fluxo-05']

                };

                switch (statusEtapa) {
                    case 0:
                        APP.controller.GestaoMelhoriaController.getUltimaDataEmissao('fluxo-00');
                        break;
                    case 1:
                        if (eProcedente == "false") {
                            naoConformidade = APP.controller.GestaoMelhoriaController.getCriarGestaoMelhoriaObj("fluxo-01");
                            APP.controller.GestaoMelhoriaController.saveFormCriarGestaoMelhoria(naoConformidade, "fluxo-01");
                        } else {
                            naoConformidade = APP.controller.GestaoMelhoriaController.getCriarGestaoMelhoriaObj("fluxo-02");
                            APP.controller.GestaoMelhoriaController.saveFormCriarGestaoMelhoria(naoConformidade, "fluxo-02");
                        }

                        break;
                    case 2:
                        naoConformidade = APP.controller.GestaoMelhoriaController.getCriarGestaoMelhoriaObj("fluxo-03");
                        APP.controller.GestaoMelhoriaController.saveFormCriarGestaoMelhoria(naoConformidade, "fluxo-03");

                        break;
                    case 3:
                        naoConformidade = APP.controller.GestaoMelhoriaController.getCriarGestaoMelhoriaObj("fluxo-04");
                        APP.controller.GestaoMelhoriaController.saveFormCriarGestaoMelhoria(naoConformidade, "fluxo-04");
                        break;
                    case 4:
                        naoConformidade = APP.controller.GestaoMelhoriaController.getCriarGestaoMelhoriaObj("fluxo-05");
                        APP.controller.GestaoMelhoriaController.saveFormCriarGestaoMelhoria(naoConformidade, "fluxo-05");
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

    getCriarGestaoMelhoriaObj: function (_fluxo) {

        var naoConformidadeObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "GestaoMelhoria":
                        naoConformidadeObj = APP.controller.GestaoMelhoriaController.getObjFormCriarGestaoMelhoria(_fluxo);
                        break;
                    //case "acaoimediata":
                    //acaoimediataObj = APP.controller.GestaoMelhoriaController.getObjFormAcaoImediata();
                    //break;
                }
            }

        });

        return naoConformidadeObj;

    },
        
    saveFormCriarGestaoMelhoria: function (registroMelhoriaObj, _fluxo) {
        var url = "";

        switch (_fluxo) {
            case "fluxo-00":
                url = "/GestaoMelhoria/SalvarPrimeiraEtapa";
                break;
            case "fluxo-01":
                url = "/GestaoMelhoria/SalvarSegundaEtapa";
                break;
            case "fluxo-02":
                url = "/GestaoMelhoria/SalvarSegundaEtapa";
                break;
            case "fluxo-03":
                url = "/GestaoMelhoria/SalvarSegundaEtapa";
                break;
            case "fluxo-04":
                url = "/GestaoMelhoria/SalvarSegundaEtapa";
                break;
            case "fluxo-05":
                url = "/GestaoMelhoria/SalvarSegundaEtapa";
                break;
        }

        $.ajax({
            type: "POST",
            data: registroMelhoriaObj,
            dataType: 'json',
            url: url,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/GestaoMelhoria/Index";
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

    HabilitaCamposGestaoMelhoria: function (perfil) {
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

        var idGestaoMelhoria = $('[name=IdRegistroConformidade]').val();  // $('#form-criar-nao-conformidade-nm-registro').val();
        var data = { "idGestaoMelhoria": idGestaoMelhoria };

        APP.controller.GestaoMelhoriaController.getResponsavelImplementarAcaoImediata();

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: "/GestaoMelhoria/DestravarDocumento",
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


    setDestravarCamposGestaoMelhoria: function () {

        this.buttonDestravar.on('click', function () {


            if (perfil == '4') {

                $('#main').find('input, textarea, button, select').removeAttr('disabled');
                $('.botaouploadarquivos').removeAttr('disabled');
                $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
                $("#form-criar-nao-conformidade-processo").attr("disabled", true);
                $("#form-criar-nao-conformidade-emissor").attr("disabled", true);
            }
            else {

                $('#main').find('input, textarea, button, select').removeAttr('disabled');
                $('.botaouploadarquivos').removeAttr('disabled');
                $("#form-criar-nao-conformidade-nm-registro").attr("disabled", true);
                $("#form-criar-nao-conformidade-dt-emissao").attr("disabled", true);

            }

            $("#form-acaoimediata-numero-ac").attr("disabled", true);


            var idGestaoMelhoria = $('[name=IdRegistroConformidade]').val();  //$('#form-criar-nao-conformidade-nm-registro').val();
            var data = { "idGestaoMelhoria": idGestaoMelhoria };


            APP.controller.GestaoMelhoriaController.getResponsavelImplementarAcaoImediata();

        });
    }

};