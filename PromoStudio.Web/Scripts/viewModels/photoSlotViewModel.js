/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/storyboardItem.js" />
/// <reference path="../models/customerTemplateScriptItem.js" />
/// <reference path="../models/customerResource.js" />

define(["knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/extensions"
    ],
    function (
        ko,
        strings,
        enums,
        logger) {
        return function (storyboardItem, customerTemplateScriptItem) {
            var self = this,
                photoTitleFormatString = strings.getResource("BuildStep__Section_num_timing"); //Sect. {0} - appx. {1} into video, slot {2}

            self.Title = ko.computed(function () {
                var storyboardSort = storyboardItem.SortOrder(),
                    storyboardTiming = calculateStoryboardTiming(storyboardItem),
                    scriptItem = customerTemplateScriptItem.ScriptItem(),
                    scriptItemSort = scriptItem.SortOrder();
                return photoTitleFormatString
                    .format(storyboardSort, storyboardTiming, scriptItemSort);
            });
            self.PhotoUrl = ko.computed(function() {
                var res = customerTemplateScriptItem.Resource();
                if (res == null) { return null; }
                return res.LinkUrl();
            });
            self.PhotoBackground = ko.computed(function() {
                var url = self.PhotoUrl();
                if (url === null) { return "none"; }
                return "url({0})".format(url);
            });
            self.PhotoName = ko.computed(function () {
                var res = customerTemplateScriptItem.Resource();
                if (res == null) { return null; }
                return res.LinkFileName();
            });
            self.IsCompleted = ko.computed(function () {
                var res = customerTemplateScriptItem.Resource();
                return res !== null;
            });
            
            function calculateStoryboardTiming() {
                var prevSeconds = 0,
                    sortOrder = storyboardItem.SortOrder(),
                    storyboard = storyboardItem.Storyboard(),
                    i, sbItems, sbItem;
                if (storyboard === null) {
                    return prevSeconds;
                }
                sbItems = storyboard.Items();
                for (i = 0; i < sbItems.length; i++) {
                    sbItem = sbItems[i];
                    if (sbItem.SortOrder() < sortOrder) {
                        prevSeconds += sbItem.LengthInSeconds();
                    }
                }
                return prevSeconds.toString().toMMSS(false);
            }

            self.AddPhoto = function () {
                // TODO: Add photo chooser module
                // TODO: Show list of existing photos
                // TODO: Present file uploader
            };
        };
    });