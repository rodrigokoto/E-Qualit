function validaData(value) {
    var err = 0;
    var psj = 0;

    if (value.length == 0 || value == "undefined") {
        return true
    }
    if ((value.length < 10) || (value.length > 10 && value.length < 16)) {
        return false;
    }
    if (value.length == 10) {
        return chkValidaData(value);
    }
    else {
        aDataHora = value.split(" ");
        var bData = chkValidaData(aDataHora[0]);
        var bHora = chkValidaHora(aDataHora[1]);
        return bData && bHora;
    }
}

function chkValidaData(value) {
    var err = 0;
    var psj = 0;
    if (value.length == 0 || value == "undefined") {
        return true
    }
    if (value.length < 10) {
        err = 1
    }
    b = value.substring(0, 2);
    c = value.substring(2, 3);
    d = value.substring(3, 5);
    e = value.substring(5, 6);
    f = value.substring(6, 10);
    if (b < 1 || b > 31) {
        err = 1
    }
    if (c != "/") {
        err = 1
    }
    if (d < 1 || d > 12) {
        err = 1
    }
    if (e != "/") {
        err = 1
    }
    if (f < 1913 || f > 2049) {
        err = 1
    }
    if (d == "04" || d == "06" || d == "09" || d == "11") {
        if (b == "31") {
            err = 1
        }
    }
    if (d == 2) {
        var g = parseInt(f / 4);
        if (isNaN(g)) {
            err = 1
        }
        if (b > 29) {
            err = 1
        }
        if (b == 29 && ((f / 4) != parseInt(f / 4))) {
            err = 1
        }
    }
    if (value.length == 0) {
        err = 0
    }
    if (err == 1) {
        return false
    } else {
        return true
    }
}

function chkValidaHora(value) {
    var valor = value.replace("_", "");
    valor = valor.replace("_", "");
    valor = valor.replace("_", "");
    valor = valor.replace("_", "");
    hora = valor.split(":");

    if (hora.length == 0) {
        return false;
    }

    //valida o formato
    if ((hora[0].trim().length < 2) || (hora[1].trim().length < 2)) {
        return false;
    }

    //valida os valores
    if ((hora[0] < 00) || (hora[0] > 23) || (hora[1] < 00) || (hora[1] > 59)) {
        return false;
    }
    return true;
}

function pad(str, max) {
    return str.length < max ? pad("0" + str, max) : str;
}

function CalculaCNPJ(CNPJ) {
    Numero_1 = parseInt(CNPJ.charAt(0))
    Numero_2 = parseInt(CNPJ.charAt(1))
    Numero_3 = parseInt(CNPJ.charAt(2))
    Numero_4 = parseInt(CNPJ.charAt(3))
    Numero_5 = parseInt(CNPJ.charAt(4))
    Numero_6 = parseInt(CNPJ.charAt(5))
    Numero_7 = parseInt(CNPJ.charAt(6))
    Numero_8 = parseInt(CNPJ.charAt(7))
    Numero_9 = parseInt(CNPJ.charAt(8))
    Numero_10 = parseInt(CNPJ.charAt(9))
    Numero_11 = parseInt(CNPJ.charAt(10))
    Numero_12 = parseInt(CNPJ.charAt(11))
    Numero_13 = parseInt(CNPJ.charAt(12))
    Numero_14 = parseInt(CNPJ.charAt(13))

    soma = "";
    soma = Numero_1 * 5 + Numero_2 * 4 + Numero_3 * 3 + Numero_4 * 2 + Numero_5 * 9 + Numero_6 * 8 + Numero_7 * 7 + Numero_8 * 6 + Numero_9 * 5 + Numero_10 * 4 + Numero_11 * 3 + Numero_12 * 2;
    soma = soma - (11 * (parseInt(soma / 11)));

    if ((soma == 0) || (soma == 1)) {
        resultado1 = 0
    } else {
        resultado1 = 11 - soma
    }

    if (resultado1 == Numero_13) {
        soma = Numero_1 * 6 + Numero_2 * 5 + Numero_3 * 4 + Numero_4 * 3 + Numero_5 * 2 + Numero_6 * 9 + Numero_7 * 8 + Numero_8 * 7 + Numero_9 * 6 + Numero_10 * 5 + Numero_11 * 4 + Numero_12 * 3 + Numero_13 * 2
        soma = soma - (11 * (parseInt(soma / 11)))
    }

    if ((soma == 0) || (soma == 1)) {
        resultado2 = 0
    } else {
        resultado2 = 11 - soma
    }

    if (resultado2 == Numero_14) {
        return true;
    } else {
        return false;
    }
}

function CalculaCPF(CPF) {
    Numero_1 = parseInt(CPF.charAt(0))
    Numero_2 = parseInt(CPF.charAt(1))
    Numero_3 = parseInt(CPF.charAt(2))
    Numero_4 = parseInt(CPF.charAt(3))
    Numero_5 = parseInt(CPF.charAt(4))
    Numero_6 = parseInt(CPF.charAt(5))
    Numero_7 = parseInt(CPF.charAt(6))
    Numero_8 = parseInt(CPF.charAt(7))
    Numero_9 = parseInt(CPF.charAt(8))
    Numero_10 = parseInt(CPF.charAt(9))
    Numero_11 = parseInt(CPF.charAt(10))
    cont = 0

    for (x = 1; x < 12; x++) {
        if (Numero_1 == eval('Numero_' + x)) {
            cont++;
        }
    }

    if (cont == 11) {
        return false;
    }

    soma = 10 * Numero_1 + 9 * Numero_2 + 8 * Numero_3 + 7 * Numero_4 + 6 * Numero_5 + 5 * Numero_6 + 4 * Numero_7 + 3 * Numero_8 + 2 * Numero_9
    soma = soma - (11 * (parseInt(soma / 11)))
    if ((soma == 0) || (soma == 1)) {
        resultado1 = 0
    } else {
        resultado1 = 11 - soma
    }
    if (resultado1 == Numero_10) {
        soma = Numero_1 * 11 + Numero_2 * 10 + Numero_3 * 9 + Numero_4 * 8 + Numero_5 * 7 + Numero_6 * 6 + Numero_7 * 5 + Numero_8 * 4 + Numero_9 * 3 + Numero_10 * 2
        soma = soma - (11 * (parseInt(soma / 11)))
    }
    if ((soma == 0) || (soma == 1)) {
        resultado2 = 0
    } else {
        resultado2 = 11 - soma
    }
    if (resultado2 == Numero_11) {
        return true;
    } else {
        return false;
    }
}

function verifica(val) {
    var texto = val.replace(".", "");
    var texto = texto.replace(".", "");
    var texto = texto.replace(".", "");
    var texto = texto.replace("-", "");
    var texto = texto.replace("-", "");
    var texto = texto.replace("/", "");

    if (texto.length <= 11) {
        return CalculaCPF(pad(texto, 11));
    }
    else {
        return CalculaCNPJ(pad(texto, 14));
    }
}

$(function () {
    jQuery.validator.addMethod("validadata", function (value, element, param) {
        return validaData(value);
    });
    jQuery.validator.unobtrusive.adapters.addBool("validadata");

    jQuery.validator.addMethod("validanasc", function (value, element, param) {
        var aData = value.split("/");

        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        var dNascimento = new Date(aData[2], aData[1], aData[0]);
        var dHoje = new Date(yyyy, mm, dd);

        if (dNascimento > dHoje) {
            return false;
        }

        var difAno = parseInt(yyyy) - parseInt(aData[2]);
        if (difAno < 18) {
            element.value = "";
            bootbox.alert("Caso deseje realizar a solicitação para menores de 18 anos, por favor, entre em contato com seu gerente de relacionamento", function () {
                $("#" + element.id).focus();
            });
            return false;
        }
        return validaData(value);
    });
    jQuery.validator.unobtrusive.adapters.addBool("validanasc");

    jQuery.validator.addMethod("validacpfcnpj", function (value, element, param) {
        if ($.trim(value) == "") return true;
        var retorno = verifica(value);
        return retorno;
    });
    jQuery.validator.unobtrusive.adapters.addBool("validacpfcnpj");


    jQuery.validator.addMethod("validalimitcount", function (value, element, param) {
        var retorno = $("input[name=" + element.name + "]:checked").length > 1;
        return retorno;
    });
    jQuery.validator.unobtrusive.adapters.addBool("validalimitcount");
});
