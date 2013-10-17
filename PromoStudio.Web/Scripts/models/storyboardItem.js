/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["models/templateScript",
        "knockout"],
    function (templateScript,
        ko) {
    var ctor = function (data) {
        var self = this;
        data = data || {};

        self.pk_StoryboardItemId = ko.observable(data.pk_StoryboardItemId || null);
        self.fk_StoryboardId = ko.observable(data.fk_StoryboardId || null);
        self.fk_StoryboardItemTypeId = ko.observable(data.fk_StoryboardItemTypeId || null);
        self.fk_TemplateScriptId = ko.observable(data.fk_TemplateScriptId || null);
        self.Name = ko.observable(data.Name || null);
        self.SortOrder = ko.observable(data.SortOrder || null);
        
        self.TemplateScript = ko.observable(null);

        self.LoadScript = function(templateScriptData) {
            if (templateScriptData) {
                self.TemplateScript(new templateScript(templateScriptData));
            }
        };
        self.LoadScript(data.TemplateScript);
    };

    ctor.prototype.toJSON = function () {
        var copy = ko.toJS(this);
        // remove any unneeded properties
        delete copy.TemplateScript;

        return copy;
    };
    return ctor;
});
