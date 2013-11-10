/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="audioScriptTemplate.js" />
/// <reference path="storyboardItem.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../ps/vidyardPlayer.js" />

"use strict";

define([
    "models/storyboardItem",
    "models/audioScriptTemplate",
    "ps/vidyardPlayer",
    "knockout",
    "ps/extensions"
    ], function (
        storyboardItem,
        audioScriptTemplate,
        vPlayer,
        ko) {
    function ctor (data) {
        var self = this;
        data = data || {};

        self.pk_StoryboardId = ko.observable(data.pk_StoryboardId || null);
        self.fk_StoryboardStatusId = ko.observable(data.fk_StoryboardStatusId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.fk_VerticalId = ko.observable(data.fk_VerticalId || null);
        self.fk_AudioScriptTemplateId = ko.observable(data.fk_AudioScriptTemplateId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.VidyardId = ko.observable(data.VidyardId || null);
        self.ThumbnailUrl = ko.computed(function () {
            var vId = self.VidyardId();
            if (vId === null) {
                return null;
            }
            return "//embed.vidyard.com/embed/{0}/thumbnail.jpg".format(vId);
        });
        self.ThumbnailBackground = ko.computed(function () {
            var url = self.ThumbnailUrl();
            if (url) {
                return "url('{0}')".format(url);
            }
            return "none";
        });

        self.Items = ko.observableArray([]);

        self.AudioScriptTemplate = ko.observable(null);

        self.Player = null;

        self.LoadPlayer = function () {
            self.Player = new vPlayer({ VideoId: self.VidyardId() });
        };
        self.PlayLightbox = function (d, e) {
            if (!self.Player && self.VidyardId()) {
                self.LoadPlayer();
            }
            if (self.Player) {
                self.Player.ShowLightbox();
            }
            e.stopImmediatePropagation();
        };

        self.LoadItems = function (audioScript, items) {
            var i, item;
            items = items || [];

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new storyboardItem(self, item);
            }

            self.Items(items);
            
            if (audioScript) {
                self.AudioScriptTemplate(new audioScriptTemplate(audioScript));
            }
        };
        self.LoadItems(data.AudioScriptTemplate, data.Items);
    }

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.ThumbnailUrl;
        delete copy.ThumbnailBackground;
        delete copy.AudioScriptTemplate;
        delete copy.Player;

        return copy;
    };
    return ctor;
});
