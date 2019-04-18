/*
|--------------------------------------------------------------------------
| Loading
|--------------------------------------------------------------------------
*/

APP.component.Loading = {

    init: function () {

        this.setup();
        this.hideLoading();

    },

    setup: function () {

        //

    },

    showLoading : function () {

        $(".loading").show().focus();

    },

    hideLoading : function () {

        $(".loading").hide();

    },

};