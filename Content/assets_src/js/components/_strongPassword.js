/*
|--------------------------------------------------------------------------
| Strong Password
|--------------------------------------------------------------------------
*/

APP.component.StrongPassword = {

    init: function (_button) {

        this.setup(_button);
        this.setStrongPassword(_button);
        this.setRuleStrongPassword();

    },

    setup: function (_button) {

        this.buttonChange = $("[name='"+ _button + "']");

    },

    setStrongPassword : function (_button) {
        this.buttonChange.on('input', function (){

            var pwd = $("[name='"+ _button + "']").val();

            var array = [];
            //Zero or more uppercase characters
            array[0] = pwd.match(/[A-Z]/);
            //Zero or more lowercase characters
            array[1] = pwd.match(/[a-z]/);
            //Zero or more decimal digits
            array[2] = pwd.match(/\d/);
            //Zero or more non-word characters (!, £, $, %, etc.)
            array[3] = pwd.match(/[!_.-]/);
            
            var sum = 0;
            for (var i=0; i<array.length; i++) {
                sum += array[i] ? 1 : 0;
            }
            
            var label = APP.component.StrongPassword.getLabelStrongPassword();

            switch (sum) {
                case 0: 
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[0]);
                break;
                case 1: 
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[1]);
                break;
                case 2: 
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[2]);
                break;
                case 3: 
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[3]);
                break;
                case 4: 
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[4]);
                break;
                default: 
                    APP.component.StrongPassword.setHtmlStrongPassword(this, label[0]);
                break;
            }

        });

    },

    getLabelStrongPassword : function () {
        
        var arrayLabelStrongPassword = [];
        var strongPassword = $('input[name^=lblStrongPassword]');

        strongPassword.each(function (i){

            arrayLabelStrongPassword[i] = $(this).val();

        });

        return arrayLabelStrongPassword;

    },

    setHtmlStrongPassword : function (e, _info) {

        var classe = APP.component.RemoveAcentos.init(_info).toLowerCase();

        var html = "";
        $('.pwd-validate').remove();
        html += '<div class="pwd-validate">';
        html += '<div class="pwd-validate-info">';
        html += '<span class="pwd-validate-info-'+ classe +'">'+ _info +'</span>';
        html += '</div>';
        html += '<div class="pwd-validate-bar pwd-validate-bar-'+ classe +'">';
        html += '</div>';
        html += '</div>';

        $(e).closest('.form-group').append(html);

    },

    setRuleStrongPassword : function () {
        
        $.validator.addMethod("strongPassword", function(){
            var statusForce = APP.component.StrongPassword.getStrongPasswordValue();
            return ($('[name=formAlterarSenhaUtilizaSenhaForte]').val() == statusForce);
        });

    },

    getStrongPasswordValue : function () {
        
        var force = $('span[class^=pwd-validate-info-]').attr('class');
        force = force.split('info-');
        var statusForce = force[1];

        if (statusForce == "forte" || statusForce == "muito-forte") {
            return "true";
        } else {
            return "false";
        }

    },

};