/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="extensions.js" />

define(["jquery", "ps/extensions"], function ($) {
    function checkPlayerExists(id) {
        var selector = "#vidyard_wrapper_{0}".format(id);
        return $(selector).length > 0;
    }
    
    function getPlayerFunction(id) {
        var playFn = "fn_vidyard_{0}".format(id.replace(/-/g, "$"));
        if (typeof window[playFn] === "function") {
            return window[playFn];
        }
        return null;
    }
            
    function buildPlayer(id, type, $container, callback) {
        var $player = $("#vidyard_wrapper_{0}".format(id)),
            head, script, interval, fn;
        if ($player.length > 0) {
            callback();
        }

        head = document.getElementsByTagName("head")[0] || document.body;
        if (head)
        {
            script = document.createElement("script");
            script.type = "text/javascript";
            script.src = "//play.vidyard.com/{0}.js?v=3.1&type={1}".format(id, type);
            head.appendChild(script);
            interval = setInterval(function() {
                fn = getPlayerFunction(id);
                if (fn) {
                    clearInterval(interval);
                    callback();
                }
            }, 10);
                
            $("<div />")
                .attr("id", "vidyard_wrapper_{0}".format(id))
                .addClass("vidyard_wrapper")
                .appendTo($container);
        }
    }
            
    function playVideo(id) {
        var fn = getPlayerFunction(id);
        if (fn) { fn(); }
    }

    return function (data) {
        var self = this;
        data = data || {};

        self.VideoId = data.VideoId || null;
        self.Container = data.Container || $("body");
                
        function build(type, callback) {
            var id = self.VideoId;
            if (!id) { return; }

            if (!checkPlayerExists(id)) {
                buildPlayer(id, type, self.Container, callback);
            } else {
                callback();
            }
        }

        self.ShowLightbox = function () {
            var id = self.VideoId;
            if (!id) { return; }
                    
            build("lightbox", function() {
                playVideo(id);
            });
        };

    };
});
