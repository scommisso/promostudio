/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="audioScriptTemplate.js" />
/// <reference path="storyboardItem.js" />
/// <reference path="../ps/extensions.js" />

"use strict";

define([
    "models/storyboardItem",
    "models/audioScriptTemplate",
    "knockout",
    "ps/extensions"
    ], function (
        storyboardItem,
        audioScriptTemplate,
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
        self.VimeoVideoId = ko.observable(data.VimeoVideoId || null);
        self.VimeoThumbnailUrl = ko.observable(data.VimeoThumbnailUrl || null);

        self.Items = ko.observableArray([]);

        self.AudioScriptTemplate = ko.observable(null);

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
        delete copy.AudioScriptTemplate;

        return copy;
    };
    return ctor;
});
