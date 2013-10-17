/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["models/templateScriptItem", "knockout"], function (templateScriptItem, ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_TemplateScriptItemId = ko.observable(data.pk_TemplateScriptItemId || null);
        self.fk_TemplateScriptId = ko.observable(data.fk_TemplateScriptId || null);
        self.fk_TemplateScriptItemTypeId = ko.observable(data.fk_TemplateScriptItemTypeId || null);
        self.fk_TemplateScriptItemCategoryId = ko.observable(data.fk_TemplateScriptItemCategoryId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.ItemWidth = ko.observable(data.ItemWidth || null);
        self.ItemHeight = ko.observable(data.ItemHeight || null);

        self.Value = ko.observable(null);
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.Value;

        return copy;
    };
    return ctor;
});