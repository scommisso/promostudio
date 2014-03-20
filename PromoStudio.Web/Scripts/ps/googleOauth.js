define(["ps/logger", "jquery", "lib/jso2"], function (logger, $, OAuth) {
    function GoogleOauth(clientId, redirectUrl, loginCallback, oauthCallback) {
        var self = this;

        self.providerId = "google";
        self.loginCallback = loginCallback;
        self.oauthCallback = oauthCallback;
        self.oauth = new OAuth(self.providerId, {
            client_id: clientId,
            redirect_uri: redirectUrl,
            authorization: "https://accounts.google.com/o/oauth2/auth"
        });
    };

    GoogleOauth.prototype.initOauthCallback = function ()
    {
        var self = this;
        self.oauth.callback(null, self.oauthCallback, self.providerId);
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
                self.loginCallback(null, data);
            },
            error: function (xHr, status, error) {
                self.loginCallback(status + ": " + error);
            }
        });
    };

    return GoogleOauth;
});