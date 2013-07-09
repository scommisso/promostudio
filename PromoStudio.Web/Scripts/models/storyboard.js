/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/storyboardItem", "knockout"], function (storyboardItem, ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.pk_StoryboardId = ko.observable(data.pk_StoryboardId || null);
        self.fk_StoryboardStatusId = ko.observable(data.fk_StoryboardStatusId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.fk_VerticalId = ko.observable(data.fk_VerticalId || null);
        self.Name = ko.observable(data.Name || null);
        self.Description = ko.observable(data.Description || null);

        self.Items = ko.observableArray([]);

        self.LoadItems = function (items) {
            var i, item;
            items = items || [];

            for (i = 0; i < items.length; i++) {
                item = items[i];
                items[i] = new storyboardItem(item);
            }

            self.Items(items);
        };
        self.LoadItems(data.Items);
    };
});
