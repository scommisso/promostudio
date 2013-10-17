define([], function () {
    var logger = new (function() {
        var self = this;

        self.log = function (message) {
            if (window.console && window.console.log) {
                window.console.log(message);
            }
        };
    })();
    return logger;
});