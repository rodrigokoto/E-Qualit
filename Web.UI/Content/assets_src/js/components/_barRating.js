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

    setBarRating : function (_selector, _theme) {
        
        $(_selector).barrating('show', {
            theme: _theme,
            initialRating: '0',
            showValues: false,
            onSelect: function (value, text) {
                //console.log(value, text);
            }
        });

    },

    setBarRatingGestaoDeRisco : function (_divGestaoDeRisco, _theme) {

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