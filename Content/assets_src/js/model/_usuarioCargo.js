/*
|--------------------------------------------------------------------------
| Model UsuarioCargo
|--------------------------------------------------------------------------
*/

APP.model.UsuarioCargo = {
    
       model: {
            IdCargo : "",
        },
       
    
        constructor : function (_IdCargo)
        {
            var usuarioCargo = {
                IdCargo : _IdCargo
            };
            
            return usuarioCargo;
        }
    
    };
    