
/*
|--------------------------------------------------------------------------
| General
|--------------------------------------------------------------------------
*/

APP.controller.General = {

    init: function () {

        var page = APP.component.Util.getPage();
        this.start();
        this.controllerPages(page);

    },

    start: function () {
        //APP.component.Modal.init();        
        APP.component.UserMenu.init();
        APP.component.Loading.init();
        APP.component.Calendar.init();
        APP.component.Menu.init();
    },

    controllerPages: function (page) {
        //Controle das Pages

        //Login
        if (page == "Login" || page == "RecuperarSenha" || page == "AlterarSenhaLogin") {
            this.loginController();
        }

        //Home
        if (page == "Index") {
            this.homeController();
        }

        //AcaoCorretiva
        if (page == "CriarAcaoCorretiva" || page == "IndexAcaoCorretiva" || page == "ExibirAcaoCorretiva") {
            this.acaoCorretivaController();
        }

        //Cliente
        if (page == "IndexCliente" || page == "CriarCliente") {
            this.clienteController();
        }

        //MeusDados
        if (page == "IndexUsuario" || page == "MeusDados" || page == "AlterarSenha" || page == "CriarUsuario") {
            this.usuarioController();
        }

        //Site
        if (page == "IndexSite" || page == "CriarSite") {
            this.siteController();
        }

        //Cargo
        if (page == "IndexCargo" || page == "CriarCargo") {
            this.cargoController();
        }

        //Nao Conformidade
        if (page == "IndexNaoConformidade" || page == "CriarNaoConformidade" || page == "EditarNaoConformidade") {
            this.naoConformidadeController();
        }

        if (page == "IndexGestaoMelhoria" || page == "CriarGestaoMelhoria" || page == "EditarGestaoMelhoria") {
            this.gestaoMelhoriaController();
        }
        
        //Gestao de Risco
        if (page == "IndexGestaoDeRisco" || page == "CriarGestaoDeRisco") {
            this.gestaoDeRiscoController();
        }

        //Instrumentos
        if (page == "IndexInstrumentos" || page == "CriarInstrumentos") {
            this.instrumentosController();
        }

        //Analise Critica
        if (page == "IndexAnaliseCritica" || page == "CriarAnaliseCritica") {
            this.analiseCriticaController();
        }

        //Indicador
        if (page == "IndexIndicador" || page == "CriarIndicador" || page == "RelatorioBarras" || page == "RelatorioColuna" || page == "DashBoard") {
            this.indicadorController();
        }

        //Control Doc
        if (page == "EmissaoDocumento" || page == "ListDocumentos" || page == "ConteudoDocumento") {
            this.controlDocController();
        }

        //Fornecedores
        if (page == "IndexProdutos" || page == "AcoesProdutos" || page == "IndexFornecedores" || page == "AcoesFornecedores") {
            this.fornecedoresController();
        }

        //Relatorio
        if (page == "DashBoard") {
            this.relatorioController();
        }

        //Auditoria
        if (page == "IndexAuditoria") {
            this.auditoriaController();
        }
        //Norma
        if (page == "IndexNorma") {
            this.normaController();
        }
        //Plai
        if (page == "AcoesPlai") {
            this.plaiController();
        }

        if (page == "IndexLicencas" || page == "CriarLicencas") {
            this.LicencaController();
        }

    },

    loginController: function () {
        APP.controller.LoginController.init();
    },

    homeController: function () {
        APP.controller.HomeController.init();
    },

    acaoCorretivaController: function () {
        APP.controller.AcaoCorretivaController.init();
    },


    clienteController: function () {

        APP.controller.ClienteController.init();

    },

    gestaoMelhoriaController: function () {
        APP.controller.GestaoMelhoriaController.init();
    },

    naoConformidadeController: function () {
        APP.controller.NaoConformidadeController.init();
    },

    acaoCorretivaController: function () {
        APP.controller.AcaoCorretivaController.init();
    },

    gestaoDeRiscoController: function () {
        APP.controller.GestaoDeRiscoController.init();
    },

    instrumentosController: function () {
        APP.controller.InstrumentosController.init();
    },

    analiseCriticaController: function () {
        APP.controller.AnaliseCriticaController.init();
    },

    indicadorController: function () {
        APP.controller.IndicadorController.init();
    },

    controlDocController: function () {
        APP.controller.ControlDocController.init();
    },

    fornecedoresController: function () {
        APP.controller.FornecedoresController.init();
    },


    usuarioController: function () {
        APP.controller.UsuarioController.init();
    },

    siteController: function () {
        APP.controller.SiteController.init();
    },

    cargoController: function () {
        APP.controller.CargoController.init();
    },

    auditoriaController: function () {
        APP.controller.AuditoriaController.init();
    },

    relatorioController: function () {
        APP.controller.RelatorioController.init();
    },

    normaController: function () {
        APP.controller.NormaController.init();
    },

    plaiController: function () {
        APP.controller.PlaiController.init();
    },

    processoController: function () {
        APP.controller.ProcessoController.init();
    },

    LicencaController: function () {
        APP.controller.LicencaController.init();
    },

};
