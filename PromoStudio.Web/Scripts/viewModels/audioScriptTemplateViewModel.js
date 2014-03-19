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
        function ctor(audioScriptTemplate) {

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
            self.ScriptText = audioScriptTemplate.ScriptText;

            self.GetScriptVariables = function () {
                return getReplacementItems();
            };

            self.IsSelected = ko.observable(false);
        }

        return ctor;
    });