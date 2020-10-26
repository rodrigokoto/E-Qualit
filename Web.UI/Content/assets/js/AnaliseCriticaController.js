/*
|--------------------------------------------------------------------------
| Controlador Analise Critica
|--------------------------------------------------------------------------
*/

APP.controller.AnaliseCriticaController = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexAnaliseCritica") {
            this.indexAnaliseCritica();
        }
        if (page == "CriarAnaliseCritica") {
            this.acoesAnaliseCritica();
        }

    },

    setup: function () {

        //Criar Analise Critica
        this.buttonAddPessoaFuncao = $(".add-pessoa-funcao");
        this.buttonSalvar = $(".btn-salvar");
        this.buttonExcluirAnaliseCritica = $(".excluir-analise-critica");
        this.buttonImprimir = $(".btn-imprimir");
    },

    //Models
    models: {

        //PerfilUsuarioModel: APP.model.PerfilUsuario,

    },

    //Index Analise Critica
    indexAnaliseCritica: function () {

        //APP.component.DataTable.init('#tb-index-analise-critica');

        $.fn.dataTable.moment('DD/MM/YYYY');    //Formatação sem Hora

        tabelaCalibracao = $('#tb-index-analise-critica').DataTable({
            columnDefs: [{
                target: [2,3],
                type: 'datetime-moment'
            }]
        });
        tabelaCalibracao.order([2, 'desc']);
        tabelaCalibracao.draw();
        this.setExcluirAnaliseCritica();
        this.eventoImprimir();
    },

    //Acoes Analise Critica
    acoesAnaliseCritica: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();

        this.setAndHide();
        this.setValidateForms();
        this.eventoImprimir();

        this.formCriarAnaliseCritica();
        if ($('[name=IdAnaliseCritica]').val() > 0) {
            APP.controller.AnaliseCriticaController.getTemasDescricao();
            APP.controller.AnaliseCriticaController.getTemasCores();

            APP.controller.AnaliseCriticaController.getTemasPossuiGestaoDeRisco();
            APP.controller.AnaliseCriticaController.setPossuiGestaoDeRisco();

            $('[name^=formGestaoDeRiscoRisco]:checked').each(function () {
                var teste = this.name;
                APP.controller.AnaliseCriticaController.getPossuiGestaoDeRiscoInformar(this);
            });

            APP.controller.AnaliseCriticaController.setPossuiGestaoDeRiscoInformar();

            APP.controller.AnaliseCriticaController.setHidePossuiGestaoDeRisco();
            APP.controller.AnaliseCriticaController.setHidePossuiGestaoDeRiscoInformar();

            APP.controller.AnaliseCriticaController.bindTemas();
        }
        else {
            APP.controller.AnaliseCriticaController.setPossuiGestaoDeRisco();
        }

        this.sendFormCriarAnaliseCritica();

    },

    setAndHide: function () {

        //

    },


    eventoImprimir: function () {

        this.buttonImprimir.on('click', function () {

            var IdAnaliseCritica = $(".IdAnaliseCritica").val();

            if (IdAnaliseCritica == null || IdAnaliseCritica == undefined) {
                IdAnaliseCritica = $(this).attr("idAnaliseCritica");
            }

            APP.controller.AnaliseCriticaController.imprimir(IdAnaliseCritica);

        });

    },

    imprimir: function (IdAnaliseCritica) {


        if (IdAnaliseCritica != null) {

            APP.component.Loading.showLoading();

            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/AnaliseCritica/PDF?id=' + IdAnaliseCritica, true);
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


    setValidateForms: function () {

        var ObjFormCriarAnaliseCriticaValidate = APP.controller.AnaliseCriticaController.getObjObjFormCriarAnaliseCriticaValidate();
        APP.component.ValidateForm.init(ObjFormCriarAnaliseCriticaValidate, '#form-criar-analisecritica');
        APP.component.ValidateForm.init(ObjFormCriarAnaliseCriticaValidate, '#form-analisecriticatema');

    },

    //Formulario Analise Critica
    formCriarAnaliseCritica: function () {

        this.setAndHideCriarAnaliseCritica();
        this.getResponsavel();
        this.addPessoaFuncao();
        this.delPessoaFuncao();
        //this.getParticipantes();
        this.setFuncaoPessoa();

        this.setTemas();
        this.editTemas();
        this.setTemaBox();
        this.delTemaCombobox();
        this.getTodosResponsaveisPorAcaoImediata();
        this.getProcessosPorSite();

    },



    setAndHideCriarAnaliseCritica: function () {

        //

    },

    getResponsavel: function () {


        var idProcesso = $('[name=IdProcesso]').val();
        var idSite = $('[name=IdSite]').val();
        var idFuncao = 48; // Funcionalidade(Registro da ata) que permite Criar Analise Critica;
        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + idProcesso + '&idSite=' + idSite + '&idFuncao=' + idFuncao + '', (result) => {
            if (result.StatusCode == 200) {
                APP.component.SelectListCompare.selectList(result.Lista, $('[name="formCriarAnaliseCriticaResponsavel"] option'), $('[name="formCriarAnaliseCriticaResponsavel"]'), 'IdUsuario', 'NmCompleto');
            }
        });
    },

    addPessoaFuncao: function () {

        this.buttonAddPessoaFuncao.unbind('click');
        this.buttonAddPessoaFuncao.on('click', function () {

            if ($("#tb-passoa-funcao").find('[name=formCriarAnaliseCriticaCargo]').last().val() != "") {

                var msgRequiredParticipante = $('[name=placeholderDropdownNameSelect]').val();
                var placeholderDropdownNameSelect = $('[name=placeholderDropdownNameSelect]').val();

                var index = $("#tb-passoa-funcao tbody tr").size();

                var html = '';
                html += '<tr>';
                html += '<!-- Participante -->';
                html += '<td align="center">';
                html += '<div class="form-group">';
                html += '<select id="form-criar-analise-critica-participante" name="formCriarAnaliseCriticaParticipante" class="form-control"';
                html += 'data-msg-required="' + msgRequiredParticipante + '">';
                html += '<option value="">' + placeholderDropdownNameSelect + '</option>';
                html += '</select>';
                html += '</div>';
                html += '</td>';
                html += '<!-- FunÃ§Ã£o -->';
                html += '<td>';
                html += '';
                html += '<div class="form-group">';
                html += '<input id="form-criar-analise-critica-cargo" name="formCriarAnaliseCriticaCargo" class="form-control" placeholder="" data-msg-required="" value="" disabled />';
                html += '</div>';
                html += '</td>';
                html += '<!-- Icon Del -->';
                html += '<td align="center">';
                html += '<a href="#" class="excluir-funcionario icon-cliente">';
                html += '<i class="fa fa-trash" aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
                html += '</a>';
                html += '</td>';
                html += '</tr>';

                $("#tb-passoa-funcao tbody:last-child").append(html);

                APP.controller.AnaliseCriticaController.bind();
            }
        });
    },

    delPessoaFuncao: function () {

        $('.excluir-funcionario').on('click', function (event) {
            event.preventDefault();
            $(this).closest('tr').remove();
        });

    },

    getParticipantes: function () {

        var idAnaliseCritica = $('[name=IdAnaliseCritica]').val();
        $.get('/AnaliseCritica/ObterUsuariosPorAnaliseCritica?idAnaliseCritica=' + idAnaliseCritica, function (result) {
            $.each(result.Lista, function (key, value) {
                if (result.StatusCode == 200) {
                    APP.component.SelectListCompare.selectList(result.Lista, $('#tb-passoa-funcao tbody tr:last-child [name="formCriarAnaliseCriticaParticipante"] option'), $('#tb-passoa-funcao tbody tr:last-child [name="formCriarAnaliseCriticaParticipante"]'), 'IdUsuario', 'NmCompleto');
                }
            });
        });

    },

    setParticipante: function () {

        $('[name=formCriarAnaliseCriticaParticipante]').each(function (key, value) {
            var idParticipanteSelected = $(this).closest('tr').find('.id-funcionario').val();
            $(value).val(idParticipanteSelected);
        });
    },

    setFuncaoPessoa: function () {

        $("[name^=formCriarAnaliseCriticaParticipante]").change(function () {
            var select = $(this).find(':selected');
            var idUsuario = $(this).val();
            $.get('/Usuario/ObterFuncao?idUsuario=' + idUsuario, function (result) {
                if (result.StatusCode == 200) {
                    $(select.closest("tr").find('[name=formCriarAnaliseCriticaCargo]').val(result.Funcoes));
                }
            });
        });

    },

    bind: function () {

        //APP.controller.AnaliseCriticaController.getResponsavel();
        APP.controller.AnaliseCriticaController.setup();
        APP.controller.AnaliseCriticaController.addPessoaFuncao();
        APP.controller.AnaliseCriticaController.getParticipantes();
        APP.controller.AnaliseCriticaController.setFuncaoPessoa();
        APP.controller.AnaliseCriticaController.delPessoaFuncao();
        APP.controller.AnaliseCriticaController.setValidateForms();

    },

    setTemas: function () {

        var idSite = $('[name=IdSite]').val();

        $.get('/ControladorCategorias/ListaAtivos?tipo=tema&site=' + idSite, function (result) {
            $.each(result.Lista, function (key, value) {
                if (result.StatusCode == 202) {
                    APP.component.SelectListCompare.selectList(result.Lista, $('[name="formCriarAnaliseCriticaTema"] option'), $('[name="formCriarAnaliseCriticaTema"]'), 'IdControladorCategorias', 'Descricao');
                }
            });
        });

    },

    editTemas: function () {

        $('.add-tema-analise-critica').unbind('click');
        $('.add-tema-analise-critica').on('click', function () {

            var idSite = $('#site-analise-critica').val();
            APP.controller.ControladorCategoriasController.init(idSite, 'tema', APP.controller.AnaliseCriticaController.setTemas, '.add-tema-analise-critica');

        });

    },

    setTemaBox: function () {

        $('[name=formCriarAnaliseCriticaTema]').change(function () {
            //Valor zero representa o selecione no combobox
            var temaSelected = $("[name=formCriarAnaliseCriticaTema] option:selected").val();
            var textoTemaSelected = $("[name=formCriarAnaliseCriticaTema] option:selected").text();
            var numeroAC = $("[name=numeroAC]").val();

            if (temaSelected != 0) {

                var id = temaSelected;
                var tema = '';
                tema += '<li class="row li-tema-AC">';
                tema += '<div class="barra-busca">';
                tema += '<!-- Titulo -->';
                tema += '<div class="col-lg-10 col-md-10 col-sm-10 col-xs-10">';
                tema += '<h2>' + textoTemaSelected + '</h2>';
                tema += '</div>';
                tema += '<!-- Acoes -->';
                tema += '<div class="col-lg-2 col-md-2 col-sm-2 col-xs-2">';
                tema += '<a href="#" class="btn icon-analise editar-tema" data-toggle="collapse">';
                tema += '<i class="fa fa-pencil fa-1" aria-hidden="true"></i>';
                tema += '</a>';
                tema += '<a href="#" class="btn icon-analise excluir-tema" data-toggle="collapse">';
                tema += '<i class="fa fa-trash fa-1" aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
                tema += '</a>';
                tema += '</div>';
                tema += '<!-- Gestao de Risco -->';
                tema += '<input type="hidden" name="idControladorCategoria" value="' + temaSelected + '">';
                tema += '<div name="GestaoDeRisco" class="gestaoDeRiscoPartial-' + temaSelected + '" id="gestaoDeRiscoPartial-' + temaSelected + '" data-name="' + temaSelected + '">';
                tema += '</div>';
                tema += '</div>';
                tema += '</li>';


                $("#panel-lista-temas").append(tema);
                $("[name=formCriarAnaliseCriticaTema] option:selected").remove();

                //$(divGestaoDeRisco).find('[name=form-Informargestao-de-risco-risco-nao' + temaSelected + ']').prop('checked', true);


                var divGestaoDeRisco = $('.gestaoDeRiscoPartial-' + temaSelected);
                APP.component.GestaoDeRiscoPartial.init(divGestaoDeRisco, temaSelected);



                APP.controller.AnaliseCriticaController.delTemaCombobox();
                APP.controller.AnaliseCriticaController.bindTemas();

                $(divGestaoDeRisco).find('[id^=form-Informargestao-de-risco-risco-nao]').prop('checked', true);
                $(divGestaoDeRisco).find('[id^=form-Informargestao-de-risco-risco-nao]').trigger("change");
                //$(divGestaoDeRisco).find('[id=form-Informargestao-de-risco-risco-nao' + temaSelected + ']').trigger("click");

            }
        });

    },

    delTemaCombobox: function () {
        $(".excluir-tema").unbind("click");
        $(".excluir-tema").bind('click', function () {

            var idControladorCategoria = $(this).closest('.li-tema-AC').find('[name=idControladorCategoria]').val();
            var tituloTema = $(this).closest('.li-tema-AC').find('h2').text();
            APP.controller.AnaliseCriticaController.setTemaCombobox(idControladorCategoria, tituloTema);
            var id = $(this).closest('.li-tema-AC').find('[name=formGestaoDeRiscoDescricao]').attr("id");
            CKEDITOR.instances[id].destroy(true);

            $(this).closest('.li-tema-AC').remove();

        });
    },

    editTemaExistenteBox: function () {

        $('.editar-tema').unbind('click');
        $('.editar-tema').on('click', function () {

            $(this).closest('.li-tema-AC').find('[id^=gestaoDeRiscoPartial]').slideToggle();

        });

    },

    setTemaCombobox: function (idTema, descricaoTema) {


        $('[name=formCriarAnaliseCriticaTema]').append($('<option />').val(idTema).text(descricaoTema));
    },

    getTodosResponsaveisPorAcaoImediata: function (_this) {

        var idSite = $('[name=IdSite]').val();
        var idFuncao = 49; // Funcionalidade(Implementar aÃ§Ã£o) que permite Criar aÃ§Ãµes Corretivas
        var idProcesso = $('[name=IdProcesso]').val();
        $.get('/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=' + idProcesso + '&idSite=' + idSite + '&idFuncao=' + idFuncao + '}', (result) => {
            if (result.StatusCode == 200) {

                if ($(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"]').find("option").length <= 1) {
                    var comboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"] option');
                    var idComboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"]');
                    APP.component.SelectListCompare.selectList(result.Lista, comboResponsavel, idComboResponsavel, 'IdUsuario', 'NmCompleto');
                }

            }
        });

    },


    getProcessosPorSite: function (_this) {

        var idSite = $('[name=IdSite]').val();
        $.get('/Processo/ListaProcessosPorSite?idSite=' + idSite, function (result) {

            if (result.StatusCode == 200) {
                var ultimo = $('[name="formGestaoDeRiscoProcesso"]').length - 1;
                var comboProcesso = $('[name="formGestaoDeRiscoProcesso"]')[ultimo];

                //original
                $.each(result.Lista, (key, val) => {
                    var $option = $('<option></option>');
                    $(comboProcesso).append(
                        $option.val(val.IdProcesso).text(val.Nome)
                    );
                });

                //if ($('[name="formGestaoDeRiscoProcesso"]').length <= 1) {
                //	var comboResponsavel = $('[name="formGestaoDeRiscoProcesso"] option')[ultimo];
                //	var idComboResponsavel = $('[name="formGestaoDeRiscoProcesso"]')[ultimo];

                //	APP.component.SelectListCompare.selectList(result.Lista, comboResponsavel, idComboResponsavel, 'IdProcesso', 'Nome');
                //}

                ////var comboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoProcesso"] option');
                ////var idComboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoProcesso"]');
                ////if ($(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoProcesso"]').find("option").length <= 1) {
                ////	var comboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoProcesso"] option');
                ////	var idComboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoProcesso"]');
                ////	
                ////	APP.component.SelectListCompare.selectList(result.Lista, comboResponsavel, idComboResponsavel, 'IdUsuario', 'NmCompleto');
                ////}

            }

        });

    },

    setPopulaGestaoRisco: function (result, idTema) {

        $('#' + idTema).find('.DsTexto').val(result.GestaoDeRisco.DescricaoRegistro);

        APP.component.GestaoDeRiscoPartial.showBarRating(('#' + idTema), result.CorRisco);

        if (result.CorRisco === "Amarelo" || result.CorRisco === "Vermelho") {
            $('#' + idTema).find("[name=divGestaoRisco]").show();
        }

        if (result.PossuiGestaoRisco) {
            $('#' + idTema).find('#list-option-1').prop('checked', true);
        }
        else {
            $('#' + idTema).find('#list-option-2').prop('checked', true);
        }

        $('#' + idTema).find('.indentificacaoDeRisco').val(result.DsEvidenciaEficaciaAcao);

    },

    delParticipanteCombobox: function () {
        $('.participante-drop').children().remove();
    },

    setTemaGestaoDeRisco: function () {

        $('.editar-tema').unbind('click');
        $('.editar-tema').on('click', function () {

            var gestaoRiscoSelecionada = $(this).closest(".li-tema-AC").find(".gestaoDeRiscoPartial");
            var idTema = $(this).closest(".li-tema-AC").find('[name=idControladorCategoria]').val();
            var idDiv = gestaoRiscoSelecionada.data("name");
            var idGestao = $(this).closest('.barra-busca').find('[name=GestaoDeRisco]');

            APP.component.GestaoDeRiscoPartial.init(gestaoRiscoSelecionada, true, idTema);
            APP.controller.AnaliseCriticaController.bind();

            var temas = JSON.parse($("#temas-analisecritica").val());

            for (var index in temas) {
                if (temas[index].IdTema == idTema) {
                    APP.controller.AnaliseCriticaController.setPopulaGestaoRisco(temas[index], idTema);
                }
            }

        });

    },

    bindTemas: function () {

        APP.controller.AnaliseCriticaController.editTemas();
        APP.controller.AnaliseCriticaController.setTemaBox();
        APP.controller.AnaliseCriticaController.delTemaCombobox();
        APP.controller.AnaliseCriticaController.editTemaExistenteBox();
        APP.controller.AnaliseCriticaController.setValidateForms();

        APP.controller.AnaliseCriticaController.getProcessosPorSite();

    },

    getObjObjFormCriarAnaliseCriticaValidate: function () {

        var acoesPadraoFormCriarPadraoObj = {
            //formCriaClienteLogo: {required: true, minFiles: 1},
            //Analise Critica
            formCriarAnaliseCriticaResponsavel: { required: true },
            formCriarAnaliseCriticaAta: { required: true },
            formCriarAnaliseCriticaDtCriacao: { required: true },
            formCriarAnaliseCriticaDtProximaAnalise: { required: true },
            //Analise Critica - Pessoa Funcao
            formCriarAnaliseCriticaParticipante: { required: true },
            formCriarAnaliseCriticaCargo: { required: true },
            //Analise Critica - Gestao de Risco
            //formCriarAnaliseCriticaTema : {required: true},
            //formGestaoDeRiscoDescricao : {required: true},
            formGestaoDeRiscoCriticidade: { required: true },
            //formGestaoDeRiscoRisco : {required: true},
            //formGestaoDeRiscoResponsavelDefinicao : {required: true},
            //formGestaoDeRiscoNumero : {required: true},
            //formGestaoDeRiscoIdentificacao : {required: true},

        };

        return acoesPadraoFormCriarPadraoObj;

    },

    getObjFormCriarAnaliseCritica: function () {


        var acoesPadraoFormCriarPadraoObj = {

            IdAnaliseCritica: $('[name=IdAnaliseCritica]').val(),
            IdSite: $('[name=IdSite]').val(),
            IdResponsavel: $('[name=formCriarAnaliseCriticaResponsavel] :selected').val(),
            Ata: $('[name=formCriarAnaliseCriticaAta]').val(),
            DataCriacao: $('[name=formCriarAnaliseCriticaDtCriacao]').val(),
            DataProximaAnalise: $('[name=formCriarAnaliseCriticaDtProximaAnalise]').val(),
            Ativo: true,
            Temas: APP.controller.AnaliseCriticaController.getObjFormTemasAnaliseCriticaArray(),
            Funcionarios: APP.controller.AnaliseCriticaController.getObjFormPessoaFuncaoAnaliseCriticaArray(),
        };


        return acoesPadraoFormCriarPadraoObj;

    },

    getObjFormPessoaFuncaoAnaliseCriticaArray: function () {

        var arrayFormAnaliseCriticaObj = [];

        $('#tb-passoa-funcao tbody tr').each(function () {

            var formCriarAnaliseCriticaObj = {
                IdAnaliseCriticaFuncionario: $(this).find('[name=formCriarAnaliseCriticaFuncionarioId]').val(),
                IdUsuario: $(this).find('[name=formCriarAnaliseCriticaParticipante] :selected').val(),
                Funcao: $(this).find('[name=formCriarAnaliseCriticaCargo]').val(),
            };

            arrayFormAnaliseCriticaObj.push(formCriarAnaliseCriticaObj);

        });

        return arrayFormAnaliseCriticaObj;

    },

    getObjFormTemasAnaliseCriticaArray: function () {
        var arrayFormTemasAnaliseCriticaObj = [];

        $('#panel-lista-temas .li-tema-AC').each(function () {

            var idCkEditor = $(this).find('[name=formGestaoDeRiscoDescricao]').attr('id');

            var formCriarTemasAnaliseCriticaObj = {
                IdTema: $(this).find('[name=IdTema]').val(),
                Descricao: CKEDITOR.instances[idCkEditor].getData(),
                CorRisco: $(this).find("[name^=formGestaoDeRiscoCriticidade] :selected").val(),
                PossuiGestaoRisco: $(this).find('[name^=formGestaoDeRiscoRisco]:checked').val(),

                PossuiInformarGestaoRisco: $(this).find('[name^=formInformarGestaoDeRiscoRisco]:checked').val(),
                IdControladorCategoria: $(this).find('[name=idControladorCategoria]').val(),
                IdGestaoDeRisco: $(this).find('[name=IdGestaoDeRisco]').val(),
                IdProcesso: $(this).find('[name=formGestaoDeRiscoProcesso]').val(),
                GestaoDeRisco:
                {
                    CriticidadeGestaoDeRisco: $('.br-current').data('rating-value') == undefined ? 0 : $(this).find("[name^=formGestaoDeRiscoCriticidade] :selected").val(),
                    //TipoRegistro: 'gr',
                    IdResponsavelInicarAcaoImediata: $(this).find('[name=formGestaoDeRiscoResponsavelDefinicao]').val(),
                    DescricaoRegistro: $(this).find('[name=formGestaoDeRiscoIdentificacao]').val(),

                    Causa: $(this).find('[name=formGestaoDeRiscoCausa]').val(),
                    DsJustificativa: $(this).find('[name=formGestaoDeRiscojustificativa]').val()
                }
            };

            arrayFormTemasAnaliseCriticaObj.push(formCriarTemasAnaliseCriticaObj);

        });

        return arrayFormTemasAnaliseCriticaObj;

    },

    //Funcoes Edit Analise Critica
    getTemasDescricao: function () {

        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

            $(".excluir-tema").hide();
            $(".editar-tema").hide();

            var idDescricao = $(element).attr('id');
            CKEDITOR.replace(idDescricao);
        });

    },

    getTemasCores: function () {

        var divBarRating = $('.barraRating');
        APP.component.BarRating.setBarRatingGestaoDeRisco(divBarRating, 'bars-1to10');

        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

            var divContext = $(element).closest('[name=GestaoDeRisco]');

            var corRisco = $('[name=formGestaoDeRiscoCriticidadeCor]').val();
            var lastCores = $("[data-rating-value='" + corRisco + "']").last().index();
            for (i = 0; i <= lastCores; i++) {
                $($('.br-theme-bars-1to10').find('.br-widget a')[i]).addClass('br-selected');
            }
            $($('.br-theme-bars-1to10').find('.br-widget a')[lastCores]).trigger("click");
            $('.br-theme-bars-1to10').find("[data-rating-value='" + corRisco + "']").addClass('br-current');
            var widget = $('.br-widget');
        });
    },

    getTemasPossuiGestaoDeRisco: function () {
        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

            var divContext = $(element).closest('[name=GestaoDeRisco]');
            var temGestaoDeRisco = divContext.find('[name=formGestaoDeRiscoRisco]').val();
            divContext.find('[name=formGestaoDeRiscoResponsavelDefinicao][value=' + temGestaoDeRisco + ']').prop('checked', temGestaoDeRisco);
        });
    },

    getTemasPossuiGestaoDeRiscoInformar: function () {
        //$('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {
        //	var divContext = $(element).closest('[name=GestaoDeRisco]');
        //	var temGestaoDeRisco = divContext.find('[name=formGestaoDeRiscoDescricao]').val();
        //	divContext.find('[name=formInformarGestaoDeRiscoRisco][value=' + temGestaoDeRisco + ']').prop('checked', temGestaoDeRisco);
        //});
    },


    setPossuiGestaoDeRisco: function () {
        $('.formGestaoDeRiscoRisco').change(function () {

            var possuiGestaoDeRisco = APP.controller.AnaliseCriticaController.getPossuiGestaoDeRisco(this);
            APP.controller.AnaliseCriticaController.setRulesPossuiGestaoDeRisco(possuiGestaoDeRisco, this);

        });

    },

    setPossuiGestaoDeRiscoInformar: function () {

        $('.formInformarGestaoDeRiscoRisco').change(function () {

            var possuiGestaoDeRisco = APP.controller.AnaliseCriticaController.getPossuiGestaoDeRiscoInformar(this);
            APP.controller.AnaliseCriticaController.setRulesPossuiGestaoDeRiscoInformar(possuiGestaoDeRisco, this);

        });

    },

    getPossuiGestaoDeRisco: function (_this) {

        var possuiGestaoDeRisco = APP.component.Radio.init(_this.name);
        return possuiGestaoDeRisco;

    },


    getPossuiGestaoDeRiscoInformar: function (_this) {
        var possuiGestaoDeRisco = APP.component.Radio.init(_this.name);
        return possuiGestaoDeRisco;

    },

    setRulesPossuiGestaoDeRisco: function (_possuiGestaoDeRisco, _this) {

        if (_possuiGestaoDeRisco == "true") {
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").show();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").show();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").show();
            $(_this).parent().parent().parent().parent().parent().find('.JustificativaGestaoDeRisco').hide();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoCausa]').closest("[class^=col]").show();

        } else {
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").hide();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").hide();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").hide();
            $(_this).parent().parent().parent().parent().parent().find('.JustificativaGestaoDeRisco').show();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoCausa]').closest("[class^=col]").hide();
        }

    },

    setRulesPossuiGestaoDeRiscoInformar: function (_possuiGestaoDeRisco, _this) {

        if (_possuiGestaoDeRisco == "true") {
            $(_this).parent().parent().parent().parent().parent().parent().find('[name=divEsconder]').show();
            //$(_this).parent().parent().parent().
            //$(_this).find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").show();
            //$(_this).find('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").show();
            //$(_this).find('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").show();
            //$(_this).find('.JustificativaGestaoDeRisco').hide();
            //$(_this).find('[name=formGestaoDeRiscoCausa]').closest("[class^=col]").show();

        } else {
            $(_this).parent().parent().parent().parent().parent().parent().find('[name=divEsconder]').hide();
            //$(_this).find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").hide();
            //$(_this).find('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").hide();
            //$(_this).find('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").hide();
            //$(_this).find('.JustificativaGestaoDeRisco').show();
            //$(_this).find('[name=formGestaoDeRiscoCausa]').closest("[class^=col]").hide();
        }

    },

    setHidePossuiGestaoDeRisco: function () {


        $('[name^=formGestaoDeRiscoRisco]:checked').each(function () {
            var teste = this.name;
            $('[name=' + teste + ']').trigger('change');
            //var isVisible = $(this).is(':visible');
            //if (isVisible) {
            //	var validate = $(this).closest('form').valid();
            //	if (validate != true) {
            //		valid = false;
            //	}

            //}
        });

        //$('[name^=formGestaoDeRiscoRisco]').trigger('change');

    },

    setHidePossuiGestaoDeRiscoInformar: function () {

        $('[name^=formInformarGestaoDeRiscoRisco]:checked').each(function () {
            var teste = this.name;
            $('[name=' + teste + ']').trigger('change');
            //var isVisible = $(this).is(':visible');
            //if (isVisible) {
            //	var validate = $(this).closest('form').valid();
            //	if (validate != true) {
            //		valid = false;
            //	}

            //}
        });


        //$('[name^=formInformarGestaoDeRiscoRisco]').trigger('change');

    },

    //Todos
    sendFormCriarAnaliseCritica: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = APP.controller.AnaliseCriticaController.validateForms();

            //var validate = true;
            if (validate == true) {
                var padraoObj = APP.controller.AnaliseCriticaController.getCriarAnaliseCriticaObj();
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

    getCriarAnaliseCriticaObj: function () {

        var analiseCriticaObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "analisecritica":
                        analiseCriticaObj = APP.controller.AnaliseCriticaController.getObjFormCriarAnaliseCritica();

                        var valida = true;

                        if (analiseCriticaObj.Funcionarios.length == 0) {

                            bootbox.alert(_options.labelParticipantesObrigatorio);
                            valida = false;
                        }
                        else if (analiseCriticaObj.Temas.length == 0) {

                            bootbox.alert(_options.labelTemaObrigatorio);
                            valida = false;
                        }
                        else if (analiseCriticaObj.Temas.length > 0) {

                            if (analiseCriticaObj.Temas[0].Descricao == "" || analiseCriticaObj.Temas[0].Descricao == null) {
                                bootbox.alert(_options.labelDescricaoObrigatorio);
                                valida = false;
                            }



                            //else if (analiseCriticaObj.Temas[0].GestaoDeRisco.DescricaoRegistro == "" || analiseCriticaObj.Temas[0].GestaoDeRisco.DescricaoRegistro == null) {
                            //    bootbox.alert(_options.labelIdentificacaoRiscoObrigatorio);
                            //    valida = false;
                            //}
                            //else if (analiseCriticaObj.Temas[0].GestaoDeRisco.CriticidadeGestaoDeRisco == "" || analiseCriticaObj.Temas[0].GestaoDeRisco.CriticidadeGestaoDeRisco == null || analiseCriticaObj.Temas[0].GestaoDeRisco.CriticidadeGestaoDeRisco == 0) {
                            //    bootbox.alert(_options.labelCriticidadeGestaoDeRisco);
                            //    valida = false;
                            //}
                            else if ((analiseCriticaObj.Temas[0].PossuiGestaoRisco == null || analiseCriticaObj.Temas[0].PossuiGestaoRisco == "") && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert(_options.labelPossuiGestaoRisco);
                                valida = false;
                            }

                            else if ((analiseCriticaObj.Temas[0].GestaoDeRisco.DescricaoRegistro == null || analiseCriticaObj.Temas[0].GestaoDeRisco.DescricaoRegistro == "") && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert("Digite uma identificação de risco");
                                valida = false;
                            }

                            else if ((analiseCriticaObj.Temas[0].Descricao == null || analiseCriticaObj.Temas[0].Descricao == "") && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert("Obrigatória digitar uma descrição");
                                valida = false;
                            }

                            else if ((analiseCriticaObj.Temas[0].IdProcesso == null || analiseCriticaObj.Temas[0].IdProcesso == "") && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert("Obrigatória escolher um processo");
                                valida = false;
                            }
                            else if ((analiseCriticaObj.Temas[0].CorRisco == null || analiseCriticaObj.Temas[0].CorRisco == "" || analiseCriticaObj.Temas[0].CorRisco <= 1) && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert("Obrigatória classificar o risco");
                                valida = false;
                            }


                            else if ((analiseCriticaObj.Temas[0].GestaoDeRisco.IdResponsavelInicarAcaoImediata == null || analiseCriticaObj.Temas[0].GestaoDeRisco.IdResponsavelInicarAcaoImediata == "") && analiseCriticaObj.Temas[0].PossuiGestaoRisco == "true" && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert(_options.labelResponsavelDefinicaoObrigatorio);
                                valida = false;
                            }

                            else if ((analiseCriticaObj.Temas[0].GestaoDeRisco.Causa == null || analiseCriticaObj.Temas[0].GestaoDeRisco.Causa == "") && analiseCriticaObj.Temas[0].PossuiGestaoRisco == "true" && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert("Causa obrigatória");
                                valida = false;
                            }


                            else if ((analiseCriticaObj.Temas[0].GestaoDeRisco.DsJustificativa == null || analiseCriticaObj.Temas[0].GestaoDeRisco.DsJustificativa == "") && analiseCriticaObj.Temas[0].PossuiGestaoRisco == "false" && analiseCriticaObj.Temas[0].PossuiInformarGestaoRisco == "true") {
                                bootbox.alert("Justificativa obrigatória");
                                valida = false;
                            }


                        }


                        if (valida == true) {
                            APP.controller.AnaliseCriticaController.saveFormCriarAnaliseCritica(analiseCriticaObj, '/AnaliseCritica/Salvar');
                        }

                        break;
                }
            }

        });


    },

    saveFormCriarAnaliseCritica: function (analiseCriticaObj, url) {

        var erro = "";
        $.ajax({
            type: "POST",
            data: analiseCriticaObj,
            dataType: 'json',
            url: url,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    bootbox.alert(result.Success, function (result) {
                        window.location.href = "/AnaliseCritica/Index";
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

    setExcluirAnaliseCritica: function () {

        this.buttonExcluirAnaliseCritica.unbind('click');
        this.buttonExcluirAnaliseCritica.bind('click', function () {

            event.preventDefault();

            var idDocumento = $(this).attr("idAnaliseCritica");

            bootbox.confirm(_options.DesejaRealmenteExcluirEsseRegistro, function (result) {
                if (result) {
                    $.post('/AnaliseCritica/Excluir/' + idDocumento, function (data, status) { }).done(function (data) {
                        if (data.StatusCode == "200") {

                            bootbox.alert(data.Success, function (result) {
                                window.location.href = "/AnaliseCritica/Index";
                            });

                        }
                        if (data.StatusCode == "500") {
                            bootbox.alert(_options.MsgErroExclusaoRegistro);
                        }
                    });
                }
            });
        });

    },



};
