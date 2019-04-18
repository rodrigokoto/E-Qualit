/*!
 * Author: Silvestre Pinheiro de Freitas Jr.
 * Date: 4 Jan 2014
 * Description:
 *      This file should be included in all pages
 !**/

/*
 * Global variables. If you change any of these vars, don't forget 
 * to change the values in the less files!
 */

(function ($) {
    var config;
    var obj;
    var methods = {
        init: function (settings) {
            obj = $(this);
            config = {
                'Token': '',
                'IdButtonPreview': "btnfa-eye",
                'PreviewTitle': "Visualização",
                'InvalidExtensionFilesMessage': 'Somente pode ser enviados arquivo com as extensões: .gif; .jpg; .jpeg ou .png',
                'InvalidExteensionTitleBox': 'Erro',
                'IdControl': 'Imagem',
                'IdButtonDelete': 'btnExcluirImagem'
            };

            if (settings) { $.extend(config, settings); }
            BindEvent();

            return this;
        },
        destroy: function () {

        },
        update: function (content) {
            BindEvent();
        }
    };

    function BindEvent() {
        $("#" + config.IdButtonPreview).bind("click", function () {
            if ($.trim($(this).attr("data-url")) == "") return;
            bootbox.dialog({
                message: "<img src='" + $(this).attr("data-url") + "' class='img-responsive'>",
                title: config.PreviewTitle,
                buttons: {
                    ok: {
                        label: "OK", className: "bootbox-button", callback: function () {
                        }
                    }
                }
            });
        });

        obj.bind("change", function (event) {
            event.preventDefault();

            var data = new FormData();
            var file = $(this).get(0).files;

            if (file.length > 0) {
                data.append("Arquivo", file[0]);
                data.append("__RequestVerificationToken", _token);
            }
            if (file.length == 0 || file[0] == "") return false;

            var arquivo = $(this).val();
            if (!validoFormato(arquivo, new Array('.gif', '.jpg', '.jpeg', '.png'))) {
                bootbox.dialog({
                    message: config.InvalidExtensionFilesMessage,
                    title: config.InvalidExteensionTitleBox,
                    buttons: {
                        ok: {
                            label: "OK", className: "bootbox-button", callback: function () {
                            }
                        }
                    }
                });
                return false;
            }

            $.ajax({
                url: $(this).attr("data-url"),
                type: 'POST',
                beforeSend: function () { ShowSplash(); },
                processData: false,
                contentType: false,
                data: data,
                complete: function () { HideSplash(); },
                error: function (result) {
                    console.log(result);
                },
                success: function (data) {
                    if (data.Success) {
                        $("#" + config.IdControl).val(data.Arquivo);
                        $("#" + config.IdButtonPreview).attr("data-url", data.Url);
                        obj.val("");
                    }
                }
            });
            return false;
        });

        $('#' + config.IdButtonDelete).bind("click", function () {
            $("#" + config.IdControl).val("");
        });
    }

    function validoFormato(arquivo, extensoes) {
        var ext, valido;
        ext = arquivo.substring(arquivo.lastIndexOf(".")).toLowerCase();

        valido = false;
        for (var i = 0; i <= arquivo.length; i++) {
            if (extensoes[i] == ext) {
                valido = true;
                break;
            }
        }

        return valido;
    }

    $.fn.g2itUpload = function (method) {
        // Metódo de chamada
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.Dashborad');
        }

        return this;
    };
})(jQuery);