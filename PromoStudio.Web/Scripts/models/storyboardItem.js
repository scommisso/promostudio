/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="templateScript.js" />
/// <reference path="stockVideo.js" />

"use strict";

define([
    "models/templateScript",
    "models/stockVideo",
    "knockout"
], function (
    templateScript,
    stockVideo,
    ko) {
    function ctor (storyboard, data) {
        var self = this;
        data = data || {};

        self.pk_StoryboardItemId = ko.observable(data.pk_StoryboardItemId || null);
        self.fk_StoryboardId = ko.observable(data.fk_StoryboardId || null);
        self.fk_StoryboardItemTypeId = ko.observable(data.fk_StoryboardItemTypeId || null);
        self.fk_TemplateScriptId = ko.observable(data.fk_TemplateScriptId || null);
        self.fk_StockVideoId = ko.observable(data.fk_StockVideoId || null);
        self.Name = ko.observable(data.Name || null);
        self.LengthInSeconds = ko.observable(data.LengthInSeconds || 0);
        self.SortOrder = ko.observable(data.SortOrder || null);

        self.Storyboard = ko.observable(storyboard || null);
        self.TemplateScript = ko.observable(null);
        self.StockVideo = ko.observable(null);

        self.LoadScript = function (templateScriptData, stockVideoData) {
            if (templateScriptData) {
                self.TemplateScript(new templateScript(templateScriptData));
            }
            if (stockVideoData) {
                self.StockVideo(new stockVideo(stockVideoData));
            }
        };
        self.LoadScript(data.TemplateScript, data.StockVideo);
    }

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.Storyboard;
        delete copy.TemplateScript;
        delete copy.StockVideo;

        return copy;
    };
    return ctor;
});
