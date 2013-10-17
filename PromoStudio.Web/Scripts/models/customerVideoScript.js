/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["knockout"], function (ko) {
    return function (data) {
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
            if (template === null) {
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
    };
});
