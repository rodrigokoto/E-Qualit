/*
|--------------------------------------------------------------------------
| SelectAll
|--------------------------------------------------------------------------
*/

APP.component.SelectAll = {
    
        init: function (_selector) {

            this.setup();
            this.setSelectAll(_selector);
    
        },
    
        setup: function () {
    
            //
    
        },
    
        setSelectAll : function (_selector) {
            
            $("[name^='" + _selector + "']").click(function(){
                $(this).closest('.tabela-check').find('input:checkbox').not(this).prop('checked', this.checked);
            });
    
        },
        
    };