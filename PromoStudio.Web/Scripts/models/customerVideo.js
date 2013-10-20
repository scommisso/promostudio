/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="customerVideoItem.js" />
/// <reference path="enums.js" />

define(["models/customerVideoItem",
        "models/storyboard",
        "models/customerVideoVoiceOver",
        "models/enums",
        "knockout"],
    function (
        customerVideoItem,
        storyboard,
        customerVideoVoiceOver,
        enums,
        ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerVideoId = ko.observable(data.pk_CustomerVideoId || null);
        self.fk_CustomerId = ko.observable(data.fk_CustomerId || null);
        self.fk_CustomerVideoRenderStatusId = ko.observable(data.fk_CustomerVideoRenderStatusId || 1); // pending
        self.fk_StoryboardId = ko.observable(data.fk_StoryboardId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.RenderFailureMessage = ko.observable(data.RenderFailureMessage || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
        self.DateCompleted = ko.observable(data.DateCompleted || null);
        self.PreviewFilePath = ko.observable(data.PreviewFilePath || null);
        self.CompletedFilePath = ko.observable(data.CompletedFilePath || null);

        self.Storyboard = ko.observable(null);
        self.VoiceOver = ko.observable(null);
        self.Items = ko.observableArray([]);

        self.CustomerVideoRenderStatus = ko.computed(function () {
            var id = self.fk_CustomerVideoRenderStatusId();
            return enums.customerVideoRenderStatus(id);
        });
        self.LinkUrl = ko.computed(function () {
            var previewPath = self.PreviewFilePath(),
                completedPath = self.CompletedFilePath(),
                displayPath = completedPath || previewPath;
            if (displayPath === null) { return "javascript:void(0);"; }
            return displayPath;
        });
        self.LinkFileName = ko.computed(function () {
            var previewPath = self.PreviewFilePath(),
                completedPath = self.CompletedFilePath(),
                displayPath = completedPath || previewPath,
                ix;
            if (displayPath === null) { return "Video Not Available"; }
            ix = displayPath.lastIndexOf("\\");
            if (ix === -1) { ix = displayPath.lastIndexOf("/"); }
            if (ix === -1) { return displayPath; }
            displayPath = displayPath.substring(ix + 1);
            ix = displayPath.indexOf("?");
            if (ix !== -1) { displayPath = displayPath.substring(0, ix); }
            return displayPath;
        });

        self.LoadItems = function (storyboardData, voiceOverData, items) {
            var i, item;
            items = items || [];

            storyboardData = storyboardData || {};
            storyboardData = new storyboard(storyboardData);

            if (voiceOverData) {
                self.VoiceOver(new customerVideoVoiceOver(voiceOverData));
            }

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new customerVideoItem(item);
            }

            self.Storyboard(storyboardData);
            self.Items(items);
        };
        self.LoadItems(data.Storyboard, data.VoiceOver, data.Items);
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.DateCreated;
        delete copy.DateUpdated;
        delete copy.DateCompleted;
        delete copy.RenderFailureMessage;
        delete copy.PreviewFilePath;
        delete copy.CompletedFilePath;
        delete copy.Storyboard;
        delete copy.CustomerVideoRenderStatus;
        delete copy.LinkUrl;
        delete copy.LinkFileName;

        return copy;
    };
    return ctor;
});
