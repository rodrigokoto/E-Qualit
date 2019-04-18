/*
|--------------------------------------------------------------------------
| Result Erros - Alert HTML
|--------------------------------------------------------------------------
*/

APP.component.ResultErros = {

    init: function (_listErros) {

        this.setup();
        return this.setResultErros(_listErros);

    },

    setup: function () {

        //

    },

    setResultErros: function (_listErros) {

        var html = "";
        html += "<div class='erros-result'>";
        html += "<ul>";

        if (Array.isArray(_listErros)) {
            $.each(_listErros, function (index, value) {
                html += "<li>";
                html += "- " + value;
                html += "</li>";
            });
        }
        else {
            html += "<li>";
            html += "- " + _listErros;
            html += "</li>";
        }

        html += "</ul>";
        html += "</div>";

        return html;

    },

};