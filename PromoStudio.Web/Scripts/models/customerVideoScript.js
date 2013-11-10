/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

"use strict";

define(["knockout"], function (ko) {
    function ctor (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerVideoScriptId = ko.observable(data.pk_CustomerVideoScriptId || null);
        self.fk_CustomerVideoId = ko.observable(data.fk_CustomerVideoId || null);
        self.fk_AudioScriptTemplateId = ko.observable(data.fk_AudioScriptTemplateId || 1); // pending
        self.ReplacementData = ko.observable(data.ReplacementData || null);

        self.AudioScriptTemplate = ko.observable(null);
        self.ReplacementItems = ko.observableArray([]);
        self.AudioScriptTemplate.subscribe(function (newValue) {
            var template = self.AudioScriptTemplate(),
                variables, i;
            if (template) {
                self.ReplacementItems([]);
            }
            else {
                variables = template.getScriptVariables();
                for (i = 0; i < variables.length; i++) {
                    variables[i] = {
                        key: variables[i],
                        value: null
                    };
                }
                self.ReplacementItems(variables);
            }
        });
    }

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.AudioScriptTemplate;

        return copy;
    };
    return ctor;
});
