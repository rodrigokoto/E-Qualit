/*
|--------------------------------------------------------------------------
| Model Processo
|--------------------------------------------------------------------------
*/

APP.model.Processo = {
    
        model : {

            IdProcesso : "",
            Nome : "",
            IdSite : "",
            FlAtivo : "",

        },
        
        constructor : function (_IdProcesso, _Nome, _IdSite, _FlAtivo) {
            
            var processo = {
                IdProcesso : _IdProcesso,
                Nome : _Nome,
                IdSite : _IdSite,
                FlAtivo : _FlAtivo,
            };
    
            return processo;
            
        },
    
    };
        