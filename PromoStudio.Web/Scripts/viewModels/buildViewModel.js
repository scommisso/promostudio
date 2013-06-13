/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/customerVideo",
        "models/customerVideoItem",
        "models/templateScript",
        "models/templateScriptItem",
        "models/stockVideo",
        "models/stockAudio",
        "models/customerResource",
        "models/customerTemplateScript",
        "models/customerTemplateScriptItem",
        "models/enums",
        "jquery",
        "knockout"], function (
            customerVideo,
            customerVideoItem,
            templateScript,
            templateScriptItem,
            stockVideo,
            stockAudio,
            customerResource,
            customerTemplateScript,
            customerTemplateScriptItem,
            enums,
            $,
            ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.Video = ko.observable(null);
        self.TemplateScripts = ko.observableArray([]);
        self.StockVideos = ko.observableArray([]);
        self.StockAudio = ko.observableArray([]);
        self.CustomerResources = ko.observableArray([]);

        self.ActiveItem = ko.observable(null);
        self.IsIntroSelected = ko.observable(false);
        self.IsMiddleSelected = ko.observable(false);
        self.IsEndingSelected = ko.observable(false);
        self.IsAudioSelected = ko.observable(false);

        self.Enums = enums;

        self.GenerateVideo = function () {
            $.ajax({
                type: "POST",
                url: "/Build/Submit",
                data: ko.toJSON(self.Video()),
                contentType: "application/json; charset=utf-8",
                success: function (data, textStatus, jqXHR) {
                    if (data.Success !== true) {
                        alert("ERROR");
                        return;
                    }
                    alert("Successfully generated.");
                    data = data.Model;
                    console.log("Saved Video");
                    console.log(data);
                    self.Video(new customerVideo(data));

                    // TODO: "Your video is being generated" screen
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("ERROR");
                }
            });
        };

        self.UpdateStatus = function () {
            $.ajax({
                type: "GET",
                url: "/Build/Status?cvid=" + self.Video().pk_CustomerVideoId(),
                dataType: "json",
                success: function (data, textStatus, jqXHR) {
                    if (data.Success !== true) {
                        alert("ERROR");
                        return;
                    }
                    console.log("Retrieved video " + data.pk_CustomerVideoId + " status: " + data.fk_CustomerVideoRenderStatusId);
                    console.log(data);
                    self.Video().fk_CustomerVideoRenderStatusId(data.fk_CustomerVideoRenderStatusId);
                    self.Video().DateUpdated(data.DateUpdated);

                    // TODO: Remove these -- do not expose to UI, these are for demo purposes only
                    self.Video().PreviewFilePath(data.PreviewFilePath);
                    self.Video().CompletedFilePath(data.CompletedFilePath);

                    // TODO: Show "Your video is being generated" screen
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error retrieving video status: " + textStatus);
                    console.log(errorThrown);
                }
            });
        };

        self.SetActiveItemValue = function (footageItem) {
            var activeItem = self.ActiveItem(),
                item, itemType;
            if (activeItem === null) { return; }
            itemType = activeItem.fk_CustomerVideoItemTypeId();

            if (itemType === 1) {
                // Stock Video
                activeItem.fk_CustomerVideoItemId(footageItem.pk_StockVideoId());
                activeItem.FootageItem(footageItem);
            }
            else if (itemType === 2) {
                // Stock Audio
                activeItem.fk_CustomerVideoItemId(footageItem.pk_StockAudioId());
                activeItem.FootageItem(footageItem);
            }
            else if (itemType === 3) {
                // Template Script
                item = new customerTemplateScript();
                footageItem = new templateScript(ko.toJS(footageItem));
                item.LoadTemplateData(footageItem);
                activeItem.fk_CustomerVideoItemId(footageItem.pk_TemplateScriptId());
                activeItem.FootageItem(item);
            }
            else if (itemType === 4) {
                // Voice Over
                // TODO: Implement
            }
        };

        self.SetActive = function (section) {
            self.IsIntroSelected(false);
            self.IsMiddleSelected(false);
            self.IsEndingSelected(false);
            self.IsAudioSelected(false);
            self.ActiveItem(null);
            if (section === "intro") {
                self.IsIntroSelected(true);
                self.ActiveItem(self.Video().Items()[0]);
            }
            else if (section === "middle") {
                self.IsMiddleSelected(true);
                self.ActiveItem(self.Video().Items()[1]);
            }
            else if (section === "ending") {
                self.IsEndingSelected(true);
                self.ActiveItem(self.Video().Items()[2]);
            }
            else if (section === "audio") {
                self.IsAudioSelected(true);
                self.ActiveItem(self.Video().Items()[3]);
            }
        };

        self.GetMatchingResources = function (templateScriptItem) {
            var filtered = ko.utils.arrayFilter(self.CustomerResources(), function (resource) {
                return resource.fk_TemplateScriptItemTypeId() === templateScriptItem.fk_TemplateScriptItemTypeId()
                    && resource.fk_TemplateScriptItemCategoryId() === templateScriptItem.fk_TemplateScriptItemCategoryId()
            });
            return filtered;
        };

        self.LoadItems = function (scripts, videos, audio, resources) {
            var i, item;
            scripts = scripts || [];
            videos = videos || [];
            audio = audio || [];
            resources = resources || [];

            for (i = 0; i < scripts.length; i++) {
                item = scripts[i];
                scripts[i] = new templateScript(item);
            }

            for (i = 0; i < videos.length; i++) {
                item = videos[i];
                videos[i] = new stockVideo(item);
            }

            for (i = 0; i < audio.length; i++) {
                item = audio[i];
                audio[i] = new stockAudio(item);
            }

            for (i = 0; i < resources.length; i++) {
                item = resources[i];
                resources[i] = new customerResource(item);
            }

            self.TemplateScripts(scripts);
            self.StockVideos(videos);
            self.StockAudio(audio);
            self.CustomerResources(resources);
            self.Video(new customerVideo({
                fk_CustomerId: 1, Items: [
                    { fk_CustomerVideoItemTypeId: 3, SortOrder: 1 },
                    { fk_CustomerVideoItemTypeId: 1, SortOrder: 2 },
                    { fk_CustomerVideoItemTypeId: 3, SortOrder: 3 },
                    { fk_CustomerVideoItemTypeId: 2 }
                ]
            }));
        };
        self.LoadItems(data.TemplateScripts, data.StockVideos, data.StockAudio, data.CustomerResources);
    };
});