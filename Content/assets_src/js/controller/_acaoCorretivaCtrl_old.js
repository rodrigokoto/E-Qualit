/*
|--------------------------------------------------------------------------
| Acao Corretiva Controller
|--------------------------------------------------------------------------
*/

APP.controller.AcaoCorretivaController_old = {

    init: function () {

        //var page = APP.component.Util.getPage();

        //this.setup();

        //if (page == "ListaAcaoCorretiva") {
        //    this.IndexAcaoCorretiva();
        //}
        //else if (page == "EditarAcaoCorretiva") {
        //    this.EditarAcaoCorretiva();
        //}

    },

    setup: function () {

        //Acao Corretiva Index
        this.buttonDelAcaoCorreita = $(".del-acao-corretiva");

        //Acao Corretiva Criar

        //Acao Corretiva Editar
        this.buttonRadioEProcedente = $("input:radio[name='EProcedente']");
        this.buttonAddAcaoImediata = $('.add-acao-imediata');
        this.selectButtonResponsavelImplementar = $('[name=IdResponsavelImplementar]');
        this.buttonDtEfetivaImplementacao = $("input[name='DtEfetivaImplementacao']");

    },

    //Chamadas
    IndexAcaoCorretiva: function () {

        this.delAcaoCorretiva();
        APP.component.DataTable.setDataTableParam('#tb-lista-acao-corretiva', 0, "desc", "Tags");

    },

    EditarAcaoCorretiva: function () {

        this.setAcaoImediata();
        this.showFileUploadAcaoImediata();
        this.setNewResponsavelImplementar();
        this.setupUploadArquivoAcaoImediata();
        this.getSelectResponsavelReverificar();
        APP.component.AtivaLobiPanel.init();
        APP.component.FileUpload.init();
        APP.component.Datapicker.init();
        APP.component.UploadFiles.init();
        $('[name=DtDescricaoAcao]').val(APP.component.Datatoday.init());
        this.sendFormSegundaEtapa();

        this.setHideAndShow();
        this.setChecksOnPage();
        this.setCheckAcaoImediataOk();
        this.setCheckAcaoImediataNotOk();
        this.setBackChecksOnPage();

    },

    //Funções Index Não Conformidade
    delAcaoCorretiva: function () {
        this.buttonDelAcaoCorreita.on('click', function (event) {

            var $idAcaoCorretiva = $($(event)[0].target).parent().data("id-nao-conformidade");
            var data = {
                "idAcaoCorretiva": $idAcaoCorretiva,
                "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()
            };

            var perguntaSeDesejaExcluir = $('[name=labelDesejaExcluirNC]').val();
            bootbox.confirm(perguntaSeDesejaExcluir, function (result) {
                if (result) {
                    $.post('/AcaoCorretiva/RemoverComAcaoConformidade', data, function (data) {
                    }).done(function (result) {
                        if (result.StatusCode == 200) {
                            window.location.reload([true]);
                        }
                    })
                    .fail(function (result) {
                    });
                }
            });
        });

    },

    setAcaoImediata: function () {
        this.buttonAddAcaoImediata.unbind('click');
        this.buttonAddAcaoImediata.bind('click', function () {

            var TraducaoDropNameSelect = $('[name="TraducaoDropNameSelect"]').val();
            var html = '';
            html += '<tr>';
            html += '<td class="descricao-acao-imediata"><textarea type="text" class="form-control input-data" name="Descricao" rows="1" style="padding:2px 3px !important;" /></textarea>';
            html += '<input type="hidden" name="IdAcaoImediata" class="form-control input-data" value="0" style="display:none;"/>';
            html += '<input type="hidden" name="Estado" value="4" style="display:none;"/>';
            html += '</td>';
            html += '<td class="prazo-para-implementar">';
            html += '<div class="input-group date" id="datetimepicker2">';
            html += '<input type="text" name="DtPrazoImplementacao" class="form-control data" />';
            html += '<span class="input-group-addon">';
            html += '<i class="fa fa-calendar" aria-hidden="true"></i>';
            html += '</span>';
            html += '</div>';
            html += '</td>';
            html += '<td class="responsavel-para-implementar">';
            html += '<select class="form-control" name="IdResponsavelImplementar" required>';
            html += '<option value="0">Selecione</option>';
            html += '</select>';
            html += '<td class="data-para-efetiva-implementacao">';
            html += '<div class="input-group dp-dt-efetiva-implementacao" id="datetimepicker2" style="display: none;">';
            html += '<input type="text" name="DtEfetivaImplementacao" class="form-control data" />';
            html += '<span class="input-group-addon">';
            html += '<i class="fa fa-calendar" aria-hidden="true"></i>';
            html += '</span>';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            html += '<div id="uplPE" class="upload upload-file hide">';
            html += '<div id="dropPE" class="drop" style="padding:2px 3px; background-color:#fff;">';
            html += '<a href="javascript:;" class="text-center" style="color: #698e9f;">';
            html += '<i class="fa fa-paperclip fa-1x" aria-hidden="true"></i>&nbsp;Anexar</a>';
            html += '<input type="file" name="upl" multiple />';
            html += '<ul><!-- The file uploads will be shown here --></ul>';
            html += '<input type="hidden" id="EvidenciaImg" name="arquivoEvidencia" value="@ViewBag.DsArquivos" /></div></div>';
            html += '</td>';
            html += '<td>';
            html += '<button type="button" class="btn btn-delete-acao-imediata trash-color">';
            html += '<i class="fa fa-trash " aria-hidden="true" ></i>';
            html += '</button>';
            html += '</td>';
            html += '</tr>';

            $('.tb-acao-imediata tbody').append(html);
            $('.add-acao-imediata').removeClass('show').addClass('hide');
            APP.controller.AcaoCorretivaController.bind();

        });

    },

    getPrimeiraEtapaAcaoCorretiva: function () {

        $.get('/AcaoCorretiva/PrimeiraEtapa', function (_partial) {
            $('#primeira-etapa').html(_partial);
            APP.controller.AcaoCorretivaController.formularioPrimeiraEtapa();
        });

    },

    //Funções Editar Não Conformidade
    getRadioAcaoCorretivaEtapa1: function () {
        var radioAcaoCorretiva = APP.component.Radio.init('EAcaoCorretivaAuditoria');
        if (radioAcaoCorretiva == "false") {
            $('.radio-eprocedente-nao').prop("checked", true);
            $('.painel-acao-corretiva-sim').removeClass('show').addClass('hide');
            $('.painel-acao-corretiva-nao').removeClass('hide').addClass('show');
        } else {
            $(".radio-eprocedente-sim").prop("checked", true);
            $(".radio-eprocedente-sim").prop("disabled", true);
            $('.painel-acao-corretiva-nao').removeClass('show').addClass('hide');
            $('.painel-acao-corretiva-sim').removeClass('hide').addClass('show');
        }
    },

    showAcaoCorretivaProcedente: function () {
        this.buttonRadioEProcedente.on('click', function () {
            var radioEProcedente = APP.component.Radio.init('EProcedente');
            if (radioEProcedente == "false") {
                $('.painel-acao-corretiva-sim').removeClass('show').addClass('hide');
                $('.painel-acao-corretiva-nao').removeClass('hide').addClass('show');
            } else {
                $('.painel-acao-corretiva-nao').removeClass('show').addClass('hide');
                $('.painel-acao-corretiva-sim').removeClass('hide').addClass('show');
            }
        });
    },

    getOptionResponsavelImplementar: function () {
        var idSite = $('#acao-corretiva-site').val();
        var idProcesso = $('[name=IdProcesso]').val();
        var idFuncao = 23; // Funcionalidade(Implementar ação imediata) que permite usuario criar GR
    
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: `/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=${idProcesso}&idSite=${idSite}&idFuncao=${idFuncao}`,
            beforeSend: function () {
                $('.add-acao-imediata').removeClass('show').addClass('hide');
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    $(result.Lista).each(function (key, value) {
                        var $option = $('<option>');
                        $('.tb-acao-imediata tbody tr:last-child [name="IdResponsavelImplementar"]').append($option.val(value.IdUsuario).text(value.NmCompleto));
                    });
                }
            },
            error: function (result) {
                bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {
                $('.add-acao-imediata').removeClass('hide').addClass('show');
            }
        });

    },

    showDataEfetivaImplementacao: function (_select) {

        var idResponsavelImplementar = _select;
        var idUsuarioLogado = $('[name=IdUsuarioLogado]').val();
        if (idResponsavelImplementar == idUsuarioLogado) {
            $('.dp-dt-efetiva-implementacao').show();
        } else {
            $('.dp-dt-efetiva-implementacao').hide();
        }

    },

    setNewResponsavelImplementar: function () {

        $('[name=IdResponsavelImplementar]').change(function () {
            var select = $(this).find(':selected').val();
            APP.controller.GestaoDeRiscoController.showDataEfetivaImplementacao(select, this);
        });

    },

    showFileUploadAcaoImediata: function () {

        $('[name="DtEfetivaImplementacao"]').on('change', function () {
            var buttonDtEfetivaImplementacao = $(this).val();
            if (buttonDtEfetivaImplementacao != null && buttonDtEfetivaImplementacao != "") {
                $(this).closest('tr').find('.acao-imediata-upload').show();
            } else {
                $(this).closest('tr').find('.acao-imediata-upload').hide();
            }
        });

    },

    setupUploadArquivoAcaoImediata: function () {

        $("div[id^=uplPE] a").unbind('click');
        $('div[id^=uplPE] a').click(function () {
            var dropPE = $(this).closest('td').find('div[id^=uplPE]').attr('id');
            var uplPE = $(this).closest('td').find('div[id^=dropPE]').attr('id');
            var arquivoEvidencia = $(this).closest('td').find('input[id^=arquivoEvidencia]').attr('id');

            var formJson = { "id": 0, "tipo": "ac", "acaoImediata": "acaoImediata" };
            APP.component.UploadFiles.setUploadFiles("/AcaoCorretiva/UploadArquivo",
                dropPE,
                uplPE,
                arquivoEvidencia,
                30000000000,
                formJson,
                "form-acao-corretiva-etapa-2",
                0,
                "/AcaoCorretiva/RemoverArquivo");

            UploadDeleteFiles("/AcaoCorretiva/RemoverArquivo");
        });
    },

    editAcaoImediata: function () {

        $('.btn-edit-acao-imediata').on('click', function () {
            $(this).closest('tr').find('.descricao-acao-imediata textarea').attr("disabled", false);
            $(this).closest('tr').find('.prazo-para-implementar input').attr("disabled", false);
            $(this).closest('tr').find('.responsavel-para-implementar select').attr("disabled", false);
            $(this).closest('tr').find('.data-para-efetiva-implementacao input').attr("disabled", false);
            $(this).closest('tr').find('.upload-file input').attr("disabled", false);
            $('.add-acao-imediata').hide();
            $('.e-correcao-acao-imediata').removeClass('hide').addClass('show');
        });

    },

    setConfirmAcaoImediata: function () {

        $('.btn-confirm-acao-imediata').on('click', function () {
            $(this).closest('tr').find('.descricao-acao-imediata textarea').attr("disabled", true);
            $(this).closest('tr').find('.prazo-para-implementar input').attr("disabled", true);
            $(this).closest('tr').find('.responsavel-para-implementar select').attr("disabled", true);
            $(this).closest('tr').find('.data-para-efetiva-implementacao input').attr("disabled", true);
            $(this).closest('tr').find('.upload-file input').attr("disabled", true);
            $('.add-acao-imediata').show();
            $('.e-correcao-acao-imediata').removeClass('hide').addClass('show');
        });

    },

    delAcaoImediata: function () {

        $('.btn-delete-acao-imediata').on('click', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
            $('.add-acao-imediata').show();
            $('.e-correcao-acao-imediata').removeClass('show').addClass('hide');
        });

    },

    getRadioECorrecao: function () {
        $("input[name='ECorrecao']").unbind('click');
        $("input[name='ECorrecao']").on('click', function (event) {
            var radioECorrecao = APP.component.Radio.init('ECorrecao');
            if (radioECorrecao == "false") {
                $('.responsavel-reverificacao').removeClass('hide').addClass('show');
                APP.controller.AcaoCorretivaController.getSelectResponsavelReverificar();
            } else {
                $('.responsavel-reverificacao').removeClass('show').addClass('hide');
            }
        });
    },

    getSelectResponsavelReverificar: function () {
        var idSite = $('#acao-corretiva-site').val();
        var idProcesso = $('[name=IdProcesso]').val();
        var idFuncao = 24; // Funcionalidade(Reverificação) que permite usuario Reverifique AC
        $.get(`/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=${idProcesso}&idSite=${idSite}&idFuncao=${idFuncao}`, function (result) {
            if (result.StatusCode == 200) {
                $(result.Lista).each((key, value) => {
                    var $option = $('<option>');
                    $('[name=IdResponsavelReverificador]').append(
                        $option.val(value.IdUsuario).text(value.NmCompleto)
                        );
                });
            }
        });

    },

    showDataEfetivaImplementacaoStatus: function () {
        var usuariosSelect = $('#AcoesImediatas tbody tr').find('[name=IdResponsavelImplementar]');
        var idUsuarioLogado = $('[name=IdUsuarioLogado]').val();

        for (i = 0; i < usuariosSelect.size() ; i++) {
            var valueSelect = $(usuariosSelect[i]).val();
            if (valueSelect != idUsuarioLogado) {
                $(usuariosSelect[i]).closest('tr').find('[name=DtEfetivaImplementacao]').prop("disabled", true);
            }
        }

    },

    setHideAndShow: function () {

        var statusEtapa = parseInt($('[name=StatusEtapa]').val());

        if (statusEtapa == 2) {
            APP.controller.AcaoCorretivaController.showDataEfetivaImplementacaoStatus();
        } else if (statusEtapa == 3) {
            $('div[id^=dropPE]').addClass('input-disable');
            $('div[id^=dropPE] a').addClass('input-disable');
            $('[name=upl]').prop("disabled", true);
        } else if (statusEtapa == 4) {
            $('.dp-dt-efetiva-implementacao').show();
            $('.acao-imediata-upload').show();
            $('[name=DtEfetivaImplementacao]').prop("disabled", true);
            $('.foi-eficaz').removeClass('hide').addClass('show');
            $('[name=FlEficaz]').prop("disabled", true);
            $('div[id^=dropPE]').addClass('input-disable');
            $('div[id^=dropPE] a').addClass('input-disable');
            $('[name=upl]').prop("disabled", true);

            var aprovadoAcaoImediata = $('[name=Aprovado]').val();
            if (aprovadoAcaoImediata == "true") {
                $('.btn-ok-acao-imediata').show();
            } else if (aprovadoAcaoImediata == "false") {
                $('.btn-notok-acao-imediata').show();
            }
            $('.btn-ok-acao-imediata, .btn-notok-acao-imediata').prop("disabled", true);
        }
    },

    //Funções Editar Ação Imediata Não Conformidade
    setChecksOnPage: function () {

        var statusEtapa = parseInt($('[name=StatusEtapa]').val());
        var idUsuarioLogado = $('[name=IdUsuarioLogado]').val();
        var IdResponsavelImplementar = $('[name=IdResponsavelImplementar]').val();
        var IdResponsavelReverificador = $('[name=IdResponsavelReverificador]').val();

        if (statusEtapa == 2 && idUsuarioLogado == IdResponsavelImplementar) {
            //$('.dp-dt-efetiva-implementacao').show();
        } else if (statusEtapa == 3) {
            $('.dp-dt-efetiva-implementacao').show();
            $('.acao-imediata-upload').show();
            $('[name=DtEfetivaImplementacao]').prop("disabled", true);
        }
        if (statusEtapa == 3 && IdResponsavelReverificador == idUsuarioLogado) {
            $('.btn-confirm-acao-imediata').show();
            $('.btn-denied-acao-imediata').show();
            $('.foi-eficaz').removeClass('hide').addClass('show');
        }

    },

    setBackChecksOnPage: function () {
        $('.fa-check-circle, .fa-times-circle').unbind('click');
        $('.fa-check-circle, .fa-times-circle').on('click', function () {
            $(this).closest('td').find('.btn-ok-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-confirm-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('[name=Aprovado]').val('');
            APP.controller.AcaoCorretivaController.getChecksAcaoImediata();

        });
    },

    setCheckAcaoImediataOk: function () {

        $('.btn-confirm-acao-imediata').on('click', function () {
            $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-ok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('[name=Aprovado]').val(true);
            $(this).closest('td').find('[name=Aprovado]').val(true);
            APP.controller.AcaoCorretivaController.getChecksAcaoImediata();
        });

    },

    setCheckAcaoImediataNotOk: function () {

        $('.btn-denied-acao-imediata').on('click', function () {
            $(this).closest('td').find('.btn-confirm-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-denied-acao-imediata').fadeOut(300);
            $(this).closest('td').find('.btn-notok-acao-imediata').delay(400).fadeIn(300);
            $(this).closest('td').find('[name=Aprovado]').val(false);
            APP.controller.AcaoCorretivaController.getChecksAcaoImediata();
        });

    },

    getChecksAcaoImediata: function () {
        var checkSize = 0;
        var checkAcaoImediata = $('#AcoesImediatas table tbody tr').find('[name=Aprovado]');
        for (i = 0; i < checkAcaoImediata.size() ; i++) {
            var valueAprovador = $(checkAcaoImediata[i]).val();
            if (valueAprovador == "true") {
                checkSize++;
            }
        }
        if (checkSize == checkAcaoImediata.size()) {
            $('[name=FlEficaz').filter('[value=true]').prop('checked', true);
        } else {
            $('[name=FlEficaz').filter('[value=false]').prop('checked', true);
        }

    },

    sendFormSegundaEtapa: function () {
        $('.btn-salvar-notificar').unbind('click');
        $('.btn-salvar-notificar').on('click', function () {

            var statusEtapa = parseInt($('[name=StatusEtapa]').val());
            var IdRegistroConformidade = $('[name=IdRegistroConformidade]').val();

            var $form = $('#form-acao-corretiva-etapa-2');
            $form.append({ "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val() });

            var acaoCorretiva = {};

            var fluxos = {
                //Status 2 - Implementação
                _001: 'fluxo-01',
                //Status 3 - Reverificação
                _002: 'fluxo-02',
                _003: 'fluxo-03'
            };
            var dtEfetivaImplementacaoFoiPreenchida = $('[name=DtEfetivaImplementacao]').filter(x=>x == this.defaultValue).val();
            var idResponsavelPorImplementar = $('[name=IdResponsavelImplementar]').is(':disabled');
            switch (statusEtapa) {
                case 1:
                    acaoCorretiva = APP.controller.AcaoCorretivaController.getAcaoCorretiva(fluxos._001);
                    APP.controller.AcaoCorretivaController.saveFormSegundaEtapa(acaoCorretiva);
                    break;

                case 2:
                    acaoCorretiva = (dtEfetivaImplementacaoFoiPreenchida == undefined && idResponsavelPorImplementar == true) ? gestaoDeRisco = APP.controller.AcaoCorretivaController.getAcaoCorretiva(fluxos._002) : APP.controller.AcaoCorretivaController.getAcaoCorretiva(fluxos._001);
                    APP.controller.AcaoCorretivaController.saveFormSegundaEtapa(acaoCorretiva);

                    break;
                case 3:
                    acaoCorretiva = APP.controller.AcaoCorretivaController.getAcaoCorretiva(fluxos._003);
                    APP.controller.AcaoCorretivaController.saveFormSegundaEtapa(acaoCorretiva);
                    break;
                case 4:

                    break;
                default:
                    APP.controller.AcaoCorretivaController.saveFormSegundaEtapa(acaoCorretiva);
            }

        });
    },

    getAcaoCorretiva: function (fluxos) {

        var acaoCorretivaObj = {};

        acaoCorretivaObj.IdRegistroConformidade = $('[name=IdRegistroConformidade]').val();
        switch (fluxos) {
            case "fluxo-01":

                acaoCorretivaObj.StatusEtapa = 2;
                acaoCorretivaObj.DtDescricaoAcao = $('[name=DtDescricaoAcao]').val();
                acaoCorretivaObj.AcoesImediatas = APP.controller.AcaoCorretivaController.getAcaoImediataObj();
                acaoCorretivaObj.IdResponsavelReverificador = $('[name=IdResponsavelReverificador]').val();
                break;
            case "fluxo-02":

                acaoCorretivaObj.StatusEtapa = 2;
                acaoCorretivaObj.AcoesImediatas = APP.controller.AcaoCorretivaController.getAcaoImediataObj();
                break;
            case "fluxo-03":

                acaoCorretivaObj.AcoesImediatas = APP.controller.AcaoCorretivaController.getAcaoImediataObj();
                acaoCorretivaObj.FlEficaz = $('[name=FlEficaz]:checked').val();
                acaoCorretivaObj.StatusEtapa = $('[name=StatusEtapa]').val();
                break;
            default:
        }

        return acaoCorretivaObj;
    },

    getAcaoImediataObj: function () {

        var arrayAcoesImediatasObj = [];
        var $trAcoesImediatas = $('#AcoesImediatas table tbody tr');
        var $idUsuarioLogado = $('[name=IdUsuarioLogado]').val();
        var statusEtapa = parseInt($('[name=StatusEtapa]').val());
        var acaoImediata = {};
        $trAcoesImediatas.each(function (index, tr) {
            if (statusEtapa == 2) {
                acaoImediata = {
                    Descricao: $(tr).find('[name=Descricao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=IdResponsavelImplementar]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=DtPrazoImplementacao]').val(),
                    DtEfetivaImplementacao: $(tr).find('[name=DtEfetivaImplementacao]').val(),
                    ArquivoEvidencia: $(tr).find('[name=ArquivoEvidencia]').val(),
                    Estado: $(tr).find('[name=DtEfetivaImplementacao]').val() != 0 ? 16 : 0,
                    IdAcaoImediata: $(tr).find('[name=IdAcaoImediata]').val(),
                    IdUsuarioLogado: $idUsuarioLogado
                };
            } else if (statusEtapa == 3) {
                acaoImediata = {
                    Aprovado: $(tr).find('[name=Aprovado]').val(),
                    IdAcaoImediata: $(tr).find('[name=IdAcaoImediata]').val(),
                    Estado: $(tr).find('[name=DtEfetivaImplementacao]').val() != 0 ? 16 : 0,
                    IdUsuarioLogado: $idUsuarioLogado
                };
            } else {
                acaoImediata = {
                    Descricao: $(tr).find('[name=Descricao]').val(),
                    DtPrazoImplementacao: $(tr).find('[name=DtPrazoImplementacao]').val(),
                    DtEfetivaImplementacao: $(tr).find('[name=DtEfetivaImplementacao]').val(),
                    IdResponsavelImplementar: $(tr).find('[name=IdResponsavelImplementar]').val(),
                    Estado: $(tr).find('[name=Estado]').val(),
                    IdRegistroConformidade: $('[name=IdRegistroConformidade]').val(),
                    IdUsuarioLogado: $idUsuarioLogado
                };
            }

            arrayAcoesImediatasObj.push(acaoImediata);
        });

        return arrayAcoesImediatasObj;

    },

    saveFormSegundaEtapa: function (formAcaoCorretiva) {

        $.ajax({
            type: "POST",
            data: formAcaoCorretiva,
            dataType: 'json',
            url: "/AcaoCorretiva/SalvarSegundaEtapa",
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert("Salvo com sucesso!");
                }
            },
            error: function (result) {
                bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
                window.location.href = "/AcaoCorretiva/Index";
            }
        });

    },

    bind: function () {

        APP.component.Datapicker.init();
        APP.controller.AcaoCorretivaController.setAcaoImediata();
        APP.controller.AcaoCorretivaController.getOptionResponsavelImplementar();
        APP.controller.AcaoCorretivaController.setNewResponsavelImplementar();
        APP.controller.AcaoCorretivaController.showFileUploadAcaoImediata();
        APP.controller.AcaoCorretivaController.editAcaoImediata();
        APP.controller.AcaoCorretivaController.setConfirmAcaoImediata();
        APP.controller.AcaoCorretivaController.delAcaoImediata();

    },
    

    destravaFormEtapa: function () {
        //destravar etapa
        $('.btn-destravar-etapa').on('click', function (e) {

            var detectPartial = $('form').val() == "partial";
            var $forms = $('form');

            $.each($forms, (index, element) => {
                var visible = $(element).visible(detectPartial);
                if (visible) {
                    var data = {
                        "idAcaoCorretiva": $('[name=IdAcaoCorretiva]').val(),
                        "etapa": index + 1,
                        "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()
                    };
                    $.post(`/AcaoCorretiva/DestravarEtapa`, data, (result) => { })
                    .done((result) => {
                        if (result.StatusCode == 200) {
                            console.log(result);
                        } else if (result.StatusCode != 200) {

                        }
                    });
                }
            });

        });
    },

};
