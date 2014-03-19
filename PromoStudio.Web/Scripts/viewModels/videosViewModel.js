/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../models/customer.js" />
/// <reference path="../models/customerVideo.js" />
/// <reference path="../viewModels/customerVideoViewModel.js" />

"use strict";

define([
    "models/customer",
    "models/customerVideo",
    "viewModels/customerVideoViewModel",
    "ps/logger",
    "jqueryui",
    "knockout"
], function (
    customer,
    customerVideo,
    customerVideoViewModel,
    logger,
    $,
    ko
    ) {
    function ctor(data) {
        var self = this;

        self.Customer = ko.observable(null);
        self.CustomerVideos = ko.observableArray([]);
        self.LoadingData = ko.observable(false);

        function isVideoPlaying() {
            // TODO: Implement lightbox and check if it is open
            return false;
        }

        self.SelectedVideo = ko.observable(null);
        self.IsSelected = function (video) {
            return self.SelectedVideo() === video;
        };
        self.SelectVideo = function (video) {
            if (self.IsSelected(video)) {
                self.SelectedVideo(null);
            } else {
                self.SelectedVideo(video);
            }
        };

        function loadItems(customerData, videos) {
            var newVideos = [],
                i, item, match;
            videos = videos || [];

            for (i = 0; i < videos.length; i++) {
                item = videos[i];
                match = findMatchingVideo(item);
                if (match === null) {
                    newVideos.push(
                        new customerVideoViewModel(
                        new customerVideo(item)));
                } else {
                    updateField(match, item, "fk_CustomerVideoRenderStatusId");
                    updateField(match, item, "fk_StoryboardId");
                    updateField(match, item, "Name");
                    updateField(match, item, "Description");
                    updateField(match, item, "RenderFailureMessage");
                    updateField(match, item, "DateUpdated");
                    updateField(match, item, "DateCompleted");
                    updateField(match, item, "VimeoVideoId");
                    updateField(match, item, "VimeoThumbnailUrl");
                    updateField(match, item, "VimeoStreamingUrl");
                }
            }
            for (i = 0; i < newVideos.length; i++) {
                self.CustomerVideos.push(newVideos[i]);
            }

            if (customerData !== null) {
                self.Customer(new customer(customerData));
            }
        };

        function updateField(video, newVideo, fieldName) {
            if (video[fieldName]() !== newVideo[fieldName]) {
                video[fieldName](newVideo[fieldName]);
            }
        }

        function findMatchingVideo(videoData) {
            var videos = self.CustomerVideos(),
                match = null;
            match = $.grep(videos, function (video) {
                return video.pk_CustomerVideoId() === videoData.pk_CustomerVideoId;
            });
            if (match && match.length) {
                return match[0];
            }
            return null;
        }

        function updateStatus() {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Videos/Status",
                cache: false
            }).done(function (vidData) {
                loadItems(null, vidData);
            }).error(function (jqXhr, textStatus, errorThrown) {
                logger.log("Error retrieving video status: " + textStatus);
                logger.log(errorThrown);
            });

        }

        self.pageLoaded = function () {
            self.LoadingData(true);
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Videos/Data",
                cache: false
            }).done(function (vidData) {
                loadItems(vidData.Customer, vidData.CustomerVideos);
                self.LoadingData(false);
                var interval = window.setInterval(function () {
                    if (!isVideoPlaying()) {
                        updateStatus();
                    }
                }, 5000);
                if ($.pjax) {
                    $.fn.pjaxScaffold.clearIntervals = function () {
                        window.clearInterval(interval);
                    };
                }
            }).error(function (jqXhr, textStatus, errorThrown) {
                logger.log("Error retrieving data: " + textStatus);
                logger.log(errorThrown);
            });
        };
    }

    return ctor;
});