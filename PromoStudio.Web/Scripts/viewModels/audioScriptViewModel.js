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
                i, script;

            for (i = 0; i < scripts.length; i++) {
                script = new audioScriptTemplate(scripts[i]);
                scripts[i] = new audioScriptTemplateViewModel(script);
            }

            self.CustomerScript(new customerVideoScript());
            self.Scripts(scripts);
        }

        var self = this;
        data = data || {};

        var isStepCompleted = null,
            video = null;

        self.Scripts = ko.observableArray([]);
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
            // TODO: Mark completed when all audio placeholders are filled in
            if (ko.isObservable(isStepCompleted)) {
                isStepCompleted(true);
            }
            return true;
        });

        loadData();
    }

    return ctor;
});