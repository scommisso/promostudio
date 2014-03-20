/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.11.0.intellisense.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../models/audioScriptTemplate.js" />
/// <reference path="../ps/logger.js" />

"use strict";

define([
    "models/audioScriptTemplate",
    "models/customerVideoScript",
    "viewModels/audioScriptTemplateViewModel",
    "jqueryui",
    "knockout",
    "strings",
    "models/enums",
    "ps/logger",
    "lib/ko.custom"
], function (
        audioScriptTemplate,
        customerVideoScript,
        audioScriptTemplateViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
    function ctor(data) {

        var self = this,
            isStepCompleted = null,
            video = null,
            sectionLength = 6;
        data = data || {};

        function stepChanging(navVm, callback) {
            var customerVideo = video(),
                script = self.CustomerScript();
            customerVideo.Script(script);
            callback();
        }

        function loadVideoData(videoData) {
            var script = videoData.Script(),
                scripts = self.Scripts(),
                templateId, i, item;
            if (script) {
                templateId = script.fk_AudioScriptTemplateId();
                for (i = 0; i < scripts.length; i++) {
                    item = scripts[i];
                    if (item.pk_AudioScriptTemplateId() === templateId) {
                        self.SelectedScript(item);
                        break;
                    }
                }
            }
        }

        function loadData() {
            var scripts = data.Scripts || [],
                scriptSections = [],
                i, script;

            for (i = 0; i < scripts.length; i++) {
                script = new audioScriptTemplate(scripts[i]);
                script = new audioScriptTemplateViewModel(script, i);
                scripts[i] = script;
                if (!scriptSections.length ||
                    (scriptSections[scriptSections.length - 1].Scripts().length % sectionLength === 0)) {
                    scriptSections.push({
                        Scripts: ko.observableArray([])
                    });
                }
                scriptSections[scriptSections.length - 1].Scripts.push(script);
            }

            self.CustomerScript(new customerVideoScript());
            self.Scripts(scripts);
            self.ScriptSections(scriptSections);

            $.initJcf.tabs(); //refresh tab UI
        }

        self.Scripts = ko.observableArray([]);
        self.ScriptSections = ko.observableArray([]);
        self.CustomerScript = ko.observable(null);
        self.SelectedScript = ko.observable(null);
        self.SelectedScriptTemplateId = ko.computed(function () {
            var script = self.SelectedScript();
            if (!script) {
                if (isStepCompleted) {
                    isStepCompleted(false);
                }
                return null;
            }
            return script.pk_AudioScriptTemplateId();
        });

        self.IsSelected = function (script) {
            return self.SelectedScript() === script;
        };

        self.SelectScript = function (script) {
            var i, scripts = self.Scripts();
            for (i = 0; i < scripts.length; i++) {
                if (scripts[i] !== script && scripts[i].IsSelected()) {
                    scripts[i].IsSelected(false);
                }
            }
            if (self.IsSelected(script)) {
                script.IsSelected(false);
                self.SelectedScript(null);
            } else {
                script.IsSelected(true);
                self.SelectedScript(script);
            }
        };

        self.Bind = function (selector, navSelector) {
            ko.applyBindings(self, $(selector)[0]);
            ko.callbackOnBind($(navSelector)[0], function (navVm) {
                isStepCompleted = navVm.IsStepCompleted;
                video = navVm.Video;
                navVm.BeforeStepChange = stepChanging;

                loadVideoData(video());

                self.IsCompleted(); // check completed status
            }, 1000);
        };

        self.IsCompleted = ko.computed(function () {
            var isCompleted = true,//false,
                script = self.SelectedScript();
            
            if (script) {
                // TODO: Mark completed when all placeholders are filled in
            }

            if (ko.isObservable(isStepCompleted)) {
                isStepCompleted(isCompleted);
            }
            return isCompleted;
        });

        loadData();
    }

    return ctor;
});