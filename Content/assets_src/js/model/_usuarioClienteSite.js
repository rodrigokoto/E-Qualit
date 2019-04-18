/*
|--------------------------------------------------------------------------
| Model UsuarioClienteSite
|--------------------------------------------------------------------------
*/

APP.model.UsuarioClienteSite = {
    
       model: {
            IdSite : "",
            IdCliente : "",
        },
       
    
        constructor : function (_IdSite, _IdCliente)
        {
            var usuarioClienteSite = {
                IdSite : _IdSite,
                IdCliente : _IdCliente
            };
            
            return usuarioClienteSite;
        }
    
    };
    