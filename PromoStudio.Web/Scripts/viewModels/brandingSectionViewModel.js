/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />

define(["viewModels/photoTemplatesViewModel",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "bootstrap",
        "jqueryui"
    ],
    function (
        photoTemplatesViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
        return function (data, video) {
            var self = this,
                transitionTime = 350, /* from bootstrap-transitions */
                customerTemplateScripts;
            data = data || {};
            video = video || {};

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
            self.SelectedSlot = ko.observable(null);
            self.LogoPreviewShown = ko.computed(function () {
                return self.SelectedSlot() !== null;
            });
            self.IsSelected = function (slot) {
                return self.SelectedSlot() === slot;
            };
            self.SelectSlot = function (slot) {
                if (!slot.IsCompleted()) {
                    self.ChangeSlot(slot);
                } else {
                    if (self.IsSelected(slot)) {
                        self.SelectedSlot(null);
                    } else {
                        self.SelectedSlot(slot);
                    }
                }
            };
            self.ChangeSlot = function (slot) {
                slot.ChoosePhoto(function () {
                    self.SelectedSlot(slot);
                });
            };
            
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
            }
            
            function registerEvents() {
                $(function () {
                    var $elems = $("#brandingCollapse .panel-heading .step-title,#brandingCollapse .panel-heading .step-subtitle,#brandingCollapse .panel-heading .step-done");
                    $('#brandingPanel')
                        .on('show.bs.collapse', function() {
                            $elems.switchClass("collapsed", "opened", transitionTime);
                        })
                        .on('hide.bs.collapse', function () {
                            $elems.switchClass("opened", "collapsed", transitionTime);
                        });
                });
            }

            loadData(data.CustomerTemplateScripts, video);
            registerEvents();
        };
    });