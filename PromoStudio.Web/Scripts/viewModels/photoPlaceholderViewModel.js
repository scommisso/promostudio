/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../models/storyboardItem.js" />
/// <reference path="../models/templateScript.js" />
/// <reference path="../models/templateScriptItem.js" />

define(["knockout",
        "strings",
        "models/enums",
        "ps/logger"
    ],
    function (
        ko,
        strings,
        enums,
        logger) {
        return function (storyboardItem, customerVideoItem) {
            var self = this;
            storyboardItem = ko.utils.unwrapObservable(storyboardItem || {});
            customerVideoItem = ko.utils.unwrapObservable(customerVideoItem || {});

            self.Title = ko.observable(storyboardItem.Name());
            self.PhotoSlots = ko.observableArray(storyboardItem.TemplateScript().Items());
            self.IsCompleted = ko.computed(function() {
                var slots = self.PhotoSlots(),
                    isCompleted = true,
                    i;
                for (i = 0; i < slots.length; i++) {
                    if (!slots[i].Value()) {
                        isCompleted = false;
                        break;
                    }
                }
                return isCompleted;
            });
        };
    });