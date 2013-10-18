/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="storyboardItem.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../ps/vidyardPlayer.js" />

define(["models/storyboardItem",
        "ps/vidyardPlayer",
        "knockout",
        "ps/extensions"],
    function (
        storyboardItem,
        vPlayer,
        ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_StoryboardId = ko.observable(data.pk_StoryboardId || null);
        self.fk_StoryboardStatusId = ko.observable(data.fk_StoryboardStatusId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.fk_VerticalId = ko.observable(data.fk_VerticalId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);
        self.VidyardId = ko.observable(data.VidyardId || null);
        self.ThumbnailUrl = ko.computed(function() {
            var vId = self.VidyardId();
            return "//embed.vidyard.com/embed/{0}/thumbnail.jpg".format(vId);
        });

        self.Items = ko.observableArray([]);

        self.Player = null;
        self.LoadPlayer = function () {
            self.Player = new vPlayer({ VideoId: self.VidyardId() });
        };
        self.PlayLightbox = function(d, e) {
            if (self.Player) {
                self.Player.ShowLightbox();
            }
            e.stopImmediatePropagation();
        };

        self.LoadItems = function (items) {
            var i, item;
            items = items || [];

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new storyboardItem(self, item);
            }

            self.Items(items);
        };
        self.LoadItems(data.Items);
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.ThumbnailUrl;
        delete copy.Player;

        return copy;
    };
    return ctor;
});
