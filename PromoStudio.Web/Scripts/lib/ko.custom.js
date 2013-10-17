define(["knockout"], function (ko) {
    ko.bindingHandlers.fadeVisible = {
        init: function (element, valueAccessor) {
            // Initially set the element to be instantly visible/hidden depending on the value
            var value = valueAccessor();
            $(element).toggle(ko.utils.unwrapObservable(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
        },
        update: function (element, valueAccessor) {
            // Whenever the value subsequently changes, slowly fade the element in or out
            var value = valueAccessor();
            ko.utils.unwrapObservable(value)
                ? $(element).fadeIn()
                : $(element).fadeOut();
        }
    };
    ko.callbackOnBind = function(element, callback, timeout) {
        var vm = null,
            interval = null,
            start = new Date().getTime(),
            now;
        interval = setInterval(function() {
            try {
                vm = ko.dataFor(element);
            } catch(e) {
                // not bound
            }
            if (vm !== undefined && vm !== null) {
                clearInterval(interval);
                callback(vm);
                return;
            }
            now = new Date().getTime();
            if ((now - start) > timeout) {
                clearInterval(interval);
            }
        }, 10);
    };
    return ko;
});
