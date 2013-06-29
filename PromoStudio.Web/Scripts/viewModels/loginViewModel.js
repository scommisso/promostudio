define(["jquery", "googleOAuth", "facebookOAuth"],
    function ($, googleOauth, facebookOAuth) {
        return function () {
            var self = this;

            function performLogin(platformId, loginKey, userName, userEmail) {
                $.ajax({
                    type: "POST",
                    url: "/OAuth/Authorize" +
                        "?pId=" + encodeURIComponent(platformId) +
                        "&key=" + encodeURIComponent(loginKey) +
                        "&name=" + encodeURIComponent(userName) +
                        "&email=" + encodeURIComponent(userEmail),
                    success: function (data, textStatus, jqXHR) {
                        $.pjax({ url: "/", container: $.fn.pjaxScaffold.getContainer() });
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log("Error logging in ");
                        console.log(errorThrown);
                        alert("error logging in");
                    }
                });
            };

            self.pageLoaded = function () {
                if ($("#loginButtons").size() > 0) {
                    var goa = new googleOauth("#gLogin", function (user) {
                        $("#loginButtons").hide();
                        $('#userName').text("Welcome, " + user.name + " (" + user.email + ")");
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
                        $('#userName').text("Welcome, " + user.name + " (" + user.email + ")");
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
        };
});