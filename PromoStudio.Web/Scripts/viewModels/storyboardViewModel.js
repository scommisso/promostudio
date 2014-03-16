/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/storyboard.js" />

"use strict";

define([
        "jqueryui",
        "knockout",
        "ps/logger",
        "strings",
        "ps/extensions"
],
    function (
        $,
        ko,
        logger,
        strings) {
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
            self.VimeoVideoId = storyboard.VimeoVideoId;
            self.VimeoThumbnailUrl = storyboard.VimeoThumbnailUrl;

            self.ThumbnailUrl = ko.computed(function () {
                var vUrl = self.VimeoThumbnailUrl();
                if (!vUrl) { return null; }
                return vUrl;
            });

            self.LightboxEmbedCode = ko.computed(function () {
                var vId = self.VimeoVideoId();
                if (!vId) { return null; }
                return strings.getResource("Vimeo__InlineEmbed").format(vId, 360, 640);
            });

            self.IsVisible = ko.observable(false);
            self.IsSelected = ko.observable(false);
            self.IsPlaying = ko.observable(true);
            self.StartPlaying = function () {
                self.IsPlaying(true);
                jQuery.fancybox.center();
            };

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
        }

        return ctor;
    });