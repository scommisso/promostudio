/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../lib/bootstrap.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="photoSlotViewModel.js" />

define(["knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "bootstrap"
    ],
    function (
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

            self.Show = function () {
                ko.cleanNode($elem[0]);
                ko.applyBindings(self, $elem[0]);
                $elem.modal("show");
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
        };
    });