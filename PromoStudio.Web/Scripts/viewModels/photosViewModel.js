/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/customerTemplateScript.js" />
/// <reference path="../models/customerTemplateScriptItem.js" />
/// <reference path="../models/customerVideoItem.js" />

define(["viewModels/photoTemplatesViewModel",
        "models/customerTemplateScript",
        "models/customerTemplateScriptItem",
        "models/customerVideoItem",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "lib/ko.custom",
        "ps/extensions",
        "bootstrap",
        "jqueryui"
    ],
    function (
        photoTemplatesViewModel,
        customerTemplateScript,
        customerTemplateScriptItem,
        customerVideoItem,
        $,
        ko,
        strings,
        enums,
        logger) {
        return function (data, video) {
            var self = this,
                transitionTime = 350, /* from bootstrap-transitions */
                photoTitle = strings.getResource("BuildStep__Num_spots_need_your_own_photos"),
                customerTemplateScripts;
            data = data || {};
            video = video || {};

            self.PhotoTemplates = ko.observableArray([]);
            self.PhotoSlots = ko.computed(function() {
                var templates = self.PhotoTemplates(),
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
            self.IsSelected = function (slot) {
                return self.SelectedSlot() === slot;
            };
            self.SelectSlot = function (slot) {
                self.SelectedSlot(slot);
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
            
            self.StartOpen = ko.observable(true);
            self.TitleText = ko.computed(function () {
                var length = self.PhotoSlots().length;
                if (length === 0) {
                    length = "no";
                }
                return photoTitle.format(length,
                    length === 1 ? "" : "s",
                    length === 1 ? "s" : "");
            });
            
            function loadData(customerTemplateScriptData, videoData) {
                customerTemplateScripts = customerTemplateScriptData || [];
                loadVideoData(videoData);
            }

            function loadVideoData(videoData) {
                var storyboardData = videoData.Storyboard(),
                    items = videoData.Items(),
                    photoTemplates = [],
                    storyboardItems, i, item, sbItem, sbItemType, scriptItem;

                storyboardItems = storyboardData.Items();

                for (i = 0; i < storyboardItems.length; i++) {
                    sbItem = storyboardItems[i];
                    sbItemType = sbItem.fk_StoryboardItemTypeId();
                    
                    // Create template items
                    if (sbItemType === 1 || sbItemType === 3 || sbItemType === 4 || sbItemType === 5) {
                        // Look for any photo matches in videoItems
                        item = $.grep(items, function(cvi) {
                            var itemType = cvi.fk_CustomerVideoItemTypeId(),
                                custScript;
                            if (itemType !== 3) { return false; }
                            custScript = cvi.CustomerScript();
                            if (!custScript) { return false; }
                            return custScript.fk_TemplateScriptId() === sbItem.fk_TemplateScriptId();
                        });
                        
                        if (item && item.length > 0) {
                            item = item[0];
                        } else {
                            item = null;
                        }
                        if (item === null) {
                            // setup a new script item as necessary
                            item = new customerVideoItem({
                                fk_CustomerVideoItemTypeId: 3,
                                SortOrder: sbItem.SortOrder()
                            });
                            scriptItem = new customerTemplateScript({ fk_CustomerId: video.fk_CustomerId() });
                            scriptItem.LoadTemplateScriptData(sbItem.TemplateScript());
                            item.FootageItem(scriptItem);
                            videoData.Items.push(item);
                        }
                        photoTemplates.push(new photoTemplatesViewModel(sbItem, item));
                    }
                    
                    // Create stock video items
                    else if (sbItemType === 2) {
                        // look for any stock video matches in videoItems
                        item = $.grep(items, function (cvi) {
                            var itemType = cvi.fk_CustomerVideoItemTypeId(),
                                videoId = cvi.fk_CustomerVideoItemId();
                            if (itemType !== 1) { return false; }
                            return cvi.fk_CustomerVideoItemId() === sbItem.fk_StockVideoId();
                        });

                        if (item && item.length > 0) {
                            item = item[0];
                        } else {
                            item = null;
                        }
                        if (item === null) {
                            // setup a new script item as necessary
                            item = new customerVideoItem({
                                fk_CustomerVideoItemTypeId: 1,
                                fk_CustomerVideoItemId: sbItem.fk_StockVideoId(),
                                SortOrder: sbItem.SortOrder()
                            });
                            item.FootageItem(sbItem.StockVideo());
                            videoData.Items.push(item);
                        }
                    }
                }

                self.PhotoTemplates(photoTemplates);
            }
            
            function registerEvents() {
                $(function () {
                    var $elems = $("#photoCollapse .panel-heading .step-title,#photoCollapse .panel-heading .step-subtitle,#photoCollapse .panel-heading .step-done");
                    $('#photoPanel')
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