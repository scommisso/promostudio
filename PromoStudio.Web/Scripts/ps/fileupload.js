/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../lib/fileuploader.js" />
/// <reference path="../ps/extensions.js" />

"use strict";

define(["knockout",
        "ps/common",
        "lib/fileuploader",
        "ps/extensions"
],
    function (
        ko,
        common,
        fileuploader) {
        function ctor(data) {

            var self = this,
                _fileUploader = null;
            data = data || {};

            self.CategoryId = ko.observable(data.CategoryId || 0);
            self.IsOrganizationResource = ko.observable(data.IsOrganizationResource || false);

            self.UploadFileId = ko.observable(null);
            self.UploadFileName = ko.observable(null);
            self.UploadFileSize = ko.observable(null);
            self.UploadFileChosen = ko.observable(false);
            self.IsUploading = ko.observable(false);
            self.BytesUploaded = ko.observable(0);

            function getUploadOptions(callback) {
                var state = {},
                    url = "/Resources/Upload?category={0}&isOrgResource={1}".format(self.CategoryId(), self.IsOrganizationResource()),
                    options = {
                        action: url,
                        minSizeLimit: 0,
                        sizeLimit: 0, // add client side limit here, also enforce server side
                        inputName: "fileName",
                        onSubmit: function (fId, fName) {
                            var fileSize = _fileUploader._handler.getSize(fId);

                            self.UploadFileId(fId);
                            self.UploadFileName(fName);
                            self.UploadFileSize(fileSize);
                            self.UploadFileChosen(true);
                            self.IsUploading(false);
                            self.BytesUploaded(0);
                            return false;
                        },
                        onProgress: function(fId, fName, loaded, total) {
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
                    };
                return options;
            }

            function initUploader(callback) {
                var options = getUploadOptions(callback);
                _fileUploader = new fileuploader.FileUploaderBasic(options);
            }

            self.ResetState = function () {
                self.UploadFileId(null);
                self.UploadFileName(null);
                self.UploadFileSize(null);
                self.UploadFileChosen(false);
                self.IsUploading(false);
                self.BytesUploaded(0);
            };

            self.UploadFile = function () {
                var fId = self.UploadFileId();
                if (fId === null || typeof fId === "undefined") { return; }
                _fileUploader._handler.upload(fId);
            };

            self.OnUploadFileCompleted = data.OnUploadFileCompleted;

            self.OnUploadFileSelected = function (fileInputChangeEventArgs) {
                var srcElement = common.getSourceElement(fileInputChangeEventArgs);
                if (!srcElement || !srcElement.files || !srcElement.files.length) { return; }

                fileuploader.UploadHandlerXhr.isSupported = function() { return false; };
                initUploader(function (err, uploadedFile) {
                    if (typeof self.OnUploadFileCompleted !== "function") { return; }
                    self.OnUploadFileCompleted(err, uploadedFile);
                });
                _fileUploader._onInputChange(srcElement);
            };
        }

        return ctor;
    });