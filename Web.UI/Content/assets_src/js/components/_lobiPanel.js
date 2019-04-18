/*
|--------------------------------------------------------------------------
| Lobi Panel
|--------------------------------------------------------------------------
*/

APP.component.AtivaLobiPanel = {

    init: function () {
 
        this.ativaLobiPanel();

    },

    ativaLobiPanel : function () {

        $('.panel').lobiPanel({
            editTitle: false,
            unpin: false,
            reload: false,
            close: false,
            minimize: {
                icon: 'fa fa-minus',
                icon2: 'fa fa-plus',
                tooltip: 'Minimiza'
            },
            expand: {
                icon: 'fa fa-expand',
                icon2: 'fa fa-compress',
                tooltip: 'Tela cheia'
            },
            close: false,
            toggleIcon: "caret"
        });
 
    },

}; 