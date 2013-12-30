/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/storyboard.js" />

"use strict";

define([
        "jquery",
        "knockout",
        "ps/vidyardPlayer",
        "ps/logger",
        "ps/extensions"
],
    function (
        $,
        ko,
        vPlayer,
        logger) {
        function ctor(sectionVm, storyboard) {
            var self = this,
                checkboxes;

            function getCheckboxes(srcElement) {
                return checkboxes || $(srcElement).closest("ul").find("input[type^='checkbox']");
            }

            self.pk_StoryboardId = storyboard.pk_StoryboardId;
            self.fk_StoryboardStatusId = storyboard.fk_StoryboardStatusId;
            self.fk_OrganizationId = storyboard.fk_OrganizationId;
            self.fk_VerticalId = storyboard.fk_VerticalId;
            self.fk_AudioScriptTemplateId = storyboard.fk_AudioScriptTemplateId;
            self.Name = storyboard.Name;
            self.Description = storyboard.Description;
            self.VidyardId = storyboard.VidyardId;

            self.ThumbnailUrl = ko.computed(function () {
                var vId = self.VidyardId();
                if (vId === null) {
                    return null;
                }
                return "//embed.vidyard.com/embed/{0}/thumbnail.jpg".format(vId);
            });

            self.IsSelected = ko.observable(false);

            self.AttrBinding = ko.computed(function () {
                var selected = self.IsSelected(),
                    id = self.pk_StoryboardId();

                var attr = {
                    id: 'sb_' + id
                };
                if (selected) {
                    attr.checked = "checked";
                }
                return attr;
            });

            self.ToggleSelection = function (data, event) {
                var selected = !self.IsSelected();
                self.IsSelected(selected);

                if (selected) {
                    // Hack to work with JCF
                    getCheckboxes(event.srcElement).each(function () {
                        this.checked = this === event.srcElement ? selected : false;
                    });
                }

                sectionVm.SelectStoryboard(self);
                $.jcfModule.customForms.refreshAll();
            };

            self.Player = null;

            self.LoadPlayer = function () {
                self.Player = new vPlayer({ VideoId: self.VidyardId() });
            };
            self.PlayLightbox = function (d, e) {
                if (!self.Player && self.VidyardId()) {
                    self.LoadPlayer();
                }
                if (self.Player) {
                    self.Player.ShowLightbox();
                }
                e.stopImmediatePropagation();
            };
        }

        return ctor;
    });