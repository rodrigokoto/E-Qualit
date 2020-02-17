
/*
|--------------------------------------------------------------------------
| Controlador Licenca
|--------------------------------------------------------------------------
*/

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
        this.buttonDelLicenca = $('.tb-Licenca-excluir');
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

        APP.component.DataTable.init('#tb-licenca');
        this.delLicenca();


    },

    CriarLicenca: function () {


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
            Equipamento: { required: true },
            IdSigla: { required: true },
            Numero: { required: true, number: true },
            Marca: { required: true },
            Modelo: { required: true },
            IdResponsavel: { required: true },
            LocalDeUso: { required: true },
            Escala: { required: true },
            MenorDivisao: { required: true },
            valorAceitacao: { required: true, number: true },
            DescricaoCriterio: { required: false },

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
            DataEmissao: $('#form-parametro-licenca').find('[name=DataEmissao]').val(),
            DataVencimento: $('#form-parametro-licenca').find('[name=DataVencimento]').val(),
            DataProximaNotificacao: $('#form-parametro-licenca').find('[name=DataVencimento]').val(),
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

    delLicenca: function () {


        tabelaLicenca = $('#tb-Licenca').DataTable();

        this.buttonDelLicenca.unbind("click");
        this.buttonDelLicenca.bind('click', function (event) {
            event.preventDefault();

            var IdLicenca = $(this).data('id');
            var $rowAtual = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (result) {
                if (result) {
                    $.ajax({
                        url: "/Licenca/Excluir/",
                        dataType: 'JSON',
                        type: 'POST',
                        data: { id: IdLicenca },
                        success: function (data) {
                            if (data.StatusCode == 200) {
                                tabelaLicenca.row($rowAtual).remove().draw();
                                APP.component.Loading.hideLoading();
                                bootbox.alert({
                                    message: _options.LicencaExcluida                                    
                                })
                                
                            }
                        },
                        beforeSend: function () {
                            APP.component.Loading.showLoading();
                        },
                        
                    });
                }
            });
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

                                        window.location.href = "/Licenca/Editar/" + result.IdLicenca;
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
