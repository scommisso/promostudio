/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/voiceActor.js" />

"use strict";

define([
        "jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/common",
        "ps/extensions"
],
    function (
        $,
        ko,
        strings,
        enums,
        logger,
        common) {
        function ctor(sectionVm, voiceActor) {
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

            self.pk_VoiceActorId = voiceActor.pk_VoiceActorId;
            self.FullName = voiceActor.FullName;
            self.Description = voiceActor.Description;
            self.PhotoUrl = ko.computed(function () {
                var id = voiceActor.pk_VoiceActorId();
                return "/Resources/ActorPhoto?voiceActorId={0}".format(id);
            });
            self.PhotoBackground = ko.computed(function () {
                var url = self.PhotoUrl();
                if (url === null) { return "none"; }
                return "url({0})".format(url);
            });

            self.IsSelected = ko.observable(false);
            self.IsPlaying = ko.observable(false);

            self.AttrBinding = ko.computed(function () {
                var selected = self.IsSelected(),
                    id = self.pk_VoiceActorId();

                var attr = {
                    id: 'va_' + id
                };
                if (selected) {
                    attr.checked = "checked";
                }
                return attr;
            });

            self.Play = function (data, event) {
                var playing = self.IsPlaying(),
                    srcElement = common.getSourceElement(event);
                getPlayers(srcElement).each(function () {
                    var $this = $(this);
                    $this.jPlayer("pause", 0);
                });
                if (!playing) {
                    $(srcElement).closest("li").find("div.jp-jplayer").jPlayer("play");
                    self.IsPlaying(true);
                }
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

                sectionVm.SelectActor(self);
                $.jcfModule.customForms.refreshAll();
            };
        }

        return ctor;
    });