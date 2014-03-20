define(["ps/logger", "jquery", "lib/jso2"], function (logger, $, OAuth) {
    function FacebookOauth(clientId, redirectUrl, loginCallback, oauthCallback) {
        var self = this;

        self.providerId = "facebook";
        self.loginCallback = loginCallback;
        self.oauthCallback = oauthCallback;
        self.oauth = new OAuth(self.providerId, {
            client_id: clientId,
            redirect_uri: redirectUrl,
            authorization: "https://www.facebook.com/dialog/oauth",
            presenttoken: "qs"
        });
    };

    FacebookOauth.prototype.initOauthCallback = function () {
        var self = this;
        self.oauth.callback(null, self.oauthCallback, self.providerId);
    };

    FacebookOauth.prototype.login = function () {
        var self = this;
        self.oauth.ajax({
            url: "https://graph.facebook.com/me?" + 'fields=' + encodeURIComponent('id,name,email,picture'),
            oauth: {
                scopes: {
                    request: ["email"],
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

    return FacebookOauth;
});