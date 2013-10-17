/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["models/customerVideo",
        "ps/logger",
        "knockout"], function (
        customerVideo,
        logger,
        ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.CurrentStep = (data.CurrentStep || 0);
        self.StepsCompleted = (data.StepsCompleted || []);
        self.IsStepCompleted = ko.observable(data.IsStepCompleted || false);
        self.Video = ko.observable(new customerVideo(data.Video || {}));

        self.BeforeStepChange = function(navVm, callback) {
            callback();
        };
        self.StepChange = function (navVm, e) {
            self.BeforeStepChange(navVm, function () {
                self.NavigateStep(e);
            });
        };

        self.NavigateStep = function (e) {
            var $tgt = $(e.target);
            e.stopImmediatePropagation();
            self.UpdateBuildCookie(function () {
                if ($tgt.hasClass("notready")
                    || $tgt.parents(".notready").length > 0) {
                    return;
                }
                var url = $tgt.attr("href");
                if (!url) {
                    url = $tgt.parents("a").first().attr("href");
                }
                if (url) {
                    document.location.href = url;
                }
            });
        };
        
        self.UpdateBuildCookie = function (callback) {
            var ix = self.StepsCompleted.indexOf(self.CurrentStep),
                cookieData;
            if (self.IsStepCompleted() && ix === -1) {
                self.StepsCompleted.push(self.CurrentStep);
            }
            else if (!self.IsStepCompleted() && ix !== -1) {
                self.StepsCompleted.splice(ix, 1);
            }

            cookieData = {
                CompletedSteps: self.StepsCompleted,
                Video: self.Video
            };
            $.ajax({
                type: "POST",
                url: "/Build/Checkpoint",
                data: ko.toJSON(cookieData),
                contentType: "application/json; charset=utf-8"
            })
                .done(function() {
                    logger.log("Video checkpoint");
                })
                .error(function() {
                    logger.log("Error setting video checkpoint");
                })
                .always(function () {
                    if (typeof callback === "function") {
                        callback();
                    }
                });
        };
    };
});