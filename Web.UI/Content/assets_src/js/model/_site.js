/*
|--------------------------------------------------------------------------
| Model Site
|--------------------------------------------------------------------------
*/

APP.model.Site = {

    SiteLogoAux : {},
    IdSite: "",
    NmFantasia : "",
    NmRazaoSocial : "",
    NuCNPJ : "",
    DsFrase : "",
    DsObservacoes : "",
    FlAtivo : "",
    SiteFuncionalidades : [],
    Processos : [],
    IdCliente : "",

    constructor : function (_SiteLogoAux, _IdSite, _NmFantasia, _NmRazaoSocial, _NuCNPJ, 
                            _DsFrase, _DsObservacoes, _FlAtivo, _SiteFuncionalidades, _Processos, _IdCliente) {
        
        var site = {
            SiteLogoAux : _SiteLogoAux,
            IdSite: _IdSite,
            NmFantasia : _NmFantasia,
            NmRazaoSocial : _NmRazaoSocial,
            NuCNPJ : _NuCNPJ,
            DsFrase : _DsFrase,
            DsObservacoes : _DsObservacoes,
            FlAtivo : _FlAtivo,
            SiteFuncionalidades : _SiteFuncionalidades,
            Processos : _Processos,
            IdCliente : _IdCliente,
        };

        return site;
        
    },

};
