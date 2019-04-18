
/*
|--------------------------------------------------------------------------
| Relatorio Controller
|--------------------------------------------------------------------------
*/

APP.controller.RelatorioController = {

    init: function () {
        //var page = APP.component.Util.getPage();

        this.dashBoard();
    },


    dashBoard: function () {
        this.getRelatorioPizza();
        this.getRelatorioBarra();
        this.getRelatorioRosquinha();
        this.getRelatorioEmpilhado();
        this.getRelatorioEmpilhado();
        this.getRelatorioSpline();
        this.getRelatorioRange();
    },

    getNaoConformidades: function () {
        var json = '';
        $.ajax({
            type: "GET",
            dataType: 'json',
            async: false,
            url: '/Relatorio/NaoConformidade',
            success: function (result) {
                if (result.StatusCode == 200) {
                    json = result.Relatorio;
                }
            },
            error: function (result) {
                bootbox.alert(_options.TesteErro);
            }
        });
        return json;
    },

    getRelatorioPizza: function () {
        var jsonLocal = this.getNaoConformidades();

        var filtro = this.montarRelatorioPizza(jsonLocal[i], i);
        APP.component.Highcharts.pizza(filtro);
    },

    montarRelatorioPizza: function (dados, identificador) {

        var seriesTotais = { data: { valorMeta: [], valorRealizado: [] } };

        var objGrafico = {
            Container: "container" + identificador,
            Chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 45,
                    beta: 0
                }
            },
            Titulo: _options.NaoConformidade,
            SeriesTotais: [{
                name: _options.NaoConformidade,
                type: 'pie',
                yAxis: 1,
                data: seriesTotais.data,
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                }
            }],
        };

        return objGrafico;

    },

    getRelatorioBarra: function () {
        var filtro = "";
        APP.component.Highcharts.barra(filtro);
    },

    getRelatorioRosquinha: function () {
        var filtro = "";
        APP.component.Highcharts.rosquinha(filtro);
    },

    getRelatorioEmpilhado: function () {
        var filtro = "";
        APP.component.Highcharts.empilhado(filtro);
    },

    getRelatorioSpline: function () {
        var filtro = "";
        APP.component.Highcharts.spline(filtro);
    },

    getRelatorioRange: function () {
        var filtro = "";
        APP.component.Highcharts.range(filtro);
    },

};
