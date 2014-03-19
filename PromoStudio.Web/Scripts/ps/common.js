"use strict";

define([], function () {
    function getSourceElement(event) {
        if (!event) { return null; }
        return event.currentTarget || event.srcElement || event.target || null;
    }

    return {
        getSourceElement: getSourceElement
    };
});
