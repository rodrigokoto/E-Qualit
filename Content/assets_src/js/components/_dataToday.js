/*
|--------------------------------------------------------------------------
| Data Today
|--------------------------------------------------------------------------
*/

APP.component.Datatoday = {

    init: function () {
        
        return this.getDataToday();

    },

    getDataToday: function () {
        
        var today = new Date(),
        DD = today.getDate(),
        MM = today.getMonth() + 1, //January is 0!
        YYYY = today.getFullYear();

        if (DD < 10) {
        DD = '0' + DD;
        }
        if (MM < 10) {
        MM = '0' + MM;
        }

        return today = DD + "/" + MM + "/" + YYYY;
       
    },

    getHoraNow : function () {

        new Date($.now());
        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();

        return time;

    },

    getDateUsFormat : function (_data) {

        // O formato da data contida na variável "data" é dd/mm/yyyy
        var split = _data.split('/');
        var novadata = split[1] + "/" +split[0]+"/"+split[2];
        var data_americana = new Date(novadata); 

        return data_americana;

    },

    getCompareDate : function (_dateNow, _dateLast) {

        var dataNow = APP.component.Datatoday.getDateUsFormat(_dateNow);
        var dataLast = APP.component.Datatoday.getDateUsFormat(_dateLast);
        if(new Date(dataNow).getTime() > new Date(dataLast).getTime())
        {
            return true;
        } else {
            return false;
        }

    }, 

}; 