/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/customer",
        "models/customerVideo",
        "jquery",
        "knockout"], function (
            customer,
            customerVideo,
            $,
            ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.Customer = ko.observable(null);
        self.CustomerVideos = ko.observableArray([]);

        self.LoadItems = function (customerData, videos) {
            var i, item;
            videos = videos || [];

            for (i = 0; i < videos.length; i++) {
                item = videos[i];
                videos[i] = new customerVideo(item);
            }

            if (customerData !== null) {
                self.Customer(new customer(customerData));
            }
            self.CustomerVideos(videos);
        };

        self.UpdateStatus = function () {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Videos/Status",
                success: function (data, textStatus, jqXHR) {
                    self.LoadItems(null, data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error retrieving video status: " + textStatus);
                    console.log(errorThrown);
                }
            });
        };

        self.LoadItems(data.Customer, data.CustomerVideos);
    };
});