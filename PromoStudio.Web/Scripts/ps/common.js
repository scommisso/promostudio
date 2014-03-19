"use strict";

define([], function () {
    function getSourceElement(event) {
        if (!event) { return null; }
        return event.currentTarget || event.srcElement || event.target || null;
    }

    function cancelEvent(event) {
        if (!event) { return; }
        if (event.preventDefault) {
            event.preventDefault();
        } else {
            event.returnValue = false;
        }
    }

    return {
        getSourceElement: getSourceElement,
        cancelEvent: cancelEvent
    };
});
