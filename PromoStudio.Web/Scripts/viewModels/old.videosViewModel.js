/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["models/customer",
        "models/customerVideo",
        "ps/logger",
        "jquery",
        "knockout"], function (
            customer,
            customerVideo,
            logger,
            $,
            ko) {
    return function (data) {
        var self = this;

        self.Customer = ko.observable(null);
        self.CustomerVideos = ko.observableArray([]);
        self.LoadingData = ko.observable(false);

        function loadItems(customerData, videos) {
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

        function updateStatus() {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Videos/Status",
                success: function (data, textStatus, jqXHR) {
                    loadItems(null, data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    logger.log("Error retrieving video status: " + textStatus);
                    logger.log(errorThrown);
                }
            });
        };

        self.pageLoaded = function () {
            self.LoadingData(true);
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Videos/Data",
                success: function (data, textStatus, jqXHR) {
                    loadItems(data.Customer, data.CustomerVideos);
                    self.LoadingData(false);
                    var interval = window.setInterval(function () {
                        updateStatus();
                    }, 5000);
                    $.fn.pjaxScaffold.clearIntervals = function () {
                        window.clearInterval(interval);
                    };
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    logger.log("Error retrieving data: " + textStatus);
                    logger.log(errorThrown);
                }
            });
        };
    };
});