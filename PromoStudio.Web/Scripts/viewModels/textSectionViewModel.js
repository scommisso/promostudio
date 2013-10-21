/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />

define(["viewModels/textItemViewModel",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "bootstrap",
        "jqueryui"
    ],
    function (
        textItemViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
        return function (data, video) {
            var self = this,
                transitionTime = 350, /* from bootstrap-transitions */
                textTitle = strings.getResource("BuildStep__Num_spots_can_be_customized"),
                customerTemplateScripts;
            data = data || {};
            video = video || {};

            self.TextTemplateItems = ko.observableArray([]);
            
            self.IsVisible = ko.computed(function () {
                var length = self.TextTemplateItems().length;
                return length > 0;
            });
            self.IsCompleted = ko.computed(function () {
                var items = self.TextTemplateItems(),
                    i;
                for (i = 0; i < items.length; i++) {
                    if (!items[i].IsCompleted()) {
                        return false;
                    }
                }
                return true;
            });
            
            self.StartOpen = ko.observable(false);
            self.TitleText = ko.computed(function () {
                var length = self.TextTemplateItems().length;
                if (length === 0) {
                    length = "no";
                }
                return textTitle.format(length,
                    length === 1 ? "" : "s");
            });
            
            function loadData(customerTemplateScriptData, videoData) {
                customerTemplateScripts = customerTemplateScriptData || [];
                loadVideoData(videoData);
            }

            function loadVideoData(videoData) {
                var storyboardData = videoData.Storyboard(),
                    items = videoData.Items(),
                    textTemplates = [],
                    storyboardItems, i, j, item, sbItem, sbItemType,
                    customerScript, scriptItems, scriptItem;

                storyboardItems = storyboardData.Items();

                for (i = 0; i < storyboardItems.length; i++) {
                    sbItem = storyboardItems[i];
                    sbItemType = sbItem.fk_StoryboardItemTypeId();

                    // Create template items
                    if (sbItemType === 1 || sbItemType === 3 || sbItemType === 4 || sbItemType === 5) {
                        // Look for any text matches in videoItems
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
                            customerScript = item.CustomerScript();
                            scriptItems = customerScript.Items();
                            for (j = 0; j < scriptItems.length; j++) {
                                scriptItem = scriptItems[j];
                                if (scriptItem.ScriptItem().fk_TemplateScriptItemTypeId() === 4) {
                                    textTemplates.push(new textItemViewModel(sbItem, scriptItem));
                                }
                            }
                        }
                    }
                }

                textTemplates.sort(function (a, b) {
                    // TODO: Sort by storyboard item, then by templatescript item
                    if (a.SortOrder() < b.SortOrder()) {
                        return -1;
                    }
                    if (a.SortOrder() > b.SortOrder()) {
                        return 1;
                    }
                    return 0;
                });
                self.TextTemplateItems(textTemplates);
            }
            
            function registerEvents() {
                $(function () {
                    var $elems = $("#textCollapse .panel-heading .step-title,#textCollapse .panel-heading .step-subtitle,#textCollapse .panel-heading .step-done");
                    $('#textPanel')
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