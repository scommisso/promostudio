/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/fileupload.js" />
/// <reference path="../ps/common.js" />
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
        "ps/logger",
        "ps/common",
        "ps/fileupload",
        "ps/extensions"
],
    function (
        customerResource,
        customerResourceViewModel,
        $,
        ko,
        strings,
        enums,
        logger,
        common,
        fileUpload) {
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
                        if (photo.IsOrganizationResource()) {
                            self.ShowOrganizationAtStart(true);
                        }
                    }
                }
                self.CustomerPhotos(custPhotos);
                self.OrganizationPhotos(orgPhotos);
                window.setTimeout(function () {
                    // JCF hack for checkboxes
                    $.jcfModule.customForms.replaceAll();
                    $.initJcf.tabs();
                    var checkedItem = $("ul.list-photo li.checked");
                    checkedItem.find("input").attr("checked", "checked");
                    checkedItem.find(".chk-area").removeClass("chk-unchecked").addClass("chk-checked");
                }, 1);
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
            self.ShowOrganizationAtStart = ko.observable(false);

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

            var _fileUpload = new fileUpload({
                CategoryId: categoryId,
                IsOrganizationResource: false,
                OnUploadFileCompleted: function (err, uploadedFile) {
                    if (err) {
                        logger.log("Error uploading file: " + err);
                        self.UploadFileError(err);
                    }
                    else if (uploadedFile) {
                        self.Show(uploadedFile.pk_CustomerResourceId, true);
                    }
                }
            });

            self.UploadFileChosen = _fileUpload.UploadFileChosen;
            self.UploadFileName = _fileUpload.UploadFileName;
            self.UploadFileSize = _fileUpload.UploadFileSize;
            self.IsUploading = _fileUpload.IsUploading;
            self.BytesUploaded = _fileUpload.BytesUploaded;

            self.UploadFileError = ko.observable(null);
            self.UploadFileSizeKb = ko.computed(function() {
                var size = self.UploadFileSize();
                size = (size / 1000).format();
                return size;
            });

            function launchUploader(e) {
                var srcElement = common.getSourceElement(e);
                $(srcElement).closest("div").find("input[type='file']").click();
            }

            self.OnUploadFileSelected = function (data, event) {
                _fileUpload.OnUploadFileSelected(event);
            };

            self.UploadFile = function (d, e) {
                if (!self.UploadFileChosen()) {
                    launchUploader(e);
                } else {
                    _fileUpload.UploadFile();
                }
                common.cancelEvent(e);
                return;
            };

            self.ChangeUploadFile = function(d, e) {
                if (self.UploadFileChosen()) {
                    launchUploader(e);
                }
                common.cancelEvent(e);
                return;
            };

            function reset() {
                _fileUpload.ResetState();
                self.UploadFileError(null);
                self.ShowOrganizationAtStart(false);
            }

            self.Show = function (selectedId, skipLaunch) {
                reset();
                selectedResourceId = selectedId;
                ko.cleanNode($elem[0]);
                $elem.find(".modal-dialog").empty();
                ko.applyBindings(self, $elem[0]);
                getPhotos();
                if (skipLaunch !== true) {
                    $(".jcf-upload-button.photo-chooser").first().click();
                }
            };

            self.Hide = function () {
                $.fancybox.close();
            };

            self.Cancel = function () {
                self.Hide();
                cancelCallback();
            };

            self.Save = function () {
                var selected = self.SelectedPhoto();
                self.Hide();
                saveCallback(selected);
            };
        }

        return ctor;
    });