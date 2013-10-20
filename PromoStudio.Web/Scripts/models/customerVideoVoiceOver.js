/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["models/voiceActor",
        "knockout"
    ],
    function (
        voiceActor,
        ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerVideoVoiceOverId = ko.observable(data.pk_CustomerVideoVoiceOverId || null);
        self.fk_CustomerVideoId = ko.observable(data.fk_CustomerVideoId || null);
        self.fk_VoiceActorId = ko.observable(data.fk_VoiceActorId || null);
        self.Script = ko.observable(data.Script || null);
        self.FilePath = ko.observable(data.FilePath || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
        self.DateUploaded = ko.observable(data.DateUploaded || null);

        self.VoiceActor = ko.observable(null);
        
        function loadData() {
            if (data.VoiceActor) {
                self.VoiceActor(new VoiceActor(data.VoiceActor));
            }
        }

        loadData();
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.DateCreated;
        delete copy.DateUpdated;
        delete copy.DateUploaded;
        delete copy.VoiceActor;

        return copy;
    };
    return ctor;
});
