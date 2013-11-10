"use strict";

require.onError = function(e) {
    if (console && console.log) {
        console.log("RequireJS: " + e.requireType + "-" + e.message);
    }
};
require.config({
    baseUrl: "/Scripts/",
    paths: {
        jquery: ["//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min", "/Scripts/lib/jquery-1.9.1"],
        jqueryui: "/Scripts/lib/jquery-ui-effects-1.10.3",
        //pjax: ["//cdnjs.cloudflare.com/ajax/libs/jquery.pjax/1.7.0/jquery.pjax.min", "/Scripts/lib/jquery.pjax"],
        //history: ["//cdnjs.cloudflare.com/ajax/libs/history.js/1.8/bundled/html4+html5/native.history", "/Scripts/lib/history"],
        knockout: ["//cdnjs.cloudflare.com/ajax/libs/knockout/2.3.0/knockout-min", "/Scripts/lib/knockout-2.3.0"],
        bootstrap: ["//netdna.bootstrapcdn.com/bootstrap/3.0.0/js/bootstrap.min", "/Scripts/lib/bootstrap"]
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