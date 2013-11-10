define(["knockout", "jquery", "lib/jquery.jplayer"], function (ko, $) {
    var supportedUpdates = [
        "size", "sizeFull", "smoothPlayBar", "fullScreen", "fullWindow",
        "audioFullScreen", "autohide", "volume", "muted", "cssSelectorAncestor", "cssSelector", "loop",
        "repeat", "emulateHtml", "nativeVideoControls", "noFullWindow", "noVolume", "timeFormat",
        "keyEnabled", "keyBindings"],
        playerClass = ".jp-jplayer",
        containerClass = '.jp-audio',
        lastId = 0;
    
    function buildPlayerHtml(playerId, containerId) {
        var playerHtml =
            '<div class="jp-jplayer" id="' + playerId + '"></div>' +
                '<div class="jp-audio" id="' + containerId + '">' +
                '<div class="jp-type-single">' +
                '<div class="jp-gui jp-interface">' +
                '<ul class="jp-controls">' +
                '<li><a href="javascript:;" class="jp-play" tabindex="1">play</a></li>' +
                '<li><a href="javascript:;" class="jp-pause" tabindex="1">pause</a></li>' +
                '<li><a href="javascript:;" class="jp-stop" tabindex="1">stop</a></li>' +
                '<li><a href="javascript:;" class="jp-mute" tabindex="1" title="mute">mute</a></li>' +
                '<li><a href="javascript:;" class="jp-unmute" tabindex="1" title="unmute">unmute</a></li>' +
                '<li><a href="javascript:;" class="jp-volume-max" tabindex="1" title="max volume">max volume</a></li>' +
                '</ul>' +
                '<div class="jp-progress">' +
                '<div class="jp-seek-bar">' +
                '<div class="jp-play-bar"></div>' +
                '</div>' +
                '</div>' +
                '<div class="jp-volume-bar">' +
                '<div class="jp-volume-bar-value"></div>' +
                '</div>' +
                '<div class="jp-time-holder">' +
                '<div class="jp-current-time"></div>' +
                '<div class="jp-duration"></div>' +
                '<ul class="jp-toggles">' +
                '<li><a href="javascript:;" class="jp-repeat" tabindex="1" title="repeat">repeat</a></li>' +
                '<li><a href="javascript:;" class="jp-repeat-off" tabindex="1" title="repeat off">repeat off</a></li>' +
                '</ul>' +
                '</div>' +
                '</div>' +
                '<div class="jp-title">' +
                '<ul>' +
                '<li data-bind="text: title"></li>' +
                '</ul>' +
                '</div>' +
                '<div class="jp-no-solution">' +
                '<span>Update Required</span>' +
                'To play the media you will need to either update your browser to a recent version or update your <a href="http://get.adobe.com/flashplayer/" target="_blank">Flash plugin</a>.' +
                '</div>' +
                '</div>' +
                '</div>';
        return playerHtml;
    }

    ko.bindingHandlers.jplayer = {        
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var vals = valueAccessor(),
                opts = ko.toJS(vals.options || {}),
                media = ko.toJS(vals.media || opts.media),
                supplied = ko.toJS(opts.supplied),
                title = ko.toJS(vals.title || opts.title),
                $element = $(element),
                $container = $element.children(playerClass),
                containerId = $container.attr("id"),
                cb = opts.ready || function () { },
                newContainerIdBase = "ko-jplayer-container-",
                newPlayerIdBase = "ko-jplayer-player-",
                playerVm = {
                    title: ko.observable(title)
                },
                playerId, prop;
            if (!opts.swfPath) {
                opts.swfPath = "/Scripts/lib";
            }
            if (!supplied) {
                supplied = [];
                for (prop in media) {
                    supplied.push(prop);
                }
                supplied = supplied.join(", ");
            }
            opts.supplied = supplied;
            if ($container.length === 0) {
                // if no container, add it
                lastId += 1;
                containerId = newContainerIdBase + lastId;
                playerId = newPlayerIdBase + lastId;
                $element.append(buildPlayerHtml(playerId, containerId));
            }
            else if (!containerId) {
                // if container is present but missing an id, add it
                lastId += 1;
                containerId = newContainerIdBase + lastId;
                $container.attr("id", containerId);
            }
            opts.cssSelectorAncestor = "#" + containerId;
            opts.ready = function (e) {
                var $player = $(this),
                    $e = $(element);
                $e.data("ko.jplayer.ready", true);
                if (media) {
                    $e.data("ko.jplayer.setmedia", true);
                    $player.jPlayer("setMedia", media);
                }
                cb(e);
            };
            opts.play = function(e) {
                var $player = $(this);
                $player.jPlayer("pauseOthers");
            };
            
            opts.wmode = "window";
            opts.smoothPlayerBar = true;
            
            $element.data("ko.jplayer.setmedia", false);
            $element.data("ko.jplayer.ready", false);
            setTimeout(function () {
                // give DOM time to update before binding since we created an ID for the container

                var $player = $(element).children(playerClass),
                    $ctr = $element.children(containerClass),
                    player = $player[0],
                    container = $ctr[0],
                    ctx = bindingContext.createChildContext(playerVm);
                
                ko.utils.domNodeDisposal.addDisposeCallback(player, function () {
                    $(element).children(playerClass).jPlayer("destroy");
                });
                
                ko.applyBindingsToDescendants(ctx, container);
                
                $player.jPlayer(opts);
            }, 1);
            
            return { controlsDescendantBindings: true };
        },
        update: function (element, valueAccessor) {
            var vals = valueAccessor(),
                opts = ko.toJS(vals.options || {}),
                $element = $(element),
                $player = $element.children(playerClass),
                i, key, oldVal, media;

            for (i = 0; i < supportedUpdates.length; i++) {
                key = supportedUpdates[i];
                if (opts.hasOwnProperty(key)) {
                    oldVal = $player.jPlayer("option", key);
                    if (oldVal !== opts[key]) {
                        $player.jPlayer("option", key, opts[key]);
                    }
                }
            }

            if ($element.data("ko.jplayer.ready")
                && !$element.data("ko.jplayer.setmedia")) {
                media = ko.toJS(vals.media || opts.media);
                $element.data("ko.jplayer.setmedia", true);
                $player.jPlayer("setMedia", media);
            }
        }
    };
    return ko;
});
