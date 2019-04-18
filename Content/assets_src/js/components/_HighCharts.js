/*
|--------------------------------------------------------------------------
| Highcharts
|--------------------------------------------------------------------------
*/

APP.component.Highcharts = {

    colunas: function (_highchartsObj) {

        return this.setHighchartsRelatorioColunas(_highchartsObj);

    },

    barraEColuna: function (_highchartsObj) {

        return this.setHighchartsRelatorioColuna(_highchartsObj);
    },

    pizza: function (_highchartsObj) {

        return this.setHighchartsPizza(_highchartsObj);
    },

    barra: function (_highchartsObj) {

        return this.setHighchartsBarra(_highchartsObj);
    },

    rosquinha: function (_highchartsObj) {

        return this.setHighchartsRosquinha(_highchartsObj);
    },

    empilhado: function (_highchartsObj) {

        return this.setHighchartsEmpilhado(_highchartsObj);
    },

    spline: function (_highchartsObj) {

        return this.setHighchartsSpline(_highchartsObj);
    },

    range: function (_highchartsObj) {

        return this.setHighchartsRange(_highchartsObj);
    },

    setHighchartsRelatorioColunas: function (filtro) {            
            
        Highcharts.chart(filtro.Container, {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: filtro.Titulo
            },
            subtitle: {
                text: filtro.SubTitulo
            },
            xAxis: [{
                categories: filtro.Categorias,
                crosshair: true
            }],
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value} '+filtro.UnidadeIndicador+'',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: '',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                title: {
                    text: '',
                },
            }],
            tooltip: {
                shared: true
            },
            legend: filtro.Legenda,
            series: [{
                name: filtro.SeriesTotais[0].name,
                type: 'column',
                data: filtro.SeriesTotais[0].data,
                tooltip: {
                    valueSuffix: filtro.UnidadeIndicador
                }
        
            }, {
                name: filtro.SeriesTotais[1].name,
                type: 'spline',
                data: filtro.SeriesTotais[1].data,
                tooltip: {
                    valueSuffix: filtro.UnidadeIndicador
                }
            }]
        });

    },


};