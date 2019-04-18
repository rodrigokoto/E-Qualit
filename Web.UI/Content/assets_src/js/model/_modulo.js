/*
|--------------------------------------------------------------------------
| Model Modulo
|--------------------------------------------------------------------------
*/

APP.model.Modulo = {

    model : {
        IdSiteFuncionalidade: "",
        IdSite : "",
        IdFuncionalidade : "",  
    },

    constructor : function (_IdSiteFuncionalidade, _IdSite, _IdFuncionalidade) {

        var modulo = {
            IdSiteFuncionalidade : _IdSiteFuncionalidade,
            IdSite : _IdSite,
            IdFuncionalidade : _IdFuncionalidade,
        };

        return modulo;
        
    },

};
    