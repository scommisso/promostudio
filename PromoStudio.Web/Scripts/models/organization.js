/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["knockout"], function (ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_OrganizationId = ko.observable(data.pk_OrganizationId || null);
        self.fk_VerticalId = ko.observable(data.fk_VerticalId || null);
        self.Name = ko.observable(data.Name || null);
        self.DisplayName = ko.observable(data.DisplayName || null);
        self.ContactPhone = ko.observable(data.ContactPhone || null);
        self.ContactEmail = ko.observable(data.ContactEmail || null);
        self.Website = ko.observable(data.Website || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
        self.DateUpdated = ko.observable(data.DateUpdated || null);
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.DateCreated;
        delete copy.DateUpdated;

        return copy;
    };
    return ctor;
});
