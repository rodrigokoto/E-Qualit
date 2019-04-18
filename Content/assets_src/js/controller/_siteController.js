/*
|--------------------------------------------------------------------------
| Controlador Site
|--------------------------------------------------------------------------
*/

APP.controller.SiteController = {
    
        init: function () {
    
            var page = APP.component.Util.getPage();
            this.setup();
    
            if (page == "IndexSite") {
                this.indexSite();
            }
            if (page == "CriarSite") {
                this.criarSite();
            }
    
        },
    
        setup: function () {
    
            //Criar Site
            this.buttonSalvar = $(".btn-salvar");
    
        },

        //Models
        models: {
            SiteModel: APP.model.Site,
            AnexoModel: APP.model.Anexo,
            ModuloModel: APP.model.Modulo,
            ProcessoModel: APP.model.Processo,
        },
    
        indexSite : function () {
    
            APP.component.DataTable.init('#tb-index-sites');
            APP.controller.SiteController.setMsgIconeAtivo();
            APP.controller.SiteController.setMsgIconeExcluir();
    
        },
    
        setMsgIconeAtivo : function () {
            $('.ativoinativo').on('click', function () {

                var idSite = $(this).data('id');
                var AtivoInativo = $(this).find('i').attr("title");
    
                var msgIconeAtivoAtivar = $('[name=msgIconeAtivoAtivar]').val();
                var msgIconeAtivoInativar = $('[name=msgIconeAtivoInativar]').val();
                
                if (AtivoInativo != "Ativar") {
                    bootbox.confirm(msgIconeAtivoAtivar, function (result) {
                        if (result == true) {
                            APP.controller.SiteController.getMsgIconeAtivo(idSite);
                        }
                    });
                } else {
                    bootbox.confirm(msgIconeAtivoInativar, function (result) {
                        if (result == true) {
                            APP.controller.SiteController.getMsgIconeAtivo(idSite);
                        }
                    });
                }
                
            });
    
        },
    
        getMsgIconeAtivo : function (_idSite) {

            var erro = "";
            var idCliente = $('[name=IdCliente]').val();
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: `/Site/AtivaInativa?idSite=${_idSite}`,
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {

                    if (result.StatusCode == 200) {
                        window.location.href = "/Site/Index/"+idCliente;
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
    
                var idCliente = $(this).data('id');
                var msgIconeExcluir = $('[name=msgIconeExcluir]').val();
    
                bootbox.confirm(msgIconeExcluir, function (result) {
                    if (result == true) {
                        APP.controller.SiteController.getMsgIconeExcluir(idCliente);
                    }
                });
                
            });
    
        },
    
        getMsgIconeExcluir : function (_idCliente) {
    
            var erro = "";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: `/Site/Excluir?id=${_idCliente}`,
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    if (result.StatusCode == 200) {
                        window.location.href = "/Site/Index";
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

        criarSite : function () {
    
            APP.component.AtivaLobiPanel.init();
            APP.component.Datapicker.init();
            APP.component.FileUpload.init();
            APP.component.Mascaras.init();
            this.setValidateForms();
            this.formCriarSite();
            this.formModulos();
            this.formProcessos();
    
            this.sendFormCriarSite();
    
        },
    
        setValidateForms : function () {
    
            var ObjFormCriarSiteValidate = APP.controller.SiteController.getObjFormCriarSiteValidate();
            APP.component.ValidateForm.init(ObjFormCriarSiteValidate, '#form-criar-site');
            
            var ObjFormModulosValidate = APP.controller.SiteController.getObjFormModulosValidate();
            APP.component.ValidateForm.init(ObjFormModulosValidate, '#form-modulos-acesso');
            
            var ObjFormProcessosValidate = APP.controller.SiteController.getObjFormProcessosValidate();
            APP.component.ValidateForm.init(ObjFormProcessosValidate, '#form-processos');
    
        },
    
        //Formulario Criar Site
        formCriarSite : function () {
    
            this.setAndHideCriarSite();
    
        },
    
        setAndHideCriarSite : function () {
    
            //
    
        },
    
        getObjFormCriarSiteValidate : function () {
            var IdSite = $('[name=IdSite]').val();
            var criarClienteFormCriarSiteObj = {
                formCriaSiteLogo: $('[name=formCriaSiteLogo]').data('b64') != "" ? {required: false} : {required: true},
                formCriaSiteNome: {required: true, maxlength: 60},
                formCriaSiteRazaoSocial: {required: true, maxlength: 60},
                formCriaSiteCnpj: {required: true, cnpj: true},
                //formCriaSiteFrase: 'required',
                //formCriaSiteObservacoes: 'required',
            };
    
            return criarClienteFormCriarSiteObj;
    
        },
    
        getObjFormCriarSite : function () {

            var siteModel = APP.controller.SiteController.models.SiteModel;
            var anexoModel = APP.controller.SiteController.models.AnexoModel;
            
            var imgB64 = $('[name=formCriaSiteLogo]').prop('files').length != 0 ? $('[name=formCriaSiteLogo]').prop('files')[0].name : $('[name=formCriaSiteLogoNome]').val();
            var anexo = anexoModel.constructor(
                $('[name=formCriaSiteLogoIdAnexo]').val(),
                imgB64,
                $('[name=formCriaSiteLogo]').data('b64')
            );

            var site =  siteModel.constructor(
                anexo,
                $('[name=IdSite]').val(),
                $('[name=formCriaSiteNome]').val(),
                $('[name=formCriaSiteRazaoSocial]').val(),
                $('[name=formCriaSiteCnpj]').val(),
                $('[name=formCriaSiteFrase]').val(),
                $('[name=formCriaSiteObservacoes]').val(),
                $('[name=formCriaSiteAtivo]').is(':checked'),
                APP.controller.SiteController.getObjFormModulos(site),
                APP.controller.SiteController.getObjFormProcessos(site),
                $('[name=IdCliente]').val()
            );
      

            return site;
            
        },
    
        //Formulario Modulos
        formModulos : function () {
            
            this.setAndHideModulos();
    
        },
        
        setAndHideModulos : function () {
    
            //
    
        },
    
        getObjFormModulosValidate : function () {
    
            var criarClienteFormModulosObj = {
                //formModulosAcessoEscolha: 'required',
            };
    
            return criarClienteFormModulosObj;
    
        },
    
        getObjFormModulos : function (site) {

            var modulo = APP.controller.SiteController.models.ModuloModel;
            var arrSiteFuncionalidades = [];
            $('[name=formModulosAcessoEscolha]:checked').each(function (i) {
                var constructor = modulo.constructor(
                    $('[name=IdSiteFuncionalidade]').val(), 
                    $('[name=IdSite]').val(), 
                    $(this).val()
                );
                arrSiteFuncionalidades.push(constructor);
            });
            
            return arrSiteFuncionalidades;
            
        },
    
        //Formulario Processos
        formProcessos : function () {
            
            this.setAndHideProcessos();
            this.setProcesso();
            this.delProcesso();
    
        },
    
        setAndHideProcessos : function () {
    
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
                html += '<input type="text" name="formProcessosNome" id="form-processos-nome-'+index+'" class="form-control"';
                html += 'placeholder="'+msgProcessosNomePlaceholder+'"';
                html += 'data-msg-required="'+msgProcessosNomeRequired+'"';
                html += 'data-msg-maxlength="'+msgProcessosNomeRequiredMaxlength+'"';
                html += '</div>';
                html += '</td>';
                html += '<td>';
                html += '<div class="form-group checkbox">';
                html += '<input type="checkbox" name="formProcessosAtivo-'+index+'" id="form-processos-ativo-'+index+'" class="form-control" value="true">';
                html += '<label for="form-processos-ativo-'+index+'">'+msgProcessosAtivo+'</label>';
                html += '</div>';
                html += '</td>';
                html += '<td>';
                html += '<a href="#" class="btn-delete-processo icon-cliente trash-color">';
                html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" data-original-title="'+msgIconeExcluir+'"></i>';
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
    
        getObjFormProcessosValidate : function () {
    
            var criarClienteFormProcessosObj = {
                formProcessosNome: 'required',
            };
    
            return criarClienteFormProcessosObj;
    
        },
    
        getObjFormProcessos : function (site) {
    
            var processo = APP.controller.SiteController.models.ProcessoModel;
            var arrProcessos = [];
            $('#tb-processos tbody tr').each(function (i) {
                var constructor = processo.constructor(
                    $(this).find('[name=formProcessosId]').val(),
                    $(this).find('[name=formProcessosNome]').val(),
                    $('[name=IdSite]').val(),
                    $(this).find('[name^=formProcessosAtivo]').is(':checked')
                );
                arrProcessos.push(constructor);
            });
    
            return arrProcessos;
            
        },

        bindProcesso : function () {
    
            APP.controller.SiteController.delProcesso();
            APP.controller.SiteController.setValidateForms();
    
        },
        
        //Todos
        sendFormCriarSite : function () {
    
            this.buttonSalvar.unbind('click');
            this.buttonSalvar.on('click', function () {
    
                var validate = APP.controller.SiteController.validateForms();
    
                //validate = true;
                if (validate == true){
                    APP.controller.SiteController.saveSiteObj();
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
    
        saveSiteObj : function (_getForm) {
    
            var siteObj = {};
    
            $('[id^=panel-form]').each(function () {
                var isVisible = $(this).is(':visible');
                var idPanel = $(this).attr('id').split('-');
                
                if (isVisible) {
                    var form = idPanel[2];
                    switch (form) {
                        case "criarsite":
                            siteObj = APP.controller.SiteController.getObjFormCriarSite();
                            APP.controller.SiteController.saveFormSite(siteObj, "/Site/Salvar/");
                        break;
                    }
                }
    
             });
    
            return siteObj;
    
        },
    
        saveFormSite : function (siteObj, _urlAction) {

            $.ajax({
                type: "POST",
                data: siteObj,
                dataType: 'json',
                url: _urlAction,
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {

                    if (result.StatusCode == 200) {
                        bootbox.alert(result.Success, function (result) {
                            window.location.href="/Site/Index/"+$('[name=IdCliente]').val();
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