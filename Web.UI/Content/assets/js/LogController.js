

/*
|--------------------------------------------------------------------------
| Controlador Log
|--------------------------------------------------------------------------
*/

APP.controller.LogController = {
    init: function () {

        var page = APP.component.Util.getPage();
        this.setup();

        if (page == "IndexLog") {
            this.indexLog();
        }
    },

    setup: function () {

    },

    indexLog: function () {

        APP.component.DataTable.setDataTableParam('#tb-log', 0, "desc", "Tags");
    },
}