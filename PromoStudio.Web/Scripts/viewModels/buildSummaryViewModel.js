/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../models/customerVideo.js" />

define(["models/customerVideo",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "lib/ko.custom"],
    function (
        customerVideo,
        $,
        ko,
        strings,
        enums,
        logger) {
    return function (data) {
        var self = this;
        data = data || {};

        var isStepCompleted = null,
            video = null;

        self.IsAccepted = ko.observable(true); // HACK: Default this to false when UI is hooked up
        self.VideoName = ko.observable(null);
        self.VideoDescription = ko.observable(null);
        self.IsGenerated = ko.observable(false);
        
        self.Bind = function (selector, navSelector) {
            ko.applyBindings(self, $(selector)[0]);
            ko.callbackOnBind($(navSelector)[0], function (navVm) {
                isStepCompleted = navVm.IsStepCompleted;
                video = navVm.Video;
                navVm.BeforeStepChange = stepChanging;

                loadVideoData(video());

                self.IsCompleted(); // check completed status
            }, 1000);
        };
        
        self.IsCompleted = ko.computed(function () {
            var isGenerated = self.IsGenerated(),
                stepCompleted = true;
            if (!isGenerated) {
                stepCompleted = false;
            }
            if (ko.isObservable(isStepCompleted)) {
                isStepCompleted(stepCompleted);
            }
            return stepCompleted;
        });

        self.CanGenerate = ko.computed(function() {
            var isGenerated = self.IsGenerated(),
                isAccepted = self.IsAccepted(),
                videoName = self.VideoName(),
                hasName = (videoName && videoName.length > 0);
            return (!isGenerated && isAccepted && hasName);
        });

        self.GenerateVideo = function () {
            var vid = video();
            vid.Name(self.VideoName());
            vid.Description(self.VideoDescription());
            self.IsGenerated(false);
            $.ajax({
                type: "POST",
                url: "/Build/Submit",
                data: ko.toJSON(vid),
                contentType: "application/json; charset=utf-8"
            })
                .done(function (data) {
                    logger.log("Saved Video");
                    logger.log(data);
                    vid.pk_CustomerVideoId(data.Model.pk_CustomerVideoId);
                    self.IsGenerated(true);

                    // TODO: Show "Your video is being generated" progress bar
                })
                .error(function (jqXhr, textStatus, errorThrown) {
                    logger.log("ERROR generataing video");
                    logger.log(errorThrown);
                    alert("ERROR");
                });
        };

        self.GoToVideos = function () {
            $.ajax({
                type: "POST",
                url: "/Build/StartOver"
            })
                .done(function () {
                    logger.log("Video cleared");
                })
                .error(function () {
                    logger.log("Error clearing video data");
                })
                .always(function () {
                    document.location.href = "/Videos";
                });
        };
        
        function stepChanging(navVm, callback) {
            video().Name(self.VideoName);
            video().Description(self.VideoDescription);
            callback();
        }

        function loadVideoData(videoData) {
            self.VideoName(videoData.Name());
            self.VideoDescription(videoData.Description());
        }
        
        function loadData() {
            // TODO: Load any additional summary data
        }

        loadData();
    };
});