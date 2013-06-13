/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["knockout"], function (ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_StockVideoId = ko.observable(data.pk_StockVideoId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.FilePath = ko.observable(data.FilePath || null);
    };
});
