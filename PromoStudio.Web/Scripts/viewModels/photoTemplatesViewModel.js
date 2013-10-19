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

define([
        "viewModels/photoSlotViewModel",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger"
    ],
    function (
        photoSlotViewModel,
        ko,
        strings,
        enums,
        logger) {
        return function (storyboardItem, customerVideoItem, categoryId) {
            var self = this;
            storyboardItem = ko.utils.unwrapObservable(storyboardItem || {});
            customerVideoItem = ko.utils.unwrapObservable(customerVideoItem || {});

            self.Title = ko.observable(storyboardItem.Name());
            self.PhotoSlots = ko.observableArray([]);
            self.IsCompleted = ko.computed(function() {
                var slots = self.PhotoSlots(),
                    i;
                for (i = 0; i < slots.length; i++) {
                    if (!slots[i].IsCompleted()) {
                        return false;
                    }
                }
                return true;
            });

            function loadPhotoSlots() {
                var scriptItems = storyboardItem.TemplateScript().Items(),
                    custScriptItems = customerVideoItem.CustomerScript().Items(),
                    photos = [],
                    i, item;
                for (i = 0; i < scriptItems.length; i++) {
                    item = scriptItems[i];
                    // only add photos
                    if (item.fk_TemplateScriptItemTypeId() === 1
                        && item.fk_TemplateScriptItemCategoryId() === categoryId) {
                        // find matching script item
                        var matching = $.grep(custScriptItems, function (csi) {
                            return csi.fk_TemplateScriptItemId() === item.pk_TemplateScriptItemId();
                        });
                        if (matching && matching.length > 0) {
                            photos.push(new photoSlotViewModel(storyboardItem, matching[0], categoryId));
                        }
                    }
                }
                self.PhotoSlots(photos);
            }

            loadPhotoSlots();
        };
    });