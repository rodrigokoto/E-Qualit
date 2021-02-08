
/*
|--------------------------------------------------------------------------
| Controlador Indicador
|--------------------------------------------------------------------------
*/
function allnumeric(inputtxt) {
    var numbers = /^[1-9]+$/;
    if (inputtxt.value.match(numbers)) {
        return true;
    }
    else {
        //    inputtxt.focus();
        return false;
    }
}
$('[name=formgestaoriscodtinclusao]').val(APP.component.Datatoday.init());

function SubstituiVirgulaPorPonto(campo) {

    
    //campo.value = campo.value.replace(/\./g, ',');
    //campo.value = campo.value.replace(/[A-Za-z]/gi, "");
}
function setPlanoDeVooDisabled() {

    var per = $('[name=formCriarIndicadorPeriodicidade]');
    var meses = $('[name^=formPlanoDeVooRealizado]');

    var medicao = parseInt($('[name=formCriarIndicadorPeriodicidadeMedicao] :selected').val());
    var $option = $('<option></option>');

    per.find('option').remove().end();
    per.prop('disabled', false);
    switch (medicao) {
        case 1:
            per.append('<option value="1">Mensal</option>');
            per.append('<option value="2">Bimestral</option>');
            per.append('<option value="3">Trimestral</option>');
            per.append('<option value="4">Semestral</option>');
            per.append('<option value="5">Anual</option>');
            break;
        case 2:
            per.append('<option value="2">Bimestral</option>');
            per.append('<option value="4">Semestral</option>');
            per.append('<option value="5">Anual</option>');
            break;
        case 3:
            per.append('<option value="3">Trimestral</option>');
            per.append('<option value="4">Semestral</option>');
            per.append('<option value="5">Anual</option>');
            break;
        case 4:
            per.append('<option value="4">Semestral</option>');
            per.append('<option value="5">Anual</option>');
            break;
        case 5:
            per.append('<option value="5">Anual</option>');
            break;
        default:
    }


}
function setDisabledMetaRealizado(meses, medicao) {
    var datMonth = new Date();
    var mes = datMonth.getMonth() + 1;


    if (!isNaN(medicao)) {
        switch (medicao) {
            case 1:
                meses.each(function (index, item) {
                    if ((index + 1) == mes) {
                        $(this).prop('disabled', true);
                    }
                    else {
                        $(this).prop('disabled', false);
                    }
                });
                break;
            case 2:
                meses.each(function (index, item) {
                    if ((index + 1) == mes) {
                        $(this).prop('disabled', true);
                    }
                    else {
                        if ((index + 1) % 2 === 0)
                            $(this).prop('disabled', false);
                        else {
                            $(this).val('');
                            $(this).prop('disabled', true);
                        }
                    }
                });
                break;
            case 3:
                meses.each(function (index, item) {
                    if ((index + 1) == mes) {
                        $(this).prop('disabled', true);
                    }
                    else {
                        if ((index + 1) % 3 === 0)
                            $(this).prop('disabled', false);
                        else {
                            $(this).val('');
                            $(this).prop('disabled', true);
                        }
                    }
                });
                break;
            case 4:
                meses.each(function (index, item) {
                    if ((index + 1) == mes) {
                        $(this).prop('disabled', true);
                    }
                    else {
                        if ((index + 1) % 6 === 0)
                            $(this).prop('disabled', false);
                        else {
                            $(this).val('');
                            $(this).prop('disabled', true);
                        }
                    }
                });
                break;
            case 5:
                meses.each(function (index, item) {
                    if ((index + 1) == mes) {
                        $(this).prop('disabled', true);
                    }
                    else {
                        if ((index + 1) % 12 === 0)
                            $(this).prop('disabled', false);
                        else {
                            $(this).val('');
                            $(this).prop('disabled', true);
                        }
                    }
                });
                break;
            default:
                meses.each(function (index, item) {
                    $(this).prop('disabled', true);
                });
                break;
        }
    }
}
function setIconPlusPlanoDeVooDisabled(Icon, periodo) {
    if (!isNaN(periodo)) {
        switch (periodo) {
            case 1:
                Icon.each(function (index, item) {
                    var data = new Date();
                    data = data.getMonth() + 1;

                    if ((index + 1) === data) {
                        $(this).hide();
                    }
                    else {
                        $(this).show();
                    }
                });
                break;
            case 2:
                Icon.each(function (index, item) {
                    if ((index + 1) % 2 === 0) {
                        var data = new Date();
                        data = data.getMonth() + 1;

                        if ((index + 1) === data) {
                            $(this).hide();
                        }
                        else {
                            $(this).show();
                        }
                    }
                    else {
                        $(this).hide();
                    }
                });
                break;
            case 3:
                Icon.each(function (index, item) {
                    if ((index + 1) % 3 === 0) {
                        var data = new Date();
                        data = data.getMonth() + 1;

                        if ((index + 1) === data) {
                            $(this).hide();
                        }
                        else {
                            $(this).show();
                        }
                    }
                    else {
                        $(this).hide();
                    }
                });
                break;
            case 4:
                Icon.each(function (index, item) {
                    if ((index + 1) % 6 === 0) {
                        var data = new Date();
                        data = data.getMonth() + 1;

                        if ((index + 1) === data) {
                            $(this).hide();
                        }
                        else {
                            $(this).show();
                        }
                    }
                    else {
                        $(this).hide();
                    }
                });
                break;
            case 5:
                Icon.each(function (index, item) {
                    if ((index + 1) % 12 === 0) {
                        var data = new Date();
                        data = data.getMonth() + 1;

                        if ((index + 1) === data) {
                            $(this).hide();
                        }
                        else {
                            $(this).show();
                        }
                    }
                    else {
                        $(this).hide();
                    }
                });
                break;
            default:
                Icon.each(function (index, item) {
                    $(this).show();
                });
                break;
        }
    }
}
function getAnaliseResultado(mes, idPlanoVoo) {
    var meses = $('[name^=formPlanoDeVoo]');
    var mesesRealizados = $('[name^=formPlanoDeVooRealizado]');

    var per = parseInt($('[name=formCriarIndicadorPeriodicidade] :selected').val());

    $('#panel-dashboard').removeClass('hidden');
    $('[name=getMonth]').val(mes);

    var media = 0;
    var mediaRealizada = 0;
    var counter = 0;
    var multiplicador = 0;
    meses.each(function (e) {
        if (per == 1) {
            if ((e + 1) == mes) {
                media = parseInt(this.value);
            }
        }
        if (per == 2) {
            if ((e + 1) >= (mes - 1) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    media = media + parseInt(this.value);
                }
            }
        }
        if (per == 3) {
            if ((e + 1) >= (mes - 2) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    media = media + parseInt(this.value);
                }
            }
        }
        if (per == 4) {
            if ((e + 1) >= (mes - 5) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    media = media + parseInt(this.value);
                }
            }
        }
        if (per == 5) {
            if ((e + 1) >= (mes - 11) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    media = media + parseInt(this.value);
                }
            }
        }



    });

    mesesRealizados.each(function (e) {
        if (per == 1) {
            if ((e + 1) == mes) {
                mediaRealizada = parseInt(this.value);
            }
        }
        if (per == 2) {
            if ((e + 1) >= (mes - 1) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    mediaRealizada = mediaRealizada + parseInt(this.value);
                }
            }
        }
        if (per == 3) {
            if ((e + 1) >= (mes - 2) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    mediaRealizada = mediaRealizada + parseInt(this.value);
                }
            }
        }
        if (per == 4) {
            if ((e + 1) >= (mes - 5) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    mediaRealizada = mediaRealizada + parseInt(this.value);
                }
            }
        }
        if (per == 5) {
            if ((e + 1) >= (mes - 11) && (e + 1) <= mes) {
                if (!isNaN(parseInt(this.value))) {
                    mediaRealizada = mediaRealizada + parseInt(this.value);
                }
            }
        }
    });

    switch (per) {
        case 1:
            multiplicador = 1;
            break;
        case 2:
            multiplicador = 2;
            break;
        case 3:
            multiplicador = 3;
            break;
        case 4:
            multiplicador = 6;
            break;
        case 5:
            multiplicador = 12;
            break;
        default:
            multiplicador = counter;
            break;
    }


    var total = media / multiplicador;
    var totalRealizado = mediaRealizada / multiplicador;

    var sentidoMeta = $('[name=formCriarIndicadorSentido]').val();


    if (per == 1) {
        $('[name=MediaAnaliseResultadoMeta]').val(total);
    } else {
        $('[name=MediaAnaliseResultadoMeta]').val(total);
    }

    if (per == 1) {
        $('[name=MediaAnaliseResultado]').val(totalRealizado);
    } else {
        $('[name=MediaAnaliseResultado]').val(totalRealizado);
    }

    var IdPeriodicidade = $('[name=IdPeriodicidade]').val();
    var url = '/Indicador/GerarPartialGestaoRisco?Periodicidade=' + IdPeriodicidade + '&mes=' + mes + '&idplanovoo=' + idPlanoVoo
    var grDiv = $('#content-gr');

    $.get(url, function (data) {
        grDiv.html(data);

        APP.controller.IndicadorController.getTemasDescricao();
        APP.controller.IndicadorController.getTemasCores();

        APP.controller.IndicadorController.getTemasPossuiGestaoDeRisco();
        APP.controller.IndicadorController.setPossuiGestaoDeRisco();

        $('[name^=formGestaoDeRiscoRisco]:checked').each(function () {
            var teste = this.name;
            APP.controller.IndicadorController.getPossuiGestaoDeRiscoInformar(this);
        });

        APP.controller.IndicadorController.setPossuiGestaoDeRiscoInformar();

        APP.controller.IndicadorController.setHidePossuiGestaoDeRisco();

        APP.controller.IndicadorController.setHidePossuiGestaoDeRiscoInformar();

        APP.controller.IndicadorController.getTodosResponsaveisPorAcaoImediata();

        if (sentidoMeta == 1) {
            if (total <= totalRealizado) {
                $('#media-analise-resultado-icon').removeClass('fa fa-times-circle');
                $('#media-analise-resultado-icon').addClass('fa fa-check-circle');
                $('#media-analise-resultado-icon').css('color', 'forestgreen');
                $('#form-gestao-de-risco-risco-nao').prop('checked', true).attr('checked', 'checked');
                $('.formGestaoDeRiscoRisco').trigger('change');
            }
            else {
                $('#media-analise-resultado-icon').removeClass('fa fa-check-circle');
                $('#media-analise-resultado-icon').addClass('fa fa-times-circle');
                $('#media-analise-resultado-icon').css('color', 'red');
                $('#form-gestao-de-risco-risco-sim').prop('checked', true).attr('checked', 'checked');
                $('.formGestaoDeRiscoRisco').trigger('change');
            }
        }
        else {
            if (total < totalRealizado) {
                $('#media-analise-resultado-icon').removeClass('fa fa-check-circle');
                $('#media-analise-resultado-icon').addClass('fa fa-times-circle');
                $('#media-analise-resultado-icon').css('color', 'red');
                $('#form-gestao-de-risco-risco-sim').prop('checked', true).attr('checked', 'checked');
                $('.formGestaoDeRiscoRisco').trigger('change');

            }
            else {
                $('#media-analise-resultado-icon').removeClass('fa fa-times-circle');
                $('#media-analise-resultado-icon').addClass('fa fa-check-circle');
                $('#media-analise-resultado-icon').css('color', 'forestgreen');
                $('#form-gestao-de-risco-risco-nao').attr('checked', 'checked');
                $('.formGestaoDeRiscoRisco').trigger('change');

            }
        }

    });
}
function AbrirRelatorio(idIndicador) {

    APP.controller.IndicadorController.jsonGlobal = [];
    APP.controller.IndicadorController.jsonGlobal = APP.controller.IndicadorController.getJSONRelatorios(idIndicador);


    APP.controller.IndicadorController.setPreencherQtdContainerPagina();
    APP.controller.IndicadorController.setRelatorioPlanoDeVoo();
}



APP.controller.IndicadorController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexIndicador") {
            this.indexIndicador();
        }
        if (page == "CriarIndicador") {
            this.acoesIndicador();
        }



    },

    setup: function () {

        //Index Indicador
        this.buttonDelIndicadore = $(".del-indicador");

        //Criar Indicador
        this.buttonSalvar = $(".btn-salvar");

        //Relatorios
        this.buttonGetRelatorioBarras = $('#relatorioBarras');
        this.buttonGetRelatorioColunas = $('#relatorioColunas');
        this.buttonDestravar = $("#btn-destravar");
        this.buttonImprimir = $(".btn-imprimir");

    },

    //Models
    models: {

        //PerfilUsuarioModel: APP.model.PerfilUsuario,

    },

    //Index Padrao
    indexIndicador: function () {

        APP.component.DataTable.init('#tb-index-indicador');
        this.delIndicador();
    },

    delIndicador: function () {
        this.buttonDelIndicadore.on('click', function () {

            var msgIconeDeleteIndicador = $('[name=msgIconeDeleteIndicador]').val();
            var id = $(this).data("id-indicador");
            bootbox.confirm(msgIconeDeleteIndicador, function (result) {
                if (result == true) {
                    APP.controller.IndicadorController.setDelIndicador(id);
                }
            });

        });

    },

    setDelIndicador: function (idIndicador) {
        var data = {
            "Id": idIndicador,
            "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()
        };

        var erro = "";

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: "/Indicador/Excluir",
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

    //Acoes Padrao
    acoesIndicador: function () {



        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();

        this.setAndHide();

        this.setValidateForms();
        this.formCriarIndicador();
        this.sendFormCriarIndicador();
        this.eventoImprimir();


        if ($('[name=IdIndicador]').val() > 0) {
            this.formEditarIndicador();
        }


        this.setDestravarCamposIndicador();

        if (habilitaLoad == 'Sim') {

            this.HabilitaCamposIndicador(perfil);
        }

        //$('[name^=formPlanoDeVooMeta]').each(function (i) {

        //    var valor = $(this).val();

        //    if (valor === "0") {
        //        $(this).val('');
        //    }


        //});


    },

    setAndHide: function () {

        $('#panel-dashboard').hide();

    },

    setValidateForms: function () {

        var ObjFormCriarIndicadorValidate = APP.controller.IndicadorController.getObjObjFormCriarIndicadorValidate();
        APP.component.ValidateForm.init(ObjFormCriarIndicadorValidate, '#form-criar-indicador');
        APP.component.ValidateForm.init(ObjFormCriarIndicadorValidate, '#form-planodevoo');

    },

    //Formulario Criar Indicador
    formCriarIndicador: function () {

        this.setAndHideCriarIndicador();
        this.setDisableCriarIndicador();
        this.getProcessosPorSite();
        this.getResponsavel();
        this.getTotalPlanoDeVoo();
        this.setTotalPlanoDeVoo();
        this.setPeriodicidadeAnalise();
        this.eventoImprimir();

        var idIndicador = $('[name=IdIndicador]').val();

        if (idIndicador > 0) {
            var per = parseInt($('[name=formCriarIndicadorPeriodicidade] :selected').val());

            var months = $('[name^=formPlanoDeVooRealizado]');
            var icons = $('[name^=formGraphPlanoDeVooRealizado]');

            //setPlanoDeVooDisabled(months, per);
            setIconPlusPlanoDeVooDisabled(icons, per);

            this.setDestravarCamposIndicador();
        }
    },

    setAndHideCriarIndicador: function () {

        $('#tb-plano-de-voo tbody tr:nth-child(2)').hide();

    },

    setDisableCriarIndicador: function () {

        $('[name=formPlanoDeVooTotal]').prop('disabled', true);

    },

    getProcessosPorSite: function () {

        var idSite = $('[name=IdSite]').val();
        $.get('/Processo/ListaProcessosPorSite?idSite=' + idSite, function (result) {
            $.each(result.Lista, (key, val) => {
                var $option = $('<option></option>');
                $('[name=formCriarIndicadorProcesso]').append(
                    $option.val(val.IdProcesso).text(val.Nome)
                );
            });
        });
    },

    getResponsavel: function () {

    
        var idProcesso = $('[name=IdProcesso]').val();
        var idSite = $('[name=IdSite]').val();
        var idFuncao = 27; // Funcionalidade(Cadastrar) que permite usuario criar nc
        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcessoIndicadores?idProcesso=' + idProcesso + '&idSite=' + idSite + '&idFuncao=' + idFuncao + '}', (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarIndicadorResponsavel] option'), $('[name=formCriarIndicadorResponsavel]'), 'IdUsuario', 'NmCompleto');
            }
        });

    },

    getObjObjFormCriarIndicadorValidate: function () {

        var formCriarIndicadorObj = {
            formCriarIndicadorProcesso: { required: true },
            formCriarIndicadorResponsavel: { required: true },
            formCriarIndicadorPeriodicidade: { required: true },
            formCriarIndicadorSentido: { required: true },
            formCriarIndicadorAno: { required: true },
            formCriarIndicadorMeta: { required: true },
            formCriarIndicadorUnidade: { required: true },
            formCriarIndicadorObjetivo: { required: true },
            formCriarIndicadorIndicador: { required: true },

            //Tabela
            formPlanoDeVooMetaJan: { required: true },
            formPlanoDeVooMetaFev: { required: true },
            formPlanoDeVooMetaMar: { required: true },
            formPlanoDeVooMetaAbr: { required: true },
            formPlanoDeVooMetaMai: { required: true },
            formPlanoDeVooMetaJun: { required: true },
            formPlanoDeVooMetaJul: { required: true },
            formPlanoDeVooMetaAgo: { required: true },
            formPlanoDeVooMetaSet: { required: true },
            formPlanoDeVooMetaOut: { required: true },
            formPlanoDeVooMetaNov: { required: true },
            formPlanoDeVooMetaDez: { required: true },

            // formPlanoDeVooRealizadoJan: {required: true},
            // formPlanoDeVooRealizadoFev: {required: true},
            // formPlanoDeVooRealizadoMar: {required: true},
            // formPlanoDeVooRealizadoAbr: {required: true},
            // formPlanoDeVooRealizadoMai: {required: true},
            // formPlanoDeVooRealizadoJun: {required: true},
            // formPlanoDeVooRealizadoJul: {required: true},
            // formPlanoDeVooRealizadoAgo: {required: true},
            // formPlanoDeVooRealizadoSet: {required: true},
            // formPlanoDeVooRealizadoOut: {required: true},
            // formPlanoDeVooRealizadoNov: {required: true},
            // formPlanoDeVooRealizadoDez: {required: true},
        };

        return formCriarIndicadorObj;

    },

    getObjFormCriarIndicador: function () {

        var formCriarIndicadorObj = {
            IdSite: $('[name=IdSite]').val(),
            IdProcesso: $('[name=formCriarIndicadorProcesso] :selected').val(),
            Id: $('[name=IdIndicador]').val(),
            IdResponsavel: $('[name=formCriarIndicadorResponsavel] :selected').val(),
            Periodicidade: $('[name=formCriarIndicadorPeriodicidade] :selected').val(),
            PeriodicidadeMedicao: $('[name=formCriarIndicadorPeriodicidadeMedicao] :selected').val(),
            Direcao: $('[name=formCriarIndicadorSentido] :selected').val(),
            Ano: $('[name=formCriarIndicadorAno]').val(),
            MetaAnual: $('[name=formCriarIndicadorMeta]').val(),
            Unidade: $('[name=formCriarIndicadorUnidade]').val(),
            Objetivo: $('[name=formCriarIndicadorObjetivo]').val(),
            Descricao: $('[name=formCriarIndicadorIndicador]').val(),
            Medida: $('[name^=formCriarIndicadorTpMedida] :selected').val(),

            PeriodicidadeDeAnalises: [{
                Id: $('[name=IdPeriodicidade]').val(),
                PlanoDeVoo: APP.controller.IndicadorController.getObjFormMetaArray(),
                MetasRealizadas: APP.controller.IndicadorController.getObjFormPlanoDeVooArray(),
            }],

        };

        return formCriarIndicadorObj;

    },

    getObjFormMetaArray: function () {

        var arrayFormMetaObj = [];
        var formMetaObj = {};

        $('[name^=formPlanoDeVooMeta]').each(function (i) {

            formMetaObj = {
                DataReferencia: APP.controller.IndicadorController.getDataProMes(i),
                Valor: $(this).val(),
            };
            arrayFormMetaObj.push(formMetaObj);

        });

        return arrayFormMetaObj;

    },

    getObjFormPlanoDeVooArray: function () {

        var arrayFormPlanoDeVooObj = [];
        var formPlanoDeVooObj = {};

        var teste = $('[name^=formPlanoDeVooRealizado]');

        $('[name^=formPlanoDeVooRealizado]').each(function (i) {
            var idCkEditor = $('[name=formGestaoDeRiscoDescricao]').attr('id');
            var dataref = APP.controller.IndicadorController.getDataProMes(i);

            var d = new Date(dataref);

            var dYear = d.getFullYear();

            var dtAtual = new Date();
            var ano = dtAtual.getFullYear()

            if (dYear == (ano - 1)) {
                ano = (ano - 1);
            }

            var mes = $('input[name=getMonth]').val();
            var mes = '01/' + mes + '/' + ano;


            if (dataref == mes) {
                if (idCkEditor == undefined) {
                    formPlanoDeVooObj = {
                        Id: $(this).closest('div').find('[name^=IdPlanoDeVoo]').val(),
                        DataReferencia: dataref,
                        Realizado: $(this).val() != '' ? $(this).val() : null,
                    };
                    arrayFormPlanoDeVooObj.push(formPlanoDeVooObj);
                }
                else {
                    formPlanoDeVooObj = {
                        Id: $(this).closest('div').find('[name^=IdPlanoDeVoo]').val(),
                        DataReferencia: dataref,
                        Analise: CKEDITOR.instances[idCkEditor].getData(),
                        CorRisco: $("[name=formGestaoDeRiscoCriticidade] :selected").val(),
                        IdProcesso: $('[name=formCriarIndicadorProcesso] :selected').val(),

                        Realizado: $(this).val() != '' ? $(this).val() : null,
                        GestaoDeRisco:
                        {
                            CriticidadeGestaoDeRisco: $('.br-current').data('rating-value') == undefined ? 0 : $("[name^=formGestaoDeRiscoCriticidade] :selected").val(),
                            //TipoRegistro: 'gr',
                            IdResponsavelInicarAcaoImediata: $('[name=formGestaoDeRiscoResponsavelDefinicao]').val(),
                            DescricaoRegistro: $('[name=formGestaoDeRiscoIdentificacao]').val(),

                            Causa: $('[name=formGestaoDeRiscoCausa]').val() == undefined ? "" : $('[name=formGestaoDeRiscoCausa]').val(),
                            DsJustificativa: $('[name=formGestaoDeRiscojustificativa]').val() == undefined ? "" : $('[name=formGestaoDeRiscojustificativa]').val(),
                        }
                    };
                    arrayFormPlanoDeVooObj.push(formPlanoDeVooObj);
                }
            }
            else {
                formPlanoDeVooObj = {
                    Id: $(this).closest('div').find('[name^=IdPlanoDeVoo]').val(),
                    DataReferencia: APP.controller.IndicadorController.getDataProMes(i),
                    Realizado: $(this).val() != '' ? $(this).val() : null,
                };
                arrayFormPlanoDeVooObj.push(formPlanoDeVooObj);
            }


        });

        return arrayFormPlanoDeVooObj;

    },

    getDataProMes: function (_mes) {

        var meses = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        var data = new Date();
        //var ano = data.getFullYear();

        var ano = $('#form-criar-indicador-ano').val();

        for (i = 0; i < meses.length; i++) {
            if (meses[i] == _mes) {
                return data = '01/' + (i + 1) + '/' + ano;

            }
        }
    },

    getTotalPlanoDeVoo: function () {


        $('[name= formCriarIndicadorTpMedida]').on('change', function () {

            var tpMedida = $('[name^=formCriarIndicadorTpMedida] :selected').val();

            switch (tpMedida) {

                case "1":
                    var valor = $('[name^=formCriarIndicadorMeta]').val().replace('.', ',');

                    if (isNaN($('[name^=formCriarIndicadorMeta]').val())) {
                        valor = 0;
                    }
                    $('[name=formPlanoDeVooTotal]').val(valor);
                    $('[name=formCriarIndicadorUnidade]').prop('disabled', false).val('%');

                    $('[name^=formPlanoDeVooMeta]').each(function () {
                        $(this).val(valor)
                    });
                    break;
                case "2":
                    var valor = $('[name^=formCriarIndicadorMeta]').val().replace('.', ',');

                    if (isNaN($('[name^=formCriarIndicadorMeta]').val())) {
                        valor = 0;
                    }
                    $('[name=formCriarIndicadorUnidade]').prop('disabled', false).val('');
                    $('[name^=formPlanoDeVooMeta]').each(function () {
                        $(this).val("")
                    });

                    $('[name=formPlanoDeVooTotal]').val(valor);
                    break;
                default:
            }

        });


        $('[name=formCriarIndicadorMeta]').on('change', function () {

            var tpMedida = $('[name=formCriarIndicadorTpMedida] :selected').val();
            var val = $(this).val();

            switch (tpMedida) {

                case '1':
                    $('[name^=formPlanoDeVooMeta]').each(function () {

                        var valor = parseFloat($('[name^=formCriarIndicadorMeta]').val());

                        $(this).val(valor)
                    });
                case '2':
                    var val = $(this).val();
                    $('[name=formPlanoDeVooTotal]').val(val);
                default:
            }
        });
    },

    setTotalPlanoDeVoo: function () {

        $('[name^=formPlanoDeVooMeta]').on('change', function () {

            var tpMedida = $('[name=formCriarIndicadorTpMedida] :selected').val();

            if (tpMedida == '2') {

                var meta = parseFloat($('[name=formCriarIndicadorMeta]').val());
                var total = parseInt(0);

                $('[name^=formPlanoDeVooMeta]').each(function () {
                    var val = parseFloat($(this).val());
                    if (!isNaN(val)) {
                        total = total + val;
                    }
                });
                var newTotal = meta - total;
                $('[name=formPlanoDeVooTotal]').val(newTotal);
            }
            else {
                var newTotal = $('[name=formCriarIndicadorMeta]').val();

                $('[name=formPlanoDeVooTotal]').val(newTotal);
            }
        });

    },

    //Editar

    formEditarIndicador: function () {

        this.setShowAndHideEditar();
        this.setDisable();
        this.dashBoard();

    },

    setShowAndHideEditar: function () {

        $('#panel-dashboard').show();
        $('#tb-plano-de-voo tbody tr:nth-child(2)').show();
        $('[name^=formPlanoDeVooMeta]').trigger('change');

    },

    setDisable: function () {

        $('[name=formCriarIndicadorProcesso]').prop('disabled', true);
        $('[name=formCriarIndicadorResponsavel]').prop('disabled', true);
        $('[name=formCriarIndicadorPeriodicidade]').prop('disabled', true);
        $('[name=formCriarIndicadorSentido]').prop('disabled', true);
        $('[name=formCriarIndicadorAno]').prop('disabled', true);
        $('[name=formCriarIndicadorMeta]').prop('disabled', true);
        $('[name=formCriarIndicadorUnidade]').prop('disabled', true);
        $('[name=formCriarIndicadorObjetivo]').prop('disabled', true);
        $('[name=formCriarIndicadorIndicador]').prop('disabled', true);
        $('[name^=formPlanoDeVooMeta]').prop('disabled', true);
        $('[name=formPlanoDeVooTotal]').prop('disabled', true);
        $('[name=formCriarIndicadorPeriodicidadeMedicao]').prop('disabled', true);
        this.setDisableMes();

    },

    setDisableMes: function () {

        var data = new Date();
        data = data.getMonth();
        var ano = $('#form-criar-indicador-ano').val();
        var datayear = new Date();
        var year = datayear.getFullYear();

        var boolYear = (ano < year);

        var meses = $('[name^=formPlanoDeVooRealizado]');
        var medicao = parseInt($('[name=formCriarIndicadorPeriodicidadeMedicao] :selected').val());

        setDisabledMetaRealizado(meses, medicao);

        $('[name^=formPlanoDeVooRealizado]').each(function (e) {
            if (!boolYear) {
                if (e > data) {
                    $(this).prop('disabled', true);
                }
            }
        });

        $('[name^=formGraphPlanoDeVooRealizado]').each(function (e) {
            if (!boolYear) {
                if (e > data) {
                    $(this).hide();
                }
            }
        });
    },

    //Todos
    sendFormCriarIndicador: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.IndicadorController.validateForms();

            //var validate = true;
            if (validate == true) {
                APP.controller.IndicadorController.getindicadorObj();
            }

        });

    },

    validateForms: function () {

        var valid = true;
        $('[id^=panel-form]').each(function () {
            var panels = $('[id^=panel-form]');
            var isVisible = $(this).is(':visible');
            if ($(this).prop('id') !== 'panel-form-dashboard') {
                if (isVisible) {
                    var validate = $(this).closest('form').valid();
                    if (validate != true) {
                        valid = false;
                    }
                }
            }
        });
        $('[name=formPlanoDeVooTotal]').val()
        var total = parseInt($('[name=formPlanoDeVooTotal]').val());

        var tpMedida = $('[name^=formCriarIndicadorTpMedida] :selected').val();

        if (tpMedida == "2") {
            if (total !== parseInt(0)) {
                valid = false;
                bootbox.alert("Preencha todos os campos do plano de vôo , total da meta deve ser maior que 0");
            }
        }
        return valid;

    },

    getindicadorObj: function () {

        var indicadorObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "indicador":
                        indicadorObj = APP.controller.IndicadorController.getObjFormCriarIndicador();
                        APP.controller.IndicadorController.saveFormCriarIndicador(indicadorObj, "/Indicador/Salvar");
                        break;
                }
            }

        });

    },

    saveFormCriarIndicador: function (indicadorObj, url) {

        var erro = "";

        $.ajax({
            type: "POST",
            data: indicadorObj,
            dataType: 'json',
            url: url,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    var id = result.IdIndicador;
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/Indicador/Editar/" + id;
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

    //Relatorios
    jsonGlobal: {},

    dashBoard: function () {

        this.setJsonGlobal();
        this.setPreencherQtdContainerPagina();
        this.setRelatorioPlanoDeVoo();
    },

    setJsonGlobal: function (idIndicador) {

        var jsonLocal = APP.controller.IndicadorController.jsonGlobal;

        if ($.isEmptyObject(jsonLocal)) {

            APP.controller.IndicadorController.jsonGlobal = this.getJSONRelatorios(idIndicador);
        }

    },

    getJSONRelatorios: function (idIndicador) {

        var jsonRelatorioBarras = '';
        if (!isNaN(idIndicador)) {
            $.ajax({
                type: "GET",
                dataType: 'json',
                async: false,
                url: '/Indicador/RelatorioBarrasJSON',
                data: { IdIndicador: idIndicador },
                success: function (result) {
                    if (result.StatusCode == 200) {

                        jsonRelatorioBarras = result.Indicadores;
                    }
                },
                error: function (result) {
                    bootbox.alert(_options.MsgOcorreuErro);
                }
            });
        }
        else {
            $.ajax({
                type: "GET",
                dataType: 'json',
                async: false,
                url: '/Indicador/RelatorioBarrasJSON',
                data: { IdIndicador: $('[name=IdIndicador]').val() },
                success: function (result) {
                    if (result.StatusCode == 200) {

                        jsonRelatorioBarras = result.Indicadores;
                    }
                },
                error: function (result) {
                    bootbox.alert(_options.MsgOcorreuErro);
                }
            });
        }
        return jsonRelatorioBarras;
    },

    setPreencherQtdContainerPagina: function () {

        var qtdDiv = APP.controller.IndicadorController.jsonGlobal.length;

        for (i = 0; i < qtdDiv; i++) {
            $("#relatorios ul").append("<li class='col-md-12'><div id='container" + i + "'></div></li>");
        }
    },

    setRelatorioPlanoDeVoo: function () {

        APP.controller.IndicadorController.getRelatorioPlanoDeVoo();

    },

    getRelatorioPlanoDeVoo: function () {

        var jsonLocal = APP.controller.IndicadorController.jsonGlobal;

        for (i = 0; i < jsonLocal.length; i++) {
            var filtro = this.getFiltroRelatorioPlanoDeVoo(jsonLocal[i]);
            APP.component.Highcharts.colunas(filtro);
        }

    },

    getFiltroRelatorioPlanoDeVoo: function (dados) {

        var categorias = [];
        var valorMeta = [];
        var valorRealizado = [];
        var unidadeIndicador = dados.Unidade;
        var serie1 = "Realizado";
        var serie2 = "Meta";

        var seriesTotais = [
            {
                name: serie1,
                data: valorRealizado,
            }, {
                name: serie2,
                data: valorMeta,
            }];

        for (f = 0; f < dados.PeriodicidadeDeAnalises.length; f++) {

            for (j = 0; j < dados.PeriodicidadeDeAnalises[f].PlanoDeVoo.length; j++) {
                categorias.push(dados.PeriodicidadeDeAnalises[f].PlanoDeVoo[j].Mes);
                valorMeta.push(dados.PeriodicidadeDeAnalises[f].PlanoDeVoo[j].Valor);
            }

            for (var x = 0; x < dados.PeriodicidadeDeAnalises[f].MetasRealizadas.length; x++) {
                valorRealizado.push(dados.PeriodicidadeDeAnalises[f].MetasRealizadas[x].Realizado);
            }

        }


        var dataCheia = APP.component.Datatoday.init();

        var filtro = {
            Container: 'container' + i,
            Titulo: _options.MetasRealizadas,
            Categorias: categorias,
            UnidadeIndicador: unidadeIndicador,
            SeriesTotais: seriesTotais,
            Chart: { type: 'bar' },
            SubTitulo: _options.AtualizadoDia + dataCheia,
            TituloTextoLinha: _options.AtualizadoCoisa,
            AlinhamentoTituloTextoLinha: 'high',
            Legenda: {
                layout: 'vertical',
                align: 'left',
                x: 50,
                verticalAlign: 'top',
                y: 50,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF',
                shadow: true,
            }
        };

        return filtro;
    },

    HabilitaCamposIndicador: function (perfil) {

        if (perfil == '4') {

            $('#main').find('input, textarea, button, select').removeAttr('disabled');
            $("#form-criar-nao-conformidade-processo").attr("disabled", true);
            $("#form-criar-nao-conformidade-emissor").attr("disabled", true);

        }
        else {
            $('#main').find('input, textarea, button, select').removeAttr('disabled');
        }
    },

    setDestravarCamposIndicador: function () {

        this.buttonDestravar.on('click', function () {


            if (perfil == '4') {

                $('#main').find('input, textarea, button, select').removeAttr('disabled');
                $("#form-criar-nao-conformidade-processo").attr("disabled", true);
                $("#form-criar-nao-conformidade-emissor").attr("disabled", true);


            }
            else {

                $('#main').find('input, textarea, button, select').removeAttr('disabled');
            }


            var idIndicador = $('[name=IdIndicador]').val();
            var data = { "idIndicador": idIndicador };

            var per = parseInt($('[name=formCriarIndicadorPeriodicidadeMedicao] :selected').val());
            var perAnalise = parseInt($('[name=formCriarIndicadorPeriodicidade] :selected').val());

            var months = $('[name^=formPlanoDeVooRealizado]');
            var icons = $('[name^=formGraphPlanoDeVooRealizado]');

            //setPlanoDeVooDisabled(months, per);
            setIconPlusPlanoDeVooDisabled(icons, perAnalise);
            APP.controller.IndicadorController.setDisableMes();
            //var data = new Date();
            //data = data.getMonth();
            //var ano = $('#form-criar-indicador-ano').val();
            //var datayear = new Date();
            //var year = datayear.getFullYear();

            //var boolYear = (ano < year);
            //months.each(function (e) {

            //    if (e > data) {
            //        $(this).prop('disabled', true);
            //    }
            //});

            //icons.each(function (e) {
            //    if (e > data) {
            //        $(this).hide();
            //    }

            //});

            $.ajax({
                type: "POST",
                dataType: 'json',
                data: data,
                url: "/Indicador/DestravarDocumento",
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


        });
    },

    eventoImprimir: function () {

        this.buttonImprimir.on('click', function () {

            var IdIndicador = $('[name=IdIndicador]').val();

           
            APP.controller.IndicadorController.imprimir(IdIndicador);

        });

    },

    imprimir: function (IdIndicador) {


        if (IdIndicador != null) {

            APP.component.Loading.showLoading();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/Indicador/PDFDownload?id=' + IdIndicador, true);
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

    setPeriodicidadeAnalise: function () {

        $('[name=formCriarIndicadorPeriodicidadeMedicao]').on('change', function () {

            var per = parseInt($('[name=formCriarIndicadorPeriodicidadeMedicao] :selected').val());

            var months = $('[name^=formPlanoDeVooMeta]');

            setPlanoDeVooDisabled();
        });
    },



    ///Inicio dos métodos de gestão de riscos
    getDescricao: function () {

        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

            $(".excluir-tema").hide();
            $(".editar-tema").hide();

            var idDescricao = $(element).attr('id');
            CKEDITOR.replace(idDescricao);
        });

    },


    getTemasCores: function () {

        var divBarRating = $('.barraRating');
        APP.component.BarRating.setBarRatingGestaoDeRisco(divBarRating, 'bars-1to10');

        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

            var divContext = $(element).closest('[name=GestaoDeRisco]');

            var corRisco = $('[name=formGestaoDeRiscoCriticidadeCor]').val();
            var lastCores = $("[data-rating-value='" + corRisco + "']").last().index();
            for (i = 0; i <= lastCores; i++) {
                $($('.br-theme-bars-1to10').find('.br-widget a')[i]).addClass('br-selected');
            }
            $($('.br-theme-bars-1to10').find('.br-widget a')[lastCores]).trigger("click");
            $('.br-theme-bars-1to10').find("[data-rating-value='" + corRisco + "']").addClass('br-current');
            var widget = $('.br-widget');
            widget.addClass('barRating-disabled');
        });
    },

    getTemasPossuiGestaoDeRisco: function () {
        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

            var divContext = $(element).closest('[name=GestaoDeRisco]');
            var temGestaoDeRisco = divContext.find('[name=formGestaoDeRiscoRisco]').val();
            divContext.find('[name=formGestaoDeRiscoResponsavelDefinicao][value=' + temGestaoDeRisco + ']').prop('checked', temGestaoDeRisco);
        });
    },

    setPossuiGestaoDeRisco: function () {
        $('.formGestaoDeRiscoRisco').change(function () {

            var possuiGestaoDeRisco = APP.controller.IndicadorController.getPossuiGestaoDeRisco(this);
            APP.controller.IndicadorController.setRulesPossuiGestaoDeRisco(possuiGestaoDeRisco, this);

        });

    },

    setHidePossuiGestaoDeRisco: function () {


        $('[name^=formGestaoDeRiscoRisco]:checked').each(function () {
            var teste = this.name;
            $('[name=' + teste + ']').trigger('change');
            //var isVisible = $(this).is(':visible');
            //if (isVisible) {
            //	var validate = $(this).closest('form').valid();
            //	if (validate != true) {
            //		valid = false;
            //	}

            //}
        });

        //$('[name^=formGestaoDeRiscoRisco]').trigger('change');

    },


    getPossuiGestaoDeRisco: function (_this) {

        var possuiGestaoDeRisco = APP.component.Radio.init(_this.name);
        return possuiGestaoDeRisco;

    },

    getPossuiGestaoDeRiscoInformar: function (_this) {
        var possuiGestaoDeRisco = APP.component.Radio.init(_this.name);
        return possuiGestaoDeRisco;

    },

    setPossuiGestaoDeRiscoInformar: function () {

        $('.formInformarGestaoDeRiscoRisco').change(function () {

            var possuiGestaoDeRisco = APP.controller.IndicadorController.getPossuiGestaoDeRiscoInformar(this);
            APP.controller.IndicadorController.setRulesPossuiGestaoDeRiscoInformar(possuiGestaoDeRisco, this);

        });

    },

    getTemasDescricao: function () {

        //$('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

        //    var idDescricao = $(element).attr('id');
        //    CKEDITOR.replace(idDescricao);
        //});

        var descricao = $('[name=formGestaoDeRiscoDescricao]');

        var idDescricao = descricao.attr('id');
        CKEDITOR.replace(idDescricao);



    },

    setHidePossuiGestaoDeRiscoInformar: function () {

        $('[name^=formInformarGestaoDeRiscoRisco]:checked').each(function () {
            var teste = this.name;
            $('[name=' + teste + ']').trigger('change');
            //var isVisible = $(this).is(':visible');
            //if (isVisible) {
            //	var validate = $(this).closest('form').valid();
            //	if (validate != true) {
            //		valid = false;
            //	}

            //}
        });


        //$('[name^=formInformarGestaoDeRiscoRisco]').trigger('change');

    },

    setRulesPossuiGestaoDeRisco: function (_possuiGestaoDeRisco) {

        if (_possuiGestaoDeRisco == "true") {
            $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").show();
            $('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").show();
            $('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").show();
            $('[name=formGestaoDeRiscoCriticidade]').closest("[class^=col]").show();
            $('[name=formGestaoDeRiscoCausa]').closest("[class^=col]").show();
            $('.br-widget').removeClass('barRating-disabled');
        } else {
            $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").hide();
            $('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").hide();
            $('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").hide();
            $('[name=formGestaoDeRiscoCriticidade]').closest("[class^=col]").hide();
            $('[name=formGestaoDeRiscoCausa]').closest("[class^=col]").hide();
            $('.br-widget').addClass('barRating-disabled');
        }

    },

    getTodosResponsaveisPorAcaoImediata: function (_this) {

        var idSite = $('[name=IdSite]').val();
        var idFuncao = 49; // Funcionalidade(Implementar aÃ§Ã£o) que permite Criar aÃ§Ãµes Corretivas
        var idProcesso = $('[name=IdProcesso]').val();
        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcessoIndicadores?idProcesso=' + idProcesso + '&idSite=' + idSite + '&idFuncao=' + idFuncao + '}', (result) => {
            if (result.StatusCode == 200) {

                if ($(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"]').find("option").length <= 1) {
                    var comboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"] option');
                    var idComboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"]');
                    APP.component.SelectListCompare.selectList(result.Lista, comboResponsavel, idComboResponsavel, 'IdUsuario', 'NmCompleto');
                }

            }
        });

    },

};
