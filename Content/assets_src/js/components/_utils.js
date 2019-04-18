/*
|--------------------------------------------------------------------------
| Utils
|--------------------------------------------------------------------------
*/

APP.component.Util = {

    init : function () {

    },

    setup : function () {

        


    },

    tratarUrl: function(url){

        var ambiente = urlBase.substring(0, urlBase.length - 1);

            var ret = ambiente + url;
            return ret;
    },

    getUrlAmbiente : function () {

        var url = (window.location.href);
        var protocol = window.location.protocol;
        var host = window.location.host;
        var pathname = window.location.pathname;
        var newURL = protocol + "//" + host;
        return newURL;
        
    },

    getController: function () {

        var controller = $('meta[name=controller]').attr('content');
        return controller ? controller : false;

    },

    getPage: function () {

    var page = $('meta[name=page]').attr('content');
    return page ? page : false;

}
};


