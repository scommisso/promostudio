/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/storyboardItem.js" />
/// <reference path="../models/customerTemplateScriptItem.js" />
/// <reference path="../models/customerResource.js" />
/// <reference path="photoChooserViewModel.js" />

"use strict";

define([
        "viewModels/photoChooserViewModel",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/extensions"
],
    function (
        photoChooserViewModel,
        ko,
        strings,
        enums,
        logger) {
        function ctor(storyboardItem, customerTemplateScriptItem, categoryId) {

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

            function onPhotoChosen(photo) {
                if (photo) {
                    self.ChosenResource(photo);
                }
                if (typeof photoCloseCallback === "function") {
                    photoCloseCallback();
                }
            }

            function onPhotoCanceled() {
                if (typeof photoCloseCallback === "function") {
                    photoCloseCallback();
                }
            }

            function createPhotoChooser() {
                $(function () {
                    photoChooser = new photoChooserViewModel({
                        Slot: self,
                        CategoryId: categoryId,
                        Element: $("#photoChooserModal"),
                        OnSave: onPhotoChosen,
                        OnCancel: onPhotoCanceled
                    });
                });
            }

            var self = this,
                photoTitleFormatString = strings.getResource("BuildStep__Section_num_timing"), //Sect. {0} - appx. {1} into video, slot {2}
                photoChooser, photoCloseCallback;

            self.ChosenResource = ko.observable(null);
            self.ChosenResource.subscribe(function(newResource) {
                customerTemplateScriptItem.fk_CustomerResourceId(newResource.pk_CustomerResourceId());
                customerTemplateScriptItem.Resource(newResource);
            });
            self.Title = ko.computed(function () {
                var storyboardSort = storyboardItem.SortOrder(),
                    storyboardTiming = calculateStoryboardTiming(storyboardItem),
                    scriptItem = customerTemplateScriptItem.ScriptItem(),
                    scriptItemSort = scriptItem.SortOrder();
                return photoTitleFormatString
                    .format(storyboardSort, storyboardTiming, scriptItemSort);
            });
            self.PhotoUrl = ko.computed(function () {
                var res = customerTemplateScriptItem.Resource();
                if (res == null) { return null; }
                return res.LinkUrl();
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
            self.SortOrder = ko.computed(function () {
                var sortValue = (storyboardItem.SortOrder() * 10000) + customerTemplateScriptItem.ScriptItem().SortOrder();
                return sortValue;
            });

            self.ChoosePhoto = function (callback) {
                // pop modal to select a photo
                logger.log("choosing photo");
                photoCloseCallback = callback;
                photoChooser.Show(customerTemplateScriptItem.fk_CustomerResourceId());
            };

            createPhotoChooser();
        }

        return ctor;
    });