/// <reference path="require.js" />
/// 
define('modernizr', [], Modernizr);
require.config({
    baseUrl: "/Scripts/",
    paths: {
        jquery: ["//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min", "/Scripts/lib/jquery-1.9.1"],
        pjax: ["//cdnjs.cloudflare.com/ajax/libs/jquery.pjax/1.7.0/jquery.pjax.min", "/Scripts/lib/jquery.pjax"],
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
            exports: '$.fn.typeahead'
        },
        bootstrapWizard: {
            deps: ["jquery", "bootstrap"],
            exports: '$.fn.bootstrapWizard'
        },
        pjax: {
            deps: ["jquery"],
            exports: '$.fn.pjax'
        }
    },
    enforceDefine: true
});
require.onError = function (err) {
    console.log("RequireJS Error: " + JSON.stringify(err));
}