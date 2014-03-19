/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />

"use strict";

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
    function ctor (data) {

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
        
        var self = this;
        data = data || {};

        self.pk_CustomerTemplateScriptItemId = ko.observable(data.pk_CustomerTemplateScriptItemId || null);
        self.fk_CustomerTemplateScriptId = ko.observable(data.fk_CustomerTemplateScriptId || null);
        self.fk_TemplateScriptItemId = ko.observable(data.fk_TemplateScriptItemId || null);
        self.fk_CustomerResourceId = ko.observable(data.fk_CustomerResourceId || null);
        
        self.CustomerScript = ko.observable(null);
        self.ScriptItem = ko.observable(null);
        self.Resource = ko.observable(null);

        self.LoadTemplateData = function (csi) {
            self.ScriptItem(csi);
            self.fk_TemplateScriptItemId(ko.utils.unwrapObservable(csi.pk_TemplateScriptItemId));
        };

        loadData(data.CustomerScript, data.ScriptItem, data.Resource);
    }

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.CustomerScript;
        delete copy.ScriptItem;

        return copy;
    };
    return ctor;
});