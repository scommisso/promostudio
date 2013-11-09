/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />

define(["strings"], function (strings) {
    return {
        storyboardItemType: function (id) {
            var arr = [
                strings.getResource("StoryboardItemType__Logo"),
                strings.getResource("StoryboardItemType__Stock"),
                strings.getResource("StoryboardItemType__Text"),
                strings.getResource("StoryboardItemType__Photo"),
                strings.getResource("StoryboardItemType__Ending")
            ];
            if (id === undefined || id === null) {
                return arr;
            }

            return arr[id];
        },
        templateScriptItemCategory: function (id) {
            var arr = [
                strings.getResource("ScriptItemCategory__Unknown"),
                strings.getResource("ScriptItemCategory__Logo"),
                strings.getResource("ScriptItemCategory__Portfolio"),
                strings.getResource("ScriptItemCategory__Intro"),
                strings.getResource("ScriptItemCategory__Ending"),
                strings.getResource("ScriptItemCategory__Text")
            ];
            if (id === undefined || id === null) {
                return arr;
            }
            
            return arr[id];
        },
        templateScriptItemType: function (id) {
            var arr = [
                strings.getResource("ScriptItemType__Unknown"),
                strings.getResource("ScriptItemType__Image"),
                strings.getResource("ScriptItemType__Video"),
                strings.getResource("ScriptItemType__Audio"),
                strings.getResource("ScriptItemType__Text")
            ];
            if (id === undefined || id === null) {
                return arr;
            }

            return arr[id];
        },
        customerVideoItemType: function (id) {
            var arr = [
                strings.getResource("VideoItemType__Unknown"),
                strings.getResource("VideoItemType__Stock Video"),
                strings.getResource("VideoItemType__Stock Audio"),
                strings.getResource("VideoItemType__Template Video"),
                strings.getResource("VideoItemType__Voice Over")
            ];
            if (id === undefined || id === null) {
                return arr;
            }

            return arr[id];
        },
        customerVideoRenderStatus: function (id) {
            var arr = [
                strings.getResource("VideoRenderStatus__Unknown"),
                strings.getResource("VideoRenderStatus__Pending"),
                strings.getResource("VideoRenderStatus__Building_Preview"),
                strings.getResource("VideoRenderStatus__Building_Completed_Preview"),
                strings.getResource("VideoRenderStatus__Rendering_Preview"),
                strings.getResource("VideoRenderStatus__Rendering_Completed_Preview"),
                strings.getResource("VideoRenderStatus__Uploading_Preview"),
                strings.getResource("VideoRenderStatus__Awaiting_Voice_Over"),
                strings.getResource("VideoRenderStatus__Voice_Over_Completed"),
                strings.getResource("VideoRenderStatus__Building"),
                strings.getResource("VideoRenderStatus__Building_Completed"),
                strings.getResource("VideoRenderStatus__Rendering"),
                strings.getResource("VideoRenderStatus__Rendering_Completed"),
                strings.getResource("VideoRenderStatus__Uploading"),
                strings.getResource("VideoRenderStatus__InProgressHostProcessing"),
                strings.getResource("VideoRenderStatus__Completed"),
                strings.getResource("VideoRenderStatus__Canceled")
            ];
            if (id === undefined || id === null) {
                return arr;
            }

            return arr[id];
        }
    };
});
