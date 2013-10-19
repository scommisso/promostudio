/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["knockout"], function (ko) {
    var ctor = function (data) {
        var self = this,
            replacementRegex = /\[\[([^\]]+)\]\]/g;
        data = data || {};

        self.pk_AudioScriptTemplateId = ko.observable(data.pk_AudioScriptTemplateId || null);
        self.fk_AudioScriptTemplateStatusId = ko.observable(data.fk_AudioScriptTemplateStatusId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.fk_VerticalId = ko.observable(data.fk_VerticalId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.ScriptText = ko.observable(data.ScriptText || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);

        function getMatches(val, re, i) {
            i = (i || 1);
            var matches = [], match;
            while (match = re.exec(val)) {
                matches.push(match[i]);
            }
            return matches;
        }

        function getReplacementItems() {
            var scriptText = self.ScriptText(),
                matches = getMatches(scriptText, replacementRegex, 1);

            return matches;
        }

        self.getScriptVariables = function () {
            return getReplacementItems();
        };
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.DateCreated;
        delete copy.DateUpdated;

        return copy;
    };
    return ctor;
});