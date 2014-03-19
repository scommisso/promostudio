/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />

"use strict";

define(["models/templateScriptItem", "knockout"], function (templateScriptItem, ko) {
    function ctor (data) {
        var self = this;
        data = data || {};

        self.pk_TemplateScriptId = ko.observable(data.pk_TemplateScriptId || null);
        self.fk_TemplateScriptStatusId = ko.observable(data.fk_TemplateScriptStatusId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.ProjectFilePath = ko.observable(data.ProjectFilePath || null);
        self.ThumbnailFilePath = ko.observable(data.ThumbnailFilePath || null);
        self.PreviewFilePath = ko.observable(data.PreviewFilePath || null);
        self.RenderCompName = ko.observable(data.RenderCompName || null);
        self.RenderPreviewCompName = ko.observable(data.RenderPreviewCompName || null);
        self.RenderCompStartTime = ko.observable(data.RenderCompStartTime || null);
        self.RenderCompEndTime = ko.observable(data.RenderCompEndTime || null);
        
        self.Items = ko.observableArray([]);

        self.LoadItems = function (items) {
            var i, item;
            items = items || [];

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new templateScriptItem(item);
            }

            self.Items(items);
        };
        self.LoadItems(data.Items);
    }

    return ctor;
});