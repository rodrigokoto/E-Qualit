/*
|--------------------------------------------------------------------------
| Controlador Padrão
|--------------------------------------------------------------------------
*/

APP.controller.PadraoController = {
    
    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexPadrao") {
            this.indexPadrao();
        }
        if (page == "CriarPadrao") {
            this.acoesPadrao();
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

    //Index Padrao
    indexPadrao : function () {

        APP.component.DataTable.init('#tb-index-padrao');

    },

    //Acoes Padrao
    acoesPadrao : function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        this.setAndHide();
        this.setValidateForms();
        this.formCriarPadrao();

        this.sendFormCriarPadrao();

    },

    setAndHide : function () {

        //

    },

    setValidateForms : function () {

        var ObjFormCriarPadraoValidate = APP.controller.PadraoController.getObjFormCriarPadraoValidate();
        APP.component.ValidateForm.init(ObjFormCriarPadraoValidate, '#form-criar-padrao');

    },

    //Formulario Criar Padrao
    formCriarPadrao : function () {

        this.setAndHideCriarPadrao();

    },

    setAndHideCriarPadrao : function () {

        //

    },
    
    getObjFormCriarPadraoValidate : function () {

        var acoesPadraoFormCriarPadraoObj = {
            //formCriaClienteLogo: {required: true, minFiles: 1},
            formCriaPadraoNome: 'required',
        };

        return acoesPadraoFormCriarPadraoObj;

    },

    getObjFormCriarPadrao : function () {

        var imgB64 = $('[name=formCriaClienteLogo]').prop('files').length != 0 ?$('[name=formCriaClienteLogo]').prop('files')[0].name : '';

        var acoesPadraoFormCriarPadraoObj = {
            TipoConteudo: imgB64,
            NomeArquivo: $('[name=formCriaUsuarioLogo]').data('b64'),
            Select: $('[name=formCriaPadraoSelect] :selected').val(),
            Checked:$('[name=formCriaPadraoChecked]:checked').val(),
            Input:$('[name=formCriaPadraoInput]').val(),
            Radio: APP.component.Radio.init('formCriaPadraoRadio'),
            Array: APP.controller.PadraoController.getObjFormCriarPadraoArray(),
        };

        return acoesPadraoFormCriarPadraoObj;

    },

    getObjFormCriarPadraoArray : function () {
        
        var arrayAcoesPadraoFormCriarPadraoObj = [];
        var acoesPadraoFormCriarPadraoObj = {
            TipoConteudo: $('[name=formCriaClienteLogo]').prop('files')[0].name,
            NmLogo: $('[name=formCriaClienteLogo]').data('b64'),
            Select: $('[name=formCriaPadraoSelect] :selected').val(),
            Checked:$('[name=formCriaPadraoChecked]:checked').val(),
        };

        arrayAcoesPadraoFormCriarPadraoObj.push(acoesPadraoFormCriarPadraoObj);

        return arrayAcoesPadraoFormCriarPadraoObj;
        
    },

    //Todos
    sendFormCriarPadrao : function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.PadraoController.validateForms();

            //var validate = true;
            if (validate == true){
                var padraoObj = APP.controller.PadraoController.getCriarPadraoObj();
                APP.controller.PadraoController.saveFormCriarPadrao(padraoObj);
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

    getCriarPadraoObj : function () {

        var padraoObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');
            
            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "padrao":
                        padraoObj = APP.controller.PadraoController.getObjFormCriarPadrao();
                        break;
                }
            }

            });

        return padraoObj;

    },

    saveFormCriarPadrao : function (padraoObj) {

        var erro = "";

        $.ajax({
            type: "POST",
            data: padraoObj,
            dataType: 'json',
            url: "/Padrao/SalvarPadrao",
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