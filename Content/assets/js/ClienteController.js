
/*
|--------------------------------------------------------------------------
| Controlador Cliente
|--------------------------------------------------------------------------
*/

APP.controller.ClienteController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexCliente") {
            this.indexCliente();
        }
        if (page == "CriarCliente") {
            this.criarCliente();
        }

    },

    setup: function () {

        //Criar Cliente
        this.buttonSalvar = $(".btn-salvar");

    },

    //Models
    models: {
        ClienteModel: APP.model.Cliente,
        SiteModel: APP.model.Site,
        AnexoModel: APP.model.Anexo,
        ModuloModel: APP.model.Modulo,
        ProcessoModel: APP.model.Processo,
    },

    indexCliente: function () {

        APP.component.DataTable.init('#tb-index-cliente');
        APP.controller.ClienteController.setMsgIconeAtivo();
        APP.controller.ClienteController.setMsgIconeExcluirCliente();

    },

    setMsgIconeAtivo: function () {
        

    },

    getMsgIconeAtivo: function (_idCliente) {

        var erro = "";
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: '/Cliente/AtivaInativa?id='+ _idCliente,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    window.location.href = "/Cliente/Index";
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

    setMsgIconeExcluirCliente: function () {
        

    },

    getMsgIconeExcluirCliente: function (_idCliente) {

        var erro = "";
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: '/Cliente/Excluir?id='+ _idCliente,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    window.location.href = "/Cliente/Index";
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


    criarCliente: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        this.setValidateForms();
        this.formCriarCliente();
        this.formCriarSite();
        this.formModulos();
        this.formProcessos();
        this.formCriarUsuario();

        this.sendFormCriarUsuario();

    },

    setValidateForms: function () {

        var ObjFormCriarClienteValidate = APP.controller.ClienteController.getObjFormCriarClienteValidate();
        APP.component.ValidateForm.init(ObjFormCriarClienteValidate, '#form-cria-cliente');

        var ObjFormCriarSiteValidate = APP.controller.ClienteController.getObjFormCriarSiteValidate();
        APP.component.ValidateForm.init(ObjFormCriarSiteValidate, '#form-cria-site');

        var ObjFormModulosValidate = APP.controller.ClienteController.getObjFormModulosValidate();
        APP.component.ValidateForm.init(ObjFormModulosValidate, '#form-modulos-acesso');

        var ObjFormProcessosValidate = APP.controller.ClienteController.getObjFormProcessosValidate();
        APP.component.ValidateForm.init(ObjFormProcessosValidate, '#form-processos');

        var ObjFormCriarUsuarioValidate = APP.controller.ClienteController.getObjFormCriarUsuarioValidate();
        APP.component.ValidateForm.init(ObjFormCriarUsuarioValidate, '#form-cria-usuario');

    },

    //Formulario Criar Cliente
    formCriarCliente: function () {

        this.setAndHideCriarCliente();

    },

    setAndHideCriarCliente: function () {

        //

    },

    getObjFormCriarClienteValidate: function () {
        var IdCliente = $('[name=IdCliente]').val();
        var criarClienteFormCriarClienteObj = {
            formCriaClienteLogo: $('[name=formCriaClienteLogo]').data('b64') != "" ? { required: false } : { required: true },
            formCriaClienteNome: { required: true, maxlength: 500 },
            formCriaClienteUrl: { required: true, maxlength: 500 },
            formCriaClienteDtValidade: 'required',
            formCriaClienteTrocaSenha: 'required',
            formCriaClienteBloquear: 'required',
            formCriaClienteArmazenar: 'required',
            formCriarClienteContrato: 'required',
        };

        return criarClienteFormCriarClienteObj;

    },


    getObjFormCriarCliente: function () {

        var clienteModel = this.models.ClienteModel;
        var anexoClienteModel = APP.controller.ClienteController.models.AnexoModel;


        var idCliente = $('[name=IdCliente]').val();
        var imgB64 = $('[name=formCriaClienteLogo]').prop('files').length != 0 ? $('[name=formCriaClienteLogo]').prop('files')[0].name : $('[name=formCriaClienteLogoNome]').val();

        var anexoLogoCliente = anexoClienteModel.constructor(
            $('[name=formCriaClienteLogoIdAnexo]').val(),
            imgB64,
            $('[name=formCriaClienteLogo]').data('b64')
        );

        var cliente = clienteModel.constructor(
            anexoLogoCliente,
            $('[name=formCriaClienteNome]').val(),
            $('[name=formCriaClienteUrl]').val(),
            $('[name=formCriaClienteDtValidade]').val(),
            $('[name=formCriaClienteTrocaSenha] :selected').val(),
            $('[name=formCriaClienteBloquear] :selected').val(),
            $('[name=formCriaClienteArmazenar] :selected').val(),
            APP.controller.ClienteController.getAnexosContratos(),
            $('[name=formCriaClienteAtivo]:checked').val(),
            $('[name=formCriaClienteCaptcha]:checked').val(),
            $('[name=formCriaClienteSenha]:checked').val(),
            idCliente == 0 ? APP.controller.ClienteController.getObjFormCriarUsuario() : null,
            idCliente
        );

        return cliente;

    },

    getAnexosContratos: function () {

        var anexoContratoModel = APP.controller.ClienteController.models.AnexoModel;
        var arrayAnexoContrato = [];

        $('.dashed li a:first-child').each(function () {

            var nameImg = $(this).text();
            var anexoContratoCliente = anexoContratoModel.constructor(
                $(this).closest('li').find('[name=formCriaClienteContratoIdAnexo]').val(),
                nameImg,
                $(this).data('b64')
            );

            arrayAnexoContrato.push(anexoContratoCliente);
        });

        return arrayAnexoContrato;

    },

    //Formulario Criar Site
    formCriarSite: function () {

        this.setAndHideCriarSite();

    },

    setAndHideCriarSite: function () {

        //

    },

    getObjFormCriarSiteValidate: function () {

        var criarClienteFormCriarSiteObj = {
            formCriaSiteLogo: $('[name=formCriaSiteLogo]').data('b64') != "" ? { required: false } : { required: true },
            formCriaSiteNome: { required: true, maxlength: 500 },
            formCriaSiteRazaoSocial: { required: true, maxlength: 500 },
            formCriaSiteCnpj: { required: true, cnpj: true },
            //formCriaSiteFrase: 'required',
            //formCriaSiteObservacoes: 'required',
        };

        return criarClienteFormCriarSiteObj;

    },

    getObjFormCriarSite: function () {

        return APP.controller.SiteController.getObjFormCriarSite();

    },

    //Formulario Modulos
    formModulos: function () {

        this.setAndHideModulos();
        //APP.component.ValidateForm.verifyGroupValidateChecked('formModulosAcessoEscolha');

    },

    setAndHideModulos: function () {

        //

    },

    getObjFormModulosValidate: function () {

        var criarClienteFormModulosObj = {
            //formModulosAcessoEscolha: {required: true, groupValidateChecked: true},
        };

        return criarClienteFormModulosObj;

    },

    getObjFormModulos: function () {

        var arrayCriarClienteFormModulosObj = [];
        $('[name=formModulosAcessoEscolha]:checked').each(function (i) {
            var criarClienteFormModulosObj = {
                IdFuncionalidade: $(this).val(),
            };
            arrayCriarClienteFormModulosObj.push(criarClienteFormModulosObj);
        });
        return arrayCriarClienteFormModulosObj;

    },

    //Formulario Processos
    formProcessos: function () {

        this.setAndHideProcessos();
        this.setProcesso();
        this.delProcesso();

    },

    setAndHideProcessos: function () {

        //

    },

    setProcesso: function () {

        $('.btn-add-processo').unbind('click');
        $('.btn-add-processo').on('click', function () {

            var msgProcessosNomeLabel = $('[name=msgProcessosNomeLabel]').val();
            var msgProcessosNomePlaceholder = $('[name=msgProcessosNomePlaceholder]').val();
            var msgProcessosNomeRequired = $('[name=msgProcessosNomeRequired]').val();
            var msgProcessosNomeRequiredMaxlength = $('[name=msgProcessosNomeRequiredMaxlength]').val();
            var msgProcessosAtivo = $('[name=msgProcessosAtivo]').val();
            var msgIconeExcluir = $('[name=msgIconeExcluir]').val();

            var index = $('#tb-processos tbody tr').size();

            var html = '';
            html += '<tr role="row" class="odd">';
            html += '<td>';
            html += '<div class="form-group">';
            html += '<input type="text" name="formProcessosNome" id="form-processos-nome-' + index + '" class="form-control"';
            html += 'placeholder="' + msgProcessosNomePlaceholder + '"';
            html += 'data-msg-required="' + msgProcessosNomeRequired + '"';
            html += 'data-msg-maxlength="' + msgProcessosNomeRequiredMaxlength + '"';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<div class="form-group checkbox">';
            html += '<input type="checkbox" name="formProcessosAtivo-' + index + '" id="form-processos-ativo-' + index + '" class="form-control" value="true">';
            html += '<label for="form-processos-ativo-' + index + '">' + msgProcessosAtivo + '</label>';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<a href="#" class="btn-delete-processo icon-cliente trash-color">';
            html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</a>';
            html += '</td>';
            html += '</tr>';

            $('#tb-processos tbody').append(html);
            APP.controller.ClienteController.bindProcesso();

        });

    },

    delProcesso: function () {

        $('.btn-delete-processo').on('click', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
        });

    },

    getObjFormProcessosValidate: function () {

        var criarClienteFormProcessosObj = {
            formProcessosNome: { required: true, maxlength: 60 }
        };

        return criarClienteFormProcessosObj;

    },

    getObjFormProcessos: function () {

        var arrayCriarClienteFormProcessosObj = [];
        var table = $('#tb-processos tbody tr');
        table.each(function () {

            var criarClienteFormProcessosObj = {
                Nome: $(this).find('[name=formProcessosNome]').val(),
                FlAtivo: $(this).find('[name^=formProcessosAtivo]').is(':checked'),
            };

            arrayCriarClienteFormProcessosObj.push(criarClienteFormProcessosObj);

        });

        return arrayCriarClienteFormProcessosObj;

    },

    bindProcesso: function () {

        APP.controller.ClienteController.delProcesso();
        APP.controller.ClienteController.setValidateForms();

    },

    //Formulario Criar Usuario
    formCriarUsuario: function () {

        this.setAndHideCriarUsuario();

    },

    setAndHideCriarUsuario: function () {

        //

    },

    getObjFormCriarUsuarioValidate: function () {

        var criarClienteFormCriarUsuarioObj = {
            //formCriaUsuarioLogo: {required: true, extension: "jpg|jpeg|png|gif"},
            formCriaUsuarioResponsavel: { required: true, maxlength: 500 },
            //formCriaUsuarioSexo: 'required',
            formCriaUsuarioEmail: { required: true, maxlength: 500, email: true },
            formCriaUsuarioCpf: { required: true, cpf: true },
            formCriaUsuarioDtExpiracao: 'required',
            //formCriaUsuarioEscolhaEmail: 'required',
            //formCriaUsuarioEscolhaCompartilhado: 'required',
            //formCriaUsuarioEscolhaAtivo: 'required',
            //formCriaUsuarioEscolhaBloqueado: 'required',
        };

        return criarClienteFormCriarUsuarioObj;

    },

    getObjFormCriarUsuario: function () {

        return APP.controller.UsuarioController.getObjFormCriarUsuario();

    },

    //Todos
    sendFormCriarUsuario: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.ClienteController.validateForms();
            var typeSave = $('[name=IdCliente]').val() > 0 ? 'edit' : 'create';

            //validate = true;
            if (validate == true) {

                var clienteObj = APP.controller.ClienteController.getCriarClienteObj();
                if (typeSave == 'create') {
                    APP.controller.ClienteController.saveFormCriarCliente(clienteObj);
                } else {
                    APP.controller.ClienteController.saveFormEditarCliente(clienteObj);
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

    getCriarClienteObj: function () {

        var clienteObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];

                switch (form) {
                    case "cliente":
                        clienteObj = APP.controller.ClienteController.getObjFormCriarCliente();
                        break;
                    case "site":
                        clienteObj.Site = APP.controller.ClienteController.getObjFormCriarSite();
                        break;
                }
            }

        });

        return clienteObj;

    },

    saveFormCriarCliente: function (clientes) {

        var erro = "";
        $.ajax({
            type: "POST",
            data: clientes,
            dataType: 'json',
            url: "/Cliente/Criar",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/Cliente/Index";
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

    saveFormEditarCliente: function (clientes) {

        var erro = "";
        $.ajax({
            type: "POST",
            data: clientes,
            dataType: 'json',
            url: "/Cliente/Editar",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/Cliente/Index";
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
