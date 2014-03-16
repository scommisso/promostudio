/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/customerResource.js" />

"use strict";

define([
        "jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "ps/extensions"
],
    function (
        $,
        ko,
        strings,
        enums,
        logger) {
        function ctor(sectionVm, customerResource) {
            var self = this,
                checkboxes;

            function getCheckboxes(srcElement) {
                return checkboxes || $(srcElement).closest("div.tab-content").find("input[type^='checkbox']");
            }

            self.pk_CustomerResourceId = customerResource.pk_CustomerResourceId;
            self.fk_CustomerId = customerResource.fk_CustomerId;
            self.fk_OrganizationId = customerResource.fk_OrganizationId;
            self.LinkFileName = customerResource.LinkFileName;
            self.LinkUrl = customerResource.LinkUrl;
            self.IsCustomerResource = customerResource.IsCustomerResource;
            self.IsOrganizationResource = customerResource.IsOrganizationResource;

            self.IsSelected = ko.observable(false);

            self.AttrBinding = ko.computed(function () {
                var selected = self.IsSelected(),
                    id = self.pk_CustomerResourceId();

                var attr = {
                    id: 'cr_' + id
                };
                if (selected) {
                    attr.checked = "checked";
                }
                return attr;
            });

            self.ToggleSelection = function (data, event) {
                var selected = !self.IsSelected();
                self.IsSelected(selected);

                if (selected) {
                    // Hack to work with JCF
                    getCheckboxes(event.srcElement).each(function () {
                        this.checked = this === event.srcElement ? selected : false;
                    });
                }

                sectionVm.SelectPhoto(self);
                $.jcfModule.customForms.refreshAll();
            };
        }

        return ctor;
    });