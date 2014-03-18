/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />

"use strict";

define(["viewModels/photoTemplatesViewModel",
        "jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
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
                    logoTemplates = [],
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
                            logoTemplates.push(new photoTemplatesViewModel(sbItem, item, 1));
                        }
                    }
                }

                self.LogoTemplates(logoTemplates);

                var slots = self.LogoSlots();
                if (slots && slots.length > 0) {
                    self.LogoItem(slots[0]);
                }
            }

            self.LogoItem = ko.observable(null);
            self.LogoUrl = ko.computed(function () {
                var logoItem = self.LogoItem();
                if (!logoItem) { return null; }
                return logoItem.PhotoUrl();
            });
            self.LogoName = ko.computed(function () {
                var logoItem = self.LogoItem();
                if (!logoItem) { return null; }
                return logoItem.PhotoName();
            });

            self.StepNumber = ko.observable(1);
            self.LogoTemplates = ko.observableArray([]);
            self.LogoSlots = ko.computed(function () {
                var templates = self.LogoTemplates(),
                    slots = [],
                    templateSlots, i, j;
                for (i = 0; i < templates.length; i++) {
                    templateSlots = templates[i].PhotoSlots();
                    for (j = 0; j < templateSlots.length; j++) {
                        slots.push(templateSlots[j]);
                    }
                }
                return slots;
            });
            
            self.IsVisible = ko.computed(function () {
                var length = self.LogoSlots().length;
                return length > 0;
            });
            self.IsCompleted = ko.computed(function () {
                var slots = self.LogoSlots(),
                    i;
                for (i = 0; i < slots.length; i++) {
                    if (!slots[i].IsCompleted()) {
                        return false;
                    }
                }
                return true;
            });

            self.StartOpen = ko.observable(false);

            self.ChooseLogo = function (d, e) {
                var logoItem = self.LogoItem();
                if (!logoItem) {
                    e.preventDefault();
                    return true;
                }

                // pop modal to select a photo
                logger.log("choosing logo");
                logoItem.ChoosePhoto(function () {
                    // apply resource to other logo items
                    var res = self.LogoItem().ChosenResource(),
                        slots = self.LogoSlots(),
                        i;
                    if (!res) { return; }
                    for (i = 0; i < slots.length; i++) {
                        var j = slots[i].ChosenResource(res);
                    }
                });
            };

            loadData(data.CustomerTemplateScripts, video);
        }

        return ctor;
    });