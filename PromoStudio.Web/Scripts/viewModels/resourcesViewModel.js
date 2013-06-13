/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define(["models/customer",
        "models/customerResource",
        "models/enums",
        "jquery",
        "knockout"], function (
            customer,
            customerResource,
            enums,
            $,
            ko) {
    return function (data) {
        var self = this;
        data = data || {};

        self.Customer = ko.observable(null);
        self.CustomerResources = ko.observableArray([]);

        self.LoadItems = function (customerData, resources) {
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

        self.CategoryList = buildCategoryList();
        self.SelectedCategory = ko.observable(self.CategoryList[0]);

        self.FormAction = ko.computed(function () {
            var cat = self.SelectedCategory();
            cat = cat ? cat.Value : 1;
            return "/Resources/Upload?category=" + cat;
        });
        

        self.LoadItems(data.Customer, data.CustomerResources);

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
    };
});