/*
|--------------------------------------------------------------------------
| Menu Side Bar
|--------------------------------------------------------------------------
*/

APP.component.MenuSideBar = {
    
        init: function () {
    
            this.setup();
            this.menuSideBar();
    
        },
    
        setup: function () {
    
            //Botao de trocar a imagem
            //this.buttonUploadImg = $("[class^=btn-upload-img]");
    
        },
    
        menuSideBar : function () {
            
            var trigger = $('.hamburger'),
            overlay = $('.overlay'),
            isClosed = false;
        
            trigger.click(function () {
                hamburger_cross();      
            });
        
            function hamburger_cross() {
        
                if (isClosed == true) {          
                    overlay.hide();
                    trigger.removeClass('is-open');
                    trigger.addClass('is-closed');
                    isClosed = false;
                    $('.wrapper-toggle').removeClass('toggled');
                } else {   
                    overlay.show();
                    trigger.removeClass('is-closed');
                    trigger.addClass('is-open');
                    isClosed = true;
                    $('.wrapper-toggle').addClass('toggled');
                }
            }
    
        },

    
    };