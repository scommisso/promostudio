﻿/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="customerVideoVoiceOver.js" />
/// <reference path="customerVideoItem.js" />
/// <reference path="storyboard.js" />

"use strict";

define([
    "models/customerVideoItem",
    "models/storyboard",
    "models/customerVideoVoiceOver",
    "knockout"
], function (
        customerVideoItem,
        storyboard,
        customerVideoVoiceOver,
        ko) {
    function ctor (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerVideoId = ko.observable(data.pk_CustomerVideoId || null);
        self.fk_CustomerId = ko.observable(data.fk_CustomerId || null);
        self.fk_CustomerVideoRenderStatusId = ko.observable(data.fk_CustomerVideoRenderStatusId || 1); // pending
        self.fk_StoryboardId = ko.observable(data.fk_StoryboardId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.RenderFailureMessage = ko.observable(data.RenderFailureMessage || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
        self.DateCompleted = ko.observable(data.DateCompleted || null);
        self.PreviewFilePath = ko.observable(data.PreviewFilePath || null);
        self.CompletedFilePath = ko.observable(data.CompletedFilePath || null);
        self.VidyardVideoId = ko.observable(data.VidyardVideoId || null);
        self.VidyardPlayerId = ko.observable(data.VidyardPlayerId || null);
        self.VidyardPlayerUuid = ko.observable(data.VidyardPlayerUuid || null);

        self.Storyboard = ko.observable(null);
        self.VoiceOver = ko.observable(null);
        self.Items = ko.observableArray([]);

        self.LoadItems = function (storyboardData, voiceOverData, items) {
            var i, item;
            items = items || [];

            storyboardData = storyboardData || {};
            storyboardData = new storyboard(storyboardData);

            if (voiceOverData) {
                self.VoiceOver(new customerVideoVoiceOver(voiceOverData));
            }

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new customerVideoItem(item);
            }

            self.Storyboard(storyboardData);
            self.Items(items);
        };
        self.LoadItems(data.Storyboard, data.VoiceOver, data.Items);
    }

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.DateCreated;
        delete copy.DateUpdated;
        delete copy.DateCompleted;
        delete copy.RenderFailureMessage;
        delete copy.PreviewFilePath;
        delete copy.CompletedFilePath;
        delete copy.Storyboard;
        delete copy.CustomerVideoRenderStatus;

        return copy;
    };
    return ctor;
});
