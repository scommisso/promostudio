/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/customer.js" />

"use strict";

define([
    "models/customer",
    "ps/logger",
    "jqueryui",
    "knockout"
], function (
    customer,
    logger,
    $,
    ko
    ) {
    function ctor(data) {
        var self = this;

        self.Customer = ko.observable(null);
        self.LoadingData = ko.observable(false);

        function loadItems(customerData) {
            if (customerData !== null) {
                self.Customer(new customer(customerData));
            }
        }

        loadItems(data.Customer);
    }

    return ctor;
});