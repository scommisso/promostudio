/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/audioScriptTemplate.js" />

"use strict";

define([
        "knockout",
        "ps/logger"
],
    function (
        ko,
        logger) {
        function ctor(audioScriptTemplate, clientId) {

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

            var self = this,
                replacementRegex = /\[\[([^\]]+)\]\]/g;
            audioScriptTemplate = audioScriptTemplate || {};

            self.pk_AudioScriptTemplateId = audioScriptTemplate.pk_AudioScriptTemplateId;
            self.Name = audioScriptTemplate.Name;
            self.ScriptText = audioScriptTemplate.ScriptText;
            self.ScriptHtml = ko.computed(function() {
                var text = self.ScriptText();
                if (!text || !text.length) { return ""; }

                // TODO: figure out replacements

                return text;
            });

            self.GetScriptVariables = function () {
                return getReplacementItems();
            };

            self.IsSelected = ko.observable(false);
            self.ClientId = ko.observable("tab" + (clientId || 0));
            self.ClientAnchor = ko.computed(function() {
                return "#" + self.ClientId();
            });
        }

        return ctor;
    });