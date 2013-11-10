"use strict";

define([], function () {
    function ctor() {
        var self = this;

        self.log = function (message) {
            if (window.console && window.console.log) {
                window.console.log(message);
            }
        };

        return self;
    }

    var logger = new ctor();

    return logger;
});