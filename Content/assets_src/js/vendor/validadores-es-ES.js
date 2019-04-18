function validaData(value) {
    var err = 0;
    var psj = 0;
    if (value.length == 0 || value == "undefined") {
        return true
    }
    if (value.length != 10) {
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

function pad(str, max) {
    return str.length < max ? pad("0" + str, max) : str;
}
$(function () {
    jQuery.validator.addMethod("validadata", function (value, element, param) {
        return validaData(value);
    });
    jQuery.validator.unobtrusive.adapters.addBool("validadata");
});
