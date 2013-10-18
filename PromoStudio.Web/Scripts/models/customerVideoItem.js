/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define([
        "models/stockVideo",
        "models/stockAudio",
        "models/customerTemplateScript",
        "knockout"
    ],
    function (
        stockVideo,
        stockAudio,
        customerTemplateScript,
        ko) {
    var ctor = function (data) {
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
        
        function loadData() {
            if (data.StockVideo) {
                self.fk_CustomerVideoItemTypeId(1);
                self.FootageItem(new stockVideo(data.StockVideo));
            }
            else if (data.StockAudio) {
                self.fk_CustomerVideoItemTypeId(2);
                self.FootageItem(new stockAudio(data.StockAudio));
            }
            else if (data.CustomerScript) {
                self.fk_CustomerVideoItemTypeId(3);
                self.FootageItem(new customerTemplateScript(data.CustomerScript));
            }
            //else if (data.VoiceOver) {
            //    self.fk_CustomerVideoItemTypeId(4);
            //    self.FootageItem(data.VoiceOver);
            //}
        }

        loadData();
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.FootageItem;
        delete copy.FootageItemName;

        return copy;
    };
    return ctor;
});
