/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/customerResource.js" />
/// <reference path="../models/customerTemplateScript.js" />
/// <reference path="../models/customerVideoItem.js" />
/// <reference path="../models/organization.js" />

"use strict";

define(["viewModels/photosSectionViewModel",
        "viewModels/brandingSectionViewModel",
        "viewModels/textSectionViewModel",
        "viewModels/callToActionSectionViewModel",
        "viewModels/endingSectionViewModel",
        "models/customerTemplateScript",
        "models/customerVideoItem",
        "models/customerResource",
        "models/organization",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "lib/ko.custom",
        "ps/extensions"],
    function (
        photosSectionViewModel,
        brandingSectionViewModel,
        textSectionViewModel,
        ctaSectionViewModel,
        endingSectionViewModel,
        customerTemplateScript,
        customerVideoItem,
        customerResource,
        organization,
        $,
        ko,
        strings,
        enums,
        logger) {
        function ctor(data) {

            function stepChanging(navVm, callback) {
                callback();
            }

            function createScriptItemTextResources(scriptItem) {
                var items = scriptItem.Items(),
                    vid = video(),
                    i, item, tsItem, res, defText;
                for (i = 0; i < items.length; i++) {
                    item = items[i];
                    tsItem = item.ScriptItem();
                    defText = tsItem.DefaultText();
                    if (tsItem && tsItem.fk_TemplateScriptItemTypeId() === 4) {
                        res = new customerResource({
                            fk_CustomerId: vid.fk_CustomerId(),
                            fk_TemplateScriptItemTypeId: 4,
                            fk_TemplateScriptItemCategoryId: 5,
                            fk_CustomerResourceStatusId: 1
                        });
                        if (customerOrganization !== null) {
                            defText = defText.replace(/\[\[COMPANY\]\]/, customerOrganization.Name());
                            defText = defText.replace(/\[\[PHONENUM\]\]/, customerOrganization.ContactPhone());
                            defText = defText.replace(/\[\[EMAIL\]\]/, customerOrganization.ContactEmail());
                            defText = defText.replace(/\[\[WEBSITE\]\]/, customerOrganization.Website());
                        }
                        res.Value(defText);
                        item.Resource(res);
                    }
                }
            }

            function loadVideoData(videoData) {
                var storyboardData = videoData.Storyboard(),
                    items = videoData.Items(),
                    vid = video(),
                    storyboardItems, i, item, sbItem, sbItemType, scriptItem;

                storyboardItems = storyboardData.Items();

                for (i = 0; i < storyboardItems.length; i++) {
                    sbItem = storyboardItems[i];
                    sbItemType = sbItem.fk_StoryboardItemTypeId();

                    // Create template items
                    if (sbItemType === 1 || sbItemType === 3 || sbItemType === 4 || sbItemType === 5) {
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
                        } else {
                            item = null;
                        }
                        if (item === null) {
                            // setup a new script item as necessary
                            item = new customerVideoItem({
                                fk_CustomerVideoItemTypeId: 3,
                                SortOrder: sbItem.SortOrder()
                            });
                            scriptItem = new customerTemplateScript({ fk_CustomerId: vid.fk_CustomerId() });
                            scriptItem.LoadTemplateScriptData(sbItem.TemplateScript());
                            createScriptItemTextResources(scriptItem);
                            item.FootageItem(scriptItem);
                            videoData.Items.push(item);
                        }
                    }

                        // Create stock video items
                    else if (sbItemType === 2) {
                        // look for any stock video matches in videoItems
                        item = $.grep(items, function (cvi) {
                            var itemType = cvi.fk_CustomerVideoItemTypeId();
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

                self.PhotoSection(new photosSectionViewModel(data, videoData));
                self.BrandingSection(new brandingSectionViewModel(data, videoData));
                self.TextSection(new textSectionViewModel(data, videoData));
                self.CtaSection(new ctaSectionViewModel(data, videoData));
                self.EndingSection(new endingSectionViewModel(data, videoData));

                if (self.PhotoSection().IsVisible() && !self.PhotoSection().IsCompleted()) {
                    self.PhotoSection().StartOpen(true);
                }
                else if (self.BrandingSection().IsVisible() && !self.BrandingSection().IsCompleted()) {
                    self.BrandingSection().StartOpen(true);
                }
                else if (self.TextSection().IsVisible() && !self.TextSection().IsCompleted()) {
                    self.TextSection().StartOpen(true);
                }
                else if (self.CtaSection().IsVisible() && !self.CtaSection().IsCompleted()) {
                    self.CtaSection().StartOpen(true);
                }
                else if (self.EndingSection().IsVisible() && !self.EndingSection().IsCompleted()) {
                    self.EndingSection().StartOpen(true);
                }

                var step = 1;
                if (self.PhotoSection().IsVisible()) {
                    self.PhotoSection().StepNumber(step);
                    step += 1;
                }
                if (self.BrandingSection().IsVisible()) {
                    self.BrandingSection().StepNumber(step);
                    step += 1;
                }
                if (self.TextSection().IsVisible()) {
                    self.TextSection().StepNumber(step);
                    step += 1;
                }
                if (self.CtaSection().IsVisible()) {
                    self.CtaSection().StepNumber(step);
                    step += 1;
                }
                if (self.EndingSection().IsVisible()) {
                    self.EndingSection().StepNumber(step);
                    step += 1;
                }
            }

            function loadData() {
                customerOrganization = new organization(data.Organization);
            }

            var self = this;
            data = data || {};

            var isStepCompleted = null,
                video = null,
                customerOrganization = null;

            self.PhotoSection = ko.observable({});
            self.BrandingSection = ko.observable({});
            self.TextSection = ko.observable({});
            self.CtaSection = ko.observable({});
            self.EndingSection = ko.observable({});

            self.Bind = function (selector, navSelector) {
                ko.callbackOnBind($(navSelector)[0], function (navVm) {
                    isStepCompleted = navVm.IsStepCompleted;
                    video = navVm.Video;
                    navVm.BeforeStepChange = stepChanging;

                    loadVideoData(video());

                    self.IsCompleted(); // check completed status

                    ko.applyBindings(self, $(selector)[0]);
                }, 1000);
            };
            self.IsCompleted = ko.computed(function () {
                var ps = ko.toJS(self.PhotoSection()),
                    bs = ko.toJS(self.BrandingSection()),
                    ts = ko.toJS(self.TextSection()),
                    cs = ko.toJS(self.CtaSection()),
                    ts = ko.toJS(self.TextSection()),
                    es = ko.toJS(self.EndingSection()),
                    stepCompleted = true;
                if (!ps || (ps.IsVisible && !ps.IsCompleted)) {
                    stepCompleted = false;
                }
                else if (!bs || (bs.IsVisible && !bs.IsCompleted)) {
                    stepCompleted = false;
                }
                else if (!ts || (ts.IsVisible && !ts.IsCompleted)) {
                    stepCompleted = false;
                }
                else if (!cs || (cs.IsVisible && !cs.IsCompleted)) {
                    stepCompleted = false;
                }
                else if (!es || (es.IsVisible && !es.IsCompleted)) {
                    stepCompleted = false;
                }

                if (ko.isObservable(isStepCompleted)) {
                    isStepCompleted(stepCompleted);
                }
                return stepCompleted;
            });

            loadData();
        }

        return ctor;
    });