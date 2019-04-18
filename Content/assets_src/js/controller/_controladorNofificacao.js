/*
|--------------------------------------------------------------------------
| ControladorNotificacaoController
|--------------------------------------------------------------------------
*/


APP.controller.NotificacaoController = {

    init: function () {
        this.setNotificacao();
        this.atualiza();
    },

    atualiza: function () {
        $.ajax({
            url: "/Notificacao",
            type: 'POST',
            data: {
                __RequestVerificationToken: _token
            },
            beforeSend: function () {
               
            },
            success: function (result) {

                $("#pnlNotificacoesUsuario").html(result);
                $('.badge').text($("[name=TotalNotificacoes]").val());
            },
            complete: function () {
                setTimeout(this.atualiza, 60000);
            },
            error: callBackError
        });
    },


    setNotificacao: function () {
        var html = "";
        html += '<li class="dropdown">';
        html += '<a class="dropdown-toggle" data-toggle="modal" data-target="#mdPendencia" href="#">';
        html += '<i class="fa fa-bell fa-2x" aria-hidden="true">  <span class="badge">0</span> </i>';
        html += '<span>Notificações</span> </i>';
        html += '</a>';
        html += '</li>';

        $('#ulLayout').append(html);
    }



};