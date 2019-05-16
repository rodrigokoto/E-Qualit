$(document).ready(function () {

    var tipoGrafico = $("#hndTipoGrafico").val();
    var tipoNaoConformidade = $("#hndTipoNaoConformidade").val();
    var dtDe = $("#hndDtDe").val();
    var dtAte = $("#hndDtAte").val();
    var estiloGrafico = $("#hndEstiloGrafico").val();

    MontarGraficos(tipoGrafico, tipoNaoConformidade, dtDe, dtAte, estiloGrafico);
});





MontarGraficos = (function (tipoGrafico, tipoNaoConformidade, dtDe, dtAte, estiloGrafico) {

    var url = '';

    if (estiloGrafico == 1) {
        url = UrlObterDadosGraficosBarra;
    }
    else {
        url = UrlObterDadosGraficosPizza;
    }

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify({
            'tipoGrafico': tipoGrafico,
            'tipoNaoConformidade': tipoNaoConformidade,
            'dtDe': dtDe,
            'dtAte': dtAte
        }),
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        error: function () {
            $("#loading").hide();
        },
        success: function (data) {

            $("#loading").hide();

            if (estiloGrafico == 1) 
                CarregarGraficoBarra(data);
            else
                CarregarGraficoPizza(data);
        }
    });

});


CarregarGraficoPizza = (function (dataPoints) {

    if (dataPoints.length > 0) {
        $.plot('#grafico', dataPoints, {
            series: {
                pie: {
                    show: true,
                    radius: 1,
                    label: {
                        show: true,
                        radius: 3 / 4,
                        background: {
                            opacity: 0.5,
                            color: '#000'
                        }
                    }
                }
            }
        });
    }
    else
        $('#grafico').empty();

    $('#grafico').show();
});


CarregarGraficoBarra = (function (dataPoints) {

    if (dataPoints.length > 0) {
        $.plot("#grafico", [dataPoints], {
            series: {
                bars: {
                    show: true,
                    barWidth: 0.6,
                    align: "center"
                }
            },
            xaxis: {
                mode: "categories",
                tickLength: 0
            }
        });
    }
    else
        $('#grafico').empty();


    $('#grafico').show();

});