/*
|--------------------------------------------------------------------------
| Login
|--------------------------------------------------------------------------
*/

APP.controller.LoginController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "Login") {
            this.indexLogin();
        }
        if (page == "RecuperarSenha") {
            this.recuperarSenha();
        }
        if (page == "AlterarSenhaLogin") {
            this.alterarSenha();
        }

    },

    setup: function () {

        //Index Login
        this.buttonLogin = $(".btn-login");

        //Recuperar Senha
        this.buttonRecuperarSenha = $(".btn-recuperar-senha");

        //Alterar Senha
        this.buttonAlterarSenha = $(".btn-alterar-senha");

    },

    //Index Login
    indexLogin : function () {

        this.setValidateForms();
        this.sendFormLogin();

    },

    setValidateForms : function () {
        
        var ObjFormLoginValidate = APP.controller.LoginController.getObjObjFormLoginValidate();
        APP.component.ValidateForm.init(ObjFormLoginValidate, '#form-login');

    },

    getObjObjFormLoginValidate : function () {
        
        var indexLoginFormLoginObj = {
            
            formLoginEmail: {required: true, email: true},
            formLoginSenha: 'required',
            hiddenRecaptcha: {
                required: function () {
                    if (grecaptcha.getResponse() == '') {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
        };

        return indexLoginFormLoginObj;

    },

    getObjFormLogin : function () {

        var indexLoginFormLoginObj = {
            CdIdentificacao:$('[name=formLoginEmail]').val(),
            CdSenha:$('[name=formLoginSenha]').val(),
        };

        return indexLoginFormLoginObj;

    },

    //Formulario Recuperar Senha
    recuperarSenha : function () {

        this.setValidateFormsRecuperarSenha();
        this.sendFormRecuperarSenha();

    },

    setValidateFormsRecuperarSenha : function () {
        
        var ObjFormRecuperarSenhaValidate = APP.controller.LoginController.getObjObjFormRecuperarSenhaValidate();
        APP.component.ValidateForm.init(ObjFormRecuperarSenhaValidate, '#form-recuperar-senha');

    },

    getObjObjFormRecuperarSenhaValidate : function () {
        
        var recuperarSenhaFormLoginObj = {

            formRecuperarSenhaEmail: {required: true, email: true},

        };

        return recuperarSenhaFormLoginObj;

    },

    getObjFormRecuperarSenha : function () {

        var recuperarSenhaFormLoginObj = {
            __RequestVerificationToken:$('[name=__RequestVerificationToken]').val(),
            CdIdentificacao:$('[name=formRecuperarSenhaEmail]').val(),
        };

        return recuperarSenhaFormLoginObj;

    },

    //Formulario Alterar Senha
    alterarSenha : function () {
        
        this.setValidateFormsAlterarSenha();
        this.setStrongPassword();
        this.setSenhaNotEquals();
        this.sendFormAlterarSenha();

    },

    setValidateFormsAlterarSenha : function () {
        
        var ObjFormAlterarSenhaValidate = APP.controller.LoginController.getObjObjFormAlterarSenhaValidate();
        APP.component.ValidateForm.init(ObjFormAlterarSenhaValidate, '#form-alterar-senha');

    },

    setStrongPassword : function () {

        APP.component.StrongPassword.init('formAlterarSenhaNova');

    },

    setSenhaNotEquals : function () {
        
        $.validator.addMethod("notEqualsTo", function(){
            var senhaAtual = $('[name=formAlterarSenhaAtual]').val();
            return ($('[name=formAlterarSenhaNova]').val() != senhaAtual);
        });

    },

    getObjObjFormAlterarSenhaValidate : function () {
        
        var alterarSenhaFormLoginObj = {
            formAlterarSenhaAtual: {required: true},
            formAlterarSenhaNova: {required: true, minlength: 6, notEqualsTo : true, strongPassword : true},
            formAlterarSenhaConfirmar: {required: true, minlength: 6, notEqualsTo : true, equalTo: "[name=formAlterarSenhaNova]"},
        };

        return alterarSenhaFormLoginObj;

    },

    getObjFormAlterarSenha : function () {

        var alterarSenhaFormLoginObj = {
            __RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
            IdUsuario: $('[name=formAlterarSenhaIdUsuario]').val(),
            SenhaAtual: $('[name=formAlterarSenhaAtual]').val(),
            CdSenha: $('[name=formAlterarSenhaNova]').val(),
            ConfirmaSenha: $('[name=formAlterarSenhaConfirmar]').val(),
            
        };

        return alterarSenhaFormLoginObj;

    },

    //Todos
    sendFormLogin : function () {
        
        this.buttonLogin.unbind('click');
        this.buttonLogin.on('click', function () {

            var validate = APP.controller.LoginController.validateForms();

            //var validate = true;
            if (validate == true){
                var loginObj = APP.controller.LoginController.getLoginObj();
                APP.controller.LoginController.saveFormLogin(loginObj);
            }

        });

    },
    
    sendFormRecuperarSenha : function () {
        
        this.buttonRecuperarSenha.unbind('click');
        this.buttonRecuperarSenha.on('click', function () {

            var validate = APP.controller.LoginController.validateForms();

            //var validate = true;
            if (validate == true){
                var recuperarSenhaObj = APP.controller.LoginController.getLoginObj();
                APP.controller.LoginController.saveFormRecuperarSenha(recuperarSenhaObj);
            }

        });

    },

    sendFormAlterarSenha : function () {
        
        this.buttonAlterarSenha.unbind('click');
        this.buttonAlterarSenha.on('click', function () {

            var validate = APP.controller.LoginController.validateForms();

            //var validate = true;
            if (validate == true){
                var alterarSenhaObj = APP.controller.LoginController.getLoginObj();
                APP.controller.LoginController.saveFormAlterarSenha(alterarSenhaObj);
            }

        });

    },

    validateForms : function () {

        var valid = true;
        $('[id^=form]').each(function () {
            var isVisible = $(this).is(':visible');
            if (isVisible) {
                var validate = $(this).valid();
                if (validate != true) {
                    valid = false;
                }

            }
        });

        return valid;

    },

    getLoginObj : function () {
        
        var loginObj = {};

        $('[id^=form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');
            
            if (isVisible) {
                var form = idPanel[1];
                switch (form) {
                    case "login":
                        loginObj = APP.controller.LoginController.getObjFormLogin();
                        break;
                    case "recuperar":
                        loginObj = APP.controller.LoginController.getObjFormRecuperarSenha();
                        break;
                    case "alterar":
                        loginObj = APP.controller.LoginController.getObjFormAlterarSenha();
                        break;
                }
            }

            });

        return loginObj;

    },

    saveFormLogin : function (loginObj) {
        
        $.ajax({
            type: "GET",
            data: loginObj,
            dataType: 'json',
            async: false,
            url: $("#hdUrl").val(),
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode === 200) {
                    window.location.href = "/Home/Index";
                } else if (result.StatusCode == 505) {
                    var erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                } else if (result.StatusCode == 500) {
                    var erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                }
            },
            error: function (result) {
                var erro = APP.component.ResultErros.init(result.Erro);
                bootbox.alert(erro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

    saveFormRecuperarSenha : function (recuperarSenhaObj) {
        
        $.ajax({
            type: "POST",
            data: recuperarSenhaObj,
            dataType: 'json',
            url: "/Login/RecuperarSenha",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode === 200) {
                    bootbox.alert(result.Success, function (result) {
                        //
                    });
                } else if (result.StatusCode == 505) {
                    var erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                } else if (result.StatusCode == 500) {
                    var erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                }
            },
            error: function (result) {
                var erro = APP.component.ResultErros.init(result.Erro);
                bootbox.alert(erro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

    saveFormAlterarSenha : function (usuario) {

        $.ajax({
            type: "POST",
            data: usuario,
            dataType: 'json',
            url: "/Login/AlterarSenha",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode === 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/";
                    });
                } else if (result.StatusCode == 505) {
                    var erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                } else if (result.StatusCode == 500) {
                    var erro = APP.component.ResultErros.init(result.Erro);
                    bootbox.alert(erro);
                }
            },
            error: function (result) {
                var erro = APP.component.ResultErros.init(result.Erro);
                bootbox.alert(erro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

};
