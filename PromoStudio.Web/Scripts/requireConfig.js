/// <reference path="require.js" />

require.config({
    baseUrl: "/Scripts/",
    paths: {
        jquery: ["//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min", "/Scripts/lib/jquery-1.9.1"],
        knockout: ["//cdnjs.cloudflare.com/ajax/libs/knockout/2.2.1/knockout-min", "/Scripts/lib/knockout-2.2.1"],
        bootstrap: ["//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.1/js/bootstrap.min", "/Scripts/lib/bootstrap"],
        bootstrapWizard: "/Scripts/lib/jquery.bootstrap.wizard"
    },
    shim: {
        bootstrap: {
            deps: ["jquery"],
            exports: "$.fn.typeahead"
        },
        bootstrapWizard: {
            deps: ["jquery", "bootstrap"],
            exports: "$.fn.bootstrapWizard"
        }
    },
    enforceDefine: true
});