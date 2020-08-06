

function imprimir() {


    APP.component.Loading.showLoading();

    var isControlada = null;

    $(".containerGraph").show();
    gerarPdfList(isControlada);
    $(".containerGraph").hide();

}

function gerarPdfList(isControlada) {
    if (document.getElementById('graphContainer') != null) {
        var svg = document.getElementById('graphContainer').children[0];
        if (svg !== undefined) {
            var copy = svg.cloneNode(true);
            copyStylesInline(copy, svg);
            var canvas = document.createElement("canvas");
            var bbox = svg.getBBox();
            canvas.width = svg.clientWidth;
            canvas.height = svg.clientHeight;
            var ctx = canvas.getContext("2d");
            ctx.clearRect(0, 0, bbox.width, bbox.height);
            var data = (new XMLSerializer()).serializeToString(copy);
            var DOMURL = window.URL || window.webkitURL || window;
            var img = new Image();
            var svgBlob = new Blob([data], { type: "image/svg+xml;charset=utf-8" });
            var url = DOMURL.createObjectURL(svgBlob);
            img.onload = function () {
                ctx.drawImage(img, 0, 0);
                DOMURL.revokeObjectURL(url);
                var fluxoBase64 = canvas.toDataURL("image/png");

                downloadPdfList(isControlada, fluxoBase64);
            };
            img.src = url;
        }
        else {
            downloadPdfList(isControlada, '');
        }
    }
    else {
        downloadPdfList(isControlada, '');
    }

}



function downloadPdfList(isControlada, fluxoBase64) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/ControlDoc/PDFList', true);
    xhr.responseType = 'arraybuffer';
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

    xhr.onload = function (e) {
        if (this.status == 200) {
            var blob = new Blob([this.response], { type: "application/pdf" });
            var pdfUrl = URL.createObjectURL(blob);
            printJS(pdfUrl);
        }
        APP.controller.ControlDocController.models.iscontrolada = null;
        APP.controller.ControlDocController.models.idusuariodestino = null;

        APP.component.Loading.hideLoading();

    };

    xhr.send(JSON.stringify({ "controlada": isControlada, "fluxoBase64": fluxoBase64 }));
}