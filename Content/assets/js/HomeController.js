
/*
|--------------------------------------------------------------------------
| HomeController
|--------------------------------------------------------------------------
*/

APP.controller.HomeController = {

    init: function () {

        var page = APP.component.Util.getPage();

        if (page === "Administrador") {
            this.startAdmin();
        } else if (page === "Index") {
            this.indexProcessos();
        }
    },

    startAdmin: function () {
        APP.component.DataTable.init('#tbCliente');
    },

    setup: function () {

        //

    },

    indexProcessos: function () {

        this.setIdProcesso();
        this.setMenuBox();

    },

    setIdProcesso: function () {
        $('[name="modulos"]').on('click', function () {
            var idProcesso = $(this).closest('div').attr('id');
            var NmProcesso = $(this).closest('div').find('h3').text();
            APP.controller.HomeController.saveProcessoSelecionado(idProcesso, NmProcesso, true);
        });
    },

    setMenuBox: function () {
        $('[name="mini-menu"]').on('click', function () {

            // Remember the link href
            var href = $(this).attr("href");

            event.preventDefault();

            var idProcesso = $(this).data('idprocesso');
            var NmProcesso = $(this).data('nomeprocesso');
            APP.controller.HomeController.saveProcessoSelecionado(idProcesso, NmProcesso, false);

            window.location = href;

        });
    },

    saveProcessoSelecionado: function (idProcesso, nomeProcesso, assync) {

        var data = { 'idProcesso': idProcesso, 'nomeProcesso': nomeProcesso };

        $.ajax({
            type: "POST",
            url: "/Processo/EscolheProcesso",
            data: data,
            async: assync,
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
            },
            error: function (result) {
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });
    },
};
