/*!
 * Author: Silvestre Pinheiro de Freitas Jr.
 * Date: 4 Jan 2014
 * Description:
 *      This file should be included in all pages
 !**/

/*
 * Global variables. If you change any of these vars, don't forget 
 * to change the values in the less files!
 */

(function ($) {
    var config;
    var obj;
    var methods = {
        init: function (settings) {
            _init(settings, $(this));
            return this;
        },
        phone: function (options) {
            _init(options, $(this));
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                if (v.length <= 10) {
                    $(this).val(telefone(v));
                }
                else {
                    $(this).val(telefone1(v));
                }

                if ($(this).val().length > 15) $(this).val($(this).val().substring(0, 15));
            });
        },
        mobile: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                $(this).val(telefone1(v));

                if ($(this).val().length > 15) $(this).val($(this).val().substring(0, 15));
            });
        },
        cpf: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                if (v.length > 11) v = v.substring(0, 11);

                $(this).val(cpf(v));
            });
        },
        cnpj: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                if (v.length > 14) v = v.substring(0, 14);

                $(this).val(cnpj(v));
            });
        },
        cpfcnpj: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                if (v.length <= 11) {
                    $(this).val(cpf(v));
                }
                else {
                    $(this).val(cnpj(v));
                }

                if ($(this).val().length > 18) $(this).val($(this).val().substring(0, 18));
            });
        },
        cep: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                $(this).val(cep(v));

                if ($(this).val().length > 9) $(this).val($(this).val().substring(0, 9));
            });
        },
        date: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                $(this).val(data(v));

                if ($(this).val().length > 10) $(this).val($(this).val().substring(0, 10));
            });
        },
        datetime: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");
                v = v.substring(0, 12);

                $(this).val(datahorasegundo(v));
            });
        },
        datetimefull: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");
                v = v.substring(0, 14);

                $(this).val(datahorasegundo(v));
            });
        },
        integer: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                $(this).val(soNumeros(v));
            })
        },
        decimal: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");

                $(this).autoNumeric({ mNum: 10, mDec: 2, aSep: '', aDec: ',', aPad: true });
            })
        },
        currency: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");
                $(this).autoNumeric({ mNum: 10, mDec: 2, aSep: '.', aDec: ',', aPad: true });
            })
        },
        qtd: function () {
            $(this).bind("keyup", function () {
                var v = $(this).val().toString();
                var v = v.replace(/\D/g, "");
                $(this).autoNumeric({ mNum: 10, mDec: 4, aSep: '.', aDec: ',', aPad: true });
            })
        }
    };

    function _init(settings, _obj) {
        obj = _obj;
        config = {
            'ClassMaskphone': 'telefone',
            'ClassMaskCelular': 'celular',
            'ClassMaskCPF': 'cpf',
            'ClassMaskCNPJ': 'cnpj',
            'ClassMaskCPFCNPJ': 'cpfcnjp',
            'ClassMaskCEP': 'cep',
            'ClassMaskDate': 'data',
            'ClassMaskDateTime': 'datahora',
            'ClassMaskDateTimeFull': 'datafull',
            'ClassMaskNumber': 'numerico',
        };

        if (settings) { $.extend(config, settings); }
    }
    function telefone(v) {
        v = v.replace(/\D/g, "")                 //Remove tudo o que não é dígito
        v = v.replace(/^(\d\d)(\d)/g, "($1) $2") //Coloca parênteses em volta dos dois primeiros dígitos
        v = v.replace(/(\d{4})(\d)/, "$1-$2")    //Coloca hífen entre o quarto e o quinto dígitos
        return v
    }

    function telefone1(v) {
        v = v.replace(/\D/g, "")                 //Remove tudo o que não é dígito
        v = v.replace(/^(\d\d)(\d)/g, "($1) $2") //Coloca parênteses em volta dos dois primeiros dígitos
        v = v.replace(/(\d{5})(\d)/, "$1-$2")    //Coloca hífen entre o quarto e o quinto dígitos
        return v
    }

    function data(v) {
        v = v.replace(/\D/g, "")
        v = v.replace(/^(\d{2})(\d)/, "$1/$2")
        v = v.replace(/(\d{2})(\d)/, "$1/$2")
        return v
    }

    function datahorasegundo(v) {
        v = v.replace(/\D/g, "")
        if (v.length == 2) v = v.replace(/^(\d{2})/, "$1/")
        //Mes
        if (v.length == 3) v = v.replace(/^(\d{2})(\d{1})/, "$1/$2")
        if (v.length == 4) v = v.replace(/^(\d{2})(\d{2})/, "$1/$2/")

        //Ano
        if (v.length == 5) v = v.replace(/^(\d{2})(\d{2})(\d{1})/, "$1/$2/$3")
        if (v.length == 6) v = v.replace(/^(\d{2})(\d{2})(\d{2})/, "$1/$2/$3")
        if (v.length == 7) v = v.replace(/^(\d{2})(\d{2})(\d{3})/, "$1/$2/$3")
        if (v.length == 8) v = v.replace(/^(\d{2})(\d{2})(\d{4})/, "$1/$2/$3 ")

        //Hora
        if (v.length == 9) v = v.replace(/^(\d{2})(\d{2})(\d{4})(\d{1})/, "$1/$2/$3 $4")
        if (v.length == 10) v = v.replace(/^(\d{2})(\d{2})(\d{4})(\d{2})/, "$1/$2/$3 $4:")

        if (v.length == 11) v = v.replace(/^(\d{2})(\d{2})(\d{4})(\d{2})(\d{1})/, "$1/$2/$3 $4:$5")
        if (v.length == 12) v = v.replace(/^(\d{2})(\d{2})(\d{4})(\d{2})(\d{2})/, "$1/$2/$3 $4:$5")

        if (v.length == 13) v = v.replace(/^(\d{2})(\d{2})(\d{4})(\d{2})(\d{2})(\d{1})/, "$1/$2/$3 $4:$5:$6")
        if (v.length == 14) v = v.replace(/^(\d{2})(\d{2})(\d{4})(\d{2})(\d{2})(\d{2})/, "$1/$2/$3 $4:$5:$6")

        return v
    }

    function soNumeros(v) {
        return v.replace(/\D/g, "")
    }

    function cnpj(v) {
        v = v.replace(/\D/g, "")                           //Remove tudo o que não é dígito
        v = v.replace(/^(\d{2})(\d)/, "$1.$2")             //Coloca ponto entre o segundo e o terceiro dígitos
        v = v.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3") //Coloca ponto entre o quinto e o sexto dígitos
        v = v.replace(/\.(\d{3})(\d)/, ".$1/$2")           //Coloca uma barra entre o oitavo e o nono dígitos
        v = v.replace(/(\d{4})(\d)/, "$1-$2")              //Coloca um hífen depois do bloco de quatro dígitos
        return v
    }

    function cpf(v) {
        v = v.replace(/\D/g, "")                    //Remove tudo o que não é dígito
        v = v.replace(/(\d{3})(\d)/, "$1.$2")       //Coloca um ponto entre o terceiro e o quarto dígitos
        v = v.replace(/(\d{3})(\d)/, "$1.$2")       //Coloca um ponto entre o terceiro e o quarto dígitos
        //de novo (para o segundo bloco de números)
        v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2") //Coloca um hífen entre o terceiro e o quarto dígitos
        return v
    }

    function cep(v) {
        v = v.replace(/D/g, "")                //Remove tudo o que não é dígito
        v = v.replace(/^(\d{5})(\d)/, "$1-$2") //Esse é tão fácil que não merece explicações
        return v
    }

    $.fn.g2itMasks = function (method) {
        // Metódo de chamada
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.g2itMasks');
        }

        return this;
    };
})(jQuery);