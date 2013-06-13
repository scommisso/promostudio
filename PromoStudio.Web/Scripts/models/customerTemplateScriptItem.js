/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/customerResource", "knockout"], function (customerResource, ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerTemplateScriptItemId = ko.observable(data.pk_CustomerTemplateScriptItemId || null);
        self.fk_CustomerTemplateScriptId = ko.observable(data.fk_CustomerTemplateScriptId || null);
        self.fk_TemplateScriptItemId = ko.observable(data.fk_TemplateScriptItemId || null);
        self.fk_CustomerResourceId = ko.observable(data.fk_CustomerResourceId || null);
        
        self.CustomerScript = ko.observable(data.CustomerScript || null);
        self.ScriptItem = ko.observable(data.ScriptItem || null);
        self.Resource = ko.observable(data.Resource || null);

        self.Value = ko.observable(null);
        self.Value.subscribe(function (newVal) {
            var scriptItem = self.ScriptItem(),
                type, res, resId;
            if (scriptItem === null) { return null; }
            type = ko.utils.unwrapObservable(scriptItem.fk_TemplateScriptItemTypeId);
            if (type === 4) {
                // Text requires creating a new customer resource
                res = new customerResource({
                    fk_TemplateScriptItemTypeId: scriptItem.fk_TemplateScriptItemTypeId(),
                    fk_TemplateScriptItemCategoryId: scriptItem.fk_TemplateScriptItemCategoryId(),
                    fk_CustomerResourceStatusId: 1, // active
                    Value: newVal                    
                });
                self.Resource(res);
            }
            else {
                // Others require Ids
                resId = parseInt(newVal, 10);
                self.fk_CustomerResourceId(resId);
            }
        });

        self.LoadTemplateData = function (templateScriptItem) {
            self.ScriptItem(templateScriptItem);
            self.fk_TemplateScriptItemId(ko.utils.unwrapObservable(templateScriptItem.pk_TemplateScriptItemId));
        };
    };
});