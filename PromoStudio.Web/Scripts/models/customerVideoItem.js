/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["knockout"], function (ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerVideoItemId = ko.observable(data.pk_CustomerVideoItemId || null);
        self.fk_CustomerVideoId = ko.observable(data.fk_CustomerVideoId || null);
        self.fk_CustomerVideoItemId = ko.observable(data.fk_CustomerVideoItemId || null);
        self.fk_CustomerVideoItemTypeId = ko.observable(data.fk_CustomerVideoItemTypeId || null);
        self.SortOrder = ko.observable(data.SortOrder || null);

        self.FootageItem = ko.observable(null);
        self.FootageItemName = ko.computed(function () {
            var item = self.FootageItem(),
                type = self.fk_CustomerVideoItemTypeId();
            if (item === null) { return null; }
            if (type === 3) {
                item = item.Template();
                if (item === null) { return null; }
            }
            return item.Name();
        });

        self.StockVideo = ko.computed(function () {
            var type = self.fk_CustomerVideoItemTypeId(),
                item = self.FootageItem();
            if (item === null || type !== 1) { return null; }
            return item;
        });

        self.StockAudio = ko.computed(function () {
            var type = self.fk_CustomerVideoItemTypeId(),
                item = self.FootageItem();
            if (item === null || type !== 2) { return null; }
            return item;
        });

        self.CustomerScript = ko.computed(function () {
            var type = self.fk_CustomerVideoItemTypeId(),
                item = self.FootageItem();
            if (item === null || type !== 3) { return null; }
            return item;
        });

        self.VoiceOver = ko.computed(function () {
            var type = self.fk_CustomerVideoItemTypeId(),
                item = self.FootageItem();
            if (item === null || type !== 4) { return null; }
            return item;
        });
    };
});
