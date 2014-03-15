/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../lib/bootstrap.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/customerResource.js" />
/// <reference path="photoSlotViewModel.js" />

"use strict";

define(["models/customerResource",
        "viewModels/customerResourceViewModel",
        "jqueryui",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger"
],
    function (
        customerResource,
        customerResourceViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
        function ctor(data) {

            function loadPhotos(photoResources) {
                var custPhotos = [],
                    orgPhotos = [],
                    i, photo;
                for (i = 0; i < photoResources.length; i++) {
                    if (photoResources[i].fk_TemplateScriptItemCategoryId !== categoryId) {
                        continue;
                    }
                    photo = new customerResource(photoResources[i]);
                    photo = new customerResourceViewModel(self, photo);
                    if (photo.IsCustomerResource()) {
                        custPhotos.push(photo);
                    }
                    else if (photo.IsOrganizationResource()) {
                        orgPhotos.push(photo);
                    }
                    if (photo.pk_CustomerResourceId() === selectedResourceId) {
                        self.SelectedPhoto(photo);
                        if (photo.IsCustomerResource()) {
                            self.ShowCustomerAtStart(true);
                        }
                    }
                }
                self.CustomerPhotos(custPhotos);
                self.OrganizationPhotos(orgPhotos);
                window.setTimeout(function () {
                    // JCF hack for checkboxes
                    $.jcfModule.customForms.replaceAll();
                    $.initJcf.tabs();
                }, 10);
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
                        self.IsLoading(false);
                    })
                    .error(function (jqHxr, textStatus, errorThrown) {
                        logger.log("Error retrieving photo data: " + textStatus);
                        logger.log(errorThrown);
                    })
                    .always(function () {
                        self.IsLoading(false);
                    });
            }

            var self = this,
                photoSlot = data.Slot,
                categoryId = data.CategoryId,
                $elem = data.Element,
                saveCallback = data.OnSave,
                cancelCallback = data.OnCancel,
                selectedResourceId = null;

            self.IsLoading = ko.observable(true);
            self.CustomerPhotos = ko.observableArray([]);
            self.OrganizationPhotos = ko.observableArray([]);
            self.ShowCustomerAtStart = ko.observable(false);

            self.SelectedPhoto = ko.observable(null);
            self.PhotoPreviewShown = ko.computed(function () {
                return self.SelectedPhoto() !== null;
            });
            self.IsSelected = function (photo) {
                return self.SelectedPhoto() === photo;
            };
            self.SelectPhoto = function (photo) {
                var i, photos = self.CustomerPhotos();
                for (i = 0; i < photos.length; i++) {
                    if (photos[i] !== photo && photos[i].IsSelected()) {
                        photos[i].IsSelected(false);
                    }
                }
                photos = self.OrganizationPhotos();
                for (i = 0; i < photos.length; i++) {
                    if (photos[i] !== photo && photos[i].IsSelected()) {
                        photos[i].IsSelected(false);
                    }
                }

                if (self.IsSelected(photo)) {
                    photo.IsSelected(false);
                    self.SelectedPhoto(null);
                } else {
                    photo.IsSelected(true);
                    self.SelectedPhoto(photo);
                }
            };

            self.Show = function (selectedId) {
                selectedResourceId = selectedId;
                ko.cleanNode($elem[0]);
                $elem.find(".modal-dialog").empty();
                ko.applyBindings(self, $elem[0]);
                getPhotos();
                $(".jcf-upload-button").first().click();
            };

            self.Hide = function () {
                $.fancybox.close();
            };

            self.Cancel = function () {
                logger.log("Canceling");
                self.Hide();
                cancelCallback();
            };

            self.Save = function () {
                logger.log("Saving");

                var selected = self.SelectedPhoto();

                self.Hide();
                saveCallback(selected);
            };
        }

        return ctor;
    });