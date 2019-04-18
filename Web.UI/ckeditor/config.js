/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {

	config.language = jsIdioma;
	//config.removePlugins = 'elementspath';
	config.removeButtons = 'About';

    config.extraPlugins = 'image,filebrowser,font,richcombo,floatpanel,panel,listblock,button';
	//config.uploadUrl = '/Upload/UploadFile';
    
	//configurações das paginas para upload e browser de arquivo
    config.filebrowserUploadUrl = "/Ckeditor/UploadImageCkEditor";
	config.filebrowserWindowWidth = "750";
	config.filebrowserWindowHeight = "600";
	config.resize_enabled = true;
	config.toolbarCanCollapse = true;
	config.image_previewText = "1.	Clique na aba buscar imagem.<br>2.	Selecione o procurar.<br>3.	Selecione o arquivo.<br>4.	Clique em upload.<br>Na nova tela será possível: alterar a largura e altura da imagem, incluir uma borda e alterar o alinhamento.";

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
        { name: 'about' }
    ];
};