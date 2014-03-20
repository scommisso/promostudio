/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.11.0.intellisense.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="musicSectionViewModel.js" />
/// <reference path="actorSectionViewModel.js" />

"use strict";

define([
        "models/customerVideoItem",
        "models/customerVideoVoiceOver",
        "viewModels/musicSectionViewModel",
        "viewModels/actorSectionViewModel",
        "jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "lib/ko.custom"
], function (
        customerVideoItem,
        customerVideoVoiceOver,
        musicSectionViewModel,
        actorSectionViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
    function ctor(data) {

        function stepChanging(navVm, callback) {
            callback();
        }

        function loadVideoData(videoData) {
            var items = videoData.Items(),
                item = null,
                musicItems, voiceData;

            // Check for existing music item, and add it if missing
            musicItems = $.grep(items, function (musicItem) {
                return musicItem.fk_CustomerVideoItemTypeId() === 2;
            });
            if (musicItems && musicItems.length > 0) {
                item = musicItems[0];
            }
            if (item === null) {
                item = new customerVideoItem({
                    fk_CustomerVideoItemTypeId: 2
                });
                videoData.Items.push(item);
            }

            // Check for existing voiceover object and add it if missing
            voiceData = videoData.VoiceOver();
            if (!voiceData) {
                voiceData = new customerVideoVoiceOver();
                videoData.VoiceOver(voiceData);
            }

            self.MusicSection(new musicSectionViewModel(data, videoData));
            self.ActorSection(new actorSectionViewModel(data, videoData));

            if (!self.MusicSection().IsCompleted()) {
                self.MusicSection().StartOpen(true);
            }
            else if (!self.ActorSection().IsCompleted()) {
                self.ActorSection().StartOpen(true);
            }
        }

        var self = this;
        data = data || {};

        var isStepCompleted = null,
            video = null;

        self.MusicSection = ko.observable({});
        self.ActorSection = ko.observable({});

        self.Bind = function (selector, navSelector) {
            ko.callbackOnBind($(navSelector)[0], function (navVm) {
                isStepCompleted = navVm.IsStepCompleted;
                video = navVm.Video;
                navVm.BeforeStepChange = stepChanging;

                loadVideoData(video());

                self.IsCompleted(); // check completed status

                ko.applyBindings(self, $(selector)[0]);
            }, 1000);
        };
        self.IsCompleted = ko.computed(function () {
            var ms = ko.toJS(self.MusicSection()),
                as = ko.toJS(self.ActorSection()),
                stepCompleted = true;
            if (!ms || !ms.IsCompleted) {
                stepCompleted = false;
            }
            if (!as || !as.IsCompleted) {
                stepCompleted = false;
            }
            if (ko.isObservable(isStepCompleted)) {
                isStepCompleted(stepCompleted);
            }
            return stepCompleted;
        });
    }

    return ctor;
});