/*
|--------------------------------------------------------------------------
| Model Cliente
|--------------------------------------------------------------------------
*/

APP.model.Cliente = {
    
        model : {
            ClienteLogoAux : {},
            NmFantasia: "",
            NmUrlAcesso: "",
            DtValidadeContrato: "",
            NuDiasTrocaSenha: "",
            NuTentativaBloqueioLogin: "",
            NuArmazenaSenha: "",
            ContratosAux: [],
            FlAtivo: "",
            FlTemCaptcha: "",
            FlExigeSenhaForte: "",
            Usuario: {},
            IdCliente: "",
        },

        constructor : function (_ClienteLogoAux, _NmFantasia, _NmUrlAcesso, _DtValidadeContrato, _NuDiasTrocaSenha, _NuTentativaBloqueioLogin,
                                _NuArmazenaSenha, _Contratos, _FlAtivo, _FlTemCaptcha, _FlExigeSenhaForte, _Usuario, _IdCliente) {
        
            var cliente = {
                ClienteLogoAux : _ClienteLogoAux,
                NmFantasia: _NmFantasia,
                NmUrlAcesso: _NmUrlAcesso,
                DtValidadeContrato: _DtValidadeContrato,
                NuDiasTrocaSenha: _NuDiasTrocaSenha,
                NuTentativaBloqueioLogin: _NuTentativaBloqueioLogin,
                NuArmazenaSenha: _NuArmazenaSenha,
                ContratosAux: _Contratos,
                FlAtivo: _FlAtivo,
                FlTemCaptcha: _FlTemCaptcha,
                FlExigeSenhaForte: _FlExigeSenhaForte,
                Usuario: _Usuario,
                IdCliente: _IdCliente,
            };

            return cliente;
            
        },
    
    };
    