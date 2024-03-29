/*
|--------------------------------------------------------------------------
| Remove Acentos
|--------------------------------------------------------------------------
*/

APP.component.RemoveAcentos = {
    
    init: function (_word) {

        this.setup();
        return this.setRemoveAcentos(_word);

    },

    setup: function (_buton) {

        //

    },

    setRemoveAcentos : function (_word) {
        
        var string = _word;
        var mapaAcentosHex 	= {
            a : /[\xE0-\xE6]/g,
            e : /[\xE8-\xEB]/g,
            i : /[\xEC-\xEF]/g,
            o : /[\xF2-\xF6]/g,
            u : /[\xF9-\xFC]/g,
            c : /\xE7/g,
            n : /\xF1/g,
            '-' : /\s/g
        };
    
        for ( var letra in mapaAcentosHex ) {
            var expressaoRegular = mapaAcentosHex[letra];
            string = string.replace( expressaoRegular, letra );
        }
    
        return string;

    },

};