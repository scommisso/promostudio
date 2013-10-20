/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />

define(["jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "lib/ko.custom"],
    function (
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
        
        self.Bind = function (selector, navSelector) {
            ko.applyBindings(self, $(selector)[0]);
            ko.callbackOnBind($(navSelector)[0], function (navVm) {
                isStepCompleted = navVm.IsStepCompleted;
                video = navVm.Video;
                navVm.BeforeStepChange = stepChanging;

                loadVideoData(video());

                self.IsCompleted(); // check completed status

                isStepCompleted(true);// TODO: Remove this when everything is hooked up
            }, 1000);
        };
        self.IsCompleted = ko.computed(function () {
            // TODO: Mark completed when all audio placeholders are filled in
            if (ko.isObservable(isStepCompleted)) {
                isStepCompleted(true);
            }
            return true;
        });
        
        function stepChanging(navVm, callback) {
            // TODO: Add any necessary items into the video
            callback();
        }

        function loadVideoData(videoData) {
            // TODO: load any script items from the video data
        }
        
        function loadData() {
            // TODO: Load audio script template from data object
        }

        loadData();
    };
});