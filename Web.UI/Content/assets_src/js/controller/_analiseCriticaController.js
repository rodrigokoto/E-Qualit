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


    },

    //Models
    models: {
        
        //PerfilUsuarioModel: APP.model.PerfilUsuario,

    },

    //Index Analise Critica
    indexAnaliseCritica : function () {

        APP.component.DataTable.init('#tb-index-analise-critica');

    },

    //Acoes Analise Critica
    acoesAnaliseCritica : function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.FileUpload.init();
        APP.component.Mascaras.init();
        this.setAndHide();
        this.setValidateForms();
        this.formCriarAnaliseCritica();

        if ($('[name=IdAnaliseCritica]').val() > 0){
            APP.controller.AnaliseCriticaController.getTemasDescricao();
            APP.controller.AnaliseCriticaController.getTemasCores();
            APP.controller.AnaliseCriticaController.getTemasPossuiGestaoDeRisco();
            APP.controller.AnaliseCriticaController.setPossuiGestaoDeRisco();
            APP.controller.AnaliseCriticaController.setHidePossuiGestaoDeRisco();
            APP.controller.AnaliseCriticaController.bindTemas();
        }

        this.sendFormCriarAnaliseCritica();

    },

    setAndHide : function () {

        //

    },

    setValidateForms : function () {

        var ObjFormCriarAnaliseCriticaValidate = APP.controller.AnaliseCriticaController.getObjObjFormCriarAnaliseCriticaValidate();
        APP.component.ValidateForm.init(ObjFormCriarAnaliseCriticaValidate, '#form-criar-analisecritica');
        APP.component.ValidateForm.init(ObjFormCriarAnaliseCriticaValidate, '#form-analisecriticatema');

    },

    //Formulario Analise Critica
    formCriarAnaliseCritica : function () {

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

    },

    setAndHideCriarAnaliseCritica : function () {

        //

    },

    getResponsavel: function () {
        
        var idProcesso = $('[name=IdProcesso]').val();
        var idSite = $('[name=IdSite]').val();
        var idFuncao = 48; // Funcionalidade(Registro da ata) que permite Criar Analise Critica;
        $.get(`/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=${idProcesso}&idSite=${idSite}&idFuncao=${idFuncao}`, (result) => {
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
                html += 'data-msg-required="'+msgRequiredParticipante+'">';
                html += '<option value="">'+placeholderDropdownNameSelect+'</option>';
                html += '</select>';
                html += '</div>';
                html += '</td>';
                html += '<!-- Função -->';
                html += '<td>';
                html += '';
                html += '<div class="form-group">';
                html += '<input id="form-criar-analise-critica-cargo" name="formCriarAnaliseCriticaCargo" class="form-control" placeholder="" data-msg-required="" value="" disabled />';
                html += '</div>';
                html += '</td>';
                html += '<!-- Icon Del -->';
                html += '<td align="center">';
                html += '<a href="#" class="excluir-funcionario icon-cliente">';
                html += '<i class="fa fa-trash" aria-hidden="true"></i>';
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

    bind : function () {
        
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

            if (temaSelected != 0) {

                var id = temaSelected;
                var tema = '';
                tema += '<li class="row li-tema-AC">';
                tema += '<div class="barra-busca">';
                tema += '<!-- Titulo -->';
                tema += '<div class="col-lg-10 col-md-10 col-sm-10 col-xs-10">';
                tema += '<h2>'+textoTemaSelected+'</h2>';
                tema += '</div>';
                tema += '<!-- Acoes -->';
                tema += '<div class="col-lg-2 col-md-2 col-sm-2 col-xs-2">';
                tema += '<a href="#" class="btn icon-analise editar-tema" data-toggle="collapse">';
                tema += '<i class="fa fa-pencil fa-1" aria-hidden="true"></i>';
                tema += '</a>';
                tema += '<a href="#" class="btn icon-analise excluir-tema" data-toggle="collapse">';
                tema += '<i class="fa fa-trash fa-1" aria-hidden="true"></i>';
                tema += '</a>';
                tema += '</div>';
                tema += '<!-- Gestao de Risco -->';
                tema += '<input type="hidden" name="idControladorCategoria" value="' + temaSelected + '">';
                tema += '<div name="GestaoDeRisco" class="gestaoDeRiscoPartial-'+temaSelected+'" id="gestaoDeRiscoPartial-'+temaSelected+'" data-name="'+temaSelected+'">';
                tema += '</div>';
                tema += '</div>';
                tema += '</li>';

                $("#panel-lista-temas").append(tema);
                $("[name=formCriarAnaliseCriticaTema] option:selected").remove();

                var divGestaoDeRisco = $('.gestaoDeRiscoPartial-' + temaSelected);
                APP.component.GestaoDeRiscoPartial.init(divGestaoDeRisco, temaSelected);
                
                APP.controller.AnaliseCriticaController.delTemaCombobox();
                APP.controller.AnaliseCriticaController.bindTemas();

            }
        });

    },

    delTemaCombobox: function () {
        $(".excluir-tema").unbind("click");
        $(".excluir-tema").bind('click', function () {
            
            var idControladorCategoria = $(this).closest('.li-tema-AC').find('[name=idControladorCategoria]').val();
            var tituloTema = $(this).closest('.li-tema-AC').find('h2').text();
            APP.controller.AnaliseCriticaController.setTemaCombobox(idControladorCategoria, tituloTema);
            CKEDITOR.instances[idControladorCategoria].destroy(true);

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
        var idFuncao = 91; // Funcionalidade(Implementar ação) que permite Criar ações Corretivas
        var idProcesso = $('[name=IdProcesso]').val();
        $.get(`/Usuario/ObterUsuariosPorFuncaoSiteEProcesso?idProcesso=${idProcesso}&idSite=${idSite}&idFuncao=${idFuncao}`, (result) => {
            if (result.StatusCode == 200) {
                var comboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"] option');
                var idComboResponsavel = $(_this).closest('#gestaoDeRisco').find('[name="formGestaoDeRiscoResponsavelDefinicao"]');
                APP.component.SelectListCompare.selectList(result.Lista, comboResponsavel, idComboResponsavel, 'IdUsuario', 'NmCompleto');
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

    bindTemas : function () {
        
        APP.controller.AnaliseCriticaController.editTemas();
        APP.controller.AnaliseCriticaController.setTemaBox();
        APP.controller.AnaliseCriticaController.delTemaCombobox();
        APP.controller.AnaliseCriticaController.editTemaExistenteBox();
        APP.controller.AnaliseCriticaController.setValidateForms();

    },

    getObjObjFormCriarAnaliseCriticaValidate : function () {

        var acoesPadraoFormCriarPadraoObj = {
            //formCriaClienteLogo: {required: true, minFiles: 1},
            //Analise Critica
            formCriarAnaliseCriticaResponsavel : {required: true},
            formCriarAnaliseCriticaAta : {required: true},
            formCriarAnaliseCriticaDtCriacao : {required: true},
            formCriarAnaliseCriticaDtProximaAnalise : {required: true},
            //Analise Critica - Pessoa Funcao
            formCriarAnaliseCriticaParticipante : {required: true},
            formCriarAnaliseCriticaCargo : {required: true},
            //Analise Critica - Gestao de Risco
            //formCriarAnaliseCriticaTema : {required: true},
            //formGestaoDeRiscoDescricao : {required: true},
            formGestaoDeRiscoCriticidade : {required: true},
            //formGestaoDeRiscoRisco : {required: true},
            //formGestaoDeRiscoResponsavelDefinicao : {required: true},
            //formGestaoDeRiscoNumero : {required: true},
            //formGestaoDeRiscoIdentificacao : {required: true},

        };

        return acoesPadraoFormCriarPadraoObj;

    },

    getObjFormCriarAnaliseCritica : function () {

        var acoesPadraoFormCriarPadraoObj = {
           
            IdAnaliseCritica: $('[name=IdAnaliseCritica]').val(),
            IdSite: $('[name=IdSite]').val(),
            IdResponsavel: $('[name=formCriarAnaliseCriticaResponsavel] :selected').val(),
            Ata:$('[name=formCriarAnaliseCriticaAta]').val(),
            DataCriacao:$('[name=formCriarAnaliseCriticaDtCriacao]').val(),
            DataProximaAnalise:$('[name=formCriarAnaliseCriticaDtProximaAnalise]').val(),
            Ativo: true,
            Temas: APP.controller.AnaliseCriticaController.getObjFormTemasAnaliseCriticaArray(),
            Funcionarios: APP.controller.AnaliseCriticaController.getObjFormPessoaFuncaoAnaliseCriticaArray(),
        };
        
        return acoesPadraoFormCriarPadraoObj;

    },

    getObjFormPessoaFuncaoAnaliseCriticaArray : function () {
        
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

    getObjFormTemasAnaliseCriticaArray : function () {
        
        var arrayFormTemasAnaliseCriticaObj = [];

        $('#panel-lista-temas .li-tema-AC').each(function () {

            var idCkEditor = $(this).find('[name=formGestaoDeRiscoDescricao]').attr('id');

            var formCriarTemasAnaliseCriticaObj = {
                IdTema: $(this).find('[name=IdTema]').val(),
                Descricao: CKEDITOR.instances[idCkEditor].getData(),
                CorRisco : $(this).find("[name^=formGestaoDeRiscoCriticidade] :selected").val(),
                PossuiGestaoRisco: $(this).find('[name^=formGestaoDeRiscoRisco]:checked').val(),
                IdControladorCategoria: $(this).find('[name=idControladorCategoria]').val(),
                IdGestaoDeRisco: $(this).find('[name=IdGestaoDeRisco]').val(),
                GestaoDeRisco:
                {
                    CriticidadeGestaoDeRisco: $(this).find("[name^=formGestaoDeRiscoCriticidade] :selected").val(),
                    //TipoRegistro: 'gr',
                    IdResponsavelInicarAcaoImediata: $(this).find('[name=formGestaoDeRiscoResponsavelDefinicao]').val(),
                    DescricaoRegistro: $(this).find('[name=formGestaoDeRiscoIdentificacao]').val(),
                    //IdSite: $('[name=IdSite]').val(),
                }
            };
            
            arrayFormTemasAnaliseCriticaObj.push(formCriarTemasAnaliseCriticaObj);

        });

        return arrayFormTemasAnaliseCriticaObj;
        
    },

    //Funcoes Edit Analise Critica
    getTemasDescricao: function () {

        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {
            
            var idDescricao = $(element).attr('id');
            CKEDITOR.replace(idDescricao);
        });

    },
    
    getTemasCores: function () {

        var divBarRating = $('.barraRating');
        APP.component.BarRating.setBarRatingGestaoDeRisco(divBarRating, 'bars-1to10');

        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {

            var divContext = $(element).closest('[name=GestaoDeRisco]');

            var corRisco = divContext.find('[name=formGestaoDeRiscoCriticidadeCor]').val();
            var lastCores = divContext.find("[data-rating-value='" + corRisco + "']").last().index();
            for (i = 0; i <= lastCores; i++) {
                $(divContext.find('.br-theme-bars-1to10').find('.br-widget a')[i]).addClass('br-selected');
            }
            $(divContext.find('.br-theme-bars-1to10').find('.br-widget a')[lastCores]).trigger("click");
            divContext.find('.br-theme-bars-1to10').find("[data-rating-value='" + corRisco + "']").addClass('br-current');
            $('.br-widget').addClass('barRating-disabled');
        });
    },

    getTemasPossuiGestaoDeRisco: function () {
        $('[name=formGestaoDeRiscoDescricao]').each(function (index, element) {
            var divContext = $(element).closest('[name=GestaoDeRisco]');
            var temGestaoDeRisco = divContext.find('[name=formGestaoDeRiscoRisco]').val();
            divContext.find(`[name=formGestaoDeRiscoResponsavelDefinicao][value=${temGestaoDeRisco}]`).prop('checked', temGestaoDeRisco);
        });
    },

    
    setPossuiGestaoDeRisco : function () {
        
        $('[name^=formGestaoDeRiscoRisco]').unbind('change');
        $('[name^=formGestaoDeRiscoRisco]').on('change', function () {
            
            var possuiGestaoDeRisco = APP.controller.AnaliseCriticaController.getPossuiGestaoDeRisco();
            APP.controller.AnaliseCriticaController.setRulesPossuiGestaoDeRisco(possuiGestaoDeRisco);
            
        });
        
    },
    
    getPossuiGestaoDeRisco : function () {
        
        var possuiGestaoDeRisco = APP.component.Radio.init('formGestaoDeRiscoRisco');
        return possuiGestaoDeRisco;
        
    },
    
    setRulesPossuiGestaoDeRisco : function (_possuiGestaoDeRisco) {
        
        if(_possuiGestaoDeRisco == "true") {
            $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").show();
            $('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").show();
            $('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").show();
        } else {
            $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest("[class^=col]").hide();
            $('[name=formGestaoDeRiscoNumero]').closest("[class^=col]").hide();
            $('[name=formGestaoDeRiscoIdentificacao]').closest("[class^=col]").hide();
        }
        
    },
    
    setHidePossuiGestaoDeRisco : function () {
        
       $('[name^=formGestaoDeRiscoRisco]').trigger('change');

    },
    
    //Todos
    sendFormCriarAnaliseCritica : function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {
            
            var validate = APP.controller.AnaliseCriticaController.validateForms();

            //var validate = true;
            if (validate == true){
                var padraoObj = APP.controller.AnaliseCriticaController.getCriarAnaliseCriticaObj();
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

    getCriarAnaliseCriticaObj : function () {

        var analiseCriticaObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');
            
            if (isVisible) {
                var form = idPanel[2];
                switch (form) {
                    case "analisecritica":
                        analiseCriticaObj = APP.controller.AnaliseCriticaController.getObjFormCriarAnaliseCritica();
                        APP.controller.AnaliseCriticaController.saveFormCriarAnaliseCritica(analiseCriticaObj, '/AnaliseCritica/Salvar');
                        break;
                }
            }

            });


    },

    saveFormCriarAnaliseCritica : function (analiseCriticaObj,url) {

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

};