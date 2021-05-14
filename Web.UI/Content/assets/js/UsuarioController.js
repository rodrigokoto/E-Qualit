
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


    },

    getMsgIconeAtivo: function (_idUsuario, _idCliente) {

        var erro = "";
        var idSite = $("#IdSite").val();

        $.ajax({
            type: "POST",
            dataType: 'json',
            url: '/Usuario/AtivaInativa?idUsuario=' + _idUsuario,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index/" + _idCliente + "?idSite=" + idSite;
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


    },

    getMsgIconeBloqueia: function (_idUsuario, _idCliente) {

        var erro = "";
        var idSite = $("#IdSite").val();

        $.ajax({
            type: "POST",
            dataType: 'json',
            url: '/Usuario/BloqueiaDesbloqueia?idUsuario=' + _idUsuario,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index/" + _idCliente + "?idSite=" + idSite;
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


    },

    getMsgIconeEmail: function (_idUsuario, _idCliente) {

        var erro = "";
        var idSite = $("#IdSite").val();

        $.ajax({
            type: "POST",
            dataType: 'json',
            url: '/Usuario/RecebeNaoRecebeEmail?idUsuario=' + _idUsuario,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {

                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index/" + _idCliente + "?idSite=" + idSite;
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


    },

    getMsgIconeInativar: function (_idUsuario, _idCliente, _idUsuarioMigracao) {

        var idSite = $("#IdSite").val();

        if (_idUsuarioMigracao == null)
        {
            var erro = "";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: '/Usuario/InativarMigracao?idUsuario=' + _idUsuario + "&idCliente=" + _idCliente,
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    if (result.StatusCode == 200) {
                        window.location.href = "/Usuario/Index/" + _idCliente + "?idSite=" + idSite;
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
        }
        else
        {

            var erro = "";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: '/Usuario/InativarMigracao?idUsuario=' + _idUsuario + "&idUsuarioMigracao=" + _idUsuarioMigracao + "&idCliente=" + _idCliente,
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    if (result.StatusCode == 200) {
                        window.location.href = "/Usuario/Index/" + _idCliente + "?idSite=" + idSite;
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
        }
    },

    getMsgIconeExcluir: function (_idUsuario, _idCliente, _idUsuarioMigracao) {

        var idSite = $("#IdSite").val();

        var erro = "";
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: '/Usuario/Excluir?id=' + _idUsuario + "&idUsuarioMigracao=" + _idUsuarioMigracao,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    window.location.href = "/Usuario/Index/" + _idCliente + "?idSite=" + idSite;
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
        this.sendFormAlterarSenha();

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

            if (codPerfil == perfil.Coordenador) {
                $('#panel-form-sites').show();
                $('#panel-form-cargos').hide();
                url = "/Site/ObterSitesPorCliente";
                APP.controller.UsuarioController.getComboPerfilUsuario(url, "Sites");
            }
            if (codPerfil == perfil.Colaborador) {
                $('#panel-form-cargos').show();
                $('#panel-form-sites').show();
                url = "/Site/ObterSitesPorCliente";
                APP.controller.UsuarioController.getComboPerfilUsuario(url, "Sites");
                APP.controller.UsuarioController.getComboSiteCargoPerfil();


            } else if (codPerfil == perfil.Suporte) {
                $('#panel-form-cargos').hide();
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
    getComboSiteCargoPerfil: function () {

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
            //formCriaUsuarioCpf: { cpf: true },

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
            var usuarioClienteSite = usuarioClienteSiteModel.constructor($('[name=formClienteSitesEscolha]:checked').val(), $(this).val() != null ? $(this).val() : $('[name=idCliente]').val());

            arrayAcoesUsuarioFormCriarUsuarioClienteObj.push(usuarioClienteSite);

        });

        $("[name=formClienteSitesEscolha]:checked").each(function () {

            var _idSite = $(this).data('idsite');
            var usuarioClienteSite = usuarioClienteSiteModel.constructor(_idSite, $('[name=formClienteClientesEscolha]:checked').val() != null ? $('[name=formClienteClientesEscolha]:checked').val() : $('[name=idCliente]').val());

            arrayAcoesUsuarioFormCriarUsuarioSitesObj.push(usuarioClienteSite);

        });

        var idPerfilSelecionado = $('[name=formCriaUsuarioPerfil] :selected').val();

        if (idPerfilSelecionado == 3) {
            var _idSite = $(this).data('idsite');
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);

            var idsiteparam = urlParams.get('IdSite');

            var usuarioClienteSite = usuarioClienteSiteModel.constructor(idsiteparam, $('[name=formClienteClientesEscolha]:checked').val() != null ? $('[name=formClienteClientesEscolha]:checked').val() : $('[name=idCliente]').val());
            arrayAcoesUsuarioFormCriarUsuarioSitesObj.push(usuarioClienteSite);
        }

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
            formAlterarSenhaNova: { required: true, minlength: 6, notEqualsTo: true, strongPassword: false },
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

        var perfill = $('[name=formCriaUsuarioPerfil] :selected').val();

        //if (perfill != "") {
        //    if (!$('#panel-form-clientes input[type="checkbox"]').is(':checked')) {
        //        bootbox.alert("Selecione ao menos um cliente");
        //    }
        //}

        if (valid) {
            if (perfill == "") {
                bootbox.alert("Selecione um perfil.");
                $('[name=formCriaUsuarioPerfil]').focus();
            }
        }

        if (!valid) {
            bootbox.alert("Existem campos de preenchimento obrigatório não informados.");
        }

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
                        APP.controller.UsuarioController.saveFormUsuario(usuarioObj, "/Usuario/Criar/", "cadastro");
                        break;
                    case "editar":
                        usuarioObj = APP.controller.UsuarioController.getObjFormCriarUsuario();
                        APP.controller.UsuarioController.saveFormUsuario(usuarioObj, "/Usuario/Editar/", "editar");
                        break;
                    case "meusdados":
                        usuarioObj = APP.controller.UsuarioController.getObjFormEditarUsuario();
                        APP.controller.UsuarioController.saveFormUsuario(usuarioObj, "/Usuario/AtualizarMeusDados/", "meusdados");
                        break;
                }
            }

        });

    },

    saveFormUsuario: function (usuarioObj, _urlAction, form) {

        var erro = "";
        var idCliente = 0;

        if (form != "meusdados") {
            idCliente = $("[name=idCliente]").val();
            //usuarioObj.UsuarioClienteSites[0].IdCliente;
        }

        var idSite = $("#IdSite").val();

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
                        if (idCliente != 0) {
                            window.location.href = "/Usuario/Index/" + idCliente + "?idSite=" + idSite;
                        }
                        else {
                            bootbox.alert(_options.RegistroSalvoComSucesso);
                        }

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

    getUsuarioMigracaoPorSite: function () {

        APP.component.Loading.showLoading();

        var IdSite = $('#IdSite').val();
        var retorno = [];
        $.ajaxSetup({ async: false });

        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idSite=' + IdSite, (result) => {
            if (result.StatusCode == 200) {
                APP.component.Loading.hideLoading();
                retorno = result.Lista;
            }
        });

        $.ajaxSetup({ async: true });

        return retorno;
    },
};
