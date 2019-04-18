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

        //GETs
        this.getResponsavel();
        this.getResponsavelAprovadorCalibracao();
        //SETs
        this.setCalibracao();
        this.setCalculaDoSistema();
        this.setParametrosCalibracao();
        this.setResponsavelInstrumento();
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
        //INCLUDEs
        APP.component.AtivaLobiPanel.init();
        APP.component.FileUpload.init();
        APP.component.Datapicker.init();
        APP.component.DataTable.init('#tb-calibracao');
        // APP.controller.InstrumentosController.setupUploadArquivoCalibracao();
        //OUTRAS
        this.setCalibracaoVisivel();
        this.setValidateForms();
        
    },

    setValidateForms : function () {
        
        var ObjFormInstrumentoValidate = APP.controller.InstrumentosController.getObjObjFormInstrumentoValidate();
        APP.component.ValidateForm.init(ObjFormInstrumentoValidate, '#form-parametro-instrumento');

    },

    getObjObjFormInstrumentoValidate : function () {
        
        var acoesPadraoFormCriarPadraoObj = {
            //formCriaClienteLogo: {required: true, minFiles: 1},
            Equipamento: {required: true},
            Numero: {required: true},
            Marca: {required: true},
            Modelo: {required: true},
            IdResponsavel: {required: true},
            LocalDeUso: {required: true},
            Escala: {required: true},
            MenorDivisao: {required: true},
            valorAceitacao: {required: true, number: true},
            
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

            bootbox.confirm("Deseja excluir o registro ? ", function (result) {
                if (result) {
                    $.post('/Instrumento/Excluir/' + IdInstrumento, function (data, status) {
                    }).done(function (data) {
                        if (data.StatusCode == "200") {
                            tabelaInstrumento.row($rowAtual).remove().draw();
                            bootbox.alert("Registro excluido com sucesso");
                        }
                    });
                }
            });
        });

    },

    editIntrumento: function () {
        
        var IdInstrumento = $('#IdInstrumento');
        var instrumento = $('#form-parametro-instrumento').serialize();

        $.post('/Instrumento/Editar/', instrumento, function (data, status) {
        })
            .success(function (data, status) {

            })
            .fail(function (data, status) {

            });
    },

    getResponsavel: function () {
        var idSite = $('#IdSite').val();
        var idFuncao = $('#idFuncao').val();

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
            IdInstrumento: $('#IdInstrumento').val(),
            DataRegistro: $('#form-pos-calibracao').find('[name=DtRegistro]').val(),
            DataNotificacao: $('#form-pos-calibracao').find('[name=DtNotificacao]').val(),
            DataProximaCalibracao: $('#form-pos-calibracao').find('[name=DataProximaCalibracao]').val(),
            Certificado: $('#form-pos-calibracao').find('[name=Certificado]').val(),
            CriterioAceitacao: [],
            OrgaoCalibrador: $('#form-pos-calibracao').find('[name=OrgaoCalibrador]').val(),
            Aprovador: $('#form-pos-calibracao').find('[name=Aprovador]').val(),
            Aprovado: $('input[name="Aprovado"]:checked').val(),
            ArquivoCertificadoAux:  APP.controller.InstrumentosController.getArquivoCertificadoAnexos(),
            Observacoes: $('#form-pos-calibracao').find('[name=Observacoes]').val(),
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
                    IdCriterioAceitacao: 0 ,// $(this).find('[name=IdCriterioAceitacao]').val(),
                    Periodicidade: $(this).find('[name=Periodicidade]').val(),
                    IdCalibracao: $("#IdCalibracao").val(),
                    Aceito: $(this).find('.ativo-color').length == 1 ? true : false,
                };
              
                PosCalibracao.CriterioAceitacao.push(CriteriosAceitacao);
            });
        }

        return PosCalibracao;
    },

    getArquivoCertificadoAnexos: function(){
        var anexoContratoModel = APP.controller.ClienteController.models.AnexoModel;
        var arrayAnexoArquivoCertificado = [];
        
        $('.dashed li a:first-child').each(function () {
            
            var nameImg = $(this).text();     
            var anexoContratoCliente = anexoContratoModel.constructor (
                $(this).closest('li').find('[name=formCriaClienteContratoIdAnexo]').val(),//IdAnexo
                nameImg,//Nome
                $(this).data('b64')//B64
            );

            arrayAnexoArquivoCertificado.push(anexoContratoCliente);
        });
        
        return arrayAnexoArquivoCertificado;
    },

    getEditPosCalibracao: function (result) {

        APP.controller.InstrumentosController.showPosCalibracao();

        $('#form-pos-calibracao').find("[name=EdicaoCalibracao]").val("1");
        $('#IdInstrumento').val(result.IdInstrumento);
        $('#form-pos-calibracao').find('[name=DtRegistro]').val(result.DataRegistro);
        $('#form-pos-calibracao').find('[name=DtNotificacao]').val(result.DataNotificacao);
        $('#form-pos-calibracao').find('[name=DataProximaCalibracao]').val(result.DataProximaCalibracao);
        $('#form-pos-calibracao').find('[name=Certificado]').val(result.Certificado);

        $('#form-pos-calibracao').find('[name=OrgaoCalibrador]').val(result.OrgaoCalibrador);
        $('#form-pos-calibracao').find('[name=Aprovado][value=' + result.Aprovado +']').prop('checked', true);

        
        $('#form-pos-calibracao').find('[name=Aprovador] option').val(result.Aprovador.IdUsuario).text(result.Aprovador.Nome).prop('selected', true);
        $('#form-pos-calibracao').find('.arquivo').val(result.ArquivoCertificado);

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
            html += '<td><input  onkeyup="SubstituiVirgulaPorPonto(this)"  class="form-control form-pos-calibracao-parametros-periodicidade" name="Periodicidade" data-val="true" title="" type="text" value="" data-original-title="Range"></td>';
            html += '<td><input onkeyup="SubstituiVirgulaPorPonto(this)"  class="form-control form-pos-calibracao-parametros-erro" name="Erro" data-val="true" title="" type="text" value="" data-original-title="Erro"></td>';
            html += '<td align="center"><i class="fa fa-plus blue-color" aria-hidden="true"></i></td>';
            html += '<td><input onkeyup="SubstituiVirgulaPorPonto(this)"  class="form-control form-pos-calibracao-parametros-incerteza" name="Incerteza" data-val="true" title="" type="text" value="" data-original-title="Incerteza"></td>';
            html += '<td align="center"><i class="fa fa-equals blue-color" aria-hidden="true"></i></td>';
            html += '<td><input class="form-control form-pos-calibracao-parametros-resultado" name="Resultado" data-val="true" title="" type="text" value="" data-original-title="Resultado" disabled=""></td>';
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
        var sizeCheck = $('.tb-parametros-pos-calibracao').find('.form-pos-calibracao-parametros-validacao i').size();
        var qntCheck = $('.tb-parametros-pos-calibracao').find('.form-pos-calibracao-parametros-validacao .fa-check').size();
        if (sizeCheck == qntCheck) {
            $('.form-pos-calibracao-aprovado-sim').prop("checked", true);
        } else {
            $('.form-pos-calibracao-aprovado-sim').prop("checked", false);
        }
    },

    delCalibracao: function () {

        tabelaCalibracao = $('#tb-calibracao').DataTable();

        this.buttonDelCalibracao.unbind("click");
        this.buttonDelCalibracao.bind('click', function (event) {
            event.preventDefault();

            var IdCalibracao = $(this).data('id');
            var $rowAtual = $(this).closest('tr');

            bootbox.confirm("Deseja excluir o registro ? ", function (result) {
                if (result) {
                    $.ajax({
                        url: "/Calibracao/Excluir/",
                        dataType: 'JSON',
                        type: 'POST',
                        data: { id: IdCalibracao },
                        success: function (data) {
                            if (data.StatusCode == 200) {
                                tabelaCalibracao.row($rowAtual).remove().draw();
                                bootbox.alert("Calibracao Excluida!");
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

            event.preventDefault();

            $("[name=EdicaoCalibracao]").val("1");

            var IdCalibracao = $(this).data("id");

            $("#IdCalibracao").val(IdCalibracao);


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
                    }
                },
                error: function (result) {
                    bootbox.alert("Ocorreu um erro!");
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
        });
    },

    sendFormParametroInstrumento: function () {
        
        var idInstrumento = $("#IdInstrumento").val();
        var url = '/Instrumento/Criar';
        var validacao = true;
        
        if (idInstrumento != "0") {
            url = '/Instrumento/Editar';
        } else {
            validacao = $('#form-parametro-instrumento').valid();
        }
        
        if (validacao) {
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: url,
                data: $('#form-parametro-instrumento').serialize(),
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    if (result.StatusCode == 200) {

                        bootbox.alert(result.Success, function (result) { });
                        if (idInstrumento == "0") { $("#IdInstrumento").val(result.IdInstrumento); }

                        APP.controller.InstrumentosController.setCalibracaoVisivel();
                    } else if (result.StatusCode == 505) {
                        erro = APP.component.ResultErros.init(result.Erro);
                        bootbox.alert(erro);

                    } else if (result.StatusCode == 500) {
                        erro = APP.component.ResultErros.init(result.Erro);
                        bootbox.alert(erro);
                    }

                },
                error: function (result) {
                    bootbox.alert("Ocorreu um erro!");
                },
                complete: function (result) {
                    APP.component.Loading.hideLoading();
                },
            });
        }
    },

    sendFormPoscalibracao: function () {

        var objPosCalibracao = APP.controller.InstrumentosController.getObjPosCalibracao();

        var url = '/Calibracao/Criar';

        var EedicaoCalibracao = $("[name=EdicaoCalibracao]").val() == "1" ? true : false;

        if (EedicaoCalibracao) {
            url = '/Calibracao/Editar';
        }
        
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
                }
            },
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
                window.location.reload([true]);

            },
        });
    },

    saveForms: function () {

        this.buttonSaveForm.click(function () {
            
            event.preventDefault();

            $("#form-pos-calibracao").validate();

            var formCalibracaoValido = $("#form-pos-calibracao").valid();

            if (formCalibracaoValido) {

                var isHidden = $('#form-pos-calibracao').is(":hidden");
                if (isHidden == true) {
                    APP.controller.InstrumentosController.sendFormParametroInstrumento();

                } else {
                    APP.controller.InstrumentosController.sendFormParametroInstrumento();
                    APP.controller.InstrumentosController.sendFormPoscalibracao();
                }

                $('#form-pos-calibracao').slideUp(500);
                $('#form-pos-calibracao').removeClass('show').addClass('hide');
                $('.tb-calibracao').slideDown(500);
                $('.pills-parametros-pos-calibracao').removeClass('show').addClass('hide');
                $('.pills-tabela-calibracao').removeClass('hide').addClass('show');
            }
        });
    },

    addCalibracaoTabela: function (calibracao) {

        var tabelaCalibracao = $('#tb-calibracao').DataTable();

        var newRow =
            [
                calibracao.IdCalibracao,
                calibracao.Certificado,
                calibracao.DataCalibracao,
                calibracao.DataProximaCalibracao,
                calibracao.OrgaoCalibrador,
                calibracao.Aprovador,
                calibracao.Aprovado,
                calibracao.Observacoes,
                '<a href="#" class="edit icon-cliente editar-color tb-calibracao-editar" data-id="' + calibracao.IdCalibracao + '">' +

                '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="" data-original-title="Editar"></i>' +
                '</a>' +
                '<a href="' + calibracao.ArquivoCertificado + '" class="view-color icon-cliente" target="_blank">' +
                '<i class="fa fa-search" aria-hidden="true" data-toggle="tooltip" title="" data-original-title="Visualizar"></i>' +
                '</a>' +
                '<a href="#" class="tb-calibracao-excluir trash-color icon-cliente" data-id="' + calibracao.IdCalibracao + '">' +
                '<i class="fa fa-trash" aria-hidden="true" data-toggle="tooltip" title="" data-original-title="Excluir"></i>' +
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
        html += '<td align="center"><i class="fa fa-minus-circle blue-color form-pos-calibracao-parametros-icone-del" aria-hidden="true"></i></td>';
        html += '<td><input class="form-control form-pos-calibracao-parametros-periodicidade" name="Periodicidade" data-val="true" title="" type="text" value="' + criterioAceitacao.Periodicidade + ' " data-original-title="Range"></td>';
        html += '<td><input class="form-control form-pos-calibracao-parametros-erro" name="Erro" data-val="true" title="" type="text" value="' + criterioAceitacao.Erro + '" data-original-title="Erro"></td>';
        html += '<td align="center"><i class="fa fa-plus blue-color" aria-hidden="true"></i></td>';
        html += '<td><input class="form-control form-pos-calibracao-parametros-incerteza" name="Incerteza" data-val="true" title="" type="text" value="' + criterioAceitacao.Incerteza + '" data-original-title="Incerteza"></td>';
        html += '<td align="center"><i class="fa fa-equals blue-color" aria-hidden="true"></i></td>';
        html += '<td><input class="form-control form-pos-calibracao-parametros-resultado" name="Resultado" data-val="true" title="" type="text" value="' + criterioAceitacao.Resultado + '" data-original-title="Resultado" disabled=""></td>';
        html += '<td align="center" class="form-pos-calibracao-parametros-validacao"><i class="' + (criterioAceitacao.Aceito == true ? "fa fa-check ativo-color" : "fa fa-close trash-color") + '" aria-hidden="true"></i></td>';
        html += '</tr>';
        $(".tb-parametros-pos-calibracao > tbody:last-child").append(html);
        APP.controller.InstrumentosController.setup();
        APP.controller.InstrumentosController.setCalibracaoAprovado();
        APP.controller.InstrumentosController.delParametroCalibracao();
        APP.controller.InstrumentosController.bind();
    },
};