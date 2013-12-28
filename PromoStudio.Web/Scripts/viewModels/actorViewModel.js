/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/voiceActor.js" />

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
                if (sectionVm.toggling === true) { return true; }

                var selected = !self.IsSelected();
                if (selected) {
                    // Hack to work with JCF
                    sectionVm.toggling = true;
                    getCheckboxes(event.srcElement).each(function () {
                        if (this !== event.srcElement) {
                            this.checked = true;
                            this.removeAttribute("checked");
                            $(this).trigger("click");
                        }
                    });
                    sectionVm.toggling = false;
                }

                sectionVm.SelectActor(self);
            };
        }

        return ctor;
    });