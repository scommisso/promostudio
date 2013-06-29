define("modernizr", [], Modernizr);
require.onError = function (e) {
    console.log("RequireJS Error: " + JSON.stringify(e));
}
require.config({
    baseUrl: "/Scripts/",
    paths: {
        jquery: ["//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min", "/Scripts/lib/jquery-1.9.1"],
        pjax: ["//cdnjs.cloudflare.com/ajax/libs/jquery.pjax/1.7.0/jquery.pjax.min", "/Scripts/lib/jquery.pjax"],
        history: ["//cdnjs.cloudflare.com/ajax/libs/history.js/1.8/bundled/html4+html5/native.history", "/Scripts/lib/history"],
        knockout: ["//cdnjs.cloudflare.com/ajax/libs/knockout/2.2.1/knockout-min", "/Scripts/lib/knockout-2.2.1"],
        bootstrap: ["//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.1/js/bootstrap.min", "/Scripts/lib/bootstrap"],
        bootstrapWizard: "/Scripts/lib/jquery.bootstrap.wizard"
    },
    shim: {
        modernizr: {
            exports: "Modernizr"
        },
        bootstrap: {
            deps: ["jquery", "modernizr"],
            exports: "$.fn.typeahead"
        },
        bootstrapWizard: {
            deps: ["jquery", "bootstrap"],
            exports: "$.fn.bootstrapWizard"
        },
        pjax: {
            deps: ["jquery"],
            exports: "$.fn.pjax"
        },
        history: {
            exports: "History"
        }
    },
    enforceDefine: true
});