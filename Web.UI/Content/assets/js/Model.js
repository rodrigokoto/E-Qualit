
/*
|--------------------------------------------------------------------------
| Model Anexo
|--------------------------------------------------------------------------
*/

APP.model.Anexo = {

    model: {

        IdAnexo: "",
        Extensao: "",
        ArquivoB64: "",

    },

    constructor: function (_IdAnexo, _Extensao, _ArquivoB64) {

        var anexo = {
            IdAnexo: _IdAnexo,
            Extensao: _Extensao,
            ArquivoB64: _ArquivoB64,
        };

        return anexo;

    },

};

/*
|--------------------------------------------------------------------------
| Model Cliente
|--------------------------------------------------------------------------
*/

APP.model.Cliente = {

    model: {
        ClienteLogoAux: {},
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

    constructor: function (_ClienteLogoAux, _NmFantasia, _NmUrlAcesso, _DtValidadeContrato, _NuDiasTrocaSenha, _NuTentativaBloqueioLogin,
        _NuArmazenaSenha, _Contratos, _FlAtivo, _FlTemCaptcha, _FlExigeSenhaForte, _Usuario, _IdCliente) {

        var cliente = {
            ClienteLogoAux: _ClienteLogoAux,
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

/*
|--------------------------------------------------------------------------
| Model Modulo
|--------------------------------------------------------------------------
*/

APP.model.Modulo = {

    model: {
        IdSiteFuncionalidade: "",
        IdSite: "",
        IdFuncionalidade: "",
    },

    constructor: function (_IdSiteFuncionalidade, _IdSite, _IdFuncionalidade) {

        var modulo = {
            IdSiteFuncionalidade: _IdSiteFuncionalidade,
            IdSite: _IdSite,
            IdFuncionalidade: _IdFuncionalidade,
        };

        return modulo;

    },

};

/*
|--------------------------------------------------------------------------
| Model Perfil Usuario
|--------------------------------------------------------------------------
*/

APP.model.PerfilUsuario = {

    Administrador: 1,
    Suporte: 2,
    Coordenador: 3,
    Colaborador: 4,

};

/*
|--------------------------------------------------------------------------
| Model Processo
|--------------------------------------------------------------------------
*/

APP.model.Processo = {

    model: {

        IdProcesso: "",
        Nome: "",
        IdSite: "",
        FlAtivo: "",

    },

    constructor: function (_IdProcesso, _Nome, _IdSite, _FlAtivo) {

        var processo = {
            IdProcesso: _IdProcesso,
            Nome: _Nome,
            IdSite: _IdSite,
            FlAtivo: _FlAtivo,
        };

        return processo;

    },

};

/*
|--------------------------------------------------------------------------
| Model Site
|--------------------------------------------------------------------------
*/

APP.model.Site = {

    SiteLogoAux: {},
    IdSite: "",
    NmFantasia: "",
    NmRazaoSocial: "",
    NuCNPJ: "",
    DsFrase: "",
    DsObservacoes: "",
    FlAtivo: "",
    SiteFuncionalidades: [],
    Processos: [],
    IdCliente: "",

    constructor: function (_SiteLogoAux, _IdSite, _NmFantasia, _NmRazaoSocial, _NuCNPJ,
        _DsFrase, _DsObservacoes, _FlAtivo, _SiteFuncionalidades, _Processos, _IdCliente) {

        var site = {
            SiteLogoAux: _SiteLogoAux,
            IdSite: _IdSite,
            NmFantasia: _NmFantasia,
            NmRazaoSocial: _NmRazaoSocial,
            NuCNPJ: _NuCNPJ,
            DsFrase: _DsFrase,
            DsObservacoes: _DsObservacoes,
            FlAtivo: _FlAtivo,
            SiteFuncionalidades: _SiteFuncionalidades,
            Processos: _Processos,
            IdCliente: _IdCliente,
        };

        return site;

    },

};

/*
|--------------------------------------------------------------------------
| Model Usuario
|--------------------------------------------------------------------------
*/

APP.model.Usuario = {

    FotoPerfilAux: {},
    NmCompleto: "",
    //FlSexo : "",
    CdIdentificacao: "",
    NuCPF: "",
    DtExpiracao: "",
    IdPerfil: "",
    FlRecebeEmail: "",
    FlCompartilhado: "",
    FlAtivo: "",
    FlBloqueado: "",
    UsuarioClienteSites: {},
    IdUsuario: "",
    UsuarioCargoes: {},

    constructor: function (_FotoPerfilAux, _NmCompleto, _CdIdentificacao, _NuCPF, _DtExpiracao, _IdPerfil, _FlRecebeEmail,
        _FlCompartilhado, _FlAtivo, _FlBloqueado, _UsuarioClienteSites, _IdUsuario, _UsuarioCargoes) {
        var usuario = {
            FotoPerfilAux: _FotoPerfilAux,
            NmCompleto: _NmCompleto,
            //FlSexo : _FlSexo,
            CdIdentificacao: _CdIdentificacao,
            NuCPF: _NuCPF,
            DtExpiracao: _DtExpiracao,
            IdPerfil: _IdPerfil,
            FlRecebeEmail: _FlRecebeEmail,
            FlCompartilhado: _FlCompartilhado,
            FlAtivo: _FlAtivo,
            FlBloqueado: _FlBloqueado,
            UsuarioClienteSites: _UsuarioClienteSites,
            IdUsuario: _IdUsuario,
            UsuarioCargoes: _UsuarioCargoes,
        };
        return usuario;
    }

};

/*
|--------------------------------------------------------------------------
| Model UsuarioCargo
|--------------------------------------------------------------------------
*/

APP.model.UsuarioCargo = {

    model: {
        IdCargo: "",
    },


    constructor: function (_IdCargo) {
        var usuarioCargo = {
            IdCargo: _IdCargo
        };

        return usuarioCargo;
    }

};

/*
|--------------------------------------------------------------------------
| Model UsuarioClienteSite
|--------------------------------------------------------------------------
*/

APP.model.UsuarioClienteSite = {

    model: {
        IdSite: "",
        IdCliente: "",
    },


    constructor: function (_IdSite, _IdCliente) {
        var usuarioClienteSite = {
            IdSite: _IdSite,
            IdCliente: _IdCliente
        };

        return usuarioClienteSite;
    }

};
