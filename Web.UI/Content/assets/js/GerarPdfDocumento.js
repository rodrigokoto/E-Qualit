function copyStylesInline(destinationNode, sourceNode) {
    var containerElements = ["svg", "g"];
    for (var cd = 0; cd < destinationNode.childNodes.length; cd++) {
        var child = destinationNode.childNodes[cd];
        if (containerElements.indexOf(child.tagName) != -1) {
            copyStylesInline(child, sourceNode.childNodes[cd]);
            continue;
        }
        var style = sourceNode.childNodes[cd].currentStyle || window.getComputedStyle(sourceNode.childNodes[cd]);
        if (style == "undefined" || style == null) continue;
        for (var st = 0; st < style.length; st++) {
            child.style.setProperty(style[st], style.getPropertyValue(style[st]));
        }
    }
}

function gerarPdf(idDocumento, isControlada, idUsuarioDestino) {
    if (document.getElementById('graphContainer') != null) {
        var svg = document.getElementById('graphContainer').children[0];
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

            APP.controller.ControlDocController.downloadPdf(idDocumento, isControlada, idUsuarioDestino, fluxoBase64);
        };
        img.src = url;
    }
    else {
        APP.controller.ControlDocController.downloadPdf(idDocumento, isControlada, idUsuarioDestino, '');
    }

}
