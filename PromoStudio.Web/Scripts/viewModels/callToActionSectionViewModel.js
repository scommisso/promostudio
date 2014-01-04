﻿/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />

"use strict";

define(["jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger"
],
    function (
        $,
        ko,
        strings,
        enums,
        logger) {
        function ctor(data, video) {

            var self = this;
            data = data || {};
            video = video || {};

            function loadData(videoData) {
                loadVideoData(videoData);
            }

            function loadVideoData(videoData) {
                video = videoData; // TODO: Load CTA-relevant items
            }

            self.StepNumber = ko.observable(1);
            
            self.IsVisible = ko.observable(false);
            self.IsCompleted = ko.observable(false);
            self.StartOpen = ko.observable(false);

            loadData(video);
        }

        return ctor;
    });