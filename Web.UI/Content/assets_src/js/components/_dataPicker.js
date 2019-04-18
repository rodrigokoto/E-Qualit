/*
|--------------------------------------------------------------------------
| Data Picker
|--------------------------------------------------------------------------
*/

APP.component.Datapicker = {

    init: function () {
		
		this.setup();
		this.ativaDataPicker();
		this.ativaTimePicker();
		this.setIconDataPicker();
		this.setIconTimePicker();

	},
	
	setup : function () {

		//

	},

    ativaDataPicker: function () {
 
		// jQuery.datetimepicker.setLocale(jsIdioma);
		// $('.data:not([readonly])').datetimepicker({
		// 	format: 'd/m/Y',
		// 	lang: jsIdioma,
		// 	timepicker: false,
		// 	scrollInput: false
		// });
		// $(".data").attr("autocomplete", "off");
		// $(".data").g2itMasks("date");

		$('.data').datepicker({
			dateFormat: "dd/mm/yy",
			minDate: new Date()
		});
 
	},

	setDataPicker : function (_this,_seletor) {

		var arr = [];
		var dataSelected = $(_this).val();
		var strNewFormat = dataSelected.split('/');
		arr.push(strNewFormat[1]);
		arr.push(strNewFormat[0]);
		arr.push(strNewFormat[2]);

		var data = new Date(arr.join('/'));
		
		$(_seletor).datepicker( "option", "minDate", data);

	},

	ativaTimePicker : function () {

		$('.time').timepicker({});

	},

	setIconDataPicker : function () {

		$('.fa-calendar').on('click', function () {
			
			$(this).closest('div').find('.data').trigger( "focusin" );

		});

	},

	setIconTimePicker : function () {

		$('.fa-clock-o').on('click', function () {
			
			$(this).closest('div').find('.time').trigger( "focus" );

		});

	},

}; 