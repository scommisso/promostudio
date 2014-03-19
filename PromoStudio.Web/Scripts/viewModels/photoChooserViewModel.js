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
        "ps/logger",
        "ps/common",
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
        common) {
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

            self.UploadFileChosen = ko.observable(false);
            self.FileToUpload = ko.observable(null);
            self.UploadFileName = ko.observable(null);
            self.UploadFileContentType = ko.observable(null);
            self.UploadFileSize = ko.observable(null);
            self.UploadFileError = ko.observable(null);
            self.IsUploading = ko.observable(false);
            self.BytesUploaded = ko.observable(0);
            self.UploadFileSizeKb = ko.computed(function() {
                var size = self.UploadFileSize();
                size = (size / 1000).format();
                return size;
            });

            function launchUploader(e) {
                var srcElement = common.getSourceElement(e);
                $(srcElement).closest("div").find("input[type='file']").click();
            }

            function performUpload(originalEvent, callback) {
                if (!("FormData" in window)) {
                    callback("Your browser does not support AJAX file uploads.");
                    return;
                }

                var file = self.FileToUpload(),
                    fd = new FormData();
                fd.append("upload_photo", file);
                self.IsUploading(true);
                $.ajax({
                    type: "POST",
                    url: "/Resources/Upload?category={0}&isOrgResource={1}".format(categoryId, false),
                    enctype: 'multipart/form-data',
                    xhr: function () {
                        var myXhr = $.ajaxSettings.xhr();
                        if (myXhr.upload) {
                            myXhr.upload.addEventListener(
                                "progress",
                                function (e) {
                                    if (e.lengthComputable) {
                                        self.BytesUploaded(e.loaded);
                                        self.UploadFileSize(e.total);
                                    }
                                }, false); // For handling the progress of the upload
                        }
                        return myXhr;
                    },
                    data: fd,
                    processData: false,
                    contentType: false,
                    cache: false,
                    success: function (data, textStatus, jqXhr) {
                        self.IsUploading(false);
                        self.BytesUploaded(0);
                        var retData = data;
                        if (typeof data === "string") {
                            try {
                                retData = JSON.parse(data);
                            } catch (e) {
                                retData = "unable to parse server response";
                            }
                        }
                        callback(retData);
                    },
                    error: function (jqXhr, textStatus, error) {
                        self.IsUploading(false);
                        self.BytesUploaded(0);
                        callback(error);
                    }
                });
            }

            self.UploadFile = function (d, e) {
                if (!self.UploadFileChosen()) {
                    launchUploader(e);
                } else {
                    performUpload(e, function (uploaded) {
                        if (typeof uploaded === "string") {
                            self.UploadFileError(uploaded);
                        } else {
                            // reload
                            console.log(uploaded);
                            self.Show(uploaded.pk_CustomerResourceId, true);
                        }
                    });
                }
                e.preventDefault();
                return true;
            };
            self.ChangeUploadFile = function(d, e) {
                if (self.UploadFileChosen()) {
                    launchUploader(e);
                }
                e.preventDefault();
                return true;
            };
            self.OnUploadFileSelected = function (d, e) {
                var srcElement = common.getSourceElement(e);
                if (!srcElement || !srcElement.files || !srcElement.files.length) { return; }
                var file = srcElement.files[0];
                self.FileToUpload(file);
                self.UploadFileChosen(true);
                self.UploadFileName(file.name);
                self.UploadFileContentType(file.type);
                self.UploadFileSize(file.size);
                self.UploadFileError(null);
                self.BytesUploaded(0);
                self.IsUploading(false);
            };

            function reset() {
                self.FileToUpload(null);
                self.UploadFileChosen(false);
                self.UploadFileName(null);
                self.UploadFileContentType(null);
                self.UploadFileSize(0);
                self.UploadFileError(null);
                self.BytesUploaded(0);
                self.IsUploading(false);

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