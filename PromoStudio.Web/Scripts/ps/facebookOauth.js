define(["ps/logger", "jquery", "lib/jso2"], function (logger, $, OAuth) {
    function FacebookOauth(clientId, redirectUrl, loginCallback) {
        var self = this;
        self.callback = loginCallback;
        self.oauth = new OAuth("facebook", {
            client_id: clientId,
            redirect_uri: redirectUrl,
            authorization: "https://www.facebook.com/dialog/oauth",
            presenttoken: "qs"
        });
    };

    FacebookOauth.prototype.init = function ()
    {
        var self = this;
        self.oauth.callback();
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
                self.callback(null, data);
            },
            error: function (xHr, status, error) {
                self.callback(status + ": " + error);
            }
        });
    };

    return FacebookOauth;
});