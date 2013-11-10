/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/voiceActor.js" />

"use strict";

define([
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/extensions"
],
    function (
        ko,
        strings,
        enums,
        logger) {
        function ctor(voiceActor) {
            var self = this;

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
        }

        return ctor;
    });