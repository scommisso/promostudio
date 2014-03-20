/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.11.0.intellisense.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../ps/extensions.js" />

"use strict";

define(["jqueryui",
        "ps/googleOauth",
        "ps/facebookOauth",
        "strings",
        "ps/logger",
        "ps/extensions"],
    function ($,
        googleOauth,
        facebookOAuth,
        strings,
        logger) {
        function ctor(data) {
            
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
                welcomeText = strings.getResource("Login__Welcome"),
                googleClientId = data.googleClientId,
                facebookClientId = data.facebookClientId,
                oauthRedirectUrl = data.redirectUrl;

            function performLoginCallback(platformId, error, user) {
                if (error) {
                    logger.log("Error logging in ");
                    logger.log(error);
                    alert("error logging in");
                    return;
                }

                $("#loginButtons").hide();
                $('#userName').text(welcomeText.format(user.name, user.email));
                $('#loginResult').show();
                $('.navbar').show();
                performLogin(platformId, user.id, user.name, user.email);
            }

            self.pageLoaded = function () {
                if ($("#loginButtons").size() > 0) {
                    var $gLogin = $("#gLogin"),
                        $fbLogin = $("#fbLogin"),
                        goa = new googleOauth(googleClientId, oauthRedirectUrl, performLoginCallback.bind(null, 1));
                    $gLogin.click(function () {
                        if ($gLogin.hasClass("wait")) {
                            return;
                        }
                        $fbLogin.hide();
                        $gLogin.addClass("wait");
                        goa.login();
                    });

                    var foa = new facebookOAuth(facebookClientId, oauthRedirectUrl, performLoginCallback.bind(null, 2));
                    $fbLogin.click(function () {
                        if ($fbLogin.hasClass("wait")) {
                            return;
                        }
                        $gLogin.hide();
                        $fbLogin.addClass("wait");
                        foa.login();
                    });

                    goa.init();
                    foa.init();
                }
            };
        }

        return ctor;
    });