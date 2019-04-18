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

    setCkEditor : function (_nameID) {
    
        CKEDITOR.replace(_nameID);
      
    },   

};