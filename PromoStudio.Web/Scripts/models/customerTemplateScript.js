/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/customerTemplateScriptItem", "knockout"], function (customerTemplateScriptItem, ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerTemplateScriptId = ko.observable(data.pk_CustomerTemplateScriptId || null);
        self.fk_CustomerId = ko.observable(data.fk_CustomerId || null);
        self.fk_TemplateScriptId = ko.observable(data.fk_TemplateScriptId || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
        self.DateCompleted = ko.observable(data.DateCompleted || null);
        self.PreviewFilePath = ko.observable(data.PreviewFilePath || null);
        self.CompletedFilePath = ko.observable(data.CompletedFilePath || null);
        
        self.Template = ko.observable(data.Template || null);
        self.Items = ko.observableArray([]);

        self.LoadTemplateData = function (templateScript) {
            var items = templateScript.Items(),
                item, i;
            self.Template(templateScript);
            self.fk_TemplateScriptId(ko.utils.unwrapObservable(templateScript.pk_TemplateScriptId));
            for (i = 0; i < items.length; i++) {
                item = new customerTemplateScriptItem();
                item.LoadTemplateData(items[i]);
                items[i] = item;
            }
            self.Items(items);
        };

        self.LoadItems = function (items) {
            var i, item;
            items = items || [];

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new customerTemplateScriptItem(item);
            }

            self.Items(items);
        };
        self.LoadItems(data.Items);
    };
});