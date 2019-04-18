/*
|--------------------------------------------------------------------------
| Model Anexo
|--------------------------------------------------------------------------
*/

APP.model.Anexo = {
    
        model : {

            IdAnexo : "",
            Extensao : "",
            ArquivoB64 : "",

        },
        
        constructor : function (_IdAnexo, _Extensao, _ArquivoB64) {
            
            var anexo = {
                IdAnexo : _IdAnexo,
                Extensao : _Extensao,
                ArquivoB64 : _ArquivoB64,
            };
    
            return anexo;
            
        },
    
    };
        