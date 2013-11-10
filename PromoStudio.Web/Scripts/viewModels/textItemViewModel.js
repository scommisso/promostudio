/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../models/storyboardItem.js" />
/// <reference path="../models/customerTemplateScript.js" />
/// <reference path="../models/customerTemplateScriptItem.js" />
/// <reference path="../models/templateScript.js" />
/// <reference path="../models/templateScriptItem.js" />
/// <reference path="photoSlotViewModel.js" />

"use strict";

define([
        "knockout",
        "strings",
        "models/enums",
        "ps/logger"
],
    function (
        ko,
        strings,
        enums,
        logger) {
        function ctor(storyboardItem, customerTemplateScriptItem) {
            var self = this,
                textTimingFormatString = strings.getResource("BuildStep__Section_num_timing"); //Sect. {0} - appx. {1} into video, slot {2}

            customerTemplateScriptItem = ko.utils.unwrapObservable(customerTemplateScriptItem || {});

            self.Name = customerTemplateScriptItem.ScriptItem().Name;
            self.Description = customerTemplateScriptItem.ScriptItem().Description;
            self.TextValue = customerTemplateScriptItem.Resource().Value;
            self.SortOrder = ko.computed(function () {
                var sortValue =
                    storyboardItem.SortOrder() * 10000
                        + customerTemplateScriptItem.ScriptItem().SortOrder();
                return sortValue;
            });

            self.TimingText = ko.computed(function () {
                var storyboardSort = storyboardItem.SortOrder(),
                    storyboardTiming = calculateStoryboardTiming(storyboardItem),
                    scriptItem = customerTemplateScriptItem.ScriptItem(),
                    scriptItemSort = scriptItem.SortOrder();
                return textTimingFormatString
                    .format(storyboardSort, storyboardTiming, scriptItemSort);
            });

            self.IsCompleted = ko.computed(function () {
                var val = self.TextValue();
                return (val && val.length > 0);
            });

            function calculateStoryboardTiming() {
                var prevSeconds = 0,
                    sortOrder = storyboardItem.SortOrder(),
                    storyboard = storyboardItem.Storyboard(),
                    i, sbItems, sbItem;
                if (!storyboard) {
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
        }

        return ctor;
    });