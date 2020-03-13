var _token = "";
var _options = {
    labelButtonYes: "Sim",
    labelButtonNo: "Não",
    labelButtonCancel: "Cancelar",
    TitleDelete: "Confirmação",
    TitleErro: "Aviso",
    TitleSuccess: "Aviso",
    TitleErroFormulario: "O formulário possui os seguintes erros:",
    messageDelete: "Confirma a exclusão do projeto <strong>{{0}}}</strong>?",
    MessageErro: "O seguinte erro ocorreu ao tentar atualizar os dados.<br>{{0}}",
    RegistroExlcuido: "Registro excluído com sucesso",
    verificationToken: "[name=__RequestVerificationToken]",
    tabelaLengthMenu: "_MENU_ registros por página",
    tabelaZeroRecords: "Nenhum registro encontrado",
    tabelaInfo: "Mostrando página _PAGE_ de _PAGES_",
    tabelaInfoEmpty: "Sem registros",
    tabelaInfoFiltered: "(filtrando de _MAX_ registros)",
    tabelaSearch: "",
    tabelaSearchPlaceholder: "Busca",
    tabelaPaginateNext: "Próximo",
    tabelaPaginatePrevious: "Anterior", 
    LicencaExcluida: "Licença excluida com sucesso"

};



$(function () {
    _token = $(_options.verificationToken).val();
    $("body").bind("resize", function () {
        SplashResize();
    })
    TitleFromPlaceholder();
    SplashResize();
    //$('a[href*="#"]:not([href="#"])').click(function () {
    //    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
    //        var target = $(this.hash);
    //        target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
    //        if (target.length) {
    //            $('html,body').animate({
    //                scrollTop: target.offset().top
    //            }, 1000);
    //            return false;
    //        }
    //    }
    //});

});

function AtivaDataPicker() {
    jQuery.datetimepicker.setLocale(jsIdioma);
    $('.data:not([readonly])').datetimepicker({
        format: 'd/m/Y',
        lang: jsIdioma,
        timepicker: false,
        scrollInput: false
    });
    $(".data").attr("autocomplete", "off");
    $(".data").g2itMasks("date");
}

function DefineMaxlengthTextArea() {
    $(function () {
        $('textarea[maxlength]').keyup(function () {
            var limit = parseInt($(this).attr('maxlength'));
            var text = $(this).val();
            var chars = text.length;
            if (chars > limit) {
                var new_text = text.substr(0, limit);
                $(this).val(new_text);
            }
        });
    });
}

function IsValidFrom(frm, CustomValidate) {
    LimpaErros(frm);
    var EhValido = $("#" + frm).valid();
    if (EhValido && CustomValidate != undefined && CustomValidate != "") {
        eval("EhValido = " + CustomValidate);
    }
    if (!EhValido) {
        if (TemErro(frm)) {
            var erros = $("#" + frm).find("#containerError").html();
            bootbox.alert({ message: erros, title: _options.TitleErroFormulario, });
        }
        return false;
    }
    return true;
}

function TemErro(frm) {
    var containerErro = $("#" + frm).find(".validation-summary-errors");
    var list = containerErro.find("ul");
    var erro = containerErro.find("ul").find("li:first").html();
    return list && list.length > 0 && erro != "";
}

function AddValidationDynamic() {
    $.validator.unobtrusive.parseDynamicContent('form');
}

function ResetUnobtrusiveValidator(bForce) {
    if (bForce != undefined && bForce) {
        $("form").removeData("validator");
        $("form").removeData("unobtrusiveValidation");
    }
    $.validator.unobtrusive.parse("form");
}

function MostraAlerta(titulo, msg, functionCallback) {
    var fnCallback = functionCallback;
    if (fnCallback == undefined) {
        fnCallback = function () { };
    }
    bootbox.alert({ title: "<span class='tituloBootbox'>" + titulo + "</span>", message: msg, callback: fnCallback });
}

function MostraDialog(titulo, msg, optionCallbackCancel, optionCallbackOK) {
    var fnCallbackCancel = optionCallbackCancel;
    var fnCallbackOK = optionCallbackOK;

    if (fnCallbackCancel == undefined) {
        fnCallbackCancel = { label: _options.labelButtonCancel, className: "btn-default", callback: function () { } };
    }
    if (fnCallbackOK == undefined) {
        fnCallbackOK = { label: _options.labelButtonYes, className: "btn btn-primary", callback: function () { } };
    }
    bootbox.dialog({
        message: msg,
        title: "<span class='tituloBootbox'>" + titulo + "</span>",
        buttons: { cancel: fnCallbackCancel, ok: fnCallbackOK }
    });
}

function AjaxFormSuccess(result, frm, titleSucesso, titleError, optionCallbackOK, optionCallbackCancel) {
    var fnCallbackOK = optionCallbackOK;
    var fnCallbackCancel = optionCallbackCancel;

    if (fnCallbackOK == undefined) {
        fnCallbackOK = { label: "OK", className: "btn btn-primary", callback: function () { } };
    } else {
        fnCallbackOK = { label: "OK", className: "btn btn-primary", callback: optionCallbackOK };
    }
    if (fnCallbackCancel == undefined) {
        fnCallbackCancel = { label: "OK", className: "btn btn-primary", callback: function () { } };
    } else {
        fnCallbackCancel = { label: "OK", className: "btn btn-primary", callback: optionCallbackCancel };
    }
    if (TemErro(frm)) {
        var erros = $("#" + frm).find("#containerError").html();
        bootbox.dialog({
            message: erros,
            title: titleError,
            buttons: { ok: fnCallbackCancel }
        });
    }
    else {
        var msg = "<span style='color:#698e9f'>" + $("#" + frm).find("#containerSucesso").html() + "</span>";
        bootbox.dialog({
            message: msg,
            title: titleSucesso,
            buttons: { ok: fnCallbackOK }
        });
    }
    ResetUnobtrusiveValidator();
}

function callBackError(result) {
    hideSplash();
    MostraAlerta(_options.TitleErro, _options.MessageErro.replace("{{0}}", (result.responseJSON == undefined ? result.responseText : result.responseJSON.Message)), function () { });
}

function AjaxFormError(result) {
    callBackError(result);
    ResetUnobtrusiveValidator();
}

function AjaxUpdateContent(vurl, UpdateTargetId, funcao) {
    $.ajax({
        url: vurl,
        type: 'POST',
        data: {
            __RequestVerificationToken: _token
        },
        beforeSend: function () { $("#screen").show(); },
        success: function (result) {
            $("#" + UpdateTargetId).html(result);
            if (funcao) funcao();
            ResetUnobtrusiveValidator();
            $("#screen").hide();
        },
        complete: function () { $("#screen").hide(); },
        error: callBackError
    });
    return false;
}

function AjaxDelete(vurl, UpdateTargetId, UpdateTargetEditId, funcao) {
    bootbox.dialog({
        message: _options.messageDelete,
        title: "<span class='tituloBootbox'>" + _options.TitleDelete + "</span>",
        buttons: {
            cancel: { label: _options.labelButtonCancel, className: "btn-primary", callback: function () { } },
            ok: {
                label: "OK", className: "btn btn-primary", callback: function () {
                    $.ajax({
                        url: vurl,
                        type: 'POST',
                        data: {
                            __RequestVerificationToken: _token
                        },
                        beforeSend: function () { $("#screen").show().focus(); },
                        success: function (result) {
                            $(UpdateTargetId).html(result);
                            $(UpdateTargetEditId).html("");
                            if (funcao) funcao();
                            bootbox.alert({
                                message: _options.RegistroExlcuido,
                                title: "<span class='tituloBootbox'>" + _options.TitleDelete + "</span>",
                                buttons: { ok: { className: 'btn-primary' } }
                            });
                            //message: _options.RegistroExlcuido, title: "Ações", buttons: { ok { className: 'btn-success' }}});
                        },
                        complete: function () { hideSplash(); },
                        error: callBackError
                    });
                }
            }
        }
    });

    return false;
}

function showPainel(id) {
    $(id).show().removeClass("hide");
}
function hidePainel(id) {
    $(id).hide().addClass("hide");
}

function chkValue(boolValue) {
    $("input[name='" + boolValue.name + "']").val(boolValue.checked ? "True" : "False");
}

function TitleFromPlaceholder() {
    $("input[placeholder], textarea[placeholder]").each(function () {
        $(this).attr("title", $(this).attr("placeholder"));
    });
}

function SplashResize() {
    $("#splash").css("height", $(window).height());
    $("#splash").css("width", $(window).width());
}

function hideSplash() {
    $("#screen").hide();
}

function ItensChecked(frm, name) {
    return $("#" + frm).find("input[name=" + name + "]:checked").length;
}

function AdicionaMensagemErro(frm, msg) {
    var containerErro = $("#" + frm).find("[data-valmsg-summary=true]");
    var list = containerErro.find("ul");
    if (TemErro("formAcao"))
        list.append("<li>" + msg + "</li>");
    else
        list.html("<li>" + msg + "</li>");
}

function LimpaErros(frm) {
    var containerErro = $("#" + frm).find("[data-valmsg-summary=true]");
    containerErro.attr("class", "validation-summary-errors");
    var list = containerErro.find("ul").html("<li style='display:none'></li>");
}

function ConfigTabelaComCriterioDeColuna(_selector, _positionConlumn, _ascOrDesc, searchPlaceholder) {
    table = $(_selector).DataTable({
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
    resizeTable(table);
}

function resizeTable(table) {
    //$(table.column(0).header()).addClass('never');
    table.responsive.rebuild();
    table.responsive.recalc();

    //$(table.column(0).header()).removeClass('never');
    table.responsive.rebuild();
    table.responsive.recalc();
}

var table;
function ConfigTabela(_selector) {

    table = $(_selector).DataTable({
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
        }
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
}

function formatFileSize(bytes) {
    if (typeof bytes !== 'number') {
        return '';
    }

    if (bytes >= 1000000000) {
        return (bytes / 1000000000).toFixed(2) + ' GB';
    }

    if (bytes >= 1000000) {
        return (bytes / 1000000).toFixed(2) + ' MB';
    }

    return (bytes / 1000).toFixed(2) + ' KB';
}

function UploadArquivoUnico(urlToUpload, IdCtrlUpload, IdCtrlDrop, nomeCampo, maxSize, formJson) {
    $('#' + IdCtrlDrop + ' .actionUp').click(function () {
        $('#' + IdCtrlDrop).find('input[type=file]').click();
    });
    $('#' + IdCtrlUpload).fileupload({
        url: urlToUpload,
        sequentialUploads: true,
        singleFileUploads: true,
        limitMultiFileUploads: 1,
        formData: formJson,
        dropZone: $('#' + IdCtrlDrop),
        add: function (e, data) {
            if (data.files[0].size > maxSize) {
                bootbox.alert("O tamanho máximo de um arquivo é " + formatFileSize(maxSize));
            }
            else {
                var tpl = $("#" + IdCtrlUpload);
                tpl.find(".prog").show();
                tpl.find('.stop').click(function () {
                    jqXHR.abort();
                });

                // Automatically upload the file once it is added to the queue
                var jqXHR = data.submit();
            }
        },
        done: function (e, data) {
            if (data.textStatus == "success") {
                var tpl = $("#" + IdCtrlDrop).find('.img-responsive').attr("src", data.result);
                var aArquivo = data.result.split("/");
                var _arquivo = aArquivo[aArquivo.length - 1];
                _arquivo = _arquivo.split("?")[0];
                $("#" + nomeCampo).val(_arquivo);
            }
        },

        progress: function (e, data) {
            // Calculate the completion percentage of the upload
            var progress = parseInt(data.loaded / data.total * 100, 10);

            var tpl = $("#" + IdCtrlUpload);
            tpl.find('.prog').find("span").html(progress + " %" + "  - " + data.files[0].name);

            if (progress == 100) {
                tpl.find('.prog').find("span").html("0 %");
                tpl.find('.prog').hide();
            }
        },

        fail: function (e, data) {
            // Something has gone wrong!
            var tpl = $("#" + IdCtrlDrop).find('span');
            tpl.removeClass('working').addClass('error');
            tpl.html("Seu arquivo não foi carregado: Erro =>" + data.errorThrown);
        }
    });
}

//TODO: AJUSTAR
function UploadMultiplosArquivos(urlToUpload, IdCtrlDrop, IdCtrlUpload, nomeCampo, maxSize, formJson, idForm, idRegistro, urlDelete) {
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
                bootbox.alert("O tamanho máximo de um arquivo é " + formatFileSize(maxSize));
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
                _a += "<a href=\"javascript:;\" class=\"deletefile\" target=\"_blank\" data-tipo=\"" + tipo + "\" data-arquivo=\"" + data.files[0].name + "\" data-campo=\"" + nomeCampo + "\" data-id=\"" + idRegistro + "\"><i class=\"fa fa-trash-o\" aria-hidden=\"true\"></i></a>";

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
}

//TODO: AJUSTAR 28.117.644/0001-83
function UploadDeleteFiles(urlDelete) {
    $(".deletefile").bind("click", function (event) {
        event.preventDefault();
        var _arquivo = $(this).data("arquivo");
        var _tipo = $(this).data("tipo");
        var _id = $(this).data("id");
        var nomeCampo = $(this).data("campo");
        var obj = $(this);

        bootbox.confirm("Você realmente deseja apagar remover o arquivo " + _arquivo, function (result) {
            if (result == true) {

                var _oldCampo = $("#" + nomeCampo).val();
                $.ajax({
                    type: "POST",
                    url: urlDelete,
                    data: {
                        "__RequestVerificationToken": $("[name=__RequestVerificationToken]").val(),
                        "arquivo": _arquivo,
                        "id": _id,
                        "tipo": _tipo,
                        "campo": nomeCampo
                    },
                    beforeSend: function () { ShowSplash(); },
                    error: function (xhr) {
                        bootbox.alert(xhr.Message);
                    },
                    success: function (result) {
                        obj.prev().remove();
                        obj.remove();
                        $("#" + nomeCampo).val(result);
                    },
                    complete: function () { HideSplash(); }
                });
            }
        });
    });
}
function AtualizaPerfil(idPerfil, idUsuario) {
    var idCliente = $("#IdCliente").val();

    if (idPerfil == 3) {
        $('#FlCompartilhado').prop('checked', false)
        $("#FlCompartilhado").attr('disabled', true);
    }
    else {
        $("#FlCompartilhado").removeAttr('disabled');
    }

    $.ajax({
        type: "POST",
        url: "/Usuario/ListaSiteCargo?idPerfil=" + idPerfil + "&idUsuario=" + idUsuario + "&idCliente=" + idCliente,
        error: function (xhr) {
            bootbox.alert(xhr.responseText);
        },
        success: function (data) {
            $('#tipoUsuario').html(data);
            ResetUnobtrusiveValidator(true);
            EventsCtrls();
        },
        beforeSend: function () { ShowSplash(); },
        complete: function () { HideSplash(); }
    });
    return false;
}

function processoIndex() {
    $("#btnAddRotina").bind("click", function () {
        var obj = $("#editRotina");
        if (obj.hasClass("hide")) {
            obj.removeClass("hide");
        }
    });
    $("#btnCancelRotina").bind("click", function () {
        $("#rotinaRow").val("");
        $("#rotinaItem").val("");
        $("#rotinaOque").val("");
        $("#rotinaQuem").val("");
        $("#rotinaRegistro").val("");
        $("#editRotina").addClass("hide");
    });
    $(".btnEditRotina").bind("click", function () {
        var row = $(this).data("row");
        $("#rotinaRow").val(row);
        $("#rotinaItem").val($("#row" + row).find("#col1").val());
        $("#rotinaOque").val($("#row" + row).find("#col2").val());
        $("#rotinaQuem").val($("#row" + row).find("#col3").val());
        $("#rotinaRegistro").val($("#row" + row).find("#col4").val());
        $("#editRotina").removeClass("hide");
    });
    $("#btnSaveRotina").bind("click", function () {
        var row = $("#rotinaRow").val();;

        var item = $("#rotinaItem").val();
        var oque = $("#rotinaOque").val();
        var quem = $("#rotinaQuem").val();
        var registro = $("#rotinaRegistro").val();

        $("#row" + row).find("#col1").val(item);
        $("#row" + row).find("#scol1").text(item)

        $("#row" + row).find("#col2").val(oque);
        $("#row" + row).find("#scol2").text(oque)

        $("#row" + row).find("#col3").val(quem);
        $("#row" + row).find("#scol3").text(quem)

        $("#row" + row).find("#col4").val(registro);
        $("#row" + row).find("#scol4").text(registro)

        $("#editRotina").addClass("hide");
    });
}
function TodasColunasComMesmaAltura(tallest, seletor) {
    $(seletor).each(function () {
        tallest = $(this).height() > tallest ? $(this).height() : tallest;
    }).css("min-height", tallest);
}