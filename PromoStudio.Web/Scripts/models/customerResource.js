/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="enums.js" />

define(["models/enums", "knockout"], function (enums, ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerResourceId = ko.observable(data.pk_CustomerResourceId || null);
        self.fk_CustomerId = ko.observable(data.fk_CustomerId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.fk_TemplateScriptItemTypeId = ko.observable(data.fk_TemplateScriptItemTypeId || null);
        self.fk_TemplateScriptItemCategoryId = ko.observable(data.fk_TemplateScriptItemCategoryId || null);
        self.fk_CustomerResourceStatusId = ko.observable(data.fk_CustomerResourceStatusId || null);
        self.Value = ko.observable(data.Value || null);

        self.IsCustomerResource = ko.computed(function() {
            return (self.fk_CustomerId() > 0);
        });
        self.IsOrganizationResource = ko.computed(function () {
            return (self.fk_OrganizationId() > 0);
        });

        self.TemplateScriptItemCategory = ko.computed(function () {
            var id = self.fk_TemplateScriptItemCategoryId();
            return enums.templateScriptItemCategory(id);
        });

        self.TemplateScriptItemType = ko.computed(function () {
            var id = self.fk_TemplateScriptItemTypeId();
            return enums.templateScriptItemType(id);
        });

        self.LinkUrl = ko.computed(function () {
            // TODO: This should point to our content host (Amazon S3) instead of local
            var type = self.TemplateScriptItemType();
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

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.IsCustomerResource;
        delete copy.IsOrganizationResource;
        delete copy.TemplateScriptItemCategory;
        delete copy.TemplateScriptItemType;
        delete copy.LinkUrl;
        delete copy.LinkFileName;

        return copy;
    };
    return ctor;
});
