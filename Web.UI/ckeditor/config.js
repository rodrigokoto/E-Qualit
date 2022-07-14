/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {

    config.language = jsIdioma;
    //config.removePlugins = 'elementspath';
    config.removeButtons = 'About';

    config.extraPlugins = 'image,filebrowser,font,richcombo,floatpanel,panel,listblock,button,wordcount,notification';
    //config.uploadUrl = '/Upload/UploadFile';

    //configurações das paginas para upload e browser de arquivo
    config.filebrowserUploadUrl = "/Ckeditor/UploadImageCkEditor";
    config.filebrowserWindowWidth = "750";
    config.filebrowserWindowHeight = "600";
    config.resize_enabled = true;
    config.toolbarCanCollapse = true;
    config.image_previewText = "1.	Clique na aba buscar imagem.<br>2.	Selecione o procurar.<br>3.	Selecione o arquivo.<br>4.	Clique em upload.<br>Na nova tela ser&aacute; poss&iacute;vel: alterar a largura e altura da imagem, incluir uma borda e alterar o alinhamento.";

    config.removeDialogTabs = 'image:Link;image:advanced';

    config.fontSize_defaultLabel = '12';

    config.toolbarGroups = [
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
        { name: 'links' },
        { name: 'insert' },
        { name: 'forms' },
        { name: 'tools' },
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'others' },
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
        { name: 'styles' },
        { name: 'colors' },
        { name: 'about' },
        { name: 'wordcount' },
        { name: 'notification' }
    ];

    config.wordcount = {

        // Whether or not you want to show the Paragraphs Count
        showParagraphs: false,

        // Whether or not you want to show the Word Count
        showWordCount: false,

        // Whether or not you want to show the Char Count
        showCharCount: true,

        // Whether or not you want to count Spaces as Chars
        countSpacesAsChars: true,

        // Whether or not to include Html chars in the Char Count
        countHTML: false,

        // Maximum allowed Word Count, -1 is default for unlimited
        maxWordCount: -1,

        // Maximum allowed Char Count, -1 is default for unlimited
        maxCharCount: -1,

        // Add filter to add or remove element before counting (see CKEDITOR.htmlParser.filter), Default value : null (no filter)
        filter: new CKEDITOR.htmlParser.filter({
            elements: {
                div: function (element) {
                    if (element.attributes.class == 'mediaembed') {
                        return false;
                    }
                }
            }
        })
    };
};


