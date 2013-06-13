/// <reference path="../vsdoc/require.js" />

define(["jquery"], function ($) {
    return function (platformId, loginKey, userName, userEmail) {
        var self = this;

        self.PerformLogin = function () {
            $.ajax({
                type: "POST",
                url: "/OAuth/Authorize" +
                    "?pId=" + encodeURIComponent(platformId) +
                    "&key=" + encodeURIComponent(loginKey) +
                    "&name=" + encodeURIComponent(userName) +
                    "&email=" + encodeURIComponent(userEmail),
                success: function (data, textStatus, jqXHR) {
                    // TODO: Redirect?
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error logging in ");
                    console.log(errorThrown);
                    alert("error logging in");
                }
            });
        };
    };
});