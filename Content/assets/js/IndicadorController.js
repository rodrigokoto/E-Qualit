
/*
|--------------------------------------------------------------------------
| Controlador Indicador
|--------------------------------------------------------------------------
*/

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

            bootbox.confirm(msgIconeDeleteIndicador, function (result) {
                if (result == true) {
                    APP.controller.IndicadorController.setDelIndicador();
                }
            });

        });

    },

    setDelIndicador: function () {

        var idIndicador = $('.del-indicador').data("id-indicador");
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
        this.setAndHide();

        this.setValidateForms();
        this.formCriarIndicador();
        this.sendFormCriarIndicador();

        if ($('[name=IdIndicador]').val() > 0) {
            this.formEditarIndicador();
        }


        this.setDestravarCamposIndicador();

        if (habilitaLoad == 'sim') {

            this.HabilitaCamposIndicador(perfil);
        }

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

        var idSite = $('[name=IdSite]').val();
        var idFuncao = 27; // Funcionalidade(Cadastrar) que permite usuario criar nc
        $.get('/Usuario/ObterUsuariosPorFuncao?idSite=' + idSite + '&idFuncao=' + idFuncao +'', (result) => {
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
            Direcao: $('[name=formCriarIndicadorSentido] :selected').val(),
            Ano: $('[name=formCriarIndicadorAno]').val(),
            MetaAnual: $('[name=formCriarIndicadorMeta]').val(),
            Unidade: $('[name=formCriarIndicadorUnidade]').val(),
            Objetivo: $('[name=formCriarIndicadorObjetivo]').val(),
            Descricao: $('[name=formCriarIndicadorIndicador]').val(),

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

        $('[name^=formPlanoDeVooRealizado]').each(function (i) {

            formPlanoDeVooObj = {
                Id: $(this).closest('div').find('[name^=IdPlanoDeVoo]').val(),
                DataReferencia: APP.controller.IndicadorController.getDataProMes(i),
                Realizado: $(this).val() != '' ? $(this).val() : null,
            };
            arrayFormPlanoDeVooObj.push(formPlanoDeVooObj);

        });

        return arrayFormPlanoDeVooObj;

    },

    getDataProMes: function (_mes) {

        var meses = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        var data = new Date();
        for (i = 0; i < meses.length; i++) {
            if (meses[i] == _mes) {
                return data = '01/' + (i + 1) + '/2018';
            }
        }
    },

    getTotalPlanoDeVoo: function () {

        $('[name=formCriarIndicadorMeta]').on('change', function () {
            var val = $(this).val();
            $('[name=formPlanoDeVooTotal]').val(val);
        });

    },

    setTotalPlanoDeVoo: function () {

        $('[name^=formPlanoDeVooMeta]').on('change', function () {

            var meta = parseInt($('[name=formCriarIndicadorMeta]').val());
            var total = parseInt(0);
            $('[name^=formPlanoDeVooMeta]').each(function () {
                var val = parseInt($(this).val());
                if (!isNaN(val)) {
                    total = total + val;
                }
            });
            var newTotal = meta - total;
            $('[name=formPlanoDeVooTotal]').val(newTotal);

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
        this.setDisableMes();

    },

    setDisableMes: function () {

        var data = new Date();
        data = data.getMonth();

        $('[name^=formPlanoDeVooRealizado]').each(function (e) {
            if (e > data) {
                $(this).prop('disabled', true);
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
            var isVisible = $(this).is(':visible');
            if (isVisible) {
                var validate = $(this).closest('form').valid();
                if (validate != true) {
                    valid = false;
                }

            }
        });


        if (parseInt($('#form-planodevoo-meta-total').val()) != parseInt($('#form-criar-indicador-meta').val()))
        {
            valid = false;
            bootbox.alert(_options.MsgMetaTotal);
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

};
