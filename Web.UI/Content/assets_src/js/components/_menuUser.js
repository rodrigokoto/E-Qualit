/*
|--------------------------------------------------------------------------
| User Menu
|--------------------------------------------------------------------------
*/

APP.component.UserMenu = {

    init: function () {

        var page = APP.component.Util.getPage();
        
        if (page != "Login") {
            
            this.setup();
            this.user();
            //this.sitesPorCliente();
            this.clientes();
            //this.sitesPorCoordenador();
            this.setBoxClientes();
            this.setSitesCoordenador();
        }
    },

    setup: function () {

        this.openCliente = $('#nav-user-clientes');
        this.closeCliente = $('#close-user-clientes');
        this.openSites = $('.box-clientes');
        this.closeSites = $('#close-user-sites');
        this.openSitesCoordenador = $('#nav-user-site');

    },

    user: function () {

        //Menu - Clientes
        this.openCliente.on('click', function () {
            $('#clientes').slideDown();
        });

        this.closeCliente.on('click', function () {
            $('#clientes').slideUp();
        });

        this.openSites.on('click', function () {
            $('#clientes').slideUp();
            $('#sites').slideDown();
        });

        this.closeSites.on('click', function () {
            $('#sites').slideUp();
        });

        this.openSitesCoordenador.on('click', function () {
            $('#sites').slideDown();
        });

    },

    sitesPorCliente: function (cliente) {
        $.ajax({
            type: "GET",
            async: false,
            dataType: 'json',
            url: "/Site/ObterSitesPorCliente",
            data: { idCliente: cliente },
            success: function (result) {

                var $siteBox = $("#sites .row-box");

                $siteBox.html("");

                $.each(result, function (key, val) {

                    var htmlSites = '';
                    var img = "";
                    
                    if (val.SiteLogoAux.Extensao != null) {
                        img = "data:image/" + val.SiteLogoAux.Extensao + ";base64," + val.SiteLogoAux.ArquivoB64;
                    } else {
                        img = val.SiteLogoAux.ArquivoB64;
                    }
                    
                    htmlSites += '<!-- Site - ' + val.NmFantasia + ' -->';
                    htmlSites += '<div class="col-md-2">';
                    htmlSites += '<div class="box-clientes">';
                    htmlSites += '<a href="/Site/EscolheSite/?idSite=' + val.IdSite + '">';
                    htmlSites += '<img src="' + img + '" class="img-responsive">';
                    htmlSites += '<span>' + val.NmFantasia + '</span></a>';
                    htmlSites += '</a>';
                    htmlSites += '</div>';
                    htmlSites += '</div>';
                    $siteBox.append(htmlSites);
                });
            },
        });
        $('#clientes').slideUp();
        $('#sites').slideDown();
    },

    clientes: function () {

        this.openCliente.on('click', function () {
            var tamanho = $("#clientes .box-clientes").size();
            var url = window.location.href;
            var $siteBox = $("#sites .row-box");
      
            $siteBox.html("");
            if (tamanho == 0) {

                $.ajax({
                    type: "GET",
                    async: false,
                    dataType: 'json',
                    url: "/Cliente/ObterClientesUsuario",
                    success: function (result) {
                        
                        var $clienteBox = $("#clientes .row-box");
                        
                        $.each(result, function (key, val) {
                            
                            var htmlCliente = '';
                            var img = "";

                            if (val.ClienteLogoAux.Extensao != null) {
                                img = "data:image/" + val.ClienteLogoAux.Extensao + ";base64," + val.ClienteLogoAux.ArquivoB64;
                            } else {
                                img = val.ClienteLogoAux.ArquivoB64;
                            }

                            htmlCliente += '<!-- Cliente - ' + val.IdCliente + ' -->';
                            htmlCliente += '<div class="col-md-2">';
                            htmlCliente += '<div class="box-clientes">';
                            htmlCliente += '<a href="#" class="get-site-cliente">';
                            htmlCliente += '<img src="' + img + '" class="img-responsive">';
                            htmlCliente += '<span><input type="hidden" class="idCliente"' + 'value="' + val.IdCliente + '">' + val.NmFantasia + '</span></a>';
                            htmlCliente += '</div>';
                            htmlCliente += '</div>';
                            $clienteBox.append(htmlCliente);
                        });

                        htmlCliente += '<!-- Sair -->';
                        htmlCliente += '<div class="col-md-2">';
                        htmlCliente += '<div class="box-clientes">';
                        htmlCliente += '<a href="#" class="get-site-cliente">';
                        htmlCliente += '<img src="../../../../img/sair.png" class="img-responsive">';
                        htmlCliente += '<span><input type="hidden" class="idCliente"' + 'value="0">Sair</span></a>';
                        htmlCliente += '</div>';
                        htmlCliente += '</div>';
                        $clienteBox.append(htmlCliente);

                        APP.component.UserMenu.setBoxClientes();

                    },
                });

            }
        });

    },

    setBoxClientes: function () {
        $('.get-site-cliente').unbind('click');
        $('.get-site-cliente').bind('click', function (e) {

            var id = $(this).find(".idCliente").val();

            APP.component.UserMenu.getEscolheCliente(id);

        });
    },

    getEscolheCliente: function (_id) {

        $.ajax({
            type: "GET",
            async: false,
            dataType: 'json',
            data: { "idCliente": _id },
            url: "/Cliente/EscolheCliente/",
            success: function (result) {
                
            },
        });
        APP.component.UserMenu.sitesPorCliente(_id);
        //APP.component.UserMenu.setImagemCliente(_id);

    },

    setSitesCoordenador: function(){
        this.openSitesCoordenador.unbind("click");
        this.openSitesCoordenador.bind("click", function() {
            $("#modal-panel-clientes-sites .modal-title").html("Sites");
            APP.component.UserMenu.sitesPorCliente();
        }); 
    },

};