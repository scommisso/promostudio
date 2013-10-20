/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/customer.js" />
/// <reference path="../models/customerVideo.js" />

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
                url: "/Videos/Status"
            }).done(function(vidData) {
                loadItems(null, vidData);
            }).error(function(jqXhr, textStatus, errorThrown) {
                logger.log("Error retrieving video status: " + textStatus);
                logger.log(errorThrown);
            });

        };

        self.pageLoaded = function () {
            self.LoadingData(true);
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Videos/Data"
            }).done(function(vidData) {
                loadItems(vidData.Customer, vidData.CustomerVideos);
                self.LoadingData(false);
                var interval = window.setInterval(function() {
                    updateStatus();
                }, 5000);
                if ($.pjax) {
                    $.fn.pjaxScaffold.clearIntervals = function() {
                        window.clearInterval(interval);
                    };
                }
            }).error(function(jqXhr, textStatus, errorThrown) {
                logger.log("Error retrieving data: " + textStatus);
                logger.log(errorThrown);
            });
        };
    };
});