/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />

"use strict";

define(["jqueryui",
        "googleOAuth",
        "facebookOAuth",
        "strings",
        "ps/logger",
        "ps/extensions"],
    function ($,
        googleOauth,
        facebookOAuth,
        strings,
        logger) {
        function ctor() {

            function performLogin(platformId, loginKey, userName, userEmail) {
                $.ajax({
                    type: "POST",
                    url: "/OAuth/Authorize" +
                        "?pId=" + encodeURIComponent(platformId) +
                        "&key=" + encodeURIComponent(loginKey) +
                        "&name=" + encodeURIComponent(userName) +
                        "&email=" + encodeURIComponent(userEmail)
                })
                    .done(function () {
                        if ($.pjax) {
                            $.pjax({ url: "/", container: $.fn.pjaxScaffold.getContainer() });
                        } else {
                            document.location.reload();
                        }
                    })
                    .error(function (jqXhr, textStatus, errorThrown) {
                        logger.log("Error logging in ");
                        logger.log(errorThrown);
                        alert("error logging in");
                    });
            }

            var self = this,
                welcomeText = strings.getResource("Login__Welcome");

            self.pageLoaded = function () {
                if ($("#loginButtons").size() > 0) {
                    var goa = new googleOauth("#gLogin", function (user) {
                        $("#loginButtons").hide();
                        $('#userName').text(welcomeText.format(user.name, user.email));
                        $('#loginResult').show();
                        $('.navbar').show();
                        performLogin(1, user.id, user.name, user.email);
                    });
                    $("#gLogin").click(function () {
                        $("#fbLogin").hide();
                        goa.login();
                    });

                    var foa = new facebookOAuth("#fbLogin", function (user) {
                        $("#loginButtons").hide();
                        $('#userName').text(welcomeText.format(user.name, user.email));
                        $('#loginResult').show();
                        $('.navbar').show();
                        performLogin(2, user.id, user.name, user.email);
                    });
                    $("#fbLogin").click(function () {
                        $("#gLogin").hide();
                        foa.login();
                    });
                }
            };
        }

        return ctor;
    });