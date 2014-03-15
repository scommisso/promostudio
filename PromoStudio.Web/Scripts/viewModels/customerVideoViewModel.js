/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="enums.js" />

"use strict";

define([
    "knockout",
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
        self.VimeoVideoId = customerVideo.VimeoVideoId || ko.observable();
        self.VimeoThumbnailUrl = customerVideo.VimeoThumbnailUrl || ko.observable();
        self.VimeoStreamingUrl = customerVideo.VimeoStreamingUrl || ko.observable();

        self.CustomerVideoRenderStatus = ko.computed(function () {
            var id = self.fk_CustomerVideoRenderStatusId();
            return enums.customerVideoRenderStatus(id);
        });
        self.IsHosted = ko.computed(function () {
            var vId = self.VimeoVideoId();
            return (!!vId);
        });
        self.IsIncomplete = ko.computed(function () {
            var id = self.fk_CustomerVideoRenderStatusId();
            return id !== 15;
        });

        self.ThumbnailUrl = ko.computed(function () {
            var vUrl = self.VimeoThumbnailUrl(),
                incomplete = self.IsIncomplete();
            if (incomplete || !vUrl) { return null; }
            return vUrl;
        });

        self.ThumbnailBackground = ko.computed(function () {
            var url = self.ThumbnailUrl();
            if (url) {
                return "url('{0}')".format(url);
            }
            return "none";
        });

        self.PlayerUrl = ko.computed(function () {
            var vId = self.VimeoVideoId(),
                url;
            if (!vId) { return null; }
            url = strings.getResource("Vimeo__PlayerUrl").format(vId);
            return url;
        });

        self.LightboxEmbedCode = ko.computed(function () {
            var vId = self.VimeoVideoId();
            if (!vId) { return null; }
            return strings.getResource("Vimeo__InlineEmbed").format(vId, 360, 640);
        });

        self.InlineEmbedCode = ko.computed(function () {
            var vId = self.VimeoVideoId();
            if (!vId) { return null; }
            return strings.getResource("Vimeo__InlineEmbed").format(vId, 486, 864);
        });

        self.Player = null;
    }
    
    return ctor;
});
