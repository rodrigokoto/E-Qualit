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

    },

    setup: function () {

        //Botoes
        this.buttonUploadImg = $("[class^=btn-upload]");
        this.buttonUploadImgMultiple = $("[class^=btn-upload-multiple]");
        this.buttonUploadArquivo = $("[class^=btn-upload-file]");
        this.buttonDelFileUpload = $("[class^=btn-delete]");

        this.buttonUploadRai = $("[class^=btn-upload-rai]");
        this.buttonDelFileUploadRai = $("[class^=btn-delete-rai]");

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
                            html += '<i class="fa fa-trash fa-1" aria-hidden="true">';
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
            var srcImg = $(this).children();
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
                        
                        srcImg.text(name);

                        var html = '';
                        html += '<a href="#" class="btn-delete">';
                        html += '<i class="fa fa-trash fa-1" aria-hidden="true">';
                        html += '</i>';

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

    fileUploadRai : function () {

        this.buttonUploadRai.unbind('click');
        this.buttonUploadRai.on('click', function () {
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
                        
                        srcImg.text('R.A.I');

                        var html = '';
                        html += '<a href="#" class="btn-delete-rai">';
                        html += '<i class="fa fa-trash fa-1" aria-hidden="true">';
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

    delItemFileUploadRai : function () {

        this.buttonDelFileUploadRai.unbind('click');
        this.buttonDelFileUploadRai.on('click', function (event) {
            
            event.preventDefault();
            $(this).closest('div').find('[class^=btn-upload]').text('');
            $(this).closest('div').find('input[type=file]').data('b64', '');
            $(this).closest('div').find('[class^=btn-upload]').append('<i class="fa fa-paperclip fa-1x" aria-hidden="true"></i> Anexar');
            $(this).remove();

        });

    },

    bind: function () {

        APP.component.FileUpload.setup();
        APP.component.FileUpload.delItemFileUpload();
        APP.component.FileUpload.delItemFileUploadRai();
        
    },

};