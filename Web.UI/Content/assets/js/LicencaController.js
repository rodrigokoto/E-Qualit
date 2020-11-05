
/*
|--------------------------------------------------------------------------
| Controlador Licenca
|--------------------------------------------------------------------------
*/


  $('.excluir').on('click', function (event) {
        event.preventDefault();
        var idLicenca = $(this).data('id');

        bootbox.dialog({
            message: _options.MsgDesejaExcluirRegistro,
            onEscape: true,
            backdrop: true,
            buttons: {

                Cancelar: {
                    label: 'Cancelar',
                    className: 'btn-primary',
                    callback: function (e) {
                        e.preventDefault;
                        bootbox.hideAll();
                    }
                },
                Ok: {
                    label: 'Ok',
                    className: 'btn-primary',
                    callback: function (e) {
                        APP.controller.LicencaController.getMsgIconeExcluir(idLicenca);
                    }
                }
            }
        });
    });

var patternValidaData = /^(((0[1-9]|[12][0-9]|3[01])([-.\/])(0[13578]|10|12)([-.\/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-.\/])(0[469]|11)([-.\/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-.\/])(02)([-.\/])(\d{4}))|((29)(\.|-|\/)(02)([-.\/])([02468][048]00))|((29)([-.\/])(02)([-.\/])([13579][26]00))|((29)([-.\/])(02)([-.\/])([0-9][0-9][0][48]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][2468][048]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][13579][26])))$/;

APP.controller.LicencaController = {


    init: function () {
        var page = APP.component.Util.getPage();



        this.setup();
        if (page == "IndexLicencas") {
            this.IndexLicenca();
        } else if (page == "CriarLicencas") {
            this.CriarLicenca();
        }

    },

    setup: function () {

        //Licenca Index

        //Licenca Criar
        this.buttonDelLicenca = $('.tb-Licenca-excluir');
        this.buttonSaveForm = $(".form-save");
        this.buttonModalCancelar = $(".btn-modal-cancelar");
    },

    models: {
        ClienteModel: APP.model.Cliente,
        SiteModel: APP.model.Site,
        AnexoModel: APP.model.Anexo,
        ModuloModel: APP.model.Modulo,
        ProcessoModel: APP.model.Processo,
    },

    //Chamadas
    IndexLicenca: function () {

        APP.component.DataTable.init('#tb-Licenca');
        this.delLicenca();

    },

    CriarLicenca: function () {

        $("#form-cadastro-dt-emissao").datepicker({
            startView: "months",
            minViewMode: "months"
        });

        //GETs
        this.getResponsavel();
        //SETs
        this.setComboProcesso();
        this.getChangeComboProcesso();
        //DELs
        this.saveForms();
        //OUTRAS

        //INCLUDEs
        APP.component.AtivaLobiPanel.init();
        APP.component.FileUpload.init();
        APP.component.Datapicker.init();
        APP.component.DataTable.init('#tb-Licenca');
        // APP.controller.LicencaController.setupUploadArquivoLicenca();
        //OUTRASfimp

        this.setValidateForms();

    },


    setValidateForms: function () {


        var ObjFormLicencaValidate = APP.controller.LicencaController.getObjObjFormLicencaValidate();
        APP.component.ValidateForm.init(ObjFormLicencaValidate, '#form-parametro-licenca');

    },

    getObjObjFormLicencaValidate: function () {


        var acoesPadraoFormCriarPadraoObj = {
            //formCriaClienteLogo: {required: true, minFiles: 1},
            Idlicenca: { required: true },
            Titulo: { required: true },
            IdProcesso: { required: true },
            IdResponsavel: { required: true },
            ArquivosLicencaAnexos: { required: false },
            DataEmissao: { required: false },
            DataVencimento: { required: false },
            DataProximaNotificacao: { required: false },

        };

        return acoesPadraoFormCriarPadraoObj;

    },

    //Funcoes Index Licenca

    getResponsavel: function () {


        var idSite = $('#IdSite').val();
        var idFuncao = 57;

        var url = '/Usuario/ObterUsuariosPorFuncao?idSite=' + idSite + '&idFuncao=' + idFuncao;
        $.ajax({
            type: "GET",
            async: false,
            dataType: 'json',
            url: url,
            success: function (result) {
                $.each(result.Lista, function (key, val) {
                    $('#dropdown-responsavel').append($("<option />").val(val.IdUsuario).text(val.NmCompleto));
                });
            }
        });
    },

    getObjLicenca: function () {


        Licenca = {
            Idlicenca: $('[name=Idlicenca]').val(),
            Titulo: $('[name=Titulo]').val(),
            IdProcesso: $('[name=formCadastroProcesso] option:selected').val(),
            IdResponsavel: $('[name=IdResponsavel] option:selected').val(),
            ArquivosLicencaAnexos: APP.controller.LicencaController.getAnexosArquivosLicencaAnexos(),
            DataCriacao: $('[name=DataCriacao]').val(),
            DataEmissao: $('#form-parametro-licenca').find('[name=DataEmissao]').val(),
            DataVencimento: $('#form-parametro-licenca').find('[name=DataVencimento]').val(),
            DataProximaNotificacao: $('#form-parametro-licenca').find('[name=DataProximaNotificacao]').val(),
            Obervacao: $('#form-parametro-licenca').find('[name=Obervacao]').val(),

        };

        return Licenca;
    },

    getArquivoCertificadoAnexos: function (IdLicenca) {
        //componente antigo
        var anexoContratoModel = APP.controller.ClienteController.models.AnexoModel;
        var arrayAnexoArquivoCertificado = [];

        $('.dashed li a:first-child').each(function () {

            var nameImg = $(this).text();
            var anexoContratoCliente = anexoContratoModel.constructor(
                $(this).closest('li').find('[name=formCriaClienteContratoIdAnexo]').val(),//IdAnexo
                nameImg,//Nome
                $(this).data('b64')//B64
            );

            arrayAnexoArquivoCertificado.push(anexoContratoCliente);
        });

        return arrayAnexoArquivoCertificado;
    },

    getArquivoCertificadoAnex2: function (IdLicenca) {
        let raiz = $("#modal-raiupacaoimeidata" + IdLicenca)[0];
        let ret = FileUploadGlobal_getArrArquivoRaiz(raiz, "IdArquivoCertificadoAnexo", "IdLicenca");
        return ret;

    },

     
    
    
  
 

    getMsgIconeExcluir: function (_idLicenca) {
        var erro = "";
        var idSite = $('[name=IdSite]').val();
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: '/Licenca/Excluir?id=' + _idLicenca,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    window.location.href = "/Licenca/Index/";
           

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
                bootbox.alert("Licença excluida com sucesso");

            }
        });
    },

    getAnexosArquivosLicencaAnexos() {
        let raiz = $("#modal-rai" + "ncabeca")[0];
        let ret = FileUploadGlobal_getArrArquivoRaiz(raiz, "IdArquivoNaoConformidadeAnexo", "IdRegistroConformidade");
        return ret;
    },

    setComboProcesso: function () {

        var idSite = $('#IdSite').val();
        var data = {
            "idSite": idSite
        };

        $.ajax({
            type: "GET",
            dataType: 'JSON',
            data: data,
            url: '/Processo/ListaProcessosPorSite',
            success: function (result) {
                if (result.StatusCode == 200) {
                    APP.component.SelectListCompare.init(result.Lista, $('[name=formCadastroProcesso] option'), '#form-cadastro-processo', 'IdProcesso', 'Nome');
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
        });

    },


    getChangeComboProcesso: function () {

        $('#form-cadastro-processo').on('change', function () {
            var IdProcesso = $('#IdProcesso');

            IdProcesso.val($(this).val());
            if (IdProcesso.val() == 0) {
                IdProcesso.val(null);
            }
        });

    },

    sendFormParametroLicenca: function () {

        APP.component.Loading.showLoading();

        var vstatus = document.getElementById('IdStatusHidden');

        $.ajaxSetup({ async: false });
        var idLicenca = $('[name=Idlicenca]').val();
        var url = '/Licenca/Criar';
        var validacao = true;
        var dataobject = APP.controller.LicencaController.getObjLicenca();
        var data = JSON.stringify(dataobject);

        if (idLicenca != "0") {
            url = '/Licenca/Editar';
        } else {
            validacao = $('#form-parametro-licenca').valid();
        }

        var dtEmissao = $('#form-parametro-licenca').find('[name=DataEmissao]').val();
        var dtVencimento = $('#form-parametro-licenca').find('[name=DataVencimento]').val();
        var dtProximaNotificacao = $('#form-parametro-licenca').find('[name=DataProximaNotificacao]').val();

        if (dtEmissao !== "") {
            if (!patternValidaData.test(dtEmissao)) {
                APP.component.Loading.hideLoading();
                bootbox.alert("Há datas em formato inválido");
                validacao = false;
            }
        }
        if (dtVencimento !== "") {
            if (!patternValidaData.test(dtVencimento)) {
                APP.component.Loading.hideLoading();
                bootbox.alert("Há datas em formato inválido");
                validacao = false;
            }
        }
        if (dtProximaNotificacao !== "") {
            if (!patternValidaData.test(dtProximaNotificacao)) {
                APP.component.Loading.hideLoading();
                bootbox.alert("Há datas em formato inválido");
                validacao = false;
            }
        }

        if (validacao) {
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: url,
                data: dataobject,
                success: function (result) {

                    if (result.StatusCode == 200) {

                        if (idLicenca == "0") {
                            $("#IdLicenca").val(result.IdLicenca);
                            bootbox.alert(
                                {
                                    message: result.Success,
                                    callback: function () {

                                        window.location.href = "/Licenca/Index/";
                                    }
                                });
                            return;

                        }
                        else {
                            if (idLicenca != "0") {
                                bootbox.alert({
                                    message: result.Success,
                                    callback: function () {
                                        window.location.href = "/Licenca/Index";
                                    }
                                });
                            }
                        }



                    } else if (result.StatusCode == 505) {
                        erro = APP.component.ResultErros.init(result.Erro);
                        bootbox.alert(erro);

                    } else if (result.StatusCode == 500) {
                        erro = APP.component.ResultErros.init(result.Erro);
                        bootbox.alert(erro);
                    }

                },
                error: function (result) {
                    bootbox.alert(_options.MsgOcorreuErro);
                },
                complete: function (result) {

                },
            });
        }

        $.ajaxSetup({ async: true });
        APP.component.Loading.hideLoading();
    },

    saveForms: function () {


        this.buttonSaveForm.click(function () {

            event.preventDefault();

            $("#form-parametro-licenca").validate();

            var formLicencaValido = $("#form-parametro-licenca").valid();

            var objPosLicenca = APP.controller.LicencaController.getObjLicenca();

            if (formLicencaValido) {

                APP.controller.LicencaController.sendFormParametroLicenca();

                $('.tb-Licenca').slideDown(500);

            }
        });
    },

    setupUploadArquivoLicenca: function () {


        var formJson = { "id": 0, "idSite": $("#IdSite").val() };
        UploadMultiplosArquivos(
            "/Licenca/UploadArquivo",
            "dropPE",
            "uplPE",
            "ArquivoCertificado",
            30000000000,
            formJson,
            "form-pos-Licenca",
            $("#IdSite").val(),
            "/Licenca/RemoverArquivo");
    },

};
