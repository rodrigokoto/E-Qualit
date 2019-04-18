
/*
|--------------------------------------------------------------------------
| Fornecedores
|--------------------------------------------------------------------------
*/
APP.controller.FornecedoresController = {

    init: function () {

        var page = APP.component.Util.getPage();
        
        this.setup();
        if (page == 'IndexProdutos') {
            this.indexProdutos();
        }
        if (page == 'AcoesProdutos') {
            var _idProduto = $('[name=IdProduto]').val();
            if (_idProduto == '') {
                this.acoesProdutos();
            } else {
                this.editAcoesProdutos();
            }
        }
        if (page == 'IndexFornecedores') {
            this.indexFornecedores();
        }
        if (page == 'AcoesFornecedores') {
            this.acoesFornecedores();
        }

    },

    setup: function () {

        //Produtos - Acoes
        this.buttonSalvar = $('.btn-salvar');

        //Produtos - Criticidade
        this.buttonAddNovoCriterioAvaliacaoCriticidadeFormProdutos = $('.btn-add-produtos-criticidade');
        this.buttonEditNovoCriterioAvaliacaoCriticidadeFormProdutos = $('.btn-edit-produtos-criticidade');
        this.buttonSaveNovoCriterioAvaliacaoCriticidadeFormProdutos = $('.btn-confirm-produtos-criticidade');
        this.buttonDelNovoCriterioAvaliacaoCriticidadeFormProdutos = $('.btn-del-produtos-criticidade');

        //Produtos - Qualificacao Fornecedor
        this.buttonAddNovoCriterioFormQualificacaoFornecedor = $('.btn-add-qualificacao-fornecedor');
        this.buttonEditNovoCriterioFormQualificacaoFornecedor = $('.btn-edit-qualificacao-fornecedor');
        this.buttonSaveNovoCriterioFormQualificacaoFornecedor = $('.btn-confirm-qualificacao-fornecedor');
        this.buttonDelNovoCriterioFormQualificacaoFornecedor = $('.btn-del-qualificacao-fornecedor');
        this.radioFormQualificacaoFornecedorAtivo = $('[name^=formQualificaFornecedorAtivo]');
        this.radioFormQualificacaoFornecedorVencimento = $('[name^=formQualificaFornecedorVencimento');

        //Produtos - Criterios Avaliacao
        this.buttonAddNovoCriterioAvaliacaoFormCriteriosAvaliacao = $('.btn-add-criterios-avaliacao');
        this.buttonEditNovoCriterioAvaliacaoFormCriteriosAvaliacao = $('.btn-edit-criterios-avaliacao');
        this.buttonSaveNovoCriterioAvaliacaoFormCriteriosAvaliacao = $('.btn-confirm-criterios-avaliacao');
        this.buttonDelNovoCriterioAvaliacaoFormCriteriosAvaliacao = $('.btn-del-criterios-avaliacao');

    },

    models: {
        AnexoModel: APP.model.Anexo,
    },

    delProdutoFornecedor: function () {

        $(".excluirProduto").on('click', function (event) {

            event.preventDefault();

            var idProduto = $(this).attr('IdProduto');
            console.log("idProduto:= ", idProduto);

            bootbox.confirm(_options.DesejaRealmenteExcluirEsseRegistro, function (result) {
                if (result) {
                    APP.component.Loading.showLoading();
                    $.get('/Fornecedor/ExcluirProduto?idProduto=' + idProduto, function (data, status) {
                    }).done(function (data) {
                        if (data.StatusCode == "200") {
                            bootbox.alert(data.Success, function (result) {
                                APP.component.Loading.hideLoading();
                                window.location.href = "/Fornecedor/Index";
                            });

                        }
                    });
                }
            });
        });


    },


    delFornecedor: function () {

        $(".excluirFornecedor").on('click', function (event) {

            event.preventDefault();

            var idFornecedor = $(this).attr('idFornecedor');
            var idProduto = $("#hdIdProduto").val();
            console.log("idFornecedor:= ", idFornecedor);

            bootbox.confirm(_options.DesejaRealmenteExcluirEsseRegistro, function (result) {
                if (result) {
                    $.get('/Fornecedor/Excluir?idFornecedor=' + idFornecedor, function (data, status) {
                    }).done(function (data) {
                        if (data.StatusCode == "200") {
                            bootbox.alert(data.Success, function (result) {
                                window.location.href = "/Fornecedor/IndexFornecedores?idProduto=" + idProduto;
                            });

                        }
                    });
                }
            });
        });


    },


    //Index Produtos
    indexProdutos: function () {
        APP.controller.FornecedoresController.delProdutoFornecedor();
        APP.component.DataTable.init('#tb-index-produtos');
    },

    //AÃ§oes Produtos
    acoesProdutos: function () {

        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        this.validateFormsProdutos();
        this.setHideAndShow();
        this.formProdutos();
        this.formCriticidade();
        this.formQualificacaoFornecedor();
        this.formCriterioAvaliacao();

        this.sendFormAcoesProdutos();

    },

    setHideAndShow: function () {

        var critico = false;
        $("input:checked").length;
        var criticos = $('[name^=formProdutosCriticidadeAtivo]:checked').length;

        if (criticos >= 1) {
            critico = true;
        }

        var Ancora = $("#Ancora").val();

        if (Ancora == "" || Ancora == null || Ancora == undefined)
        {
            if ($("#form-produtos-produto").val() != "" && $("#form-produto-responsavel").val() != "") {
                if (($("#form-criticidade").attr("style") == "display: block;" || $("#form-criticidade").attr("style") == undefined) && critico == true) {
                    $('#form-qualifica-fornecedor').show();
                    $('#form-criterios-avaliacao').show();
                    $('#form-parametros-critérios-avaliacao').show();
                }
                else {
                    $('#form-qualifica-fornecedor').hide();
                    $('#form-criterios-avaliacao').hide();
                    $('#form-parametros-critérios-avaliacao').hide();
                }

                $('#form-criticidade').show();                
            }
            else {
                $('#form-criticidade').hide();
                $('#form-qualifica-fornecedor').hide();
                $('#form-criterios-avaliacao').hide();
                $('#form-parametros-critérios-avaliacao').hide();
            }
        }



        //
        //$('#form-criterios-avaliacao').toggle();

    },

    sendFormAcoesProdutos: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {

            var validate = [];
            var ValidateOK = true;

            validate = APP.controller.FornecedoresController.validateForms();

            for (var i = 0; i < validate.length; i++) {
                if (validate[i].valid != true) {
                    ValidateOK = false;
                }
            }

            //var ValidateOK = true;
            if (ValidateOK == true) {
                var produto = APP.controller.FornecedoresController.getAcoesProdutosObj();
                if ($('[name=IdProduto]').val() > 0) {
                    produto.IdProduto = $('[name=IdProduto]').val();
                    APP.controller.FornecedoresController.saveFilhosProduto(produto);
                } else {
                    APP.controller.FornecedoresController.saveFormAcoesProdutos(produto);
                }

            }

        });

    },

    getAcoesProdutosObj: function () {

        var produtoObj = {};

        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            var form = idPanel[2];
            switch (form) {
                case 'protudos':
                    produtoObj = APP.controller.FornecedoresController.getObjFormProdutos();
                    break;
                case 'criticidade':
                    produtoObj.AvaliacoesCriticidade = APP.controller.FornecedoresController.getObjFormCriticidade();
                    break;
                case 'qualificacaofornecedor':
                    produtoObj.CriteriosQualificacao = APP.controller.FornecedoresController.getObjFormQualificacaoFornecedor();
                    break;
                case 'criteriosavaliacao':
                    produtoObj.CriteriosAvaliacao = APP.controller.FornecedoresController.getObjFormCriterioAvaliacao();
                    break;
            }

        });

        return produtoObj;

    },

    validateFormsProdutos: function () {

        var ObjFormProdutosValidate = APP.controller.FornecedoresController.getObjFormProdutosValidate();
        APP.component.ValidateForm.init(ObjFormProdutosValidate, '#form-fornecedores-produtos');

        
        $("input:checked").length;
        var criticos = $('[name^=formProdutosCriticidadeAtivo]:checked').length;

        if (criticos >= 1) {
            var ObjFormProdutosParametrosValidate = APP.controller.FornecedoresController.getObjFormProdutosParametrosValidate();
            APP.component.ValidateForm.init(ObjFormProdutosParametrosValidate, '#form-parametros-critérios-avaliacao');
        }

    },

    getAvaliacaoCriticidadePadrao: function () {

        $.ajax({
            type: 'GET',
            dataType: 'json',
            url: '/AvaliacaoCriticidade/ListaCriteriosPadroes/',
            beforeSend: function () { },
            success: function (result) {
                if (result.StatusCode == 200) {

                    if($('#tb-form-produtos-criterios tbody tr').length == 0)
                    {
                        $.each(result.AvaliacoesCriticidadePadrao, function (key, value) {

                            var trElements = $('<tr>');
                            var tdformProdutosCriticidadeCriterio = $('<td><textarea name="formProdutosCriticidadeCriterio" id="form-produtos-criticidade-criterio" class="form-control" placeholder="Critério" title="Critério" rows="3" >' + value.Titulo + '</textarea></td>');
                            var tdCheck = $('<td><div class="checkbox"><input type="checkbox" name="formProdutosCriticidadeAtivo-' + key + '" id="form-produtos-criticidade-ativo-' + key + '" onclick="chkValue(this);"><label for="form-produtos-criticidade-ativo-' + key + '" style="padding-left: 25px !important;">Crítico</label></div></td>');
                            var tdbtns = $('<td><button type="button" class="btn btn-edit-produtos-criticidade editar-color"><i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i><div class="tooltip fade top in" style="top: 22px; left: 1148.08px; display: block;"><div class="tooltip-arrow"></div></div></button><button type="button" class="btn btn-confirm-produtos-criticidade ativo-color"><i class="fa fa-check " aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i></button><button type="button" class="btn btn-del-produtos-criticidade trash-color"><i class="fa fa-trash " aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i></button></td>');

                            trElements.append(tdformProdutosCriticidadeCriterio).append(tdCheck).append(tdbtns);

                            $('#tb-form-produtos-criterios tbody').append(trElements);
                        });
                    }
                }
            },
            error: function (result) {

            },
            complete: function (result) {

            }
        });

    },


    getCriterioQualificacaoFornecedorPadrao: function () {

        $.ajax({
            type: 'GET',
            dataType: 'json',
            url: '/CriterioQualificacao/ListaCriteriosPadroes/',
            beforeSend: function () { },
            success: function (result) {
                if (result.StatusCode == 200) {
                    $.each(result.CriterioQualificacaoPadrao, function (key, value) {

                        var trElements = $('<tr>');
                        var tdformProdutosCriterioQualificacao = $('<td><textarea name="formQualificaFornecedorCriterio" id="form-qualifica-fornecedor-criterio" class="form-control" placeholder="Critério" title="Critério" rows="3" >' + value.Titulo + '</textarea></td>');
                        var tdCheck = $('<td><div class="checkbox"><input type="checkbox" name="formQualificaFornecedorAtivo-' + key + '" id="form-qualifica-fornecedor-ativo-' + key + '" onclick="chkValue(this);"><label for="form-qualifica-fornecedor-ativo-' + key + '" style="padding-left: 25px !important;">Ativo</label></div></td>');
                        var tdbtns = $('<td><button type="button" class="btn btn-edit-produtos-qualificacao-fornecedor editar-color"><i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i><div class="tooltip fade top in" style="top: 22px; left: 1148.08px; display: block;"><div class="tooltip-arrow"></div></div></button><button type="button" class="btn btn-confirm-produtos-qualificacao-fornecedor ativo-color"><i class="fa fa-check " aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i></button><button type="button" class="btn btn-del-produtos-qualidade-fornecedor trash-color"><i class="fa fa-trash " aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i></button></td>');

                        trElements.append(tdformProdutosCriterioQualificacao).append(tdCheck).append(tdbtns);

                        $('#tb-form-qualificacao-fornecedor tbody').append(trElements);
                    });
                }
            },
            error: function (result) {

            },
            complete: function (result) {

            }
        });

    },

    getCriterioAvaliacaoPadrao: function () {

        $.ajax({
            type: 'GET',
            dataType: 'json',
            url: '/CriterioAvaliacao/ListaCriteriosPadroes/',
            beforeSend: function () { },
            success: function (result) {
                if (result.StatusCode == 200) {
                    $.each(result.CriterioAvaliacaoPadrao, function (key, value) {

                        var trElements = $('<tr>');
                        var tdformProdutosCriterioAvaliacao = $('<td><textarea name="formCriteriosAvaliacaoDisponibilidade" id="form-criterios-avaliacao-disponibilidade" class="form-control" placeholder="Critério" title="Critério" rows="3" >' + value.Titulo + '</textarea></td>');
                        var tdCheck = $('<td><div class="checkbox"><input type="checkbox" name="formCriteriosAvaliacaoAtivo-' + key + '" id="form-criterios-avaliacao-ativo-' + key + '" onclick="chkValue(this);"><label for="form-criterios-avaliacao-ativo-' + key + '" style="padding-left: 25px !important;">Ativo</label></div></td>');
                        var tdbtns = $('<td><button type="button" class="btn btn-edit-produtos-criterios-avaliacao editar-color"><i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i><div class="tooltip fade top in" style="top: 22px; left: 1148.08px; display: block;"><div class="tooltip-arrow"></div></div></button><button type="button" class="btn btn-confirm-produtos-criterios-avaliacao ativo-color"><i class="fa fa-check " aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i></button><button type="button" class="btn btn-del-produtos-criterios-avaliacao trash-color"><i class="fa fa-trash " aria-hidden="true" data-toggle="tooltip" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i></button></td>');

                        trElements.append(tdformProdutosCriterioAvaliacao).append(tdCheck).append(tdbtns);

                        $('#tb-form-criterios-avaliacao tbody').append(trElements);
                    });
                }
            },
            error: function (result) {

            },
            complete: function (result) {

            }
        });

    },

    saveFilhosProduto: function (produto) {

        $.ajax({
            type: 'POST',
            data: produto,
            dataType: 'json',
            url: '/Fornecedor/SalvaAvaliacoesCriticidadeCriteriosQualificaoCriterioAvaliacao/',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {

                    var Ancora = $("#Ancora").val();

                    if ($("#form-criterios-avaliacao").attr("style") == "display: block;") {

                        window.location.href = "/Fornecedor/Produtos";
                    }
                    else if (Ancora != null && Ancora != undefined && Ancora != "") {
                        window.location.href = "/Fornecedor/Produtos";
                    }
                    else {
                        $('#form-criterios-avaliacao').show();
                        $('#form-parametros-critérios-avaliacao').show();
                        $('#form-qualifica-fornecedor').show();
                    }
                }

                if (result.StatusCode == 400) {
                    var texto = "";

                    $(result.Erros).each(function (item) {
                        texto = texto + item;
                    });
                    console.log(result.Erros);
                    bootbox.alert(texto);
                }
            },
            error: function (result) {

                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

    saveFormAcoesProdutos: function (produto) {

        $.ajax({
            type: 'POST',
            data: produto,
            dataType: 'json',
            url: '/Fornecedor/SalvaProdutos/',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    $('[name=IdProduto]').val(result.IdProduto);
                    APP.controller.FornecedoresController.setHideAndShow();
                    //APP.controller.FornecedoresController.getAvaliacaoCriticidadePadrao();
                    //APP.controller.FornecedoresController.getCriterioQualificacaoFornecedorPadrao();
                    //APP.controller.FornecedoresController.getCriterioAvaliacaoPadrao();
                }

                if (result.StatusCode == 400) {
                    // var texto = "";

                    // $(result.Erros).each(function (item) {
                    //     texto = texto + item;
                    // });
                    console.log(result.Erros);
                    //bootbox.alert(texto);
                }
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

    //Form PRODUTO
    formProdutos: function () {

        this.setHideAndShowProdutos();
        this.setComboResponsavelProdutos();

    },

    setHideAndShowProdutos: function () {

        //

    },

    setComboResponsavelProdutos: function () {

        var idSite = $('#fornecedores-site').val();
        var idFuncao = $('#fornecedores-funcao').val();
        var data = {
            'idSite': idSite,
            'idFuncao': idFuncao
        };
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            data: data,
            url: '/Usuario/ObterUsuariosPorFuncao',
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    APP.component.SelectListCompare.init(result.Lista, $('[name=formProdutosResponsavel] option'), '#form-produto-responsavel', 'IdUsuario', 'NmCompleto');
                }
            },
            error: function (result) {
                //bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {

            }
        });

    },

    getObjFormProdutosValidate: function () {

        var acoesProdutosFormProdutosObj = {
            formProdutosProduto: 'required',
            //formProdutosCodigo: 'required',
            formProdutosResponsavel: 'required',
            //formProdutosEspecificacao: 'required',
        };
        return acoesProdutosFormProdutosObj;
    },

    getObjFormProdutosParametrosValidate: function () {

        var acoesProdutosFormProdutosObj = {

            MinAprovado: 'required',
            MaxAprovado: 'required',
            MinAprovadoAnalise: 'required',
            MaxAprovadoAnalise: 'required',
            MinReprovado: 'required',
            MaxReprovado: 'required',

        };
        return acoesProdutosFormProdutosObj;
    },

    getObjFormProdutos: function () {

        var idSite = $('#fornecedores-site').val();

        var acoesProdutosFormProdutosObj = {
            IdSite: idSite,
            Nome: $('[name=formProdutosProduto]').val(),
            Tags: $('[name=formProdutosCodigo]').val(),
            IdResponsavel: $('[name=formProdutosResponsavel] :selected').val(),
            Especificacao: $('[name=formProdutosEspecificacao]').val(),

            MinAprovado: $('[name=MinAprovado]').val(),
            MaxAprovado: $('[name=MaxAprovado]').val(),

            MinAprovadoAnalise: $('[name=MinAprovadoAnalise]').val(),
            MaxAprovadoAnalise: $('[name=MaxAprovadoAnalise]').val(),

            MinReprovado: $('[name=MinReprovado]').val(),
            MaxReprovado: $('[name=MaxReprovado]').val()
            
        };

        return acoesProdutosFormProdutosObj;

    },

    //Form CRITICIDADE
    formCriticidade: function () {

        this.setHideAndShowCriticidade();
        this.setNovoCriterioAvaliacaoCriticidadeFormCriticidade();
        this.setEditNovoCriterioAvaliacaoCriticidadeFormCriticidade();
        this.setSaveNovoCriterioAvaliacaoCriticidadeFormCriticidade();
        this.delNovoCriterioAvaliacaoCriticidadeFormCriticidade();

    },

    setHideAndShowCriticidade: function () {

        //

    },

    setNovoCriterioAvaliacaoCriticidadeFormCriticidade: function () {

        this.buttonAddNovoCriterioAvaliacaoCriticidadeFormProdutos.unbind('click');
        this.buttonAddNovoCriterioAvaliacaoCriticidadeFormProdutos.on('click', function (event) {
            event.preventDefault();

            var index = $('#tb-form-produtos-criterios tbody tr').size() + 1;

            var html = '';
            html += '<tr>';
            html += '<td>';
            html += '<textarea  name="formProdutosCriticidadeCriterio" id="form-produtos-criticidade-criterio" class="form-control" placeholder="Critério" rows="3" ></textarea>';
            html += '</td>';
            html += '<td>';
            html += '<div class="checkbox">';
            html += '<input type="checkbox" name="formProdutosCriticidadeAtivo-' + index + '" id="form-produtos-criticidade-ativo-' + index + '"  onclick="chkValue(this);">';
            html += '<label for="form-produtos-criticidade-ativo-' + index + '" style="padding-left: 25px !important;">';
            html += 'Crítico';
            html += '</label>';
            html += '<span class="field-validation-valid" data-valmsg-replace="true"></span>';
            html += '</div>';
            html += '</td>';
            html += '<td>';
            //html += '<button type="button" class="btn btn-edit-produtos-criticidade editar-color">';
            //html += '<i class="fa fa-pencil" aria-hidden="true" data-toggle="tooltip"  title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            //html += '</button>';
            //html += '<button type="button" class="btn btn-confirm-produtos-criticidade ativo-color">';
            //html += '<i class="fa fa-check " aria-hidden="true" data-toggle="tooltip"  title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i>';
            //html += '</button>';
            html += '<button type="button" class="btn btn-del-produtos-criticidade trash-color">';
            html += '<i class="fa fa-trash " aria-hidden="true" data-toggle="tooltip"  title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</button>';
            html += '</td>';
            html += '</tr>';


            $('#tb-form-produtos-criterios tbody').append(html);

            APP.controller.FornecedoresController.setup();
            APP.controller.FornecedoresController.bindFormCriticidade();

        });

    },

    setEditNovoCriterioAvaliacaoCriticidadeFormCriticidade: function () {

        this.buttonEditNovoCriterioAvaliacaoCriticidadeFormProdutos.unbind('click');
        this.buttonEditNovoCriterioAvaliacaoCriticidadeFormProdutos.on('click', function (event) {
            event.preventDefault();

            $(this).closest('tr').find('[name^=formProdutosCriticidadeCriterio]').prop('disabled', false);
            $(this).closest('tr').find('[name^=formProdutosCriticidadeAtivo]').prop('disabled', false);
            $(this).closest('tr').find('[name^=formProdutosCriticidadeCritico]').prop('disabled', false);

        });

    },

    setSaveNovoCriterioAvaliacaoCriticidadeFormCriticidade: function () {

        this.buttonSaveNovoCriterioAvaliacaoCriticidadeFormProdutos.unbind('click');
        this.buttonSaveNovoCriterioAvaliacaoCriticidadeFormProdutos.on('click', function (event) {
            event.preventDefault();

            var _this = $(this);

            var id = $(this).attr("item");
            var idText = "#" + $(this).attr("IdText");
            var valor = $(idText).val();
            var jsonUpdate = { id: id, valor: valor };
            $.ajax({
                async: true,
                type: 'POST',
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(jsonUpdate),
                url: '/Fornecedor/SalvarAvaliacaoCriticidade/',
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {

                    _this.closest('tr').find('[name^=formProdutosCriticidadeCriterio]').prop('disabled', true);
                    _this.closest('tr').find('[name^=formProdutosCriticidadeAtivo]').prop('disabled', true);
                    _this.closest('tr').find('[name^=formProdutosCriticidadeCritico]').prop('disabled', true);
                    
                },
                error: function (result) {
                    APP.component.Loading.hideLoading();
                },
                complete: function (result) {
                    APP.component.Loading.hideLoading();
                }
            });

        });

    },

    delNovoCriterioAvaliacaoCriticidadeFormCriticidade: function () {

        this.buttonDelNovoCriterioAvaliacaoCriticidadeFormProdutos.unbind('click');
        this.buttonDelNovoCriterioAvaliacaoCriticidadeFormProdutos.on('click', function (event) {

            event.preventDefault();

            var id = $(this).attr("item");

            var btnExcluirCriterio = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (resultConfirm) {
                if (resultConfirm == true) {

                    $.ajax({
                        async: true,
                        type: 'GET',
                        contentType: "application/json",
                        dataType: 'json',
                        url: '/Fornecedor/ExcluirAvaliacoesCriticidade/?id=' + id,
                        beforeSend: function () {
                            APP.component.Loading.showLoading();
                        },
                        success: function (result) {

                            APP.component.Loading.hideLoading();

                            if (result.StatusCode == 200) {
                                bootbox.alert(result.Success, function (retorno) {
                                    btnExcluirCriterio.remove()
                                });                                
                            }
                            else {

                                btnExcluirCriterio.remove();
                                console.log(result.Erros[0]);
                                bootbox.alert(result.Erros[0]);
                            }

                            
                            
                        },
                        error: function (result) {
                            APP.component.Loading.hideLoading();
                            btnExcluirCriterio.remove();
                        },
                        complete: function (result) {
                            
                        }
                    });
                }
            });
        });
    },

    getObjFormCriticidade: function () {

        var table = $('#tb-form-produtos-criterios tbody');
        var arrayFormCriticidadeObj = [];
        var criticidade = {};

        table.find('tr').each(function () {
            var ativo = $(this).find('[name^=formProdutosCriticidadeAtivo]').is(':checked');

            ativo = ativo == undefined || ativo == null || ativo == "" ? "False" : ativo;

            criticidade = {
                IdAvaliacaoCriticidade: $(this).find('[name=IdAvaliacaoCriticidade]').val(),
                Titulo: $(this).find('[name=formProdutosCriticidadeCriterio]').val(),
                IdProduto: $('[name=IdProduto]').val(),
                Ativo: ativo
            };

            arrayFormCriticidadeObj.push(criticidade);
        });
        return arrayFormCriticidadeObj;

    },

    bindFormCriticidade: function () {

        APP.controller.FornecedoresController.setNovoCriterioAvaliacaoCriticidadeFormCriticidade();
        APP.controller.FornecedoresController.setEditNovoCriterioAvaliacaoCriticidadeFormCriticidade();
        APP.controller.FornecedoresController.setSaveNovoCriterioAvaliacaoCriticidadeFormCriticidade();
        APP.controller.FornecedoresController.delNovoCriterioAvaliacaoCriticidadeFormCriticidade();

    },

    //Form QUALIFICACAO FORNECEDOR
    formQualificacaoFornecedor: function () {

        this.setHideAndShowQualificacaoFornecedor();
        this.setNovoCriterioFormQualificacaoFornecedor();
        this.setEditNovoCriterioFormQualificacaoFornecedor();
        this.setSaveNovoCriterioFormQualificacaoFornecedor();
        this.delNovoCriterioFormQualificacaoFornecedor();
        this.getCheckboxAtivoFormQualificacaoFornecedor();
        this.getradioVencimentoFormQualificacaoFornecedor();

    },

    setHideAndShowQualificacaoFornecedor: function () {

        $('.form-qualifica-fornecedor-panel').hide();
        $('.form-qualifica-fornecedor-panel-dt-vencimento').hide();
        $('[name^=formQualificaFornecedorAtivo]').each(function () {
            var check = $(this).is(':checked');
            if (check == true) {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel').show();
            } else {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel').hide();
            }
        });

        $('.form-qualifica-fornecedor-panel').each(function () {
            var idFormQualificaFornecedorVencimento = $(this).find('[name^=formQualificaFornecedorVencimento]').attr('name');
            var check = APP.component.Radio.init(idFormQualificaFornecedorVencimento);
            if (check == 'true') {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel-dt-vencimento').show();
            } else {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel-dt-vencimento').hide();
            }
        });

    },

    setNovoCriterioFormQualificacaoFornecedor: function () {

        this.buttonAddNovoCriterioFormQualificacaoFornecedor.unbind('click');
        this.buttonAddNovoCriterioFormQualificacaoFornecedor.on('click', function (event) {
            event.preventDefault();

            var index = $('#tb-form-qualificacao-fornecedor tbody tr').size() + 1;

            var html = '';
            html += '';
            html += '<tr>';
            html += '<!-- Criterio -->';
            html += '<td>';
            html += '<textarea name="formQualificaFornecedorCriterio" id="form-qualifica-fornecedor-criterio" class="form-control" value="" placeholder=" '+_options.Criterio+'"  rows="3" ></textarea>';
            html += '</td>';
            html += '<!-- Ativa Item -->';
            html += '<td>';
            html += '<div class="checkbox">';
            html += '<input type="checkbox" name="formQualificaFornecedorAtivo-' + index + '" id="form-qualifica-fornecedor-ativo-' + index + '" onclick="chkValue(this);" value="true">';
            html += '<label for="form-qualifica-fornecedor-ativo-' + index + '" style="padding-left: 25px !important;">'+_options.labelButtonAtivar+'</label>';
            html += '</div>';
            html += '</td>';

            html += '<td>';
            html += '<div class="checkbox">';
            html += '<input type="checkbox" name="formQualificaFornecedorTemControleVencimento-' + index + '" id="form-qualifica-fornecedor-TemControleVencimento-' + index + '" onclick="chkValue(this);" value="true">';
            html += '<label for="form-qualifica-fornecedor-TemControleVencimento-' + index + '" style="padding-left: 25px !important;">' + _options.ControlaDataVencimento +'</label>';
            html += '</div>';
            html += '</td>';

            //html += '<!-- Vencimento -->';
            //html += '<td>';
            //html += '<div class="row form-qualifica-fornecedor-panel" style="margin-top: 5px;">';
            //html += '<label class="demo-list-radio mdl-radio mdl-js-radio mdl-js-ripple-effect" >';
            //html += '<input name="formQualificaFornecedorVencimento-' + index + '" id="form-qualifica-fornecedor-vencimento-sim" type="radio" value="true">';
            //html += ' Sim';
            //html += '</label>';
            //html += '<label class="demo-list-radio mdl-radio mdl-js-radio mdl-js-ripple-effect" >';
            //html += '<input type="radio" name="formQualificaFornecedorVencimento-' + index + '" id="form-qualifica-fornecedor-vencimento-nao" value="false">';
            //html += ' NÃ£o';
            //html += '</label>';
            //html += '</div>';
            //html += '</td>';
            //html += '<!-- Data Vencimento -->';
            //html += '<td>';
            //html += '<div class="input-group form-qualifica-fornecedor-panel-dt-vencimento date" id="datetimepicker2">';
            //html += '<input type="text" name="formQualificaFornecedorDtVencimento" id="form-qualifica-fornecedor-dt-vencimento" class="form-control data datepicker" data-msg-required="" value="" />';
            //html += '<span class="input-group-addon">';
            //html += '<i class="fa fa-calendar" aria-hidden="true"></i>';
            //html += '</span>';
            //html += '</div>';
            //html += '</td>';
            html += '<!-- Acoes -->';
            html += '<td>';
            //html += '<button type="button" class="btn btn-edit-qualificacao-fornecedor editar-color">';
            //html += '<i class="fa fa-pencil" aria-hidden="true" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            //html += '</button>';
            //html += '<button type="button" class="btn btn-confirm-qualificacao-fornecedor ativo-color">';
            //html += '<i class="fa fa-check " aria-hidden="true" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i>';
            //html += '</button>';
            html += '<button type="button" class="btn btn-del-qualificacao-fornecedor trash-color">';
            html += '<i class="fa fa-trash " aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</button>';
            html += '</td>';
            html += '</tr>';


            $('#tb-form-qualificacao-fornecedor tbody').append(html);

            APP.controller.FornecedoresController.setup();
            APP.controller.FornecedoresController.bindFormQualificacaoFornecedor();

        });

    },

    setEditNovoCriterioFormQualificacaoFornecedor: function () {

        this.buttonEditNovoCriterioFormQualificacaoFornecedor.unbind('click');
        this.buttonEditNovoCriterioFormQualificacaoFornecedor.on('click', function (event) {
            event.preventDefault();

            $(this).closest('tr').find('[name^=formQualificaFornecedorCriterio]').prop('disabled', false);
            $(this).closest('tr').find('[name^=formQualificaFornecedorAtivo]').prop('disabled', false);
            $(this).closest('tr').find('[name^=formQualificaFornecedorVencimento]').prop('disabled', false);
            $(this).closest('tr').find('[name^=formQualificaFornecedorTemControleVencimento]').prop('disabled', false);

        });

    },

    setSaveNovoCriterioFormQualificacaoFornecedor: function () {

        this.buttonSaveNovoCriterioFormQualificacaoFornecedor.unbind('click');
        this.buttonSaveNovoCriterioFormQualificacaoFornecedor.on('click', function (event) {
            event.preventDefault();

            var id = $(this).attr("item");
            var idText = "#" + $(this).attr("IdText");
            var valor = $(idText).val();
            var jsonUpdate = { id: id, valor: valor };
            var _this = $(this);
            $.ajax({
                async: true,
                type: 'POST',
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(jsonUpdate),
                url: '/Fornecedor/SalvarCriterioQualificacaoFornecedor/',
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {

                    _this.closest('tr').find('[name^=formQualificaFornecedorCriterio]').prop('disabled', true);
                    _this.closest('tr').find('[name^=formQualificaFornecedorAtivo]').prop('disabled', true);
                    _this.closest('tr').find('[name^=formQualificaFornecedorVencimento]').prop('disabled', true);
                    _this.closest('tr').find('[name^=formQualificaFornecedorTemControleVencimento]').prop('disabled', false);

                },
                error: function (result) {
                    APP.component.Loading.hideLoading();
                },
                complete: function (result) {
                    APP.component.Loading.hideLoading();
                }
            });

        });

    },

    delNovoCriterioFormQualificacaoFornecedor: function () {

        this.buttonDelNovoCriterioFormQualificacaoFornecedor.unbind('click');
        this.buttonDelNovoCriterioFormQualificacaoFornecedor.on('click', function (event) {
            event.preventDefault();

            var id = $(this).attr("item");

            var btnExcluirCriterio = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (resultConfirm) {
                if (resultConfirm == true) {

                    $.ajax({
                        async: true,
                        type: 'GET',
                        contentType: "application/json",
                        dataType: 'json',
                        url: '/Fornecedor/ExcluirCriterioFormQualificacao/?id=' + id,
                        beforeSend: function () {
                            APP.component.Loading.showLoading();
                        },
                        success: function (result) {

                            APP.component.Loading.hideLoading();

                            if (result.StatusCode == 200) {
                                bootbox.alert(result.Success, function (retorno) {
                                    btnExcluirCriterio.remove()
                                });                                
                            }
                            else {                               
                                btnExcluirCriterio.remove();
                                console.log(result.Erros[0]);
                                bootbox.alert(result.Erros[0]);
                            }
                        },
                        error: function (result) {
                            APP.component.Loading.hideLoading();
                            btnExcluirCriterio.remove();
                        },
                        complete: function (result) {
                            
                        }
                    });
                }
            });


        });

    },

    getCheckboxAtivoFormQualificacaoFornecedor: function () {

        this.radioFormQualificacaoFornecedorAtivo.unbind('click');
        this.radioFormQualificacaoFornecedorAtivo.on('click', function (event) {

            var checkboxAtivo = $(this).val();
            if (checkboxAtivo != 'False') {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel').show();
            } else {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel').hide();
            }

        });

    },
    
    getradioVencimentoFormQualificacaoFornecedor: function () {

        this.radioFormQualificacaoFornecedorVencimento.on('change', function (event) {

            var radioVencimento = APP.component.Radio.init($(this).attr('name'));
            if (radioVencimento == 'true') {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel-dt-vencimento').show();
            } else {
                $(this).closest('tr').find('.form-qualifica-fornecedor-panel-dt-vencimento').hide();
            }

        });

    },

    getObjFormQualificacaoFornecedor: function () {

        var table = $('#tb-form-qualificacao-fornecedor tbody');
        var arrayFormQualificacaoFornecedorObj = [];
        var qualificacaofornecedor = {};

        table.find('tr').each(function () {
            var ativo = $(this).find('[name^=formQualificaFornecedorAtivo]:checked').val();
            var TemControleVencimento = $(this).find('[name^=formQualificaFornecedorTemControleVencimento]:checked').val();

            ativo = ativo == undefined || ativo == null || ativo == "" ? "False" : ativo;

            qualificacaofornecedor = {
                IdCriterioQualificacao: $(this).find('[name=IdCriterioQualificacao]').val(),
                Titulo: $(this).find('[name=formQualificaFornecedorCriterio]').val(),
                TemControleVencimento: $(this).find('[name^=formQualificaFornecedorVencimento]:checked').val(),
                DtVencimento: $(this).find('[name=formQualificaFornecedorDtVencimento]').val(),
                IdProduto: $('[name=IdProduto]').val(),
                Ativo: ativo,
                TemControleVencimento: TemControleVencimento
            };
            arrayFormQualificacaoFornecedorObj.push(qualificacaofornecedor);
        });

        return arrayFormQualificacaoFornecedorObj;

    },

    bindFormQualificacaoFornecedor: function () {

        APP.controller.FornecedoresController.setNovoCriterioFormQualificacaoFornecedor();
        APP.controller.FornecedoresController.setEditNovoCriterioFormQualificacaoFornecedor();
        APP.controller.FornecedoresController.setSaveNovoCriterioFormQualificacaoFornecedor();
        APP.controller.FornecedoresController.delNovoCriterioFormQualificacaoFornecedor();
        APP.controller.FornecedoresController.getCheckboxAtivoFormQualificacaoFornecedor();
        APP.controller.FornecedoresController.getradioVencimentoFormQualificacaoFornecedor();
        APP.component.Datapicker.init();
        APP.controller.FornecedoresController.setHideAndShowQualificacaoFornecedor();

    },

    //Form CRITERIO AVALIACAO
    formCriterioAvaliacao: function () {

        this.setHideAndShowCriterioAvaliacao();
        this.setNovoCriterioAvaliacaoFormCriterioAvaliacao();
        this.setEditNovoCriterioAvaliacaoFormCriterioAvaliacao();
        this.setSaveNovoCriterioAvaliacaoFormCriterioAvaliacao();
        this.delNovoCriterioAvaliacaoFormCriterioAvaliacao();

    },

    setHideAndShowCriterioAvaliacao: function () {

        //

    },

    setNovoCriterioAvaliacaoFormCriterioAvaliacao: function () {
        this.buttonAddNovoCriterioAvaliacaoFormCriteriosAvaliacao.unbind('click');
        this.buttonAddNovoCriterioAvaliacaoFormCriteriosAvaliacao.on('click', function (event) {
            event.preventDefault();

            var index = $('#tb-form-criterios-avaliacao tbody tr').size() + 1;

            var html = '';
            html += '<tr>';
            html += '<!-- Disponibilidade -->';
            html += '<td>';
            html += '<textarea type="text" name="formCriteriosAvaliacaoDisponibilidade" id="form-criterios-avaliacao-disponibilidade" class="form-control" value="" placeholder="'+_options.Criterio+'" rows="3" ></textarea>';
            html += '</td>';
            html += '<!-- Ativo -->';
            html += '<td>';
            html += '<div class="checkbox">';
            html += '<input type="checkbox" name="formCriteriosAvaliacaoAtivo-' + index + '" id="form-criterios-avaliacao-ativo-' + index + '" onclick="chkValue(this);" value="true">';
            html += '<label for="form-criterios-avaliacao-ativo-' + index + '" style="padding-left: 25px !important;">Ativar</label>';
            html += '</div>';
            html += '</td>';
            html += '<!-- Acoes -->';
            html += '<td>';
            //html += '<button type="button" class="btn btn-edit-criterios-avaliacao editar-color">';
            //html += '<i class="fa fa-pencil" aria-hidden="true" title="' + _options.labelButtonEditar + '" data-original-title="' + _options.labelButtonEditar + '"></i>';
            //html += '</button>';
            //html += '<button type="button" class="btn btn-confirm-criterios-avaliacao ativo-color">';
            //html += '<i class="fa fa-check " aria-hidden="true" title="' + _options.labelButtonAtivar + '" data-original-title="' + _options.labelButtonAtivar + '"></i>';
            //html += '</button>';
            html += '<button type="button" class="btn btn-del-criterios-avaliacao trash-color">';
            html += '<i class="fa fa-trash " aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '"></i>';
            html += '</button>';
            html += '</td>';
            html += '</tr>';


            $('#tb-form-criterios-avaliacao tbody').append(html);

            APP.controller.FornecedoresController.setup();
            APP.controller.FornecedoresController.bindFormCriterioAvaliacao();

        });

    },

    setEditNovoCriterioAvaliacaoFormCriterioAvaliacao: function () {

        this.buttonEditNovoCriterioAvaliacaoFormCriteriosAvaliacao.unbind('click');
        this.buttonEditNovoCriterioAvaliacaoFormCriteriosAvaliacao.on('click', function (event) {
            event.preventDefault();

            $(this).closest('tr').find('[name^=formCriteriosAvaliacaoDisponibilidade]').prop('disabled', false);
            $(this).closest('tr').find('[name^=formCriteriosAvaliacaoAtivo]').prop('disabled', false);

        });

    },

    setSaveNovoCriterioAvaliacaoFormCriterioAvaliacao: function () {

        this.buttonSaveNovoCriterioAvaliacaoFormCriteriosAvaliacao.unbind('click');
        this.buttonSaveNovoCriterioAvaliacaoFormCriteriosAvaliacao.on('click', function (event) {
            event.preventDefault();

            var id = $(this).attr("item");
            var idText = "#" + $(this).attr("IdText");
            var valor = $(idText).val();
            var _this = $(this);
            var jsonUpdate = { id: id, valor: valor };
            $.ajax({
                async: true,
                type: 'POST',
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(jsonUpdate),
                url: '/Fornecedor/SalvarCriterioAvaliacao/',
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {

                    _this.closest('tr').find('[name^=formCriteriosAvaliacaoDisponibilidade]').prop('disabled', true);
                    _this.closest('tr').find('[name^=formCriteriosAvaliacaoAtivo]').prop('disabled', true);

                },
                error: function (result) {
                    APP.component.Loading.hideLoading();
                },
                complete: function (result) {
                    APP.component.Loading.hideLoading();
                }
            });

        });

    },

    delNovoCriterioAvaliacaoFormCriterioAvaliacao: function () {

        this.buttonDelNovoCriterioAvaliacaoFormCriteriosAvaliacao.unbind('click');
        this.buttonDelNovoCriterioAvaliacaoFormCriteriosAvaliacao.on('click', function (event) {
            event.preventDefault();

            var id = $(this).attr("item");

            var btnExcluirCriterio = $(this).closest('tr');

            bootbox.confirm(_options.MsgDesejaExcluirRegistro, function (resultConfirm) {
                if (resultConfirm == true) {

                    $.ajax({
                        async: true,
                        type: 'GET',
                        contentType: "application/json",
                        dataType: 'json',
                        url: '/Fornecedor/ExcluirCriterioFormAvaliacao/?id=' + id,
                        beforeSend: function () {
                            APP.component.Loading.showLoading();
                        },
                        success: function (result) {

                            APP.component.Loading.hideLoading();

                            if (result.StatusCode == 200) {
                                bootbox.alert(result.Success, function (retorno) {
                                    btnExcluirCriterio.remove();
                                });                                
                            }
                            else {

                                btnExcluirCriterio.remove();
                                console.log(result.Erros[0]);
                                bootbox.alert(result.Erros[0]);
                            }                            
                        },
                        error: function (result) {
                            APP.component.Loading.hideLoading();
                            btnExcluirCriterio.remove();
                        },
                        complete: function (result) {
                           
                        }
                    });
                }
            });


        });

    },

    getObjFormCriterioAvaliacao: function () {

        var table = $('#tb-form-criterios-avaliacao tbody');
        var arrayFormCriterioAvaliacaoObj = [];
        var criterioAvaliacao = {};

        table.find('tr').each(function () {
            var ativo = $(this).find('[name^=formCriteriosAvaliacaoAtivo]:checked').val();

            ativo = ativo == undefined || ativo == null || ativo == "" ? "False" : ativo;

            criterioAvaliacao = {
                IdCriterioAvaliacao: $(this).find('[name=IdCriterioAvaliacao]').val(),
                Titulo: $(this).find('[name=formCriteriosAvaliacaoDisponibilidade]').val(),
                IdProduto: $('[name=IdProduto]').val(),
                Ativo: ativo
            };

            arrayFormCriterioAvaliacaoObj.push(criterioAvaliacao);

        });

        return arrayFormCriterioAvaliacaoObj;

    },

    bindFormCriterioAvaliacao: function () {

        APP.controller.FornecedoresController.setNovoCriterioAvaliacaoFormCriterioAvaliacao();
        APP.controller.FornecedoresController.setEditNovoCriterioAvaliacaoFormCriterioAvaliacao();
        APP.controller.FornecedoresController.setSaveNovoCriterioAvaliacaoFormCriterioAvaliacao();
        APP.controller.FornecedoresController.delNovoCriterioAvaliacaoFormCriterioAvaliacao();

    },

    //Edit Acoes Produtos
    editAcoesProdutos: function () {

        this.setHideAndShow();
        this.validateFormsProdutos();
        this.formProdutos();

        this.formCriticidade();
        this.formQualificacaoFornecedor();
        this.formCriterioAvaliacao();

        this.sendFormAcoesProdutos();

        //APP.controller.FornecedoresController.getCriterioQualificacaoFornecedorPadrao();
        //APP.controller.FornecedoresController.getCriterioAvaliacaoPadrao();
    },

    setHideAndShowEdit: function () {

        //

    },

    //Index Fornecedores
    indexFornecedores: function () {

        APP.controller.FornecedoresController.delFornecedor();
        APP.component.DataTable.init('#tb-index-fornecedores');

    },

    //AÃ§oes Fornecedores
    acoesFornecedores: function () {
        APP.component.AtivaLobiPanel.init();
        APP.component.Datapicker.init();
        APP.component.Mascaras.init();
        APP.component.FileUpload.init();

        this.setValidateFormsFornecedores();
        if ($('[name=IdFornecedor]').val() > 0) {
            $('.input-nome-fornecedor').val($('[name=formFornecedoresCadastroNome]').val());
            this.formQualificacaoFornecedores();
            this.formAvaliacaoFornecedores();
            //this.setHideAndShowFornecedores();

        } else {
            this.formCadastroFornecedores();
        }

        this.sendFormAcoesFornecedores();

    },

    setHideAndShowFornecedores: function () {

        $('[id^=form-fornecedores-qualificacao-]').toggle();
        $('#form-fornecedores-avaliacao').toggle();
        $('#form-fornecedores-historico').toggle();

    },

    setValidateFormsFornecedores: function () {

        //Regras Validacao dos FormulÃ¡rios
        var ObjFormCadastroFornecedoresValidate = APP.controller.FornecedoresController.getObjFormCadastroFornecedoresValidate();
        APP.component.ValidateForm.init(ObjFormCadastroFornecedoresValidate, '#form-fornecedores-cadastro');

        var ObjFormQualificacaoFornecedoresValidate = APP.controller.FornecedoresController.getObjFormQualificacaoFornecedoresValidate();
        $('form[id^=form-fornecedores-qualificacao-]').each(function () {
            APP.component.ValidateForm.init(ObjFormQualificacaoFornecedoresValidate, "#" + $(this).attr('id') + "");
        });

        var ObjFormAvaliacaoFornecedoresValidate = APP.controller.FornecedoresController.getObjFormAvaliacaoFornecedoresValidate();
        APP.component.ValidateForm.init(ObjFormAvaliacaoFornecedoresValidate, '#form-fornecedores-avaliacao');

    },

    sendFormAcoesFornecedores: function () {

        this.buttonSalvar.unbind('click');
        this.buttonSalvar.on('click', function () {
            var validate = APP.controller.FornecedoresController.validateForms();

            validate = true;
            if (validate == true) {
                var fornecedor = APP.controller.FornecedoresController.getAcoesFornecedoresObj('cadastro');
                var criteriosQualificacao = APP.controller.FornecedoresController.getAcoesFornecedoresObj('qualificacao');
                var avaliacoes = APP.controller.FornecedoresController.getAcoesFornecedoresObj('avaliacao');
                if ($('[name=IdFornecedor]').val() == 0) {

                    if (fornecedor != undefined) {
                        APP.controller.FornecedoresController.saveFormAcoesFornecedores(fornecedor);
                    }

                } else {
                    if (criteriosQualificacao != undefined && avaliacoes != undefined) {
                        var Ancora = $("#Ancora").val();
                        var retornoQualificacao = null;
                        var retornoAvaliacao = null;
                        var retorno = false;

                        if (Ancora == "Qualificar") {

                            retornoQualificacao = APP.controller.FornecedoresController.saveFormAcoesQualificacoes(criteriosQualificacao);

                            if (retornoQualificacao.StatusCode == 200) {
                                retorno = true;
                            }
                            else {

                                erro = APP.component.ResultErros.init(retornoQualificacao.Erros);
                                bootbox.alert(erro);

                            }
                        }
                        else if (Ancora == "Avaliar") {

                            retornoAvaliacao = APP.controller.FornecedoresController.saveFormAcoesAvaliacoes(avaliacoes);

                            if (retornoAvaliacao.StatusCode == 200) {
                                retorno = true;
                            }
                            else {
                                erro = APP.component.ResultErros.init(retornoAvaliacao.Erros);
                                bootbox.alert(erro);
                                retorno = false;

                            }}

                        }
                        else {

                            retornoQualificacao = APP.controller.FornecedoresController.saveFormAcoesQualificacoes(criteriosQualificacao);

                            if (retornoQualificacao.StatusCode == 200) {
                                retorno = true;
                            }
                            else {

                                erro = APP.component.ResultErros.init(retornoQualificacao.Erros);
                                bootbox.alert(erro);

                            }


                            retornoAvaliacao = APP.controller.FornecedoresController.saveFormAcoesAvaliacoes(avaliacoes);

                            if (retorno == true) {
                                if (retornoAvaliacao.StatusCode == 200) {
                                    retorno = true;
                                }
                                else {
                                    erro = APP.component.ResultErros.init(retornoAvaliacao.Erros);
                                    bootbox.alert(erro);
                                    retorno = false;

                                }
                            }

                        }
                                                
                        if(retorno == true)
                        {
                            bootbox.confirm(_options.RegistroSalvoComSucesso, function (result) {
                                if (result == true) {

                                    if (Ancora == "Qualificar") {
                                        window.location.href = "/Fornecedor/AcoesFornecedores/" + $('[name=IdFornecedor]').val() + "?idProduto=" + $('[name=IdProduto]').val() + "&Ancora=Avaliar";
                                    }
                                    else {
                                        window.location.href = "/Fornecedor/IndexFornecedores?idProduto=" + $("#fornecedores-produto").val();
                                    }

                                    
                                }
                            });                            
                        }

                    }
                    //if (avaliacoes != undefined) {
                    //	APP.controller.FornecedoresController.saveFormAcoesAvaliacoes(avaliacoes);
                    //}




            }

            

        });

    },

    saveFormAcoesAvaliacoes: function (avaliacoes) {
        
        var retorno = 0;
        $.ajax({
            async: false,
            type: 'POST',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify(avaliacoes),
            url: '/Fornecedor/SalvaAvaliacoes',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                retorno = result;
            },
            error: function (result) {
                APP.component.Loading.hideLoading();
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

        return retorno;
    },

    validateForms: function () {

        var valid = true;
        $('[id^=panel-form]').each(function () {
            var isVisible = $(this).is(':visible');
            if (isVisible) {
                try {

                    var validate = $(this).closest('form').valid();
                    if (validate != true) {
                        valid = false;
                    }

                } catch (e) {
                    valid = true;
                }
                

            }
        });

        return valid;

    },

    getAcoesFornecedoresObj: function (_getForm) {

        var fornecedorObj = {};

        $('[id^=panel-form-fornecedores]').each(function () {
            var isVisible = $(this).is(':visible');
            var idPanel = $(this).attr('id').split('-');

            if (isVisible) {
                var form = _getForm;
                switch (form) {
                    case 'cadastro':
                        fornecedorObj = APP.controller.FornecedoresController.getObjFormCadastroFornecedores();
                        break;
                    case 'qualificacao':
                        fornecedorObj = APP.controller.FornecedoresController.getObjFormQualificacaoFornecedores();
                        break;
                    case 'avaliacao':
                        fornecedorObj = APP.controller.FornecedoresController.getObjFormAvaliacaoFornecedores();
                        break;
                }
            }
        });
        return fornecedorObj;

    },

    saveFormAcoesFornecedores: function (fornecedor) {

        var idProduto = $("#hdIdProduto").val();

        $.ajax({
            type: 'POST',
            data: fornecedor,
            dataType: 'json',
            url: '/Fornecedor/SalvaFornecedor/',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                if (result.StatusCode == 200) {

                    bootbox.alert(result.Success, function (retorno) {
                        window.location.href = "/Fornecedor/AcoesFornecedores/" + result.IdFornecedor + "?idProduto=" + $('[name=IdProduto]').val() + "&Ancora=Qualificar";                       
                    });

                    //$('[name=IdFornecedor]').val();
                    //var idProduto = $('[name=IdProduto]').val();
                    //APP.controller.FornecedoresController.setHideAndShowFornecedores();
                    
                    //$('.input-nome-fornecedor').val($('[name=formFornecedoresCadastroNome]').val());
                    //APP.controller.FornecedoresController.setComboResponsavelControlarQualificacao();
                    //APP.controller.FornecedoresController.setComboResponsavelQualificacao();

                    //APP.controller.FornecedoresController.getAvaliacaoCriticidadePadrao();
                    
                } else {
                    // var texto = "";

                    // $(result.Erros).each(function (item) {
                    //     texto = texto + item;
                    // });
                    console.log(result.Erros);
                    //bootbox.alert(texto);
                }
            },
            error: function (result) {

                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

    saveFormAcoesQualificacoes: function (criteriosQualificacao) {
        var myJSON = JSON.stringify(criteriosQualificacao);
        var retorno = 0;
        $.ajax({
            async: false,
            type: 'POST',
            contentType: "application/json",
            dataType: 'json',
            data: myJSON,
            url: '/Fornecedor/SalvaQualificacoes',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                retorno = result;
            },
            error: function (result) {
                APP.component.Loading.hideLoading();                
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

        return retorno;

    },

    //Formulario CADASTRO FORNECEDORES
    formCadastroFornecedores: function () {

        this.setHideAndShowCadastroFornecedores();
        this.setComboDepartamento();

    },

    setHideAndShowCadastroFornecedores: function () {

        if ($('[name=IdFornecedor]').val() != '') {
            APP.controller.FornecedoresController.setHideAndShowFornecedores();
        }

    },

    setComboDepartamento: function () {

        var idSite = $('#fornecedores-site').val();
        // var idFuncao = $('#fornecedores-funcao').val();
        // var data = { "idSite": idSite, "idFuncao": idFuncao };
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            url: '/Processo/ListaProcessosPorSite?idSite='+ idSite +'',
            beforeSend: function () { },
            success: function (result) {
                if (result.StatusCode == 200) {
                    APP.component.SelectListCompare.init(result.Lista, $('[name=formFornecedoresCadastroDepartamento] option'), '#form-fornecedores-cadastro-departamento', 'IdProcesso', 'Nome');
                }
            },
            error: function (result) {
                //bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {

            }
        });

    },

    getObjFormCadastroFornecedoresValidate: function () {

        var acoesFornecedoresFormCadastroFornecedoresObj = {
            formFornecedoresCadastroNome: 'required',
            //formFornecedoresCadastroContato: 'required',
            formFornecedoresCadastroEmail: {                
                'email': true
            },
            //formFornecedoresCadastroDepartamento: 'required',
        };

        return acoesFornecedoresFormCadastroFornecedoresObj;

    },

    getObjFormCadastroFornecedores: function () {

        var idSite = $('#fornecedores-site').val();

        var acoesFornecedoresFormCadastroFornecedoresObj = {
            IdSite: idSite,
            Nome: $('[name=formFornecedoresCadastroNome]').val(),
            IdUsuarioAvaliacao: $('[name=formFornecedoresAvaliacaoResponsavel]').val(),            
            Telefone: $('[name=formFornecedoresCadastroContato]').val(),
            Email: $('[name=formFornecedoresCadastroEmail]').val(),
            IdProcesso: $('[name=formFornecedoresCadastroDepartamento] :selected').val(),
            Produtos: [{
                IdProduto: $('[name=IdProduto]').val()
            }],
        };

        return acoesFornecedoresFormCadastroFornecedoresObj;

    },

    bindFormCadastroFornecedores: function () {

        //

    },

    //Formulario QUALIFICACAO FORNECEDORES
    formQualificacaoFornecedores: function () {

        this.setHideAndShowQualificacaoFornecedores();
        this.setComboResponsavelQualificacao();

        this.setComboResponsavelControlarQualificacao();
        this.setComboResponsavelAvaliarQualificacao();

    },

    setHideAndShowQualificacaoFornecedores: function () {

        //

    },

    setComboResponsavelQualificacao: function () {

        var idSite = $('#fornecedores-site').val();
        var idFuncao = $('#fornecedores-funcao').val();
        var data = {
            'idSite': idSite,
            'idFuncao': idFuncao
        };
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            data: data,
            url: '/Usuario/ObterUsuariosPorFuncao',
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    
                    $(".form-fornecedores-qualificacao").each(function () {
                    
                        APP.component.SelectListCompare.init(result.Lista, $(this).find('[name=formFornecedoresQualificacaoResponsavel] option'), ("#" + $(this).find('[name=formFornecedoresQualificacaoResponsavel]').attr("id")), 'IdUsuario', 'NmCompleto');

                    });

                    
                }
            },
            error: function (result) {
                //bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {

            }
        });
    },

    
    setComboResponsavelAvaliar: function () {

        var idSite = $('#fornecedores-site').val();
        var idFuncao = 113;
        var data = {
            'idSite': idSite,
            'idFuncao': idFuncao
        };
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            data: data,
            url: '/Usuario/ObterUsuariosPorFuncao',
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {

                    APP.component.SelectListCompare.init(result.Lista, $('[name=formFornecedoresAvaliacaoResponsavel] option'), ("#" + $('[name=formFornecedoresAvaliacaoResponsavel]').attr("id")), 'IdUsuario', 'NmCompleto');
                   
                }
            },
            error: function (result) {
                //bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {

            }
        });
    },

    setComboResponsavelControlarQualificacao: function () {

        var idSite = $('#fornecedores-site').val();
        var idFuncao = $('#fornecedores-funcao').val();
        var data = {
            'idSite': idSite,
            'idFuncao': idFuncao
        };
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            data: data,
            url: '/Usuario/ObterUsuariosPorFuncao',
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {

                    $(".form-fornecedores-qualificacao").each(function () {

                        APP.component.SelectListCompare.init(result.Lista, $(this).find('[name=formFornecedoresQualificacaoResponsavelControlar] option'), ("#" + $(this).find('[name=formFornecedoresQualificacaoResponsavelControlar]').attr("id")), 'IdUsuario', 'NmCompleto');

                    });                    
                
                }
            },
            error: function (result) {
                //bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {

            }
        });

    },

    setComboResponsavelAvaliarQualificacao: function () {

        var idSite = $('#fornecedores-site').val();
        var idFuncao = $('#fornecedores-funcao').val();
        var data = {
            'idSite': idSite,
            'idFuncao': idFuncao
        };
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            data: data,
            url: '/Usuario/ObterUsuariosPorFuncao',
            beforeSend: function () {

            },
            success: function (result) {
                if (result.StatusCode == 200) {
                    APP.component.SelectListCompare.init(result.Lista, $('[name=formFornecedoresQualificacaoResponsavelAvaliar] option'), '#form-fornecedores-qualificacao-responsavel-avaliar', 'IdUsuario', 'NmCompleto');
                }
            },
            error: function (result) {
                //bootbox.alert("Ocorreu um erro!");
            },
            complete: function (result) {

            }
        });

    },

    getObjFormQualificacaoFornecedoresValidate: function () {

        var acoesFornecedoresFormQualificacaoFornecedoresObj = {
            formFornecedoresQualificacaoProduto: 'required',
            formFornecedoresQualificacaoFornecedor: 'required',
            formFornecedoresQualificacaoResponsavel: 'required',
            formFornecedoresQualificacaoDtQualificacao: 'required',
            arquivoEvidencia: 'required',
            formFornecedoresQualificacaoAprovado: 'required',
            //formFornecedoresQualificacaoObservacoes: 'required',
            formFornecedoresQualificacaoResponsavelControlar: 'required',
            formFornecedoresQualificacaoDtEmissao: 'required',
            formFornecedoresQualificacaoNmDocumento: 'required',
            formFornecedoresQualificacaoOrgaoExpeditor: 'required',
            formFornecedoresQualificacaoDtVencimento: 'required',
            formFornecedoresQualificacaoObservacoesControlar: 'required',
            formFornecedoresQualificacaoResponsavelAvaliar: 'required',
            formFornecedoresQualificacaoDtEstimativa: 'required',
        };

        return acoesFornecedoresFormQualificacaoFornecedoresObj;

    },

    getObjFormQualificacaoFornecedores: function () {

        var idSite = $('#fornecedores-site').val();

        
        var acoesFornecedoresFormQualificacaoFornecedoresObj = [];
        $('form[id^=form-fornecedores-qualificacao-]').each(function (key, val) {
            var obj = {
                IdAvaliaCriterioQualificacao: $(this).find('[name=IdAvaliaCriterioQualificacao]').val(),
                IdCriterioQualificacao: $(this).find('[name=IdCriterioQualificacao]').val(),
                IdFornecedor: $('[name=IdFornecedor]').val(),
                DtVencimento: $(this).find('[name=formFornecedoresQualificacaoDtVencimento]').val(),
                IdResponsavelPorQualificar: $(this).find('[name=formFornecedoresQualificacaoResponsavel] :selected').val(),
                DtQualificacaoVencimento: $(this).find('[name=formFornecedoresQualificacaoDtQualificacao]').val(),
                ArquivosDeEvidenciaAux: APP.controller.FornecedoresController.getAnexosContratos(this),
                Aprovado: $(this).find('[name=formFornecedoresQualificacaoAprovado]:checked').val(),
                Observacoes: $(this).find('[name=formFornecedoresQualificacaoObservacoes]').val(),
                IdResponsavelPorControlarVencimento: $(this).find('[name=formFornecedoresQualificacaoResponsavelControlar] :selected').val(),

                DtEmissao: $(this).find('[name=formFornecedoresQualificacaoDtEmissao]').val(),
                NumeroDocumento: $(this).find('[name=formFornecedoresQualificacaoNmDocumento]').val(),
                OrgaoExpedidor: $(this).find('[name=formFornecedoresQualificacaoOrgaoExpeditor]').val(),
                ObservacoesDocumento: $(this).find('[name=formFornecedoresQualificacaoObservacoesControlar]').val(),

            };
            //obj.IdSite = idSite;




            acoesFornecedoresFormQualificacaoFornecedoresObj.push(obj);
        });

        return acoesFornecedoresFormQualificacaoFornecedoresObj;

    },


    getAnexosContratos: function (_this) {

        var anexoContratoModel = APP.controller.FornecedoresController.models.AnexoModel;
        var arrayAnexoContrato = [];

        $(_this).find('.dashed li a:first-child').each(function () {

            var nameImg = $(this).text();
            var id = $(this).closest('li').find('[name=c]').val() != undefined && $(this).closest('li').find('[name=formCriarFornecedorEvidenciaIdAnexo]').val() != null && $(this).closest('li').find('[name=formCriarFornecedorEvidenciaIdAnexo]').val() != "" ? $(this).closest('li').find('[name=formCriarFornecedorEvidenciaIdAnexo]').val() : "0";
            var anexoContratoCliente = anexoContratoModel.constructor(
                id,
                nameImg,
                $(this).data('b64'),
            );

            arrayAnexoContrato.push(anexoContratoCliente);
        });

        return arrayAnexoContrato;

    },


    bindFormQualificacaoFornecedores: function () {

        //

    },

    //Formulario AVALIACAO FORNECEDORES
    formAvaliacaoFornecedores: function () {

        this.setHideAndShowAvaliacaoFornecedores();
        this.setComboResponsavelAvaliar();

    },

    setHideAndShowAvaliacaoFornecedores: function () {

        //

    },

    getObjFormAvaliacaoFornecedoresValidate: function () {

        var acoesFornecedoresFormAvaliacaoFornecedoresObj = {
            formFornecedoresAvaliacaoNome: 'required',
            formFornecedoresAvaliacaoCriterio: 'required',
            formFornecedoresAvaliacaoNota: {
                'required': true,
                'max': 100,
                'min': 1
            },
        };

        return acoesFornecedoresFormAvaliacaoFornecedoresObj;

    },

    getObjFormAvaliacaoFornecedores: function () {
        
        var dataProximaAvaliacao = $('[name=formFornecedoresDtProximaAvaliacao]').val();

        var avaliacoes = [];
        $('#tb-fornecedores-avaliacao tbody tr').each(function () {

            var avaliacao = {
                IdCriterioAvaliacao: $(this).find('[name=formFornecedoresAvaliacaoCriterioId]').val(),
                IdFornecedor: $('[name=IdFornecedor]').val(),
                NotaAvaliacao: $(this).find('[name=formFornecedoresAvaliacaoNota]').val(),
                DtProximaAvaliacao: dataProximaAvaliacao,
                IdUsuarioAvaliacao: $('[name=formFornecedoresAvaliacaoResponsavel]').val(),  
            };
            avaliacoes.push(avaliacao);
        });

        return avaliacoes;

    },

    bindFormAvaliacaoFornecedores: function () {

        //

    },

    //Formulario HISTORICO FORNECEDORES
    formHistoricoFornecedores: function () {

        this.setHideAndShowHistoricoFornecedores();

    },

    setHideAndShowHistoricoFornecedores: function () {

        //

    },

};
