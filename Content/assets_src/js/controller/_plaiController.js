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

    },

    //Models
    models: {
        
        //PerfilUsuarioModel: APP.model.PerfilUsuario,

    },

    //Acoes Plai
    acoesPlai : function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        this.setAndHide();
        this.setValidateForms();
        this.formCriarPlai();

        this.sendFormCriarPlai();

    },

    setAndHide : function () {

        //

    },

    setValidateForms : function () {

        var ObjFormCriarPlaiValidate = APP.controller.PlaiController.getObjFormCriarPlaiValidate();
        APP.component.ValidateForm.init(ObjFormCriarPlaiValidate, '#form-criar-plai');

    },

    //Formulario Criar Plai
    formCriarPlai : function () {

        this.setAndHideCriarPlai();
        this.getElaborador();
        this.setRulesDateReuniaoEncerramento();

    },

    setAndHideCriarPlai : function () {

        //

    },

    getElaborador : function () {

        var idSite = $('[name=IdSite]').val();
        var idFuncao = 12; // Funcionalidade(Cadastrar) que permite usuario criar nc
        $.get(`/Usuario/ObterUsuariosPorFuncao?idSite=${idSite}&idFuncao=${idFuncao}`, (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name=formCriarPlaiElaborador] option'), $('[name=formCriarPlaiElaborador]'), 'IdUsuario', 'NmCompleto');
            }
        });

    },
    
    getObjFormCriarPlaiValidate : function () {

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

    getObjFormCriarPlai : function () {

        var formCriarPlaiObj = {

            formCriarPlaiDtReuniaoAbertura: $('[name=formCriarPlaiDtReuniaoAbertura]').val(),
            formCriarPlaiHrReuniaoAbertura: $('[name=formCriarPlaiHrReuniaoAbertura]').val(),
            formCriarPlaiDtReuniaoEncerramento: $('[name=formCriarPlaiDtReuniaoEncerramento]').val(),
            formCriarPlaiHrReuniaoEncerramento: $('[name=formCriarPlaiHrReuniaoEncerramento]').val(),
            formCriarPlaiElaborador: $('[name=formCriarPlaiElaborador] :selected').val(),
            formCriarPlaiProcessos: APP.controller.PlaiController.getObjFormCriarPlaiProcessos(),
            
        };

        return formCriarPlaiObj;

    },

    getObjFormCriarPlaiProcessos : function () {
        
        var arrayFormCriarPlaiObj = [];
        var formCriarPlaiObj = {};
        
        $('.plai-processo').each(function () {
            
            formCriarPlaiObj = {
                idProcesso: $(this).find('[name=idProcesso]').val(),
                formCriarPlaiProcessoDt: $(this).find('[name=formCriarPlaiProcessoDt]').val(),
                formCriarPlaiProcessoHr: $(this).find('[name=formCriarPlaiProcessoHr]').val(),
                formCriarPlaiNormas: APP.controller.PlaiController.getObjFormCriarPlaiNormas(this),
            };

            arrayFormCriarPlaiObj.push(formCriarPlaiObj);
        });

        return arrayFormCriarPlaiObj;
        
    },

    getObjFormCriarPlaiNormas : function (_this) {
        
        var arrayFormCriarPlaiNormasObj = [];
        var formCriarPlaiNormasObj = {};
        
        $(_this).find('[name^=formCriarPlaiProcessoNormas]').each(function () {

            formCriarPlaiNormasObj = {
                idNorma: $(this).val(),
                Ativo: $(this).is(':checked'),
            };

            arrayFormCriarPlaiNormasObj.push(formCriarPlaiNormasObj);
        });

        return arrayFormCriarPlaiNormasObj;
        
    },

    setRulesDateReuniaoEncerramento : function () {

        $('[name=formCriarPlaiDtReuniaoAbertura]').on('change', function() {
                        
            APP.component.Datapicker.setDataPicker(this, '[name=formCriarPlaiDtReuniaoEncerramento]');

        });

    },

    //Todos
    sendFormCriarPlai : function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.PlaiController.validateForms();

            //var validate = true;
            if (validate == true){
                var plaiObj = APP.controller.PlaiController.getCriarPlaiObj();
                APP.controller.PlaiController.saveFormCriarPlai(plaiObj);
            }

        });

    },

    validateForms : function () {

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

    getCriarPlaiObj : function () {

        var plaiObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');
            
            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "plai":
                        plaiObj = APP.controller.PlaiController.getObjFormCriarPlai();
                        break;
                }
            }

            });

        return plaiObj;

    },

    saveFormCriarPlai : function (plaiObj) {

        var erro = "";
        
        $.ajax({
            type: "POST",
            data: plaiObj,
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