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

    setUploadFiles : function (urlToUpload, IdCtrlDrop, IdCtrlUpload, nomeCampo, maxSize, formJson, idForm, idRegistro, urlDelete) {

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