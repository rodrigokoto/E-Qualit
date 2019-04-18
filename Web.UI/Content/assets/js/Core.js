
/*
|--------------------------------------------------------------------------
| Core
|--------------------------------------------------------------------------
*/

APP.core.Main = {

    init: function () {
        this.loadPageController();
        APP.controller.General.init();
    },

    loadPageController: function () {

        var ctrl = APP.component.Util.getController();

        if (ctrl) {
            APP.controller[ctrl].init();
        }

    },
    downloadArquivo: function () {
        $("[download]").on("click", function (event) {
            event.preventDefault();

            $(".loading").show().focus();

            var filename = $(this).attr("download");
            var image_data = atob($(this).attr("href").split(',')[1]);
            var arraybuffer = new ArrayBuffer(image_data.length);
            var view = new Uint8Array(arraybuffer);
            var blob;

            for (var i = 0; i < image_data.length; i++) {
                view[i] = image_data.charCodeAt(i) & 0xff;
            }

            try {
                blob = new Blob([arraybuffer], { type: 'application/octet-stream' });
            } catch (e) {
                var bb = new (window.WebKitBlobBuilder || window.MozBlobBuilder);
                bb.append(arraybuffer);
                blob = bb.getBlob('application/octet-stream');
            }

            // Use the URL object to create a temporary URL
            var url = (window.webkitURL || window.URL).createObjectURL(blob);

            var a = document.createElement('a');
            a.setAttribute("download", filename);
            a.setAttribute("href", url);

            document.body.appendChild(a);

            a.click();

            setTimeout(() => {
                document.body.removeChild(a);
                $(".loading").hide();
            }, 700);
        });
    }
};

/*
|--------------------------------------------------------------------------
| Chamada
|--------------------------------------------------------------------------
*/
$(document).ready(function () {
    APP.core.Main.init();
    APP.core.Main.downloadArquivo();
});


