APP.controller.FuncionarioController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "AcoesFuncionario") {
            this.acoesFuncionario();
        }

    },

    setup: function () {

        //Criar Cliente
        this.buttonSalvar = $(".btn-salvar");

    },

    acoesFuncionario: function () {

        this.setValidateForms();
        this.formCriarCliente();
        this.formCriarSite();

        this.sendFormCriarUsuario();

    },

    setValidateForms: function () {

        var ObjFormCriarClienteValidate = APP.controller.ClienteController.getObjFormCriarFuncionarioValidate();
        APP.component.ValidateForm.init(ObjFormCriarClienteValidate, '#form-cria-funcionario');

    },

    //Formulario Criar Funcionario
    formCriarFuncionario: function () {

        this.setAndHideCriarFuncionario();

    },

    setAndHideCriarFuncionario: function () {

        //

    },


    getObjFormCriarFuncionarioValidate: function () {

        var funcionarioFormCriarFuncionarioObj = {
            //formCriaClienteLogo: {required: true, minFiles: 1},
            formCriaClienteNome: 'required',
            formCriaClienteUrl: 'required',
            formCriaClienteDtValidade: 'required',
            formCriaClienteTrocaSenha: 'required',
            formCriaClienteBloquear: 'required',
            formCriaClienteArmazenar: 'required',
            //formCriarClienteContrato: 'required',
        };

        return funcionarioFormCriarFuncionarioObj;

    },

    getObjFormCriarFuncionario: function () {

        var funcionarioFormCriarFuncionarioeObj = {
            formCriaClienteSenha: $('[name=formCriaClienteSenha]:checked').val(),
        };

        return funcionarioFormCriarFuncionarioeObj;

    },

    //Todos
    sendFormCriarFuncionario: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.ClienteController.validateForms();

            //var validate = true;
            if (validate == true) {

                var clienteObj = APP.controller.ClienteController.getCriarClienteObj();
                APP.controller.ClienteController.saveFormCriarCliente(clienteObj);
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

    getCriarFuncionarioObj: function () {

        var funcionarioObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "funcionario":
                        funcionarioObj = APP.controller.ClienteController.getObjFormCriarCliente();
                        break;
                }
            }

        });

        return funcionarioObj;

    },

    saveFormCriarFuncionario: function (funcionario) {

        $.ajax({
            type: "POST",
            data: funcionario,
            dataType: 'json',
            processData: false,
            contentType: false,// not json
            url: "/Cliente/SalvarCliente",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    bootbox.alert(_options.MsgSaveSuccess);
                    //window.location.reload();
                }
                else if (result.StatusCode == 500) {
                    var erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                }

            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

};
