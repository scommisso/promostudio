/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/stockAudio.js" />

"use strict";

define([
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/extensions"
],
    function (
        $,
        ko,
        strings,
        enums,
        logger) {
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

            self.Play = function (data, event) {
                var playing = self.IsPlaying();
                getPlayers(event.srcElement).each(function () {
                    var $this = $(this);
                    $this.jPlayer("pause", 0);
                });
                if (!playing) {
                    $(event.srcElement).closest("li").find("div.jp-jplayer").jPlayer("play");
                    self.IsPlaying(true);
                }
            };

            self.ToggleSelection = function (data, event) {
                var selected = !self.IsSelected();
                self.IsSelected(selected);

                if (selected) {
                    // Hack to work with JCF
                    getCheckboxes(event.srcElement).each(function () {
                        this.checked = this === event.srcElement ? selected : false;
                    });
                }

                sectionVm.SelectMusic(self);
                $.jcfModule.customForms.refreshAll();
            };
        }

        return ctor;
    });