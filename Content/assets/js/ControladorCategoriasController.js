
/*
|--------------------------------------------------------------------------
| ControladorCategoriasController
|--------------------------------------------------------------------------
*/

APP.controller.ControladorCategoriasController = {

    init: function (_tipo, _site, funcaoListaAtivos, _btnClick) {

        APP.component.BootBox.chamaBootBox("Cadastro", "ControladorCategorias", "pnlCadUsu", this.behavior, "Cadastro", funcaoListaAtivos, _btnClick);

        this.behavior(_tipo, _site);

    },


    behavior: function (_tipo, _site) {

        var tipo = _tipo;
        var site = _site;
        limpaCamposEdicao();

        bind();

        function bind() {

            ////////////   Tabela unica da PartialView
            ConfigTabela('#tbCadastroCategoria');

            ////////////   Clean
            $('.cadastro-categoria-clean').on('click', () => {
                limpaCamposEdicao();
            });

            ////////////   Exibir Novo Registro
            $('#btn-new-tipo').on('click', function () {
                $('table tbody tr #descricao-hidden').parent().show();
            });

            ////////////   Edit
            $('tr td .edita-descricao').on('click', function (e) {

                ////Input Show
                var $idCurrentClick = $(e)[0].currentTarget.dataset.url;
                var $descricaoCategoria = $(e)[0].currentTarget.parentElement.parentElement.innerText;

                $('table tbody tr #descricao-hidden').parent().show();
                $('#id-cadastro-categoria').val($idCurrentClick);
                $('#nova-descricao-cadastro-categoria').val($descricaoCategoria);

            });

            ////////////   Save
            $('#cadastro-categoria-insert').on('click', () => {

                if ($('#nova-descricao-cadastro-categoria').val().length > 200) {

                    $("#msgMaxCaracteres").show();

                }
                else {

                    $("#msgMaxCaracteres").hide();

                    var controladorCategorias = {
                        'IdControladorCategorias': $('#id-cadastro-categoria').val(),
                        'IdSite': site,
                        'Descricao': $('#nova-descricao-cadastro-categoria').val(),
                        'TipoTabela': tipo,
                        'Ativo': true,
                        "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val(),
                    };

                    ///ajax
                    $.post('/ControladorCategorias/Salvar', controladorCategorias, function (callback) {
                        limpaCamposEdicao();

                    }).done(function (result) {
                        refresh(tipo, site);



                    }).fail(function (result) {
                        callBackError(result);
                    });

                }

            });

            ////////////   Delete
            $('tr td .exclui-descricao').on('click', function (e) {
                var $idCurrentClick = $(e)[0].currentTarget.dataset.url;
                var data = {
                    'idControladorCategorias': $idCurrentClick,
                    "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()
                };

                bootbox.confirm(_options.DesejaExcluir, function (result) {
                    if (result) {
                        // 
                        $.post('/ControladorCategorias/Excluir', data, function (data) {
                        }).done(function () {
                            refresh(tipo, site);
                        })
                            .fail(function (result) {
                                callBackError(result);
                            })
                            .always(function () {
                                refresh(tipo, site);
                            });
                    }
                });

            });

            ////////////   Ativa Desativa
            $('tr td .ativa-desativa').on('click', function (e) {
                var $idCurrentClick = $(e)[0].currentTarget.dataset.url;
                var data = {
                    'idControladorCategorias': $idCurrentClick,
                    "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val()

                };
                $.post('/ControladorCategorias/AtivarDesativa', data, function (callback) {
                    limpaCamposEdicao();
                }).done(function () {
                    refresh(tipo, site);
                })
                    .fail(function (result) {
                        callBackError(result);
                    });

            });


        }

        function hideDescricao() {
            $('#descricao-hidden').parent().hide();
        }

        function limpaCamposEdicao() {
            $('#id-cadastro').val("");
            $('#nova-descricao-cadastro-categoria').val("");
            hideDescricao();
        }

        function refresh(tipo, site) {
            $.get("/ControladorCategorias/AtualizaTabela?tipo=" + tipo + "&site=" + site, function (result) {
                $("#pnlCadUsu").html(result);
                limpaCamposEdicao();
                bind();
            });
        }

    },

};
