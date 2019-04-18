/*
|--------------------------------------------------------------------------
| Compare Select List
|--------------------------------------------------------------------------
*/

APP.component.SelectListCompare = {

	init : function (_listResult, _listPage, _idSelect, _paramCompare, _paramTexto) {

		this.setup();
		this.selectList(_listResult, _listPage, _idSelect, _paramCompare, _paramTexto);

	},

	setup : function () {

		//

	},

	selectList : function (_listResult, _listPage, _idSelect, _paramCompare, _paramTexto) {
		var array = _listResult;
		var anotherOne = [];
		var obj = {};

		if(_listPage.val() == undefined)
			_listPage = [0];

		$(_listPage).each(function (key, value) {
			obj = {[_paramCompare.valueOf()] : parseInt($(value).val())};
			anotherOne.push(obj);
		});

		var filteredArray = array.filter(myCallBack);

		function myCallBack(el){
			return anotherOne.findIndex(x=>x[`${_paramCompare}`] == el[`${_paramCompare}`]) < 0;
		}

		this.addSelectOnPage(filteredArray, _idSelect, obj, _paramTexto);

	},

	myCallBack : function (el) {

		return anotherOne.indexOf(el) < 0;

	},

	addSelectOnPage : function (filteredArray, _idSelect, obj, _paramTexto) {


		$(filteredArray).each(function (key, value) {

			var keyName = Object.keys(obj)[0];
			var $option = $('<option>');
			$(_idSelect).append($option.val(value[keyName]).text(value[_paramTexto.valueOf()]));

		});

	},
 
};