/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["models/customer",
        "models/customerResource",
        "models/enums",
        "ps/logger",
        "jquery",
        "knockout",
        "lib/ko.custom",
        "form"], function (
            customer,
            customerResource,
            enums,
            logger,
            $,
            ko) {
    return function () {
        var self = this;

        self.Customer = ko.observable(null);
        self.CustomerResources = ko.observableArray([]);

        self.CategoryList = buildCategoryList();
        self.SelectedCategory = ko.observable(self.CategoryList[0]);

        self.FormAction = ko.computed(function () {
            var cat = self.SelectedCategory();
            cat = cat ? cat.Value : 1;
            return "/Resources/Upload?category=" + cat;
        });

        self.LoadingData = ko.observable(false);
        self.LoadingFile = ko.observable(false);
        self.LoadingFilePercentage = ko.observable(0);
        self.FileLoadSuccess = ko.observable(false);
        self.FileLoadError = ko.observable(false);

        function loadItems(customerData, resources) {
            var i, item;
            resources = resources || [];

            for (i = 0; i < resources.length; i++) {
                item = resources[i];
                resources[i] = new customerResource(item);
            }

            if (customerData !== null) {
                self.Customer(new customer(customerData));
            }
            self.CustomerResources(resources);
        };

        function buildCategoryList() {
            var items = enums.templateScriptItemCategory(),
                i;
            items = items.slice(1, items.length);
            for (i = 0; i < items.length; i++) {
                items[i] = {
                    Value: i + 1,
                    Text: items[i]
                };
            }
            return items;
        };

        function loadData(callback) {
            self.LoadingData(true);
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Resources/Data",
                success: function (data, textStatus, jqXHR) {
                    loadItems(data.Customer, data.CustomerResources);
                    self.LoadingData(false);
                    if (typeof (callback) === "function") {
                        callback();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    self.LoadingData(false);
                    logger.log("Error retrieving data: " + textStatus);
                    logger.log(errorThrown);
                }
            });
        };

        self.pageLoaded = function () {
            loadData(function () {
                var form = $("form");
                form.ajaxForm({
                    beforeSubmit: function (arr, $form, options) {
                        self.LoadingFilePercentage(0);
                        self.LoadingFile(true);
                        self.FileLoadSuccess(false);
                        self.FileLoadError(false);
                    },
                    success: function (responseText, statusText, xhr, form) {
                        form.clearForm();
                        self.LoadingFile(false);
                        if (xhr.status === 200) {
                            self.FileLoadSuccess(true);
                            loadData();
                        }
                        else {
                            self.FileLoadError(true);
                        }
                    },
                    uploadProgress: function (event, position, total, percentComplete) {
                        self.LoadingFilePercentage(percentComplete);
                    }
                });
            });
        };
    };
});


