/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.2.1.debug.js" />

define([], function (ko) {
    return {
        templateScriptItemCategory: function (id) {
            var arr = [
                "Unknown",
                "Logo",
                "Portfolio",
                "Intro",
                "Ending",
                "Text"
            ];
            if (id === undefined || id === null) {
                return arr
            }
            
            return arr[id];
        },
        templateScriptItemType: function (id) {
            var arr = [
                "Unknown",
                "Image",
                "Video",
                "Audio",
                "Text"
            ];
            if (id === undefined || id === null) {
                return arr
            }

            return arr[id];
        },
        customerVideoItemType: function (id) {
            var arr = [
                "Unknown",
                "Stock Video",
                "Stock Audio",
                "Template Video",
                "Voice Over"
            ];
            if (id === undefined || id === null) {
                return arr
            }

            return arr[id];
        },
        customerVideoRenderStatus: function (id) {
            var arr = [
                "Unknown",
                "Pending",
                "Template Builds In Progress - Preview",
                "Template Builds Completed - Preview",
                "Render In Progress - Preview",
                "Render Completed - Preview",
                "Uploading - Preview",
                "Awaiting Voice Over",
                "Voice Over Completed, Pending Final Render",
                "Template Builds In Progress",
                "Template Builds Completed",
                "Render In Progress",
                "Render Completed",
                "Uploading",
                "Completed",
                "Canceled"
            ];
            if (id === undefined || id === null) {
                return arr
            }

            return arr[id];
        }
    };
});
