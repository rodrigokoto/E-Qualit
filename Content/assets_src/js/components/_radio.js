/*
|--------------------------------------------------------------------------
| Radio Sim Não
|--------------------------------------------------------------------------
*/

APP.component.Radio = {

    init: function (_nomeRadio) {

        this.setup();
        return this.getRadio(_nomeRadio);

    },

    setup: function () {

        //

    },

    getRadio: function (_nomeRadio) {
        var radio_val = $("input[name='" + _nomeRadio + "']:checked").val();
        return radio_val;
    },

};