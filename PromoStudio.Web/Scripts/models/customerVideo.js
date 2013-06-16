﻿/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/customerVideoItem", "models/enums", "knockout"], function (customerVideoItem, enums, ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerVideoId = ko.observable(data.pk_CustomerVideoId || null);
        self.fk_CustomerId = ko.observable(data.fk_CustomerId || null);
        self.fk_CustomerVideoRenderStatusId = ko.observable(data.fk_CustomerVideoRenderStatusId || 1); // pending
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.RenderFailureMessage = ko.observable(data.RenderFailureMessage || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
        self.DateCompleted = ko.observable(data.DateCompleted || null);
        self.PreviewFilePath = ko.observable(data.PreviewFilePath || null);
        self.CompletedFilePath = ko.observable(data.CompletedFilePath || null);

        self.Items = ko.observableArray([]);

        self.CustomerVideoRenderStatus = ko.computed(function () {
            var id = self.fk_CustomerVideoRenderStatusId();
            return enums.customerVideoRenderStatus(id);
        });
        self.LinkUrl = ko.computed(function () {
            var previewPath = self.PreviewFilePath(),
                completedPath = self.CompletedFilePath(),
                displayPath = completedPath || previewPath;
            if (displayPath === null) { return "javascript:void(0);"; }
            return displayPath;
        });
        self.LinkFileName = ko.computed(function () {
            var previewPath = self.PreviewFilePath(),
                completedPath = self.CompletedFilePath(),
                displayPath = completedPath || previewPath,
                ix;
            if (displayPath === null) { return "Video Not Available"; }
            ix = displayPath.lastIndexOf("\\");
            if (ix === -1) { ix = displayPath.lastIndexOf("/"); }
            if (ix === -1) { return displayPath; }
            displayPath = displayPath.substring(ix + 1);
            ix = displayPath.indexOf("?");
            if (ix !== -1) { displayPath = displayPath.substring(0, ix); }
            return displayPath;
        });

        self.LoadItems = function (items) {
            var i, item;
            items = items || [];

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new customerVideoItem(item);
            }

            self.Items(items);
        };
        self.LoadItems(data.Items);
    };
});
