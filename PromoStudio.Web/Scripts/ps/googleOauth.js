define(["ps/logger", "jquery", "lib/jso2"], function (logger, $, OAuth) {
    function GoogleOauth(clientId, redirectUrl, loginCallback) {
        var self = this;
        self.callback = loginCallback;
        self.oauth = new OAuth("google", {
            client_id: clientId,
            redirect_uri: redirectUrl,
            authorization: "https://accounts.google.com/o/oauth2/auth"
        });
    };

    GoogleOauth.prototype.init = function ()
    {
        var self = this;
        self.oauth.callback();
    };

    GoogleOauth.prototype.login = function () {
        var self = this;
        self.oauth.ajax({
            url: "https://www.googleapis.com/oauth2/v1/userinfo",
            oauth: {
                scopes: {
                    request: ["profile", "email"],
                    require: ["email"]
                }
            },
            dataType: 'json',
            success: function(data) {
                self.callback(null, data);
            },
            error: function (xHr, status, error) {
                self.callback(status + ": " + error);
            }
        });
    };

    return GoogleOauth;
});