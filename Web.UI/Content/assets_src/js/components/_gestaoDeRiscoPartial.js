/*
|--------------------------------------------------------------------------
| GestaoDeRiscoPartial
|--------------------------------------------------------------------------
*/

APP.component.GestaoDeRiscoPartial = {
    
        init: function (_divGestaoDeRisco, _temaSelected) {
    
            var page = APP.component.Util.getPage();
    
            this.setup();
    
            if (page == "CriarAnaliseCritica") {
                this.gestaoDeRiscoCriarAnaliseCritica(_divGestaoDeRisco, _temaSelected);
                //this.gestaoDeRisco(_selector);
            }
            if (page == "EditarAnaliseCritica") {
                this.gestaoDeRiscoEditAnaliseCritica();
            }
    
        },
    
        setup: function () {
    
            //this.buttonNome = $('');
           
        },
    
        gestaoDeRiscoCriarAnaliseCritica : function (_divGestaoDeRisco, _temaSelected) {
    
            this.getPartialGestaoDeRisco(_divGestaoDeRisco, _temaSelected);
            this.setCkeditorGestaoDeRisco(_divGestaoDeRisco, _temaSelected);
            this.setBarRating(_divGestaoDeRisco);
            
            this.setCriticidade();
            this.setERisco();
            this.getERisco();


            var numeroRisco = $('[name=numeroAC]').val();
            var busca = ".gestaoDeRiscoPartial-" + _temaSelected;
            $(busca).find($('[name=formGestaoDeRiscoNumero]')).val(numeroRisco);
            $('[name=numeroAC]').val(parseInt(numeroRisco) + parseInt('1'));

        },
    
        //Gestão de Risco - Criar Analise Critica
        getPartialGestaoDeRisco : function (_divGestaoDeRisco, _temaSelected) {
    
            $.ajax({
                type: "POST",
                async: false,
                url: `/GestaoDeRisco/ObterPartialGestaoDeRisco`,
                beforeSend: function(){
                    APP.component.Loading.showLoading();
                },
                success: function (result) {
                    _divGestaoDeRisco.html(result);
                    _divGestaoDeRisco.find('[name=formGestaoDeRiscoRisco]').attr('name', 'formGestaoDeRiscoRisco-'+_temaSelected);
                    APP.component.GestaoDeRiscoPartial.setHideAndShowGestaodeRisco(_divGestaoDeRisco);
                },
                error: function (result) {
                    bootbox.alert("Ocorreu um erro!");
                },
                complete: function (result) {
                    APP.component.Loading.hideLoading();
                }
            });
    
        },
    
        setCkeditorGestaoDeRisco : function (_divGestaoDeRisco, _temaSelected) {
            
            $(_divGestaoDeRisco).find('textarea').first().attr('id', _temaSelected);
            var ckeditorID = $(_divGestaoDeRisco).find('textarea').first().attr('id');
            APP.component.CkEditor.init(ckeditorID);
    
        },
    
        setBarRating : function (_divGestaoDeRisco) {
    
            var divBarRating = $(_divGestaoDeRisco).find('.barraRating');
            APP.component.BarRating.setBarRatingGestaoDeRisco(divBarRating, 'bars-1to10');
    
        },
    
        setHideAndShowGestaodeRisco : function (_divGestaoDeRisco) {
    
            //_divGestaoDeRisco.find('[name^=formGestaoDeRiscoRisco]').closest('[class^=col]').hide();
            _divGestaoDeRisco.find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').hide();
            _divGestaoDeRisco.find('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').hide();
            _divGestaoDeRisco.find('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').hide();
            //_divGestaoDeRisco.find('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').hide();
    
        },
    
        //Changes
        setCriticidade : function (value, text) {
            
            $('[name^=formGestaoDeRiscoCriticidade]').on('change', function () {
    
                var barRatingSelect = $(this).val();
                if (barRatingSelect == 2 || barRatingSelect == 3) {
                    $(this).closest('#gestaoDeRisco').find('[name^=formGestaoDeRiscoRisco]').closest('[class^=col]').show();
                    APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(this);
                } else {
                    $(this).closest('#gestaoDeRisco').find('[name^=formGestaoDeRiscoRisco]').attr('checked',false);
                    $(this).closest('#gestaoDeRisco').find('[name^=formGestaoDeRiscoRisco]').closest('[class^=col]').hide();
                    $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').hide();
                    $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').hide();
                    $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').hide();
                    $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').hide();
                }
    
            });
        
        },
        
        setERisco : function () {
            
            $('[name^=formGestaoDeRiscoRisco]').unbind('change');
            $('[name^=formGestaoDeRiscoRisco]').on('change', function (){
				
                var ERisco = APP.component.GestaoDeRiscoPartial.getERisco(this);
                APP.component.GestaoDeRiscoPartial.setRulesERisco(ERisco, this);
    
            });
    
        },
    
        
    
        //Auxiliares
        getERisco : function (_this) {
    
            var ERisco = $(_this).val();
            return ERisco;
    
        },
    
        //Rules
        setRulesERisco : function (_ERisco, _this) {
            
            if (_ERisco == "true") {
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').show();
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').show();
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoNumero]').prop('disabled', true);
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').show();
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').show();
                APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(this);
            } else {
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').hide();
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').hide();
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').hide();
                $(_this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').hide();
            }
    
        },
    
        setRulesCriticidade : function (_barRatingSelect) {
           
            if (barRatingSelect == 2 || barRatingSelect == 3) {
                $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').show();
                $('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').show();
                $('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').show();
                $('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').show();
                APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(this);
            } else {
                $('[name=formGestaoDeRiscoResponsavelDefinicao]').closest('[class^=col]').hide();
                $('[name=formGestaoDeRiscoNumero]').closest('[class^=col]').hide();
                $('[name=formGestaoDeRiscoCausa]').closest('[class^=col]').show();
                $('[name=formGestaoDeRiscoIdentificacao]').closest('[class^=col]').hide();
            }
    
        },
    
        getRadioPossuiGestaoDeRisco : function () {
            
            $('[name^=formGestaoDeRiscoRisco]').unbind('change');
			$('[name^=formGestaoDeRiscoRisco]').bind('change', function () {
				
                var radioPossuiGestaoDeRisco = APP.component.Radio.init('formGestaoDeRiscoRisco');
                if (radioPossuiGestaoDeRisco == "true") {
                    $(this).closest('#gestaoDeRisco').find('[name=InformacoesGestaoDeRisco]').show();
                    $(this).closest('#gestaoDeRisco').find('.responsavel-gestao-de-risco').show();
                    $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoCausa]').show();
                    APP.controller.AnaliseCriticaController.getTodosResponsaveisPorAcaoImediata(this);
    
                } else {
                    $(this).closest('#gestaoDeRisco').find('[name=InformacoesGestaoDeRisco]').hide();
                    $(this).closest('#gestaoDeRisco').find('.responsavel-gestao-de-risco').hide();
                    $(this).closest('#gestaoDeRisco').find('[name=formGestaoDeRiscoCausa]').hide();
                }
    
            });
    
        },
    
       
    
        //Gestão de Risco - Editar Analise Critica
        gestaoDeRiscoEditAnaliseCritica : function () {
    
            APP.component.BarRating.init('.barraRating', 'bars-1to10');
                
            $(('[id^=DsTexto]')).each(function (key, value) {
                
                var idDsTexto = $(this).attr('id');
                APP.component.CkEditor.init('#' + idDsTexto);
            });
    
            this.setDisableForm();
    
        },
    
        setDisableForm : function () {
    
            $('[name=options2]').prop("disabled", true);
    
        },
    
        setInfosEditGestaoDeRisco : function () {
            var selectRisco = $('.tema-cor-risco').val();
            //$('.rating').closest('div').next();data('');
    
        },
    
    };