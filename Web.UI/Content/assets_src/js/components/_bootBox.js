/*
|--------------------------------------------------------------------------
| BootBox
|--------------------------------------------------------------------------
*/

APP.component.BootBox = {

    init: function () {

        this.chamaBootBox(_actionPartialViewBackend, _controllerBackend, idPainelHtml, _controllerIntJS, TituloPartial, funcaoListaAtivos, _btnClick);

    },

    chamaBootBox: function (_actionPartialViewBackend, _controllerBackend, idPainelHtml, _controllerIntJS, TituloPartial, funcaoListaAtivos, _btnClick) {
       
            var _tipo = $(_btnClick).data("tipo");
            var _site = $(_btnClick).data("site");
            $.ajax({
                url: "/ControladorCategorias/" + _actionPartialViewBackend + "?tipo=" + _tipo + "&site=" + _site,
                type: 'POST',
                data: {
                    __RequestVerificationToken: $("[name=__RequestVerificationToken]").val()
                },
                beforeSend: function () {
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    bootbox.confirm({
                        message: "<div id='" + idPainelHtml + "'>" + result + "</div>", title: TituloPartial,
                        "className": "modal-form",
                        callback: function (result) {
                            funcaoListaAtivos();
                        }
                    });
                    _controllerIntJS(_tipo, _site);
                },
                complete: function () { 
                    APP.component.Loading.hideLoading();
                },
                error: callBackError
            });

    }

};