/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../ps/vidyardPlayer.js" />
/// <reference path="enums.js" />

"use strict";

define([
    "knockout",
    "ps/vidyardPlayer",
    "models/enums",
    "strings",
    "ps/extensions"
], function (
        ko,
        vPlayer,
        enums,
        strings) {
    function ctor(customerVideo) {
        var self = this;
        customerVideo = customerVideo || {};

        self.pk_CustomerVideoId = customerVideo.pk_CustomerVideoId || ko.observable();
        self.fk_CustomerId = customerVideo.fk_CustomerId || ko.observable();
        self.fk_CustomerVideoRenderStatusId = customerVideo.fk_CustomerVideoRenderStatusId || ko.observable(); // pending
        self.fk_StoryboardId = customerVideo.fk_StoryboardId || ko.observable();
        self.Name = customerVideo.Name || ko.observable();
        self.Description = customerVideo.Description || ko.observable();
        self.RenderFailureMessage = customerVideo.RenderFailureMessage || ko.observable();
        self.DateCreated = customerVideo.DateCreated || ko.observable();
        self.DateUpdated = customerVideo.DateUpdated || ko.observable();
        self.DateCompleted = customerVideo.DateCompleted || ko.observable();
        self.VidyardVideoId = customerVideo.VidyardVideoId || ko.observable();
        self.VidyardPlayerId = customerVideo.VidyardPlayerId || ko.observable();
        self.VidyardPlayerUuid = customerVideo.VidyardPlayerUuid || ko.observable();

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

        self.ThumbnailUrl = ko.computed(function () {
            var vId = self.VidyardPlayerUuid(),
                incomplete = self.IsIncomplete();
            if (incomplete || !vId) { return null; }
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
    }
    
    return ctor;
});
