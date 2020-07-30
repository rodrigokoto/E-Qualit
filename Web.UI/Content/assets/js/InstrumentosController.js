
/*
|--------------------------------------------------------------------------
| Controlador Instrumentos
|--------------------------------------------------------------------------
*/

APP.controller.InstrumentosController = {

    init: function () {
        var page = APP.component.Util.getPage();

        this.setup();
        if (page == "IndexInstrumentos") {
            this.IndexInstrumentos();
        } else if (page == "CriarInstrumentos") {
            this.CriarInstrumentos();
        }

    },

    setup: function () {

        //Instrumento Index

        //Instrumento Criar
        this.radioButton = $("input:radio[name='SistemaDefineStatus']");
        this.buttonAddCalibracao = $('.tb-calibracao-add');
        this.buttonDelCalibracao = $('.tb-calibracao-excluir');
        this.buttonDelParametroCalibracao = $('.form-pos-calibracao-parametros-icone-del');
        this.buttonAddParametrosCalibracao = $('.form-pos-calibracao-parametros-icone-add');
        this.buttoEditParametroCalibracao = $('.tb-calibracao-editar');
        this.buttonSaveForm = $(".form-save");
        this.buttonDelInstrumento = $('.tb-instrumento-excluir');
        this.buttonImprimir = $("#btn-imprimir");
        this.buttonStatus = $(".btn-status");
        this.buttonEmCalibracao = $(".btn-EmCalibracao");
        this.buttonForaDeUso = $(".btn-ForaDeUso");
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
    IndexInstrumentos: function () {

        APP.component.DataTable.init('#tb-instrumento');
        this.delInstrumento();


    },

    CriarInstrumentos: function () {

        if (dtCalibracao !== null) {
            $("input[name='DtRegistro']").datepicker({
                dateFormat: _options.datepicker,
                pickTime: true,
            });
        }

        //GETs
        this.getResponsavel();
        this.getResponsavelAprovadorCalibracao();
        //SETs
        this.setCalibracao();
        this.setCalculaDoSistema();
        this.setParametrosCalibracao();
        this.setResponsavelInstrumento();
        this.setComboSigla();
        this.setSigla();
        //DELs
        this.delCalibracao();
        this.delParametroCalibracao();
        //EDTs
        this.editPosCalibracao();
        //SENDs
        //this.sendFormParametroInstrumento();
        //this.sendFormPoscalibracao();
        this.saveForms();
        //OUTRAS
        this.viewArquivoCalibracao();
        this.calcCriterioAceitacao();
        this.bind();

        this.eventoImprimir();
        this.eventoAlterarStatus();

        //INCLUDEs
        APP.component.AtivaLobiPanel.init();
        APP.component.FileUpload.init();
        APP.component.Datapicker.init();
        APP.component.DataTable.init('#tb-calibracao');

        $.fn.dataTable.moment('DD/MM/YYYY');    //Formatação sem Hora

        tabelaCalibracao = $('#tb-calibracao').DataTable();
        tabelaCalibracao.order([3, 'desc']);
        tabelaCalibracao.draw();
        // APP.controller.InstrumentosController.setupUploadArquivoCalibracao();
        //OUTRASfimp
        this.changeCadastroSigla();
        this.setCalibracaoVisivel();
        this.setValidateForms();

    },

    eventoImprimir: function () {


        this.buttonImprimir.on('click', function () {

            var IdInstrumento = $("#IdInstrumento").val();
            APP.controller.InstrumentosController.imprimir(IdInstrumento);

        });

    },



    eventoAlterarStatus: function () {
        var status = document.getElementById('IdStatusHidden');
        var lblstatus = $('.status-aprovado');

        var statusB = status;
        var lblstatusB = lblstatus;
        this.buttonStatus.on('click', function () {
            $('#modal-alter-status').modal("show");
        });

        this.buttonEmCalibracao.on('click', function () {
            status.value = 3;

            lblstatus[0].className = '';
            lblstatus[0].className = 'status-em-calibracao status-aprovado';
            lblstatus[0].innerHTML = 'Em Calibração';

        });

        this.buttonForaDeUso.on('click', function () {
            status.value = 2;

            lblstatus[0].className = '';
            lblstatus[0].className = 'status-fora-de-uso status-aprovado';
            lblstatus[0].innerHTML = 'Fora de Uso';
        });

        this.buttonModalCancelar.on('click', function () {
            status.value = statusB;
            lblstatus[0] = lblstatusB[0];
        });
    },



    imprimir: function (IdInstrumento) {



        if (IdInstrumento != null) {

            APP.component.Loading.showLoading();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/Instrumento/PDF?id=' + IdInstrumento, true);
            xhr.responseType = 'arraybuffer';
            xhr.onload = function (e) {
                if (this.status == 200) {
                    var blob = new Blob([this.response], { type: "application/pdf" });
                    var pdfUrl = URL.createObjectURL(blob);
                    printJS(pdfUrl);
                }

                APP.component.Loading.hideLoading();

            };

            xhr.send();

        }
    },

    changeCadastroSigla: function () {

        $('[name=IdSigla]').change(function () {

            var sigla = $(this).val();

            $.post('/Instrumento/RetornaNumeroPorSigla/' + sigla, function (data, status) { }).done(function (data) {
                if (data.StatusCode == "200") {

                    $('[name=Numero]').val(data.Retorno);

                }
            });

        });

    },

    setValidateForms: function () {


        var ObjFormInstrumentoValidate = APP.controller.InstrumentosController.getObjObjFormInstrumentoValidate();
        APP.component.ValidateForm.init(ObjFormInstrumentoValidate, '#form-parametro-instrumento');

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

    //Funcoes Index Instrumentos
    delInstrumento: function () {


        var tabelaInstrumento = $('#tb-instrumento').DataTable();

        this.buttonDelInstrumento.on('click', function (event) {
            event.preventDefault();

            var IdInstrumento = $(this).data('id');
            var $rowAtual = $(this).parents('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (result) {
                if (result) {
                    $.post('/Instrumento/Excluir/' + IdInstrumento, function (data, status) {
                    }).done(function (data) {
                        if (data.StatusCode == "200") {
                            tabelaInstrumento.row($rowAtual).remove().draw();
                            bootbox.alert(_options.MsgRegistroExcluidoSucesso);
                        }
                    });
                }
            });
        });

    },

    editIntrumento: function () {


        var IdInstrumento = $('#IdInstrumento');
        var instrumento = $('#form-parametro-instrumento').serialize();
        $.ajaxSetup({ async: false });

        $.post('/Instrumento/Editar/', instrumento, function (data, status) {
        })
            .success(function (data, status) {

            })
            .fail(function (data, status) {

            });

        $.ajaxSetup({ async: true });
    },

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

    getResponsavelAprovadorCalibracao: function () {


        var idSite = $('#IdSite').val();
        var idFuIdAprovadorCalibracaoncao = $('#IdAprovadorCalibracao').val();
        var url = '/Usuario/ObterUsuariosPorFuncao?idSite=' + idSite + '&idFuncao=61';
        $.ajax({
            type: "GET",
            async: false,
            dataType: 'json',
            url: url,
            success: function (result) {
                $.each(result.Lista, function (key, val) {
                    $('.form-pos-calibracao-aprovado-por').append($("<option />").val(val.IdUsuario).text(val.NmCompleto));
                });
            }
        });
    },

    getObjPosCalibracao: function () {


        var PosCalibracao = {
            IdCalibracao: $("#IdCalibracao").val(),
            IdFilaEnvio: $("#IdFilaEnvio").val(),
            IdInstrumento: $('#IdInstrumento').val(), 'required': true, 'minlength': 1, 'maxlength': 500,
            IdSigla: $('[name=IdSigla]').val(),
            DataRegistro: $('#form-pos-calibracao').find('[name=DtRegistro]').val(),
            DataNotificacao: $('#form-pos-calibracao').find('[name=DtNotificacao]').val(),
            DataProximaCalibracao: $('#form-pos-calibracao').find('[name=DataProximaCalibracao]').val(),
            Certificado: $('#form-pos-calibracao').find('[name=Certificado]').val(),
            CriterioAceitacao: [],
            OrgaoCalibrador: $('#form-pos-calibracao').find('[name=OrgaoCalibrador]').val(),
            Aprovador: $('#form-pos-calibracao').find('[name=Aprovador]').val(),
            Aprovado: $('input[name="Aprovado"]:checked').val(),
            ArquivoCertificadoAux: APP.controller.InstrumentosController.getArquivoCertificadoAnexos($("#IdCalibracao").val()),
            SubmitArquivosCertificado: APP.controller.InstrumentosController.getArquivoCertificadoAnex2($("#IdCalibracao").val()),
            Observacoes: $('#form-pos-calibracao').find('[name=Observacoes]').val(), 'required': true, 'minlength': 1, 'maxlength': 500,
            NomeUsuarioAprovador: $('#form-pos-calibracao').find('[name=Aprovador] option:selected').text(),
        };

        var temCriterioAceitacao = $("[name=SistemaDefineStatus]:checked").val() == "true" ? true : false;

        if (temCriterioAceitacao) {

            PosCalibracao.Instrumento =
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
                    IdCalibracao: $("#IdCalibracao").val(),
                    Aceito: $(this).find('.ativo-color').length == 1 ? true : false,
                };

                PosCalibracao.CriterioAceitacao.push(CriteriosAceitacao);
            });
        }

        return PosCalibracao;
    },

    getArquivoCertificadoAnexos: function (IdCalibracao) {
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

    getArquivoCertificadoAnex2: function (IdCalibracao) {
        let raiz = $("#modal-raiupacaoimeidata" + IdCalibracao)[0];
        let ret = FileUploadGlobal_getArrArquivoRaiz(raiz, "IdArquivoCertificadoAnexo", "IdCalibracao");
        return ret;

    },

    getEditPosCalibracao: function (result) {


        APP.controller.InstrumentosController.showPosCalibracao();

        $('#form-pos-calibracao').find("[name=EdicaoCalibracao]").val("1");
        $('#IdInstrumento').val(result.IdInstrumento);
        $('#IdFilaEnvio').val(result.IdFilaEnvio);
        $('#form-pos-calibracao').find('[name=DtRegistro]').val(result.DataRegistro);
        $('#form-pos-calibracao').find('[name=DtNotificacao]').val(result.DataNotificacao);
        $('#form-pos-calibracao').find('[name=DataProximaCalibracao]').val(result.DataProximaCalibracao);
        $('#form-pos-calibracao').find('[name=Certificado]').val(result.Certificado);

        $('#form-pos-calibracao').find('[name=OrgaoCalibrador]').val(result.OrgaoCalibrador);



        if (result.Aprovado == 0) {
            $('#form-pos-calibracao-nao-aprovado').prop('checked', true);
        }
        else if (result.Aprovado == 1) {
            $('#form-pos-calibracao-aprovado').prop('checked', true);
        }
        else if (result.Aprovado == 2) {
            $('#form-pos-calibracao-fora-uso').prop('checked', true);
        }
        else if (result.Aprovado == 3) {
            $('#form-pos-calibracao-em-calibracao').prop('checked', true);
        }




        $('.form-pos-calibracao-aprovado-por').val(result.Aprovador.IdUsuario);
        //$('#form-pos-calibracao').find('.arquivo').val(result.ArquivoCertificado);
        var stringHtml = '';

        for (var i = 0; i < result.ArquivoCertificado.length; i++) {
            var evidencia = result.ArquivoCertificado[i];
            stringHtml += '<li>';
            stringHtml += '<a href="' + evidencia.ArquivoB64 + '" target="_blank" download="' + evidencia.Nome + '" >' + evidencia.Nome + '</a >';
            stringHtml += '<input type="hidden" name="formCriarNaoConformidadeEvidenciaNome" value="' + evidencia.Nome + '">';
            stringHtml += '<input type="hidden" name="formCriarNaoConformidadeEvidenciaIdAnexo" value="' + evidencia.IdAnexo + '">';
            stringHtml += '</li>';
        }

        $('.ulAnexos').html(stringHtml);
        APP.core.Main.downloadArquivo();

        for (var i = 0; i < result.CriteriosAceitacao.length; i++) {
            APP.controller.InstrumentosController.addCriterioAceitacao(result.CriteriosAceitacao[i]);
        }

        //Set Radio
        var $radios = $('input:radio[name=Aprovado]');
        if ($radios.is(':checked') === false) {
            $radios.filter('[value=' + result.Aprovado + ']').prop('checked', true);
        }

        // var tamanhoSplit = result.ArquivoCertificado.split("/").length - 1;
        // var nomeArquivo = result.ArquivoCertificado.split("/")[tamanhoSplit];

        // $('#form-pos-calibracao').find('.arquivo').attr("href", result.ArquivoCertificado).html(nomeArquivo);

        // $('#form-pos-calibracao').find('.arquivo').append('<a href="javascript:;" class="deletefile" target="_blank" data-tipo="undefined" data-arquivo="' + nomeArquivo + '" data-campo="ArquivoCertificado" data-id="1"><i class="fa fa-trash-o" aria-hidden="true"></i></a>');

        $('#form-pos-calibracao').find('[name=Observacoes]').val(result.Observacoes);
    },

    setCalibracao: function () {



        this.buttonAddCalibracao.bind('click', function () {
            event.preventDefault();
            APP.controller.InstrumentosController.showPosCalibracao();
        });

    },

    showPosCalibracao: function () {


        $('.tb-calibracao').slideUp(500);
        $('#form-pos-calibracao').removeClass('hide').addClass('show');
        $('#form-pos-calibracao').slideDown(500);
        $('.pills-parametros-pos-calibracao').removeClass('hide').addClass('show');
        $('.pills-tabela-calibracao').removeClass('show').addClass('hide');
        APP.controller.InstrumentosController.showTablePosCalibracaoParametros();
        //transfere o botão correto
        $("#uploadbotaodestino").html($("#uploadbotao0").html());


    },

    showTablePosCalibracaoParametros: function () {


        var nomeRadio = 'SistemaDefineStatus';
        var radio = APP.component.Radio.init(nomeRadio);
        if (radio == "true") {
            $('.form-pos-calibracao-parametros').removeClass('hide').addClass('show');
        } else {
            $('.form-pos-calibracao-parametros').removeClass('show').addClass('hide');
        }
    },

    setCalculaDoSistema: function () {


        this.radioButton.bind('click', function () {
            APP.controller.InstrumentosController.showTablePosCalibracaoParametros();
        });

    },

    setParametrosCalibracao: function () {




        this.buttonAddParametrosCalibracao.on('click', function (event) {
            event.preventDefault();
            var html = '';
            html += '<tr class="campos-tabela">';
            html += '<td align="center"><i class="fa fa-minus-circle blue-color form-pos-calibracao-parametros-icone-del" aria-hidden="true"></i></td>';
            html += '<td><input onkeyup="SubstituiVirgulaPorPonto(this)" style="/* width: 76px; */" class="form-control form-pos-calibracao-parametros-periodicidade" name="Periodicidade" data-val="true" title="" type="text" value="" data-original-title="Range"  ></td>';
            html += '<td><input onkeyup="SubstituiVirgulaPorPonto(this)" style="/* width: 76px; */" class="form-control form-pos-calibracao-parametros-erro" name="Erro" data-val="true" title="" type="text" value="" data-original-title="Erro"></td>';
            html += '<td align="center"><i class="fa fa-plus blue-color" aria-hidden="true"></i></td>';
            html += '<td><input onkeyup="SubstituiVirgulaPorPonto(this)" style="/* width: 76px; */" class="form-control form-pos-calibracao-parametros-incerteza" name="Incerteza" data-val="true" title="" type="text" value="" data-original-title="Incerteza"></td>';
            html += '<td align="center"><i class="fa fa-equals blue-color" aria-hidden="true"></i></td>';
            html += '<td><input class="form-control form-pos-calibracao-parametros-resultado" style="/* width: 76px; */" name="Resultado" data-val="true" title="" type="text" value="" data-original-title="Resultado" disabled=""></td>';
            html += '<td align="center" class="form-pos-calibracao-parametros-validacao"><i class="" aria-hidden="true"></i></a></td>';
            html += '</tr>';
            $(".tb-parametros-pos-calibracao > tbody:last-child").append(html);
            APP.controller.InstrumentosController.setup();
            APP.controller.InstrumentosController.setCalibracaoAprovado();
            APP.controller.InstrumentosController.delParametroCalibracao();
            APP.controller.InstrumentosController.bind();
        });

    },

    setResponsavelInstrumento: function () {


        var idResponsavel = $("#IdResponsavelHidden").val();
        $('#dropdown-responsavel').val(idResponsavel);
    },

    setCalibracaoAprovado: function () {


        //var sizeCheck = $('.tb-parametros-pos-calibracao').find('.form-pos-calibracao-parametros-validacao i').size();
        //var qntCheck = $('.tb-parametros-pos-calibracao').find('.form-pos-calibracao-parametros-validacao .fa-check').size();
        //if (sizeCheck == qntCheck) {
        //    $('.form-pos-calibracao-aprovado-sim').prop("checked", true);
        //} else {
        //    $('.form-pos-calibracao-aprovado-sim').prop("checked", false);
        //}
    },

    delCalibracao: function () {


        tabelaCalibracao = $('#tb-calibracao').DataTable();

        this.buttonDelCalibracao.unbind("click");
        this.buttonDelCalibracao.bind('click', function (event) {
            event.preventDefault();

            var IdCalibracao = $(this).data('id');
            var $rowAtual = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (result) {
                if (result) {
                    $.ajax({
                        url: "/Calibracao/Excluir/",
                        dataType: 'JSON',
                        type: 'POST',
                        data: { id: IdCalibracao },
                        success: function (data) {
                            if (data.StatusCode == 200) {
                                tabelaCalibracao.row($rowAtual).remove().draw();
                                bootbox.alert({
                                    message: _options.CalibracaoExcluida,
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

    delParametroCalibracao: function () {


        this.buttonDelParametroCalibracao.unbind('click');

        this.buttonDelParametroCalibracao.bind('click', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
        });
    },

    editPosCalibracao: function () {


        this.buttoEditParametroCalibracao.unbind('click');
        this.buttoEditParametroCalibracao.bind('click', function (event) {

            //event.preventDefault();

            $("[name=EdicaoCalibracao]").val("1");

            var IdCalibracao = $(this).data("id");
            var IdFilaEnvio = $(this).data("idFilaEnvio");

            $("#IdCalibracao").val(IdCalibracao);
            $("#IdFilaEnvio").val(IdFilaEnvio);



            $.ajax({
                type: "get",
                dataType: 'json',
                url: "/Calibracao/Editar",
                data: { id: IdCalibracao },
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    if (result.StatusCode == 200) {
                        APP.controller.InstrumentosController.getEditPosCalibracao(result.Calibracao);
                        //transfere o botão correto
                        $("#uploadbotaodestino").html($("#uploadbotao" + result.Calibracao.IdCalibracao).html());

                    }
                },
                error: function (result) {
                    bootbox.alert(_options.MsgOcorreuErro);
                },
                complete: function (result) {
                    APP.component.Loading.hideLoading();
                },
            });
        });
    },

    calcCriterioAceitacao: function () {


        $('.form-pos-calibracao-parametros-erro, .form-pos-calibracao-parametros-incerteza, .form-parametro-instrumento-criterio-aceitacao').on('keyup change', function () {

            var erro, incerteza, criterioAceitacao;

            var $parametroErro = $(this).closest('tr').find('.form-pos-calibracao-parametros-erro');
            if (typeof ($parametroErro.val()) != "undefined")
                erro = Number($parametroErro.val().replace(",", "."));

            var $parametroIncerteza = $(this).closest('tr').find('.form-pos-calibracao-parametros-incerteza');
            if (typeof ($parametroIncerteza.val()) != "undefined")
                incerteza = Number($parametroIncerteza.val().replace(",", "."));

            var $parametroCriterioAceitacao = $('.form-parametro-instrumento-criterio-aceitacao');
            if (typeof ($parametroCriterioAceitacao.val()) != "undefined")
                criterioAceitacao = Number($parametroCriterioAceitacao.val().replace(",", "."));

            var valor = Number(erro + incerteza);
            if (Number.isNaN(valor)) { valor = 0; }
            if (Number.isNaN(criterioAceitacao)) { criterioAceitacao = 0; }



            if (criterioAceitacao != 0 && criterioAceitacao != 0 && valor != 0) {
                $(this).closest('.campos-tabela').find('.form-pos-calibracao-parametros-resultado').val(valor);
                if (valor <= criterioAceitacao) {
                    classIcone = 'fa fa-check ativo-color';
                    $(this).closest('.campos-tabela').find('.form-pos-calibracao-parametros-validacao i').removeClass().addClass(classIcone);
                } else {
                    classIcone = 'fa fa-close trash-color';
                    $(this).closest('.campos-tabela').find('.form-pos-calibracao-parametros-validacao i').removeClass().addClass(classIcone);
                }
                APP.controller.InstrumentosController.setCalibracaoAprovado();
            }
            else {
                $(this).closest('.campos-tabela').find('.form-pos-calibracao-parametros-resultado').val(valor);
                classIcone = 'fa fa-check ativo-color';
                $(this).closest('.campos-tabela').find('.form-pos-calibracao-parametros-validacao i').removeClass().addClass(classIcone);
                APP.controller.InstrumentosController.setCalibracaoAprovado();
            }

            var aprovado = true;
            $(".form-pos-calibracao-parametros-validacao").each(function () {
                var classCss = $(this).find("i").attr("class");
                if (classCss == "fa fa-close trash-color") {
                    aprovado = false;
                }
            });

            if (aprovado == false) {
                $(".form-pos-calibracao-aprovado-nao").prop("checked", true);;
                $(".form-pos-calibracao-aprovado-sim").removeAttr("checked");
            }
            else {
                $(".form-pos-calibracao-aprovado-sim").prop("checked", true);;
                $(".form-pos-calibracao-aprovado-nao").removeAttr("checked");
            }

        });
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
                    APP.component.SelectListCompare.init(result.Lista, $('[name=IdSigla] option'), '#form-cadastro-sigla', 'IdControladorCategorias', 'Descricao');
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
                APP.controller.InstrumentosController.setComboSigla,
                ".add-sigla");

        });

    },

    sendFormParametroInstrumento: function () {

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

                        APP.controller.InstrumentosController.setCalibracaoVisivel();

                        var isHidden = $('#form-pos-calibracao').is(":hidden");
                        if (isHidden == false) {
                            APP.controller.InstrumentosController.sendFormPoscalibracao(idInstrumento, result.Success);
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

    sendFormPoscalibracao: function (idInstrumento, msgRetorno) {


        var objPosCalibracao = APP.controller.InstrumentosController.getObjPosCalibracao();

        var url = '/Calibracao/Criar';

        var EedicaoCalibracao = $("[name=EdicaoCalibracao]").val() == "1" ? true : false;

        if (EedicaoCalibracao) {
            url = '/Calibracao/Editar';
        }

        var retorno = false;

        $.ajax({
            url: url,
            dataType: 'JSON',
            type: 'POST',
            data: objPosCalibracao,
            success: function (data) {

                if (data.StatusCode == 200) {
                    APP.controller.InstrumentosController.setFormCalibracaoVazio();

                    if (!EedicaoCalibracao) {

                        objPosCalibracao.IdCalibracao = data.calibracao.IdCalibracao;
                        objPosCalibracao.ArquivoCertificado = data.calibracao.ArquivoCertificado;
                        objPosCalibracao.DataCalibracao = data.calibracao.DataCalibracao;
                        objPosCalibracao.Aprovador = data.calibracao.Aprovador;
                        objPosCalibracao.Aprovado = data.calibracao.Aprovado;

                        APP.controller.InstrumentosController.addCalibracaoTabela(objPosCalibracao);
                        APP.controller.InstrumentosController.setup();
                        APP.controller.InstrumentosController.editPosCalibracao();
                        APP.controller.InstrumentosController.delCalibracao();

                    }

                    if (idInstrumento != "0") {
                        bootbox.alert({
                            message: msgRetorno,
                            callback: function () {
                                window.location.href = "/Instrumento/Index";
                            }
                        });
                    }

                }
                else if (data.StatusCode == 505) {
                    erro = APP.component.ResultErros.init(data.Erro);
                    bootbox.alert(erro);
                    retorno = false;

                } else if (data.StatusCode == 500) {
                    erro = APP.component.ResultErros.init(data.Erro);
                    bootbox.alert(erro);
                    retorno = false;
                }
            },
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();

            },
        });
        return retorno;
    },

    saveForms: function () {


        this.buttonSaveForm.click(function () {

            event.preventDefault();

            $("#form-pos-calibracao").validate();

            var formCalibracaoValido = $("#form-pos-calibracao").valid();

            var objPosCalibracao = APP.controller.InstrumentosController.getObjPosCalibracao();
            var dataNotificacaoSplit = objPosCalibracao.DataNotificacao.split("/");
            var DataProximaCalibracaoSplit = objPosCalibracao.DataProximaCalibracao.split("/");;
            var DataNotificacao = new Date(dataNotificacaoSplit[2], dataNotificacaoSplit[1], dataNotificacaoSplit[0]);
            var DataProximaCalibracao = new Date(DataProximaCalibracaoSplit[2], DataProximaCalibracaoSplit[1], DataProximaCalibracaoSplit[0]);
            var DataRegistroSplit = objPosCalibracao.DataRegistro.split("/");
            var Aprovado = objPosCalibracao.Aprovado;
            var OrgaoCalibrador = objPosCalibracao.OrgaoCalibrador;
            var Certificado = objPosCalibracao.Certificado;
            var DataRegistro = new Date(DataRegistroSplit[2], DataRegistroSplit[1], DataRegistroSplit[0]);


            var valido = true;
            if (DataRegistro > DataNotificacao) {
                valido = false;
                APP.component.Loading.hideLoading();
                bootbox.alert(_options.labelValidaDataRegistro);


            }


            if (DataNotificacao > DataProximaCalibracao) {
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


                if (formCalibracaoValido) {


                    APP.controller.InstrumentosController.sendFormParametroInstrumento();

                    $('#form-pos-calibracao').slideUp(500);
                    $('#form-pos-calibracao').removeClass('show').addClass('hide');
                    $('.tb-calibracao').slideDown(500);
                    $('.pills-parametros-pos-calibracao').removeClass('show').addClass('hide');
                    $('.pills-tabela-calibracao').removeClass('hide').addClass('show');
                }
            }
        });
    },

    addCalibracaoTabela: function (calibracao) {


        var tabelaCalibracao = $('#tb-calibracao').DataTable();

        var newRow =
            [
                calibracao.IdCalibracao,
                calibracao.IdFilaEnvio,
                calibracao.Certificado,
                calibracao.DataCalibracao,
                calibracao.DataProximaCalibracao,
                calibracao.OrgaoCalibrador,
                calibracao.Aprovador,
                calibracao.Aprovado,
                calibracao.Observacoes,
                '<a href="#" class="edit icon-cliente editar-color tb-calibracao-editar" data-id="' + calibracao.IdCalibracao + '">' +

                '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>' +
                '</a>' +
                '<a href="' + calibracao.ArquivoCertificado + '" class="view-color icon-cliente" target="_blank">' +
                '<i class="fa fa-search" aria-hidden="true" data-toggle="tooltip" title="" data-original-title="Visualizar"></i>' +
                '</a>' +
                '<a href="#" class="tb-calibracao-excluir trash-color icon-cliente" data-id="' + calibracao.IdCalibracao + '">' +
                '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>' +
                '</a>'
            ];

        var newrow = tabelaCalibracao.row.add(newRow).draw().node();

        $(newrow).find('td').eq(0).addClass('hidden');

        $(newrow).find('td').eq(8).addClass('text-nowrap');
    },

    //fazer
    viewArquivoCalibracao: function () {


    },

    setupUploadArquivoCalibracao: function () {


        var formJson = { "id": 0, "idSite": $("#IdSite").val() };
        UploadMultiplosArquivos(
            "/Calibracao/UploadArquivo",
            "dropPE",
            "uplPE",
            "ArquivoCertificado",
            30000000000,
            formJson,
            "form-pos-calibracao",
            $("#IdSite").val(),
            "/Calibracao/RemoverArquivo");
    },

    bind: function () {


        APP.controller.InstrumentosController.calcCriterioAceitacao();

    },

    setCalibracaoVisivel() {


        var idInstrumento = $("#IdInstrumento").val();

        if (idInstrumento != "0") {
            $(".tabs").removeClass("hide").fadeIn();
            $(".tb-calibracao").removeClass("hide").fadeIn();
        }
    },

    setFormCalibracaoVazio() {


        $('#form-pos-calibracao .arquivo').html("").addClass("hide");
        $('#form-pos-calibracao .deletefile').addClass("hide").attr('disabled', 'disabled');
        $("[name='CriteriosAceitacao'] tbody").html("");
        $('#form-pos-calibracao')[0].reset();
    },

    addCriterioAceitacao(criterioAceitacao) {


        var html = '';
        html += '<tr class="campos-tabela">';
        html += '<input type="hidden" name="IdCriterioAceitacao" value="' + criterioAceitacao.IdCriterioAceitacao + '">';
        html += '<td align="center"></td>';
        html += '<td><input style="/* width: 76px; */" class="form-control form-pos-calibracao-parametros-periodicidade" name="Periodicidade" data-val="true" title="" type="text" value="' + criterioAceitacao.Periodicidade + ' " data-original-title="Range"></td>';
        html += '<td><input style="/* width: 76px; */" class="form-control form-pos-calibracao-parametros-erro" name="Erro" data-val="true" title="" type="text" value="' + criterioAceitacao.Erro + '" data-original-title="Erro"></td>';
        html += '<td align="center"><i class="fa fa-plus blue-color" aria-hidden="true"></i></td>';
        html += '<td><input style="/* width: 76px; */" class="form-control form-pos-calibracao-parametros-incerteza" name="Incerteza" data-val="true" title="" type="text" value="' + criterioAceitacao.Incerteza + '" data-original-title="Incerteza"></td>';
        html += '<td align="center"><i class="fa fa-equals blue-color" aria-hidden="true"></i></td>';
        html += '<td><input style="/* width: 76px; */" class="form-control form-pos-calibracao-parametros-resultado" name="Resultado" data-val="true" title="" type="text" value="' + criterioAceitacao.Resultado + '" data-original-title="Resultado" disabled=""></td>';
        html += '<td align="center" class="form-pos-calibracao-parametros-validacao"><i class="' + (criterioAceitacao.Aceito == true ? "fa fa-check ativo-color" : "fa fa-close trash-color") + '" aria-hidden="true"></i></td>';
        html += '</tr>';
        $(".tb-parametros-pos-calibracao > tbody:last-child").append(html);
        APP.controller.InstrumentosController.setup();
        APP.controller.InstrumentosController.setCalibracaoAprovado();
        APP.controller.InstrumentosController.delParametroCalibracao();
        APP.controller.InstrumentosController.bind();
    },
};
