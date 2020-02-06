
/*
|--------------------------------------------------------------------------
| Controlador Licenca
|--------------------------------------------------------------------------
*/

APP.controller.LicencaController = {

    init: function () {
        var page = APP.component.Util.getPage();

        this.setup();
        if (page == "IndexLicenca") {
            this.IndexLicenca();
        } else if (page == "CriarLicenca") {
            this.CriarLicenca();
        }

    },

    setup: function () {

        //Instrumento Index

        //Instrumento Criar
        this.buttonDelLicenca = $('.tb-Licenca-excluir');
        this.buttonSaveForm = $(".form-save");
        this.buttonDelInstrumento = $('.tb-instrumento-excluir');
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

        APP.component.DataTable.init('#tb-instrumento');
        this.delInstrumento();


    },

    CriarLicenca: function () {


        //GETs
        this.getResponsavel();
        
        //SETs
        this.setComboProcesso();
        //DELs
        this.delLicenca();
        this.delParametroLicenca();
        this.saveForms();
        //OUTRAS
        this.viewArquivoLicenca();
        
        //INCLUDEs
        APP.component.AtivaLobiPanel.init();
        APP.component.FileUpload.init();
        APP.component.Datapicker.init();
        APP.component.DataTable.init('#tb-Licenca');
        // APP.controller.LicencaController.setupUploadArquivoLicenca();
        //OUTRASfimp
        this.changeCadastroSigla();
        this.setLicencaVisivel();
        this.setValidateForms();

    },



    setValidateForms: function () {


        var ObjFormInstrumentoValidate = APP.controller.LicencaController.getObjObjFormInstrumentoValidate();
        APP.component.ValidateForm.init(ObjFormInstrumentoValidate, '#form-parametro-licenca');

    },

    getObjObjFormInstrumentoValidate: function () {


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


        var Licenca = {



            IdLicenca: $('[name=Titulo]').val(),



            IdFilaEnvio: $("#IdFilaEnvio").val(),
            IdInstrumento: $('#IdInstrumento').val(), 'required': true, 'minlength': 1, 'maxlength': 500,
            IdSigla: $('[name=IdSigla]').val(),
            DataRegistro: $('#form-pos-Licenca').find('[name=DtRegistro]').val(),
            DataNotificacao: $('#form-pos-Licenca').find('[name=DtNotificacao]').val(),
            DataProximaLicenca: $('#form-pos-Licenca').find('[name=DataProximaLicenca]').val(),
            Certificado: $('#form-pos-Licenca').find('[name=Certificado]').val(),
            CriterioAceitacao: [],
            OrgaoCalibrador: $('#form-pos-Licenca').find('[name=OrgaoCalibrador]').val(),
            Aprovador: $('#form-pos-Licenca').find('[name=Aprovador]').val(),
            Aprovado: $('input[name="Aprovado"]:checked').val(),
            ArquivoCertificadoAux: APP.controller.LicencaController.getArquivoCertificadoAnexos($("#IdLicenca").val()),
            SubmitArquivosCertificado: APP.controller.LicencaController.getArquivoCertificadoAnex2($("#IdLicenca").val()),
            Observacoes: $('#form-pos-Licenca').find('[name=Observacoes]').val(), 'required': true, 'minlength': 1, 'maxlength': 500,
            NomeUsuarioAprovador: $('#form-pos-Licenca').find('[name=Aprovador] option:selected').text(),
        };

        var temCriterioAceitacao = $("[name=SistemaDefineStatus]:checked").val() == "true" ? true : false;

        if (temCriterioAceitacao) {

            PosLicenca.Instrumento =
                {
                    SistemaDefineStatus: $("[name=SistemaDefineStatus]:checked").val(),
                    valorAceitacao: $("[name=valorAceitacao]").val(),
                    IdInstrumento: $("#IdInstrumento").val()
                };

            $('.campos-tabela').each(function (i) {

                CriteriosAceitacao = {
                    Erro: $(this).find('[name=Erro]').val(),
                    Incerteza: $(this).find('[name=Incerteza]').val(),
                    Resultado: $(this).find('[name=Resultado]').val(),
                    IdCriterioAceitacao: $(this).find('[name=IdCriterioAceitacao]').val(),
                    Periodicidade: $(this).find('[name=Periodicidade]').val(),
                    IdLicenca: $("#IdLicenca").val(),
                    Aceito: $(this).find('.ativo-color').length == 1 ? true : false,
                };

                PosLicenca.CriterioAceitacao.push(CriteriosAceitacao);
            });
        }

        return PosLicenca;
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
                                bootbox.alert({
                                    message: _options.LicencaExcluida,
                                    callback: function () {
                                        window.location.reload();
                                    }
                                });
                            }
                        },
                        beforeSend: function () {
                            APP.component.Loading.showLoading();
                        },
                        complete: function (result) {
                            APP.component.Loading.hideLoading();
                        },
                    });
                }
            });
        });
    },

    setComboProcesso: function () {

        var idSite = $('#emissao-documento-site').val();
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
    
    sendFormParametroLicenca: function () {

        APP.component.Loading.showLoading();

        var vstatus = document.getElementById('IdStatusHidden');

        $.ajaxSetup({ async: false });
        var idInstrumento = $("#IdInstrumento").val();
        var url = '/Instrumento/Criar';
        var validacao = true;

        if (idInstrumento != "0") {
            url = '/Instrumento/Editar';
        } else {
            validacao = $('#form-parametro-instrumento').valid();
        }

        if ($("[name=SistemaDefineStatus]:checked").val() == null && $("[name=SistemaDefineStatus]:checked").val() == undefined) {
            validacao = false;
            $("#emErroSistemaDefineStatus").show();
        }

        var dataobject = $('#form-parametro-instrumento').serialize();

        dataobject["Status"] = vstatus.value;

        if (validacao) {
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: url,
                data: $('#form-parametro-instrumento').serialize(),
                success: function (result) {

                    if (result.StatusCode == 200) {

                        if (idInstrumento == "0") {
                            $("#IdInstrumento").val(result.IdInstrumento);
                            bootbox.alert(
                                {
                                    message: result.Success,
                                    callback: function () {

                                        window.location.href = "/Instrumento/Editar/" + result.IdInstrumento;
                                    }
                                });
                            return;

                        }

                        APP.controller.LicencaController.setLicencaVisivel();

                        var isHidden = $('#form-pos-Licenca').is(":hidden");
                        if (isHidden == false) {
                            APP.controller.LicencaController.sendFormPosLicenca(idInstrumento, result.Success);
                        }
                        else {
                            if (idInstrumento != "0") {
                                bootbox.alert({
                                    message: result.Success,
                                    callback: function () {
                                        window.location.href = "/Instrumento/Index";
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

            $("#form-pos-Licenca").validate();

            var formLicencaValido = $("#form-pos-Licenca").valid();

            var objPosLicenca = APP.controller.LicencaController.getObjPosLicenca();
            var dataNotificacaoSplit = objPosLicenca.DataNotificacao.split("/");
            var DataProximaLicencaSplit = objPosLicenca.DataProximaLicenca.split("/");;
            var DataNotificacao = new Date(dataNotificacaoSplit[2], dataNotificacaoSplit[1], dataNotificacaoSplit[0]);
            var DataProximaLicenca = new Date(DataProximaLicencaSplit[2], DataProximaLicencaSplit[1], DataProximaLicencaSplit[0]);
            var DataRegistroSplit = objPosLicenca.DataRegistro.split("/");
            var Aprovado = objPosLicenca.Aprovado;
            var OrgaoCalibrador = objPosLicenca.OrgaoCalibrador;
            var Certificado = objPosLicenca.Certificado;
            var DataRegistro = new Date(DataRegistroSplit[2], DataRegistroSplit[1], DataRegistroSplit[0]);


            var valido = true;
            if (DataRegistro > DataNotificacao) {
                valido = false;
                APP.component.Loading.hideLoading();
                bootbox.alert(_options.labelValidaDataRegistro);


            }


            if (DataNotificacao > DataProximaLicenca) {
                valido = false;
                APP.component.Loading.hideLoading();
                bootbox.alert(_options.labelValidaDataNotificacao);
            }

            if (Aprovado < 2 && $('#IdInstrumento').val() != "0") {

                if (OrgaoCalibrador == "" || OrgaoCalibrador == null || OrgaoCalibrador == undefined) {
                    valido = false;
                    $("#lblErroOrgaoCalibrador").show();
                    APP.component.Loading.hideLoading();
                }

                if (Certificado == "" || Certificado == null || Certificado == undefined) {
                    valido = false;
                    $("#lblErroNuCertificado").show();
                    APP.component.Loading.hideLoading();
                }
            }

            if (valido) {


                if (formLicencaValido) {


                    APP.controller.LicencaController.sendFormParametroInstrumento();

                    $('#form-pos-Licenca').slideUp(500);
                    $('#form-pos-Licenca').removeClass('show').addClass('hide');
                    $('.tb-Licenca').slideDown(500);
                    $('.pills-parametros-pos-Licenca').removeClass('show').addClass('hide');
                    $('.pills-tabela-Licenca').removeClass('hide').addClass('show');
                }
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
