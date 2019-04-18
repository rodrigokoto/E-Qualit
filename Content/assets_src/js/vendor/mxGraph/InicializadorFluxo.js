var editor;

// Program starts here. The document.onLoad executes the
// mxApplication constructor with a given configuration.
// In the config file, the mxEditor.onInit method is
// overridden to invoke this global function as the
// last step in the editor constructor.
function onInit(editor) {
    // Enables rotation handle
    mxVertexHandler.prototype.rotationEnabled = true;

    // Enables guides
    mxGraphHandler.prototype.guidesEnabled = true;

    // Alt disables guides
    mxGuide.prototype.isEnabledForEvent = function (evt) {
        return !mxEvent.isAltDown(evt);
    };

    // Enables snapping waypoints to terminals
    mxEdgeHandler.prototype.snapToTerminals = true;

    // Defines an icon for creating new connections in the connection handler.
    // This will automatically disable the highlighting of the source vertex.
    mxConnectionHandler.prototype.connectImage = new mxImage('./javascript/src/images/connector.gif', 16, 16);

    // Enables connections in the graph and disables
    // reset of zoom and translate on root change
    // (ie. switch between XML and graphical mode).
    editor.graph.setConnectable(true);

    // Clones the source if new connection has no target
    editor.graph.connectionHandler.setCreateTarget(true);

    // Changes the zoom on mouseWheel events
    mxEvent.addMouseWheelListener(function (evt, up) {
        if (!mxEvent.isConsumed(evt)) {
            if (up) {
                editor.execute('zoomIn');
            }
            else {
                editor.execute('zoomOut');
            }

            mxEvent.consume(evt);
        }
    });
}

function LoadFlow(editor) {
    var graph = editor.graph;
    var xmlString = document.getElementById("hiddenFluxo").value;
    var doc = mxUtils.parseXml(xmlString);
    var node = doc.documentElement;
    editor.readGraphModel(node);
}

function CarregarFluxo(editor, valor) {
    var graph = editor.graph;
    var xmlString = valor;
    var doc = mxUtils.parseXml(xmlString);
    var node = doc.documentElement;
    editor.readGraphModel(node);
}

$(document).ready(function () {

    // Carrega o arquivo de Configurações Utilizado pelo MxGraph
    var config = mxUtils.load('./javascript/src/config/diagrameditor.xml').getDocumentElement();

    // Carrega o editor
    var editor = new mxEditor(config);

    // Inicializa o componente
    onInit(editor);

    $("#Copy").click(function () {
        editor.execute('copy');
    });

    $("#Paste").click(function () {
        editor.execute('paste');
    });

    $("#Delete").click(function () {
        editor.execute('delete');
    });

    $("#Undo").click(function () {
        editor.execute('undo');
    });

    $("#Redo").click(function () {
        editor.execute('redo');
    });

    $("#Cut").click(function () {
        editor.execute('cut');
    });

    $(".btn-SaveDocumento-ControlDoc").click(function () {
        var graph = editor.graph;
        var encoder = new mxCodec();
        var node = encoder.encode(graph.getModel());
        var dadosxml = mxUtils.getXml(node);

        document.getElementById("hiddenFluxo").value = dadosxml;
        alert(dadosxml);
    });

    $("#View").click(function () {
        var graph = editor.graph;
        mxUtils.show(graph, null, 100, 200);
    });

    //$("#ExportPdf").click(function () {
    //    window.open("@Url.Action("PDFPadrao", "Fluxo")")
    //});

    $("#zoomActual").click(function () {
        var graph = editor.graph;
        graph.zoomActual();
        graph.view.rendering = true;
        graph.refresh();
    });

    $("#zoomIn").click(function () {
        var graph = editor.graph;
        graph.zoomIn();
        graph.view.rendering = true;
        graph.refresh();
    });

    $("#zoomOut").click(function () {
        var graph = editor.graph;
        graph.zoomOut();
        graph.view.rendering = true;
        graph.refresh();
    });

    $("#zoomFit").click(function () {
        var graph = editor.graph;
        graph.fit();
        graph.view.rendering = true;
        graph.refresh();
    });

    $("#unselectAll").click(function () {
        var graph = editor.graph;
        graph.clearSelection();
    });

    $("#selectAll").click(function () {
        var graph = editor.graph;
        graph.selectAll();
    });

    $('#panel').on('onFullScreen.lobiPanel', function (ev, lobiPanel) {
        $('#graphContainer').height($("#panelBody").height() - 50);
    });

    $('#panel').on('onSmallSize.lobiPanel', function (ev, lobiPanel) {
        $('#graphContainer').height(300);
    });

    $("[data-fluxo]").each(function (event) {
        if ($(this).data("fluxo") == "auto")
        {
            LoadFlow(editor);
        }
    });
});

// window.onbeforeunload = function () { return mxResources.get('changesLost'); };

