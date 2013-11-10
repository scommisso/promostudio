/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

"use strict";

define(["knockout"], function (ko) {
    function ctor (data) {
        var self = this;
        data = data || {};

        self.pk_VoiceActorId = ko.observable(data.pk_VoiceActorId || null);
        self.fk_VoiceActorStatusId = ko.observable(data.fk_VoiceActorStatusId || null);
        self.UserName = ko.observable(data.UserName || null);
        self.FullName = ko.observable(data.FullName || null);
        self.Description = ko.observable(data.Description || null);
        self.SampleFilePath = ko.observable(data.SampleFilePath || null);
        self.PhotoFilePath = ko.observable(data.PhotoFilePath || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
    }

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.SampleFilePath;
        delete copy.PhotoFilePath;
        delete copy.DateCreated;
        delete copy.DateUpdated;

        return copy;
    };
    return ctor;
});
