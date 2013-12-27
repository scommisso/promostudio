"use strict";

require.onError = function(e) {
    if (console && console.log) {
        console.log("RequireJS: " + e.requireType + "-" + e.message);
        console.log(e.stack);
    }
};
require.config({
    baseUrl: "/optimizedScripts/",
    paths: {
        jquery: ["//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min", "/optimizedScripts/lib/jquery-1.9.1"],
        jqueryui: "/Scripts/lib/jquery.main",
        //pjax: ["//cdnjs.cloudflare.com/ajax/libs/jquery.pjax/1.7.0/jquery.pjax.min", "/optimizedScripts/lib/jquery.pjax"],
        //history: ["//cdnjs.cloudflare.com/ajax/libs/history.js/1.8/bundled/html4+html5/native.history", "/optimizedScripts/lib/history"],
        knockout: ["//cdnjs.cloudflare.com/ajax/libs/knockout/2.3.0/knockout-min", "/optimizedScripts/lib/knockout-2.3.0"],
        bootstrap: ["//netdna.bootstrapcdn.com/bootstrap/3.0.0/js/bootstrap.min", "/optimizedScripts/lib/bootstrap"]
    },
    //shim: {
        //pjax: {
        //    deps: ["jquery", "History"],
        //    exports: "$.fn.pjax"
        //},
        //history: {
        //    exports: "History"
        //}
    //},
    map: {
        "*": {
            "css": "lib/css.min"
        }
    },
    enforceDefine: true
});