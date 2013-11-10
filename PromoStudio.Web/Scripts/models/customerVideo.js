/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="customerVideoItem.js" />
/// <reference path="enums.js" />

"use strict";

define([
    "models/customerVideoItem",
    "models/storyboard",
    "models/customerVideoVoiceOver",
    "knockout",
    "ps/vidyardPlayer",
    "models/enums",
    "strings",
    "ps/extensions"
], function (
        customerVideoItem,
        storyboard,
        customerVideoVoiceOver,
        ko,
        vPlayer,
        enums,
        strings) {
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

        self.CustomerVideoRenderStatus = ko.computed(function () {
            var id = self.fk_CustomerVideoRenderStatusId();
            return enums.customerVideoRenderStatus(id);
        });
        self.IsHosted = ko.computed(function () {
            var vId = self.VidyardPlayerUuid();
            return (!!vId);
        });
        self.IsIncomplete = ko.computed(function () {
            var id = self.fk_CustomerVideoRenderStatusId();
            return id !== 15;
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
            if (!displayPath) { return null; }
            ix = displayPath.lastIndexOf("\\");
            if (ix === -1) { ix = displayPath.lastIndexOf("/"); }
            if (ix === -1) { return displayPath; }
            displayPath = displayPath.substring(ix + 1);
            ix = displayPath.indexOf("?");
            if (ix !== -1) { displayPath = displayPath.substring(0, ix); }
            return displayPath;
        });
        
        self.ThumbnailUrl = ko.computed(function () {
            var vId = self.VidyardPlayerUuid();
            if (!vId) { return null; }
            return strings.getResource("Vidyard__ThumbnailUrl").format(vId);
        });

        self.ThumbnailBackground = ko.computed(function () {
            var url = self.ThumbnailUrl();
            if (url) {
                return "url('{0}')".format(url);
            }
            return "none";
        });

        self.PlayerUrl = ko.computed(function () {
            var vId = self.VidyardPlayerUuid(),
                statusId = self.fk_CustomerVideoRenderStatusId(),
                url;
            if (!vId) { return null; }
            url = strings.getResource("Vidyard__PlayerUrl").format(vId);
            return url;
        });

        self.InlineEmbedCode = ko.computed(function () {
            var vId = self.VidyardPlayerUuid();
            if (!vId) { return null; }
            return strings.getResource("Vidyard__InlineEmbed").format(vId);
        });

        self.LightboxEmbedCode = ko.computed(function () {
            var vId = self.VidyardPlayerUuid();
            if (!vId) { return null; }
            return strings.getResource("Vidyard__LightboxEmbed").format(
                vId, vId.replace(/-/g, "$"), self.Name());
        });

        self.Player = null;

        self.LoadPlayer = function () {
            self.Player = new vPlayer({ VideoId: self.VidyardPlayerUuid() });
        };
        self.PlayLightbox = function (d, e) {
            if (!self.Player && self.VidyardPlayerUuid()) {
                self.LoadPlayer();
            }
            if (self.Player) {
                self.Player.ShowLightbox();
            }
            e.stopImmediatePropagation();
        };

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
        delete copy.IsHosted;
        delete copy.IsIncomplete;
        delete copy.LinkUrl;
        delete copy.LinkFileName;
        delete copy.ThumbnailUrl;
        delete copy.ThumbnailBackground;
        delete copy.PlayerUrl;
        delete copy.InlineEmbedCode;
        delete copy.LightboxEmbedCode;
        delete copy.Player;

        return copy;
    };
    return ctor;
});
