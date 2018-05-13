var AnnotatorApiSettings =
    {
        baseUrl: "http://localhost:54520/",
        apiUrl: "http://localhost:54520/api",
    };

var annotatorApiFileLoadHelper = {};
annotatorApiFileLoadHelper.annotatorTouchCssLoaded = false;
annotatorApiFileLoadHelper.highlighterJsLoaded = false;

annotatorApiFileLoadHelper.addAnnotatorApiScript = function (filename, callBackFunction) {
    console.log(filename + ".js Loading");
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.src = AnnotatorApiSettings.baseUrl + "js/" + filename + ".js";
    script.type = 'text/javascript';
    if (callBackFunction) {
        script.onload = callBackFunction;
    }
    head.appendChild(script);
};

annotatorApiFileLoadHelper.addAnnotatorApiCss = function (filename, callBackFunction) {
    console.log(filename + ".css Loading");
    var head = document.getElementsByTagName('head')[0];
    var css = document.createElement('link');
    css.href = AnnotatorApiSettings.baseUrl + "css/" + filename + ".css";
    css.rel = "stylesheet";
    css.type = "text/css";
    if (callBackFunction) {
        css.onload = callBackFunction;
    }

    head.appendChild(css);
};

annotatorApiFileLoadHelper.annotatorCssLoadedHandler = function () {
    annotatorApiFileLoadHelper.addAnnotatorApiCss("annotator.touch", annotatorApiFileLoadHelper.annotatortouchCssLoadedHandler);
};

annotatorApiFileLoadHelper.annotatortouchCssLoadedHandler = function () {
    annotatorApiFileLoadHelper.annotatorTouchCssLoaded = true;
    annotatorApiFileLoadHelper.annotatorAllFilesLoadedHandler();
};

annotatorApiFileLoadHelper.annotatorJsLoadedHandler = function () {
    annotatorApiFileLoadHelper.addAnnotatorApiScript("annotator.touch.min", annotatorApiFileLoadHelper.annotatorTouchJsLoadedHandler);
};

annotatorApiFileLoadHelper.annotatorTouchJsLoadedHandler = function () {
    annotatorApiFileLoadHelper.addAnnotatorApiScript("highlighter", annotatorApiFileLoadHelper.highlighterJsLoadedLoadedHandler);
};

annotatorApiFileLoadHelper.highlighterJsLoadedLoadedHandler = function () {
    annotatorApiFileLoadHelper.highlighterJsLoaded = true;
    annotatorApiFileLoadHelper.annotatorAllFilesLoadedHandler();
};

annotatorApiFileLoadHelper.jqueryLoadedHandler = function () {
    annotatorApiFileLoadHelper.addAnnotatorApiCss("annotator.min", annotatorApiFileLoadHelper.annotatorCssLoadedHandler);
    annotatorApiFileLoadHelper.addAnnotatorApiScript("annotator-full.min", annotatorApiFileLoadHelper.annotatorJsLoadedHandler);
};

annotatorApiFileLoadHelper.loadAnnotatorApp = function () {
    console.log("Jquery Loading");
    if (typeof window.jQuery == 'undefined') {
        var head = document.getElementsByTagName("head")[0];
        var jquery = document.createElement('script');
        jquery.type = 'text/javascript';
        jquery.src = 'https://code.jquery.com/jquery-3.3.1.min.js';
        jquery.integrity = "sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=";
        jquery.crossOrigin = "anonymous";
        jquery.onload = annotatorApiFileLoadHelper.jqueryLoadedHandler;
        head.appendChild(jquery);
    } else {
        annotatorApiFileLoadHelper.jqueryLoadedHandler();
    }
};

annotatorApiFileLoadHelper.annotatorAllFilesLoadedHandler = function () {
    console.log("All files Loading");
    if (annotatorApiFileLoadHelper.highlighterJsLoaded && annotatorApiFileLoadHelper.annotatorTouchCssLoaded) {
        jQuery(function () {
            var annotator = jQuery('body').annotator();
            annotator.annotator("addPlugin",
                    "Touch",
                    { force: true, useHighlighter: true });
            annotator.annotator('addPlugin',
                    'Store',
                    { prefix: AnnotatorApiSettings.apiUrl });
        });
    }
};

annotatorApiFileLoadHelper.loadAnnotatorApp();
