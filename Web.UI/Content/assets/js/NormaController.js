
/*
|--------------------------------------------------------------------------
| Controlador Norma
|--------------------------------------------------------------------------
*/

APP.controller.NormaController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexNorma") {
            this.indexNorma();
        }

    },

    setup: function () {

        //Criar Pagina
        this.buttonSalvar = $(".btn-salvar");

        //Form Norma
        this.buttonAddNewNormaFormNorma = $('.novo');
        this.buttonSaveNewNormaFormNorma = $('.save-norma');
        this.buttonEditNewNormaFormNorma = $('.edit-norma');
        this.buttonDelNewNormaFormNorma = $('.del-norma');
        this.buttonActiveNewNormaFormNorma = $('.active-norma');

    },

    //Index
    indexNorma: function () {

        APP.component.DataTable.init('#tb-index-norma');
        this.acoesNorma();

    },

    //Acoes
    acoesNorma: function () {

        APP.component.AtivaLobiPanel.init();

        this.setAndHide();
        this.setValidateForms();
        this.formCriarNorma();

    },

    setAndHide: function () {

        $('#tb-index-norma tbody').find('input').prop('disabled', true);
        $('#tb-index-norma tbody').find('select').prop('disabled', true);

    },

    setValidateForms: function () {

        var ObjFormNormaValidate = APP.controller.NormaController.getObjObjFormNormaValidate();
        APP.component.ValidateForm.init(ObjFormNormaValidate, '#form-norma');

    },

    //Formulario Criar Norma
    formCriarNorma: function () {

        this.setAndHideCriarNorma();
        this.setNewNormaFormNorma();
        this.setSaveNormaFormNorma();
        this.setEditNormaFormNorma();
        this.setActiveNormaFormNorma();
        this.setDelNormaFormNorma();

    },

    setAndHideCriarNorma: function () {

        var empty = $('#tb-index-norma tbody tr td').hasClass('dataTables_empty');
        if (empty) {
            $('.odd').remove();
        }

    },

    setNewNormaFormNorma: function () {

        this.buttonAddNewNormaFormNorma.unbind('click');
        this.buttonAddNewNormaFormNorma.on('click', function () {
            event.preventDefault();

            var index = $('#tb-index-norma tbody tr').size();

            var html = '';
            html += '<!-- Norma -->';
            html += '<tr role="row">';
            html += '<td>';
            html += '<div class="form-group">';
            html += '<input type="text" class="form-control" id="form-norma-item" name="formNormaItem" placeholder= '+ _options.Item +' data-msg-required="" value="-" disabled/>';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<div class="form-group">';
            html += '<input type="text" class="form-control" id="form-norma-codigo" name="formNormaCodigo" placeholder='+ _options.Codigo +' data-msg-required="" value="" />';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<div class="form-group" style="width: 100%;">';
            html += '<input type="text" class="form-control" id="form-norma-titulo" name="formNormaTitulo" placeholder='+ _options.Titulo +' data-msg-required="" value=""/>';
            html += '</div>';
            html += '</td>';
            html += '<td class="text-nowrap">';
            html += '<a href="#" class="save-norma ativo-color">';
            html += '<i class="fa fa-check" aria-hidden="true" data-toggle="tooltip" data-original-title='+ _options.buttonSalvar +'></i>';
            html += '</a>';
            html += '<a href="#" class="edit-norma editar-color">';
            html += '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            html += '</a>';
            html += '<a href="#" class="del-norma trash-color">';
            html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</a>';
            html += '<a href="#" class="active-norma ativo-color">';
            html += '<i class="fa fa-circle" aria-hidden="true" data-toggle="tooltip" data-original-title= '+_options.btn_lbl_desativar+' ></i>';
            html += '</a>';
            html += '</td>';
            html += '</tr>';


            $('#tb-index-norma tbody').append(html);

            APP.controller.NormaController.bindFormNorma();

        });

    },

    setSaveNormaFormNorma: function () {

        this.buttonSaveNewNormaFormNorma.unbind('click');
        this.buttonSaveNewNormaFormNorma.on('click', function () {
            event.preventDefault();

            APP.controller.NormaController.sendFormNorma(this, 'save');

        });

    },

    setEditNormaFormNorma: function () {

        this.buttonEditNewNormaFormNorma.unbind('click');
        this.buttonEditNewNormaFormNorma.on('click', function () {
            event.preventDefault();

            $(this).closest('tr').find('[name^=formNormaItem]').prop('disabled', true);
            $(this).closest('tr').find('[name^=formNormaCodigo]').prop('disabled', false);
            $(this).closest('tr').find('[name^=formNormaTitulo]').prop('disabled', false);

        });

    },

    setDelNormaFormNorma: function () {

        $('.del-norma').unbind('click');
        $('.del-norma').on('click', function () {
            event.preventDefault();

            var idNorma = $(this).data('id-norma');
            var msgIconeExcluir = $('[name=msgIconeExcluir]').val();
            var _this = this;

            bootbox.confirm(msgIconeExcluir, function (result) {
                if (result == true) {

                    APP.controller.NormaController.sendFormNorma(_this, 'del');
                }
            });

        });

    },

    setActiveNormaFormNorma: function () {

        $('.active-norma').unbind('click');
        $('.active-norma').on('click', function () {
            event.preventDefault();

            var idNorma = $(this).data('id-norma');
            var AtivoInativo = $(this).find('i').attr("title");

            var msgIconeAtivoAtivar = $('[name=msgIconeAtivoAtivar]').val();
            var msgIconeAtivoInativar = $('[name=msgIconeAtivoInativar]').val();
            var _this = this;

            if (AtivoInativo != _options.labelButtonAtivar) {
                bootbox.confirm(msgIconeAtivoAtivar, function (result) {
                    if (result == true) {
                        APP.controller.NormaController.sendFormNorma(_this, 'active');
                    }
                });
            } else {
                bootbox.confirm(msgIconeAtivoInativar, function (result) {
                    if (result == true) {
                        APP.controller.NormaController.sendFormNorma(_this, 'active');
                    }
                });
            }

        });

    },

    bindFormNorma: function () {

        APP.controller.NormaController.setup();
        APP.controller.NormaController.setNewNormaFormNorma();
        APP.controller.NormaController.setSaveNormaFormNorma();
        APP.controller.NormaController.setEditNormaFormNorma();
        APP.controller.NormaController.setDelNormaFormNorma();
        APP.controller.NormaController.setActiveNormaFormNorma();
        APP.controller.NormaController.setValidateForms();

    },

    getObjObjFormNormaValidate: function () {

        var acoesNormaFormNormaObj = {
            formNormaCodigo: 'required',
            formNormaTitulo: 'required',
        };

        return acoesNormaFormNormaObj;

    },

    getObjFormNorma: function (e) {

        var acoesNormaFormNormaObj = {
            IdSite: $('[name=IdSite]').val(),
            IdNorma: $(e).closest('tr').find('[name^=formNormaItem]').attr("IdNorma"),
            DataCadastro: $(e).closest('tr').find('[name^=formNormaDtCadastro]').val(),
            Codigo: $(e).closest('tr').find('[name^=formNormaCodigo]').val(),
            Titulo: $(e).closest('tr').find('[name^=formNormaTitulo]').val(),
            Ativo: $(e).closest('tr').find('[class^=active-norma]').val(),
        };

        return acoesNormaFormNormaObj;

    },

    //Todos
    sendFormNorma: function (e, _action) {

        var validate = APP.controller.NormaController.validateForms();

        //var validate = true;
        if (validate == true) {
            
            switch (_action) {
                case 'save':
                    var normaObj = APP.controller.NormaController.getObjFormNorma(e);
                    APP.controller.NormaController.saveFormNorma(normaObj);
                    break;
                case 'del':
                    APP.controller.NormaController.delFormNorma(e);
                    break;
                case 'active':
                    var normaObj = APP.controller.NormaController.getObjFormNorma(e);
                    APP.controller.NormaController.activeFormNorma(normaObj);
                    break;
            }

        }

    },

    validateForms: function () {

        var valid = true;
        var validate = $('#form-norma').valid();
        if (validate != true) {
            valid = false;
        }

        return valid;

    },

    saveFormNorma: function (normaObj) {

        $.ajax({
            type: "POST",
            data: normaObj,
            dataType: 'json',
            url: "/Norma/Salvar",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.reload([true]);
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

    delFormNorma: function (e) {


        event.preventDefault();
        IdNorma = $(e).closest('tr').find('[name^=formNormaItem]').attr("IdNorma");
        
        $.ajax({
            type: "GET",            
            url: "/Norma/Excluir/?Id=" + IdNorma,
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

    activeFormNorma: function (normaObj) {

        $.ajax({
            type: "POST",
            data: normaObj,
            dataType: 'json',
            url: "/Norma/AtivaInativa",
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

};
