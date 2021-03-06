﻿/// <reference path="../vsdoc/require.js" />

define(["jquery", "bootstrap"], function ($) {
    var init = function () {
        require(["lib/bootstrap-lightbox", "css!/Content/css/bootstrap-lightbox"], function () {
            return $.fn.lightbox;
        });
    };

    init();

    return null;
});
