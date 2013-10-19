﻿/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../lib/bootstrap.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/customerResource.js" />
/// <reference path="photoSlotViewModel.js" />

define(["models/customerResource",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "bootstrap"
    ],
    function (
        customerResource,
        ko,
        strings,
        enums,
        logger) {
        return function (data) {
            var self = this,
                photoSlot = data.Slot,
                $elem = data.Element,
                saveCallback = data.OnSave,
                cancelCallback = data.OnCancel;

            self.IsLoading = ko.observable(true);
            self.CustomerPhotos = ko.observableArray([]);
            self.OrganizationPhotos = ko.observableArray([]);

            self.Show = function () {
                // TODO: Move the modal contents into a template
                ko.cleanNode($elem[0]);
                ko.applyBindings(self, $elem[0]);
                $elem.modal("show");
                getPhotos();
            };

            self.Hide = function() {
                $elem.modal("hide");
            };

            self.Cancel = function () {
                logger.log("Canceling");
                self.Hide();
                cancelCallback();
            };

            self.Save = function () {
                logger.log("Saving");
                self.Hide();
                saveCallback();
            };
            
            function loadPhotos(photoResources) {
                var custPhotos = [],
                    orgPhotos = [],
                    i, photo;
                for (i = 0; i < photoResources.length; i++) {
                    photo = new customerResource(photoResources[i]);
                    if (photo.IsCustomerResource()) {
                        custPhotos.push(photo);
                    }
                    else if (photo.IsOrganizationResource()) {
                        orgPhotos.push(photo);
                    }
                }
                self.CustomerPhotos(custPhotos);
                self.OrganizationPhotos(orgPhotos);
            }
            
            function getPhotos() {
                self.IsLoading(true);
                $.ajax({
                    type: "GET",
                    cache: false,
                    dataType: "json",
                    url: "/Resources/Data?typeId=1"
                })
                    .done(function (photoData) {
                        loadPhotos(photoData.CustomerResources);
                        self.LoadingData(false);
                    })
                    .error(function (jqHxr, textStatus, errorThrown) {
                        logger.log("Error retrieving photo data: " + textStatus);
                        logger.log(errorThrown);
                    })
                    .always(function () {
                        self.LoadingData(false);
                    });
            }
        };
    });