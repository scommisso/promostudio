/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />

define(["viewModels/photosViewModel",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "lib/ko.custom",
        "ps/extensions"],
    function (
        photosViewModel,
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

        self.PhotoSection = ko.observable({});
        self.Bind = function (selector, navSelector) {
            ko.applyBindings(self, $(selector)[0]);
            ko.callbackOnBind($(navSelector)[0], function (navVm) {
                isStepCompleted = navVm.IsStepCompleted;
                video = navVm.Video;
                navVm.BeforeStepChange = stepChanging;

                loadVideoData(video());
            }, 1000);
        };
        self.IsCompleted = ko.computed(function() {
            var ps = ko.toJS(self.PhotoSection()),
                stepCompleted = true;
            if (!ps || !ps.IsCompleted) {
                stepCompleted = false;
            }
            if (ko.isObservable(isStepCompleted)) {
                isStepCompleted(stepCompleted);
            }
            return stepCompleted;
        });
        
        function stepChanging(navVm, callback) {
            // TODO: Set appropriate video items prior to update
            callback();
        }
        
        function loadVideoData(videoData) {
            self.PhotoSection(new photosViewModel(data, videoData));
        }
        
        function loadData() {
            // TODO: Load any static data
        }

        loadData();
    };
});