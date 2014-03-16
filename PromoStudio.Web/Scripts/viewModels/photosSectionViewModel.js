/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />

"use strict";

define(["viewModels/photoTemplatesViewModel",
        "jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/extensions",
        "lib/jquery.crop"
],
    function (
        photoTemplatesViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
        function ctor(data, video) {

            var self = this,
                transitionTime = 350, /* from bootstrap-transitions */
                photoTitle = strings.getResource("BuildStep__Num_spots_need_your_own_photos"),
                customerTemplateScripts;
            data = data || {};
            video = video || {};

            function loadData(customerTemplateScriptData, videoData) {
                customerTemplateScripts = customerTemplateScriptData || [];
                loadVideoData(videoData);
            }

            function loadVideoData(videoData) {
                var storyboardData = videoData.Storyboard(),
                    items = videoData.Items(),
                    photoTemplates = [],
                    storyboardItems, i, item, sbItem, sbItemType;

                storyboardItems = storyboardData.Items();

                for (i = 0; i < storyboardItems.length; i++) {
                    sbItem = storyboardItems[i];
                    sbItemType = sbItem.fk_StoryboardItemTypeId();

                    // Create template items
                    if (sbItemType === 1 || sbItemType === 3 || sbItemType === 4 || sbItemType === 5) {
                        // Look for any photo matches in videoItems
                        item = $.grep(items, function (cvi) {
                            var itemType = cvi.fk_CustomerVideoItemTypeId(),
                                custScript;
                            if (itemType !== 3) { return false; }
                            custScript = cvi.CustomerScript();
                            if (!custScript) { return false; }
                            return custScript.fk_TemplateScriptId() === sbItem.fk_TemplateScriptId();
                        });
                        if (item && item.length > 0) {
                            item = item[0];
                            photoTemplates.push(new photoTemplatesViewModel(sbItem, item, 2));
                        }
                    }
                }

                self.PhotoTemplates(photoTemplates);
            }

            function registerEvents() {
                $(function () {
                    var $panel = $("#photoPanel .photo-selected");
                    self.PhotoPreviewShown.subscribe(function (newVal) {
                        if (newVal) {
                            $panel.slideDown(transitionTime);
                        }
                    });
                });
            }

            self.StepNumber = ko.observable(1);
            self.PhotoTemplates = ko.observableArray([]);
            self.PhotoSlots = ko.computed(function () {
                var templates = self.PhotoTemplates(),
                    slots = [],
                    templateSlots, i, j;
                for (i = 0; i < templates.length; i++) {
                    templateSlots = templates[i].PhotoSlots();
                    for (j = 0; j < templateSlots.length; j++) {
                        slots.push(templateSlots[j]);
                    }
                }
                slots.sort(function (a, b) {
                    if (a.SortOrder() < b.SortOrder()) {
                        return -1;
                    }
                    if (a.SortOrder() > b.SortOrder()) {
                        return 1;
                    }
                    return 0;
                });
                return slots;
            });
            self.SelectedSlot = ko.observable(null);
            self.PhotoPreviewShown = ko.computed(function () {
                return self.SelectedSlot() !== null;
            });
            self.IsSelected = function (slot) {
                return self.SelectedSlot() === slot;
            };
            self.SelectSlot = function (slot) {
                if (!slot.IsCompleted()) {
                    self.ChangeSlot(slot);
                } else {
                    self.SelectedSlot(slot);
                }
            };
            self.ChangeSlot = function (slot) {
                slot.ChoosePhoto(function () {
                    self.SelectedSlot(slot);
                });
            };

            self.IsVisible = ko.computed(function () {
                var length = self.PhotoSlots().length;
                return length > 0;
            });
            self.IsCompleted = ko.computed(function () {
                var slots = self.PhotoSlots(),
                    i;
                for (i = 0; i < slots.length; i++) {
                    if (!slots[i].IsCompleted()) {
                        return false;
                    }
                }
                return true;
            });

            self.StartOpen = ko.observable(false);
            self.TitleText = ko.computed(function () {
                var length = self.PhotoSlots().length;
                if (length === 0) {
                    length = "no";
                }
                return photoTitle.format(length,
                    length === 1 ? "" : "s",
                    length === 1 ? "s" : "");
            });

            loadData(data.CustomerTemplateScripts, video);
            registerEvents();
        }

        return ctor;
    });