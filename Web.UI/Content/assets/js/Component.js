
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
                zoomType: 'xy', 
                events: {
                    load: function () {

                        for (var i = 0; i < this.series[0].points.length; i++) {
                            if (filtro.Direcao === "1") {
                                if (this.series[0].points[i].y < this.series[1].points[i].y) {
                                    var point = this.series[0].points[i];

                                    point.update({
                                        color: 'red'
                                    });
                                }
                            }
                            else
                            {
                                if (this.series[0].points[i].y > this.series[1].points[i].y) {
                                    var point = this.series[0].points[i];

                                    point.update({
                                        color: 'red'
                                    });
                                }
                            }
                        }
                    }
                }
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
                    format: '{value} ' + filtro.UnidadeIndicador + '',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    },
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
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true
                    }
                }
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
/*
|--------------------------------------------------------------------------
| Bar Rating
|--------------------------------------------------------------------------
*/

APP.component.BarRating = {

    init: function (_selector, _theme) {

        this.setup();
        this.setBarRating(_selector, _theme);

    },

    setup: function () {

        //

    },

    setBarRating: function (_selector, _theme) {

        $(_selector).barrating('show', {
            theme: _theme,
            initialRating: '0',
            showValues: false,
            onSelect: function (value, text) {
                //console.log(value, text);
            }
        });

    },

    setBarRatingGestaoDeRisco: function (_divGestaoDeRisco, _theme) {

        $(_divGestaoDeRisco).barrating('show', {
            theme: _theme,
            initialRating: '0',
            showValues: false,
            onSelect: function (value, text) {
                APP.component.GestaoDeRiscoPartial.setCriticidade(_divGestaoDeRisco, value, text);
            }
        });

    },

};
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
/*
|--------------------------------------------------------------------------
| CKEditor
|--------------------------------------------------------------------------
*/

APP.component.CkEditor = {

    init: function (_nameID) {

        this.setup();
        this.setCkEditor(_nameID);

    },

    setup: function () {

        //

    },

    setCkEditor: function (_nameID) {

        CKEDITOR.replace(_nameID);

    },

};
/*
|--------------------------------------------------------------------------
| Data Picker
|--------------------------------------------------------------------------
*/

APP.component.Datapicker = {

    init: function () {

        this.setup();
        this.ativaDataPicker();
        this.ativaTimePicker();
        this.setIconDataPicker();
        this.setIconTimePicker();

    },

    setup: function () {

        //

    },

    ativaDataPicker: function () {

        $('.data').each(function () {

            if ($(this).attr("class") != "form-control data time") {
                if ($(this).attr("name") == "formAcaoImadiataTbDtEfetivaImplementacao" || $(this).attr("name") == "formFornecedoresQualificacaoDtQualificacao" || $(this).attr("name") == "formFornecedoresQualificacaoDtEmissao") {
                    $(this).datepicker({
                        dateFormat: _options.datepicker,
                        maxDate: new Date(),
                        pickTime: true,
                    });
                }
                else {
                    $(this).datepicker({
                        dateFormat: _options.datepicker,
                        minDate: new Date(),
                        pickTime: true,

                    });
                }
            }

        });

    },

    setDataPicker: function (_this, _seletor) {

        var arr = [];
        var dataSelected = $(_this).val();
        var strNewFormat = dataSelected.split('/');
        arr.push(strNewFormat[1]);
        arr.push(strNewFormat[0]);
        arr.push(strNewFormat[2]);

        var data = new Date(arr.join('/'));

        $(_seletor).datepicker("option", "minDate", data);

    },

    ativaTimePicker: function () {

        $('.time').timepicker();

        //$('.time').timepicker({
        //    use24hours: true,
        //    format: 'HH:mm PP',
        //    showMeridian: false,
        //    pick12HourFormat: true
        //});

    },

    setIconDataPicker: function () {

        $('.fa-calendar').on('click', function () {

            if ($(this).closest('div').find('.data').attr("disabled") == undefined) {
                $(this).closest('div').find('.data').trigger("focusin");
            }

        });

    },

    setIconTimePicker: function () {

        $('.fa-clock-o').on('click', function () {

            $(this).closest('div').find('.time').trigger("focus");

        });

    },

};
/*
|--------------------------------------------------------------------------
| DataTable
|--------------------------------------------------------------------------
*/

APP.component.DataTable = {

    init: function (_nomeTable) {

        this.setup();
        return this.setDataTable(_nomeTable);

    },

    setup: function () {

        //

    },

    setDataTable: function (_nomeTable) {
        table = $(_nomeTable).DataTable({
            "order": [[0, "asc"],[1 , "asc"]],
            "columnDefs": [{
                target: [1],
                orderData: [0, 1],
            }],
            "destroy": true,
            "responsive": true,
            "lengthMenu": [[10, 50, 100, -1], [10, 50, 100, _options.tabelaTodosRegistros]],
            "iDisplayLength": 10,
            "language": {
                "lengthMenu": _options.tabelaLengthMenu,
                "zeroRecords": _options.tabelaZeroRecords,
                "info": _options.tabelaInfo,
                "infoEmpty": _options.tabelaInfoEmpty,
                "infoFiltered": _options.tabelaInfoFiltered,
                "search": _options.tabelaSearch,
                "searchPlaceholder": _options.tabelaSearchPlaceholder,
                "paginate": {
                    "next": _options.tabelaPaginateNext,
                    "previous": _options.tabelaPaginatePrevious
                }
            },
            "createdRow": function (row, data, dataIndex) {
                $(row).children().addClass("details-control");
                $(row).attr("id", dataIndex);
            },

        });

        // Apply the search
        $("table thead th input[type=text]").on('keyup change', function () {
            table
                .column($(this).closest('th').index() + ':visible')
                .search(this.value)
                .draw();
        });
        // Apply the search
        $("table thead th select").on('change', function () {
            var val = $.fn.dataTable.util.escapeRegex(
                $(this).val()
            );
            table
                .column($(this).closest('th').index() + ':visible')
                .search(val ? '^' + val + '$' : '', true, false)
                .draw();
        });

        table.on('responsive-resize', function (e, datatable, columns) {
            var i = 0;
            $("#hr1 > th").each(function () {
                if (columns[i]) {
                    $(this).removeClass("hide");
                } else {
                    $(this).addClass("hide");
                }
                i++;
            });
        });

        $(table.column(0).header()).addClass('never');
        table.responsive.rebuild();
        table.responsive.recalc();

        $(table.column(0).header()).removeClass('never');
        table.responsive.rebuild();
        table.responsive.recalc();
    },

    setDataTableParam: function (_nomeTable, _positionConlumn, _ascOrDesc, searchPlaceholder) {

        table = $(_nomeTable).DataTable({
            "destroy": true,
            "responsive": true,
            "lengthMenu": [[10, 50, 100, -1], [10, 50, 100, _options.tabelaTodosRegistros]],
            "iDisplayLength": 10,
            "order": [[_positionConlumn, _ascOrDesc]],
            "language": {
                "lengthMenu": _options.tabelaLengthMenu,
                "zeroRecords": _options.tabelaZeroRecords,
                "info": _options.tabelaInfo,
                "infoEmpty": _options.tabelaInfoEmpty,
                "infoFiltered": _options.tabelaInfoFiltered,
                "search": _options.tabelaSearch,
                "searchPlaceholder": searchPlaceholder,
                "paginate": {
                    "next": _options.tabelaPaginateNext,
                    "previous": _options.tabelaPaginatePrevious
                }
            },
            "createdRow": function (row, data, dataIndex) {
                $(row).children().addClass("details-control");
                $(row).attr("id", dataIndex);
            },

        });

        // Apply the search
        $("table thead th input[type=text]").on('keyup change', function () {
            table
                .column($(this).closest('th').index() + ':visible')
                .search(this.value)
                .draw();
        });
        // Apply the search
        $("table thead th select").on('change', function () {
            var val = $.fn.dataTable.util.escapeRegex(
                $(this).val()
            );
            table
                .column($(this).closest('th').index() + ':visible')
                .search(val ? '^' + val + '$' : '', true, false)
                .draw();
        });

        table.on('responsive-resize', function (e, datatable, columns) {
            var i = 0;
            $("#hr1 > th").each(function () {
                if (columns[i]) {
                    $(this).removeClass("hide");
                } else {
                    $(this).addClass("hide");
                }
                i++;
            });
        });

        $(table.column(0).header()).addClass('never');
        table.responsive.rebuild();
        table.responsive.recalc();

        $(table.column(0).header()).removeClass('never');
        table.responsive.rebuild();
        table.responsive.recalc();

        resizeTable(table);

    },

};
/*
|--------------------------------------------------------------------------
| Data Today
|--------------------------------------------------------------------------
*/

APP.component.Datatoday = {

    init: function () {

        return this.getDataToday();

    },

    getDataToday: function () {

        var today = new Date(),
            DD = today.getDate(),
            MM = today.getMonth() + 1, //January is 0!
            YYYY = today.getFullYear();

        if (DD < 10) {
            DD = '0' + DD;
        }
        if (MM < 10) {
            MM = '0' + MM;
        }

        return today = DD + "/" + MM + "/" + YYYY;

    },

    getHoraNow: function () {

        new Date($.now());
        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();

        return time;

    },

    getDateUsFormat: function (_data) {

        // O formato da data contida na variÃ¡vel "data" Ã© dd/mm/yyyy
        var split = _data.split('/');
        var novadata = split[1] + "/" + split[0] + "/" + split[2];
        var data_americana = new Date(novadata);

        return data_americana;

    },

    getCompareDate: function (_dateNow, _dateLast) {

        var dataNow = APP.component.Datatoday.getDateUsFormat(_dateNow);
        var dataLast = APP.component.Datatoday.getDateUsFormat(_dateLast);
        if (new Date(dataNow).getTime() > new Date(dataLast).getTime()) {
            return true;
        } else {
            return false;
        }

    },

};
/*
|--------------------------------------------------------------------------
| Drag And Drop
|--------------------------------------------------------------------------
*/

APP.component.DragAndDrop = {

    dragSrcEl: {},

    bindDraggables: function () {
        var cols = document.querySelectorAll('.pai-calendario-processo, .calendar ul li');

        [].forEach.call(cols, function (col) {
            col.addEventListener('dragstart', APP.component.DragAndDrop.drag, false);
            col.addEventListener('dragenter', APP.component.DragAndDrop.handleDragEnter, false);
            col.addEventListener('dragover', APP.component.DragAndDrop.handleDragOver, false);
            col.addEventListener('dragleave', APP.component.DragAndDrop.handleDragLeave, false);
            col.addEventListener('drop', APP.component.DragAndDrop.drop, false);
            col.addEventListener('dragend', APP.component.DragAndDrop.handleDragEnd, false);
        });
    },

    createDraggables: function () {

    },

    allowDrop: function (ev) {
        ev.preventDefault();
    },

    //https://developer.mozilla.org/en-US/docs/Web/API/Document/drag_event
    dragged: null,

    drag: function (ev) {
        ev.dataTransfer.setData("text", ev.target.id);
        this.dragged = event.target;
    },

    drop: function (ev) {

        ev.preventDefault();
        if (this.dragged === null)
            return;
        var nodeCopy = this.dragged.cloneNode(true);

        var temNovoNode = false;

        //target é o UL do topo, estava podnendo arrastar para dentro de item já existente
        let target = $(ev.target);
        if (target.prop("tagName").toLowerCase() != "ul")
            target = target.closest("ul");

        target.find("li").each(function () {
            if ($(this).find("span").text().trim() == $(nodeCopy).find("span").text().trim()) {
                if (!temNovoNode) {
                    bootbox.alert(_options.Auditoria_ProcessoExisteMes);
                }
                temNovoNode = true;
            }
        });

        if (!temNovoNode) {
            $(nodeCopy).find("span").html($(nodeCopy).find("span").text() + "<a href='#' onclick='ExcluirProcessoAuditoria(this);' style='float: right; margin-right: 10px; color: white'><i class='fa fa-trash'></i></a>")
            target.append(nodeCopy);
            //remover do mes anteriori, se tiver um botão de excluir
            $(this.dragged).find("a").click();
        }
    },





    handleDrop: function (e) {
        if (e.stopPropagation) {
            e.stopPropagation();
        }

        if (APP.component.DragAndDrop.dragSrcEl != this) {
            APP.component.DragAndDrop.dragSrcEl.innerHTML = this.innerHTML;
            this.innerHTML = e.dataTransfer.getData('text/html');
        }
        return false;
    },

    handleDragEnd: function (e) {
        var cols = document.querySelectorAll('.pai-calendario-processo, .calendar ul li');
        this.style.opacity = 1;

        [].forEach.call(cols, function (col) {
            col.classList.remove('over');
        });
    },

    handleDragEnter: function (e) {
        this.classList.add('over');
    },

    handleDragLeave: function (e) {
        this.classList.remove('over');
    },

    handleDragOver: function (e) {
        if (e.preventDefault) {
            e.preventDefault();
        }

        e.dataTransfer.dropEffect = 'copy';
        return false;
    },

    handleDragStart: function (e) {
        this.style.opacity = 0.4;
        APP.component.DragAndDrop.dragSrcEl = this;
        e.dataTransfer.effectAllowed = 'copy';
        e.dataTransfer.setData('text/html', this.innerHTML);
    },

    init: function () {
        var readyStateCheckInterval = setInterval(function () {
            if (document.readyState === "complete") {
                clearInterval(readyStateCheckInterval);
                //APP.component.DragAndDrop.createDraggables();
                //APP.component.DragAndDrop.bindDraggables();
            }
        }, 10);
    }

};
/*
|--------------------------------------------------------------------------
| File Upload
|--------------------------------------------------------------------------
*/

APP.component.FileUpload = {

    init: function () {

        this.setup();
        this.fileUpload();
        this.delItemFileUpload();
        this.fileUploadMultiple();
        this.fileUploadArquivo();

        this.fileUploadRai();
        this.delItemFileUploadRai();
        this.fileUpload2Rai();
        this.delItemFileUpload2Rai();

    },

    setup: function () {

        //Botoes
        this.buttonUploadImg = $("[class^=btn-upload]");
        this.buttonUploadImgMultiple = $("[class^=btn-upload-multiple]");
        this.buttonUploadArquivo = $("[class^=btn-upload-file]");
        this.buttonDelFileUpload = $("[class^=btn-delete]");

        this.buttonUploadRai = $("[class^=btn-upload-rai]");
        this.buttonUpload2Rai = $("[class^=btn-upload2-rai]");
        this.buttonDelFileUploadRai = $("[class^=btn-delete-rai]");
        this.buttonDelFileUpload2Rai = $("[class^=btn-delete2-rai]");

    },

    fileUpload: function () {

        this.buttonUploadImg.unbind('click');
        this.buttonUploadImg.on('click', function () {
            $(this).closest('div').find('input').trigger("click");

            //Variaveis de auxilio
            var idInput = $(this).closest('div').find('input').attr('id');
            var srcImg = $(this).children();
            var base64 = "";

            $('#' + idInput + '').unbind('change');
            $('#' + idInput + '').on('change', function () {

                var _files = this.files;

                if (_files && _files[0]) {

                    var reader = new FileReader();
                    var name = "";

                    reader.onload = function (e) {

                        srcImg.attr('src', e.target.result);

                        //Pega somente a String Base64 e coloca da tag data-b64
                        base64 = e.target.result.split(',');
                        $('#' + idInput + '').data('b64', '' + base64[1] + '');

                        //Pega o nome do arquivo e inputa na UL de descricao de arquivos
                        name = $('#' + idInput + '').prop('files')[0].name;

                    };

                    reader.readAsDataURL(_files[0]);
                }

            });

        });

    },

    fileUploadMultiple: function () {

        var names = [];

        this.buttonUploadImgMultiple.unbind('click');
        this.buttonUploadImgMultiple.on('click', function () {
            $(this).closest('div').find('input[type=file]').trigger("click");

            //Variaveis de auxilio
            var idInput = $(this).closest('div').find('input[type=file]').attr('id');
            var srcImg = $(this).children();
            var base64 = "";
            var idUl = $(this).closest('div').find('ul');

            //Variaveis para verificacao de extensao e setar img default
            //var nameInput = $(this).next('input').attr('name');
            //var urlSrcImg = $('[name='+nameInput+']').closest('.form-group').find('[name='+nameInput+'Default]').val();
            //var typeImg = "";
            $('#' + idInput + '').unbind('change');
            $('#' + idInput + '').on('change', function () {

                for (var i = 0; i < $(this).get(0).files.length; ++i) {
                    names.push($(this).get(0).files[i].name);
                }

                var _files = this.files;

                if (_files && _files[0]) {

                    var reader = new FileReader();
                    var name = "";

                    reader.onload = function (e) {

                        var tamLi = idUl.find('li').size();
                        if (tamLi == 0) {

                            srcImg.attr('src', e.target.result);

                            //Pega somente a String Base64 e coloca da tag data-b64
                            base64 = e.target.result.split(',');
                            $('#' + idInput + '').data('b64', '' + base64[1] + '');

                            //Pega o nome do arquivo e inputa na UL de descricao de arquivos
                            name = $('#' + idInput + '').prop('files')[0].name;

                        }
                        //Add List to UL
                        if (idUl.size() != 0) {

                            //Add Class na div UL
                            $(idUl).addClass('dashed');
                            $(idUl).closest('div').addClass('box-upload-arq');
                            //Pega somente a String Base64 e coloca da tag data-b64
                            base64 = e.target.result.split(',');

                            var html = '';
                            html += '';
                            html += '<li>';
                            html += '<a href="#" data-b64="' + base64[1] + '">' + names[names.length - 1] + '</a>';
                            html += '<input type="hidden" name="formCriaClienteContratoNome" value="0">';
                            html += '<input type="hidden" name="formCriaClienteContratoIdAnexo" value="0">';
                            html += '<input type="hidden" name="formCriaClienteContratoDefault" value="0" />';
                            html += '<a href="#" class="btn-delete">';
                            html += '<i class="fa fa-trash fa-1" aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '">';
                            html += '</i>';
                            html += '</a>';
                            html += '</li>';

                            idUl.append(html);

                        } else {
                            $(idUl).removeClass('dashed');
                        }

                        APP.component.FileUpload.bind();

                    };

                    reader.readAsDataURL(_files[0]);
                }

            });


        });

    },

    fileUploadArquivo: function () {

        this.buttonUploadArquivo.unbind('click');
        this.buttonUploadArquivo.on('click', function () {
            $(this).closest('div').find('input[type=file]').trigger("click");

            //Variaveis de auxilio
            var idInput = $(this).closest('div').find('input[type=file]').attr('id');
            var srcImg = $(this);
            var base64 = "";

            $('#' + idInput + '').unbind('change');
            $('#' + idInput + '').on('change', function () {

                var _files = this.files;

                if (_files && _files[0]) {

                    var reader = new FileReader();
                    var name = "";

                    reader.onload = function (e) {

                        name = $('#' + idInput + '').prop('files')[0].name;
                        base64 = e.target.result.split(',');

                        var extencao = name.split('.')[1];

                        srcImg.children().text(name.substring(0, 8) + "..." + extencao);

                        var html = '<div class="DivExcluirAnexoEvidencia" style="float: left; margin-top: -50px">';
                        html += '<a href="#" class="btn-delete" onclick="DeleteAnexoAcaoImediata(this)">';
                        html += '<i class="fa fa-trash fa-1" aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '">';
                        html += '</i>';
                        html += '</div>';

                        srcImg.parent().append(html);

                        //Pega somente a String Base64 e coloca da tag data-b64
                        $('#' + idInput + '').data('b64', '' + base64[1] + '');


                    };

                    reader.readAsDataURL(_files[0]);
                }

            });

        });

    },

    delItemFileUpload: function () {

        this.buttonDelFileUpload.unbind('click');
        this.buttonDelFileUpload.on('click', function (event) {

            event.preventDefault();
            $(this).closest('li').remove();

        });

    },

    saveFileUpload: function (_idName, _url) {

        var xhr = new XMLHttpRequest();
        var fd = new FormData();
        fd.append("file", document.getElementById('form-cria-cliente-logo').files[0]);
        xhr.open("POST", "/Cliente/UploadArquivoLogo/", true);
        xhr.send(fd);
        xhr.addEventListener("load", function (result) {

        }, false);

    },

    fileUploadRai: function () {

        this.buttonUploadRai.unbind('click');
        this.buttonUploadRai.on('click', function () {
            $(this).closest('div').find('input[type=file]').trigger("click");

            //Variaveis de auxilio
            var idInput = $(this).closest('div').find('input[type=file]').attr('id');
            var NomeAnexo = $(this).closest('div').find('.NomeAnexo');
            var srcImg = $(this);
            var base64 = "";

            $('#' + idInput + '').unbind('change');
            $('#' + idInput + '').on('change', function () {

                var _files = this.files;

                if (_files && _files[0]) {

                    var reader = new FileReader();
                    var name = "";

                    reader.onload = function (e) {

                        name = $('#' + idInput + '').prop('files')[0].name;
                        base64 = e.target.result.split(',');

                        NomeAnexo.val(name);
                        srcImg.text('R.A.I');

                        var html = '';
                        html += '<a href="#" class="btn-delete-rai">';
                        html += '<i class="fa fa-trash fa-1" aria-hidden="true" title="' + _options.labelButtonExcluir + '" data-original-title="' + _options.labelButtonExcluir + '">';
                        html += '</i>';
                        html += '</a>';

                        srcImg.parent().append(html);

                        //Pega somente a String Base64 e coloca da tag data-b64
                        $('#' + idInput + '').data('b64', '' + base64[1] + '');

                        APP.component.FileUpload.bind();

                    };

                    reader.readAsDataURL(_files[0]);

                }

            });


        });

    },

    fileUpload2RaiDropCommon: function (fileInput, topoUpload, origcopia, arquivos) {
        var _files = arquivos;

        if (_files && _files[0]) {
            for (let ifile = 0; ifile < _files.length; ifile++) {
                let thisifile = _files[ifile];

                if (thisifile.size > 5 * 1024 * 1024) {
                    bootbox.alert("Erro: arquivo muito grande. Somente são permitidos anexos até 5 megabytes.");
                    continue;
                }
                if (thisifile.name.toLowerCase().indexOf(".exe") > 0 || thisifile.name.toLowerCase().indexOf(".bat") > 0 || thisifile.name.toLowerCase().indexOf(".com") > 0 || thisifile.name.toLowerCase().indexOf(".cmd") > 0) {
                    bootbox.alert("Erro: extensão não permitida. Não são aceitos arquivos .EXE, .COM, .CMD ou .BAT.");
                    continue;
                }

                let reader = new FileReader();
                let name = "";

                reader.onload = function (e) {
                    let copia = $(origcopia.cloneNode(true));
                    let NomeAnexo = copia.find("input[name='NomeAnexo']");
                    let NomeAnexo2 = copia.find('.noemarquivo');
                    let base64 = "";
                    let ConteudoAnexo = copia.find("input[name='ConteudoAnexo']");

                    name = thisifile.name;
                    base64 = e.target.result.split(',');

                    //Pega somente a String Base64 e coloca da tag data-b64
                    NomeAnexo.val(name);
                    NomeAnexo2.text(name);
                    ConteudoAnexo.data('b64', '' + base64[1] + '');

                    copia.insertAfter(topoUpload);

                    APP.component.FileUpload.bind();

                    $("." + fileInput.data("spannroarquivos")).text(topoUpload.parent().find("div:visible").length - 1);
                };

                reader.readAsDataURL(thisifile);
            }
        }
    },

    fileUpload2RaiDrop: function (raiz, arquivos) {

        //parcialmente duplciado com fileUpload2Rai
        //Variaveis de auxilio
        var fileInput = $(raiz).find('.raizdrop').find('input[type=file]');
        var topoUpload = $(raiz).find('.raizdrop');

        var origcopia = $(raiz).find('.raizdrop').find('.templatenovoarquivo').find("div")[0].cloneNode(true);

        APP.component.FileUpload.fileUpload2RaiDropCommon(fileInput, topoUpload, origcopia, arquivos);
    },

    fileUpload2Rai: function () {

        this.buttonUpload2Rai.unbind('click');
        this.buttonUpload2Rai.on('click', function (evt) {
            if ($(evt.target).closest(".modal-body").find(".botaouploadarquivos").prop('disabled')) {
                bootbox.alert("Upload de arquivos está desabilitado.");
                return;
            }

            $(this).closest('div').find('input[type=file]').first().trigger("click");

            //Variaveis de auxilio
            var fileInput = $(this).closest('div').find('input[type=file]');
            var topoUpload = $(this).closest('div');

            fileInput.unbind('change');
            fileInput.on('change', function () {

                var origcopia = $(this).closest('div').find('.templatenovoarquivo').find("div")[0].cloneNode(true);

                var _files = this.files;
                APP.component.FileUpload.fileUpload2RaiDropCommon(fileInput, topoUpload, origcopia, _files);
            });


        });

    },

    delItemFileUploadRai: function () {

        this.buttonDelFileUploadRai.unbind('click');
        this.buttonDelFileUploadRai.on('click', function (event) {

            var id = $(this).parent().find(".download-rai-form-auditoria-mes").attr("id");

            event.preventDefault();
            $("#" + id).parent().find('[class^=btn-upload]').text('');
            $(this).parent().find('input[type=file]').data('b64', '');
            $(this).parent().find('input[type=file]').attr('data-b64', '');

            $(this).parent().find('.IdAnexo').val('');
            $(this).parent().find('.NomeAnexo').val('');

            $("#" + id).attr("class", id);
            $("#" + id).removeAttr("href");
            $("#" + id).removeAttr("target");
            $("#" + id).removeAttr("download");
            $("#" + id).attr("onclick", "UploadArquivoRai(this);");


            $(this).closest('div').find('[class^=btn-upload]').html('<i class="fa fa-paperclip fa-1x" aria-hidden="true"></i> Anexar');
            $(this).remove();

        });

    },

    delItemFileUpload2Rai: function () {

        this.buttonDelFileUpload2Rai.unbind('click');
        this.buttonDelFileUpload2Rai.on('click', function (event) {
            event.preventDefault();
            if ($(event.target).closest(".modal-body").find(".botaouploadarquivos").prop('disabled')) {
                bootbox.alert("Exclusão de arquivos está desabilitada.");
                return;
            }
            $(this).parent().find("input[name='ApagarAnexo']").val(1);
            $(this).parent().hide();
            $("." + $(this).data("spannroarquivos")).text($(this).parent().parent().find("div:visible").length - 1);
        });

    },

    bind: function () {

        APP.component.FileUpload.setup();
        APP.component.FileUpload.delItemFileUpload();
        APP.component.FileUpload.delItemFileUploadRai();
        APP.component.FileUpload.delItemFileUpload2Rai();

    },

};

function FileUploadGlobal_getArrArquivo(_this, nomeChave1, nomeChave2) {

    let divArquivosSel = $(_this).data("divarquivos");
    return FileUploadGlobal_getArrArquivoRaiz(divArquivosSel, nomeChave1, nomeChave2)
}

function FileUploadGlobal_getArrArquivoRaiz(raiz, nomeChave1, nomeChave2) {

    let arquivos = $(raiz).find(".upload-arq");
    var arrAnexoAuditoria = new Array();

    for (let iarquivos = 0; iarquivos < arquivos.length; iarquivos++) {
        let arq = $(arquivos[iarquivos]);
        var nameImg = arq.find("input[name='NomeAnexo']").val();
        if (!nameImg)
            continue;
        if (nameImg == "") //skip temporary buttons
            continue;
        var id = arq.find("input[name='IdAnexo']").val();
        if (!id)
            id = 0;
        let ArquivoB64 = arq.find('.anexo-plai').data('b64');
        let Anexo = {
            IdAnexo: id,
            Extensao: nameImg,
            ArquivoB64: ArquivoB64,
        };
        let anexoAuditoria = {
            IdAnexo: id,
            Anexo: Anexo,
            ApagarAnexo: arq.find("input[name='ApagarAnexo']").val(),
        };
        anexoAuditoria[nomeChave1] = arq.find("input[name='IdArquivoPlaiAnexo']").val();
        anexoAuditoria[nomeChave2] = arq.find("input[name='IdPlai']").val();

        //se ainda nao existe e é para apagar, não colocamos
        let inserir = true;
        if (anexoAuditoria.ApagarAnexo != 0 && anexoAuditoria.ApagarAnexo != "0")
            if (anexoAuditoria.IdAnexo == "" || anexoAuditoria.IdAnexo == "0")
                inserir = false;

        if (inserir)
            arrAnexoAuditoria.push(anexoAuditoria);
    }
    return arrAnexoAuditoria;

}

/*
|--------------------------------------------------------------------------
| GestaoDeRiscoPartial
|--------------------------------------------------------------------------
*/

APP.component.GestaoDeRiscoPartial = {

    init: function (_divGestaoDeRisco, _temaSelected) {

        var page = APP.component.Util.getPage();

        this.setup();

        if (page == "CriarAnaliseCritica") {
            this.gestaoDeRiscoCriarAnaliseCritica(_divGestaoDeRisco, _temaSelected);
            //this.gestaoDeRisco(_selector);

        }
        if (page == "EditarAnaliseCritica") {
            this.gestaoDeRiscoEditAnaliseCritica();
        }

    },

    setup: function () {

        //this.buttonNome = $('');

    },

    gestaoDeRiscoCriarAnaliseCritica: function (_divGestaoDeRisco, _temaSelected) {


        this.getPartialGestaoDeRisco(_divGestaoDeRisco, _temaSelected);
        this.setCkeditorGestaoDeRisco(_divGestaoDeRisco, _temaSelected);
        this.setBarRating(_divGestaoDeRisco);

        this.setCriticidade();
        this.setERisco();
        this.getERisco();

        this.setInformarRisco();
        this.getInformarRisco();



        var numeroRisco = $('[name=numeroAC]').val();
        var busca = ".gestaoDeRiscoPartial-" + _temaSelected;
        $(busca).find($('[name=formGestaoDeRiscoNumero]')).val(numeroRisco);
        $('[name=numeroAC]').val(parseInt(numeroRisco) + parseInt('1'));



    },

    //GestÃ£o de Risco - Criar Analise Critica
    getPartialGestaoDeRisco: function (_divGestaoDeRisco, _temaSelected) {

        $.ajax({
            type: "POST",
            async: false,
            url: '/GestaoDeRisco/ObterPartialGestaoDeRisco',
            beforeSend: function () {
                APP.component.Loading.showLoading();
            },
            success: function (result) {
                _divGestaoDeRisco.html(result);
                _divGestaoDeRisco.find('[name=formGestaoDeRiscoRisco]').attr('name', 'formGestaoDeRiscoRisco-' + _temaSelected);
                _divGestaoDeRisco.find('[name=formInformarGestaoDeRiscoRisco]').attr('name', 'formInformarGestaoDeRiscoRisco-' + _temaSelected);
                APP.component.GestaoDeRiscoPartial.setHideAndShowGestaodeRisco(_divGestaoDeRisco);
            },
            error: function (result) {
                bootbox.alert(_options.MsgOcorreuErro);
            },
            complete: function (result) {
                APP.component.Loading.hideLoading();
            }
        });

    },

    setCkeditorGestaoDeRisco: function (_divGestaoDeRisco, _temaSelected) {

        $(_divGestaoDeRisco).find('textarea').first().attr('id', _temaSelected);
        var ckeditorID = $(_divGestaoDeRisco).find('textarea').first().attr('id');
        APP.component.CkEditor.init(ckeditorID);

    },

    setBarRating: function (_divGestaoDeRisco) {

        var divBarRating = $(_divGestaoDeRisco).find('.barraRating');
        APP.component.BarRating.setBarRatingGestaoDeRisco(divBarRating, 'bars-1to10');

    },

    setHideAndShowGestaodeRisco: function (_divGestaoDeRisco) {

        _divGestaoDeRisco.find('.responsavel-gestao-de-risco').hide();
        _divGestaoDeRisco.find('.numeroGestaoRisco').hide();
        _divGestaoDeRisco.find('.JustificativaGestaoDeRisco').hide();

        _divGestaoDeRisco.find('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').hide();
        _divGestaoDeRisco.find('.numeroGestaoRisco').hide();

    },

    //Changes
    setCriticidade: function (value, text) {

        //$('[name^=formGestaoDeRiscoCriticidade]').on('change', function () {
        //    APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(this);
        //});

    },


    setInformarRisco: function () {

        $('[name^=formInformarGestaoDeRiscoRisco]').unbind('change');
        $('[name^=formInformarGestaoDeRiscoRisco]').on('change', function () {

            var ERisco = APP.component.GestaoDeRiscoPartial.getInformarRisco(this);
            APP.component.GestaoDeRiscoPartial.setRulesInformarRisco(ERisco, this);

        });

    },



    //Auxiliares
    getInformarRisco: function (_this) {

        var ERisco = $(_this).val();
        return ERisco;

    },

    setERisco: function () {

        $('[name^=formGestaoDeRiscoRisco]').unbind('change');
        $('[name^=formGestaoDeRiscoRisco]').on('change', function () {

            var ERisco = APP.component.GestaoDeRiscoPartial.getERisco(this);
            APP.component.GestaoDeRiscoPartial.setRulesERisco(ERisco, this);

        });

    },

    setRulesInformarRisco: function (_ERisco, _this) {
        if (_ERisco == "true") {
            $(_this).parent().parent().parent().parent().parent().parent().find('[name=divEsconder]').show();
            APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(_this);
        } else {

            $(_this).parent().parent().parent().parent().parent().parent().find('[name=divEsconder]').hide();
        }

    },

    //Auxiliares
    getERisco: function (_this) {

        var ERisco = $(_this).val();
        return ERisco;

    },

    //Rules
    setRulesERisco: function (_ERisco, _this) {
        if (_ERisco == "true") {
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').show();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').show();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoNumero]').prop('disabled', true);
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').show();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').show();
            $(_this).parent().parent().parent().parent().parent().find('.numeroGestaoRisco').show();
            $(_this).parent().parent().parent().parent().parent().find('.JustificativaGestaoDeRisco').hide();
            APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(_this);
        } else {
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').hide();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').hide();
            //$(_thi').find('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').hide();
            $(_this).parent().parent().parent().parent().parent().find('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').hide();
            $(_this).parent().parent().parent().parent().parent().find('.JustificativaGestaoDeRisco').show();

        }

    },

    setRulesCriticidade: function (_barRatingSelect) {

        if (barRatingSelect == 2 || barRatingSelect == 3) {
            $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').show();
            $('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').show();
            $('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').show();
            $('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').show();
            APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(this);
        } else {
            $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').hide();
            $('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').hide();
            $('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').hide();
            $('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').hide();
        }

    },

    getRadioPossuiGestaoDeRisco: function () {

        $('[name^=formGestaoDeRiscoRisco]').unbind('change');
        $('[name^=formGestaoDeRiscoRisco]').bind('change', function () {

            var radioPossuiGestaoDeRisco = APP.component.Radio.init('formGestaoDeRiscoRisco');
            if (radioPossuiGestaoDeRisco == "true") {
                $(this).closest('#gestaoDeRisco').find('[name=InformacoesGestaoDeRisco]').show();
                $(this).closest('#gestaoDeRisco').find('.responsavel-gestao-de-risco').show();
                $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoCausa]').show();
                APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(this);

            } else {
                $(this).closest('#gestaoDeRisco').find('[name=InformacoesGestaoDeRisco]').hide();
                $(this).closest('#gestaoDeRisco').find('.responsavel-gestao-de-risco').hide();
                $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoCausa]').hide();
            }

        });

    },



    //GestÃ£o de Risco - Editar Analise Critica
    gestaoDeRiscoEditAnaliseCritica: function () {

        APP.component.BarRating.init('.barraRating', 'bars-1to10');

        $(('[id^=DsTexto]')).each(function (key, value) {

            var idDsTexto = $(this).attr('id');
            APP.component.CkEditor.init('#' + idDsTexto);
        });

        this.setDisableForm();

    },

    setDisableForm: function () {

        $('[name=options2]').prop("disabled", true);

    },

    setInfosEditGestaoDeRisco: function () {
        var selectRisco = $('.tema-cor-risco').val();
        //$('.rating').closest('div').next();data('');

    },

};


/*
|--------------------------------------------------------------------------
| Loading
|--------------------------------------------------------------------------
*/

APP.component.Calendar = {

    init: function () {

        this.setup();
    },

    setup: function () {

        $(".fa-calendar").click(function () {

            if ($(this).parent().parent().find("input").attr("disabled") == undefined) {
                $(this).parent().parent().find("input").datepicker("show");
            }


        });



    },
};

/*
|--------------------------------------------------------------------------
| Loading
|--------------------------------------------------------------------------
*/

APP.component.Loading = {

    init: function () {

        this.setup();
        this.hideLoading();

    },

    setup: function () {

        //

    },

    showLoading: function () {

        $(".loading").show().focus();

    },

    hideLoading: function () {

        $(".loading").hide();

    },

};
/*
|--------------------------------------------------------------------------
| Lobi Panel
|--------------------------------------------------------------------------
*/

APP.component.AtivaLobiPanel = {

    init: function () {

        this.ativaLobiPanel();

    },

    ativaLobiPanel: function () {

        $('.panel').lobiPanel({
            editTitle: false,
            unpin: false,
            reload: false,
            close: false,
            expand: false,
            minimize: {
                icon: 'fa fa-minus',
                icon2: 'fa fa-plus',
                tooltip: _options.Minimiza
            },
            ////expand: {
            ////    icon: 'fa fa-expand',
            ////    icon2: 'fa fa-compress',
            ////    tooltip: _options.TelaCheia
            ////},
            close: false,
            toggleIcon: "caret"
        });

    },

};

/*
|--------------------------------------------------------------------------
| Máscaras
|--------------------------------------------------------------------------
*/
APP.component.Mascaras = {

    init: function () {

        this.mascara();
        this.mascaraCelular();


    },

    mascara: function () {

        $('.input-cpf').mask('000.000.000-00', { reverse: true });
        $('.input-telefone').mask('(00) 0000-0000');
        $('.input-cep').mask('00000-000');
        $('.input-data').mask('00/00/0000');
        $('.input-cnpj').mask('00.000.000/0000-00');
        //".mask").inputmask('Regex', { regex: "^[0-9]{1,6}(\\.\\d{1,2})?$" });
        $('.input-metrica').mask("#.##0,000", { reverse: true });
    },

    mascaraCelular: function () {

        $('.input-celular').focusout(function () {

            var elemento, elementoThis; elementoThis = $(this);
            elementoThis.unmask(); elemento = elementoThis.val().replace(/\D/g, "");

            var mascara = '(00) 0000-00000';
            var placeholder = '(__) ____-_____';

            elementoThis.mask(mascara, {
                placeholder: placeholder,
            });

        }).trigger("focusout");

    },

    mascaraDinheiro: function () {

        $(".input-money").maskMoney({
            prefix: 'R$ ', allowNegative: true,
            thousands: '.',
            decimal: ',',
            affixesStay: false
        });

    }




};

/*
|--------------------------------------------------------------------------
| Menu
|--------------------------------------------------------------------------
*/

APP.component.Menu = {

    init: function () {
        this.setup();
        this.abrirMenu();
        this.fecharMenu();
    },

    setup: function () {

        this.OpenMenu = $('.menu-lateral');
        this.CloseMenu = $('.close-menu-lateral');

    },

    abrirMenu: function () {

        this.OpenMenu.on('click', function () {
            $("#mySidenav").css("width", "250px");
            $("#main").css("marginLeft", "250px");
        });



    },

    fecharMenu: function () {

        this.CloseMenu.on('click', function () {
            $("#mySidenav").css("width", "0px");
            $("#main").css("marginLeft", "0px");

        });



    },

};
/*
|--------------------------------------------------------------------------
| Menu Side Bar
|--------------------------------------------------------------------------
*/

APP.component.MenuSideBar = {

    init: function () {

        this.setup();
        this.menuSideBar();

    },

    setup: function () {

        //Botao de trocar a imagem
        //this.buttonUploadImg = $("[class^=btn-upload-img]");

    },

    menuSideBar: function () {

        var trigger = $('.hamburger'),
            overlay = $('.overlay'),
            isClosed = false;

        trigger.click(function () {
            hamburger_cross();
        });

        function hamburger_cross() {

            if (isClosed == true) {
                overlay.hide();
                trigger.removeClass('is-open');
                trigger.addClass('is-closed');
                isClosed = false;
                $('.wrapper-toggle').removeClass('toggled');
            } else {
                overlay.show();
                trigger.removeClass('is-closed');
                trigger.addClass('is-open');
                isClosed = true;
                $('.wrapper-toggle').addClass('toggled');
            }
        }

    },


};
/*
|--------------------------------------------------------------------------
| User Menu
|--------------------------------------------------------------------------
*/

APP.component.UserMenu = {

    init: function () {

        var page = APP.component.Util.getPage();

        if (page != "Login") {

            this.setup();
            this.user();
            //this.sitesPorCliente();
            this.clientes();
            //this.sitesPorCoordenador();
            this.setBoxClientes();
            this.setSitesCoordenador();
        }
    },

    setup: function () {

        this.openCliente = $('#nav-user-clientes');
        this.closeCliente = $('#close-user-clientes');
        this.openSites = $('.box-clientes');
        this.closeSites = $('#close-user-sites');
        this.openSitesCoordenador = $('#nav-user-site');

    },

    user: function () {

        this.closeCliente.on('click', function () {
            $('#clientes').slideUp();
        });

        this.openSites.on('click', function () {
            $('#clientes').slideUp();
            $('#sites').slideDown();
        });

        this.closeSites.on('click', function () {
            $('#sites').slideUp();
        });

        this.openSitesCoordenador.on('click', function () {
            $('#sites').slideDown();
        });

    },

    sitesPorCliente: function (cliente) {
        $.ajax({
            type: "GET",
            async: false,
            dataType: 'json',
            url: "/Site/ObterSitesPorCliente",
            data: { idCliente: cliente },
            success: function (result) {

                var $siteBox = $("#sites .row-box");

                $siteBox.html("");

                $.each(result, function (key, val) {

                    var htmlSites = '';
                    var img = "";

                    if (val.SiteLogoAux.Extensao != null) {
                        img = "data:image/" + val.SiteLogoAux.Extensao + ";base64," + val.SiteLogoAux.ArquivoB64;
                    } else {
                        img = val.SiteLogoAux.ArquivoB64;
                    }

                    document.cookie = "siteSelecionado=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
                    document.cookie = "clienteSelecionado=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

                    htmlSites += '<!-- Site - ' + val.NmFantasia + ' -->';
                    htmlSites += '<div class="col-md-2">';
                    htmlSites += '<div class="box-clientes">';
                    htmlSites += '<a href="/Site/EscolheSite/?idSite=' + val.IdSite + '">';
                    htmlSites += '<img src="' + img + '" class="img-responsive">';
                    htmlSites += '<span>' + val.NmFantasia + '</span></a>';
                    htmlSites += '</a>';
                    htmlSites += '</div>';
                    htmlSites += '</div>';
                    $siteBox.append(htmlSites);
                });
            },
        });
        $('#clientes').slideUp();
        $('#sites').slideDown();
    },

    clientes: function () {

        this.openCliente.on('click', function () {

            var tamanho = $("#clientes .box-clientes").size();
            var url = window.location.href;
            var $siteBox = $("#sites .row-box");

            $siteBox.html("");
            if (tamanho == 0) {

                $.ajax({
                    type: "GET",
                    async: true,
                    dataType: 'json',
                    url: "/Cliente/ObterClientesUsuario",
                    beforeSend: function () {
                        APP.component.Loading.showLoading();
                    },
                    success: function (result) {

                        var $clienteBox = $("#clientes .row-box");

                        var index = 1;

                        $.each(result, function (key, val) {

                            var htmlCliente = '';
                            var img = "";

                            if (val.ClienteLogoAux.Extensao != null) {
                                img = "data:image/" + val.ClienteLogoAux.Extensao + ";base64," + val.ClienteLogoAux.ArquivoB64;
                            } else {
                                img = val.ClienteLogoAux.ArquivoB64;
                            }

                           
                            htmlCliente += '<!-- Site - ' + val.NmFantasia + ' -->';
                            htmlCliente += '<div class="col-md-2">';
                            htmlCliente += '<div class="box-clientes">';
                            htmlCliente += '<a href="/Site/EscolheSite/?idSite=' + val.Site.IdSite + '">';
                            htmlCliente += '<img src="' + img + '" class="img-responsive">';
                            htmlCliente += '<span>' + val.NmFantasia + '</span></a>';
                            htmlCliente += '</a>';
                            htmlCliente += '</div>';
                            htmlCliente += '</div>';


                            if (result.length == index) {
                                htmlCliente += '<!-- Sair -->';
                                htmlCliente += '<div class="col-md-2">';
                                htmlCliente += '<div class="box-clientes">';
                                htmlCliente += '<a href="/Site/EscolheSite/?idSite=0" class="get-site-cliente">';
                                htmlCliente += '<img src="../../../img/sair.png" class="img-responsive">';
                                htmlCliente += '<span><input type="hidden" class="idCliente" value="0">' + _options.Sair + '</span></a>';
                                htmlCliente += '</div>';
                                htmlCliente += '</div>';

                            }

                            $clienteBox.append(htmlCliente);

                            index++;

                        });

                        APP.component.UserMenu.setBoxClientes();

                        $('#clientes').slideDown();
                        $("#modal-panel-clientes-sites").modal();




                    },
                    error: function (result) {
                        erro = APP.component.ResultErros.init(result.Erro);
                        bootbox.alert(erro);
                        APP.component.Loading.hideLoading();
                    },
                    complete: function (result) {
                        APP.component.Loading.hideLoading();

                    }
                });

            }
            else {
                $("#modal-panel-clientes-sites").modal();
                $('#clientes').slideDown();
            }
        });

    },

    setBoxClientes: function () {
        $('.get-site-cliente').unbind('click');
        $('.get-site-cliente').bind('click', function (e) {

            var id = $(this).find(".idCliente").val();

            APP.component.UserMenu.getEscolheCliente(id);

        });
    },

    getEscolheCliente: function (_id) {

        $.ajax({
            type: "GET",
            async: false,
            dataType: 'json',
            data: { "idCliente": _id },
            url: "/Cliente/EscolheCliente/",
            success: function (result) {

            },
        });
        APP.component.UserMenu.sitesPorCliente(_id);
        //APP.component.UserMenu.setImagemCliente(_id);

    },

    setSitesCoordenador: function () {
        this.openSitesCoordenador.unbind("click");
        this.openSitesCoordenador.bind("click", function () {
            $("#modal-panel-clientes-sites .modal-title").html("Sites");
            APP.component.UserMenu.sitesPorCliente();
        });
    },

};

/*
|--------------------------------------------------------------------------
| Modal
|--------------------------------------------------------------------------
*/

APP.component.Modal = {

    init: function () {

        this.fecharModal();
        this.abrirModal();

    },

    fecharModal: function () {

        $('.modal-close').on('click', function () {
            $('.modal-box').hide();
            $('.modal').hide();
        });

    },

    abrirModal: function (titulo, texto) {

        $('.modal').fadeIn();
        $('.modal-box').fadeIn();
        $('.modal-title').text(titulo);
        $('.modal-text').text(texto);

    },

    alert: function (_title, _text) {

        $('#modal-alert .modal-title').text(_title);
        $('#modal-alert .modal-body').text(_text);
        $("#modal-alert").modal('show');

    }

};


/*
|--------------------------------------------------------------------------
| Radio Sim Não
|--------------------------------------------------------------------------
*/

APP.component.Radio = {

    init: function (_nomeRadio) {

        this.setup();
        return this.getRadio(_nomeRadio);

    },

    setup: function () {

        //

    },

    getRadio: function (_nomeRadio) {
        var radio_val = $("input[name='" + _nomeRadio + "']:checked").val();
        return radio_val;
    },

};
/*
|--------------------------------------------------------------------------
| Radio Sim Não
|--------------------------------------------------------------------------
*/

APP.component.RadioSimNao = {

    init: function (_nomeRadio) {

        this.setup();
        return this.getRadio(_nomeRadio);

    },

    setup: function () {

        //

    },

    getRadio: function (_nomeRadio) {
        var radio_val = $("input[name='" + _nomeRadio + "']:checked").val();
        return radio_val;
    },

};
/*
|--------------------------------------------------------------------------
| Notas / Avaliação
|--------------------------------------------------------------------------
*/

APP.component.Rating = {

    init: function () {

        this.nota();

    },

    enviarNota: function () {

        $("#rateYo").rateYo({
            starWidth: "20px",
            fullStar: true,
            rating: $('#nota-usuario').text(),
            onSet: function (rating) {

                obj = {
                    idConteudo: $('#conteudoID').text(),
                    nota: rating
                };

                $.ajax({
                    type: "POST",
                    async: false,
                    dataType: 'json',
                    url: tratarUrl("/Avaliacao/Avaliar"),
                    data: obj,
                });
            }
        });

    },

};
/*
|--------------------------------------------------------------------------
| Remove Acentos
|--------------------------------------------------------------------------
*/

APP.component.RemoveAcentos = {

    init: function (_word) {

        this.setup();
        return this.setRemoveAcentos(_word);

    },

    setup: function (_buton) {

        //

    },

    setRemoveAcentos: function (_word) {

        var string = _word;
        var mapaAcentosHex = {
            a: /[\xE0-\xE6]/g,
            e: /[\xE8-\xEB]/g,
            i: /[\xEC-\xEF]/g,
            o: /[\xF2-\xF6]/g,
            u: /[\xF9-\xFC]/g,
            c: /\xE7/g,
            n: /\xF1/g,
            '-': /\s/g
        };

        for (var letra in mapaAcentosHex) {
            var expressaoRegular = mapaAcentosHex[letra];
            string = string.replace(expressaoRegular, letra);
        }

        return string;

    },

};
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
/*
|--------------------------------------------------------------------------
| SelectAll
|--------------------------------------------------------------------------
*/

APP.component.SelectAll = {

    init: function (_selector) {

        this.setup();
        this.setSelectAll(_selector);

    },

    setup: function () {

        //

    },

    setSelectAll: function (_selector) {

        $("[name^='" + _selector + "']").click(function () {
            $(this).closest('.tabela-check').find('input:checkbox').not(this).prop('checked', this.checked);
        });

    },

};
/*
|--------------------------------------------------------------------------
| Compare Select List
|--------------------------------------------------------------------------
*/

APP.component.SelectListCompare = {

    init: function (_listResult, _listPage, _idSelect, _paramCompare, _paramTexto) {

        this.setup();
        this.selectList(_listResult, _listPage, _idSelect, _paramCompare, _paramTexto);

    },

    setup: function () {

        //

    },

    selectList: function (_listResult, _listPage, _idSelect, _paramCompare, _paramTexto) {
        var array = _listResult;
        var anotherOne = [];
        var obj = {};

        if (_listPage.val() == undefined)
            _listPage = [0];

        $(_listPage).each(function (key, value) {


            obj = { [_paramCompare.valueOf()]: parseInt($(value).val()) };
            anotherOne.push(obj);

        });

        var filteredArray = array.filter(myCallBack);

        function myCallBack(el) {
            return anotherOne.findIndex(x => x[_paramCompare] == el[_paramCompare]) < 0;
        }

        this.addSelectOnPage(filteredArray, _idSelect, obj, _paramTexto);

    },

    myCallBack: function (el) {

        return anotherOne.indexOf(el) < 0;

    },

    addSelectOnPage: function (filteredArray, _idSelect, obj, _paramTexto) {


        $(filteredArray).each(function (key, value) {

            var keyName = Object.keys(obj)[0];
            var $option = $('<option>');
            $(_idSelect.selector).append($option.val(value[keyName]).text(value[_paramTexto.valueOf()]));
            $(_idSelect).append($option.val(value[keyName]).text(value[_paramTexto.valueOf()]));

        });

    },

};

/*
|--------------------------------------------------------------------------
| Slider
|--------------------------------------------------------------------------
*/

APP.component.Slider = {

    init: function () {

        this.sliderPrincipal();


    },

    sliderPrincipal: function () {

        $('.bx-slider').bxSlider({
            auto: true,
            speed: 1000,
            onSliderLoad: function () {
                $(".slider").css("visibility", "visible");
            }
        });

    },

};

/*
|--------------------------------------------------------------------------
| Strong Password
|--------------------------------------------------------------------------
*/

APP.component.StrongPassword = {

    init: function (_button) {

        this.setup(_button);
        this.setStrongPassword(_button);
        this.setRuleStrongPassword();

    },

    setup: function (_button) {

        this.buttonChange = $("[name='" + _button + "']");

    },

    setStrongPassword: function (_button) {
        this.buttonChange.on('input', function () {

            var pwd = $("[name='" + _button + "']").val();

            var array = [];
            //Zero or more uppercase characters
            array[0] = pwd.match(/[A-Z]/);
            //Zero or more lowercase characters
            array[1] = pwd.match(/[a-z]/);
            //Zero or more decimal digits
            array[2] = pwd.match(/\d/);
            //Zero or more non-word characters (!, Â£, $, %, etc.)
            array[3] = pwd.match(/[!_.-]/);

            var sum = 0;
            for (var i = 0; i < array.length; i++) {
                sum += array[i] ? 1 : 0;
            }

            var label = APP.component.StrongPassword.getLabelStrongPassword();

            switch (sum) {
                case 0:
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[0]);
                    break;
                case 1:
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[1]);
                    break;
                case 2:
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[2]);
                    break;
                case 3:
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[3]);
                    break;
                case 4:
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[4]);
                    break;
                default:
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[0]);
                    break;
            }

        });

    },

    getLabelStrongPassword: function () {

        var arrayLabelStrongPassword = [];
        var strongPassword = $('input[name^=lblStrongPassword]');

        strongPassword.each(function (i) {

            arrayLabelStrongPassword[i] = $(this).val();

        });

        return arrayLabelStrongPassword;

    },

    setHtmlStrongPassword: function (e, _info) {

        var classe = APP.component.RemoveAcentos.init(_info).toLowerCase();

        var html = "";
        $('.pwd-validate').remove();
        html += '<div class="pwd-validate">';
        html += '<div class="pwd-validate-info">';
        html += '<span class="pwd-validate-info-' + classe + '">' + _info + '</span>';
        html += '</div>';
        html += '<div class="pwd-validate-bar pwd-validate-bar-' + classe + '">';
        html += '</div>';
        html += '</div>';

        $(e).closest('.form-group').append(html);

    },

    setRuleStrongPassword: function () {

        $.validator.addMethod("strongPassword", function () {
            var statusForce = APP.component.StrongPassword.getStrongPasswordValue();
            return ($('[name=formAlterarSenhaUtilizaSenhaForte]').val() == statusForce);
        });

    },

    getStrongPasswordValue: function () {

        var force = $('span[class^=pwd-validate-info-]').attr('class');
        force = force.split('info-');
        var statusForce = force[1];

        if (statusForce == "forte" || statusForce == "muito-forte") {
            return "true";
        } else {
            return "false";
        }

    },

};
/*
|--------------------------------------------------------------------------
| TINYMCE EDITOR
|--------------------------------------------------------------------------
*/

APP.component.TinyMCE = {

    init: function (_selector) {
        this.start(_selector);
    },

    start: function (_selector) {
        tinymce.init({
            selector: _selector,
            height: 200,
            width: 500,

            menubar: false,
            //plugins: [
            //  'advlist autolink lists link image charmap print preview anchor',
            //  'searchreplace visualblocks code fullscreen',
            //  'insertdatetime media table contextmenu paste code'
            //],
            toolbar: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
            content_css: [
                '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
                '//www.tinymce.com/css/codepen.min.css']
        });
    },



};

/*
|--------------------------------------------------------------------------
| Tracking
|--------------------------------------------------------------------------
*/

APP.component.Tracking = {

    init: function () {

        this.trackUrl();
        this.trackEventos();
        this.trackAudio();

    },

    // Envia URL para trackear

    trackUrl: function () {

        obj = {
            descricaoNavegacao: window.location.href
        };


        $.ajax({
            type: "POST",
            async: false,
            dataType: 'json',
            url: tratarUrl("/Registro/Navegacao"),
            data: obj
        });

    },

    // Envia o track de cada evento

    trackEventos: function () {

        var tipoEvento = $('[data-tipoevento]');

        tipoEvento.on('click', function (event) {

            obj = {
                idConteudo: $('#conteudoID').text(),
                idTipoEvento: $(this).data("tipoevento")
            };

            $.ajax({
                type: "POST",
                async: false,
                dataType: 'json',
                url: tratarUrl("/Registro/Evento"),
                data: obj
            });


        });

    },

    // Envia o track de play de audio

    trackAudio: function () {
        var player = $("#player");

        player.on("play", function () {
            obj = {
                idConteudo: $('#conteudoID').text(),
                idTipoEvento: $(this).data("tipoevento")
            };

            $.ajax({
                type: "POST",
                async: false,
                dataType: 'json',
                url: tratarUrl("/Registro/Evento"),
                data: obj
            });

        });
    },





};

/*
|--------------------------------------------------------------------------
| Upload Files
|--------------------------------------------------------------------------
*/

APP.component.UploadFiles = {

    init: function () {

        this.setup();

    },

    setup: function () {

        //

    },

    setUploadFiles: function (urlToUpload, IdCtrlDrop, IdCtrlUpload, nomeCampo, maxSize, formJson, idForm, idRegistro, urlDelete) {

        $('#' + IdCtrlDrop + ' a').click(function () {
            // Simulate a click on the file input button
            // to show the file browser dialog
            $(this).parent().find('input').click();
        });

        // Initialize the jQuery File Upload plugin
        $('#' + IdCtrlUpload).fileupload({
            url: urlToUpload,
            sequentialUploads: true,
            limitMultiFileUploads: 1,
            formData: formJson,
            // This element will accept file drag/drop uploading
            dropZone: $('#' + IdCtrlDrop),

            // This function is called when a file is added to the queue;
            // either via the browse button, or via drag/drop:
            add: function (e, data) {
                if (data.files[0].size > maxSize) {
                    bootbox.alert(_options.MsgValidaUploadArquivoUnico + formatFileSize(maxSize));
                }
                else {
                    var tpl = $('<li class="working"><p></p><span></span></li>');

                    // Append the file name and file size
                    tpl.find('p').text(data.files[0].name);

                    // Add the HTML to the UL element
                    var ul = $('#' + IdCtrlUpload + ' ul');
                    data.context = tpl.appendTo(ul);

                    // Listen for clicks on the cancel icon
                    tpl.find('span').click(function () {

                        if (!tpl.hasClass('working')) {
                            jqXHR.abort();
                        }

                        tpl.fadeOut(function () {
                            tpl.remove();
                        });
                    });

                    // Automatically upload the file once it is added to the queue
                    var jqXHR = data.submit();
                }
            },

            progress: function (e, data) {
                // Calculate the completion percentage of the upload
                var progress = parseInt(data.loaded / data.total * 100, 10);

                // Update the hidden input field and trigger a change
                // so that the jQuery knob plugin knows to update the dial
                data.context.find('p').html(progress + " %" + "  - " + data.files[0].name);;

                if (progress == 100) {
                    data.context.find('p').html(data.files[0].name);
                }
            },
            done: function (e, data) {
                if (data.textStatus == "success") {
                    var tipo = formJson.tipo;
                    var _a = "<a class='arquivo' href='" + data.result + "' target='_blank'>" + data.files[0].name + "</a>";
                    _a += "<a href=\"javascript:;\" class=\"deletefile\" target=\"_blank\" data-tipo=\"" + tipo + "\" data-arquivo=\"" + data.files[0].name + "\" data-campo=\"" + nomeCampo + "\" data-id=\"" + idRegistro + "\"><i class=\"fa fa-trash-o\" aria-hidden=\"true\" title='" + _options.labelButtonExcluir + "' data-original-title='" + _options.labelButtonExcluir + "'></i></a>";

                    data.context.find('p').html(_a);
                    $("#" + IdCtrlUpload).find(".working").removeClass("working");
                    $("#" + idForm).find("#" + nomeCampo).val(data.files[0].name + "|" + $("#" + nomeCampo).val());

                    var validator = $("#" + idForm).validate();
                    validator.element("#" + nomeCampo);

                    UploadDeleteFiles(urlDelete);
                }
            },
            fail: function (e, data) {
                // Something has gone wrong!
                bootbox.alert(data.errorThrown);
            }
        });

    },

    ativaFileUpload: function () {

        //// Prevent the default action when a file is dropped on the window
        $(document).on('drop dragover', function (e) {
            e.preventDefault();
        });
        $(document).on('drop dragenter', function (e) {
            e.preventDefault();
            if (event.target.className == "drop") {
                event.target.style.background = "#dcdcdc";
            }
        });
        $(document).on('drop dragleave', function (e) {
            e.preventDefault();
            if (event.target.className == "drop") {
                event.target.style.background = "";
            }
        });

    },

};
/*
|--------------------------------------------------------------------------
| Utils
|--------------------------------------------------------------------------
*/

APP.component.Util = {

    init: function () {

    },

    setup: function () {




    },

    tratarUrl: function (url) {

        var ambiente = urlBase.substring(0, urlBase.length - 1);

        var ret = ambiente + url;
        return ret;
    },

    getUrlAmbiente: function () {

        var url = (window.location.href);
        var protocol = window.location.protocol;
        var host = window.location.host;
        var pathname = window.location.pathname;
        var newURL = protocol + "//" + host;
        return newURL;

    },

    getController: function () {

        var controller = $('meta[name=controller]').attr('content');
        return controller == null || controller == undefined ? "" : controller;

    },

    getPage: function () {

        var page = $('meta[name=page]').attr('content');
        return page ? page : false;

    }
};
/*
|--------------------------------------------------------------------------
| Validate Form
|--------------------------------------------------------------------------
*/

APP.component.ValidateForm = {

    init: function (_objFormValidade, _formName) {

        this.setup();
        this.validateForm(_objFormValidade, _formName);

    },

    setup: function () {

        this.cpfIsValid();
        this.cnpjIsValid();
        this.verifyTableAcaoImediata();

    },

    validateForm: function (_objFormValidade, _formName) {

        var page = APP.component.Util.getPage();

        $(_formName).validate({
            ignore: ".ignore",
            rules: _objFormValidade,
            messages: {

            },
            errorElement: "em",
            errorPlacement: function (error, element) {

                //Verifica se tem grupo de checkbox
                var hasTableCheck = $(element).closest('.form-group').find('.tabela-check').size();


                // Add the 'help-block' class to the error element
                error.addClass("help-block");
                //error.addClass( "arrow_box " );
                //console.log(error);

                // Add 'has-feedback' class to the parent div.form-group
                // in order to add icons to inputs
                element.closest(".form-group").addClass("has-feedback");
                //console.log(element);


                if (element.prop("type") === "checkbox") {
                    if (hasTableCheck != 0) {
                        element.closest(".form-group").find('.tabela-check').addClass("has-feedback");
                        error.insertAfter(element.closest('.form-group').children().last());
                    } else {
                        error.insertAfter(element.closest('.form-group').children());
                    }

                } else if (element.prop("type") === "radio") {
                    //console.log('radio');
                    error.insertAfter(element.closest('.form-group').children().last());
                } else {
                    //console.log('normal');
                    if (page == "Login" || page == "RecuperarSenha" || page == "AlterarSenhaLogin") {
                        error.insertAfter(element.closest('.form-group').children().first());
                    } else if (element.hasClass('data')) {
                        error.insertAfter(element.closest('.form-group').children().last());
                    } else {
                        error.insertAfter(element);
                    }
                }


                // Add the span element, if doesn't exists, and apply the icon classes to it.
                if (!element.next("span")[0]) {

                    if (hasTableCheck != 0) {
                        $("<span class='glyphicon glyphicon-remove form-control-feedback'></span>").insertAfter(element.closest('.tabela-check'));
                    } else {
                        $("<span class='glyphicon glyphicon-remove form-control-feedback'></span>").insertAfter(element);
                    }
                }
            },
            success: function (label, element) {
                // Add the span element, if doesn't exists, and apply the icon classes to it.
                if (!$(element).next("span")[0]) {
                    $("<span class='glyphicon glyphicon-ok form-control-feedback'></span>").insertAfter($(element));
                } else if (!$(element).next("span")[0]) {
                    $("<span class='glyphicon glyphicon-ok form-control-feedback'></span>").insertAfter($(element));
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).closest('.form-group').addClass("has-error").removeClass("has-success");
                $(element).next("span").addClass("glyphicon-remove").removeClass("glyphicon-ok");
                //$( element ).closest(".form-group").find( "em" ).addClass( "arrow_box" );
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).closest(".form-group").addClass("has-success").removeClass("has-error");
                $(element).next("span").addClass("glyphicon-ok").removeClass("glyphicon-remove");
                //$( element ).closest(".form-group").find( "em" ).removeClass( "arrow_box" );
            }

        });

    },

    verifyGroupValidateChecked: function (_nameGroup) {

        $.validator.addMethod("groupValidateChecked ", function () {
            return ($('[name=' + _nameGroup + ']:checked').size() != 0);
        });

    },

    cpfIsValid: function () {

        $.validator.addMethod("cpf", function (value, element) {
            value = jQuery.trim(value);

            value = value.replace('.', '');
            value = value.replace('.', '');
            cpf = value.replace('-', '');
            while (cpf.length < 11) cpf = "0" + cpf;
            var expReg = /^0+$|^1+$|^2+$|^3+$|^4+$|^5+$|^6+$|^7+$|^8+$|^9+$/;
            var a = [];
            var b = new Number();
            var c = 11;
            for (i = 0; i < 11; i++) {
                a[i] = cpf.charAt(i);
                if (i < 9) b += (a[i] * --c);
            }
            if ((x = b % 11) < 2) { a[9] = 0 } else { a[9] = 11 - x }
            b = 0;
            c = 11;
            for (y = 0; y < 10; y++) b += (a[y] * c--);
            if ((x = b % 11) < 2) { a[10] = 0; } else { a[10] = 11 - x; }

            var retorno = true;
            if ((cpf.charAt(9) != a[9]) || (cpf.charAt(10) != a[10]) || cpf.match(expReg)) retorno = false;

            return this.optional(element) || retorno;

        });

    },

    cnpjIsValid: function () {

        $.validator.addMethod("cnpj", function (value, element) {
            cnpj = value.replace(/[^\d]+/g, '');
            if (cnpj == '') return false;
            if (cnpj.length != 14)
                return false;
            // Elimina CNPJs invalidos conhecidos
            if (cnpj == "00000000000000" ||
                cnpj == "11111111111111" ||
                cnpj == "22222222222222" ||
                cnpj == "33333333333333" ||
                cnpj == "44444444444444" ||
                cnpj == "55555555555555" ||
                cnpj == "66666666666666" ||
                cnpj == "77777777777777" ||
                cnpj == "88888888888888" ||
                cnpj == "99999999999999")
                return false;

            // Valida DVs
            tamanho = cnpj.length - 2;
            numeros = cnpj.substring(0, tamanho);
            digitos = cnpj.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0))
                return false;

            tamanho = tamanho + 1;
            numeros = cnpj.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;

            return true;

        });

    },


    //Nao Conformidade
    //Tabela de AÃ§Ã£o imediata

    verifyTableAcaoImediata: function () {

        $.validator.addMethod("verifyTableAcaoImediata ", function () {
            console.log('opa')
            return $('[name=formAcaoImadiataJustificativa]').val() != "" ? true : false;
        });

    },

    modelMsgErro: function () {

        //

    },

};
