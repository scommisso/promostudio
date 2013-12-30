/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/stockAudio.js" />

"use strict";

define(["models/stockAudio",
        "viewModels/musicViewModel",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger"
],
    function (
        stockAudio,
        musicViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
        function ctor(data, video) {

            var self = this,
                transitionTime = 350, /* from bootstrap-transitions */
                musicItem = null,
                customerTemplateScripts;
            data = data || {};
            video = video || {};

            function loadData(customerTemplateScriptData, stockAudioData, videoData) {
                var id, songs, song, i;

                customerTemplateScripts = customerTemplateScriptData || [];
                loadAudioData(stockAudioData);
                loadVideoData(videoData);

                id = musicItem.fk_CustomerVideoItemId();

                if (id > 0) {
                    songs = self.AvailableMusic();
                    for (i = 0; i < songs.length; i++) {
                        song = songs[i];
                        if (song.pk_StockAudioId() === id) {
                            self.SelectMusic(song);
                            break;
                        }
                    }
                }
            }

            function loadAudioData(stockAudioData) {
                var music = [],
                    i, item;
                for (i = 0; i < stockAudioData.length; i++) {
                    item = stockAudioData[i];
                    item = new stockAudio(item);
                    item = new musicViewModel(self, item);
                    music.push(item);
                }
                self.AvailableMusic(music);
            }

            function loadVideoData(videoData) {
                // Get existing voice acting item applied
                var items = videoData.Items(),
                    musicItems = $.grep(items, function (item) {
                        return item.fk_CustomerVideoItemTypeId() === 2;
                    });
                if (musicItems && musicItems.length > 0) {
                    musicItem = musicItems[0];
                }
            }

            self.AvailableMusic = ko.observableArray([]);
            self.SelectedMusic = ko.observable(null);
            self.IsSelected = function (music) {
                return self.SelectedMusic() === music;
            };
            self.SelectMusic = function (music) {
                var i, songs = self.AvailableMusic();
                for (i = 0; i < songs.length; i++) {
                    if (songs[i] !== music && songs[i].IsSelected()) {
                        songs[i].IsSelected(false);
                    }
                }
                if (self.IsSelected(music)) {
                    music.IsSelected(false);
                    self.SelectedMusic(null);
                    musicItem.fk_CustomerVideoItemId(null);
                } else {
                    music.IsSelected(true);
                    self.SelectedMusic(music);
                    musicItem.fk_CustomerVideoItemId(music.pk_StockAudioId());
                }
            };

            self.IsCompleted = ko.computed(function () {
                var selectedMusic = self.SelectedMusic();
                return !!selectedMusic;
            });

            self.StartOpen = ko.observable(false);

            loadData(data.CustomerTemplateScripts, data.StockAudio, video);
        }

        return ctor;
    });