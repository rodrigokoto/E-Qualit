/*
|--------------------------------------------------------------------------
| Menu
|--------------------------------------------------------------------------
*/

APP.component.Menu = {

    init : function () {
        this.setup();
        this.abrirMenu();
        this.fecharMenu();
    },

    setup : function () {

        this.OpenMenu = $('.menu-lateral');
        this.CloseMenu = $('.close-menu-lateral');

    },

    abrirMenu : function () {
        
        this.OpenMenu.on('click', function () {
            $("#mySidenav").css("width", "250px");
            $("#main").css("marginLeft", "250px");
        });

    },

    fecharMenu : function () {

        this.CloseMenu.on('click', function () {
            $("#mySidenav").css("width", "0px");
            $("#main").css("marginLeft", "0px");

        });

    },
 
};