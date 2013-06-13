/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/enums", "knockout"], function (enums, ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerResourceId = ko.observable(data.pk_CustomerResourceId || null);
        self.fk_CustomerId = ko.observable(data.fk_CustomerId || null);
        self.fk_TemplateScriptItemTypeId = ko.observable(data.fk_TemplateScriptItemTypeId || null);
        self.fk_TemplateScriptItemCategoryId = ko.observable(data.fk_TemplateScriptItemCategoryId || null);
        self.fk_CustomerResourceStatusId = ko.observable(data.fk_CustomerResourceStatusId || null);
        self.Value = ko.observable(data.Value || null);

        self.TemplateScriptItemCategory = ko.computed(function () {
            var id = self.fk_TemplateScriptItemCategoryId();
            return enums.templateScriptItemCategory(id);
        });

        self.TemplateScriptItemType = ko.computed(function () {
            var id = self.fk_TemplateScriptItemTypeId();
            return enums.templateScriptItemType(id);
        });

        self.LinkUrl = ko.computed(function () {
            // TODO: This should point to our content host (VidYard) instead of local
            var type = self.TemplateScriptItemType(),
                val = self.Value();
            if (type === "Text") { return "javascript:void(0);"; }
            return "/Resources/Download?crid=" + self.pk_CustomerResourceId();
        });

        self.LinkFileName = ko.computed(function () {
            var type = self.TemplateScriptItemType(),
                displayPath = self.Value(),
                ix;
            if (type === "Text") { return "Text"; }
            ix = displayPath.lastIndexOf("\\");
            if (ix === -1) { ix = displayPath.lastIndexOf("/"); }
            if (ix === -1) { return displayPath; }
            displayPath = displayPath.substring(ix + 1);
            return displayPath;
        });
    };
});
