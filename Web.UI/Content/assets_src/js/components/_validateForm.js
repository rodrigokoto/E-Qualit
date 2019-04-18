/*
|--------------------------------------------------------------------------
| Validate Form
|--------------------------------------------------------------------------
*/

APP.component.ValidateForm = {

    init: function (_objFormValidade, _formName) {

        this.setup();
        this.validateForm(_objFormValidade, _formName);

    },

    setup: function () {

        this.cpfIsValid();
        this.cnpjIsValid();
        this.verifyTableAcaoImediata();

    },

    validateForm : function (_objFormValidade, _formName) {

        var page = APP.component.Util.getPage();

        $(_formName).validate({
            ignore: ".ignore",
            rules: _objFormValidade,
                messages: {
                    
                },
                errorElement: "em",
                errorPlacement: function ( error, element ) {

                    //Verifica se tem grupo de checkbox
                    var hasTableCheck = $(element).closest('.form-group').find('.tabela-check').size();


                    // Add the `help-block` class to the error element
                    error.addClass( "help-block" );
                    //error.addClass( "arrow_box " );
                    //console.log(error);

                    // Add `has-feedback` class to the parent div.form-group
                    // in order to add icons to inputs
                    element.closest( ".form-group" ).addClass( "has-feedback" );
                    //console.log(element);

                    
                    if ( element.prop( "type" ) === "checkbox" ) {
                        if (hasTableCheck != 0){
                            element.closest( ".form-group" ).find('.tabela-check').addClass( "has-feedback" );
                            error.insertAfter( element.closest('.form-group').children().last() );
                        } else {
                            error.insertAfter( element.closest('.form-group').children() );
                        }
                        
                    } else if ( element.prop( "type" ) === "radio" ) {
                        //console.log('radio');
                        error.insertAfter( element.closest('.form-group').children().last() );
                    } else {
                        //console.log('normal');
                        if(page == "Login" || page == "RecuperarSenha" || page == "AlterarSenhaLogin") {
                            error.insertAfter( element.closest('.form-group').children().first());
                        } else if ( element.hasClass('data') ){
                            error.insertAfter( element.closest('.form-group').children().last());
                        } else {
                            error.insertAfter( element );
                        }
                    }

                    
                    // Add the span element, if doesn't exists, and apply the icon classes to it.
                    if ( !element.next( "span" )[ 0 ] ) {

                        if (hasTableCheck != 0) {
                            $( "<span class='glyphicon glyphicon-remove form-control-feedback'></span>" ).insertAfter( element.closest('.tabela-check') );
                        } else {
                            $( "<span class='glyphicon glyphicon-remove form-control-feedback'></span>" ).insertAfter( element );
                        }
                    }
                },
                success: function ( label, element ) {
                    // Add the span element, if doesn't exists, and apply the icon classes to it.
                    if ( !$( element ).next( "span" )[ 0 ] ) {
                        $( "<span class='glyphicon glyphicon-ok form-control-feedback'></span>" ).insertAfter( $( element ) );
                    } else if ( !$( element ).next( "span" )[ 0 ] ) {
                        $( "<span class='glyphicon glyphicon-ok form-control-feedback'></span>" ).insertAfter( $( element ) );
                    }
                },
                highlight: function ( element, errorClass, validClass ) {
                    $( element ).closest('.form-group').addClass( "has-error" ).removeClass( "has-success" );
                    $( element ).next( "span" ).addClass( "glyphicon-remove" ).removeClass( "glyphicon-ok" );
                    //$( element ).closest(".form-group").find( "em" ).addClass( "arrow_box" );
                },
                unhighlight: function ( element, errorClass, validClass ) {
                    $( element ).closest(".form-group").addClass( "has-success" ).removeClass( "has-error" );
                    $( element ).next( "span" ).addClass( "glyphicon-ok" ).removeClass( "glyphicon-remove" );
                    //$( element ).closest(".form-group").find( "em" ).removeClass( "arrow_box" );
                }

        });

    },

    verifyGroupValidateChecked : function (_nameGroup) {

        $.validator.addMethod("groupValidateChecked ", function(){
            return ($('[name='+_nameGroup+']:checked').size() != 0);
        });

    },

    cpfIsValid : function () {

        $.validator.addMethod("cpf", function(value, element) {
            value = jQuery.trim(value);

            value = value.replace('.','');
            value = value.replace('.','');
            cpf = value.replace('-','');
            while(cpf.length < 11) cpf = "0"+ cpf;
            var expReg = /^0+$|^1+$|^2+$|^3+$|^4+$|^5+$|^6+$|^7+$|^8+$|^9+$/;
            var a = [];
            var b = new Number();
            var c = 11;
            for (i=0; i<11; i++){
                a[i] = cpf.charAt(i);
                if (i < 9) b += (a[i] * --c);
            }
            if ((x = b % 11) < 2) { a[9] = 0 } else { a[9] = 11-x }
            b = 0;
            c = 11;
            for (y=0; y<10; y++) b += (a[y] * c--);
            if ((x = b % 11) < 2) { a[10] = 0; } else { a[10] = 11-x; }

            var retorno = true;
            if ((cpf.charAt(9) != a[9]) || (cpf.charAt(10) != a[10]) || cpf.match(expReg)) retorno = false;

            return this.optional(element) || retorno;

        });

    },

    cnpjIsValid : function () {
        
        $.validator.addMethod("cnpj", function(value, element) {
            cnpj = value.replace(/[^\d]+/g, '');
            if (cnpj == '') return false;
            if (cnpj.length != 14)
                return false;
            // Elimina CNPJs invalidos conhecidos
            if (cnpj == "00000000000000" ||
                cnpj == "11111111111111" ||
                cnpj == "22222222222222" ||
                cnpj == "33333333333333" ||
                cnpj == "44444444444444" ||
                cnpj == "55555555555555" ||
                cnpj == "66666666666666" ||
                cnpj == "77777777777777" ||
                cnpj == "88888888888888" ||
                cnpj == "99999999999999")
                return false;
        
            // Valida DVs
            tamanho = cnpj.length - 2;
            numeros = cnpj.substring(0, tamanho);
            digitos = cnpj.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0))
                return false;
        
            tamanho = tamanho + 1;
            numeros = cnpj.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;
        
            return true;
        
        });

    },


    //Nao Conformidade
    //Tabela de Ação imediata

    verifyTableAcaoImediata : function () {
        
        $.validator.addMethod("verifyTableAcaoImediata ", function(){
            console.log('opa')
            return $('[name=formAcaoImadiataJustificativa]').val() != "" ? true : false;
        });

    },

    modelMsgErro : function () {

        //

    },

};