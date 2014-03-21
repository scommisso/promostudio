define(["knockout", "lib/soundjs-0.5.2"], function (ko, createJs) {

    var _playerInitialized = false, // globals
        _playerCanPlay = true,
        _lastId = 0,
        _playerState = {},
        _playerLoadListener = false;

    function registerLoadListener() {
        if (_playerLoadListener) { return; }
        createjs.Sound.addEventListener("fileload", function (e) {
            var state = _playerState[e.id];
            if (!state) { return; }
            state.playerReady = true;
        });
        _playerLoadListener = true;
    }

    function play(id) {
        var state = _playerState[id];
        if (!state.playerReady) {
            var interval = window.setInterval(function() {
                if (!state.playerReady) {
                    return;
                }
                window.clearInterval(interval);
                playSound(id);
            }, 20);
        } else {
            playSound(id);
        }
    }

    function stopHandler(state) {
        state.isPlaying = false;
        state.isPlayingObservable(false);
    }

    function stopAll() {
        var id, state;
        for (id in _playerState)
        {
            state = _playerState[id];
            stopHandler(state);
        }
        createJs.Sound.stop();
    }

    function playSound(id) {
        var state = _playerState[id],
            playerInstance;

        if (state.isPlaying) { return; }

        stopAll();
        state.isPlayingObservable(true);
        playerInstance = createJs.Sound.play(id, createJs.Sound.INTERRUPT_ANY);
        if (!playerInstance || playerInstance.playState === createjs.Sound.PLAY_FAILED) {
            state.isPlayingObservable(false);
            return;
        }
        state.isPlaying = true;
        playerInstance.addEventListener("complete", stopHandler.bind(null, state));
        playerInstance.addEventListener("interrupted", stopHandler.bind(null, state));
        playerInstance.addEventListener("failed", stopHandler.bind(null, state));
    }

    function registerMedia(id, media) {
        var state = _playerState[id],
            parsed = parseMedia(media),
            i, ext;

        if (state.media) {
            if (state.media.src !== parsed.src) {
                createJs.Sound.removeSound(id); // reset media
            } else {
                for (i = 0; i < parsed.alternateExtensions.length; i++) {
                    ext = parsed.alternateExtensions[i];
                    if (!createJs.Sound.alternateExtensions) {
                        createJs.Sound.alternateExtensions = [];
                    }
                    if (createJs.Sound.alternateExtensions.indexOf(ext) < 0) {
                        createJs.Sound.alternateExtensions.push(ext);
                    }
                }
                return; // no need to re-register
            }
        }

        state.media = parsed;
        createJs.Sound.registerSound(parsed.src, id);
    }

    function parseMedia(media) {
        if (typeof media === "string") {
            return { src: media, alternateExtensions: [] };
        } else {
            var retVal = { src: null, alternateExtensions: [] };
            for (prop in media) {
                if (typeof media[prop] !== "string") { continue; }
                if (!retVal.src) {
                    retVal.src = media[prop];
                } else if (retVal.alternateExtensions.indexOf(prop) < 0) {
                    retVal.alternateExtensions.push(prop);
                }
            }
            return retVal;
        }
    }

    ko.bindingHandlers.soundjs = {        
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var vals = valueAccessor(),
                media = ko.toJS(vals.media || opts.media),
                isPlayingObservable = vals.isPlaying || opts.isPlaying,
                swfPath = vals.swfPath || opts.swfPath,
                id = (++_lastId).toString(),
                state;

            ko.utils.domData.set(element, "ko_soundjs_id", id);
            if (!ko.isWriteableObservable(isPlayingObservable)) {
                isPlayingObservable = ko.observable(false);
            }

            if (!_playerInitialized && _playerCanPlay) {
                _playerInitialized = true;
                if (swfPath) {
                    createjs.FlashPlugin.swfPath = swfPath;
                }
                _playerCanPlay = createjs.Sound.registerPlugins([
                    createjs.WebAudioPlugin,
                    createjs.HTMLAudioPlugin,
                    createjs.FlashPlugin
                ]);
            }
            if (!_playerCanPlay) { return; }

            state = {};
            _playerState[id] = state;
            state.playerReady = false;
            state.isPlayingObservable = isPlayingObservable;

            registerLoadListener();
            registerMedia(id, media);
            
            if (state.isPlayingObservable()) {
                play(id);
            }
        },
        update: function (element, valueAccessor) {
            var vals = valueAccessor(),
                media = ko.toJS(vals.media || opts.media),
                id = ko.utils.domData.get(element, "ko_soundjs_id"),
                isPlayingObservable = vals.isPlaying || opts.isPlaying,
                state, shouldPlay;

            if (id) {
                state = _playerState[id];
                if (state && !state.isPlayingObservable) {
                    state.isPlayingObservable = isPlayingObservable;
                }
            }

            if (!_playerCanPlay) { return; }
            registerMedia(id, media);

            if (state) {
                shouldPlay = ko.utils.unwrapObservable(state.isPlayingObservable);
                if (shouldPlay) {
                    play(id);
                }
                else if (state.isPlaying) {
                    stopAll();
                }
            }
        }
    };
    return ko;
});
