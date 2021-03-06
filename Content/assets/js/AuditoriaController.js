﻿/*
|--------------------------------------------------------------------------
| Controlador Auditoria
|--------------------------------------------------------------------------
*/

APP.controller.AuditoriaController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexAuditoria") {
            this.indexAuditoria();
        }

    },

    setup: function () {

        //Criar Cliente
        this.buttonSalvar = $(".btn-salvar");

    },

    models: {        
        AnexoModel: APP.model.Anexo,        
    },

    indexAuditoria: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.FileUpload.init();
        APP.component.DragAndDrop.init();
        //$('.carousel').carousel();
        this.setAndHide();
        this.getGestorPorSite();
        this.setValidateForms();
        this.sendFormAuditoria();       

    },

    setAndHide: function () {

        //

    },

    getArquivo: function (_this) {

        var anexoContratoModel = APP.controller.FornecedoresController.models.AnexoModel;
        var anexoAuditoria = {};
        
        var nameImg = $(_this).parent().find('.NomeAnexo').val();
        var id = $(_this).parent().find('.IdAnexo').val() != undefined && $(_this).parent().find('.IdAnexo').val() != null && $(_this).parent().find('.IdAnexo').val() != "" ? $(_this).parent().find('.IdAnexo').val() : "0";
        
        var anexoAuditoria = anexoContratoModel.constructor(
            id,
            nameImg,
            $(_this).parent().find('.anexo-plai').data('b64'),
        );

        return anexoAuditoria;

    },
   
    getGestorPorSite: function () {

        var idSite = $('[name=IdSite]').val();
        var idFuncao = 40; // Funcionalidade(Cadastrar) que permite usuario criar nc
        $.get('/Usuario/ObterUsuariosPorFuncao?idSite=' + idSite + '&idFuncao=' + idFuncao +'', (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name=formAuditoriaGestor] option'), $('[name=formAuditoriaGestor]'), 'IdUsuario', 'NmCompleto');
            }
        });

    },

    setValidateForms: function () {

        var ObjFormPaiValidate = APP.controller.AuditoriaController.getObjObjFormPaiValidate();
        APP.component.ValidateForm.init(ObjFormPaiValidate, '#form-auditoria');

    },

    getObjObjFormPaiValidate: function () {

        var acoesCalendarioFormPaiObj = {
            formAuditoriaGestor: 'required',
        };

        return acoesCalendarioFormPaiObj;

    },

    getObjFormAuditoria: function (_this) {

        var formCriarAuditoriaObj = {
            IdPai: $(_this).find(".formAuditoriaIdPai").val(),
            IdGestor: $('[name=formAuditoriaGestor] :selected').val(),
            Ano: $(_this).find(".pai-ano").find("span").html(),
            Plais: APP.controller.AuditoriaController.getObjFormAuditoriaPlais(_this)
        };

        return formCriarAuditoriaObj;

    },

    getObjFormAuditoriaAno: function () {

        var arrayFormAuditoriaAnoObj = [];

        $('#myCarousel .item').each(function () {
            var hasItem = APP.controller.AuditoriaController.getProcessoMesAno(this);

            if (hasItem) {

                var formAuditoriaAnoObj = {
                    formAuditoriaAno: $(this).find('.pai-ano span').text(),
                    formAuditoriaMeses: APP.controller.AuditoriaController.getObjFormAuditoriaMeses(this),
                };

                arrayFormAuditoriaAnoObj.push(formAuditoriaAnoObj);
            }
        });

        return arrayFormAuditoriaAnoObj;

    },

    getObjFormAuditoriaPlais: function (_this) {

        var arrayFormAuditoriaMesesObj = [];
        $(_this).find('.calendar ul').each(function () {
            var formAuditoriaMesesObj = {
                Mes: $(this).find('[name^=formAuditoriaMes]').val(),
                Arquivo: APP.controller.AuditoriaController.getArquivo(this),
                PlaiProcessoNorma: APP.controller.AuditoriaController.getObjFormAuditoriaMesesProcessos(this),
            };

            arrayFormAuditoriaMesesObj.push(formAuditoriaMesesObj);
        });

        return arrayFormAuditoriaMesesObj;

    },

    getObjFormAuditoriaMesesProcessos: function (e) {

        var arrayFormAuditoriaMesesProcessosObj = [];
        $(e).find('li').each(function () {
            var formAuditoriaMesesProcessosObj = {
                NomeProcesso: $(this).find('span').text(),
            };
            arrayFormAuditoriaMesesProcessosObj.push(formAuditoriaMesesProcessosObj);
        });

        return arrayFormAuditoriaMesesProcessosObj;

    },

    getIdProcessoPorNome: function (_Nome) {
     
        $.ajax({
            type: "GET",
            dataType: 'JSON',
            data: { Nome: _Nome },
            async: false,
            url: '/Auditoria/RetornaProcessoPorNome',
            success: function (result) {
                if (result.StatusCode == 202) {
                    return result.IdProcesso;
                }
                else
                {
                    return 0;
                }
            },
            error: function (result) {
                return 0;
            },
        });
    },

    getProcessoMesAno: function (_this) {

        var itens = $(_this).find('.calendar ul li').size();
        if (itens > 0) {
            return true;
        } else {
            return false;
        }
    },

    //Todos
    sendFormAuditoria: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.AuditoriaController.validateForms();

            //var validate = true;
            if (validate == true) {
                var auditoriaObj = APP.controller.AuditoriaController.getAuditoriaObj();
                APP.controller.AuditoriaController.saveFormAuditoria(auditoriaObj);
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

    getAuditoriaObj: function () {

        var ArrayAuditoriaObj = [];

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                
                if (form == "auditoria")
                {
                    $('.carousel-inner .item').each(function () {
                        var auditoriaObj = APP.controller.AuditoriaController.getObjFormAuditoria(this);                    
                        ArrayAuditoriaObj.push(auditoriaObj);
                    });
                }                
            }

        });

        return ArrayAuditoriaObj;

    },

    saveFormAuditoria: function (auditoriaObj) {

        var erro = "";

        $.ajax({
            type: "POST",
            data: { pais: auditoriaObj },
            dataType: 'json',
            url: "/Auditoria/Salvar",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/Auditoria/Index";
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
