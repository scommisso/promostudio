/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

"use strict";

define(["knockout"], function (ko) {
    function ctor (data) {
        var self = this;
        data = data || {};

        self.pk_StockVideoId = ko.observable(data.pk_StockVideoId || null);
        self.fk_StockItemStatusId = ko.observable(data.fk_StockItemStatusId || null);
        self.fk_StoryboardItemTypeId = ko.observable(data.fk_StoryboardItemTypeId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.fk_VerticalId = ko.observable(data.fk_VerticalId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.FilePath = ko.observable(data.FilePath || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
    }

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.FilePath;
        delete copy.DateCreated;
        delete copy.DateUpdated;

        return copy;
    };
    return ctor;
});