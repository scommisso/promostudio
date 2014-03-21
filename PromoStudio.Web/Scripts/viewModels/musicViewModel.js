/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/stockAudio.js" />

"use strict";

define([
        "jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/common",
        "ps/extensions",
        "lib/ko.soundjs"
],
    function (
        $,
        ko,
        strings,
        enums,
        logger,
        common) {
        function ctor(sectionVm, stockAudio) {
            var self = this,
                slider,
                checkboxes,
                players;

            function getSlider(srcElement) {
                return slider || $(srcElement).closest("div.slide");
            }

            function getCheckboxes(srcElement) {
                return checkboxes || getSlider(srcElement).find("input[type^='checkbox']");
            }

            function getPlayers(srcElement) {
                return players || getSlider(srcElement).find("div.jp-jplayer");
            }

            self.pk_StockAudioId = stockAudio.pk_StockAudioId;
            self.Name = stockAudio.Name;
            self.Description = stockAudio.Description;
            self.FilePath = stockAudio.FilePath;

            self.IsSelected = ko.observable(false);
            self.IsPlaying = ko.observable(false);

            self.AttrBinding = ko.computed(function () {
                var selected = self.IsSelected(),
                    id = self.pk_StockAudioId();

                var attr = {
                    id: 'sa_' + id
                };
                if (selected) {
                    attr.checked = "checked";
                }
                return attr;
            });

            self.MediaSource = ko.computed({
                read: function () {
                    var filePath = self.FilePath(),
                        extension = filePath.split('.').pop(),
                        source = {};
                    source[extension] = filePath;
                    return source;
                },
                deferEvaluation: true
            });

            self.Play = function (data, event) {
                self.IsPlaying(!self.IsPlaying());
            };

            self.ToggleSelection = function (data, event) {
                var selected = !self.IsSelected(),
                    srcElement = common.getSourceElement(event);
                self.IsSelected(selected);

                if (selected) {
                    // Hack to work with JCF
                    getCheckboxes(srcElement).each(function () {
                        this.checked = this === srcElement ? selected : false;
                    });
                }

                sectionVm.SelectMusic(self);
                $.jcfModule.customForms.refreshAll();
            };
        }

        return ctor;
    });