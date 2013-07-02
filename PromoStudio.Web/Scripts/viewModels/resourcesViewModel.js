/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/customer",
        "models/customerResource",
        "models/enums",
        "jquery",
        "knockout",
        "form"], function (
            customer,
            customerResource,
            enums,
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
        }

        self.pageLoaded = function () {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/Resources/Data",
                success: function (data, textStatus, jqXHR) {
                    loadItems(data.Customer, data.CustomerResources);
                    $('form').ajaxForm({
                        beforeSubmit: function (arr, $form, options) {
                            // TODO: Submitting                
                        },
                        success: function (responseText, statusText, xhr, form) {
                            // TODO: Hide submission
                            $('form input').val("");
                        },
                        uploadProgress: function (event, position, total, percentComplete) {
                            // TODO: Show progress
                        }
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error retrieving data: " + textStatus);
                    console.log(errorThrown);
                }
            });
        };
    };
});


