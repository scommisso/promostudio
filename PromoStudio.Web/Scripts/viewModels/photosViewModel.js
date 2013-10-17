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

define(["viewModels/photoPlaceholderViewModel",
        "models/customerTemplateScript",
        "models/customerTemplateScriptItem",
        "models/customerVideoItem",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "lib/ko.custom",
        "ps/extensions", ,
        "bootstrap",
        "jqueryui"
    ],
    function (
        photoPlaceholderViewModel,
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
                customerTemplateScripts = [];
            data = data || {};
            video = video || {};

            self.PhotoTemplates = ko.observableArray([]);
            self.PhotoPlaceholders = ko.computed(function() {
                var templates = self.PhotoTemplates(),
                    placeholders = [],
                    slots, i, j;
                for (i = 0; i < templates.length; i++) {
                    slots = templates[i].PhotoSlots();
                    for (j = 0; j < slots.length; j++) {
                        placeholders.push(slots[j]);
                    }
                }
                return placeholders;
            });
            
            self.IsVisible = ko.computed(function () {
                var length = self.PhotoTemplates().length;
                return length > 0;
            });
            self.IsCompleted = ko.computed(function () {
                // TODO: Check if all photos have been selected
                return true;//true;
            });
            self.StartOpen = ko.observable(true);
            self.TitleText = ko.computed(function () {
                var length = self.PhotoPlaceholders().length;
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
                    storyboardItems, i, item, sbItem, scriptItem;

                storyboardItems = storyboardData.Items();

                for (i = 0; i < storyboardItems.length; i++) {
                    sbItem = storyboardItems[i];
                    if (sbItem.fk_StoryboardItemTypeId() === 4) {
                        // Look for any matches in videoItems
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
                                fk_CustomerVideoItemTypeId: 3
                            });
                            scriptItem = new customerTemplateScript({ fk_CustomerId: video.fk_CustomerId() });
                            scriptItem.LoadTemplateScriptData(sbItem.TemplateScript());
                            item.FootageItem(scriptItem);
                            videoData.Items.push(item);
                        }
                        photoTemplates.push(new photoPlaceholderViewModel(sbItem, item));
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