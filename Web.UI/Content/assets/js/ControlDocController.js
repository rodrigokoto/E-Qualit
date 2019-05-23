
/*
|--------------------------------------------------------------------------
| Control Doc
|--------------------------------------------------------------------------
*/

APP.controller.ControlDocController = {

    init: function () {


        var page = APP.component.Util.getPage();

        this.setup();
        APP.component.MenuSideBar.init();
        if (page == "ListDocumentos") {
            this.listDocumentos();
        }
        if (page == "EmissaoDocumento") {

            if (this.IdDocumento.val() != 0) {
                this.emissaoDocumentoEdicao();
            } else {
                this.emissaoDocumento();
            }
        }

        if (page == "ConteudoDocumento") {

            var xmlString = $("#form-emissao-documento-fluxo-conteudo").val();
            if (xmlString != "" && xmlString != null && xmlString != undefined) {
                this.formFluxo();
            }

            this.setImprimirDocumento();
            this.imprimir();
            //this.DisabledEditor(true);

        }

        if (page == "PDF") {
            var xmlString2 = $("#form-emissao-documento-fluxo-conteudo").val();
            if (xmlString2 != "" && xmlString2 != null && xmlString2 != undefined) {
                this.formFluxo();
            }
        }

        $(document).on("change", "#ddlCopiaControlada", function () {

            var copiaControlada = this.value;

            if (copiaControlada == "true") {
                $(".usuarioDestinoCopiaControlada").show();
            }
            else {
                $(".usuarioDestinoCopiaControlada").hide();
            }

            APP.controller.ControlDocController.models.iscontrolada = copiaControlada;

        });

        $(document).on("change", "#ddlUsuarioDestino", function () {

            APP.controller.ControlDocController.models.idusuariodestino = this.value;
        });



        function moveItems(origin, dest) {
            $(origin).find(':selected').appendTo(dest);
        }

        function orderItems(origin, orderUp) {
            
            $(origin).find(':selected').appendTo(dest);
        }

        $('#add').click(function () {
            if ($('#form-cadastro-verificadorBase').find(':selected').length + $('#form-cadastro-verificador').find('option').length > 3) {
                bootbox.alert("Numero máximo de verificadores 3.");
            } else {
                moveItems('#form-cadastro-verificadorBase', '#form-cadastro-verificador');
            }
            
            //moveItems('#form-cadastro-verificadorBase', '#form-cadastro-verificador');
        });

        $('#remove').click(function () {
            moveItems('#form-cadastro-verificador', '#form-cadastro-verificadorBase');
        });

        $('#addAprovador').click(function () {
            if ($('#form-cadastro-aprovadorBase').find(':selected').length + $('#form-cadastro-aprovador').find('option').length > 3) {
                bootbox.alert("Numero máximo de aprovadores 3.");
            } else {
                moveItems('#form-cadastro-aprovadorBase', '#form-cadastro-aprovador');
            }
        });

        $('#removeAprovador').click(function () {
            moveItems('#form-cadastro-aprovador', '#form-cadastro-aprovadorBase');
        });





        $('#up').click(function () {
            var opt = $('#form-cadastro-verificador option:selected');

            if (opt.is(':first-child')) {
                opt.insertAfter($('#form-cadastro-verificador option:last-child'));
            }
            else {
                opt.insertBefore(opt.prev());
            }
            //orderItems('#form-cadastro-verificadorBase', true);
        });

        $('#down').click(function () {
            var opt = $('#form-cadastro-verificador option:selected');

            if (opt.is(':last-child')) {
                opt.insertBefore($('#form-cadastro-verificador option:first-child'));
            }
            else {
                opt.insertAfter(opt.next());
            }
        });




        $('#upAprovador').click(function () {
            var opt = $('#form-cadastro-aprovador option:selected');

            if (opt.is(':first-child')) {
                opt.insertAfter($('#form-cadastro-aprovador option:last-child'));
            }
            else {
                opt.insertBefore(opt.prev());
            }
            //orderItems('#form-cadastro-verificadorBase', true);
        });

        $('#downAprovador').click(function () {
            var opt = $('#form-cadastro-aprovador option:selected');

            if (opt.is(':last-child')) {
                opt.insertBefore($('#form-cadastro-aprovador option:first-child'));
            }
            else {
                opt.insertAfter(opt.next());
            }
        });
    },

    setup: function () {


        //Control Doc - EmissaoDocumento
        this.buttonTabs = $(".menu-one li");
        this.buttonSalvar = $('.btn-salvar');
        this.buttonEnviarVerificacao = $('.btn-enviar-verificacao');
        this.buttonEnviarAprovacao = $('.btn-enviar-aprovacao');
        this.buttonVoltarElaboracao = $('.btn-voltar-elaboracao');
        this.buttonAprovar = $('.btn-aprovar');

        //Control Doc - ListDocumentos
        this.buttonExcluirDocumento = $(".excluir-documento");

        //Control Doc - Impressao Documentos
        this.buttonImprimirDocumento = $(".imprimir-documento");

        //Control Doc - REGISTROS
        this.buttonAddNovoRegistroFormRegistro = $('.form-registros-add-registro');
        this.buttonSaveNovoRegistroFormRegistro = $('.salvar-form-registros');
        this.buttonEditNovoRegistroFormRegistro = $('.editar-form-registros');
        this.buttonDelNovoRegistroFormRegistro = $('.excluir-form-registros');

        //Control Doc - ROTINAS
        this.buttonAddNovaRotinaFormRotina = $('.form-rotina-add-rotina');
        this.buttonSaveNovaRotinaFormRotina = $('.salvar-form-rotina');
        this.buttonEditNovaRotinaFormRotina = $('.editar-form-rotina');
        this.buttonDelNovaRotinaFormRotina = $('.excluir-form-rotina');

        //Control Doc - INDICADORES
        this.buttonAddNovaIndicadoresFormIndicadores = $('.form-rotina-add-indicadores');
        this.buttonDelNovaIndicadoresFormIndicadores = $('.excluir-form-indicadores');
        this.buttonSaveNovaIndicadoresFormIndicadores = $('.salvar-form-indicadores');
        this.buttonEditNovaIndicadoresFormIndicadores = $('.editar-form-indicadores');


        //Control Doc - UPLOAD
        this.buttonAddUploadFormUpload = $('.form-upload-add-upload');
        this.buttonDelUploadFormUpload = $('.excluir-form-upload');

        //Control Doc - ASSUNTOS
        this.buttonAddNovoAssuntoFormAssuntos = $('.form-assuntos-add-assunto');
        this.buttonSaveNovoAssuntoFormAssuntos = $('.salvar-form-assuntos');
        this.buttonEditNovoAssuntoFormAssuntos = $('.editar-form-assuntos');
        this.buttonDelAssuntoFormAssuntos = $('.excluir-form-assuntos');

        //Control Doc - COMENTARIOS
        this.buttonAddNovaMensagemFormComentarios = $('.btn-form-comentario-enviar');

        //IdDocumento
        this.IdDocumento = $("#emissao-documento-IdDocumento");

    },

    models: {
        AnexoModel: APP.model.Anexo,
        iscontrolada: null,
        idusuariodestino: null,
        idDocumento: null
    },

    //Chamadas
    listDocumentos: function () {

        APP.component.DataTable.init("#tb-list-documentos");
        this.setRevisarDocumento();
        this.setExcluirDocumento();
        this.setImprimirDocumento();
        this.imprimir();
    },

    changeCadastroSigla: function () {

        $('[name=formCadastroSigla]').change(function () {

            var sigla = $(this).val();

            $.post('/ControlDoc/RetornaNumeroPorSigla/' + sigla, function (data, status) { }).done(function (data) {
                if (data.StatusCode == "200") {

                    $('[name=formCadastroNmDocumento]').val(data.Retorno);

                }
            });

        });

    },

    setImprimirDocumento: function () {

        var tabela = $("#tb-list-documentos").DataTable();

        this.buttonImprimirDocumento.unbind('click');
        this.buttonImprimirDocumento.bind('click', function () {
            $(".usuarioDestinoCopiaControlada").show();
            event.preventDefault();
            APP.controller.ControlDocController.models.idDocumento = $(this).data("iddocumento");
            var perfil = $(this).data("perfil");
            var $rowAtual = $(this).parents("tr");

            if (perfil == "1" || perfil == "3") {

                var modal = bootbox.dialog({
                    title: "Impressão de Documentos",
                    message: $(".modal-impressao").html(),
                    buttons: [
                        {
                            label: _options.BtnImprimir,
                            className: "btn btn-primary pull-right",
                            callback: function () {
                                APP.controller.ControlDocController.imprimir(APP.controller.ControlDocController.models.idDocumento, true);
                            }
                        },
                        {
                            label: _options.botao_cancelar,
                            className: "btn btn-default pull-right",
                            callback: function () {
                                modal.modal("hide");
                            }
                        }
                    ],
                    show: false,
                    onEscape: function () {
                        modal.modal("hide");
                    }
                });
                modal.modal("show");
            }
            else {
                APP.controller.ControlDocController.models.iscontrolada = null;
                APP.controller.ControlDocController.models.idusuariodestino = null;
                APP.controller.ControlDocController.imprimir(APP.controller.ControlDocController.models.idDocumento, false);
            }
        });
    },

    imprimir: function (idDocumento, possuiPerfilControle) {



        if (idDocumento != null) {

            APP.component.Loading.showLoading();

            var isControlada = null;
            idUsuarioDestino = null;

            if (possuiPerfilControle) {
                isControlada = APP.controller.ControlDocController.models.iscontrolada == null ? $("#ddlCopiaControlada").val() : APP.controller.ControlDocController.models.iscontrolada;
                idUsuarioDestino = APP.controller.ControlDocController.models.idusuariodestino == null ? $("#ddlUsuarioDestino").val() : APP.controller.ControlDocController.models.idusuariodestino;
            } else {
                isControlada = false;
                idUsuarioDestino = "";
            }

            gerarPdf(idDocumento, isControlada, idUsuarioDestino);

        }
    },

    downloadPdf: function (idDocumento, isControlada, idUsuarioDestino, fluxoBase64) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/ControlDoc/PDF', true);
        xhr.responseType = 'arraybuffer';
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

        xhr.onload = function (e) {
            if (this.status == 200) {
                var blob = new Blob([this.response], { type: "application/pdf" });
                var pdfUrl = URL.createObjectURL(blob);
                printJS(pdfUrl);
            }
            APP.controller.ControlDocController.models.iscontrolada = null;
            APP.controller.ControlDocController.models.idusuariodestino = null;

            APP.component.Loading.hideLoading();

        };

        xhr.send(JSON.stringify({ "id": idDocumento, "controlada": isControlada, "usuarioDest": idUsuarioDestino, "fluxoBase64": fluxoBase64 })); 
    },

    setExcluirDocumento: function () {

        var tabela = $('#tb-list-documentos').DataTable();

        this.buttonExcluirDocumento.unbind('click');
        this.buttonExcluirDocumento.bind('click', function () {
            event.preventDefault();

            var idDocumento = $(this).data("iddocumento");

            var $rowAtual = $(this).parents('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (result) {
                if (result) {
                    $.post('/ControlDoc/Excluir/' + idDocumento, function (data, status) { }).done(function (data) {
                        if (data.StatusCode == "200") {
                            tabela.row($rowAtual).remove().draw();
                            bootbox.alert(_options.MsgRegistroExcluidoSucesso);
                        }
                        if (data.StatusCode == "500") {
                            bootbox.alert(_options.MsgErroExclusaoRegistro);
                        }
                    });
                }
            });
        });

    },

    setRevisarDocumento: function () {
        $('.controldoc-revisar').on('click', function () {
            var erro = "";


            var idDoc = $(this).data('id-doc');

            $.ajax({
                type: "POST",
                dataType: 'json',
                url: '/ControlDoc/Revisar?Id=' + idDoc,
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    if (result.StatusCode == 200) {
                        window.location.href = "/ControlDoc/Editar/?id=" + result.IdRevisao;
                    }
                    else if (result.StatusCode == 505) {
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

        });

    },

    //Criar
    emissaoDocumento: function () {
        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        this.setValidateForms();
        this.setHideAndShow();
        this.getTabs();
        this.getEscolhaPanel();
        this.formCadastro();
        this.formTexto();
        this.formFluxo();
        this.formRegistro();
        this.formIndicadores();
        this.formRotina();
        this.formRecursos();
        this.formUpload();
        this.formRiscos();
        this.formLicenca();
        this.formDocsExternos();
        this.formAssuntos();
        this.formComentarios();
        this.formCargos();
        this.setCargosEmissaoDocumentos();

        this.sendFormEmissaoDocumento();
        this.sendFormEmissaoDocumentoVerificacao();
        this.sendFormEmissaoDocumentoAprovacao();
        this.sendFormEmissaoDocumentoElaboracao();
        this.changeCadastroSigla();
        //this.setValidateFormEmissaoDocumentoCadastro();

    },

    emissaoDocumentoEdicao: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        this.formCadastro();
        this.setHideAndShowEdit();
        this.getTabs();
        this.getEscolhaPanel();
        this.formTexto();
        this.formFluxo();
        this.formRegistro();
        this.formRotina();
        this.formIndicadores();
        this.formRecursos();
        this.formUpload();
        this.formRiscos();
        this.formLicenca();
        this.formDocsExternos();
        this.formAssuntos();
        this.formComentarios();
        this.formCargos();
        this.setCargosEmissaoDocumentos();
        this.setWorkFlow();
        APP.controller.ControlDocController.getTemasCores();

        this.sendFormEmissaoDocumento();
        this.sendFormEmissaoDocumentoVerificacao();
        this.sendFormEmissaoDocumentoAprovacao();
        this.sendFormEmissaoDocumentoElaboracao();
        this.sendFormEmissaoDocumentoAprovado();
        this.changeCadastroSigla();
        //HIde and show Templates
        this.setShowTemplatesEmissaoDocumento();

        //$("#form-emissao-documento-texto").show();
        $('[name=formRiscosNecessitaAcao]').trigger('change');
        $('[name=formCadastroWorkflow]').trigger('change');

        this.setImprimirDocumento();
        this.imprimir();
    },

    //Funcoes Emissao Documento
    setValidateForms: function () {

        var ObjFormCadastroValidate = APP.controller.ControlDocController.getObjFormCadastroValidate();
        APP.component.ValidateForm.init(ObjFormCadastroValidate, '#form-emissao-documento-cadastro');

    },

    setHideAndShow: function () {

        var statusEtapa = parseInt($('[name=StatusEtapa]').val());

        //Hide All
        $('[id^=panel-form-]').hide();

        var idDocumento = $("#emissao-documento-IdDocumento").val();

        if (idDocumento == null || idDocumento == undefined || idDocumento == "0") {
            $('[id^=panel-tab-]').hide();
        }
        else {
            $(".menu-one").show();
        }

        $('#panel-form-cadastro').show();
        $('.btn-enviar-verificacao').hide();
        $('.btn-enviar-aprovacao').hide();
        $('.btn-voltar-elaboracao').hide();
        $('.btn-aprovar').hide();

        var getWorkFlow = $('[name=formCadastroWorkflow]').val();

        if (getWorkFlow == 'true') {

            if (statusEtapa != 0) {

                $('[name=StatusEtapa]').prop('disabled', true);

            }

            switch (statusEtapa) {
                case 0:
                    $('.um').hide();
                    $('.dois').hide();
                    $('.tres').hide();
                    break;
                case 1:
                    this.setDisableVerificacao();
                    $('.btn-enviar-verificacao').hide();
                    $('.btn-voltar-elaboracao').show();
                    break;
                case 2:
                    $('.btn-enviar-aprovacao').show();
                    $('.btn-voltar-elaboracao').show();
                    break;
            }
        }



    },

    getTabs: function () {
        $('a[data-toggle="tab"]').on('click', function (e) {
            var target = $(e.target).attr("href");
        });
    },

    getEscolhaPanel: function () {

        $('[id^=form-cadastro-escolha]').unbind('click');
        $('[id^=form-cadastro-escolha]').on('click', function () {
            
            $(".menu-one").show();

            var panel = $(this).attr('id').split('-');
            $('#panel-tab-' + panel[3]).slideToggle();
            $('#panel-form-' + panel[3]).slideToggle();

            var tam = 0;
            setTimeout(function () {
                tam = $('[id^=panel-tab-]:visible').size();
                setTimeout(function () {
                    APP.controller.ControlDocController.setTabs(tam);

                }, 1);
            }, 500);



        });

    },

    setTabs: function (_tam) {

        switch (_tam) {
            case 0:
                $('#panel-tab-cadastro').slideToggle();
                break;
            case 1:
                $('#panel-tab-cadastro').slideToggle();
                $('[id^=panel-tab-]:visible').css('width', '50%');
                break;
            case 2:
                $('[id^=panel-tab-]:visible').css('width', '50%');
                break;
            case 3:
                $('[id^=panel-tab-]:visible').css('width', '33.3%');
                break;
            case 4:
                $('[id^=panel-tab-]:visible').css('width', '25%');
                break;
            case 5:
                $('[id^=panel-tab-]:visible').css('width', '20%');
                break;
            case 6:
                $('[id^=panel-tab-]:visible').css('width', '16.6%');
                break;
            case 7:
                $('[id^=panel-tab-]:visible').css('width', '14.2%');
                break;
            case 8:
                $('[id^=panel-tab-]:visible').css('width', '12.5%');
                break;
            case 9:
                $('[id^=panel-tab-]:visible').css('width', '11.1%');
                break;
            case 10:
                $('[id^=panel-tab-]:visible').css('width', '10%');
                break;
            default:

        }

    },

    validateForms: function () {

        var arrayFormValidate = [];
        var validate = {};
        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');

            var id = $(this).attr('id');
            if (id !== 'panel-form-fluxo') {
                if (isVisible) {
                    validate = {

                        valid: $(this).closest('form').valid(),
                    };
                    arrayFormValidate.push(validate);
                }
            }
        });

        return arrayFormValidate;

    },

    sendFormEmissaoDocumento: function () {
        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = [];
            var ValidateOK = true;

            validate = APP.controller.ControlDocController.validateForms();

            for (i = 0; i < validate.length; i++) {
                if (validate[i].valid != true) {
                    ValidateOK = false;
                }
            }

            //ValidateOK = true;
            if (ValidateOK == true) {
                var emissaoDocumento = APP.controller.ControlDocController.getEmissaoDocumentoObj();

                if ($("#emissao-documento-IdDocumento").val() != 0) {

                    emissaoDocumento.Assuntos = APP.controller.ControlDocController.getObjFormAssuntos();
                    emissaoDocumento.Comentarios = APP.controller.ControlDocController.getObjFormComentarios();
                }

                var statusEtapa = parseInt($('[name=StatusEtapa]').val());
                var workFlow = $("[name=formCadastroWorkflow]").val();

                if (workFlow != "sim") {
                    statusEtapa = 3;
                }

                APP.controller.ControlDocController.saveFormEmissaoDocumento(emissaoDocumento, statusEtapa);
            }

        });

    },

    getEmissaoDocumentoObj: function (_statusEmissaoDocumento) {
        var formEmissaoDocumentoCadastro = $('#form-emissao-documento-cadastro');
        var formEmissaoDocumentoTexto = $('#form-emissao-documento-texto');
        var formEmissaoDocumentoFluxo = $('#form-emissao-documento-fluxo');
        var formEmissaoDocumentoRegistros = $('#form-emissao-documento-registros');
        var formEmissaoDocumentoRotina = $('#form-emissao-documento-rotina');
        var formEmissaoDocumentoRecursos = $('#form-emissao-documento-recursos');
        var formEmissaoDocumentoUpload = $('#form-emissao-documento-upload');
        var formEmissaoDocumentoRiscos = $('#form-emissao-documento-riscos');
        var formEmissaoDocumentoLicenca = $('#form-emissao-documento-licenca');
        var formEmissaoDocumentoDocsExternos = $('#form-emissao-documento-docsexternos');
        var formEmissaoDocumentoAssuntos = $('#form-emissao-documento-assuntos');
        var formEmissaoDocumentoComentarios = $('#form-emissao-documento-comentarios');
        var formEmissaoDocumentoCargos = $('#form-emissao-documento-cargos');

        var formEmissaoDocumentoIndicadores = $('#form-emissao-documento-indicadores');



        var emissaoDocumentoObj = {};

        var ConteudoDocumento = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "cadastro":
                        emissaoDocumentoObj = APP.controller.ControlDocController.getObjFormCadastro(_statusEmissaoDocumento);
                        break;
                    case "texto":
                        emissaoDocumentoObj.TextoDoc = APP.controller.ControlDocController.getObjFormTexto();
                        break;
                    case "fluxo":
                        emissaoDocumentoObj.FluxoDoc = APP.controller.ControlDocController.getObjFormFluxo();
                        break;
                    case "registros":
                        emissaoDocumentoObj.Registros = APP.controller.ControlDocController.getObjFormRegistro();
                        break;
                    case "rotina":
                        emissaoDocumentoObj.Rotinas = APP.controller.ControlDocController.getObjFormRotina();
                        break;
                    case "recursos":
                        emissaoDocumentoObj.RecursoDoc = APP.controller.ControlDocController.getObjFormRecursos();
                        break;
                    case "riscos":
                        emissaoDocumentoObj.GestaoDeRisco = APP.controller.ControlDocController.getObjFormRiscos();
                        emissaoDocumentoObj.CorRisco = $('[name=formRiscosCorDoRisco]:checked').val();
                        emissaoDocumentoObj.PossuiGestaoRisco = $('[name=formRiscosNecessitaAcao]:checked').val();
                        break;
                    case "licenca":
                        emissaoDocumentoObj.Licenca = APP.controller.ControlDocController.getObjFormLicenca();
                        break;
                    case "docsexternos":
                        emissaoDocumentoObj.DocExterno = APP.controller.ControlDocController.getObjFormDocsExternos();
                        break;
                    case "indicadores":
                        emissaoDocumentoObj.Indicadores = APP.controller.ControlDocController.getObjFormIndicadores();
                        break;
                }
            }
        });

        emissaoDocumentoObj.ConteudoDocumento = ConteudoDocumento;
        emissaoDocumentoObj.DocCargo = APP.controller.ControlDocController.getObjFormCargos();

        return emissaoDocumentoObj;
    },

    aux: { IdDocumento: '' },

    saveFormEmissaoDocumento: function (emissaoDocumento, _statusEtapa) {
        var url = "/ControlDoc/Salvar/";
        var eEdicao = false;

        if (this.IdDocumento.val() != 0) {
            eEdicao = true;
        }

        $.ajax({
            type: "POST",
            data: { "doc": emissaoDocumento, "status": _statusEtapa },
            dataType: 'json',
            url: url,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {


                APP.controller.ControlDocController.aux.IdDocumento = result.IdDocumento;
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        if (!emissaoDocumento.FlWorkFlow) {
                            window.location.href = "/ControlDoc/DocumentosAprovados/?id=" + emissaoDocumento.IdCategoria + "&?idProcesso=" + emissaoDocumento.IdProcesso;
                            return;
                        }
                        if (eEdicao) {
                            window.location.reload(true);
                            return;
                        }
                        window.location.href = "/ControlDoc/Editar/" + APP.controller.ControlDocController.aux.IdDocumento;
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
        //}

    },

    sendFormEmissaoDocumentoVerificacao: function (emissaoDocumento) {

        this.buttonEnviarVerificacao.unbind('click');
        this.buttonEnviarVerificacao.on('click', function () {

            var emissaoDocumento = APP.controller.ControlDocController.getEmissaoDocumentoObj(1);
            emissaoDocumento.Assuntos = APP.controller.ControlDocController.getObjFormAssuntos();
            emissaoDocumento.Comentarios = APP.controller.ControlDocController.getObjFormComentarios();

            APP.controller.ControlDocController.saveFormEmissaoDocumentoEtapaVerificacao(emissaoDocumento);
        });

    },

    saveFormEmissaoDocumentoEtapaVerificacao: function (emissaoDocumento) {

        var erro = "";

        $.ajax({
            type: "POST",
            data: emissaoDocumento,
            dataType: 'json',
            url: '/ControlDoc/EnviarDocumentoParaVerificacao',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/ControlDoc/DocumentosVerificacao";
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

    sendFormEmissaoDocumentoAprovacao: function (emissaoDocumento) {
        this.buttonEnviarAprovacao.unbind('click');
        this.buttonEnviarAprovacao.on('click', function () {

            var emissaoDocumento = APP.controller.ControlDocController.getEmissaoDocumentoObj(2);
            emissaoDocumento.Assuntos = APP.controller.ControlDocController.getObjFormAssuntos();
            emissaoDocumento.Comentarios = APP.controller.ControlDocController.getObjFormComentarios();

            APP.controller.ControlDocController.saveFormEmissaoDocumentoEtapaAprovacao(emissaoDocumento);
        });
    },

    saveFormEmissaoDocumentoEtapaAprovacao: function (emissaoDocumento) {

        $.ajax({
            type: "POST",
            data: emissaoDocumento,
            dataType: 'json',
            url: '/ControlDoc/EnviarParaAprovacao',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/ControlDoc/DocumentosAprovacao";
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

    saveFormEmissaoDocumentoEtapaAprovar: function (documento) {

        $.ajax({
            type: "POST",
            data: documento,
            dataType: 'json',
            url: '/ControlDoc/EnviarParaAprovado',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/ControlDoc/DocumentosAprovados/?id=" + documento.IdCategoria + "&?idProcesso=" + documento.IdProcesso;
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

    sendFormEmissaoDocumentoElaboracao: function (emissaoDocumento) {

        this.buttonVoltarElaboracao.unbind('click');
        this.buttonVoltarElaboracao.on('click', function () {

            var emissaoDocumento = APP.controller.ControlDocController.getEmissaoDocumentoObj(0);
            emissaoDocumento.Assuntos = APP.controller.ControlDocController.getObjFormAssuntos();
            emissaoDocumento.Comentarios = APP.controller.ControlDocController.getObjFormComentarios();

            APP.controller.ControlDocController.saveFormEmissaoDocumentoEtapaVoltarElaboracao(emissaoDocumento);

        });

    },

    saveFormEmissaoDocumentoEtapaVoltarElaboracao: function (emissaoDocumento) {
        $.ajax({
            type: "POST",
            data: emissaoDocumento,
            dataType: 'json',
            url: '/ControlDoc/EnviarParaElaboracao',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {

                        window.location.href = "/ControlDoc/DocumentosElaboracao";
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

    sendFormEmissaoDocumentoAprovado: function () {
        this.buttonAprovar.unbind('click');
        this.buttonAprovar.on('click', function () {
            var idDocumento = $("#emissao-documento-IdDocumento").val();

            var emissaoDocumento = APP.controller.ControlDocController.getEmissaoDocumentoObj(1);
            emissaoDocumento.Assuntos = APP.controller.ControlDocController.getObjFormAssuntos();
            emissaoDocumento.Comentarios = APP.controller.ControlDocController.getObjFormComentarios();

            APP.controller.ControlDocController.saveFormEmissaoDocumentoEtapaAprovar(emissaoDocumento);
        });
    },

    //Formulario CADASTRO
    formCadastro: function () {

        //Preenchimento dos Combos
        this.setComboSigla();
        this.setSigla();
        this.setCategoria();
        this.setComboCategoria();


        this.setComboProcesso();
        this.setElaborador();
        this.setComboVerificador();
        this.setComboAprovador();
        this.getChangeComboProcesso();
        this.getRadioFormCadastroRevisaoPeriodica();
        this.getRadioFormCadastroWorkflow();

        //ConstruÃ§Ã£o OBJ
        //this.getObjFormCadastro();

        // Hide OR Show de Campos do Formulario
        this.setHideAndShowFormCadastro();

        this.setRadioFormCadastroRevisaoPeriodica();


    },

    setHideAndShowFormCadastro: function () {

        $('#form-cadastro-dt-notificacao').closest('.form-group').hide();
        $('#form-cadastro-verificador').closest('.form-group').hide();
        $('#form-cadastro-aprovador').closest('.form-group').hide();

    },


    setRadioFormCadastroRevisaoPeriodica: function () {

        var RadioFormCadastroRevisaoPeriodica = APP.component.Radio.init('formCadastroRevisaoPeriodica');

        if (RadioFormCadastroRevisaoPeriodica != "sim") {
            $('#form-cadastro-dt-notificacao').closest('.form-group').hide();
        } else {
            $('#form-cadastro-dt-notificacao').closest('.form-group').show();
        }

    },

    setComboSigla: function () {

        var idSite = $('#emissao-documento-site').val();
        let idFuncao = 15;
        var dataTipo = $("#form-cadastro-sigla").closest('div').find('i').data("tipo");
        var dataSite = $("#form-cadastro-sigla").closest('div').find('i').data("site");
        var data = {
            "tipo": dataTipo,
            "site": dataSite
        };

        $.ajax({
            type: "GET",
            dataType: 'JSON',
            data: data,
            url: '/ControladorCategorias/ListaAtivos',
            success: function (result) {
                if (result.StatusCode == 202) {
                    APP.component.SelectListCompare.init(result.Lista, $('[name=formCadastroSigla] option'), '#form-cadastro-sigla', 'IdControladorCategorias', 'Descricao');
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
        });
    },

    setSigla: function () {

        $('.add-sigla').on('click', function () {

            APP.component.BootBox.chamaBootBox("Cadastro",
                "ControladorCategorias",
                "pnlCadUsu",
                APP.controller.ControladorCategoriasController.behavior,
                "Cadastro",
                APP.controller.ControlDocController.setComboSigla,
                ".add-sigla");

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

    setComboCategoria: function () {

        var idSite = $('#emissao-documento-site').val();
        let idFuncao = 15;
        var dataTipo = $("#form-cadastro-categoria").closest('div').find('i').data("tipo");
        var dataSite = $("#form-cadastro-categoria").closest('div').find('i').data("site");
        var data = {
            "tipo": dataTipo,
            "site": dataSite
        };

        $.ajax({
            type: "GET",
            dataType: 'JSON',
            data: data,
            url: '/ControladorCategorias/ListaAtivos',
            success: function (result) {
                if (result.StatusCode == 202) {
                    APP.component.SelectListCompare.init(result.Lista, $('[name=formCadastroCategoria] option'), '#form-cadastro-categoria', 'IdControladorCategorias', 'Descricao');
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
        });
    },

    setCategoria: function () {

        $('.add-categoria').on('click', function () {

            APP.component.BootBox.chamaBootBox("Cadastro",
                "ControladorCategorias",
                "pnlCadUsu",
                APP.controller.ControladorCategoriasController.behavior,
                "Cadastro",
                APP.controller.ControlDocController.setComboCategoria,
                ".add-categoria");

        });

    },

    setElaborador: function () {

        var idSite = $('#emissao-documento-site').val();
        let idFuncao = 3;
        var data = {
            "idSite": idSite,
            "idFuncao": idFuncao
        };

        $.ajax({
            type: "GET",
            dataType: 'JSON',
            data: data,
            url: '/Usuario/ObterUsuariosPorFuncao',
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    APP.component.SelectListCompare.init(result.Lista, $('[name=formCadastroElaborador] option'), '#form-cadastro-elaborador', 'IdUsuario', 'NmCompleto');
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {

            }
        });

        var eColaborador = $('#emissao-documento-e-colaborador').val();
        var IdUsuarioLogado = $('#emissao-documento-id-usuario-logado').val();
        if (eColaborador == "true") {
            $('[name=formCadastroElaborador]').prop('disabled', true);
            $('[name=formCadastroElaborador]').val(IdUsuarioLogado);
        }

    },

    setComboVerificador: function (_IdProcesso) {

        var idSite = $('#emissao-documento-site').val();
        let idFuncao = 101;
        var data = "";
        var url = "";

        if (_IdProcesso != 0 && _IdProcesso != undefined) {
            url = '/Usuario/ObterUsuariosPorFuncaoSiteEProcesso';
            data = {
                "idProcesso": _IdProcesso,
                "idSite": idSite,
                "idFuncao": idFuncao
            };
        } else {
            url = '/Usuario/ObterUsuariosPorFuncao';
            data = {
                "idSite": idSite,
                "idFuncao": idFuncao
            };
        }


        function arrayRemove(arr, value) {
            return arr.filter(function (ele) {
                return ele.IdUsuario != value;
            });

        }

        $.ajax({
            type: "GET",
            dataType: 'JSON',
            data: data,
            url: url,
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    //var retorno = result.Lista.pop();
                    //retorno = result.Lista.pop();

                    var retultado = result.Lista;
                    var lista = $('[name=formCadastroVerificador] option');
                    for (var i = 0; i < lista.length; i++) {
                        var valor = lista[i].value;
                        retultado = arrayRemove(retultado, valor);

                    }


                    APP.component.SelectListCompare.init(retultado, $('[name=formCadastroVerificadorBase] option'), '#form-cadastro-verificadorBase', 'IdUsuario', 'NmCompleto');


                    //APP.component.SelectListCompare.init(result.Lista, $('[name=formCadastroVerificador] option'), '#form-cadastro-verificadorBase', 'IdUsuario', 'NmCompleto');
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {

            }
        });

    },

    setComboAprovador: function (_IdProcesso) {
        var idSite = $('#emissao-documento-site').val();
        let idFuncao = 102;
        var data = "";
        var url = "";

        if (_IdProcesso != 0 && _IdProcesso != undefined) {
            url = '/Usuario/ObterUsuariosPorFuncaoSiteEProcesso';
            data = {
                "idProcesso": _IdProcesso,
                "idSite": idSite,
                "idFuncao": idFuncao
            };
        } else {
            url = '/Usuario/ObterUsuariosPorFuncao';
            data = {
                "idSite": idSite,
                "idFuncao": idFuncao
            };
        }

        function arrayRemove(arr, value) {
            return arr.filter(function (ele) {
                return ele.IdUsuario != value;
            });

        }

        $.ajax({
            type: "GET",
            dataType: 'JSON',
            data: data,
            url: url,
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {


                    var retultado = result.Lista;
                    var lista = $('[name=formCadastroAprovador] option');
                    for (var i = 0; i < lista.length; i++) {
                        var valor = lista[i].value;
                        retultado = arrayRemove(retultado, valor);

                    }

                    APP.component.SelectListCompare.init(retultado, $('[name=formCadastroAprovadorBase] option'), '#form-cadastro-aprovadorBase', 'IdUsuario', 'NmCompleto');
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {

            }
        });

    },

    getChangeComboProcesso: function () {

        $('#form-cadastro-processo').on('change', function () {
            var IdProcesso = $(this).val();
            if (IdProcesso == 0) {
                IdProcesso = null;
            }

            APP.controller.ControlDocController.setComboVerificador(IdProcesso);
            APP.controller.ControlDocController.setComboAprovador(IdProcesso);
            APP.controller.ControlDocController.getResponsavel(IdProcesso);

        });

    },

    getRadioFormCadastroRevisaoPeriodica: function () {
        $('[name=formCadastroRevisaoPeriodica]').on('change', function () {

            var RadioFormCadastroRevisaoPeriodica = APP.component.Radio.init('formCadastroRevisaoPeriodica');

            if (RadioFormCadastroRevisaoPeriodica != "sim") {
                $('#form-cadastro-dt-notificacao').closest('.form-group').hide();
            } else {
                $('#form-cadastro-dt-notificacao').closest('.form-group').show();
            }
        });
    },

    getRadioFormCadastroWorkflow: function () {
        $('[name=formCadastroWorkflow]').on('change', function () {
            APP.controller.ControlDocController.hideOrShowWorkFlow();
        });
    },

    getFormCadastroVerificadores: function () {

        var objVerificadores = [];

        // Objeto Verificadores
        //$('[name=formCadastroVerificador] :selected').each(function () {
        $('[name=formCadastroVerificador]').find('option').each(function () {
            var verificador = {
                IdUsuario: $(this).val(),
                TpEtapa: "V",
                FlVerificou: "0",
                IdDocUsuarioVerificaAprova: $(this).data("iddocusuarioverificaaprova"),
                IdDocumento: $("#emissao-documento-IdDocumento").val()
            };
            objVerificadores.push(verificador);
        });

        return objVerificadores;

    },

    getFormCadastroAprovadores: function () {

        var objAprovadores = [];

        // Objeto Aprovadores
        //$('[name=formCadastroAprovador] :selected').each(function () {
        $('[name=formCadastroAprovador]').find('option').each(function () {
            var aprovador = {
                IdUsuario: $(this).val(),
                TpEtapa: "A",
                FlAprovou: "0",
                IdDocUsuarioVerificaAprova: $(this).data("iddocusuarioverificaaprova"),
                IdDocumento: $("#emissao-documento-IdDocumento").val()
            };
            objAprovadores.push(aprovador);
        });

        return objAprovadores;

    },

    getObjFormCadastroValidate: function () {

        var emissaoDocumentoFormCadastroObj = {
            formCadastroTituloDocumento: 'required',
            formCadastroSigla: 'required',
            formCadastroNmDocumento: 'required',
            //formCadastroProcesso: 'required',
            formCadastroCategoria: 'required',
            //formCadastroEscolha: 'required',
            formCadastroRevisaoPeriodica: 'required',
            // formCadastroDtNotificacao: 'required',
            //formCadastroWorkflow: 'required',
            formCadastroElaborador: 'required',
            // formCadastroVerificador: 'required',
            kformCadastroAprovador: 'required',
        };

        return emissaoDocumentoFormCadastroObj;

    },

    getObjFormCadastro: function (_statusEmissaoDocumento) {

        var idSite = $('#emissao-documento-site').val();
        var idProcesso = $('[name=formCadastroProcesso] option:selected').val();
        var FlRevisaoPeriodica = APP.component.Radio.init('formCadastroRevisaoPeriodica') == "sim" ? true : false;
        var FlWorkFlow = APP.component.Radio.init('formCadastroWorkflow') == "sim" ? true : false;

        var docUsuarios = [];
        docUsuarios.push(APP.controller.ControlDocController.getFormCadastroAprovadores());
        docUsuarios.push(APP.controller.ControlDocController.getFormCadastroVerificadores());

        var emissaoDocumentoFormCadastroObj = {
            IdDocumento: $('#emissao-documento-IdDocumento').val(),
            IdSite: idSite,
            IdDocIdentificador: "",
            IdProcesso: idProcesso == 0 ? null : idProcesso,
            IdCategoria: $('[name=formCadastroCategoria]').val(),
            Titulo: $('[name=formCadastroTituloDocumento]').val(),
            IdSigla: $('[name=formCadastroSigla]').val(),
            NumeroDocumento: $('[name=formCadastroNmDocumento]').val(),
            IdElaborador: $('[name=formCadastroElaborador]').val(),
            FlRevisaoPeriodica: FlRevisaoPeriodica,
            DtNotificacao: FlRevisaoPeriodica == true ? $('[name=formCadastroDtNotificacao]').val() : null,
            FlWorkFlow: FlWorkFlow,
            FlStatus: _statusEmissaoDocumento,
            IdLicenca: $('[name=formLicensaAnexoIdAnexo]').val() != "" ? $('[name=formLicensaAnexoIdAnexo]').val() : null,
            Licenca: null,
            IdDocExterno: $('[name=formDocExternoAnexoIdDocExterno]').val() != "" ? $('[name=formDocExternoAnexoIdDocExterno]').val() : null,
            Ativo: true,
            Aprovadores: APP.controller.ControlDocController.getFormCadastroAprovadores(),
            Verificadores: APP.controller.ControlDocController.getFormCadastroVerificadores(),
            DocTemplate: APP.controller.ControlDocController.getListFormCadastroEscolha(),
            NuRevisao: $('[name=nuRevisao]').val(),
        };

        //emissaoDocumentoFormCadastroObj.DocUSuarioVerificaAprova = docUsuarios;

        return emissaoDocumentoFormCadastroObj;
    },

    getListFormCadastroEscolha: function () {

        var arrayListEscolhaObj = [];
        var cadastroEscolha = {};
        $('[name=formCadastroEscolha]:checked').each(function () {
            cadastroEscolha = {
                TpTemplate: $(this).val(),
                IdDocTemplate: $(this).data("id"),
                IdDocumento: $("#emissao-documento-IdDocumento").val(),
            };
            arrayListEscolhaObj.push(cadastroEscolha);
        });
        return arrayListEscolhaObj;

    },

    //Formulario TEXTO
    formTexto: function () {

        this.setCkeditorFormTexto();
    },

    setCkeditorFormTexto: function () {
        // APP.component.CkEditkor.init('dstexto-emissao-documento-texto');
        CKEDITOR.replace('dstexto-emissao-documento-texto');

    },

    getObjFormTexto: function () {

        var emissaoDocumentoFormTextoObj = CKEDITOR.instances['dstexto-emissao-documento-texto'].getData();

        return emissaoDocumentoFormTextoObj;

    },

    //Formulario FLUXO
    formFluxo: function () {

        this.setEditorFormFluxo();

    },

    setEditorFormFluxo: function () {

        // Carrega o arquivo de Configuracao utilizado pelo MxGraph
        var config = mxUtils.load('/Content/assets_src/js/vendor/mxGraph/javascript/src/config/diagrameditor.xml').getDocumentElement();

        // Carrega o editor
        editor = new mxEditor(config);

        // Inicializa o componente
        onInit(editor);

        function DisabledEditor(status) {
            editor.graph.setEnabled(status);
        }

        $("#Copy").click(function () {
            editor.execute('copy');
        });

        $("#Paste").click(function () {
            editor.execute('paste');
        });

        $("#Delete").click(function () {
            editor.execute('delete');
        });

        $("#Undo").click(function () {
            editor.execute('undo');
        });

        $("#Redo").click(function () {
            editor.execute('redo');
        });

        $("#Cut").click(function () {
            editor.execute('cut');
        });

        $("#View").click(function () {
            var graph = editor.graph;
            var document2 = mxUtils.show(graph, null, 10, 200);
            document2.body.style.overflow = "auto";
        });

        $("#zoomActual").click(function () {
            var graph = editor.graph;
            graph.zoomActual();
            graph.view.rendering = true;
            graph.refresh();
        });

        $("#zoomIn").click(function () {
            var graph = editor.graph;
            graph.zoomIn();
            graph.view.rendering = true;
            graph.refresh();
        });

        $("#zoomOut").click(function () {
            var graph = editor.graph;
            graph.zoomOut();
            graph.view.rendering = true;
            graph.refresh();
        });

        $("#zoomFit").click(function () {
            var graph = editor.graph;
            graph.fit();
            graph.view.rendering = true;
            graph.refresh();
        });

        $("#unselectAll").click(function () {
            var graph = editor.graph;
            graph.clearSelection();
        });

        $("#selectAll").click(function () {
            var graph = editor.graph;
            graph.selectAll();
        });

        $('#panel').on('onFullScreen.lobiPanel', function (ev, lobiPanel) {
            $('#graphContainer').height($("#panelBody").height() - 50);
        });

        $('#panel').on('onSmallSize.lobiPanel', function (ev, lobiPanel) {
            $('#graphContainer').height(300);
        });

        LoadFlow(editor);

        function onInit(editor) {
            var enabled = true;

            var page = APP.component.Util.getPage();

            if (page == "ConteudoDocumento") {
                enabled = false;
                $(".tollbarGraph").hide();
                editor.graph.setEnabled(false);

            }
            else if (page == "EmissaoDocumento") {
                var statusEtapa = parseInt($('[name=StatusEtapa]').val());

                if (statusEtapa == 1 || statusEtapa == 2) {
                    $("#menu-graph").hide();
                    $("#toolbar").hide();
                }
            }

            // Enables rotation handle
            mxVertexHandler.prototype.rotationEnabled = enabled;

            // Enables guides
            mxGraphHandler.prototype.guidesEnabled = enabled;

            // Alt disables guides
            mxGuide.prototype.isEnabledForEvent = function (evt) {
                return !mxEvent.isAltDown(evt);
            };

            // Enables snapping waypoints to terminals
            mxEdgeHandler.prototype.snapToTerminals = enabled;

            // Defines an icon for creating new connections in the connection handler.
            // This will automatically disable the highlighting of the source vertex.
            mxConnectionHandler.prototype.connectImage = new mxImage('/Content/assets_src/js/vendor/mxGraph/javascript/src/images/connector.gif', 16, 16);

            // Enables connections in the graph and disables
            // reset of zoom and translate on root change
            // (ie. switch between XML and graphical mode).
            editor.graph.setConnectable(enabled);

            // Clones the source if new connection has no target
            editor.graph.connectionHandler.setCreateTarget(enabled);

            //editor.graph.setEnabled(true);

        }

        function LoadFlow(editor) {

            var graph = editor.graph;
            var xmlString = $("#form-emissao-documento-fluxo-conteudo").val();
            if (xmlString != "") {
                var doc = mxUtils.parseXml(xmlString);
                var node = doc.documentElement;
                editor.readGraphModel(node);
            }

        }

    },

    getObjFormFluxo: function () {

        // Recupera os dados do plugin de fluxo
        var graph = editor.graph;
        var encoder = new mxCodec();
        var node = encoder.encode(graph.getModel());
        var dadosxml = mxUtils.getXml(node);
        var emissaoDocumentoFormFluxoObj = dadosxml;

        return emissaoDocumentoFormFluxoObj;

    },

    //Formulario REGISTRO
    formRegistro: function () {

        this.setNovoRegistroFormRegistro();
        this.setSaveNovoRegistroFormRegistro();
        this.setEditNovoRegistroFormRegistro();
        this.delNovoRegistroFormRegistro();

    },

    setNovoRegistroFormRegistro: function () {
        this.buttonAddNovoRegistroFormRegistro.unbind('click');
        this.buttonAddNovoRegistroFormRegistro.on('click', function () {
            event.preventDefault();

            var html = '';
            html += '<tr>';
            html += '<td>';
            html += '<textarea name="formRegistrosIdentificar" if="form-registros-identificar" maxlength="300"  class="form-control" data-val="true" title="" type="text" value=""></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<textarea name="formRegistrosArmazenar" if="form-registros-armazenar" maxlength="300"  class="form-control" data-val="true" title="" type="text" value=""></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<textarea name="formRegistrosProteger" if="form-registros-proteger" maxlength="300"  class="form-control" data-val="true" title="" type="text" value=""></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<textarea name="formRegistrosRecuperar" if="form-registros-recuperar" maxlength="300"  class="form-control" data-val="true" title="" type="text" value=""></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<textarea name="formRegistrosRetencao" if="form-registros-retencao" maxlength="300"  class="form-control" data-val="true" title="" type="text" value=""></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<textarea name="formRegistrosDisposicao" if="form-registros-disposicao" maxlength="300"  class="form-control" data-val="true" title="" type="text" value=""></textarea>';
            html += '</td>';
            html += '<td class="text-nowrap">';
            html += '<a href="#" class="editar-form-registros icon-cliente editar-color">';
            html += '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            html += '</a>';
            html += '<a href="#" data-valor="True" class="salvar-form-registros icon-cliente">';
            html += '<i class="fa fa-check  ativo-color" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i>';
            html += '</a>';
            html += '<a href="#" class="excluir-form-registros icon-cliente trash-color">';
            html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</a>';
            html += '</td>';
            html += '</tr>';

            $('#tb-form-registros tbody').append(html);

            APP.controller.ControlDocController.setup();
            APP.controller.ControlDocController.bindFormRegistro();

        });

    },

    setSaveNovoRegistroFormRegistro: function () {

        this.buttonSaveNovoRegistroFormRegistro.unbind('click');
        this.buttonSaveNovoRegistroFormRegistro.on('click', function () {
            event.preventDefault();

            $(this).closest('tr').find('[name=formRegistrosIdentificar]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRegistrosArmazenar]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRegistrosProteger]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRegistrosRecuperar]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRegistrosRetencao]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRegistrosDisposicao]').prop('disabled', true);
            editor.graph.setEnabled(false);
        });

    },

    setEditNovoRegistroFormRegistro: function () {

        this.buttonEditNovoRegistroFormRegistro.unbind('click');
        this.buttonEditNovoRegistroFormRegistro.on('click', function () {
            event.preventDefault();

            $(this).closest('tr').find('[name=formRegistrosIdentificar]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRegistrosArmazenar]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRegistrosProteger]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRegistrosRecuperar]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRegistrosRetencao]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRegistrosDisposicao]').prop('disabled', false);
            editor.graph.setEnabled(true);
        });

    },

    delNovoRegistroFormRegistro: function () {

        this.buttonDelNovoRegistroFormRegistro.unbind('click');
        this.buttonDelNovoRegistroFormRegistro.on('click', function (event) {
            event.preventDefault();

            var itemExcluir = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (result) {
                if (result) {
                    itemExcluir.remove();
                }

            });

        });

    },

    getObjFormRegistro: function () {

        var table = $('#tb-form-registros tbody');
        var arrayFormRegistrosObj = [];
        var registros = {};

        table.find('tr').each(function () {
            registros = {
                IdDocRegistro: $(this).find('[name=formRegistrosIdRegistroDoc]').val(),
                IdDocumento: $('[name=IdDocumento]').val(),
                Identificar: $(this).find('[name=formRegistrosIdentificar]').val(),
                Armazenar: $(this).find('[name=formRegistrosArmazenar]').val(),
                Proteger: $(this).find('[name=formRegistrosProteger]').val(),
                Retencao: $(this).find('[name=formRegistrosRetencao]').val(),
                Recuperar: $(this).find('[name=formRegistrosRecuperar]').val(),
                Disposicao: $(this).find('[name=formRegistrosDisposicao]').val(),
            };
            arrayFormRegistrosObj.push(registros);
        });

        return arrayFormRegistrosObj;

    },

    bindFormRegistro: function () {

        APP.controller.ControlDocController.setNovoRegistroFormRegistro();
        APP.controller.ControlDocController.setSaveNovoRegistroFormRegistro();
        APP.controller.ControlDocController.setEditNovoRegistroFormRegistro();
        APP.controller.ControlDocController.delNovoRegistroFormRegistro();

    },

    //------------------------------

    //Formulario INDICADORES
    formIndicadores: function () {

        this.setHideAndShowFormIndicadores();
        // -- nao this.setNovaRotinaFormRotina();
        this.setNovaIndicadoresFormIndicadores();
        this.getResponsavelImplementarIndicadores();
        this.setSaveNovaIndicadoresFormIndicadores();
        this.setEditNovaIndicadoresFormIndicadores();
        this.delNovaIndicadoresFormIndicadores();

    },

    setHideAndShowFormIndicadores: function () {
        //$('[name=formRotinaItem]').prop('disabled', true);
    },

    //setContNumberRotina: function () {
    //    $('[name=formRotinaItem]').each(function (i) {
    //        $(this).val(i + 1);
    //    });
    //},


    setNovaIndicadoresFormIndicadores: function () {
        this.buttonAddNovaIndicadoresFormIndicadores.unbind('click');
        this.buttonAddNovaIndicadoresFormIndicadores.on('click', function () {
            event.preventDefault();
            

            var TraducaoDropNameSelect = 'Selecione';

            var html = '';
            html += '<tr>';
            //html += '<td style="width: 5%;">';
            html += '<td>';
            html += '<textarea type="text" name="formIndicadoresObjetivo" maxlength="8000" rows="5" if="form-indicadores-objetivo" class="form-control"></textarea>';
            html += '</td>';

            //html += '<td>';
            //html += '<textarea type="text" name="formIndicadoresResponsavel" if="form-indicadores-responsavel" class="form-control"></textarea>';
            //html += '</td>';
            html += '<td>';

            html += '<select id="form-indicadores-responsavel" name="formIndicadoresResponsavel" class="form-control" ';
            html += 'data-msg-required="">';
            html += '<option value="">' + TraducaoDropNameSelect + '</option>';
            html += '</select>';
            html += '</td>';


            html += '<td>';
            html += '<input type="text" name="formIndicadoresMeta" if="form-indicadores-meta" class="form-control">';
            html += '</td>';

            html += '<td>';
            html += '<input type="text" name="formIndicadoresMetaMaximaMinima" if="form-indicadores-MetaMaximaMinima" >';
            html += '</td>';

            html += '<td>';
            html += '<textarea type="text" name="formIndicadoresIndicadores" maxlength="8000" rows="5" if="form-indicadores-indicadores" class="form-control"></textarea>';
            html += '</td>';

            html += '<td>';
            html += '<input type="text" name="formIndicadoresUnidadeMeta" if="form-indicadores-UnidadeMeta" class="form-control">';
            html += '</td>';





            html += '<td class="text-nowrap">';
            html += '<a href="#" class="editar-form-indicadores icon-cliente editar-color">';
            html += '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            html += '</a>';
            html += '<a href="#" data-valor="True" class="salvar-form-indicadores icon-cliente">';
            html += '<i class="fa fa-check  ativo-color" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i>';
            html += '</a>';
            html += '<a href="#" class="excluir-form-indicadores icon-cliente trash-color">';
            html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</a>';
            html += '</td>';
            html += '</tr>';

            $('#tb-form-indicadores tbody').append(html);



            APP.controller.ControlDocController.setup();
            APP.controller.ControlDocController.bindFormIndicadores();

        });
    },

    setSaveNovaIndicadoresFormIndicadores: function () {

        this.buttonSaveNovaIndicadoresFormIndicadores.unbind('click');
        this.buttonSaveNovaIndicadoresFormIndicadores.on('click', function () {
            event.preventDefault();
            $(this).closest('tr').find('[name=formIndicadoresResponsavel]').prop('disabled', true);
            $(this).closest('tr').find('[name=formIndicadoresObjetivo]').prop('disabled', true);
            $(this).closest('tr').find('[name=formIndicadoresResponsavel]').prop('disabled', true);
            $(this).closest('tr').find('[name=formIndicadoresMeta]').prop('disabled', true);
            $(this).closest('tr').find('[name=formIndicadoresIndicadores]').prop('disabled', true);
            $(this).closest('tr').find('[name=formIndicadoresUnidadeMeta]').prop('disabled', true);
            $(this).closest('tr').find('[name=formIndicadoresMetaMaximaMinima]').prop('disabled', true);
            editor.graph.setEnabled(false);


            //var itemAtual = $(this).closest('tr').find('[name=formRotinaItem]').val();
            //var total = $('#tb-form-rotina tbody tr').length;
            //var IdItemAtual = $(this).closest('tr').find('[name=formRotinaIdRotina]').val();

            //if (total > itemAtual && (IdItemAtual == null || IdItemAtual == undefined)) {

            //    var quantidadeLoop = total - itemAtual;

            //    for (var i = 0; i < quantidadeLoop; i++) {
            //        var $parent = $(this).parents('#tb-form-rotina tbody tr');
            //        $parent.insertBefore($parent.prev());
            //    }

            //    APP.controller.ControlDocController.setContNumberRotina();
            //}

        });

    },




    setEditNovaIndicadoresFormIndicadores: function () {
        
        this.buttonEditNovaIndicadoresFormIndicadores.unbind('click');
        this.buttonEditNovaIndicadoresFormIndicadores.on('click', function () {
            event.preventDefault();
            $(this).closest('tr').find('[name=formIndicadoresResponsavel]').prop('disabled', false);
            $(this).closest('tr').find('[name=formIndicadoresObjetivo]').prop('disabled', false);
            $(this).closest('tr').find('[name=formIndicadoresResponsavel]').prop('disabled', false);
            $(this).closest('tr').find('[name=formIndicadoresMeta]').prop('disabled', false);
            $(this).closest('tr').find('[name=formIndicadoresIndicadores]').prop('disabled', false);
            $(this).closest('tr').find('[name=formIndicadoresUnidadeMeta]').prop('disabled', false);
            $(this).closest('tr').find('[name=formIndicadoresMetaMaximaMinima]').prop('disabled', false);
            editor.graph.setEnabled(true);
        });

    },





    getResponsavelImplementarIndicadores: function () {
        
        var idSite = $('#emissao-documento-site').val();
        var idFuncao = 23; // Funcionalidade(Implementar aÃ§Ã£o) que permite usuario Implementar aÃ§Ã£o NC
        var idProcesso = $('[name=IdProcesso]').val();
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: '/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + idProcesso + ' &idSite=' + idSite + '&idFuncao=' + idFuncao + '',
            //beforeSend: function () {
            //    $('.add-acao-imediata').removeClass('show').addClass('hide');
            //},
            success: function (result) {
                if (result.StatusCode == 200) {
                    //APP.component.SelectListCompare.selectList(result.Lista, $('#tb-acao-imediata tbody tr:last-child [name="formAcaoImadiataTbResponsavelImplementar"] option'), $('#tb-acao-imediata tbody tr:last-child [name="formAcaoImadiataTbResponsavelImplementar"]'), 'IdUsuario', 'NmCompleto');
                    APP.component.SelectListCompare.selectList(result.Lista, $('#tb-form-indicadores tbody tr:last-child [name="formIndicadoresResponsavel"] option'), $('#tb-form-indicadores tbody tr:last-child [name="formIndicadoresResponsavel"]'), 'IdUsuario', 'NmCompleto');
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {
                //$('.add-acao-imediata').removeClass('hide').addClass('show');
            }
        });

    },




    delNovaIndicadoresFormIndicadores: function () {

        this.buttonDelNovaIndicadoresFormIndicadores.unbind('click');
        this.buttonDelNovaIndicadoresFormIndicadores.on('click', function (event) {
            event.preventDefault();

            var itemExcluir = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (result) {
                if (result) {
                    itemExcluir.remove();
                    APP.controller.ControlDocController.setContNumberRotina();
                }
            });

        });

    },


    getObjFormIndicadores: function () {

        var table = $('#tb-form-indicadores tbody');
        var arrayFormIndicadoresObj = [];
        var indicadores = {};

        table.find('tr').each(function () {
            indicadores = {
                IdIndicadores: $(this).find('[name=formIndicadoresIdIndicadores]').val(),
                Objetivo: $(this).find('[name=formIndicadoresObjetivo]').val(),
                IdResponsavel: $(this).find('[name=formIndicadoresResponsavel]').val(),
                IndicadoresMeta: $(this).find('[name=formIndicadoresMeta]').val(),
                Indicadores: $(this).find('[name=formIndicadoresIndicadores]').val(),
                IndicadoresUnidadeMeta: $(this).find('[name=formIndicadoresUnidadeMeta]').val(),
                IndicadoresMetaMaximaMinima: $(this).find('[name=formIndicadoresMetaMaximaMinima]').val(),
            };
            arrayFormIndicadoresObj.push(indicadores);

        });
        
        return arrayFormIndicadoresObj;

    },



    bindFormIndicadores: function () {
        APP.controller.ControlDocController.getResponsavelImplementarIndicadores();
        APP.controller.ControlDocController.setNovaIndicadoresFormIndicadores();
        APP.controller.ControlDocController.setSaveNovaIndicadoresFormIndicadores();
        APP.controller.ControlDocController.setEditNovaIndicadoresFormIndicadores();
        APP.controller.ControlDocController.delNovaIndicadoresFormIndicadores();
        //APP.controller.ControlDocController.getResponsavelImplementarAcaoImediata();
        
        //APP.controller.ControlDocController.setHideAndShowFormRotina();
        

        //APP.controller.ControlDocController.setContNumberRotina();

    },



    //------------------------------

















    //Formulario ROTINA
    formRotina: function () {

        this.setHideAndShowFormRotina();
        this.setNovaRotinaFormRotina();
        //this.setNovaRotinaFormIndicadores();
        this.setSaveNovaRotinaFormRotina();
        this.setEditNovaRotinaFormRotina();
        this.delNovaRotinaFormRotina();

    },

    setHideAndShowFormRotina: function () {

        //$('[name=formRotinaItem]').prop('disabled', true);

    },

    setContNumberRotina: function () {
        $('[name=formRotinaItem]').each(function (i) {
            $(this).val(i + 1);
        });
    },

    setNovaRotinaFormRotina: function () {
        this.buttonAddNovaRotinaFormRotina.unbind('click');
        this.buttonAddNovaRotinaFormRotina.on('click', function () {
            event.preventDefault();

            var html = '';
            html += '<tr>';
            html += '<td style="width: 5%;">';
            html += '<input type="text" name="formRotinaItem" if="form-rotina-item" class="form-control">';
            html += '</td>';
            html += '<td>';
            html += '<textarea type="text" name="formRotinaOQue" if="form-rotina-o-que" class="form-control"></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<textarea type="text" name="formRotinaQuem" if="form-rotina-quem" class="form-control"></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<textarea type="text" name="formRotinaRegistro" if="form-rotina-registro" class="form-control"></textarea>';
            html += '</td>';
            html += '<td style="width: 40%;">';
            html += '<textarea type="text" name="formRotinaComo" maxlength="8000" rows="5" if="form-rotina-como" class="form-control"></textarea>';
            html += '</td>';
            html += '<td class="text-nowrap">';
            html += '<a href="#" class="editar-form-rotina icon-cliente editar-color">';
            html += '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            html += '</a>';
            html += '<a href="#" data-valor="True" class="salvar-form-rotina icon-cliente">';
            html += '<i class="fa fa-check  ativo-color" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i>';
            html += '</a>';
            html += '<a href="#" class="excluir-form-rotina icon-cliente trash-color">';
            html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</a>';
            html += '</td>';
            html += '</tr>';

            $('#tb-form-rotina tbody').append(html);

            APP.controller.ControlDocController.setup();
            APP.controller.ControlDocController.bindFormRotina();

        });
    },


    setSaveNovaRotinaFormRotina: function () {

        this.buttonSaveNovaRotinaFormRotina.unbind('click');
        this.buttonSaveNovaRotinaFormRotina.on('click', function () {
            event.preventDefault();

            $(this).closest('tr').find('[name=formRotinaItem]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRotinaOQue]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRotinaQuem]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRotinaRegistro]').prop('disabled', true);
            $(this).closest('tr').find('[name=formRotinaComo]').prop('disabled', true);
            editor.graph.setEnabled(false);


            var itemAtual = $(this).closest('tr').find('[name=formRotinaItem]').val();
            var total = $('#tb-form-rotina tbody tr').length;
            var IdItemAtual = $(this).closest('tr').find('[name=formRotinaIdRotina]').val();

            if (total > itemAtual && (IdItemAtual == null || IdItemAtual == undefined)) {

                var quantidadeLoop = total - itemAtual;

                for (var i = 0; i < quantidadeLoop; i++) {
                    var $parent = $(this).parents('#tb-form-rotina tbody tr');
                    $parent.insertBefore($parent.prev());
                }

                APP.controller.ControlDocController.setContNumberRotina();

            }

        });

    },

    setEditNovaRotinaFormRotina: function () {

        this.buttonEditNovaRotinaFormRotina.unbind('click');
        this.buttonEditNovaRotinaFormRotina.on('click', function () {
            event.preventDefault();

            $(this).closest('tr').find('[name=formRotinaOQue]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRotinaQuem]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRotinaRegistro]').prop('disabled', false);
            $(this).closest('tr').find('[name=formRotinaComo]').prop('disabled', false);
            editor.graph.setEnabled(true);
        });

    },

    delNovaRotinaFormRotina: function () {

        this.buttonDelNovaRotinaFormRotina.unbind('click');
        this.buttonDelNovaRotinaFormRotina.on('click', function (event) {
            event.preventDefault();

            var itemExcluir = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (result) {
                if (result) {
                    itemExcluir.remove();
                    APP.controller.ControlDocController.setContNumberRotina();
                }
            });

        });

    },

    getObjFormRotina: function () {

        var table = $('#tb-form-rotina tbody');
        var arrayFormRotinasObj = [];
        var rotinas = {};

        table.find('tr').each(function () {
            rotinas = {
                IdDocRotina: $(this).find('[name=formRotinaIdRotina]').val(),
                IdDocumento: $('[name=IdDocumento]').val(),
                Item: $(this).find('[name=formRotinaItem]').val(),
                OQUE: $(this).find('[name=formRotinaOQue]').val(),
                QUEM: $(this).find('[name=formRotinaQuem]').val(),
                Registro: $(this).find('[name=formRotinaRegistro]').val(),
                Como: $(this).find('[name=formRotinaComo]').val(),
            };
            arrayFormRotinasObj.push(rotinas);

        });

        return arrayFormRotinasObj;

    },

    bindFormRotina: function () {

        APP.controller.ControlDocController.setHideAndShowFormRotina();
        APP.controller.ControlDocController.setNovaRotinaFormRotina();
        //APP.controller.ControlDocController.setNovaIndicadoresFormIndicadores();

        APP.controller.ControlDocController.setSaveNovaRotinaFormRotina();
        APP.controller.ControlDocController.setEditNovaRotinaFormRotina();
        APP.controller.ControlDocController.delNovaRotinaFormRotina();
        APP.controller.ControlDocController.setContNumberRotina();

    },

    //Formulario RECURSOS
    formRecursos: function () {

        this.setCkeditorFormRecursos();

    },

    setCkeditorFormRecursos: function () {

        APP.component.CkEditor.init('dstexto-emissao-documento-recursos');

    },

    getObjFormRecursos: function () {

        return CKEDITOR.instances['dstexto-emissao-documento-recursos'].getData();

    },

    //Formulario UPLOAD
    formUpload: function () {

        this.setdelUploadFormUpload();
        this.delUploadFormUpload();
        this.setupUploadArquivoFormUpload();

    },

    setdelUploadFormUpload: function () {
        this.buttonAddUploadFormUpload.unbind('click');
        this.buttonAddUploadFormUpload.on('click', function () {
            var index = $('#tb-form-upload tbody tr').size();
            event.preventDefault();

            var html = '';
            html += '<tr>';
            html += '<td>';
            html += '<input type="text" name="formUploadTitulo" id="form-upload-titulo" class="form-control"  value="" />';
            html += '</td>';
            html += '<td>';
            html += '<div id="uplPE-form-upload' + index + '" class="upload upload-file">';
            html += '<div id="dropPE-form-upload' + index + '" class="drop" style="padding:2px 3px; background-color:#fff;">';
            html += '<a href="javascript:;" class="text-center" style="color: #698e9f;">';
            html += '<i class="fa fa-paperclip fa-1x" aria-hidden="true"></i><br>Anexar</a>';
            html += '<input type="file" name="upl" multiple />';
            html += '<ul><!-- The file uploads will be shown here --></ul>';
            html += '<input type="hidden" id="arquivoEvidencia-form-upload' + index + '" value=""/></div></div>';
            html += '</td>';
            html += '<td>';
            html += '<button type="button" class="excluir-form-upload btn trash-color">';
            html += '<i class="fa fa-trash " aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</button>';
            html += '</td>';
            html += '</tr>';

            $('#tb-form-upload tbody').append(html);

            var dropPE = "dropPE-form-upload" + index;
            var uplPE = "uplPE-form-upload" + index;
            var arquivoEvidencia = "arquivoEvidencia-form-upload" + index;

            APP.controller.ControlDocController.setup();
            APP.controller.ControlDocController.bindFormUpload(dropPE, uplPE, arquivoEvidencia);

        });

    },

    delUploadFormUpload: function () {

        this.buttonDelUploadFormUpload.unbind('click');
        this.buttonDelUploadFormUpload.on('click', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
        });

    },

    setupUploadArquivoFormUpload: function (dropPE, uplPE, arquivoEvidencia) {

        // var formJson = {
        //     "tipo": "Upload"
        // };
        // APP.component.UploadFiles.setUploadFiles("/CriarCliente/UploadArquivo",
        //     dropPE,
        //     uplPE,
        //     arquivoEvidencia,
        //     30000000000,
        //     formJson,
        //     "form-cria-cliente",
        //     0,
        //     "/CriarCliente/RemoverArquivo");

        // UploadDeleteFiles("/CriarCliente/RemoverArquivo");
    },

    getObjFormUpload: function () {

        var table = $('#tb-form-upload tbody');
        var arrayFormUploadObj = [];
        var uploads = {};

        table.find('tr').each(function () {
            uploads = {
                Titulo: $(this).find('[name=formUploadTitulo]').val(),
                Arquivo: $(this).find('input[id^=arquivoEvidencia]').val().split("|")[0],
            };
            arrayFormUploadObj.push(uploads);
        });

        return arrayFormUploadObj;

    },

    bindFormUpload: function (dropPE, uplPE, arquivoEvidencia) {

        APP.controller.ControlDocController.setup();
        APP.controller.ControlDocController.setupUploadArquivoFormUpload(dropPE, uplPE, arquivoEvidencia);
        APP.controller.ControlDocController.setdelUploadFormUpload();
        APP.controller.ControlDocController.delUploadFormUpload();
    },

    //Formulario RISCOS
    formRiscos: function () {

        this.setHideAndShowFormRiscos();
        this.setNecessitaAcao();

        var IdProcesso = $('[name=formCadastroProcesso]').val();
        if (IdProcesso != "" && IdProcesso != null && IdProcesso != undefined) {
            APP.controller.ControlDocController.getResponsavel(IdProcesso);
        }


        APP.component.BarRating.init('.barraRating', 'bars-1to10');

    },

    setHideAndShowFormRiscos: function () {

        $('#form-riscos-numero').closest('.form-group').hide();
        $('[name=formRiscosResponsavel]').closest('[class^=col]').hide();
        //$('[name=formRiscosIdentificacao]').closest('[class^=col]').hide();
        //$('[name=formRiscosCriticidade]').closest('[class^=col]').hide();
        $('[name=formRiscosJustificativa]').closest('[class^=col]').hide();
    },

    getTemasCores: function () {

        var StatusEtapa = $('[name=StatusEtapa]').val();
        var divBarRating = $('.barraRating');
        var corRisco = $('[name=formRiscosCriticidade]').val();
        var lastCores = $("[data-rating-value='" + corRisco + "']").last().index();
        for (i = 0; i <= lastCores; i++) {
            $($('.br-theme-bars-1to10').find('.br-widget a')[i]).addClass('br-selected');
        }
        $($('.br-theme-bars-1to10').find('.br-widget a')[lastCores]).trigger("click");
        $('.br-theme-bars-1to10').find("[data-rating-value='" + corRisco + "']").addClass('br-current');

        if (StatusEtapa > 0) {
            $('.br-widget').addClass('barRating-disabled');
        }

    },

    getResponsavel: function (_idProcesso) {

        var idSite = $('[name=IdSite]').val();
        var idFuncao = 90;
        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + _idProcesso + '&idSite=' + idSite + '&idFuncao=' + idFuncao + '', (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name="formRiscosResponsavel"] option'), $('[name="formRiscosResponsavel"]'), 'IdUsuario', 'NmCompleto');
            }
        });
    },

    setNecessitaAcao: function () {

        $('[name=formRiscosNecessitaAcao]').unbind('change');
        $('[name=formRiscosNecessitaAcao]').on('change', function () {

            var necessitaAcao = APP.controller.ControlDocController.getNecessitaAcao();
            APP.controller.ControlDocController.setRulesNecessitaAcao(necessitaAcao);

        });

    },

    getNecessitaAcao: function () {

        var necessitaAcao = APP.component.Radio.init('formRiscosNecessitaAcao');
        return necessitaAcao;

    },

    setRulesNecessitaAcao: function (_necessitaAcao) {

        if (_necessitaAcao == "true") {
            $('[name=formRiscosResponsavel]').closest('[class^=col]').show();
            //$('[name=formRiscosIdentificacao]').closest('[class^=col]').show();
            //$('[name=formRiscosCriticidade]').closest('[class^=col]').show();
            $('[name=formRiscosJustificativa]').closest('[class^=col]').hide();
        } else {
            $('[name=formRiscosResponsavel]').closest('[class^=col]').hide();
            //$('[name=formRiscosIdentificacao]').closest('[class^=col]').hide();
            //$('[name=formRiscosCriticidade]').closest('[class^=col]').hide();
            $('[name=formRiscosJustificativa]').closest('[class^=col]').show();
        }

    },

    getObjFormRiscos: function () {
        var emissaoDocumentoFormRiscosObj = {

            CriticidadeGestaoDeRisco: $('.br-current').data('rating-value'),
            IdResponsavelInicarAcaoImediata: $('[name=formRiscosResponsavel] :selected').val(),
            DescricaoRegistro: $('[name=formRiscosIdentificacao]').val(),
            Causa: $('[name=formRiscosCausa]').val(),
            IdRegistroConformidade: $('#idgestaorisco').val(),
            DsJustificativa: $('[name=formRiscosJustificativa]').val()
        };

        return emissaoDocumentoFormRiscosObj;

    },

    //Formulario LICENCA
    formLicenca: function () {

        this.setUploadArquivoFormLicenca();

    },

    setUploadArquivoFormLicenca: function () {

        // var dropPE = "dropPE-novo-fluxo";
        // var uplPE = "uplPE-novo-fluxo";
        // var arquivoEvidencia = "arquivoEvidencia-novo-fluxo";

        // var formJson = {
        //     "tipo": "Licenca"
        // };
        // APP.component.UploadFiles.setUploadFiles("/ControlDoc/UploadArquivo",
        //     dropPE,
        //     uplPE,
        //     arquivoEvidencia,
        //     30000000000,
        //     formJson,
        //     "form-emissao-documento-licenca",
        //     0,
        //     "/ControlDoc/RemoverArquivo");

        // UploadDeleteFiles("/ControlDoc/RemoverArquivo");

    },

    getObjFormLicenca: function () {
        var anexoLicencaoModel = APP.controller.ControlDocController.models.AnexoModel;
        var nameImg = $('.btn-upload-file-form-licensa-anexo').find('i').text(); //Precisa pegar

        var anexo = anexoLicencaoModel.constructor(
            $('[name=formLicensaAnexoIdAnexo]').val(),
            nameImg,
            $('[name=formLicensaAnexo]').data('b64')
        );

        var emissaoDocumentoFormLicensaObj = {

            DataEmissao: $('[name=formLicencaDtEmissao]').val(),
            DataVencimento: $('[name=formLicencaDtVencimento]').val(),
            IdAnexo: 0,
            Anexo: anexo
        };

        return emissaoDocumentoFormLicensaObj;

    },

    //Formulario DOCS EXTERNOS
    formDocsExternos: function () {
        this.setupUploadArquivoFormDocsExternos();
    },

    setupUploadArquivoFormDocsExternos: function () {
        var btnUploadDocExterno = $(".btn-file-form-docexterno-anexo");

        btnUploadDocExterno.unbind('click');
        btnUploadDocExterno.on('click', function (event) {
            event.preventDefault();

            $('#form-docexterno-anexo').trigger("click");

            var idInput = "form-docexterno-anexo";
            var base64;

            $('#' + idInput + '').unbind('change');
            $('#' + idInput + '').on('change', function () {
                var _files = this.files;
                if (_files && _files[0]) {

                    var reader = new FileReader();
                    var name = "";

                    reader.onload = function (e) {

                        name = $('#' + idInput + '').prop('files')[0].name;
                        base64 = e.target.result.split(',');

                        $('#form-docexterno-anexo').attr('data-b64', '' + base64[1] + '');
                        $("[name=formDocExternoAnexoNome]").val(name);

                        var _file = $("#formDocExternoAnexoUpload");
                        _file.attr("href", e.target.result);
                        _file.attr("download", name);
                        _file.html(name);

                    };

                    reader.readAsDataURL(_files[0]);
                }
            });
        });
    },

    getObjFormDocsExternos: function () {
        var anexoDocExternoModel = APP.controller.ControlDocController.models.AnexoModel;
        var nameImg = $('#formDocExternoAnexoUpload').attr("download"); //Precisa pegar

        var anexo = anexoDocExternoModel.constructor(
            $('[name=formDocExternoAnexoIdAnexo]').val(),
            nameImg,
            $('#form-docexterno-anexo').data('b64')
        );

        var emissaoDocumentoFormDocsExternosObj = {
            LinkDocumentoExterno: $('[name=formDocsexternosLink]').val(),
            IdAnexo: $('[name=formDocExternoAnexoIdAnexo]').val(),
            IdDocExterno: $('[name=formDocExternoAnexoIdDocExterno]').val(),
            Anexo: anexo,
        };

        return emissaoDocumentoFormDocsExternosObj;

    },

    //Formulario ASSUNTOS
    formAssuntos: function () {

        APP.component.DataTable.init('#tb-emissao-documento-form-assuntos');
        this.setHideAndShowAssunto();
        this.setNovoAssuntoFormAssuntos();
        this.setSaveAssuntoFormAssuntos();
        this.setEditAssuntoFormAssuntos();

    },

    setHideAndShowAssunto: function () {

        var empty = $('#tb-emissao-documento-form-assuntos tbody tr td').hasClass('dataTables_empty');
        if (empty) {
            $('.odd').remove();
        }
    },

    setNovoAssuntoFormAssuntos: function () {
        this.buttonAddNovoAssuntoFormAssuntos.unbind('click');
        this.buttonAddNovoAssuntoFormAssuntos.on('click', function () {
            event.preventDefault();

            var html = '';
            html += '<tr>';

            html += '<td>';
            html += '<input type="text" name="formAssuntoRevisao" class="form-control" disabled id="form-assunto-revisao"> ';
            html += '</td>';

            html += '<td>';
            html += '<div class="input-group date" id="datetimepicker77">';
            html += '<input type="text" name="formAssuntoDtVersao" class="form-control data datepicker" />';
            html += '<span class="input-group-addon">';
            html += '<i class="fa fa-calendar" aria-hidden="true"></i>';
            html += '</span>';
            html += '</div>';
            html += '</td>';

            html += '<td>';
            html += '<textarea name="formAssuntoDescricao" class="form-control" id="form-assunto-descricao" data-val="true"  rows="5"></textarea>';
            html += '</td>';
            html += '<td class="text-nowrap">';
            html += '<a href="#" class="editar-form-assuntos icon-cliente editar-color">';
            html += '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            html += '</a>';
            html += '<a href="#" data-valor="True" class="salvar-form-assuntos icon-cliente">';
            html += '<i class="fa fa-check  ativo-color" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i>';
            html += '</a>';
            //html += '<a href="#" class="excluir-form-assuntos icon-cliente trash-color">';
            //html += '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            //html += '</a>';
            html += '</td>';
            html += '</tr>';



            $('#tb-emissao-documento-form-assuntos tbody').append(html);

            APP.controller.ControlDocController.setup();
            APP.controller.ControlDocController.bindFormAssuntos();

        });

    },

    setSaveAssuntoFormAssuntos: function () {

        this.buttonSaveNovoAssuntoFormAssuntos.unbind('click');
        this.buttonSaveNovoAssuntoFormAssuntos.on('click', function () {
            event.preventDefault();

            $(this).closest('tr').find('[name=formAssuntoDtVersao]').prop('disabled', true);
            $(this).closest('tr').find('[name=formAssuntoRevisao]').prop('disabled', true);
            $(this).closest('tr').find('[name=formAssuntoDescricao]').prop('disabled', true);
            //editor.graph.setEnabled(false);
        });

    },

    setEditAssuntoFormAssuntos: function () {

        this.buttonEditNovoAssuntoFormAssuntos.unbind('click');
        this.buttonEditNovoAssuntoFormAssuntos.on('click', function () {
            event.preventDefault();

            $(this).closest('tr').find('[name=formAssuntoDtVersao]').prop('disabled', false);
            $(this).closest('tr').find('[name=formAssuntoRevisao]').prop('disabled', false);
            $(this).closest('tr').find('[name=formAssuntoDescricao]').prop('disabled', false);
            //editor.graph.setEnabled(true);
        });

    },

    delAssuntoFormAssuntos: function () {

        this.buttonDelAssuntoFormAssuntos.unbind('click');
        this.buttonDelAssuntoFormAssuntos.on('click', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
        });

    },

    getObjFormAssuntos: function () {

        var table = $('#tb-emissao-documento-form-assuntos tbody');
        var arrayFormAssuntosObj = [];
        var assuntos = {};

        table.find('tr').each(function () {
            assuntos = {
                Id: $(this).find('[name=formAssuntoId]').val(),
                DataAssunto: $(this).find('[name=formAssuntoDtVersao]').val(),
                Revisao: $(this).find('[name=formAssuntoRevisao]').val(),
                Descricao: $(this).find('[name=formAssuntoDescricao]').val(),
                IdDocumento: $("#emissao-documento-IdDocumento").val(),
            };
            arrayFormAssuntosObj.push(assuntos);
        });

        return arrayFormAssuntosObj;
    },

    bindFormAssuntos: function () {

        APP.controller.ControlDocController.setNovoAssuntoFormAssuntos();
        APP.controller.ControlDocController.setSaveAssuntoFormAssuntos();
        APP.controller.ControlDocController.setEditAssuntoFormAssuntos();
        APP.controller.ControlDocController.delAssuntoFormAssuntos();
        APP.controller.ControlDocController.setNumeroRevisaoAssunto();

        APP.component.Datapicker.init();

    },

    setNumeroRevisaoAssunto: function () {
        $('[name=formAssuntoRevisao]').each(function (i) {
            $(this).val(i + 1);
        });
    },

    //Formulario Comentarios
    formComentarios: function () {

        this.setNovaMensagemFormComentarios();

    },

    setNovaMensagemFormComentarios: function () {

        this.buttonAddNovaMensagemFormComentarios.unbind('click');
        this.buttonAddNovaMensagemFormComentarios.on('click', function () {
            event.preventDefault();

            var usuario = $('[name=IdUsuarioLogado]').val();
            var nome = $('[name=NmUsuarioLogado]').val();
            var comentario = $('[name=formComentariosMensagem]').val();
            var data = APP.component.Datatoday.init();
            var hora = APP.component.Datatoday.getHoraNow();

            var html = '';
            html += '<!-- Comentario -->';
            html += '<div class="form-comentarios-comentario-info">';
            html += '<input type="hidden" name="IdComentario" value="0" />';
            html += '<span name="formComentariosComentarioUsuario" class="form-comentarios-comentario-usuario">' + nome + ' -</span>';
            html += '<p name="formComentariosComentarioTexto" class="form-comentarios-comentario-texto">&nbsp;' + comentario + '</p>';
            html += '<span name="formComentariosComentarioData" class="form-comentarios-comentario-data">' + data + ' - ' + hora + '</span>';
            html += '</div>';

            $('#form-comentarios-comentario').append(html);
            document.getElementById("form-emissao-documento-comentarios").reset();
            APP.controller.ControlDocController.setup();

        });

    },

    getObjFormComentarios: function () {

        var table = $('#form-comentarios-comentario');
        var arrayFormComentariosObj = [];
        var comentarios = {};

        table.find('.form-comentarios-comentario-info').each(function () {
            comentarios = {
                Id: $(this).find('[name=IdComentario]').val(),
                IdUsuario: $('[name=IdUsuarioLogado]').val(),
                Descricao: $(this).find('[name=formComentariosComentarioTexto]').text(),
                DataComentario: $(this).find('[name=formComentariosComentarioData]').text(),
                IdDocumento: $("#emissao-documento-IdDocumento").val()
            };
            arrayFormComentariosObj.push(comentarios);

        });

        return arrayFormComentariosObj;
    },

    setTodosOsCamposNecessariosForamPreenchidos: function () {

        if ($('[name=formCargosEscolha]:checked').length == 0) {
            bootbox.alert(_options.MsgCargoSelect);
            return false;
        }

        if ($('[name=formCadastroEscolha]:checked').length == 0) {
            $('.tabela-check').addClass("input-validation-error");
            return false;
        }

        return true;
    },

    //Formulario CARGOS
    formCargos: function () {

        this.getSelectAllCargos();

    },

    getSelectAllCargos: function () {

        $('#form-cargos-escolha-all').unbind('click');
        $('#form-cargos-escolha-all').on('click', function () {
            $('[name=formCargosEscolha]').not(this).prop('checked', this.checked);
        });

    },

    setCargosEmissaoDocumentos: function () {

        $.ajax({
            type: "GET",
            data: {
                id: $('#emissao-documento-IdDocumento').val()
            },
            dataType: 'json',
            url: '/ControlDoc/CargosPorDocumento',
            beforeSend: function () { },
            success: function (result) {
                if (result.StatusCode == 200) {

                    $.each(result.Dados, function (index, cargo) {
                        APP.controller.ControlDocController.addCargosEmissaoDocumentos(cargo);
                    });
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) { }
        });

    },

    addCargosEmissaoDocumentos: function (cargo) {

        var $divCargos = $(".tabela-cargo-check");

        var html = '';
        html += '<div class="col-md-12 checkbox">';
        html += '<input type="checkbox" name="formCargosEscolha" id="form-cargos-escolha-cargo' + cargo.IdCargo + '" class="form-control" value="' + cargo.IdCargo + '" data-idDocCargo="' + cargo.IdDocCargo + '" ' + (cargo.IdDocCargo != 0 ? 'checked' : '') + '>';
        html += '<label for="form-cargos-escolha-cargo' + cargo.IdCargo + '">' + cargo.NmNome + '</label>';
        html += '</div>';

        $divCargos.append(html);

    },

    getObjFormCargos: function () {

        var table = $('.tabela-cargo-check');
        var arrayFormCargosObj = [];
        var cargos = {};

        table.find('[name=formCargosEscolha]:checked').each(function () {
            cargos = {
                Id: $(this).data("iddoccargo"),
                IdCargo: $(this).val(),
                IdDocumento: $("#emissao-documento-IdDocumento").val(),

            };
            arrayFormCargosObj.push(cargos);
        });

        return arrayFormCargosObj;

    },

    //Funcoes Emissao Documento Edit
    setHideAndShowEdit: function () {

        //Hide All
        $('div[id^=panel-form-]').hide();

        var idDocumento = $("#emissao-documento-IdDocumento").val();

        if (idDocumento == null || idDocumento == undefined || idDocumento == "0") {
            $('.nav-pills li').hide();
        }
        else {
            $(".menu-one").show();
            var tam = $('[id^=panel-tab-]:visible').size();
            APP.controller.ControlDocController.setTabs(tam);
        }

        $('#panel-form-cadastro').show();
        $('.btn-enviar-verificacao').hide();
        $('.btn-enviar-aprovacao').hide();
        $('.btn-voltar-elaboracao').hide();
        $('.btn-aprovar').hide();

        APP.controller.ControlDocController.hideOrShowWorkFlow();
        $('[name=formCadastroWorkflow]').trigger('change');

    },

    setHideButton: function () {

        $('.btn-enviar-verificacao').hide();
        $('.btn-enviar-aprovacao').hide();
        $('.btn-voltar-elaboracao').hide();
        $('.btn-aprovar').hide();

    },

    setWorkFlow: function () {

        $('[name=formCadastroWorkflow]').on('change', function () {

            var workFlow = APP.controller.ControlDocController.getWorkFlow();
            APP.controller.ControlDocController.setRulesWorkFlow(workFlow);

        });

    },

    getWorkFlow: function () {

        var workFlow = APP.component.Radio.init('formCadastroWorkflow');
        return workFlow;

    },

    setRulesWorkFlow: function (_workFlow) {

        var statusEtapa = parseInt($('[name=StatusEtapa]').val());

        if (_workFlow == 'sim') {
            switch (statusEtapa) {
                case 0:
                    $('.btn-enviar-verificacao').show();
                    break;
                case 1:
                    APP.controller.ControlDocController.setDisableVerificacao(statusEtapa);
                    $('.btn-enviar-aprovacao').show();
                    $('.btn-voltar-elaboracao').show();
                    $("input").attr("disabled", "disabled");
                    $(".quatro").hide();
                    $(".close").removeAttr("disabled");
                    editor.graph.setEnabled(false);
                    break;
                case 2:
                    APP.controller.ControlDocController.setDisableVerificacao(statusEtapa);
                    $('.btn-aprovar').show();
                    $('.btn-voltar-elaboracao').skjhgeds
                    $("input").attr("disabled", "disabled");
                    $("#form-cargos-escolha-all").removeAttr("disabled");
                    $(".closeCargos").removeAttr("disabled");
                    editor.graph.setEnabled(false);
                    //editor.graph.setHideButton();

                    break;
                case 3:
                    APP.controller.ControlDocController.setDisableVerificacao(statusEtapa);
                    break;
            }
        } else {
            APP.controller.ControlDocController.setHideButton();
        }

    },

    setDisableVerificacao: function (_statusEtapa) {

        $("[id^=form-emissao-documento-] :input").attr("disabled", true);
        $("[id^=tb-form-] tbody tr td:last-child").hide();
        $("#form-cadastro-sigla-i").hide();
        $("#form-cadastro-categoria-i").hide();
        $(".novo").hide();
        $('.btn-salvar').hide();


        if (_statusEtapa == 1) {
            $("#form-emissao-documento-comentarios :input").prop("disabled", false);
        } else if (_statusEtapa == 3) {
            $("#form-emissao-documento-assuntos :input").prop("disabled", false);
        }

    },

    hideOrShowWorkFlow: function () {

        var RadioFormCadastroWorkflow = APP.component.Radio.init('formCadastroWorkflow');

        if (RadioFormCadastroWorkflow != "sim") {
            $('#form-cadastro-verificador').closest('.form-group').hide();
            $('#form-cadastro-aprovador').closest('.form-group').hide();
        } else {
            $('#form-cadastro-verificador').closest('.form-group').show();
            $('#form-cadastro-aprovador').closest('.form-group').show();
        }
    },

    setShowTemplatesEmissaoDocumento: function () {
        $('[name=formCadastroEscolha]:checked').each(function () {
            var templateUtilizado = $(this).attr("id").split("-")[3];
            $("#panel-form-" + templateUtilizado).show();
        });
    },

};
