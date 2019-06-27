
/*
|--------------------------------------------------------------------------
| Controlador Plai
|--------------------------------------------------------------------------
*/

APP.controller.PlaiController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "AcoesPlai") {
            this.acoesPlai();
        }

    },

    setup: function () {

        //Criar Pagina
        this.buttonSalvar = $(".btn-salvar");
        this.buttonImprimir = $(".btn-imprimir");

    },

    //Models
    models: {

        //PerfilUsuarioModel: APP.model.PerfilUsuario,

    },

    //Acoes Plai
    acoesPlai: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        this.setAndHide();
        this.setValidateForms();
        this.formCriarPlai();
        this.eventoImprimir();
		this.sendFormCriarPlai();
		this.setDatePickerDate();

    },

    eventoImprimir: function () {

        this.buttonImprimir.on('click', function () {

            var idPlai = $(".IdPlai").val();
            APP.controller.PlaiController.imprimir(idPlai);

        });

    },



    imprimir: function (idPlai) {


        if (idPlai != null) {

            APP.component.Loading.showLoading();
            
            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/Plai/DownloadPdf?idPlai=' + idPlai, true);
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

        //

    },

    setValidateForms: function () {

        var ObjFormCriarPlaiValidate = APP.controller.PlaiController.getObjFormCriarPlaiValidate();
        APP.component.ValidateForm.init(ObjFormCriarPlaiValidate, '#form-criar-plai');

    },

    //Formulario Criar Plai
    formCriarPlai: function () {

        this.setAndHideCriarPlai();
        this.getElaborador();
        //this.getGestores();
        this.setRulesDateReuniaoEncerramento();

    },

    setAndHideCriarPlai: function () {

        //

    },

    getElaborador: function () {

        var idSite = $('[name=IdSite]').val();
        var idFuncao = 43; // Funcionalidade(Cadastrar) que permite usuario criar nc
        $.get('/Usuario/ObterUsuariosPorFuncao?idSite=' + idSite + '&idFuncao=' + idFuncao +'', (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarPlaiElaborador] option'), $('[name=formCriarPlaiElaborador]'), 'IdUsuario', 'NmCompleto');
            }
        });

    },

    //getGestores: function () {

    //    var idSite = $('[name=IdSite]').val();
    //    var idPerfil = 3; // Perfil Coordenador
    //    $.get('/Usuario/ObterUsuariosPorPerfilESite?idSite=${idSite}&idPerfil=${idPerfil}', (result) => {
    //        if (result.StatusCode == 200) {
    //            APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarPlaiGerentes] option'), $('[name=formCriarPlaiGerentes]'), 'IdUsuario', 'NmCompleto');
    //        }
    //    });

    //},

    getObjFormCriarPlaiValidate: function () {

        var formCriarPlaioObj = {
            formCriarPlaiDtReuniaoAbertura: 'required',
            formCriarPlaiHrReuniaoAbertura: 'required',
            formCriarPlaiDtReuniaoEncerramento: 'required',
            formCriarPlaiHrReuniaoEncerramento: 'required',
            formCriarPlaiElaborador: 'required',
            formCriarPlaiProcessoDt: 'required',
            formCriarPlaiProcessoHr: 'required',
            //formCriarPlaiProcessoNormas: 'required',
        };

        return formCriarPlaioObj;

    },

    getObjFormCriarPlai: function () {

        var formCriarPlaiObj = {
            IdPlai: $('[name=IdPlai]').val(),
            DataReuniaoAbertura: $('[name=formCriarPlaiDtReuniaoAbertura]').val() + " " + $('[name=formCriarPlaiHrReuniaoAbertura]').val(),
            DataReuniaoEncerramento: $('[name=formCriarPlaiDtReuniaoEncerramento]').val() + " " + $('[name=formCriarPlaiHrReuniaoEncerramento]').val(),
            IdElaborador: $('[name=formCriarPlaiElaborador] :selected').val(),
            PlaiGerentes: APP.controller.PlaiController.getObjFormCriarPlaiGerentes(),
            PlaiProcessoNorma: APP.controller.PlaiController.getObjFormCriarPlaiProcessos(),

        };

        return formCriarPlaiObj;

    },

    getObjFormCriarPlaiProcessos: function () {

        var arrayFormCriarPlaiObj = [];
        var formCriarPlaiObj = {};

        $('.plai-processo').each(function () {
            
            var IdProcesso = $(this).find('[name=IdProcesso]').val();
            var IdPlai = $(this).find('[name=IdPlai]').val();
            var data = $(this).find('[name=formCriarPlaiProcessoDt]').val() + " " + $(this).find('[name=formCriarPlaiProcessoHr]').val();
            
            $(this).find('[name^=formCriarPlaiProcessoNormas]').each(function () {

                formCriarPlaiObj = {
                    IdPlai: IdPlai,
                    IdProcesso: IdProcesso,
                    Data: data,
                    IdNorma: $(this).val(),
                    Ativo: $(this).is(':checked'),
                };
                if (formCriarPlaiObj.Ativo == true)
                {
                    arrayFormCriarPlaiObj.push(formCriarPlaiObj);                
                }
                
            });

            
        });

        return arrayFormCriarPlaiObj;

    },

    getObjFormCriarPlaiGerentes: function () {

        var arrayFormCriarPlaiGerentesObj = [];
        var formCriarPlaiGerenteObj = {};

        $("#form-criar-plai-gerentes :selected").each(function () {

            formCriarPlaiGerenteObj = {
                IdPlai: $('[name=IdPlai]').val(),
                IdUsuario: $(this).val(),
            };

            arrayFormCriarPlaiGerentesObj.push(formCriarPlaiGerenteObj);
        });

        return arrayFormCriarPlaiGerentesObj;

    },
    

    setRulesDateReuniaoEncerramento: function () {

        $('[name=formCriarPlaiDtReuniaoAbertura]').on('change', function () {
			debugger;
            APP.component.Datapicker.setDataPicker(this, '[name=formCriarPlaiDtReuniaoEncerramento]');

        });

	},

	setDatePickerDate: function () {
		debugger;
		
		var mes = $('[name=MesPlai]').val();
		//startDate
		var dataAtual = new Date();
		var ano = dataAtual.getFullYear();

		var dataMinima = new Date(ano, parseInt(mes) -1 , 1);
		var dataMaxima = new Date(ano, mes, 0);


		//$('[name=formCriarPlaiDtReuniaoAbertura]').datepicker('option', 'minDate', new Date(dataMinima));
		var dataInicial = $('[name=formCriarPlaiDtReuniaoAbertura]').val();
		$('[name=formCriarPlaiDtReuniaoAbertura]').datepicker('option', 'minDate', new Date(dataMinima));
		$('[name=formCriarPlaiDtReuniaoAbertura]').datepicker('option', 'maxDate', new Date(dataMaxima));
		$('[name=formCriarPlaiDtReuniaoAbertura]').val(dataInicial);
		//$('[name=formCriarPlaiDtReuniaoAbertura]').on('change', function () {
		//	debugger;
		//	APP.component.Datapicker.setDataPicker(this, '[name=formCriarPlaiDtReuniaoEncerramento]');

		//});

	},

    //Todos
    sendFormCriarPlai: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.PlaiController.validateForms();

            //var validate = true;
            if (validate == true) {
                var plaiObj = APP.controller.PlaiController.getCriarPlaiObj();
                APP.controller.PlaiController.saveFormCriarPlai(plaiObj);
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

    getCriarPlaiObj: function () {

        var plaiObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                if(form == "plai")
                {
                    plaiObj = APP.controller.PlaiController.getObjFormCriarPlai();
                 
                }
            }

        });

        return plaiObj;

    },

    saveFormCriarPlai: function (plaiObj) {

        var erro = "";

        $.ajax({
            type: "POST",
            data: { Plai: plaiObj },
            dataType: 'json',
            url: "/Plai/Salvar",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        //window.location.href = "/Usuario/Index";
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

};
