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
        "lib/fileuploader",
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
        fileuploader) {
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
            self.UploadFileId = ko.observable(null);
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

            var _fileUploader = null;
            function initUpload(callback) {
                var file = self.FileToUpload(),
                    state = {},
                    url = "/Resources/Upload?category={0}&isOrgResource={1}".format(categoryId, false);

                _fileUploader = new fileuploader.UploadHandlerXhr({
                    action: url,
                    inputName: "fileName",
                    params: {},
                    sizeLimit: 0, // add client side limit here, also enforce server side
                    onSubmit: function() {
                        self.IsUploading(true);
                        self.BytesUploaded(0);
                    },
                    onProgress: function(id, name, loaded, total) {
                        self.BytesUploaded(loaded);
                        self.UploadFileSize(total);
                    },
                    onError: function(fId, fName, xHr) {
                        state.errored = true;
                        self.IsUploading(false);
                        self.BytesUploaded(0);
                        callback(xHr.responseText);
                    },
                    onComplete: function(fId, fName, uploadData) {
                        if (state.errored) {
                            return;
                        }
                        if (typeof uploadData === "string") {
                            try {
                                uploadData = JSON.parse(uploadData);
                            } catch (e) {
                                uploadData = "unable to parse server response";
                            }
                        }
                        self.IsUploading(false);
                        self.BytesUploaded(0);
                        callback(null, uploadData);
                    }
                });
                var fileId = _fileUploader.add(file),
                    fileName = _fileUploader.getName(fileId),
                    fileSize = _fileUploader.getSize(fileId);

                self.UploadFileId(fileId);
                self.UploadFileName(fileName);
                self.UploadFileSize(fileSize);
                self.UploadFileError(null);
                self.BytesUploaded(0);
                self.IsUploading(false);
                self.UploadFileChosen(true);
            }

            function performUpload(originalEvent, callback) {
                if (!fileuploader.UploadHandlerXhr.isSupported) {
                    callback("Your browser does not support AJAX file uploads.");
                    return;
                }
                _fileUploader.upload(self.UploadFileId());
            }

            self.UploadFile = function (d, e) {
                if (!self.UploadFileChosen()) {
                    launchUploader(e);
                } else {
                    performUpload();
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
                initUpload(function (err, uploaded) {
                    if (err) {
                        self.UploadFileError(err);
                    } else {
                        // reload
                        console.log(uploaded);
                        self.Show(uploaded.pk_CustomerResourceId, true);
                    }
                });
            };

            function reset() {
                self.FileToUpload(null);
                self.UploadFileChosen(false);
                self.UploadFileId(null);
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