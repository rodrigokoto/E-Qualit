
/*
|--------------------------------------------------------------------------
| Máscaras
|--------------------------------------------------------------------------
*/

APP.component.Mascaras = {

    init: function () {
 
        this.mascara();
        this.mascaraCelular();

    },

    mascara: function () {
 
        $('.input-cpf').mask('000.000.000-00', {reverse: true});
        $('.input-telefone').mask('(00) 0000-0000'); 
        $('.input-cep').mask('00000-000');
        $('.input-data').mask('00/00/0000'); 
        $('.input-cnpj').mask('00.000.000/0000-00');
 
    },

    mascaraCelular: function () {

        $('.input-celular').focusout(function () {

            var elemento, elementoThis; elementoThis = $(this);
            elementoThis.unmask(); elemento = elementoThis.val().replace(/\D/g, "");

            if (elemento.length > 10) {
                elementoThis.mask("(00) 90000-0000",{
                    placeholder: "(__) _____-____",
                });
            }
            else { 
                elementoThis.mask("(00) 0000-0000",{
                    placeholder: "(__) ____-____",
                });
             }

        }).trigger("focusout"); 

    },

     mascaraDinheiro: function () {

         $(".input-money").maskMoney({
                prefix:'R$ ', allowNegative: true,
                thousands:'.',
                decimal:',',
                affixesStay: false
        });

    }


}; 
