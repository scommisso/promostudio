/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["knockout"], function (ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_StockAudioId = ko.observable(data.pk_StockAudioId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.FilePath = ko.observable(data.FilePath || null);
    };
});
