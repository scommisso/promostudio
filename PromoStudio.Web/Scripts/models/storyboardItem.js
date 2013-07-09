/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["knockout"], function (ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_StoryboardItemId = ko.observable(data.pk_StoryboardItemId || null);
        self.fk_StoryboardId = ko.observable(data.fk_StoryboardId || null);
        self.fk_StoryboardItemTypeId = ko.observable(data.fk_StoryboardItemTypeId || null);
        self.Name = ko.observable(data.Name || null);
        self.SortOrder = ko.observable(data.SortOrder || null);
    };
});
