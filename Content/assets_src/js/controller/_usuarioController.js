/*
|--------------------------------------------------------------------------
| Controlador Usuario
|--------------------------------------------------------------------------
*/

APP.controller.UsuarioController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexUsuario") {
            this.indexUsuario();
        }
        if (page == "CriarUsuario") {
            this.acoesUsuario();
        }
        if (page == "MeusDados") {
            this.meusDados();
        }
        if (page == "AlterarSenha") {
            this.alterarSenha();
        }

    },

    setup: function () {

        //Criar Usuario
        this.buttonSalvar = $(".btn-salvar");

        //Usuario MeusDados
        this.buttonSaveForm = $("#btnSalvar");
        this.formSenha = $('#form-alterar-senha');
        this.formAlterarSenha = $("#form-alterar-senha");

        //Alterar Senha
        this.buttonAlterarSenha = $('.btn-alterar-senha');

    },

    //Models
    models: {
        UsuarioModel: APP.model.Usuario,
        PerfilUsuarioModel: APP.model.PerfilUsuario,
        UsuarioClienteSiteModel: APP.model.UsuarioClienteSite,
        UsuarioCargoModel: APP.model.UsuarioCargo,
        AnexoModel: APP.model.Anexo,
    },

    //Index Usuario
    indexUsuario: function () {

        APP.component.DataTable.init('#tb-index-usuario');
        APP.controller.UsuarioController.setMsgIconeAtivo();
        APP.controller.UsuarioController.setMsgIconeBloqueia();
        APP.controller.UsuarioController.setMsgIconeEmail();
        APP.controller.UsuarioController.setMsgIconeExcluir();

    },

    setMsgIconeAtivo: function () {
        $('.ativoinativo').on('click', function () {

            var idCliente = $('[name=IdCliente]').val();
            var idUsuario = $(this).data('id');
            var AtivoInativo = $(this).find('i').attr("title");

            var msgIconeAtivoAtivar = $('[name=msgIconeAtivoAtivar]').val();
            var msgIconeAtivoInativar = $('[name=msgIconeAtivoInativar]').val();

            if (AtivoInativo != "Ativo") {
                bootbox.confirm(msgIconeAtivoAtivar, function (result) {
                    if (result == true) {
                        APP.controller.UsuarioController.getMsgIconeAtivo(idUsuario, idCliente);
                    }
                });
            } else {
                bootbox.confirm(msgIconeAtivoInativar, function (result) {
                    if (result == true) {
                        APP.controller.UsuarioController.getMsgIconeAtivo(idUsuario, idCliente);
                    }
                });
            }

        });

    },

    getMsgIconeAtivo: function (_idUsuario, _idCliente) {

        var erro = "";

        $.ajax({
            type: "POST",
            dataType: 'json',
            url: `/Usuario/AtivaInativa?idUsuario=${_idUsuario}`,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index/"+_idCliente;
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

    setMsgIconeBloqueia: function () {
        $('.desbloqueaUsuario').on('click', function () {

            var idCliente = $('[name=IdCliente]').val();
            var idUsuario = $(this).data('id');
            var AtivoInativo = $(this).find('i').attr("title");

            var msgIconeBloqueiaAtivar = $('[name=msgIconeBloqueiaAtivar]').val();
            var msgIconeBloqueiaInativar = $('[name=msgIconeBloqueiaInativar]').val();

            if (AtivoInativo == "Desbloqueado") {
                bootbox.confirm(msgIconeBloqueiaAtivar, function (result) {
                    if (result == true) {
                        APP.controller.UsuarioController.getMsgIconeBloqueia(idUsuario, idCliente);
                    }
                });
            } else {
                bootbox.confirm(msgIconeBloqueiaInativar, function (result) {
                    if (result == true) {
                        APP.controller.UsuarioController.getMsgIconeBloqueia(idUsuario, idCliente);
                    }
                });
            }

        });

    },

    getMsgIconeBloqueia: function (_idUsuario, _idCliente) {

        var erro = "";

        $.ajax({
            type: "POST",
            dataType: 'json',
            url: `/Usuario/BloqueiaDesbloqueia?idUsuario=${_idUsuario}`,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index/"+_idCliente;
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

    setMsgIconeEmail: function () {
        $('.naoRecebeEmail').on('click', function () {

            var idCliente = $('[name=IdCliente]').val();
            var idUsuario = $(this).data('id');
            var AtivoInativo = $(this).find('i').attr("title");

            var msgIconeEmailAtivar = $('[name=msgIconeEmailAtivar]').val();
            var msgIconeEmailInativar = $('[name=msgIconeEmailInativar]').val();

            if (AtivoInativo == "Receber e-mail") {
                bootbox.confirm(msgIconeEmailAtivar, function (result) {
                    if (result == true) {
                        APP.controller.UsuarioController.getMsgIconeEmail(idUsuario, idCliente);
                    }
                });
            } else {
                bootbox.confirm(msgIconeEmailInativar, function (result) {
                    if (result == true) {
                        APP.controller.UsuarioController.getMsgIconeEmail(idUsuario, idCliente);
                    }
                });
            }

        });

    },

    getMsgIconeEmail: function (_idUsuario, _idCliente) {

        var erro = "";

        $.ajax({
            type: "POST",
            dataType: 'json',
            url: `/Usuario/RecebeNaoRecebeEmail?idUsuario=${_idUsuario}`,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index/"+_idCliente;
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

    setMsgIconeExcluir: function () {
        $('.excluir').on('click', function () {

            var idCliente = $('[name=IdCliente]').val();
            var idUsuario = $(this).data('id');
            var msgIconeExcluir = $('[name=msgIconeExcluir]').val();

            bootbox.confirm(msgIconeExcluir, function (result) {
                if (result == true) {
                    APP.controller.UsuarioController.getMsgIconeExcluir(idUsuario, idCliente);
                }
            });

        });

    },

    getMsgIconeExcluir: function (_idUsuario, _idCliente) {

        var erro = "";
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: `/Usuario/Excluir?id=${_idUsuario}`,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index"+_idCliente;
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

    //Criar Usuario
    acoesUsuario: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        this.setAndHide();
        this.setValidateForms();
        this.formCriarUsuario();

        this.sendFormCriarUsuario();

    },

    setAndHide: function () {

        //

    },

    setValidateForms: function () {

        var ObjFormCriarUsuarioValidate = APP.controller.UsuarioController.getObjFormCriarUsuarioValidate();
        APP.component.ValidateForm.init(ObjFormCriarUsuarioValidate, '#form-cria-usuario');

    },

    //Formulario Criar Usuario
    formCriarUsuario: function () {

        this.setAndHideCriarUsuario();
        this.setComboPerfilUsuario();
        this.getComboSiteCargo();

    },

    setAndHideCriarUsuario: function () {
        var idUsuario = $('[name=IdUsuario]').val();
        var idPerfilLogado = $('[name=IdPerfilLogado]').val();

        if (idUsuario == 0) {
            $('#panel-form-clientes').hide();
            $('[name=formClienteClientesEscolha]').prop('checked', false);
            $('#panel-form-sites').hide();
            $('#panel-form-cargos').hide();
        } else {
            if (idPerfilLogado != 1) {
                $('#panel-form-clientes').hide();
            }
        }

    },

    setComboPerfilUsuario: function () {

        var perfil = APP.controller.UsuarioController.models.PerfilUsuarioModel;

        $('[name=formCriaUsuarioPerfil]').on('change', function (e) {
            APP.controller.UsuarioController.setAndHideCriarUsuario();
            var codPerfil = $(this).val();
            var url = "";

            if (codPerfil == perfil.Coordenador || codPerfil == perfil.Colaborador) {

                $('#panel-form-sites').show();
                url = "/Site/ObterSitesPorCliente";
                APP.controller.UsuarioController.getComboPerfilUsuario(url, "Sites");

            } else if (codPerfil == perfil.Suporte) {

                $('#panel-form-clientes').show();
                url = "/Cliente/ObterClientes";
                APP.controller.UsuarioController.getComboPerfilUsuario(url, "Clientes");

            } else {

                APP.controller.UsuarioController.setAndHideCriarUsuario();

            }

        });

    },

    getComboPerfilUsuario: function (_url, _box) {

        var id = _box == "Sites" ? $('[name=idCliente]').val() : null;

        $.ajax({
            type: "GET",
            async: false,
            data: {
                idCliente: id
            },
            dataType: 'json',
            url: _url,
            beforeSend: function () { },
            success: function (result) {
                APP.controller.UsuarioController.setBoxClienteSites(result, _box);
            },
            error: function (result) { },
            complete: function (result) { }
        });

    },

    setBoxClienteSites: function (_result, _box) {

        var resultList = _box != "Clientes" ? _result : _result.Lista;
        var html = '';

        $.each(resultList, function (key, val) {
            var idSiteCliente = _box != "Clientes" ? val.IdSite : val.IdCliente;
            html += '<!-- ' + _box + '-' + idSiteCliente + '-->';
            html += '<div class="col-lg-2 col-md-3 col-sm-6 col-xs-12 checkbox">';
            html += '<input type="hidden" name="idSiteBox" value="' + val.IdSite + '">';
            html += '<input type="hidden" name="nameSiteBox" value="' + val.NmFantasia + '">';
            html += '<input type="checkbox" name="formCliente' + _box + 'Escolha" id="form-cliente-' + _box.toLowerCase() + '-escolha-' + idSiteCliente + '" class="form-control" value="' + idSiteCliente + '" data-idsite="' + val.IdSite + '">';
            html += '<label for="form-cliente-' + _box.toLowerCase() + '-escolha-' + idSiteCliente + '">' + val.NmFantasia + '</label>';
            html += '</div>';
        });

        var boxContent = $('#form-cliente-' + _box.toLowerCase() + '').find('.tabela-check');
        boxContent.html("");
        boxContent.append(html);

        APP.controller.UsuarioController.bind();

    },

    getComboSiteCargo: function () {

        $('[name^=formClienteSitesEscolha]').unbind('change');
        $('[name^=formClienteSitesEscolha]').on('change', function () {

            var idSite = $(this).closest('div').find('[name=idSiteBox]').val();
            var nmSite = $(this).closest('div').find('[name=nameSiteBox]').val();
            var checkedIs = $(this).is(':checked');
            var idPerfilSelecionado = $('[name=formCriaUsuarioPerfil] :selected').val();

            if (checkedIs && idPerfilSelecionado == 4) {
                $.ajax({
                    type: "GET",
                    async: false,
                    data: {
                        idSite: idSite
                    },
                    dataType: 'json',
                    url: "/Cargo/ObterPorSite",
                    beforeSend: function () { },
                    success: function (result) {
                        if (checkedIs) {
                            $('#panel-form-cargos').show();
                            APP.controller.UsuarioController.setBoxSiteCargo(result, idSite, nmSite);
                        }
                    },
                    error: function (result) { },
                    complete: function (result) { }
                });
            } else {
                var boxContentCargos = $('#form-site-cargos [name=idCargoSiteBox-' + idSite + ']').closest('.panel-body');
                boxContentCargos.remove();
                if ($('#form-site-cargos .panel-body').size() == 0) {
                    $('#panel-form-clientes').hide();
                    $('#panel-form-cargos').hide();
                }
            }

        });

    },

    setBoxSiteCargo: function (_result, _idSite, _nmSite) {

        var html = '';
        var idSite = _idSite;
        var nmSite = _nmSite;

        html += '<div class="panel-body">';
        html += '<div class="barra-busca">';
        html += '<input type="hidden" name="idCargoSiteBox-' + idSite + '" value="' + idSite + '">';
        html += '<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">';
        html += '<div class="form-group">';
        html += '<!-- Site -->';
        html += '<label for="form-cria-usuario-escolha">Escolha os Cargos para o site ' + nmSite + '</label>';
        html += '<div class="row tabela-check">';
        html += '<!-- Cargos -->';
        $.each(_result.Cargo, function (key, val) {
            html += '<div class="col-lg-2 col-md-3 col-sm-6 col-xs-12 checkbox">';
            html += '<input type="checkbox" name="formSiteCargos" id="form-site-cargos-' + val.IdCargo + '" class="form-control" value="' + val.IdCargo + '">';
            html += '<label for="form-site-cargos-' + val.IdCargo + '">' + val.NmNome + '</label>';
            html += '</div>';
        });
        html += '</div>';
        html += '</div>';
        html += '</div>';
        html += '</div>';
        html += '</div>';

        var boxContent = $("#form-site-cargos").find('.panel');
        boxContent.append(html);

        APP.controller.UsuarioController.bind();

    },

    bind: function () {

        APP.controller.UsuarioController.getComboSiteCargo();

    },

    getObjFormCriarUsuarioValidate: function () {
        
        var acoesUsuarioFormCriarUsuarioObj = {
            //formCriaUsuarioLogo: $('[name=formCriaUsuarioLogo]').data('b64') != "" ? {required: false} : {required: true},
            formCriaUsuarioResponsavel: { required: true, maxlength: 60 },
            //formCriaUsuarioSexo: 'required',
            formCriaUsuarioEmail: { required: true, maxlength: 60, email: true },
            formCriaUsuarioCpf: { cpf: true },
            //formCriaUsuarioDtExpiracao: 'required',
            //formCriaUsuarioEscolhaEmail: 'required',
            //formCriaUsuarioEscolhaCompartilhado: 'required',
            //formCriaUsuarioEscolhaAtivo: 'required',
            //formCriaUsuarioEscolhaBloqueado: 'required',
        };

        return acoesUsuarioFormCriarUsuarioObj;

    },

    getObjFormCriarUsuario: function () {

        var usuarioModel = APP.controller.UsuarioController.models.UsuarioModel;
        var anexoModel = APP.controller.SiteController.models.AnexoModel;

        var imgB64 = $('[name=formCriaUsuarioLogo]').prop('files').length != 0 ? $('[name=formCriaUsuarioLogo]').prop('files')[0].name : $('[name=formCriaUsuarioLogoNome]').val();

        var anexoUsuario = anexoModel.constructor(
            $('[name=formCriaUsuarioLogoIdAnexo]').val(),
            imgB64,
            $('[name=formCriaUsuarioLogo]').data('b64')
        );
        var usuario = usuarioModel.constructor(
            anexoUsuario,
            $('[name=formCriaUsuarioResponsavel]').val(),
            //$('[name=formCriaUsuarioSexo]:checked').val(),
            $('[name=formCriaUsuarioEmail]').val(),
            $('[name=formCriaUsuarioCpf]').val(),
            $('[name=formCriaUsuarioDtExpiracao]').val(),
            $('[name=formCriaUsuarioPerfil] :selected').val(),
            $('[name=formCriaUsuarioEscolhaEmail]').is(':checked'),
            $('[name=formCriaUsuarioEscolhaCompartilhado]').is(':checked'),
            $('[name=formCriaUsuarioEscolhaAtivo]').is(':checked'),
            $('[name=formCriaUsuarioEscolhaBloqueado]').is(':checked'),
            APP.controller.UsuarioController.getObjFormCriarUsuarioClienteSiteArray(),
            $('[name=IdUsuario]').val(),
            APP.controller.UsuarioController.getObjFormCriarSiteCargoseArray()
        );
        return usuario;

    },

    getObjFormCriarUsuarioClienteSiteArray: function () {
        
        var usuarioClienteSiteModel = APP.controller.UsuarioController.models.UsuarioClienteSiteModel;

        var arrayAcoesUsuarioFormCriarUsuarioClienteObj = [];
        var arrayAcoesUsuarioFormCriarUsuarioSitesObj = [];

        $("[name=formClienteClientesEscolha]:checked").each(function () {
            var usuarioClienteSite = usuarioClienteSiteModel.constructor($('[name=formClienteSitesEscolha]:checked').val(), $('[name=formClienteClientesEscolha]').val() != null ? $('[name=formClienteClientesEscolha]').val() : $('[name=idCliente]').val());

            arrayAcoesUsuarioFormCriarUsuarioClienteObj.push(usuarioClienteSite);

        });

        $("[name=formClienteSitesEscolha]:checked").each(function () {

            var _idSite = $(this).data('idsite');
            var usuarioClienteSite = usuarioClienteSiteModel.constructor(_idSite, $('[name=formClienteSitesEscolha]').val() != null ? $('[name=formClienteSitesEscolha]').val() : $('[name=idCliente]').val());

            arrayAcoesUsuarioFormCriarUsuarioSitesObj.push(usuarioClienteSite);

        });
        
        if (arrayAcoesUsuarioFormCriarUsuarioClienteObj.length > 0) {
            return arrayAcoesUsuarioFormCriarUsuarioClienteObj;
        } else {
            return arrayAcoesUsuarioFormCriarUsuarioSitesObj;
        }

    },

    getObjFormCriarSiteCargoseArray: function () {
        var usuarioCargoModel = APP.controller.UsuarioController.models.UsuarioCargoModel;
        
        var arrayAcoesUsuarioFormCriarUsuarioObj = [];

        var siteCargo = $('[name=formSiteCargos]:checked');

        $(siteCargo).each(function () {
            var usuarioCargo = usuarioCargoModel.constructor($(this).val());
            arrayAcoesUsuarioFormCriarUsuarioObj.push(usuarioCargo);

        });

        return arrayAcoesUsuarioFormCriarUsuarioObj;

    },

    //Meus Dados
    meusDados: function () {

        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        this.formEditarUsuario();
        this.sendFormCriarUsuario();

    },

    formEditarUsuario: function () {

        //

    },

    getObjObjFormEditarUsuarioValidate: function () {

        var meusDadosFormEditarUsuarioObj = {
            //formCriaUsuarioLogo: $('[name=formCriaUsuarioLogo]').data('b64') != "" ? {required: false} : {required: true},
            formCriaUsuarioResponsavel: { required: true, maxlength: 30 },
            //formCriaUsuarioSexo: 'required',
            formCriaUsuarioEmail: { required: true, maxlength: 60, email: true },
            //formCriaUsuarioCpf: 'required',
            //formCriaUsuarioDtExpiracao: 'required',
            //formCriaUsuarioEscolhaEmail: 'required',
            //formCriaUsuarioEscolhaCompartilhado: 'required',
            //formCriaUsuarioEscolhaAtivo: 'required',
            //formCriaUsuarioEscolhaBloqueado: 'required',
        };

        return meusDadosFormEditarUsuarioObj;

    },

    getObjFormEditarUsuario: function () {


        var anexoModel = APP.controller.SiteController.models.AnexoModel;

        var usuario = APP.controller.UsuarioController.models.UsuarioModel;
        var imgB64 = $('[name=formCriaUsuarioLogo]').prop('files').length != 0 ? $('[name=formCriaUsuarioLogo]').prop('files')[0].name : $('[name=formCriaUsuarioLogoNome]').val();

        var anexoUsuario = anexoModel.constructor(
            $('[name=formCriaUsuarioLogoIdAnexo]').val(),
            imgB64,
            $('[name=formCriaUsuarioLogo]').data('b64')
        );

        usuario.FotoPerfilAux = anexoUsuario;
        usuario.NmCompleto = $('[name=formCriaUsuarioResponsavel]').val();
        //usuario.FlSexo = $('[name=formCriaUsuarioSexo]:checked').val();
        usuario.CdIdentificacao = $('[name=formCriaUsuarioEmail]').val();
        usuario.NuCPF = $('[name=formCriaUsuarioCpf]').val();
        usuario.IdUsuario = $('[name=IdUsuario]').val();

        return usuario;

    },

    //Alterar Senha
    alterarSenha: function () {

        this.setValidateFormAleterarSenha();
        this.setSenhaNotEquals();
        this.setStrongPassword();
        this.sendFormAlterarSenha();

    },

    setValidateFormAleterarSenha: function () {

        var ObjFormAlterarSenhaValidate = APP.controller.UsuarioController.getObjObjFormAlterarSenhaValidate();
        APP.component.ValidateForm.init(ObjFormAlterarSenhaValidate, '#form-alterar-senha');

    },

    setStrongPassword: function () {

        APP.component.StrongPassword.init('formAlterarSenhaNova');

    },

    setSenhaNotEquals: function () {

        $.validator.addMethod("notEqualsTo", function () {

            var senhaAtual = $('[name=formAlterarSenhaAtual]').val();
            return ($('[name=formAlterarSenhaNova]').val() != senhaAtual);
        });

    },

    getObjObjFormAlterarSenhaValidate: function () {

        var formAlterarSenhaObj = {
            formAlterarSenhaAtual: { required: true },
            formAlterarSenhaNova: { required: true, minlength: 6, notEqualsTo: true, strongPassword: true },
            formAlterarSenhaConfirmar: { required: true, minlength: 6, notEqualsTo: true, equalTo: "[name=formAlterarSenhaNova]" },
        };

        return formAlterarSenhaObj;

    },

    getObjFormAlterarSenha: function () {

        var alterarSenhaFormLoginObj = {
            //__RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
            Usuario: APP.controller.UsuarioController.getObjFormAlterarSenhaUsuario(),
            NovaSenha: $('[name=formAlterarSenhaNova]').val(),
            ConfirmaSenha: $('[name=formAlterarSenhaConfirmar]').val(),

        };

        return alterarSenhaFormLoginObj;

    },

    getObjFormAlterarSenhaUsuario: function () {

        var formAlterarSenhaObj = {
            IdUsuario: $('[name=formAlterarSenhaIdUsuario]').val(),
            CdSenha: $('[name=formAlterarSenhaAtual]').val(),

        };

        return formAlterarSenhaObj;

    },

    sendFormAlterarSenha: function () {

        this.buttonAlterarSenha.unbind('click');
        this.buttonAlterarSenha.on('click', function () {
            var validate = APP.controller.LoginController.validateForms();

            //var validate = true;
            if (validate == true) {
                var alterarSenhaObj = APP.controller.UsuarioController.getObjFormAlterarSenha();
                APP.controller.UsuarioController.saveFormAlterarSenha(alterarSenhaObj);
            }

        });

    },

    saveFormAlterarSenha: function (alterarSenhaObj) {

        var erro = "";

        $.ajax({
            type: "POST",
            data: alterarSenhaObj,
            dataType: 'json',
            url: "/Usuario/AlterarSenha/",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode === 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/";
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

    //Todos
    sendFormCriarUsuario: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.UsuarioController.validateForms();

            //validate = true;
            if (validate == true) {

                APP.controller.UsuarioController.saveObjUsuario();

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

    saveObjUsuario: function () {

        var usuarioObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "cadastro":
                        usuarioObj = APP.controller.UsuarioController.getObjFormCriarUsuario();
                        APP.controller.UsuarioController.saveFormUsuario(usuarioObj, "/Usuario/Criar/");
                        break;
                    case "editar":
                        usuarioObj = APP.controller.UsuarioController.getObjFormCriarUsuario();
                        APP.controller.UsuarioController.saveFormUsuario(usuarioObj, "/Usuario/Editar/");
                        break;
                    case "meusdados":
                        usuarioObj = APP.controller.UsuarioController.getObjFormEditarUsuario();
                        APP.controller.UsuarioController.saveFormUsuario(usuarioObj, "/Usuario/AtualizarMeusDados/");
                        break;
                }
            }

        });

    },

    saveFormUsuario: function (usuarioObj, _urlAction) {
        
        var erro = "";
        var idCliente = $('[name=idCliente]').val();
        
        $.ajax({
            type: "POST",
            data: usuarioObj,
            dataType: 'json',
            url: _urlAction,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/Usuario/Index/"+idCliente;
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