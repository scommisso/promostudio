/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["knockout"], function (ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_CustomerId = ko.observable(data.pk_CustomerId || null);
        self.fk_OrganizationId = ko.observable(data.fk_OrganizationId || null);
        self.fk_VerticalId = ko.observable(data.fk_VerticalId || null);
        self.FullName = ko.observable(data.FullName || null);
        self.EmailAddress = ko.observable(data.EmailAddress || null);
        self.DateCreated = ko.observable(data.DateCreated || null);
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.DateCreated;

        return copy;
    };
    return ctor;
});
