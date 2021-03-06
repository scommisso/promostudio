﻿/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/storyboard.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />

"use strict";

define(["models/storyboard",
        "jquery",
        "knockout",
        "ps/logger",
        "lib/ko.custom"],
    function (
        storyboard,
        $,
        ko,
        logger) {
        function ctor(data) {

            function stepChanging(navVm, callback) {
                var customerVideo = video(),
                    selectedStoryboard = self.SelectedStoryboardId();
                if (customerVideo.fk_StoryboardId() !== selectedStoryboard) {
                    // clear previous selection
                    customerVideo.fk_StoryboardId(self.SelectedStoryboardId());
                    customerVideo.Items([]);
                }
                callback();
            }

            function loadVideoData(videoData) {
                var storyboardId = videoData.fk_StoryboardId(),
                    storyboards = self.Storyboards(),
                    i, item;
                if (storyboardId) {
                    for (i = 0; i < storyboards.length; i++) {
                        item = storyboards[i];
                        if (item.pk_StoryboardId() === storyboardId) {
                            self.SelectedStoryboard(item);
                            break;
                        }
                    }
                }
            }

            function loadData(storyboards) {
                var i, item;
                storyboards = storyboards || [];

                for (i = 0; i < storyboards.length; i++) {
                    item = storyboards[i];
                    storyboards[i] = new storyboard(item);
                    storyboards[i].LoadPlayer();
                }

                self.Storyboards(storyboards);
            }

            var self = this;
            data = data || {};

            var isStepCompleted = null,
                video = null;

            self.Storyboards = ko.observableArray([]);
            self.SelectedStoryboard = ko.observable(null);
            self.SelectedStoryboardId = ko.computed(function () {
                var sb = self.SelectedStoryboard();
                if (!sb) {
                    if (isStepCompleted) {
                        isStepCompleted(false);
                    }
                    return null;
                }
                if (isStepCompleted) {
                    isStepCompleted(true);
                }
                return sb.pk_StoryboardId();
            });

            self.IsSelected = function (sb) {
                return self.SelectedStoryboard() === sb;
            };

            self.SelectStoryboard = function (sb) {
                if (self.IsSelected(sb)) {
                    self.SelectedStoryboard(null);
                } else {
                    self.SelectedStoryboard(sb);
                }
            };

            self.Bind = function (selector, navSelector) {
                ko.applyBindings(self, $(selector)[0]);
                ko.callbackOnBind($(navSelector)[0], function (navVm) {
                    isStepCompleted = navVm.IsStepCompleted;
                    video = navVm.Video;
                    navVm.BeforeStepChange = stepChanging;

                    loadVideoData(video());
                }, 1000);
            };

            loadData(data.Storyboards);
        }

        return ctor;
    });