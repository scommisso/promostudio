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
            var isAccepted = self.IsAccepted(),
                videoName = self.VideoName(),
                hasName = (videoName && videoName.length > 0),
                stepCompleted = true;
            if (!isAccepted) {
                stepCompleted = false;
            }
            if (!hasName) {
                stepCompleted = false;
            }
            if (ko.isObservable(isStepCompleted)) {
                isStepCompleted(stepCompleted);
            }
            return stepCompleted;
        });

        self.GenerateVideo = function () {
            var vid = video();
            vid.Name(self.VideoName);
            $.ajax({
                type: "POST",
                url: "/Build/Submit",
                data: ko.toJSON(vid),
                contentType: "application/json; charset=utf-8"
            })
                .done(function (data, textStatus, jqXHR) {
                    if (data.Success !== true) {
                        alert("ERROR");
                        return;
                    }
                    alert("Successfully generated.");
                    data = data.Model;
                    logger.log("Saved Video");
                    logger.log(data);
                    video(new customerVideo(data));

                    // TODO: "Your video is being generated" screen
                })
                .error(function (jqXHR, textStatus, errorThrown) {
                    logger.log("ERROR generataing video");
                    logger.log(errorThrown);
                    alert("ERROR");
                });
        };
        
        function stepChanging(navVm, callback) {
            video().Name(self.VideoName);
            callback();
        }

        function loadVideoData(videoData) {
            self.VideoName(videoData.Name());
        }
        
        function loadData() {
            // TODO: Load any additional summary data
        }

        loadData();
    };
});