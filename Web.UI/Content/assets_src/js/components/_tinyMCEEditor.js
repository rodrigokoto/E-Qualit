/*
|--------------------------------------------------------------------------
| TINYMCE EDITOR
|--------------------------------------------------------------------------
*/

APP.component.TinyMCE = {

    init : function (_selector) {
        this.start(_selector);
    },

    start: function (_selector)
    {
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


