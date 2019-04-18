/*
|--------------------------------------------------------------------------
| Controlador Cargo
|--------------------------------------------------------------------------
*/
APP.controller.CargoController = {
    
    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexCargo") {
            this.indexCargo();
        }
        if (page == "CriarCargo") {
            this.acoesCargo();
        }

    },

    setup: function () {

        //Criar Pagina
        this.buttonSalvar = $(".btn-salvar");

    },

    //Index Cargo
    indexCargo : function () {

        APP.component.DataTable.init('#tb-index-cargo');
        APP.controller.CargoController.setMsgIconeAtivo();
        APP.controller.CargoController.setMsgIconeExcluir();
        
    },

    setMsgIconeAtivo : function () {
        $('.ativoinativo').on('click', function () {

            var idCargo = $(this).data('id');
            var AtivoInativo = $(this).find('i').attr("title");

            var msgIconeAtivoAtivar = $('[name=msgIconeAtivoAtivar]').val();
            var msgIconeAtivoInativar = $('[name=msgIconeAtivoInativar]').val();

            if (AtivoInativo != "Ativo") {
                bootbox.confirm(msgIconeAtivoAtivar, function (result) {
                    if (result == true) {
                        APP.controller.CargoController.getMsgIconeAtivo(idCargo);
                    }
                });
            } else {
                bootbox.confirm(msgIconeAtivoInativar, function (result) {
                    if (result == true) {
                        APP.controller.CargoController.getMsgIconeAtivo(idCargo);
                    }
                });
            }
            
        });

    },

    getMsgIconeAtivo : function (_idCargo) {

        var erro = "";
        var idSite = $('[name=IdSite]').val();
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: `/Cargo/AtivaInativa?idCargo=${_idCargo}`,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.href = "/Cargo/Index/"+idSite;
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

    setMsgIconeExcluir : function () {
        $('.excluir').on('click', function () {

            var idCargo = $(this).data('id');
            var msgIconeExcluir = $(this).find('i').attr("title");

            bootbox.confirm(msgIconeExcluir, function (result) {
                if (result == true) {
                    APP.controller.UsuarioController.getMsgIconeExcluir(idCargo);
                }
            });
            
        });

    },

    getMsgIconeExcluir : function (_idCargo) {

        var erro = "";
        var idSite = $('[name=IdSite]').val();
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: `/Usuario/Excluir?idCargo=${_idCargo}`,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    window.location.href = "/Cargo/Index/"+idSite;
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

    //Acoes Cargo
    acoesCargo : function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        APP.component.SelectAll.init('formCargoModuloFuncionalidadeAll');
        this.setAndHide();
        this.setValidateForms();
        this.formCriarCargo();

        this.sendFormCriarCargo();

    },

    setAndHide : function () {

        //

    },

    setValidateForms : function () {

        var ObjFormCriarCargoValidate = APP.controller.CargoController.getObjFormCriarCargoValidate();
        APP.component.ValidateForm.init(ObjFormCriarCargoValidate, '#form-criar-cargo');

    },

    //Formulario Criar Cargo
    formCriarCargo : function () {

        this.setAndHideCriarCargo();

    },

    setAndHideCriarCargo : function () {

        //

    },
    
    getObjFormCriarCargoValidate : function () {

        var acoesCargoFormCriarCargoObj = {
            formCargoNome: 'required',
        };

        return acoesCargoFormCriarCargoObj;

    },

    getObjFormCriarCargo : function () {

        var acoesCargoFormCriarCargoObj = {
            IdCargo: $('[name=idCargo]').val(),
            IdSite: $('[name=IdSite]').val(),
            NmNome: $('[name=formCargoNome]').val(),
            CargoProcessos: APP.controller.CargoController.getObjFormCargoProcessosArray(),
        };

        return acoesCargoFormCriarCargoObj;

    },

    getObjFormCargoProcessosArray : function () {
        
        var arrayAcoesCargoFormCriarCargoProcessosObj = [];
        $('[id^=form-cargo-processo-]').each(function (){
            $(this).find('input[id^=form-cargo-modulo-funcionalidade]:checked:not(:first-child)').each(function () {
            
            var acoesCargoFormCriarCargoProcessosbj = {
                IdCargoProcesso: $(this).closest('div').find('[name=IdCargoProcesso]').val(),
                IdCargo: $('[name=idCargo]').val(),
                IdProcesso: $(this).closest('form').find('[name=formCargoProcessoIdProcesso]').val(),
                IdFuncao: $(this).val()
            };
            
            arrayAcoesCargoFormCriarCargoProcessosObj.push(acoesCargoFormCriarCargoProcessosbj);
            });
        });
        
        return arrayAcoesCargoFormCriarCargoProcessosObj;
        
    },

    //Todos
    sendFormCriarCargo : function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.CargoController.validateForms();

            //var validate = true;
            if (validate == true){
                var cargoObj = APP.controller.CargoController.getCriarCargoObj();
                APP.controller.CargoController.saveFormCriarCargo(cargoObj);
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

    getCriarCargoObj : function () {

        var cargoObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');
            
            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "cargo":
                        cargoObj = APP.controller.CargoController.getObjFormCriarCargo();
                        break;
                }
            }

            });

        return cargoObj;

    },

    saveFormCriarCargo : function (cargo) {

        var idSite = $('[name=IdSite]').val();
        $.ajax({
            type: "POST",
            data: cargo,
            dataType: 'json',
            url: "/Cargo/Salvar",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = `/Cargo/Index/${idSite}`;
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