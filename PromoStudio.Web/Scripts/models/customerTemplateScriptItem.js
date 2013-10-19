/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define([
        "models/customerResource",
        "models/customerTemplateScript",
        "models/templateScriptItem",
        "knockout"
], function (
    customerResource,
    customerTemplateScript,
    templateScriptItem,
    ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerTemplateScriptItemId = ko.observable(data.pk_CustomerTemplateScriptItemId || null);
        self.fk_CustomerTemplateScriptId = ko.observable(data.fk_CustomerTemplateScriptId || null);
        self.fk_TemplateScriptItemId = ko.observable(data.fk_TemplateScriptItemId || null);
        self.fk_CustomerResourceId = ko.observable(data.fk_CustomerResourceId || null);
        
        self.CustomerScript = ko.observable(null);
        self.ScriptItem = ko.observable(null);
        self.Resource = ko.observable(null);

        self.Value = ko.observable(null);
        self.Value.subscribe(function (newVal) {
            var scriptItem = self.ScriptItem(),
                type, res, resId;
            if (scriptItem === null) { return; }
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

        self.LoadTemplateData = function (csi) {
            self.ScriptItem(csi);
            self.fk_TemplateScriptItemId(ko.utils.unwrapObservable(csi.pk_TemplateScriptItemId));
        };
        
        function loadData(customerScriptData, scriptItemData, resourceData) {
            if (customerScriptData) {
                if (ko.isObservable(customerScriptData.pk_CustomerTemplateScriptId)) {
                    self.CustomerScript(customerScriptData);
                } else {
                    self.CustomerScript(new customerTemplateScript(customerScriptData));
                }
            }
            if (scriptItemData) {
                if (ko.isObservable(scriptItemData.pk_TemplateScriptItemId)) {
                    self.ScriptItem(scriptItemData);
                } else {
                    self.ScriptItem(new templateScriptItem(scriptItemData));
                }
            }
            if (resourceData) {
                if (ko.isObservable(resourceData.pk_CustomerResourceId)) {
                    self.Resource(resourceData);
                } else {
                    self.Resource(new customerResource(resourceData));
                }
            }
        }

        loadData(data.CustomerScript, data.ScriptItem, data.Resource);
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.CustomerScript;
        delete copy.ScriptItem;
        delete copy.Value;

        return copy;
    };
    return ctor;
});